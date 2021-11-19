using System.IO.Abstractions;
using CrazyFS.Encryption;
using CrazyFS.Storage.Passthrough.Linux;

namespace CrazyFS.FileSystem.Encrypted.Linux
{
    public class LinuxEncFileInfoFactory : LinuxFileInfoFactory
    {
        private readonly IEncryption _encryption;

        public LinuxEncFileInfoFactory(IFileSystem fileSystem, string source, string destination, IEncryption encryption) : base(fileSystem, source, destination)
        {
            _encryption = encryption;
        }
    }
}