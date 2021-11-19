using System.IO.Abstractions;
using CrazyFS.Encryption;
using CrazyFS.Storage.Passthrough.Linux;

namespace CrazyFS.FileSystem.Encrypted.Linux
{
    public class LinuxEncDirectoryWrapper : LinuxDirectoryWrapper
    {
        private readonly IEncryption _encryption;

        public LinuxEncDirectoryWrapper(IFileSystem fileSystem, string source, IEncryption encryption) : base(fileSystem, source)
        {
            _encryption = encryption;
        }
    }
}