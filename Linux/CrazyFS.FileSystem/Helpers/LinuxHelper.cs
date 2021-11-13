using System.IO;
using Mono.Unix;
using Mono.Unix.Native;

namespace CrazyFS.FileSystem.Helpers
{
    internal static class LinuxHelper
    {
        public static Result CreateHardLink(string from, string to)
        {
            if (!PermissionHelper.HasAccess(from, PathAccessModes.R_OK) || !PermissionHelper.HasAccess(Path.GetDirectoryName(to), PathAccessModes.W_OK))
            {
                return new Result(ResultStatus.AccessDenied);
            }
            int r = Syscall.link (from, to);
            if (r == -1)
            {
                return new Result(ResultStatus.Error);
            }
            return new Result(ResultStatus.Success);
        }

        public static Result CreateSymlink(string from, string to)
        {
            if (!PermissionHelper.HasAccess(from, PathAccessModes.R_OK) || !PermissionHelper.HasAccess(Path.GetDirectoryName(to), PathAccessModes.W_OK))
            {
                return new Result(ResultStatus.AccessDenied);
            }
            var f = new UnixFileInfo(from);
            f.CreateSymbolicLink(to);
            return new Result(ResultStatus.Success);
        }

        public static string GetSymlinkTarget(string path)
        {
            return (new UnixFileInfo(path).IsSymbolicLink) ?  Mono.Unix.UnixPath.GetRealPath(path) :  path;
        }
    }
}