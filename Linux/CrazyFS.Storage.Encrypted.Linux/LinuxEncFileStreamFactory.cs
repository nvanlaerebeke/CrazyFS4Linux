using System;
using System.IO;
using System.IO.Abstractions;
using CrazyFS.Encryption;
using CrazyFS.FileSystem.Encrypted.Linux.Extensions;
using CrazyFS.Passthrough.Linux.Extensions;
using EncFIleStorage;
using Microsoft.Win32.SafeHandles;

namespace CrazyFS.FileSystem.Encrypted.Linux
{
    [Serializable]
    public class LinuxEncFileStreamFactory : IFileStreamFactory
    {
        private readonly IEncryption _encryption;
        private readonly IFileSystem _fileSystem;
        private readonly string _source;

        public LinuxEncFileStreamFactory(IFileSystem fileSystem, string source, IEncryption encryption)
        {
            _fileSystem = fileSystem;
            _source = source;
            _encryption = encryption;
        }

        public Stream Create(string path, FileMode mode)
        {
            return new EncryptedFile(
                _fileSystem.Path.GetEncryptedPath(path, false).GetPath(_source)
            ).GetStream(mode);
        }

        public Stream Create(string path, FileMode mode, FileAccess access)
        {
            return new EncryptedFile(
                _fileSystem.Path.GetEncryptedPath(path, false).GetPath(_source)
            ).GetStream(mode, access);
        }

        public Stream Create(string path, FileMode mode, FileAccess access, FileShare share)
        {
            return new EncryptedFile(
                _fileSystem.Path.GetEncryptedPath(path, false).GetPath(_source)
            ).GetStream(mode, access, share);
        }

        public Stream Create(string path, FileMode mode, FileAccess access, FileShare share, int bufferSize)
        {
            return new EncryptedFile(
                _fileSystem.Path.GetEncryptedPath(path, false).GetPath(_source)
            ).GetStream(mode, access, share, bufferSize);
        }

        public Stream Create(string path, FileMode mode, FileAccess access, FileShare share, int bufferSize, FileOptions options)
        {
            return new EncryptedFile(
                _fileSystem.Path.GetEncryptedPath(path, false).GetPath(_source)
            ).GetStream(mode, access, share, bufferSize, options);
        }

        public Stream Create(string path, FileMode mode, FileAccess access, FileShare share, int bufferSize, bool useAsync)
        {
            return new EncryptedFile(
                _fileSystem.Path.GetEncryptedPath(path, false).GetPath(_source)
            ).GetStream(mode, access, share, bufferSize, useAsync);
        }

        public Stream Create(SafeFileHandle handle, FileAccess access)
        {
            return new FileStream(handle, access);
        }

        public Stream Create(SafeFileHandle handle, FileAccess access, int bufferSize)
        {
            return new FileStream(handle, access, bufferSize);
        }

        public Stream Create(SafeFileHandle handle, FileAccess access, int bufferSize, bool isAsync)
        {
            return new FileStream(handle, access, bufferSize, isAsync);
        }

        [Obsolete("This method has been deprecated. Please use new Create(SafeFileHandle handle, FileAccess access) instead. http://go.microsoft.com/fwlink/?linkid=14202")]
        public Stream Create(IntPtr handle, FileAccess access)
        {
            throw new NotImplementedException();
        }

        [Obsolete("This method has been deprecated. Please use new Create(SafeFileHandle handle, FileAccess access) instead, and optionally make a new SafeFileHandle with ownsHandle=false if needed. http://go.microsoft.com/fwlink/?linkid=14202")]
        public Stream Create(IntPtr handle, FileAccess access, bool ownsHandle)
        {
            throw new NotImplementedException();
        }

        [Obsolete("This method has been deprecated. Please use new Create(SafeFileHandle handle, FileAccess access, int bufferSize) instead, and optionally make a new SafeFileHandle with ownsHandle=false if needed. http://go.microsoft.com/fwlink/?linkid=14202")]
        public Stream Create(IntPtr handle, FileAccess access, bool ownsHandle, int bufferSize)
        {
            throw new NotImplementedException();
        }

        [Obsolete("This method has been deprecated. Please use new Create(SafeFileHandle handle, FileAccess access, int bufferSize, bool isAsync) instead, and optionally make a new SafeFileHandle with ownsHandle=false if needed. http://go.microsoft.com/fwlink/?linkid=14202")]
        public Stream Create(IntPtr handle, FileAccess access, bool ownsHandle, int bufferSize, bool isAsync)
        {
            throw new NotImplementedException();
        }
    }
}