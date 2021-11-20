using System.IO.Abstractions;
using CrazyFS.Encryption;
using CrazyFS.FileSystem.Encrypted.Linux.Extensions;
using CrazyFS.FileSystem.Encrypted.Linux.Interfaces;
using CrazyFS.Passthrough.Linux.Interfaces;
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

        /// <summary>
        /// Gets the IDirectory from the directory path
        /// </summary>
        /// <param name="directoryName">None encrypted path</param>
        /// <returns></returns>
        public override IDirectoryInfo FromDirectoryName(string directoryName)
        {
            return new LinuxEncDirectoryInfo(_fileSystem, _source, _destination, directoryName, _encryption);
        }

        public override IDirectoryInfo FromDirectoryInfo(IDirectoryInfo info)
        {
            if (info is ILinuxEncDirectoryInfo) return info;
            return new LinuxEncDirectoryInfo(_fileSystem, _source, _destination, info, _encryption);
        }
    }
}