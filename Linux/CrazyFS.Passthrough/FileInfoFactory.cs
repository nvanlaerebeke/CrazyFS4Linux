using System;
using System.IO;
using System.IO.Abstractions;

namespace CrazyFS.Passthrough
{
    [Serializable]
    internal class FileInfoFactory : IFileInfoFactory
    {
        private readonly IFileSystem _fileSystem;

        public FileInfoFactory(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
        }

        public IFileInfo FromFileName(string fileName)
        {
            var realFileInfo = new FileInfo(fileName);
            return new FileInfoWrapper(_fileSystem, realFileInfo);
        }
    }
}