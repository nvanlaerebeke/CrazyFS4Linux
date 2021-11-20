using System.IO.Abstractions;
using CrazyFS.Passthrough.Linux.Interfaces;

namespace CrazyFS.Storage.Passthrough.Linux
{
    public class LinuxDirectoryInfoFactory: ILinuxDirectoryInfoFactory
    {
        protected readonly IFileSystem _fileSystem;
        protected readonly string _source;
        protected readonly string _destination;

        public LinuxDirectoryInfoFactory(IFileSystem fileSystem, string source, string destination)
        {
            _fileSystem = fileSystem;
            _source = source;
            _destination = destination;
        }

        public virtual IDirectoryInfo FromDirectoryName(string directoryName)
        {
            return new LinuxDirectoryInfo(_fileSystem, _source, _destination, directoryName);
        }

        public virtual IDirectoryInfo FromDirectoryInfo(IDirectoryInfo info)
        {
            if (info is ILinuxDirectoryInfo) return info;
            return new LinuxDirectoryInfo(_fileSystem, _source, _destination, info);
        }
    }
}