using System.IO;
using System.IO.Abstractions;

namespace CrazyFS.Linux
{
    public class LinuxDirectoryInfoFactory: IDirectoryInfoFactory
    {
        private readonly IFileSystem _fileSystem;
        private readonly string _basePath;

        public LinuxDirectoryInfoFactory(IFileSystem fileSystem, string basePath)
        {
            _fileSystem = fileSystem;
            _basePath = basePath;
        }

        public IDirectoryInfo FromDirectoryName(string directoryName)
        {
            return new LinuxDirectoryInfo(_fileSystem, _basePath, directoryName);
        }
    }
}