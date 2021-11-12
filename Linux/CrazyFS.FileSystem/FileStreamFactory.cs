using System;
using System.IO;
using System.IO.Abstractions;
using Microsoft.Win32.SafeHandles;

namespace CrazyFS.FileSystem
{
    [Serializable]
    internal sealed class FileStreamFactory : IFileStreamFactory
    {
        private readonly IFileSystem _fileSystem;

        public FileStreamFactory(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
        }

        public Stream Create(string path, FileMode mode)
            => _fileSystem.FileStream.Create(path, mode);

        public Stream Create(string path, FileMode mode, FileAccess access)
            => _fileSystem.FileStream.Create(path, mode, access);

        public Stream Create(string path, FileMode mode, FileAccess access, FileShare share)
            => _fileSystem.FileStream.Create(path, mode, access, share);

        public Stream Create(string path, FileMode mode, FileAccess access, FileShare share, int bufferSize)
            => _fileSystem.FileStream.Create(path, mode, access, share, bufferSize);

        public Stream Create(string path, FileMode mode, FileAccess access, FileShare share, int bufferSize, FileOptions options)
            => _fileSystem.FileStream.Create(path, mode, access, share, bufferSize, options);

        public Stream Create(string path, FileMode mode, FileAccess access, FileShare share, int bufferSize, bool useAsync)
            => _fileSystem.FileStream.Create(path, mode, access, share, bufferSize, useAsync);

        public Stream Create(SafeFileHandle handle, FileAccess access)
            => _fileSystem.FileStream.Create(handle, access);

        public Stream Create(SafeFileHandle handle, FileAccess access, int bufferSize)
            => _fileSystem.FileStream.Create(handle, access, bufferSize);

        public Stream Create(SafeFileHandle handle, FileAccess access, int bufferSize, bool isAsync)
            => _fileSystem.FileStream.Create(handle, access, bufferSize, isAsync);

        [Obsolete("This method has been deprecated. Please use new Create(SafeFileHandle handle, FileAccess access) instead. http://go.microsoft.com/fwlink/?linkid=14202")]
        public Stream Create(IntPtr handle, FileAccess access)
            => _fileSystem.FileStream.Create(handle, access);
        
        [Obsolete("This method has been deprecated. Please use new Create(SafeFileHandle handle, FileAccess access) instead, and optionally make a new SafeFileHandle with ownsHandle=false if needed. http://go.microsoft.com/fwlink/?linkid=14202")]
        public Stream Create(IntPtr handle, FileAccess access, bool ownsHandle)
            => _fileSystem.FileStream.Create(handle, access, ownsHandle);

        [Obsolete("This method has been deprecated. Please use new Create(SafeFileHandle handle, FileAccess access, int bufferSize) instead, and optionally make a new SafeFileHandle with ownsHandle=false if needed. http://go.microsoft.com/fwlink/?linkid=14202")]
        public Stream Create(IntPtr handle, FileAccess access, bool ownsHandle, int bufferSize)
            => _fileSystem.FileStream.Create(handle, access, ownsHandle, bufferSize);

        [Obsolete("This method has been deprecated. Please use new Create(SafeFileHandle handle, FileAccess access, int bufferSize, bool isAsync) instead, and optionally make a new SafeFileHandle with ownsHandle=false if needed. http://go.microsoft.com/fwlink/?linkid=14202")]
        public Stream Create(IntPtr handle, FileAccess access, bool ownsHandle, int bufferSize, bool isAsync)
            => _fileSystem.FileStream.Create(handle, access, ownsHandle, bufferSize, isAsync);
    }
}