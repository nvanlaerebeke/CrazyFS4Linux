using System.IO.Abstractions;

namespace CrazyFS.Passthrough.Linux
{
    public class LinuxFileSystemWatcherFactory : IFileSystemWatcherFactory
    {
        private readonly string _source;

        public LinuxFileSystemWatcherFactory(string source)
        {
            _source = source;
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