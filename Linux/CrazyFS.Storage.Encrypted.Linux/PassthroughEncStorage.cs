using System.IO.Abstractions;
using CrazyFS.Encryption;
using CrazyFS.Passthrough;

namespace CrazyFS.FileSystem.Encrypted.Linux
{
    public class PassthroughEncStorage : IFileSystem
    {
        public PassthroughEncStorage(string source, string destination, string password, byte[] salt)
        {
            var encryption = new ByteCrypto(password, salt);

            DriveInfo = new DriveInfoFactory(this);
            DirectoryInfo = new LinuxEncDirectoryInfoFactory(this, source, destination, encryption);
            FileInfo = new LinuxEncFileInfoFactory(this, source, destination, encryption);
            Path = new LinuxEncPathWrapper(this, source, destination, encryption);
            File = new LinuxEncFileWrapper(this, source, encryption);
            Directory = new LinuxEncDirectoryWrapper(this, source, encryption);
            FileStream = new LinuxEncFileStreamFactory(this, source, encryption);
            FileSystemWatcher = new LinuxEncFileSystemWatcherFactory(source, encryption);
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