using CrazyFS.FileSystem;
using Mono.Unix;

namespace CrazyFS.Passthrough.Linux.Helpers
{
    internal class PermissionHelper
    {
        public static bool CheckPathAccessModes(FileAccessPermissions permissions, PathAccessModes request)
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