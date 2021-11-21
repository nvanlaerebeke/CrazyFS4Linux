using System.IO.Abstractions;
using CrazyFS.Encryption;
using CrazyFS.FileSystem.Encrypted.Linux.Extensions;
using CrazyFS.FileSystem.Encrypted.Linux.Interfaces;
using CrazyFS.Passthrough.Linux.Interfaces;
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

        public override IFileInfo FromFileName(string fileName)
        {
            return new LinuxEncFileInfo(_fileSystem, _source, _destination, _fileSystem.Path.GetEncryptedPath(fileName, true), _encryption);
        }

        public override IFileInfo FromFileInfo(IFileInfo info)
        {
            if (info is ILinuxEncFileInfo) return info;
            return new LinuxEncFileInfo(_fileSystem, _source, _destination, info, _encryption);
        }
    }
}