using System.IO;
using System.IO.Abstractions;

namespace CrazyFS.Linux
{
    public class LinuxDirectoryInfoFactory: IDirectoryInfoFactory
    {
        private readonly IFileSystem _fileSystem;

        public LinuxDirectoryInfoFactory(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
        }

        public IDirectoryInfo FromDirectoryName(string directoryName)
        {
            return new LinuxDirectoryInfo(_fileSystem, new DirectoryInfo(directoryName));
        }
    }
}