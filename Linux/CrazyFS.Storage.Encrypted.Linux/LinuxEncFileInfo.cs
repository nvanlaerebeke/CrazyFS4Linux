using System.IO.Abstractions;
using CrazyFS.Storage.Passthrough.Linux;

namespace CrazyFS.FileSystem.Encrypted.Linux
{
    public class LinuxEncFileInfo : LinuxFileInfo
    {
        public LinuxEncFileInfo(IFileSystem fileSystem, string source, string destination, string filePath) : base(fileSystem, source, destination, filePath)
        {
        }

        public LinuxEncFileInfo(IFileSystem fileSystem, string source, string destination, IFileSystemInfo info) : base(fileSystem, source, destination, info)
        {
        }

        public LinuxEncFileInfo(IFileSystem fileSystem, string source, string destination, IFileInfo info) : base(fileSystem, source, destination, info)
        {
        }
    }
}