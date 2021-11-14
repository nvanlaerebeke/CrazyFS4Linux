using System;
using System.IO;
using System.IO.Abstractions;
using CrazyFS.Linux;
using Mono.Unix;
using Mono.Unix.Native;

namespace CrazyFS.FileSystem
{
    public static class PathExtensions
    {
       
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pathWrapper"></param>
        /// <param name="path"></param>
        /// <param name="modes"></param>
        /// <returns></returns>
        /// <exception cref="FileNotFoundException"></exception>
        public static bool HasAccess(this IPath pathWrapper, string path, PathAccessModes modes)
        {
            var fs = pathWrapper.FileSystem;
            if (fs.File.Exists(path))
            {
                return CheckPathAccessModes(new UnixFileInfo(fs.FileInfo.FromFileName(path).FullName).FileAccessPermissions, modes);
            }
            else if(fs.Directory.Exists(path))
            {
                return CheckPathAccessModes(new UnixDirectoryInfo(fs.DirectoryInfo.FromDirectoryName(path).FullName).FileAccessPermissions, modes);
            }
            throw new FileNotFoundException();
        }
        
        public static void Chmod(this IPath pathWrapper, string path, FilePermissions permissions)
        {
            if (!HasAccess(pathWrapper, path, PathAccessModes.W_OK))
            {
                throw new UnauthorizedAccessException();
            }
            Syscall.chmod(pathWrapper.GetFullPath(path), permissions);
        }
        
        public static void Chown(this IPath pathWrapper, string path, uint uid, uint gid)
        {
            if (!HasAccess(pathWrapper, path, PathAccessModes.W_OK))
            {
                throw new UnauthorizedAccessException();
            }
            Syscall.lchown(pathWrapper.GetFullPath(path), uid, gid);
        }

        public static void CreateHardLink(this IPath pathWrapper, string from, string to)
        {
            if (!HasAccess(pathWrapper, from, PathAccessModes.R_OK) || !HasAccess(pathWrapper, Path.GetDirectoryName(to), PathAccessModes.W_OK))
            {
                throw new UnauthorizedAccessException();
            }
            if (Syscall.link (pathWrapper.GetFullPath(from),  pathWrapper.GetFullPath(to)) == -1)
            {
                throw new Exception();
            }
        }

        public static void CreateSymlink(this IPath pathWrapper, string from, string to)
        {
            if (!HasAccess(pathWrapper, from, PathAccessModes.R_OK) || !HasAccess(pathWrapper, Path.GetDirectoryName(to), PathAccessModes.W_OK))
            {
                throw new UnauthorizedAccessException();
            }
            var f = new UnixFileInfo(pathWrapper.GetFullPath(from));
            f.CreateSymbolicLink(to);
        }

        public static string GetSymlinkTarget(this IPath pathWrapper, string path)
        {
            return (new UnixFileInfo(pathWrapper.GetFullPath(path)).IsSymbolicLink) ?  pathWrapper.GetRelativePath(pathWrapper.GetFullPath("/"), Mono.Unix.UnixPath.GetRealPath(path)) : pathWrapper.GetRelativePath(pathWrapper.GetFullPath("/"), path);
        }
        
        private static bool CheckPathAccessModes(FileAccessPermissions permissions, PathAccessModes request)
        {
            if (request.HasFlag(PathAccessModes.R_OK))
            {
                if (!(permissions.HasFlag(FileAccessPermissions.UserRead) || permissions.HasFlag(FileAccessPermissions.GroupRead) || permissions.HasFlag(FileAccessPermissions.OtherRead)))
                {
                    return false;
                }
            }

            if (request.HasFlag(PathAccessModes.W_OK))
            {
                if (!(permissions.HasFlag(FileAccessPermissions.UserWrite) || permissions.HasFlag(FileAccessPermissions.GroupWrite) || permissions.HasFlag(FileAccessPermissions.OtherWrite)))
                {
                    return false;
                }
            }

            if (request.HasFlag(PathAccessModes.X_OK))
            {
                if (!(permissions.HasFlag(FileAccessPermissions.UserExecute) || permissions.HasFlag(FileAccessPermissions.GroupExecute) || permissions.HasFlag(FileAccessPermissions.OtherExecute)))
                {
                    return false;
                }
            }
            return true;
        }
    }
}