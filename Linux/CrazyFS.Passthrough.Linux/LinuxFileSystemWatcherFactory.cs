using System.IO.Abstractions;

namespace CrazyFS.Linux
{
    public class LinuxFileSystemWatcherFactory : IFileSystemWatcherFactory
    {
        private readonly string _source;
        private readonly string _destination;
        private readonly IFileSystemWatcherFactory _fileSystemWatcherFactory;
        
        public LinuxFileSystemWatcherFactory(string source, string destination)
        {
            _source = source;
            _destination = destination;
            _fileSystemWatcherFactory = new FileSystemWatcherFactory();
        }
        
        public IFileSystemWatcher CreateNew()
        {
            return new FileSystemWatcherWrapper();
        }

        public IFileSystemWatcher CreateNew(string path) =>
            new FileSystemWatcherWrapper(path);

        public IFileSystemWatcher CreateNew(string path, string filter)
            => new FileSystemWatcherWrapper(path, filter);
    }
}