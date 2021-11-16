using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using CrazyFS.FileSystem;
using Mono.Unix.Native;

namespace CrazyFS.Passthrough.Linux
{
    public class LinuxPassthroughFileSystem : Fuse
    {
        public LinuxPassthroughFileSystem(IFileSystem fileSystem) : base(fileSystem)
        {
        }

        public override Result Chown(string path, long uid, long gid)
        {
#if DEBUG
            var request = new CrazyFsRequest(CrazyFsRequestName.Chown, new[]
            {
                new KeyValuePair<string, string>("path", path),
                new KeyValuePair<string, string>("uid", uid.ToString()),
                new KeyValuePair<string, string>("gid", gid.ToString())
            }).Log();
#endif

            try
            {
                FileSystem.Path.Chown(path, (uint) uid, (uint) gid);
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

        public override Result Chmod(string path, FilePermissions permissions)
        {
#if DEBUG
            var request = new CrazyFsRequest(CrazyFsRequestName.Chmod, new[]
            {
                new KeyValuePair<string, string>("path", path),
                new KeyValuePair<string, string>("permissions", permissions.ToString())
            }).Log();
#endif

            try
            {
                FileSystem.Path.Chmod(path, permissions);
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
        public override Result CheckAccess(string path, PathAccessModes access)
        {
#if DEBUG
            var request = new CrazyFsRequest(CrazyFsRequestName.CheckAccess, new[]
            {
                new KeyValuePair<string, string>("path", path),
                new KeyValuePair<string, string>("access", access.ToString())
            }).Log();
#endif
            try
            {
                var result = FileSystem.Path.HasAccess(path, access)
                    ? new Result(ResultStatus.Success)
                    : new Result(ResultStatus.AccessDenied);
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

        public override Result CreateHardLink(string from, string to)
        {
#if DEBUG
            var request = new CrazyFsRequest(CrazyFsRequestName.CreateHardLink, new[]
            {
                new KeyValuePair<string, string>("from", from),
                new KeyValuePair<string, string>("to", to)
            }).Log();
#endif

            try
            {
                FileSystem.Path.CreateHardLink(from, to);
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

        public override Result CreateSpecialFile(string path, FilePermissions mode, ulong rdev)
        {
#if DEBUG
            var request = new CrazyFsRequest(CrazyFsRequestName.CreateSpecialFile, new[]
            {
                new KeyValuePair<string, string>("path", path),
                new KeyValuePair<string, string>("rdev", rdev.ToString())
            }).Log();
#endif
            try
            {
                FileSystem.File.CreateSpecialFile(path, mode, rdev);
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

        public override Result CreateSymlink(string from, string to)
        {
#if DEBUG
            var request = new CrazyFsRequest(CrazyFsRequestName.CreateSymlink, new[]
            {
                new KeyValuePair<string, string>("from", from),
                new KeyValuePair<string, string>("to", to)
            }).Log();
#endif
            try
            {
                FileSystem.Path.CreateSymlink(from, to);
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

        public override Result GetPathExtendedAttribute(string path, string name, byte[] value, out int bytesWritten)
        {
#if DEBUG
            var request = new CrazyFsRequest(CrazyFsRequestName.GetPathExtendedAttribute, new[]
            {
                new KeyValuePair<string, string>("path", path),
                new KeyValuePair<string, string>("name", name)
            }).Log();
#endif

            bytesWritten = 0;
            try
            {
                FileSystem.Path.GetExtendedAttribute(path, name, value, out bytesWritten);
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

        public override Result GetSymbolicLinkTarget(string path, out string target)
        {
#if DEBUG
            var request = new CrazyFsRequest(CrazyFsRequestName.GetSymbolicLinkTarget, new[]
            {
                new KeyValuePair<string, string>("path", path)
            }).Log();
#endif
            target = path;
            try
            {
                if (FileSystem.File.Exists(path))
                {
                    target = (FileSystem.FileInfo.FromFileName(path) as LinuxFileInfo)?.GetRealPath();
                }
                else if (FileSystem.Directory.Exists(path))
                {
                    target = (FileSystem.DirectoryInfo.FromDirectoryName(path) as LinuxDirectoryInfo)?.GetRealPath();
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

        public override Result ListPathExtendedAttributes(string path, out string[] names)
        {
#if DEBUG
            var request = new CrazyFsRequest(CrazyFsRequestName.ListPathExtendedAttributes, new[]
            {
                new KeyValuePair<string, string>("path", path)
            }).Log();
#endif

            try
            {
                names = FileSystem.Path.ListExtendedAttributes(path);
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
        
        public override Result RemovePathExtendedAttribute(string path, string name)
        {
#if DEBUG
            var request = new CrazyFsRequest(CrazyFsRequestName.RemovePathExtendedAttribute, new[]
            {
                new KeyValuePair<string, string>("path", path),
                new KeyValuePair<string, string>("name", name)
            }).Log();
#endif
            try
            {
                FileSystem.Path.RemoveExtendedAttributes(path, name);
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
        
        public override Result SetPathExtendedAttribute (string path, string name, byte[] value, XattrFlags flags)
        {
#if DEBUG
            var request = new CrazyFsRequest(CrazyFsRequestName.SetPathExtendedAttribute, new[]
            {
                new KeyValuePair<string, string>("path", path),
                new KeyValuePair<string, string>("name", name),
                new KeyValuePair<string, string>("flags", flags.ToString())
            }).Log();
#endif
            try
            {
                FileSystem.Path.SetExtendedAttributes(path, name, value, flags);
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
        
        public override Result CreateDirectory(string path, FilePermissions mode)
        {
#if DEBUG
            var request = new CrazyFsRequest(CrazyFsRequestName.CreateDirectory, new[]
            {
                new KeyValuePair<string, string>("path", path),
                new KeyValuePair<string, string>("mode", mode.ToString())
            }).Log();
#endif            
            try
            {
                FileSystem.Directory.CreateDirectory(path, mode);
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
    }
}