using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using CrazyFS.Linux;
using CrazyFS.Log;
using Fuse.NET;
using Mono.Unix.Native;
using Serilog;

namespace CrazyFS.FileSystem
{
    public class Fuse : IFuse 
    {
        private readonly IFileSystem _fileSystem;
        private readonly ILogger _logger;
        
        public Fuse(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
            _logger = new LogProvider().Get();
        }

        public Result ChangeTimes(string path, long atime, long mtime)
        {
            _logger.Information(new CrazyFSRequest(CrazyFSRequestName.ChangeTimes, new []
            {
                new KeyValuePair<string, string>("path", path),
                new KeyValuePair<string, string>("atime", atime.ToString()),
                new KeyValuePair<string, string>("mtime", mtime.ToString())
            }));
            
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
            _logger.Information(new CrazyFSRequest(CrazyFSRequestName.Chown, new []
            {
                new KeyValuePair<string, string>("path", path),
                new KeyValuePair<string, string>("uid", uid.ToString()),
                new KeyValuePair<string, string>("gid", gid.ToString())
            }));
            
            try
            {
                _fileSystem.Path.Chown(path, (uint)uid, (uint)gid);
                return new Result(ResultStatus.Success);
            }
            catch (Exception ex)
            {
                return ex.GetResult();
            }
        }
        public Result Chmod(string path, FilePermissions permissions)
        {
            _logger.Information(new CrazyFSRequest(CrazyFSRequestName.Chmod, new []
            {
                new KeyValuePair<string, string>("path", path),
                new KeyValuePair<string, string>("permissions", permissions.ToString())
            }));
            
            try
            {
                _fileSystem.Path.Chmod(path, permissions);
                return new Result(ResultStatus.Success);
            }
            catch (Exception ex)
            {
                return ex.GetResult();
            }
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
            _logger.Information(new CrazyFSRequest(CrazyFSRequestName.CheckAccess, new []
            {
                new KeyValuePair<string, string>("path", path),
                new KeyValuePair<string, string>("access", access.ToString())
            }));
            
            try
            {
                return _fileSystem.Path.HasAccess(path, access) ? new Result(ResultStatus.Success) : new Result(ResultStatus.AccessDenied);
            }
            catch (Exception ex)
            {
                return ex.GetResult();
            }
        }

        public Result CreateDirectory(string path, FilePermissions mode)
        {
            _logger.Information(new CrazyFSRequest(CrazyFSRequestName.CreateDirectory, new []
            {
                new KeyValuePair<string, string>("path", path),
                new KeyValuePair<string, string>("mode", mode.ToString())
            }));
            
            try
            {
                _fileSystem.Path.Chmod(path, mode);
                return new Result(ResultStatus.Success);
            }
            catch (Exception ex)
            {
                return ex.GetResult();
            }
        }

        public Result CreateHardLink(string from, string to)
        {
            _logger.Information(new CrazyFSRequest(CrazyFSRequestName.CreateHardLink, new []
            {
                new KeyValuePair<string, string>("from", from),
                new KeyValuePair<string, string>("to", to)
            }));
            
            try
            {
                _fileSystem.Path.CreateHardLink(from, to);
                return new Result(ResultStatus.Success);
            }
            catch (Exception ex)
            {
                return ex.GetResult();
            }
        }
        public Errno CreateSpecialFile(string path, FilePermissions mode, ulong rdev)
        {
            _logger.Information(new CrazyFSRequest(CrazyFSRequestName.CreateSpecialFile, new []
            {
                new KeyValuePair<string, string>("path", path),
                new KeyValuePair<string, string>("rdev", rdev.ToString())
            }));
            
            var result = Syscall.mknod(path, mode, rdev);
            if (result == -1)
            {
                return Stdlib.GetLastError ();
            }
            return 0;
        }
        public Result CreateSymlink(string from, string to)
        {
            _logger.Information(new CrazyFSRequest(CrazyFSRequestName.CreateSymlink, new []
            {
                new KeyValuePair<string, string>("from", from),
                new KeyValuePair<string, string>("to", to)
            }));
            
            try
            {
                _fileSystem.Path.CreateSymlink(from, to);
                return new Result(ResultStatus.Success);
            }
            catch (Exception ex)
            {
                return ex.GetResult();
            }
        }
        public Errno GetFileSystemStatus(string path, out Statvfs stbuf)
        {
            _logger.Information(new CrazyFSRequest(CrazyFSRequestName.GetFileSystemStatus, new []
            {
                new KeyValuePair<string, string>("path", path)
            }));
            
            int r = Syscall.statvfs (path, out stbuf);
            if (r == -1)
            {
                return Stdlib.GetLastError();
            }
            return 0;
        }

        public Errno GetPathExtendedAttribute(string path, string name, byte[] value, out int bytesWritten)
        {
            _logger.Information(new CrazyFSRequest(CrazyFSRequestName.GetPathExtendedAttribute, new []
            {
                new KeyValuePair<string, string>("path", path),
                new KeyValuePair<string, string>("name", name)
            }));
            
            int r = bytesWritten = (int) Syscall.lgetxattr (path, name, value, (ulong) (value?.Length ?? 0));
            if (r == -1)
                return Stdlib.GetLastError ();
            return 0;
        }
        public Result GetPathInfo(string path, out IFileSystemInfo info)
        {
            _logger.Information(new CrazyFSRequest(CrazyFSRequestName.GetPathInfo, new []
            {
                new KeyValuePair<string, string>("path", path)
            }));
            
            info = null;
            if (_fileSystem.File.Exists(path))
            {
                info = _fileSystem.FileInfo.FromFileName(path);
            } else if (_fileSystem.Directory.Exists((path)))
            {
                info = _fileSystem.DirectoryInfo.FromDirectoryName(path);
            }
            return new Result(info ==  null ? ResultStatus.PathNotFound : ResultStatus.Success);
        }

        public Result GetSymbolicLinkTarget(string path, out string target)
        {
            
            _logger.Information(new CrazyFSRequest(CrazyFSRequestName.GetSymbolicLinkTarget, new []
            {
                new KeyValuePair<string, string>("path", path)
            }));
            
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
            _logger.Information(new CrazyFSRequest(CrazyFSRequestName.ListPathExtendedAttributes, new []
            {
                new KeyValuePair<string, string>("path", path)
            }));
            
            int r = (int) Syscall.llistxattr (path, out names);
            if (r == -1)
                return Stdlib.GetLastError ();
            return 0; 
        }

        public Result Ls(string path, out IEnumerable<IFileSystemInfo> paths)
        {
            _logger.Information(new CrazyFSRequest(CrazyFSRequestName.Ls, new []
            {
                new KeyValuePair<string, string>("path", path)
            }));
            
            var dir = _fileSystem.DirectoryInfo.FromDirectoryName(path);
            paths = dir.EnumerateFileSystemInfos();
            return new Result(ResultStatus.Success);
        }
        
        public Errno Lock(string path, OpenedPathInfo info, FcntlCommand cmd, ref Flock @lock)
        {           
            _logger.Information(new CrazyFSRequest(CrazyFSRequestName.Lock, new []
            {
                new KeyValuePair<string, string>("path", path)
            }));
            
            Flock _lock = @lock;
            Errno e = ProcessFile (path, info.OpenFlags, fd => Syscall.fcntl (fd, cmd, ref _lock));
            @lock = _lock;
            return e;
        }

        public void Mount()
        {
            _logger.Information(new CrazyFSRequest(CrazyFSRequestName.Mount, Array.Empty<KeyValuePair<string, string>>()));
        }

        public Result Read(string path, long offset, ulong size, out byte[] buffer, out int bytesRead)
        {
            _logger.Information(new CrazyFSRequest(CrazyFSRequestName.Read, new []
            {
                new KeyValuePair<string, string>("path", path),
                new KeyValuePair<string, string>("offset", offset.ToString()),
                new KeyValuePair<string, string>("size", size.ToString())
            }));
            
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
            _logger.Information(new CrazyFSRequest(CrazyFSRequestName.RemoveFile, new []
            {
                new KeyValuePair<string, string>("path", path)
            }));
            
            _fileSystem.File.Delete(path);
            return new Result(ResultStatus.Success);
        }

        public Result RemoveDirectory(string path)
        {
            _logger.Information(new CrazyFSRequest(CrazyFSRequestName.RemoveDirectory, new []
            {
                new KeyValuePair<string, string>("path", path)
            }));
            _fileSystem.Directory.Delete(path);
            return new Result(ResultStatus.Success);
        }

        public Errno Open(string path, OpenedPathInfo info)
        {
            _logger.Information(new CrazyFSRequest(CrazyFSRequestName.Open, new []
            {
                new KeyValuePair<string, string>("path", path)
            }));
            return ProcessFile (path, info.OpenFlags, delegate (int fd) {return 0;});
        }

        public Result Move(string from, string to)
        {
            _logger.Information(new CrazyFSRequest(CrazyFSRequestName.Move, new []
            {
                new KeyValuePair<string, string>("from", from),
                new KeyValuePair<string, string>("to", to)
            }));
            
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
            _logger.Information(new CrazyFSRequest(CrazyFSRequestName.RemovePathExtendedAttribute, new []
            {
                new KeyValuePair<string, string>("path", path),
                new KeyValuePair<string, string>("name", name)
            }));
            
            int r = Syscall.lremovexattr (path, name);
            if (r == -1)
                return Stdlib.GetLastError ();
            return 0;
        }
        
        public Errno SetPathExtendedAttribute (string path, string name, byte[] value, XattrFlags flags)
        {
            _logger.Information(new CrazyFSRequest(CrazyFSRequestName.SetPathExtendedAttribute, new []
            {
                new KeyValuePair<string, string>("path", path),
                new KeyValuePair<string, string>("name", name),
                new KeyValuePair<string, string>("flags", flags.ToString())
            }));
            
            int r = Syscall.lsetxattr (path, name, value, (ulong) value.Length, flags);
            if (r == -1) {
                return Stdlib.GetLastError();
            }
            return 0;
        }

        public Result Truncate(string path, long size)
        {
            _logger.Information(new CrazyFSRequest(CrazyFSRequestName.Truncate, new []
            {
                new KeyValuePair<string, string>("path", path),
                new KeyValuePair<string, string>("size", size.ToString())
            }));
            
            try
            {
                var f = _fileSystem.FileInfo.FromFileName(path);
                using (var s = f.Open(FileMode.Open))
                {
                    s.SetLength(size);
                }
                return new Result(ResultStatus.Success);
            }
            catch (Exception ex)
            {
                return ex.GetResult();
            }
        }

        public void UnMount()
        {
            _logger.Information(new CrazyFSRequest(CrazyFSRequestName.UnMount, Array.Empty<KeyValuePair<string, string>>()));
        }

        public Result Write(string path, byte[] buffer, out int bytesWritten, long offset) {
            _logger.Information(new CrazyFSRequest(CrazyFSRequestName.Write, new []
            {
                new KeyValuePair<string, string>("path", path),
                new KeyValuePair<string, string>("offset", offset.ToString())
            }));
            
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