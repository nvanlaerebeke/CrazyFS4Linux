using System.IO.Abstractions;
using Mono.Unix.Native;

namespace CrazyFS.FileSystem
{
    public interface IFuse
    {
        Result ChangeTimes(string path, long atime, long mtime);
        Result Chmod(string path, FilePermissions permissions);
        Result Chown(string path, long uid, long gid);
        Result CheckAccess(string path, PathAccessModes access);
        Result CreateDirectory(string path, FilePermissions mode);
        Result CreateHardLink(string from, string to);
        Errno CreateSpecialFile(string path, FilePermissions mode, ulong rdev);
        Result CreateSymlink(string from, string to);
        Errno GetFileSystemStatus(string path, out Statvfs stbuf);
        Result Move(string from, string to);
        
        Result GetPathInfo(string path, out IFileSystemInfo info);
        Result Write(string path, byte[] buffer, out int bytesWritten, long offset);
        
    }
}