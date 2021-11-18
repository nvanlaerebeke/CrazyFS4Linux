using System.Collections.Generic;
using System.IO.Abstractions;

namespace CrazyFS.FileSystem
{
    public interface ICrazyFSFileSystem
    {
        Result ChangeTimes(string path, long atime, long mtime);
        Result CreateHardLink(string from, string to);
        Result CreateSymlink(string from, string to);
        Result GetPathExtendedAttribute(string path, string name, byte[] value, out int bytesWritten);
        Result GetPathInfo(string path, out IFileSystemInfo info);
        Result GetSymbolicLinkTarget(string path, out string target);
        Result ListPathExtendedAttributes(string path, out string[] names);
        Result Ls(string path, out IEnumerable<IFileSystemInfo> paths);
        Result Open(string path, OpenFlags openFlags);
        Result Move(string from, string to);
        void Mount();
        Result Read(string path, long offset, ulong size, out byte[] buffer, out int bytesRead);
        Result RemoveFile(string path);
        Result RemoveDirectory(string path);
        Result RemovePathExtendedAttribute(string path, string name);
        Result Truncate(string path, long size);
        void UnMount();
        Result Write(string path, byte[] buffer, out int bytesWritten, long offset);
    }
}