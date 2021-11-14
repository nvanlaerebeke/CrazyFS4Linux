using System.Collections.Generic;
using System.IO.Abstractions;
using CrazyFS.Linux;
using Fuse.NET;
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
        Result CreateSpecialFile(string path, FilePermissions mode, ulong rdev);
        Result CreateSymlink(string from, string to);
        Errno GetFileSystemStatus(string path, out Statvfs stbuf);
        Result GetPathExtendedAttribute(string path, string name, byte[] value, out int bytesWritten);
        Result GetPathInfo(string path, out IFileSystemInfo info);
        Result GetSymbolicLinkTarget(string path, out string target);
        Result ListPathExtendedAttributes(string path, out string[] names);
        Result Ls(string path, out IEnumerable<IFileSystemInfo> paths);
        Errno Lock(string path, OpenedPathInfo info, FcntlCommand cmd, ref Flock @lock);
        Result Open(string path, OpenedPathInfo info);
        Result Move(string from, string to);
        void Mount();
        Result Read(string path, long offset, ulong size, out byte[] buffer, out int bytesRead);
        Result RemoveFile(string path);
        Result RemoveDirectory(string path);
        Result RemovePathExtendedAttribute(string path, string name);
        Result SetPathExtendedAttribute(string path, string name, byte[] value, XattrFlags flags);
        Result Truncate(string path, long size);
        void UnMount();
        Result Write(string path, byte[] buffer, out int bytesWritten, long offset);
    }
}