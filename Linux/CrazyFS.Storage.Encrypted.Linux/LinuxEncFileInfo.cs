using System;
using System.IO;
using System.IO.Abstractions;
using System.Security.AccessControl;
using CrazyFS.Encryption;
using CrazyFS.FileSystem.Encrypted.Linux.Extensions;
using CrazyFS.FileSystem.Encrypted.Linux.Interfaces;
using CrazyFS.Passthrough.Linux.Interfaces;
using CrazyFS.Storage.Passthrough.Linux;
using Mono.Unix.Native;

namespace CrazyFS.FileSystem.Encrypted.Linux
{
    public class LinuxEncFileInfo : LinuxFileInfo, ILinuxEncFileInfo
    {
        private readonly IEncryption _encryption;

        public LinuxEncFileInfo(IFileSystem fileSystem, string source, string destination, string filePath, IEncryption encryption) : base(fileSystem, source, destination, filePath)
        {
            _encryption = encryption;
        }

        public LinuxEncFileInfo(IFileSystem fileSystem, string source, string destination, IFileSystemInfo info, IEncryption encryption) : base(fileSystem, source, destination, info)
        {
            _encryption = encryption;
        }

        public LinuxEncFileInfo(IFileSystem fileSystem, string source, string destination, IFileInfo info, IEncryption encryption) : base(fileSystem, source, destination, info)
        {
            _encryption = encryption;
        }
        
        public IFileInfo CopyTo(string destFileName)
        {
            throw new NotImplementedException();
        }

        public IFileInfo CopyTo(string destFileName, bool overwrite)
        {
            throw new NotImplementedException();
        }
        
        public void MoveTo(string destFileName)
        {
            throw new NotImplementedException();
        }

        public void MoveTo(string destFileName, bool overwrite)
        {
            throw new NotImplementedException();
        }
        
        public IFileInfo Replace(string destinationFileName, string destinationBackupFileName)
        {
            throw new NotImplementedException();
        }

        public IFileInfo Replace(string destinationFileName, string destinationBackupFileName, bool ignoreMetadataErrors)
        {
            throw new NotImplementedException();
        }

        public IDirectoryInfo Directory { get; }
        public string DirectoryName { get; }
      
        public string GetRealPath()
        {
            throw new NotImplementedException();
        }

        public string GetEncryptedName()
        {
            return base.Name;
        }
    }
}