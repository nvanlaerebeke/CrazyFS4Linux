using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using System.Security.AccessControl;
using CrazyFS.Encryption;
using CrazyFS.FileSystem.Encrypted.Linux.Extensions;
using CrazyFS.FileSystem.Encrypted.Linux.Interfaces;
using CrazyFS.Passthrough.Linux.Extensions;
using Mono.Unix;
using Mono.Unix.Native;

namespace CrazyFS.FileSystem.Encrypted.Linux
{
    public class LinuxEncDirectoryInfo : ILinuxEncDirectoryInfo
    {
        private readonly string _destination;
        private readonly IEncryption _encryption;
        private readonly IDirectoryInfo _info;
        private readonly string _source;
        private Stat _stat;

        public LinuxEncDirectoryInfo(IFileSystem fileSystem, string source, string destination, string dirName, IEncryption encryption) :
            this(
                fileSystem,
                source,
                destination,
                new DirectoryInfoWrapper(fileSystem, new DirectoryInfo(Path.Combine(source, fileSystem.Path.GetEncryptedPath(dirName)))),
                encryption
            )
        {
        }

        public LinuxEncDirectoryInfo(IFileSystem fileSystem, string source, string destination, IFileSystemInfo info, IEncryption encryption) : this(fileSystem, source, destination, (IDirectoryInfo) info, encryption)
        {
        }

        public LinuxEncDirectoryInfo(IFileSystem fileSystem, string source, string destination, IDirectoryInfo info, IEncryption encryption)
        {
            if (info is ILinuxEncDirectoryInfo) throw new ArgumentException("Unable to create a LinuxEncDirectory from a ILinuxEncDirectoryInfo instance");
            FileSystem = fileSystem;
            _source = source;
            _destination = destination;
            _info = info;
            _encryption = encryption;
            _ = Syscall.stat(_info.FullName, out _stat);
        }

        public DateTime LastWriteTimeUtc { get; set; }
        public string Name => FileSystem.Path.GetDecryptedPath(_info.Name);

        public IEnumerable<IFileInfo> EnumerateFiles(string searchPattern, EnumerationOptions enumerationOptions)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IFileSystemInfo> EnumerateFileSystemInfos()
        {
            return FileSystem.Path.GetFromFileSystemInfos(_info.EnumerateFileSystemInfos());
        }

        public IEnumerable<IFileSystemInfo> EnumerateFileSystemInfos(string searchPattern)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IFileSystemInfo> EnumerateFileSystemInfos(string searchPattern, SearchOption searchOption)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IFileSystemInfo> EnumerateFileSystemInfos(string searchPattern, EnumerationOptions enumerationOptions)
        {
            throw new NotImplementedException();
        }

        public DirectorySecurity GetAccessControl()
        {
            throw new NotImplementedException();
        }

        public DirectorySecurity GetAccessControl(AccessControlSections includeSections)
        {
            throw new NotImplementedException();
        }

        public IDirectoryInfo[] GetDirectories()
        {
            throw new NotImplementedException();
        }

        public IDirectoryInfo[] GetDirectories(string searchPattern)
        {
            throw new NotImplementedException();
        }

        public IDirectoryInfo[] GetDirectories(string searchPattern, SearchOption searchOption)
        {
            throw new NotImplementedException();
        }

        public IDirectoryInfo[] GetDirectories(string searchPattern, EnumerationOptions enumerationOptions)
        {
            throw new NotImplementedException();
        }

        public IFileInfo[] GetFiles()
        {
            throw new NotImplementedException();
        }

        public IFileInfo[] GetFiles(string searchPattern)
        {
            throw new NotImplementedException();
        }

        public IFileInfo[] GetFiles(string searchPattern, SearchOption searchOption)
        {
            throw new NotImplementedException();
        }

        public IFileInfo[] GetFiles(string searchPattern, EnumerationOptions enumerationOptions)
        {
            throw new NotImplementedException();
        }

        public IFileSystemInfo[] GetFileSystemInfos()
        {
            var list = _info.GetFileSystemInfos();
            return FileSystem.Path.GetFromFileSystemInfos(list).ToArray();
        }

        public IFileSystemInfo[] GetFileSystemInfos(string searchPattern)
        {
            throw new NotImplementedException();
        }

        public IFileSystemInfo[] GetFileSystemInfos(string searchPattern, SearchOption searchOption)
        {
            throw new NotImplementedException();
        }

        public IFileSystemInfo[] GetFileSystemInfos(string searchPattern, EnumerationOptions enumerationOptions)
        {
            throw new NotImplementedException();
        }

        public void SetAccessControl(DirectorySecurity directorySecurity)
        {
            throw new NotImplementedException();
        }

        public IDirectoryInfo Parent => FileSystem.DirectoryInfo.GetFromDirectoryInfo(_info.Parent);

        public IDirectoryInfo Root => FileSystem.DirectoryInfo.GetFromDirectoryInfo(_info.Root);

        public string GetRealPath()
        {
            throw new NotImplementedException();
        }

        public void Create()
        {
            throw new NotImplementedException();
        }

        public void Create(DirectorySecurity directorySecurity)
        {
            throw new NotImplementedException();
        }

        public IDirectoryInfo CreateSubdirectory(string path)
        {
            throw new NotImplementedException();
            //return _info.CreateSubdirectory(FileSystem.Path.GetEncryptedPath(path));
        }

        public void Delete(bool recursive)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IDirectoryInfo> EnumerateDirectories()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IDirectoryInfo> EnumerateDirectories(string searchPattern)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IDirectoryInfo> EnumerateDirectories(string searchPattern, SearchOption searchOption)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IDirectoryInfo> EnumerateDirectories(string searchPattern, EnumerationOptions enumerationOptions)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IFileInfo> EnumerateFiles()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IFileInfo> EnumerateFiles(string searchPattern)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IFileInfo> EnumerateFiles(string searchPattern, SearchOption searchOption)
        {
            throw new NotImplementedException();
        }

        public void Delete()
        {
            _info.Delete();
        }

        public void Refresh()
        {
            _info.Refresh();
            _ = Syscall.stat(_info.FullName, out _stat);
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
        public string Extension => Path.GetExtension(FileSystem.Path.GetDecryptedPath(_info.Name));
        public string FullName => FileSystem.Path.GetDecryptedPath(_info.FullName.GetRelative(_destination)).GetPath(_destination);

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

        public void MoveTo(string destDirName)
        {
            throw new NotImplementedException();
            //_info.MoveTo(FileSystem.Path.GetEncryptedPath(destDirName));
        }

        /*public string GetRealPath()
        {
            return FileSystem.Path.GetDecryptedPath((_info.Getre.GetRealPath());
        }*/

        public string GetEncryptedName()
        {
            return FileSystem.Path.GetDecryptedPath(_info.Name);
        }

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