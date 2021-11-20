using System.IO.Abstractions;
using System.Net.NetworkInformation;
using CrazyFS.Passthrough.Linux.Interfaces;

namespace CrazyFS.Storage.Passthrough.Linux
{
    public class LinuxFileInfoFactory: ILinuxFileInfoFactory
    {
        protected readonly IFileSystem _fileSystem;
        protected readonly string _source;
        protected readonly string _destination;

        public LinuxFileInfoFactory(IFileSystem fileSystem, string source, string destination)
        {
            _fileSystem = fileSystem;
            _source = source;
            _destination = destination;
        }
        
        public virtual IFileInfo FromFileName(string fileName)
        {
            return new LinuxFileInfo(_fileSystem, _source, _destination, fileName);
        }

        public virtual IFileInfo FromFileInfo(IFileInfo info)
        {
            if (info is ILinuxFileInfo) return info;
            return new LinuxFileInfo(_fileSystem, _source, _destination, info);
        }
    }
}