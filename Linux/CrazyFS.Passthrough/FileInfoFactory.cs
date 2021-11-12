using System;
using System.IO;
using System.IO.Abstractions;

namespace CrazyFS.Passthrough
{
    [Serializable]
    internal class FileInfoFactory : IFileInfoFactory
    {
        private readonly IFileSystem fileSystem;

        public FileInfoFactory(IFileSystem fileSystem)
        {
            this.fileSystem = fileSystem;
        }

        public IFileInfo FromFileName(string fileName)
        {
            var realFileInfo = new FileInfo(fileName);
            return new FileInfoWrapper(fileSystem, realFileInfo);
        }
    }
}