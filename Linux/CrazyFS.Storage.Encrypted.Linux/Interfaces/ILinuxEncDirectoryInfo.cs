using System.IO.Abstractions;
using CrazyFS.Passthrough.Linux.Interfaces;

namespace CrazyFS.FileSystem.Encrypted.Linux.Interfaces
{
    public interface ILinuxEncDirectoryInfo: ILinuxEncFileSystemInfo, ILinuxDirectoryInfo
    {
    }
}