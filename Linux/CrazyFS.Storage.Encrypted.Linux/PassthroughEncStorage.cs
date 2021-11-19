using System;
using System.IO.Abstractions;
using CrazyFS.Encryption;
using CrazyFS.Passthrough;
using CrazyFS.Storage.Passthrough.Linux;

namespace CrazyFS.FileSystem.Encrypted.Linux
{
    public class PassthroughEncStorage : IFileSystem, IDisposable
    {
        private readonly IEncryption _cryptor;
        public PassthroughEncStorage(string source, string destination, string password, string salt, byte[] iv)
        {
            _cryptor = new ByteCrypto(password, salt, iv);
            DriveInfo = new DriveInfoFactory(this);
            DirectoryInfo = new LinuxEncDirectoryInfoFactory(this, source, destination, _cryptor);
            FileInfo = new LinuxEncFileInfoFactory(this, source, destination, _cryptor);
            Path = new LinuxEncPathWrapper(this, source, destination, _cryptor);
            File = new LinuxEncFileWrapper(this, source, _cryptor);
            Directory = new LinuxEncDirectoryWrapper(this, source, _cryptor);
            FileStream = new LinuxEncFileStreamFactory(source, _cryptor);
            FileSystemWatcher = new LinuxEncFileSystemWatcherFactory(source, _cryptor);
        }

        public IFile File { get; }
        public IDirectory Directory { get; }
        public IFileInfoFactory FileInfo { get; }
        public IFileStreamFactory FileStream { get; }
        public IPath Path { get; }
        public IDirectoryInfoFactory DirectoryInfo { get; }
        public IDriveInfoFactory DriveInfo { get; }
        public IFileSystemWatcherFactory FileSystemWatcher { get; }

        public void Dispose()
        {
            _cryptor.Dispose();
        }
    }
}