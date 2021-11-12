using System;
using Mono.Unix;
using Mono.Unix.Native;

namespace CrazyFS.FileSystem.Helpers
{
    internal static class PermissionHelper
    {
        /// <summary>
        /// Todo: fixed permission checking
        /// </summary>
        /// <param name="path"></param>
        /// <param name="modes"></param>
        /// <returns></returns>
        public static bool HasAccess(string path, PathAccessModes modes)
        {
            var access = new UnixFileInfo(path).FileAccessPermissions;
            if (modes.HasFlag(PathAccessModes.R_OK))
            {
                if (!(access.HasFlag(FileAccessPermissions.UserRead) || access.HasFlag(FileAccessPermissions.GroupRead) || access.HasFlag(FileAccessPermissions.OtherRead)))
                {
                    return false;
                }
            }

            if (modes.HasFlag(PathAccessModes.W_OK))
            {
                if (!(access.HasFlag(FileAccessPermissions.UserWrite) || access.HasFlag(FileAccessPermissions.GroupWrite) || access.HasFlag(FileAccessPermissions.OtherWrite)))
                {
                    return false;
                }
            }

            if (modes.HasFlag(PathAccessModes.X_OK))
            {
                if (!(access.HasFlag(FileAccessPermissions.UserExecute) || access.HasFlag(FileAccessPermissions.GroupExecute) || access.HasFlag(FileAccessPermissions.OtherExecute)))
                {
                    return false;
                }
            }
            return true;
        }

        internal static Result Chown(string path, long uid, long gid)
        {
            if (!HasAccess(path, PathAccessModes.W_OK))
            {
                return new Result(ResultStatus.AccessDenied);
            }
            Syscall.lchown(path, (uint)uid, (uint)gid);
            return new Result(ResultStatus.Success);
        }

        internal static Result Chmod(string path, FilePermissions permissions)
        {
            if (!HasAccess(path, PathAccessModes.W_OK))
            {
                return new Result(ResultStatus.AccessDenied);
            }
            Syscall.chmod(path, permissions);
            return new Result(ResultStatus.Success);
        }
    }
}