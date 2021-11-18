using Mono.Unix;
using Mono.Unix.Native;

namespace CrazyFS.Passthrough.Linux.Helpers
{
    internal static class PermissionHelper
    {
        public static bool CheckPathAccessModes(FileAccessPermissions permissions, AccessModes request)
        {
            if (request.HasFlag(AccessModes.R_OK))
            {
                if (!(permissions.HasFlag(FileAccessPermissions.UserRead) || permissions.HasFlag(FileAccessPermissions.GroupRead) || permissions.HasFlag(FileAccessPermissions.OtherRead)))
                {
                    return false;
                }
            }

            if (request.HasFlag(AccessModes.W_OK))
            {
                if (!(permissions.HasFlag(FileAccessPermissions.UserWrite) || permissions.HasFlag(FileAccessPermissions.GroupWrite) || permissions.HasFlag(FileAccessPermissions.OtherWrite)))
                {
                    return false;
                }
            }

            // ReSharper disable once InvertIf
            if (request.HasFlag(AccessModes.X_OK))
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