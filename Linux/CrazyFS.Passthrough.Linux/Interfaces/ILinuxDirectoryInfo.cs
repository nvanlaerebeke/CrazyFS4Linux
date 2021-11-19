using System.IO.Abstractions;

namespace CrazyFS.Passthrough.Linux.Interfaces
{
    public interface ILinuxDirectoryInfo : IDirectoryInfo, ILinuxFileSystemInfo
    {
        string GetRealPath();
    }
}