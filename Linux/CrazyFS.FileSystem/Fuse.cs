using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using CrazyFS.FileSystem.Helpers;
using CrazyFS.Linux;
using Fuse.NET;
using Mono.Unix.Native;

namespace CrazyFS.FileSystem
{
    public class Fuse : IFuse 
    {
        private readonly IFileSystem _fileSystem;
        public Fuse(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
        }

        public Result ChangeTimes(string path, long atime, long mtime)
        {
            var result = GetPathInfo(path, out var info);
            if (result.Status != ResultStatus.Success)
            {
                return result;
            }
            info.LastAccessTime = DateTimeOffset.FromUnixTimeSeconds(atime).DateTime;
            info.LastWriteTime = DateTimeOffset.FromUnixTimeSeconds(mtime).DateTime;
            return new Result(ResultStatus.Success);
        }
        public Result Chown(string path, long uid, long gid)
        {
            return PermissionHelper.Chown(path, uid, gid);
        }

        public Result Chmod(string path, FilePermissions permissions)
        {
            return PermissionHelper.Chmod(path, permissions);
        }
        
        /// <summary>
        /// R_OK = read allowed
        /// W_OK = write allowed
        /// x_OK = exec allowed
        /// F_OK = exists
        /// </summary>
        /// <param name="path"></param>
        /// <param name="access"></param>
        /// <returns></returns>
        public Result CheckAccess(string path, PathAccessModes access)
        {
            if (!_fileSystem.File.Exists(path) && !_fileSystem.Directory.Exists(path))
            {
                return new Result(ResultStatus.PathNotFound);
            }
            return (PermissionHelper.HasAccess(path, access)) ? new Result(ResultStatus.Success) : new Result(ResultStatus.AccessDenied);
        }

        public Result CreateDirectory(string path, FilePermissions mode)
        {
            if (!PermissionHelper.HasAccess(path, PathAccessModes.W_OK))
            {
                return new Result(ResultStatus.AccessDenied);
            }
            _fileSystem.Directory.CreateDirectory(path);
            PermissionHelper.Chmod(path, mode);
            
            return new Result(ResultStatus.Success);
        }

        public Result CreateHardLink(string from, string to)
        {
            return LinuxHelper.CreateHardLink(from, to);
        }
        public Errno CreateSpecialFile(string path, FilePermissions mode, ulong rdev)
        {
            var result = Syscall.mknod(path, mode, rdev);
            if (result == -1)
            {
                return Stdlib.GetLastError ();
            }
            return 0;
        }
        public Result CreateSymlink(string from, string to)
        {
            return LinuxHelper.CreateSymlink(from, to);
        }
        public Errno GetFileSystemStatus(string path, out Statvfs stbuf)
        {
            int r = Syscall.statvfs (path, out stbuf);
            if (r == -1)
            {
                return Stdlib.GetLastError();
            }
            return 0;
        }

        public Errno GetPathExtendedAttribute(string path, string name, byte[] value, out int bytesWritten)
        {
            int r = bytesWritten = (int) Syscall.lgetxattr (path, name, value, (ulong) (value?.Length ?? 0));
            if (r == -1)
                return Stdlib.GetLastError ();
            return 0;
        }
        public Result GetPathInfo(string path, out IFileSystemInfo info)
        {
            info = null;
            if (_fileSystem.File.Exists(path))
            {
                info = _fileSystem.FileInfo.FromFileName(path);
                //info = new LinuxFileInfo(_fileSystem, new System.IO.FileInfo(path), true);
            } else if (_fileSystem.Directory.Exists((path)))
            {
                info = _fileSystem.DirectoryInfo.FromDirectoryName(path);
                //info = new LinuxDirectoryInfo(_fileSystem, new System.IO.DirectoryInfo(path), true);
            }
            if (info != null)
            {
                if (info.IsSymlink())
                {
                    return GetPathInfo(info.GetRealPath(), out info);
                }
            }
            return new Result(info ==  null ? ResultStatus.PathNotFound : ResultStatus.Success);
        }

        public Result GetSymbolicLinkTarget(string path, out string target)
        {
            target = path;
            IFileSystemInfo info = null;
            if (_fileSystem.File.Exists(path))
            {
                info = _fileSystem.FileInfo.FromFileName(path);
            } else if (_fileSystem.Directory.Exists((path)))
            {
                info = _fileSystem.DirectoryInfo.FromDirectoryName(path);
            }

            if (info != null && info.IsSymlink())
            {
                target = info.GetRealPath();
            }
            return new Result(ResultStatus.Success);
        }
        
        public Errno ListPathExtendedAttributes(string path, out string[] names)
        {
            int r = (int) Syscall.llistxattr (path, out names);
            if (r == -1)
                return Stdlib.GetLastError ();
            return 0; 
        }

        public Result Ls(string path, out IEnumerable<IFileSystemInfo> paths)
        {
            //var result = _fileSystem.Directory.;
            var dir = _fileSystem.DirectoryInfo.FromDirectoryName(path);
            paths = dir.EnumerateFileSystemInfos();
            return new Result(ResultStatus.Success);
        }
        
        public Errno Lock(string file, OpenedPathInfo info, FcntlCommand cmd, ref Flock @lock)
        {
            Flock _lock = @lock;
            Errno e = ProcessFile (file, info.OpenFlags, fd => Syscall.fcntl (fd, cmd, ref _lock));
            @lock = _lock;
            return e;
        }

        public void Mount()
        {
        }

        public Result Read(string path, long offset, ulong size, out byte[] buffer, out int bytesRead)
        {
            buffer = new byte[size];
            using (var s = _fileSystem.FileInfo.FromFileName(path).OpenRead())
            {
                //make sure the offset isn't higher than the filesize
                if (offset > s.Length)
                {
                    bytesRead = 0;
                    return new Result(ResultStatus.Success);
                }
                //Set the position to start reading
                s.Position = offset;
                //read until this byte
                var end = (long) size + offset;
                //make sure we don't read passed the filesize
                if (end <= s.Length)
                {
                    bytesRead = s.Read(buffer, 0, (int)size);
                }
                else
                {
                    bytesRead = s.Read(buffer, 0, (int)(s.Length - offset));
                }
            }
            return new Result(ResultStatus.Success);
        }
        public Result RemoveFile(string path)
        {
            _fileSystem.File.Delete(path);
            return new Result(ResultStatus.Success);
        }

        public Result RemoveDirectory(string path)
        {
            _fileSystem.Directory.Delete(path);
            return new Result(ResultStatus.Success);
        }

        public Errno Open(string path, OpenedPathInfo info)
        {
            return ProcessFile (path, info.OpenFlags, delegate (int fd) {return 0;});
        }

        public Result Move(string from, string to)
        {
            if (_fileSystem.File.Exists(to) || _fileSystem.Directory.Exists(to))
            {
                return new Result(ResultStatus.AlreadyExists);
            }

            if (_fileSystem.File.Exists(to))
            {
                _fileSystem.File.Move(from, to);
            }
            else
            {
                _fileSystem.Directory.Move(from, to);
            }

            return new Result(ResultStatus.Success);
        }
        
        public Errno RemovePathExtendedAttribute(string path, string name)
        {
            int r = Syscall.lremovexattr (path, name);
            if (r == -1)
                return Stdlib.GetLastError ();
            return 0;
        }
        
        public Errno SetPathExtendedAttribute (string path, string name, byte[] value, XattrFlags flags)
        {
            int r = Syscall.lsetxattr (path, name, value, (ulong) value.Length, flags);
            if (r == -1) {
                return Stdlib.GetLastError();
            }
            return 0;
        }

        public Result Truncate(string path, long size)
        {
            try
            {
                var f = _fileSystem.FileInfo.FromFileName(path);
                using (var s = f.Open(FileMode.Open))
                {
                    s.SetLength(size);
                }
                return new Result(ResultStatus.Success);
            }
            catch (FileNotFoundException)
            {
                return new Result(ResultStatus.FileNotFound);
            }
            catch (Exception)
            {
                return new Result(ResultStatus.Error);
            }
        }

        public void UnMount()
        {
        }

        public Result Write(string path, byte[] buffer, out int bytesWritten, long offset) {
            using (var s = _fileSystem.FileStream.Create(path, System.IO.FileMode.Open, System.IO.FileAccess.Write)) {
                s.Position = offset;
                s.Write(buffer, 0, buffer.Length);
                bytesWritten = buffer.Length;
            }
            return new Result(ResultStatus.Success);
        }
        
        /**
         * Private - to be replaced
         */
        private delegate int FdCb (int fd);
        private static Errno ProcessFile (string path, OpenFlags flags, FdCb cb)
        {
            int fd = Syscall.open (path, flags);
            if (fd == -1)
            {
                return Stdlib.GetLastError();
            }

            int r = cb (fd);
            Errno res = 0;
            if (r == -1)
            {
                res = Stdlib.GetLastError();
            }
            Syscall.close (fd);
            return res;
        }
    }
}