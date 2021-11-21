using System.IO.Abstractions;
using CrazyFS.Encryption;
using CrazyFS.FileSystem.Encrypted.Linux.Extensions;
using CrazyFS.Storage.Passthrough.Linux;

namespace CrazyFS.FileSystem.Encrypted.Linux
{
    public class LinuxEncFileWrapper : LinuxFileWrapper
    {
        private readonly IEncryption _encryption;

        public LinuxEncFileWrapper(IFileSystem fileSystem, string source, IEncryption encryption) : base(fileSystem, source)
        {
            _encryption = encryption;
        }

        public override bool Exists(string path)
        {
            return !string.IsNullOrEmpty(FileSystem.Path.GetEncryptedPath(path, true));
        }
    }
}