using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
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
#if DEBUG
            var request = new CrazyFSRequest(CrazyFSRequestName.ChangeTimes, new[]
            {
                new KeyValuePair<string, string>("path", path),
                new KeyValuePair<string, string>("atime", atime.ToString()),
                new KeyValuePair<string, string>("mtime", mtime.ToString())
            }).Log();
#endif           
            try
            {
                var result = GetPathInfo(path, out var info);
                if (result.Status != ResultStatus.Success)
                {
#if DEBUG
                    request.Log(result);
#endif
                    return result;  
                }

                info.LastAccessTime = DateTimeOffset.FromUnixTimeSeconds(atime).DateTime;
                info.LastWriteTime = DateTimeOffset.FromUnixTimeSeconds(mtime).DateTime;
                
                result = new Result(ResultStatus.Success);
#if DEBUG
                request.Log(result);
#endif
                return result;
            }
            catch (Exception ex)
            {
                var result = ex.GetResult();
#if DEBUG
                request.Log(result);                
#endif                
                return result;
            }
        }
        public Result Chown(string path, long uid, long gid)
        {
#if DEBUG            
            var request = new CrazyFSRequest(CrazyFSRequestName.Chown, new[]
            {
                new KeyValuePair<string, string>("path", path),
                new KeyValuePair<string, string>("uid", uid.ToString()),
                new KeyValuePair<string, string>("gid", gid.ToString())
            }).Log();
#endif
            
            try
            {
                _fileSystem.Path.Chown(path, (uint)uid, (uint)gid);
                var result = new Result(ResultStatus.Success);
#if DEBUG
                request.Log(result);                
#endif
                return result;
            }
            catch (Exception ex)
            {
                var result = ex.GetResult();
#if DEBUG
                request.Log(result);                
#endif                
                return result;
            }
        }
        public Result Chmod(string path, FilePermissions permissions)
        {
#if DEBUG            
            var request = new CrazyFSRequest(CrazyFSRequestName.Chmod, new []
            {
                new KeyValuePair<string, string>("path", path),
                new KeyValuePair<string, string>("permissions", permissions.ToString())
            }).Log();
#endif
           
            try
            {
                _fileSystem.Path.Chmod(path, permissions);
                var result = new Result(ResultStatus.Success);
#if DEBUG
                request.Log(result);                
#endif                
                return result;
            }
            catch (Exception ex)
            {
                var result = ex.GetResult(); 
#if DEBUG
                request.Log(result);                
#endif
                return result;
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
#if DEBUG
            var request = new CrazyFSRequest(CrazyFSRequestName.CheckAccess, new[]
            {
                new KeyValuePair<string, string>("path", path),
                new KeyValuePair<string, string>("access", access.ToString())
            }).Log();
 #endif           
            try
            {
                var result = _fileSystem.Path.HasAccess(path, access) ? new Result(ResultStatus.Success) : new Result(ResultStatus.AccessDenied);
#if DEBUG
                request.Log(result);                
#endif                
                return result;
            }
            catch (Exception ex)
            {
                var result = ex.GetResult();
#if DEBUG
                request.Log(result);                
#endif
                return result;
            }
        }

        public Result CreateDirectory(string path, FilePermissions mode)
        {
#if DEBUG
            var request = new CrazyFSRequest(CrazyFSRequestName.CreateDirectory, new[]
            {
                new KeyValuePair<string, string>("path", path),
                new KeyValuePair<string, string>("mode", mode.ToString())
            }).Log();
#endif            
            try
            {
                _fileSystem.Directory.CreateDirectory(path, mode);
                var result = new Result(ResultStatus.Success);
#if DEBUG
                request.Log(result);                
#endif
                return result;
            }
            catch (Exception ex)
            {
                var result = ex.GetResult();
#if DEBUG
                request.Log(result);                
#endif                
                return result;
            }
        }

        public Result CreateHardLink(string from, string to)
        {
#if DEBUG
            var request = new CrazyFSRequest(CrazyFSRequestName.CreateHardLink, new[]
            {
                new KeyValuePair<string, string>("from", from),
                new KeyValuePair<string, string>("to", to)
            }).Log();
#endif
            
            try
            {
                _fileSystem.Path.CreateHardLink(from, to);
                var result = new Result(ResultStatus.Success);
#if DEBUG
                request.Log(result);                
#endif                
                return result;
            }
            catch (Exception ex)
            {
                var result = ex.GetResult();
#if DEBUG
                request.Log(result);                
#endif
                return result;
            }
        }
        public Result CreateSpecialFile(string path, FilePermissions mode, ulong rdev)
        {
#if DEBUG
            var request = new CrazyFSRequest(CrazyFSRequestName.CreateSpecialFile, new[]
            {
                new KeyValuePair<string, string>("path", path),
                new KeyValuePair<string, string>("rdev", rdev.ToString())
            }).Log();
#endif
            try
            {
                _fileSystem.File.CreateSpecialFile(path, mode, rdev);
                var result = new Result(ResultStatus.Success);
#if DEBUG
                request.Log(result);                
#endif                
                return result;
            }
            catch (Exception ex)
            {
                var result = ex.GetResult();
#if DEBUG
                request.Log(result);                
#endif                
                return result;
            }
        }
        public Result CreateSymlink(string from, string to)
        {
#if DEBUG
            var request = new CrazyFSRequest(CrazyFSRequestName.CreateSymlink, new[]
            {
                new KeyValuePair<string, string>("from", from),
                new KeyValuePair<string, string>("to", to)
            }).Log();
#endif
            try
            {
                _fileSystem.Path.CreateSymlink(from, to);
                var result = new Result(ResultStatus.Success);
#if DEBUG
                request.Log(result);                
#endif
                return result;
            }
            catch (Exception ex)
            {
                var result = ex.GetResult();
#if DEBUG
                request.Log(result);                
#endif                
                return result;
            }
        }
        public Errno GetFileSystemStatus(string path, out Statvfs stbuf)
        {
#if DEBUG
            new CrazyFSRequest(CrazyFSRequestName.GetFileSystemStatus, new[]
            {
                new KeyValuePair<string, string>("path", path)
            }).Log();
#endif            
     
            int r = Syscall.statvfs (path, out stbuf);
            if (r == -1)
            {
                return Stdlib.GetLastError();
            }
            return 0;
        }

        public Result GetPathExtendedAttribute(string path, string name, byte[] value, out int bytesWritten)
        {
#if DEBUG
            var request = new CrazyFSRequest(CrazyFSRequestName.GetPathExtendedAttribute, new[]
            {
                new KeyValuePair<string, string>("path", path),
                new KeyValuePair<string, string>("name", name)
            }).Log();
#endif
            
            bytesWritten = 0;
            try
            {
                _fileSystem.Path.GetExtendedAttribute(path, name, value, out bytesWritten);
                var result = (bytesWritten == -1) ? new Result(ResultStatus.NotSet) : new Result(ResultStatus.Success);
#if DEBUG
                request.Log(result);                
#endif
                return result;
            }
            catch (Exception ex)
            {
                var result = ex.GetResult();
#if DEBUG
                request.Log(result);                
#endif                
                return result;
            }
        }
        public Result GetPathInfo(string path, out IFileSystemInfo info)
        {
#if DEBUG
            var request = new CrazyFSRequest(CrazyFSRequestName.GetPathInfo, new[]
            {
                new KeyValuePair<string, string>("path", path)
            }).Log();
#endif
 
            info = null;
            try
            {
                if (_fileSystem.File.Exists(path))
                {
                    info = _fileSystem.FileInfo.FromFileName(path);
                }
                else if (_fileSystem.Directory.Exists((path)))
                {
                    info = _fileSystem.DirectoryInfo.FromDirectoryName(path);
                }

                var result = new Result(info == null ? ResultStatus.PathNotFound : ResultStatus.Success);
#if DEBUG
                request.Log(result);                
#endif                
                return result;
            }
            catch (Exception ex)
            {
                var result = ex.GetResult();
#if DEBUG
                request.Log(result);                
#endif                
                return result;
            }
        }

        public Result GetSymbolicLinkTarget(string path, out string target)
        {
#if DEBUG
            var request = new CrazyFSRequest(CrazyFSRequestName.GetSymbolicLinkTarget, new[]
            {
                new KeyValuePair<string, string>("path", path)
            }).Log();
#endif            
            target = path;
            try
            {
                if (_fileSystem.File.Exists(path))
                {
                    target = (_fileSystem.FileInfo.FromFileName(path) as LinuxFileInfo)?.GetRealPath();
                }
                else if (_fileSystem.Directory.Exists((path)))
                {
                    target = (_fileSystem.DirectoryInfo.FromDirectoryName(path) as LinuxDirectoryInfo)?.GetRealPath();
                }
                var result = new Result(ResultStatus.Success);
#if DEBUG
                request.Log(result);                
#endif                
                return result;
            }
            catch (Exception ex)
            {
                var result = ex.GetResult();
#if DEBUG
                request.Log(result);                
#endif
                return result;
            }
        }
        
        public Result ListPathExtendedAttributes(string path, out string[] names)
        {
#if DEBUG
            var request = new CrazyFSRequest(CrazyFSRequestName.ListPathExtendedAttributes, new[]
            {
                new KeyValuePair<string, string>("path", path)
            }).Log();
#endif

            try
            {
                names = _fileSystem.Path.ListExtendedAttributes(path);
                var result = new Result(ResultStatus.Success);
#if DEBUG
                request.Log(result);                
#endif                
                return result;                
            }
            catch (Exception ex)
            {
                names = Array.Empty<string>();
                var result = ex.GetResult();
#if DEBUG
                request.Log(result);                
#endif
                return result;
            }

        }

        public Result Ls(string path, out IEnumerable<IFileSystemInfo> paths)
        {
#if DEBUG
            var request = new CrazyFSRequest(CrazyFSRequestName.Ls, new[]
            {
                new KeyValuePair<string, string>("path", path)
            }).Log();
#endif

            try
            {
                var dir = _fileSystem.DirectoryInfo.FromDirectoryName(path);
                paths = dir.EnumerateFileSystemInfos();
                var result = new Result(ResultStatus.Success);
#if DEBUG
                request.Log(result);                
#endif                
                return result;
            }
            catch (Exception ex)
            {
                paths = new List<IFileSystemInfo>();
                var result = ex.GetResult();
#if DEBUG
                request.Log(result);                
#endif                
                return result;
            }
        }
        
        public Errno Lock(string path, OpenedPathInfo info, FcntlCommand cmd, ref Flock @lock)
        {
#if DEBUG
            new CrazyFSRequest(CrazyFSRequestName.Lock, new[]
            {
                new KeyValuePair<string, string>("path", path)
            }).Log();
#endif
            Flock _lock = @lock;
            Errno e = ProcessFile (path, info.OpenFlags, fd => Syscall.fcntl (fd, cmd, ref _lock));
            @lock = _lock;
            return e;
        }

        public void Mount()
        {
#if DEBUG
            new CrazyFSRequest(CrazyFSRequestName.Mount, Array.Empty<KeyValuePair<string, string>>()).Log();
#endif
        }

        public Result Read(string path, long offset, ulong size, out byte[] buffer, out int bytesRead)
        {
#if DEBUG
            var request = new CrazyFSRequest(CrazyFSRequestName.Read, new[]
            {
                new KeyValuePair<string, string>("path", path),
                new KeyValuePair<string, string>("offset", offset.ToString()),
                new KeyValuePair<string, string>("size", size.ToString())
            }).Log();
#endif
            try
            {
                buffer = new byte[size];
                using (var s = _fileSystem.FileInfo.FromFileName(path).OpenRead())
                {
                    //make sure the offset isn't higher than the filesize
                    if (offset > s.Length)
                    {
                        bytesRead = 0;
                        var result_oob = new Result(ResultStatus.Success);
#if DEBUG
                        request.Log(result_oob);                
#endif                        
                        return result_oob;
                    }

                    //Set the position to start reading
                    s.Position = offset;
                    //read until this byte
                    var end = (long) size + offset;
                    //make sure we don't read passed the filesize
                    if (end <= s.Length)
                    {
                        bytesRead = s.Read(buffer, 0, (int) size);
                    }
                    else
                    {
                        bytesRead = s.Read(buffer, 0, (int) (s.Length - offset));
                    }
                }
                var result = new Result(ResultStatus.Success);
#if DEBUG
                request.Log(result);                
#endif
                return result;
            }
            catch (Exception ex)
            {
                buffer = Array.Empty<byte>();
                bytesRead = 0;
                var result = ex.GetResult();
#if DEBUG
                request.Log(result);                
#endif                
                return result;
            }
        }
        public Result RemoveFile(string path)
        {
#if DEBUG
            var request = new CrazyFSRequest(CrazyFSRequestName.RemoveFile, new[]
            {
                new KeyValuePair<string, string>("path", path)
            }).Log();
#endif
            try
            {
                _fileSystem.File.Delete(path);
                var result = new Result(ResultStatus.Success);
#if DEBUG
                request.Log(result);                
#endif                
                return result;
            }
            catch (Exception ex)
            {
                var result = ex.GetResult();
#if DEBUG
                request.Log(result);                
#endif                
                return result;
            }
        }

        public Result RemoveDirectory(string path)
        {
#if DEBUG
            var request = new CrazyFSRequest(CrazyFSRequestName.RemoveDirectory, new[]
            {
                new KeyValuePair<string, string>("path", path)
            }).Log();
#endif
            try
            {
                _fileSystem.Directory.Delete(path);
                var result = new Result(ResultStatus.Success);
#if DEBUG
                request.Log(result);                
#endif                
                return result;
            }
            catch (Exception ex)
            {
                var result = ex.GetResult();
#if DEBUG
                request.Log(result);                
#endif                
                return result;
            }
        }

        /// <summary>
        /// ToDo: Cache open file handles, currently not implemented
        /// </summary>
        /// <param name="path"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        public Result Open(string path, OpenedPathInfo info)
        {
#if DEBUG
            var request = new CrazyFSRequest(CrazyFSRequestName.Open, new[]
            {
                new KeyValuePair<string, string>("path", path)
            }).Log();
#endif
            //Not implmemented
            var result =  new Result(ResultStatus.Success);
#if DEBUG
            request.Log(result);                
#endif
            return result;
        }

        public Result Move(string from, string to)
        {
#if DEBUG
            var request = new CrazyFSRequest(CrazyFSRequestName.Move, new[]
            {
                new KeyValuePair<string, string>("from", from),
                new KeyValuePair<string, string>("to", to)
            }).Log();
#endif
            try
            {
                if (_fileSystem.File.Exists(to) || _fileSystem.Directory.Exists(to))
                {
                    var result_exists = new Result(ResultStatus.AlreadyExists);
#if DEBUG
                    request.Log(result_exists);                
#endif                    
                    return result_exists;
                }

                if (_fileSystem.File.Exists(to))
                {
                    _fileSystem.File.Move(from, to);
                }
                else
                {
                    _fileSystem.Directory.Move(from, to);
                }
                var result = new Result(ResultStatus.Success);
#if DEBUG
                request.Log(result);                
#endif                
                return result;
            }
            catch (Exception ex)
            {
                var result = ex.GetResult();
#if DEBUG
                request.Log(result);                
#endif
                return result;
            }
        }
        
        public Result RemovePathExtendedAttribute(string path, string name)
        {
#if DEBUG
            var request = new CrazyFSRequest(CrazyFSRequestName.RemovePathExtendedAttribute, new[]
            {
                new KeyValuePair<string, string>("path", path),
                new KeyValuePair<string, string>("name", name)
            }).Log();
#endif
            try
            {
                _fileSystem.Path.RemoveExtendedAttributes(path, name);
                var result = new Result(ResultStatus.Success);
#if DEBUG
                request.Log(result);                
#endif                
                return result;
            }
            catch (Exception ex)
            {
                var result = ex.GetResult();
#if DEBUG
                request.Log(result);                
#endif                
                return result;
            }
        }
        
        public Result SetPathExtendedAttribute (string path, string name, byte[] value, XattrFlags flags)
        {
#if DEBUG
            var request = new CrazyFSRequest(CrazyFSRequestName.SetPathExtendedAttribute, new[]
            {
                new KeyValuePair<string, string>("path", path),
                new KeyValuePair<string, string>("name", name),
                new KeyValuePair<string, string>("flags", flags.ToString())
            }).Log();
#endif
            try
            {
                _fileSystem.Path.SetExtendedAttributes(path, name, value, flags);
                var result = new Result(ResultStatus.Success);
#if DEBUG
                request.Log(result);                
#endif                
                return result;
            }
            catch (Exception ex)
            {
                var result = ex.GetResult();
#if DEBUG
                request.Log(result);                
#endif                
                return result;
            }
        }

        public Result Truncate(string path, long size)
        {
#if DEBUG
            var request = new CrazyFSRequest(CrazyFSRequestName.Truncate, new[]
            {
                new KeyValuePair<string, string>("path", path),
                new KeyValuePair<string, string>("size", size.ToString())
            }).Log();
#endif
            try
            {
                var f = _fileSystem.FileInfo.FromFileName(path);
                using (var s = f.Open(FileMode.Open))
                {
                    s.SetLength(size);
                }
                var result = new Result(ResultStatus.Success);
#if DEBUG
                request.Log(result);                
#endif                
                return result;
            }
            catch (Exception ex)
            {
                var result = ex.GetResult();
#if DEBUG
                request.Log(result);                
#endif                
                return result;
            }
        }

        public void UnMount()
        {
#if DEBUG
            new CrazyFSRequest(CrazyFSRequestName.UnMount, Array.Empty<KeyValuePair<string, string>>()).Log();
#endif
        }

        public Result Write(string path, byte[] buffer, out int bytesWritten, long offset)
        {
#if DEBUG
            var request = new CrazyFSRequest(CrazyFSRequestName.Write, new[]
            {
                new KeyValuePair<string, string>("path", path),
                new KeyValuePair<string, string>("offset", offset.ToString())
            }).Log();
#endif
            try
            {
                using (var s = _fileSystem.FileStream.Create(path, FileMode.Open, FileAccess.Write))
                {
                    s.Position = offset;
                    s.Write(buffer, 0, buffer.Length);
                    bytesWritten = buffer.Length;
                }
                var result = new Result(ResultStatus.Success);
#if DEBUG
                request.Log(result);                
#endif                
                return result;
            }
            catch (Exception ex)
            {
                bytesWritten = 0;
                var result = ex.GetResult();
#if DEBUG
                request.Log(result);                
#endif                
                return result;
            }
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