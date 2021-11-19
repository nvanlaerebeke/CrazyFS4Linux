using System.IO.Abstractions;
using CrazyFS.Storage.Passthrough.Linux;

namespace CrazyFS.FileSystem.Encrypted.Linux
{
    public class LinuxEncDirectoryInfo : LinuxDirectoryInfo
    {
        public LinuxEncDirectoryInfo(IFileSystem fileSystem, string source, string destination, string dirName) : base(fileSystem, source, destination, dirName)
        {
        }

        public LinuxEncDirectoryInfo(IFileSystem fileSystem, string source, string destination, IFileSystemInfo info) : base(fileSystem, source, destination, info)
        {
        }

        public LinuxEncDirectoryInfo(IFileSystem fileSystem, string source, string destination, IDirectoryInfo info) : base(fileSystem, source, destination, info)
        {
        }
    }
}