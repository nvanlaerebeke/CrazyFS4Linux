using System;
using System.IO.Abstractions;
using CrazyFS.FileSystem.Helpers;
using CrazyFS.Linux;
using Mono.Unix.Native;

namespace CrazyFS.FileSystem
{
    public class Fuse<T> : IFuse 
        where T: IFileSystem, new()
    {
        private readonly IFileSystem _fileSystem;
        public Fuse()
        {
            
            _fileSystem = new T();
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

        public Result CreateSymlink(string from, string to)
        {
            return LinuxHelper.CreateSymlink(from, to);
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
        public Result Write(string path, byte[] buffer, out int bytesWritten, long offset) {
            using (var s = _fileSystem.FileStream.Create(path, System.IO.FileMode.Open, System.IO.FileAccess.Write)) {
                s.Position = offset;
                s.Write(buffer, 0, buffer.Length);
                bytesWritten = buffer.Length;
            }
            return new Result(ResultStatus.Success);
        }

        public Result GetPathInfo(string path, out IFileSystemInfo info)
        {
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
    }
}