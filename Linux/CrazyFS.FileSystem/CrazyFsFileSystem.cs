using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;

namespace CrazyFS.FileSystem
{
    public abstract class CrazyFsFileSystem : ICrazyFSFileSystem 
    {
        protected readonly IFileSystem FileSystem;

        protected CrazyFsFileSystem(IFileSystem fileSystem)
        {
            FileSystem = fileSystem;
        }

        public Result ChangeTimes(string path, long atime, long mtime)
        {
#if DEBUG
            var request = new CrazyFsRequest(CrazyFsRequestName.ChangeTimes, new[]
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

        public abstract Result CreateHardLink(string from, string to);
        public abstract Result CreateSymlink(string from, string to);

        public abstract Result GetPathExtendedAttribute(string path, string name, byte[] value, out int bytesWritten);
        
        public Result GetPathInfo(string path, out IFileSystemInfo info)
        {
#if DEBUG
            var request = new CrazyFsRequest(CrazyFsRequestName.GetPathInfo, new[]
            {
                new KeyValuePair<string, string>("path", path)
            }).Log();
#endif
 
            info = null;
            try
            {
                if (FileSystem.File.Exists(path))
                {
                    info = FileSystem.FileInfo.FromFileName(path);
                }
                else if (FileSystem.Directory.Exists(path))
                {
                    info = FileSystem.DirectoryInfo.FromDirectoryName(path);
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
        public abstract Result GetSymbolicLinkTarget(string path, out string target);
        public abstract Result ListPathExtendedAttributes(string path, out string[] names);
        
        public Result Ls(string path, out IEnumerable<IFileSystemInfo> paths)
        {
#if DEBUG
            var request = new CrazyFsRequest(CrazyFsRequestName.Ls, new[]
            {
                new KeyValuePair<string, string>("path", path)
            }).Log();
#endif

            try
            {
                var dir = FileSystem.DirectoryInfo.FromDirectoryName(path);
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
        public void Mount()
        {
#if DEBUG
            new CrazyFsRequest(CrazyFsRequestName.Mount, Array.Empty<KeyValuePair<string, string>>()).Log();
#endif
        }

        public Result Read(string path, long offset, ulong size, out byte[] buffer, out int bytesRead)
        {
#if DEBUG
            var request = new CrazyFsRequest(CrazyFsRequestName.Read, new[]
            {
                new KeyValuePair<string, string>("path", path),
                new KeyValuePair<string, string>("offset", offset.ToString()),
                new KeyValuePair<string, string>("size", size.ToString())
            }).Log();
#endif
            try
            {
                buffer = new byte[size];
                using (var s = FileSystem.FileInfo.FromFileName(path).OpenRead())
                {
                    //make sure the offset isn't higher than the filesize
                    if (offset > s.Length)
                    {
                        bytesRead = 0;
                        var resultOverflow = new Result(ResultStatus.Success);
#if DEBUG
                        request.Log(resultOverflow);                
#endif                        
                        return resultOverflow;
                    }

                    //Set the position to start reading
                    s.Position = offset;
                    //read until this byte
                    var end = (long) size + offset;
                    //make sure we don't read passed the filesize
                    bytesRead = end <= s.Length ? s.Read(buffer, 0, (int) size) : s.Read(buffer, 0, (int) (s.Length - offset));
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
            var request = new CrazyFsRequest(CrazyFsRequestName.RemoveFile, new[]
            {
                new KeyValuePair<string, string>("path", path)
            }).Log();
#endif
            try
            {
                FileSystem.File.Delete(path);
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
            var request = new CrazyFsRequest(CrazyFsRequestName.RemoveDirectory, new[]
            {
                new KeyValuePair<string, string>("path", path)
            }).Log();
#endif
            try
            {
                FileSystem.Directory.Delete(path);
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

        public abstract Result RemovePathExtendedAttribute(string path, string name);

        /// <summary>
        /// ToDo: Cache open file handles, currently not implemented
        /// </summary>
        /// <param name="path"></param>
        /// <param name="openFlags"></param>
        /// <returns></returns>
        public Result Open(string path, OpenFlags openFlags)
        {
#if DEBUG
            var request = new CrazyFsRequest(CrazyFsRequestName.Open, new[]
            {
                new KeyValuePair<string, string>("path", path)
            }).Log();
#endif
            //Not implemented
            var result =  new Result(ResultStatus.Success);
#if DEBUG
            request.Log(result);                
#endif
            return result;
        }

        public Result Move(string from, string to)
        {
#if DEBUG
            var request = new CrazyFsRequest(CrazyFsRequestName.Move, new[]
            {
                new KeyValuePair<string, string>("from", from),
                new KeyValuePair<string, string>("to", to)
            }).Log();
#endif
            try
            {
                if (FileSystem.File.Exists(to) || FileSystem.Directory.Exists(to))
                {
                    var resultExists = new Result(ResultStatus.AlreadyExists);
#if DEBUG
                    request.Log(resultExists);                
#endif                    
                    return resultExists;
                }

                if (FileSystem.File.Exists(to))
                {
                    FileSystem.File.Move(from, to);
                }
                else
                {
                    FileSystem.Directory.Move(from, to);
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

        public Result Truncate(string path, long size)
        {
#if DEBUG
            var request = new CrazyFsRequest(CrazyFsRequestName.Truncate, new[]
            {
                new KeyValuePair<string, string>("path", path),
                new KeyValuePair<string, string>("size", size.ToString())
            }).Log();
#endif
            try
            {
                var f = FileSystem.FileInfo.FromFileName(path);
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
            new CrazyFsRequest(CrazyFsRequestName.UnMount, Array.Empty<KeyValuePair<string, string>>()).Log();
#endif
        }

        public Result Write(string path, byte[] buffer, out int bytesWritten, long offset)
        {
#if DEBUG
            var request = new CrazyFsRequest(CrazyFsRequestName.Write, new[]
            {
                new KeyValuePair<string, string>("path", path),
                new KeyValuePair<string, string>("offset", offset.ToString())
            }).Log();
#endif
            try
            {
                using (var s = FileSystem.FileStream.Create(path, FileMode.Open, FileAccess.Write))
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
    }
}