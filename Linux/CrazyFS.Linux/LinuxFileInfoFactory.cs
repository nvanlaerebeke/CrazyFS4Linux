using System.IO;
using System.IO.Abstractions;

namespace CrazyFS.Linux
{
    public class LinuxFileInfoFactory: IFileInfoFactory
    {
        private readonly IFileSystem _fileSystem;

        public LinuxFileInfoFactory(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
        }
        
        public IFileInfo FromFileName(string fileName)
        {
            return new LinuxFileInfo(_fileSystem, new FileInfo(fileName));
        }
    }
}