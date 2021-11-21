using System;
using System.IO;
using System.IO.Abstractions;
using System.Security.AccessControl;
using CrazyFS.Encryption;
using CrazyFS.FileSystem.Encrypted.Linux.Extensions;
using CrazyFS.FileSystem.Encrypted.Linux.Interfaces;
using CrazyFS.Passthrough.Linux.Extensions;
using CrazyFS.Passthrough.Linux.Interfaces;
using CrazyFS.Storage.Passthrough.Linux;
using Mono.Unix;
using Mono.Unix.Native;

namespace CrazyFS.FileSystem.Encrypted.Linux
{
    public class LinuxEncFileInfo : ILinuxEncFileInfo
    {
        public IFileSystem FileSystem { get; }
        private readonly IEncryption _encryption;
        private readonly string _destination;
        private readonly IFileInfo _info;
        private readonly string _source;
        private Stat _stat;

        public LinuxEncFileInfo(IFileSystem fileSystem, string source, string destination, string filePath, IEncryption encryption) : 
            this(fileSystem, source, destination, fileSystem.FileInfo.FromFileName(filePath), encryption)
        {
        }

        public LinuxEncFileInfo(IFileSystem fileSystem, string source, string destination, IFileSystemInfo info, IEncryption encryption) : 
            this(fileSystem, source, destination, (info as IFileInfo), encryption)
        {
        }

        public LinuxEncFileInfo(IFileSystem fileSystem, string source, string destination, IFileInfo info, IEncryption encryption)
        {
            FileSystem = fileSystem;
            _source = source;
            _destination = destination;
            _info = info;
            _encryption = encryption;
            _ = Syscall.lstat(_info.FullName.GetRealPath(_source, _destination), out _stat);
        }
        
        public FileAttributes Attributes { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime CreationTimeUtc { get; set; }
        public bool Exists { get; }
        public string Extension { get; }
        public string FullName { get; }
        public DateTime LastAccessTime { get; set; }
        public DateTime LastAccessTimeUtc { get; set; }
        public DateTime LastWriteTime { get; set; }
        public DateTime LastWriteTimeUtc { get; set; }
        public string Name { get; }
        
        public StreamWriter AppendText()
        {
            throw new NotImplementedException();
        }

        public IFileInfo CopyTo(string destFileName)
        {
            return FileSystem.FileInfo.GetFromFileInfo(_info.CopyTo(FileSystem.Path.GetEncryptedPath(destFileName, false)));
        }

        public IFileInfo CopyTo(string destFileName, bool overwrite)
        {
            return FileSystem.FileInfo.GetFromFileInfo(_info.CopyTo(FileSystem.Path.GetEncryptedPath(destFileName, false)));
        }

        public Stream Create()
        {
            throw new NotImplementedException();
        }

        public StreamWriter CreateText()
        {
            throw new NotImplementedException();
        }

        public void Decrypt()
        {
            throw new NotImplementedException();
        }

        public void Encrypt()
        {
            throw new NotImplementedException();
        }

        public FileSecurity GetAccessControl()
        {
            throw new NotImplementedException();
        }

        public FileSecurity GetAccessControl(AccessControlSections includeSections)
        {
            throw new NotImplementedException();
        }

        public void MoveTo(string destFileName)
        {
            _info.MoveTo(FileSystem.Path.GetEncryptedPath(destFileName, false));
        }

        public void MoveTo(string destFileName, bool overwrite)
        {
            _info.MoveTo(FileSystem.Path.GetEncryptedPath(destFileName, false), overwrite);
        }

        public Stream Open(FileMode mode)
        {
            return _info.Open(mode);
        }

        public Stream Open(FileMode mode, FileAccess access)
        {
            return _info.Open(mode, access);
        }

        public Stream Open(FileMode mode, FileAccess access, FileShare share)
        {
            return _info.Open(mode, access, share);
        }

        public Stream OpenRead()
        {
            return _info.OpenRead();
        }

        public StreamReader OpenText()
        {
            return _info.OpenText();
        }

        public Stream OpenWrite()
        {
            return _info.OpenWrite();
        }

        public IFileInfo Replace(string destinationFileName, string destinationBackupFileName)
        {
            return FileSystem.FileInfo.GetFromFileInfo(
                _info.Replace(
                    FileSystem.Path.GetEncryptedPath(destinationFileName, true), 
                    destinationBackupFileName
                )
            );
        }

        public IFileInfo Replace(string destinationFileName, string destinationBackupFileName, bool ignoreMetadataErrors)
        {
            return FileSystem.FileInfo.GetFromFileInfo(
                _info.Replace(
                    FileSystem.Path.GetEncryptedPath(destinationFileName, true), 
                    destinationBackupFileName,
                    ignoreMetadataErrors
                )
            );
        }

        public void SetAccessControl(FileSecurity fileSecurity)
        {
            _info.SetAccessControl(fileSecurity);
        }

        public IDirectoryInfo Directory => FileSystem.DirectoryInfo.GetFromDirectoryInfo(_info.Directory);
        public string DirectoryName => FileSystem.DirectoryInfo.GetFromDirectoryInfo(_info.Directory).Name;

        public string GetRealPath()
        {
            throw new NotImplementedException();
        }

        public string GetEncryptedName()
        {
            return _info.Name;
        }

        public void Delete()
        {
            _info.Delete();
        }

        public void Refresh()
        {
            _info.Refresh();
            _ = Syscall.lstat(_info.FullName.GetRealPath(_source, _destination), out _stat);
        }

        public bool IsReadOnly
        {
            get => _info.IsReadOnly;
            set => _info.IsReadOnly = value;
        }

        public long Length => _info.Length;
        public ulong st_dev => _stat.st_dev;
        public ulong st_ino => _stat.st_ino;
        public FilePermissions st_mode => _stat.st_mode;
        public ulong st_nlink => _stat.st_nlink;
        public uint st_uid => _stat.st_uid;
        public uint st_gid => _stat.st_gid;
        public ulong st_rdev => _stat.st_rdev;
        public long st_atime => _stat.st_atime;
        public long st_mtime => _stat.st_mtime;
        public long st_ctime => _stat.st_ctime;
        public long st_atime_nsec => _stat.st_atime_nsec;
        public long st_mtime_nsec => _stat.st_mtime_nsec;
        public long st_ctime_nsec => _stat.st_ctime_nsec;
        public Timespec st_atim => _stat.st_atim;
        public Timespec st_mtim => _stat.st_mtim;
        public Timespec st_ctim => _stat.st_ctim;

        public bool IsSymLink()
        {
            return new UnixSymbolicLinkInfo(FullName).FileType switch
            {
                FileTypes.SymbolicLink => true,
                _ => false
            };
        }
    }
}