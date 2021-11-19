using CrazyFS.Encryption;
using CrazyFS.Storage.Passthrough.Linux;

namespace CrazyFS.FileSystem.Encrypted.Linux
{
    public class LinuxEncFileSystemWatcherFactory : LinuxFileSystemWatcherFactory
    {
        private readonly IEncryption _encryption;

        public LinuxEncFileSystemWatcherFactory(string source, IEncryption encryption) : base(source)
        {
            _encryption = encryption;
        }
    }
}