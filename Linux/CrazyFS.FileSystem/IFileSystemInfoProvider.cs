using System.IO.Abstractions;

namespace CrazyFS.FileSystem
{
    public interface IFileSystemInfoProvider
    {
        IFileSystemInfo Get(string path);
    }
}