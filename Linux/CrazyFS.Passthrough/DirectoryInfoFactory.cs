using System;
using System.IO;
using System.IO.Abstractions;

namespace CrazyFS.Passthrough
{
    [Serializable]
    internal class DirectoryInfoFactory : IDirectoryInfoFactory
    {
        private readonly IFileSystem _fileSystem;

        public DirectoryInfoFactory(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
        }

        public IDirectoryInfo FromDirectoryName(string directoryName)
        {
            var realDirectoryInfo = new DirectoryInfo(directoryName);
            return new DirectoryInfoWrapper(_fileSystem, realDirectoryInfo);
        }
    }
}