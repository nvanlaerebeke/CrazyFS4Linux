using System;
using System.IO;
using System.IO.Abstractions;
using System.Security.AccessControl;
using CrazyFS.Passthrough.Linux.Extensions;
using CrazyFS.Passthrough.Linux.Interfaces;
using Mono.Unix;
using Mono.Unix.Native;

// ReSharper disable IdentifierTypo

namespace CrazyFS.Storage.Passthrough.Linux
{
    public class LinuxFileInfo : IFileInfo, ILinuxFileSystemInfo
    {
        private readonly string _destination;
        private readonly IFileInfo _info;
        private readonly string _source;
        private Stat _stat;

        public LinuxFileInfo(IFileSystem fileSystem, string source, string destination, string filePath) :
            this(fileSystem, source, destination,
                new FileInfo(Path.Combine(source, filePath.Trim(Path.DirectorySeparatorChar))))
        {
        }

        private LinuxFileInfo(IFileSystem fileSystem, string source, string destination, FileInfo info) :
            this(fileSystem, source, destination, new FileInfoWrapper(fileSystem, info))
        {
        }

        public LinuxFileInfo(IFileSystem fileSystem, string source, string destination, IFileSystemInfo info) :
            this(fileSystem, source, destination, info as IFileInfo)
        {
        }

        public LinuxFileInfo(IFileSystem fileSystem, string source, string destination, IFileInfo info)
        {
            _source = source;
            _destination = destination;
            FileSystem = fileSystem;
            _info = info;
            _ = Syscall.lstat(_info.FullName, out _stat);
        }

        public void Delete()
        {
            _info.Delete();
        }

        public void Refresh()
        {
            _info.Refresh();
            _ = Syscall.stat(FullName, out _stat);
        }

        public IFileSystem FileSystem { get; }

        public FileAttributes Attributes
        {
            get => _info.Attributes;
            set => _info.Attributes = value;
        }

        public DateTime CreationTime
        {
            get => _info.CreationTime;
            set => _info.CreationTime = value;
        }

        public DateTime CreationTimeUtc
        {
            get => _info.CreationTimeUtc;
            set => _info.CreationTimeUtc = value;
        }

        public bool Exists => _info.Exists;
        public string Extension => _info.Extension;

        public string FullName => _info.FullName.GetMountedPath(_source, _destination);

        public DateTime LastAccessTime
        {
            get => _info.LastAccessTime;
            set => _info.LastAccessTime = value;
        }

        public DateTime LastAccessTimeUtc
        {
            get => _info.LastAccessTimeUtc;
            set => _info.LastAccessTimeUtc = value;
        }

        public DateTime LastWriteTime
        {
            get => _info.LastWriteTime;
            set => _info.LastWriteTime = value;
        }

        public DateTime LastWriteTimeUtc
        {
            get => _info.LastWriteTimeUtc;
            set => _info.LastAccessTimeUtc = value;
        }

        public string Name => _info.Name;

        public StreamWriter AppendText()
        {
            return _info.AppendText();
        }

        public IFileInfo CopyTo(string destFileName)
        {
            return FileSystem.FileInfo.GetFromFileInfo(_info.CopyTo(Path.Combine(_destination,
                destFileName.Trim('/'))));
        }

        public IFileInfo CopyTo(string destFileName, bool overwrite)
        {
            return FileSystem.FileInfo.GetFromFileInfo(_info.CopyTo(Path.Combine(_destination, destFileName.Trim('/')),
                overwrite));
        }

        public Stream Create()
        {
            return _info.Create();
        }

        public StreamWriter CreateText()
        {
            return _info.CreateText();
        }

        public void Decrypt()
        {
            _info.Decrypt();
        }

        public void Encrypt()
        {
            _info.Encrypt();
        }

        public FileSecurity GetAccessControl()
        {
            return _info.GetAccessControl();
        }

        public FileSecurity GetAccessControl(AccessControlSections includeSections)
        {
            return _info.GetAccessControl(includeSections);
        }

        public void MoveTo(string destFileName)
        {
            _info.MoveTo(Path.Combine(_destination, destFileName));
        }

        public void MoveTo(string destFileName, bool overwrite)
        {
            _info.MoveTo(Path.Combine(_destination, destFileName), overwrite);
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
                    Path.Combine(_destination, destinationFileName.Trim('/')),
                    Path.Combine(_destination, destinationBackupFileName.Trim('/'))
                )
            );
        }

        public IFileInfo Replace(string destinationFileName, string destinationBackupFileName,
            bool ignoreMetadataErrors)
        {
            return FileSystem.FileInfo.GetFromFileInfo(
                _info.Replace(
                    Path.Combine(_destination, destinationFileName.Trim('/')),
                    Path.Combine(_destination, destinationBackupFileName.Trim('/')
                    ),
                    ignoreMetadataErrors
                )
            );
        }

        public void SetAccessControl(FileSecurity fileSecurity)
        {
            _info.SetAccessControl(fileSecurity);
        }

        public IDirectoryInfo Directory => FileSystem.DirectoryInfo.FromDirectoryName(_info.DirectoryName);
        public string DirectoryName => _info.DirectoryName.GetMountedPath(_source, _destination);

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

        public string GetRealPath()
        {
            var passThroughPath = FullName.GetRealPath(_source, _destination);
            return UnixPath.GetRealPath(passThroughPath).GetMountedPath(_source, _destination);
        }
    }
}