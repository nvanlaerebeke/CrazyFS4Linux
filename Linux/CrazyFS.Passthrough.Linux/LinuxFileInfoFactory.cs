using System.IO;
using System.IO.Abstractions;

namespace CrazyFS.Linux
{
    public class LinuxFileInfoFactory: IFileInfoFactory
    {
        private readonly IFileSystem _fileSystem;
        private readonly string _basePath;

        public LinuxFileInfoFactory(IFileSystem fileSystem, string basePath)
        {
            _fileSystem = fileSystem;
            _basePath = basePath;
        }
        
        public IFileInfo FromFileName(string fileName)
        {
            return new LinuxFileInfo(_fileSystem, _basePath, fileName);
        }
    }
}