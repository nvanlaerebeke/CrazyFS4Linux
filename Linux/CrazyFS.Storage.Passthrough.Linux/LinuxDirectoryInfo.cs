using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using System.Security.AccessControl;
using CrazyFS.Passthrough.Linux;
using CrazyFS.Passthrough.Linux.Extensions;
using CrazyFS.Passthrough.Linux.Interfaces;
using Mono.Unix;
using Mono.Unix.Native;

// ReSharper disable IdentifierTypo
namespace CrazyFS.Storage.Passthrough.Linux
{
    public class LinuxDirectoryInfo : ILinuxDirectoryInfo
    {
        protected readonly string _source;
        protected readonly string _destination;
        protected readonly IDirectoryInfo _info;
        protected Stat _stat;
        public LinuxDirectoryInfo(IFileSystem fileSystem, string source, string destination, string dirName) : 
            this(fileSystem, source, destination, new DirectoryInfo(Path.Combine(source, dirName.Trim(Path.DirectorySeparatorChar))))
        { }
        
        private LinuxDirectoryInfo(IFileSystem fileSystem, string source,  string destination, DirectoryInfo info) : 
            this(fileSystem, source, destination, new DirectoryInfoWrapper(fileSystem, info))
        { }
        
        public LinuxDirectoryInfo(IFileSystem fileSystem, string source, string destination, IFileSystemInfo info):
            this(fileSystem, source, destination, info as IDirectoryInfo)
        { }
        
        public LinuxDirectoryInfo(IFileSystem fileSystem, string source, string destination, IDirectoryInfo info)
        {
            FileSystem = fileSystem;
            _source = source;
            _destination = destination;
            _info = info;
            _ = Syscall.stat(_info.FullName.GetRealPath(_source, _destination), out _stat);
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

        public void Delete() => _info.Delete();
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
        public virtual string FullName => _info.FullName.GetMountedPath(_source, _destination);
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
            set => _info.LastAccessTime = value;
        }

        public DateTime LastWriteTimeUtc
        {
            get => _info.LastWriteTimeUtc;
            set => _info.LastWriteTimeUtc = value;
        }

        public virtual string Name => _info.Name;
        public void Create() => _info.Create();
        public void Create(DirectorySecurity directorySecurity) => 
            _info.Create(directorySecurity);
        public virtual IDirectoryInfo CreateSubdirectory(string path) => 
            FileSystem.DirectoryInfo.GetFromDirectoryInfo(_info.CreateSubdirectory(Path.Combine(_destination, path.Trim('/'))));
        public void Delete(bool recursive) => 
            _info.Delete(recursive);

        public IEnumerable<IDirectoryInfo> EnumerateDirectories()
        {
            var name = this.Name;
            var path = this.FullName;
            return FileSystem.DirectoryInfo.GetFromDirectoryInfos(_info.EnumerateDirectories());
        }

        public IEnumerable<IDirectoryInfo> EnumerateDirectories(string searchPattern) => 
            FileSystem.DirectoryInfo.GetFromDirectoryInfos(_info.EnumerateDirectories(searchPattern));
        public IEnumerable<IDirectoryInfo> EnumerateDirectories(string searchPattern, SearchOption searchOption) => 
            FileSystem.DirectoryInfo.GetFromDirectoryInfos(_info.EnumerateDirectories(searchPattern, searchOption));
        public IEnumerable<IDirectoryInfo> EnumerateDirectories(string searchPattern, EnumerationOptions enumerationOptions) => 
            FileSystem.DirectoryInfo.GetFromDirectoryInfos(_info.EnumerateDirectories(searchPattern, enumerationOptions));
        public IEnumerable<IFileInfo> EnumerateFiles() => 
            FileSystem.FileInfo.GetFromFileInfos(_info.EnumerateFiles());
        public IEnumerable<IFileInfo> EnumerateFiles(string searchPattern) => 
            FileSystem.FileInfo.GetFromFileInfos(_info.EnumerateFiles(searchPattern));

        public IEnumerable<IFileInfo> EnumerateFiles(string searchPattern, SearchOption searchOption) =>
            FileSystem.FileInfo.GetFromFileInfos(_info.EnumerateFiles(searchPattern, searchOption));

        public IEnumerable<IFileInfo> EnumerateFiles(string searchPattern, EnumerationOptions enumerationOptions) =>
            FileSystem.FileInfo.GetFromFileInfos(_info.EnumerateFiles(searchPattern, enumerationOptions));

        public virtual IEnumerable<IFileSystemInfo> EnumerateFileSystemInfos() => 
            FileSystem.Path.GetFromFileSystemInfos(_info.GetFileSystemInfos());
        public IEnumerable<IFileSystemInfo> EnumerateFileSystemInfos(string searchPattern) => 
            FileSystem.Path.GetFromFileSystemInfos(_info.EnumerateFileSystemInfos(searchPattern));
        public IEnumerable<IFileSystemInfo> EnumerateFileSystemInfos(string searchPattern, SearchOption searchOption) => 
            FileSystem.Path.GetFromFileSystemInfos(_info.EnumerateFileSystemInfos(searchPattern, searchOption));
        public IEnumerable<IFileSystemInfo> EnumerateFileSystemInfos(string searchPattern, EnumerationOptions enumerationOptions) => 
            FileSystem.Path.GetFromFileSystemInfos(_info.EnumerateFileSystemInfos(searchPattern, enumerationOptions));
        public DirectorySecurity GetAccessControl() => _info.GetAccessControl();
        public DirectorySecurity GetAccessControl(AccessControlSections includeSections) => _info.GetAccessControl(includeSections);
        public IDirectoryInfo[] GetDirectories() => 
            FileSystem.DirectoryInfo.GetFromDirectoryInfos(_info.GetDirectories()).ToArray();
        public IDirectoryInfo[] GetDirectories(string searchPattern) => 
            FileSystem.DirectoryInfo.GetFromDirectoryInfos(_info.GetDirectories(searchPattern)).ToArray();
        public IDirectoryInfo[] GetDirectories(string searchPattern, SearchOption searchOption) => 
            FileSystem.DirectoryInfo.GetFromDirectoryInfos(_info.GetDirectories(searchPattern, searchOption)).ToArray();
        public IDirectoryInfo[] GetDirectories(string searchPattern, EnumerationOptions enumerationOptions) => 
            FileSystem.DirectoryInfo.GetFromDirectoryInfos(_info.GetDirectories(searchPattern, enumerationOptions)).ToArray();
        public IFileInfo[] GetFiles() => 
            FileSystem.FileInfo.GetFromFileInfos(_info.GetFiles()).ToArray();
        public IFileInfo[] GetFiles(string searchPattern) => 
            FileSystem.FileInfo.GetFromFileInfos(_info.GetFiles(searchPattern)).ToArray();
        public IFileInfo[] GetFiles(string searchPattern, SearchOption searchOption) => 
            FileSystem.FileInfo.GetFromFileInfos(_info.GetFiles(searchPattern, searchOption)).ToArray();
        public IFileInfo[] GetFiles(string searchPattern, EnumerationOptions enumerationOptions) => 
            FileSystem.FileInfo.GetFromFileInfos(_info.GetFiles(searchPattern, enumerationOptions)).ToArray();
        public IFileSystemInfo[] GetFileSystemInfos() => 
            FileSystem.Path.GetFromFileSystemInfos(_info.GetFileSystemInfos()).ToArray();
        public IFileSystemInfo[] GetFileSystemInfos(string searchPattern) => 
            FileSystem.Path.GetFromFileSystemInfos(_info.GetFileSystemInfos(searchPattern)).ToArray();
        public IFileSystemInfo[] GetFileSystemInfos(string searchPattern, SearchOption searchOption) => 
            FileSystem.Path.GetFromFileSystemInfos(_info.GetFileSystemInfos(searchPattern, searchOption)).ToArray();
        public IFileSystemInfo[] GetFileSystemInfos(string searchPattern, EnumerationOptions enumerationOptions) => 
            FileSystem.Path.GetFromFileSystemInfos(_info.GetFileSystemInfos(searchPattern, enumerationOptions)).ToArray();
        public virtual void MoveTo(string destDirName) => 
            _info.MoveTo(Path.Combine(_destination, destDirName.Trim('/')));
        public void SetAccessControl(DirectorySecurity directorySecurity) => 
            _info.SetAccessControl(directorySecurity);
        public virtual IDirectoryInfo Parent => 
            FileSystem.DirectoryInfo.GetFromDirectoryInfo(_info.Parent);
        public virtual IDirectoryInfo Root => 
            FileSystem.DirectoryInfo.GetFromDirectoryInfo(_info.Root);
        
        public virtual string GetRealPath()
        {
            var passThroughPath = FullName.GetRealPath(_source, _destination);
            return UnixPath.GetRealPath(passThroughPath).GetMountedPath(_source, _destination);
        }
        
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