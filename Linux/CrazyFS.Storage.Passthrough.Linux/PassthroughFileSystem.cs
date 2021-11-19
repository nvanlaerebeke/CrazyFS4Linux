using System.IO.Abstractions;
using CrazyFS.Passthrough;

namespace CrazyFS.Storage.Passthrough.Linux
{
    public class PassthroughFileSystem : IFileSystem
    {
        public PassthroughFileSystem(string source, string destination)
        {
            DriveInfo = new DriveInfoFactory(this);
            DirectoryInfo = new LinuxDirectoryInfoFactory(this, source, destination);
            FileInfo = new LinuxFileInfoFactory(this, source, destination);
            Path = new LinuxPathWrapper(this, source, destination);
            File = new LinuxFileWrapper(this, source);
            Directory = new LinuxDirectoryWrapper(this, source);
            FileStream = new LinuxFileStreamFactory(source);
            FileSystemWatcher = new LinuxFileSystemWatcherFactory(source);
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