using System.IO.Abstractions;
using CrazyFS.Passthrough.Linux.Interfaces;

namespace CrazyFS.Storage.Passthrough.Linux
{
    public class LinuxFileInfoFactory: ILinuxFileInfoFactory
    {
        private readonly IFileSystem _fileSystem;
        private readonly string _source;
        private readonly string _destination;

        public LinuxFileInfoFactory(IFileSystem fileSystem, string source, string destination)
        {
            _fileSystem = fileSystem;
            _source = source;
            _destination = destination;
        }
        
        public IFileInfo FromFileName(string fileName)
        {
            return new LinuxFileInfo(_fileSystem, _source, _destination, fileName);
        }

        public IFileInfo FromFileInfo(IFileInfo info)
        {
            return new LinuxFileInfo(_fileSystem, _source, _destination, info);
        }
    }
}