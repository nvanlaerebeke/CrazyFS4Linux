using System.IO.Abstractions;

namespace CrazyFS.FileSystem
{
    public class FileSystem<T> : IFileSystem where T: IFileSystem, new() 
    {
        private readonly IFileSystem _fileSystem;
        
        public FileSystem()
        {
            _fileSystem = new T();
        }
        
        public IFile File => _fileSystem.File;
        public IDirectory Directory => _fileSystem.Directory;
        public IFileInfoFactory FileInfo => _fileSystem.FileInfo;
        public IFileStreamFactory FileStream => _fileSystem.FileStream;
        public IPath Path => _fileSystem.Path;
        public IDirectoryInfoFactory DirectoryInfo => _fileSystem.DirectoryInfo;
        public IDriveInfoFactory DriveInfo => _fileSystem.DriveInfo;
        public IFileSystemWatcherFactory FileSystemWatcher => _fileSystem.FileSystemWatcher;
    }
}