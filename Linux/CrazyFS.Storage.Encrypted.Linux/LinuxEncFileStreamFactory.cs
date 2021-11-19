using System;
using CrazyFS.Encryption;
using CrazyFS.Storage.Passthrough.Linux;

namespace CrazyFS.FileSystem.Encrypted.Linux
{
    [Serializable]
    public class LinuxEncFileStreamFactory : LinuxFileStreamFactory
    {
        public LinuxEncFileStreamFactory(string source, IEncryption encryption) : base(source)
        {
        }
    }
}