using System.IO.Abstractions;
using CrazyFS.Linux;

namespace CrazyFS.Passthrough
{
    public class FileSystem: IFileSystem
    {
        public FileSystem()
        {
            DriveInfo = new DriveInfoFactory(this);
            DirectoryInfo = new DirectoryInfoFactory(this);
            FileInfo = new LinuxFileInfoFactory(this);
            Path = new PathWrapper(this);
            File = new FileWrapper(this);
            Directory = new DirectoryWrapper(this);
            FileStream = new FileStreamFactory();
            FileSystemWatcher = new FileSystemWatcherFactory();
        }

        public IFile File { get; }
        public IDirectory Directory { get; }
        public IFileInfoFactory FileInfo { get; }
        public IFileStreamFactory FileStream { get; }
        public IPath Path { get; }
        public IDirectoryInfoFactory DirectoryInfo { get; }
        public IDriveInfoFactory DriveInfo { get; }
        public IFileSystemWatcherFactory FileSystemWatcher { get; }
    }
}