using System.IO.Abstractions;
using CrazyFS.Linux;

namespace CrazyFS.Passthrough
{
    public class PassthroughFileSystem : IFileSystem
    {
        public PassthroughFileSystem(string source, string destination)
        {
            DriveInfo = new DriveInfoFactory(this);
            DirectoryInfo = new LinuxDirectoryInfoFactory(this, source, destination);
            FileInfo = new LinuxFileInfoFactory(this, source, destination);
            Path = new LinuxPathWrapper(this, source, destination);
            File = new LinuxFileWrapper(this, source, destination);
            Directory = new LinuxDirectoryWrapper(this, source, destination);
            FileStream = new LinuxFileStreamFactory(source, destination);
            FileSystemWatcher = new LinuxFileSystemWatcherFactory(source, destination);
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