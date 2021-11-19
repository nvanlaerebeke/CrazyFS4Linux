using System.IO.Abstractions;
using CrazyFS.Encryption;
using CrazyFS.Storage.Passthrough.Linux;

namespace CrazyFS.FileSystem.Encrypted.Linux
{
    public class LinuxEncDirectoryInfoFactory : LinuxDirectoryInfoFactory
    {
        private readonly IEncryption _encryption;

        public LinuxEncDirectoryInfoFactory(IFileSystem fileSystem, string source, string destination, IEncryption encryption) : base(fileSystem, source, destination)
        {
            _encryption = encryption;
        }
    }
}