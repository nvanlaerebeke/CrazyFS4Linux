using System.IO.Abstractions;
using CrazyFS.FileSystem;

namespace CrazyFS.Linux
{
    public class LinuxFileSystemWatcherFactory : IFileSystemWatcherFactory
    {
        private readonly string _source;
        private readonly IFileSystemWatcherFactory _fileSystemWatcherFactory;
        
        public LinuxFileSystemWatcherFactory(string source, string destination)
        {
            _source = source;
            _fileSystemWatcherFactory = new FileSystemWatcherFactory();
        }
        
        public IFileSystemWatcher CreateNew()
        {
            return new FileSystemWatcherWrapper();
        }

        public IFileSystemWatcher CreateNew(string path) =>
            new FileSystemWatcherWrapper(path.GetPath(_source));

        public IFileSystemWatcher CreateNew(string path, string filter)
            => new FileSystemWatcherWrapper(path.GetPath(_source), filter);
    }
}