using System.IO.Abstractions;
using CrazyFS.Passthrough.Linux.Interfaces;

namespace CrazyFS.Storage.Passthrough.Linux
{
    public class LinuxDirectoryInfoFactory: ILinuxDirectoryInfoFactory
    {
        private readonly IFileSystem _fileSystem;
        private readonly string _source;
        private readonly string _destination;

        public LinuxDirectoryInfoFactory(IFileSystem fileSystem, string source, string destination)
        {
            _fileSystem = fileSystem;
            _source = source;
            _destination = destination;
        }

        public IDirectoryInfo FromDirectoryName(string directoryName)
        {
            return new LinuxDirectoryInfo(_fileSystem, _source, _destination, directoryName);
        }

        public IDirectoryInfo FromDirectoryInfo(IDirectoryInfo info)
        {
            return new LinuxDirectoryInfo(_fileSystem, _source, _destination, info);
        }
    }
}