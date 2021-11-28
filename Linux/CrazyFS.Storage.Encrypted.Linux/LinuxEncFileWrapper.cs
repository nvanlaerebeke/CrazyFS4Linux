using System.IO.Abstractions;
using CrazyFS.Encryption;
using CrazyFS.FileSystem.Encrypted.Linux.Extensions;
using CrazyFS.Passthrough.Linux.Extensions;
using CrazyFS.Storage.Passthrough.Linux;
using Mono.Unix.Native;

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
            var encPath = FileSystem.Path.GetEncryptedPath(path, true);
            if (!string.IsNullOrEmpty(encPath))
            {
                return System.IO.File.Exists(encPath.GetPath(_source));
            }
            return false;
        }

        public override void CreateSpecialFile(string path, FilePermissions mode, ulong rdev)
        {
            var encPath = FileSystem.Path.GetEncryptedPath(path, false);
            base.CreateSpecialFile(encPath, mode, rdev);
        }
    }
}