using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using System.Security.AccessControl;
using CrazyFS.FileSystem;
using Mono.Unix;
using Mono.Unix.Native;

namespace CrazyFS.Linux
{
    public class LinuxDirectoryInfo : IDirectoryInfo, ILinuxFileSystemInfo
    {
        private readonly IFileSystem _fileSystem;
        private readonly string _source;
        private readonly string _destination;
        private readonly IDirectoryInfo _info;
        private Stat _stat;
        public LinuxDirectoryInfo(IFileSystem fileSystem, string source, string destination, string dirName) : 
            this(fileSystem, source, destination, new DirectoryInfo(Path.Combine(source, dirName.Trim(Path.DirectorySeparatorChar))))
        { }
        
        public LinuxDirectoryInfo(IFileSystem fileSystem, string source,  string destination, DirectoryInfo info) : 
            this(fileSystem, source, destination, new DirectoryInfoWrapper(fileSystem, info))
        { }
        
        public LinuxDirectoryInfo(IFileSystem fileSystem, string source, string destination, IFileSystemInfo info):
            this(fileSystem, source, destination, info as IDirectoryInfo)
        { }
        
        public LinuxDirectoryInfo(IFileSystem fileSystem, string source, string destination, IDirectoryInfo info)
        {
            _fileSystem = fileSystem;
            _source = source;
            _destination = destination;
            _info = info;
            _ = Syscall.stat(_info.FullName, out _stat);
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

        public IFileSystem FileSystem => _fileSystem;

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
            set => _info.LastAccessTime = value;
        }

        public DateTime LastWriteTimeUtc
        {
            get => _info.LastWriteTimeUtc;
            set => _info.LastWriteTimeUtc = value;
        }

        public string Name => _info.Name;
        public void Create() => _info.Create();
        public void Create(DirectorySecurity directorySecurity) => _info.Create(directorySecurity);
        public IDirectoryInfo CreateSubdirectory(string path) => _info.CreateSubdirectory(Path.Combine(_destination, path.Trim('/'))).GetPassthroughDirectoryInfo(_source, _destination);
        public void Delete(bool recursive) => _info.Delete(recursive);
        public IEnumerable<IDirectoryInfo> EnumerateDirectories() => _info.EnumerateDirectories().GetPassthroughDirectoryInfos(_source, _destination);
        public IEnumerable<IDirectoryInfo> EnumerateDirectories(string searchPattern) => _info.EnumerateDirectories(searchPattern).GetPassthroughDirectoryInfos(_source, _destination);
        public IEnumerable<IDirectoryInfo> EnumerateDirectories(string searchPattern, SearchOption searchOption) => _info.EnumerateDirectories(searchPattern, searchOption).GetPassthroughDirectoryInfos(_source, _destination);
        public IEnumerable<IDirectoryInfo> EnumerateDirectories(string searchPattern, EnumerationOptions enumerationOptions) => _info.EnumerateDirectories(searchPattern, enumerationOptions).GetPassthroughDirectoryInfos(_source, _destination);
        public IEnumerable<IFileInfo> EnumerateFiles() => _info.EnumerateFiles().GetPassthroughFileInfos(_source, _destination);
        public IEnumerable<IFileInfo> EnumerateFiles(string searchPattern) => _info.EnumerateFiles(searchPattern).GetPassthroughFileInfos(_source, _destination);
        public IEnumerable<IFileInfo> EnumerateFiles(string searchPattern, SearchOption searchOption) => _info.EnumerateFiles(searchPattern, searchOption).GetPassthroughFileInfos(_source, _destination);
        public IEnumerable<IFileInfo> EnumerateFiles(string searchPattern, EnumerationOptions enumerationOptions) => _info.EnumerateFiles(searchPattern, enumerationOptions).GetPassthroughFileInfos(_source, _destination);

        public IEnumerable<IFileSystemInfo> EnumerateFileSystemInfos() => _info.GetFileSystemInfos().GetPassthroughFileSystemInfo(_source, _destination);
        public IEnumerable<IFileSystemInfo> EnumerateFileSystemInfos(string searchPattern) => _info.EnumerateFileSystemInfos(searchPattern).GetPassthroughFileSystemInfo(_source, _destination);
        public IEnumerable<IFileSystemInfo> EnumerateFileSystemInfos(string searchPattern, SearchOption searchOption) => _info.EnumerateFileSystemInfos(searchPattern, searchOption).GetPassthroughFileSystemInfo(_source, _destination);
        public IEnumerable<IFileSystemInfo> EnumerateFileSystemInfos(string searchPattern, EnumerationOptions enumerationOptions) => _info.EnumerateFileSystemInfos(searchPattern, enumerationOptions).GetPassthroughFileSystemInfo(_source, _destination);
        public DirectorySecurity GetAccessControl() => _info.GetAccessControl();
        public DirectorySecurity GetAccessControl(AccessControlSections includeSections) => _info.GetAccessControl(includeSections);
        public IDirectoryInfo[] GetDirectories() => _info.GetDirectories().GetPassthroughDirectoryInfos(_source, _destination).ToArray();
        public IDirectoryInfo[] GetDirectories(string searchPattern) => _info.GetDirectories(searchPattern).GetPassthroughDirectoryInfos(_source, _destination).ToArray();
        public IDirectoryInfo[] GetDirectories(string searchPattern, SearchOption searchOption) => _info.GetDirectories(searchPattern, searchOption).GetPassthroughDirectoryInfos(_source, _destination).ToArray();
        public IDirectoryInfo[] GetDirectories(string searchPattern, EnumerationOptions enumerationOptions) => _info.GetDirectories(searchPattern, enumerationOptions).GetPassthroughDirectoryInfos(_source, _destination).ToArray();
        public IFileInfo[] GetFiles() => _info.GetFiles().GetPassthroughFileInfos(_source, _destination).ToArray();
        public IFileInfo[] GetFiles(string searchPattern) => _info.GetFiles(searchPattern).GetPassthroughFileInfos(_source, _destination).ToArray();
        public IFileInfo[] GetFiles(string searchPattern, SearchOption searchOption) => _info.GetFiles(searchPattern, searchOption).GetPassthroughFileInfos(_source, _destination).ToArray();
        public IFileInfo[] GetFiles(string searchPattern, EnumerationOptions enumerationOptions) => _info.GetFiles(searchPattern, enumerationOptions).GetPassthroughFileInfos(_source, _destination).ToArray();
        public IFileSystemInfo[] GetFileSystemInfos() => _info.GetFileSystemInfos().GetPassthroughFileSystemInfo(_source, _destination).ToArray();
        public IFileSystemInfo[] GetFileSystemInfos(string searchPattern) => _info.GetFileSystemInfos(searchPattern).GetPassthroughFileSystemInfo(_source, _destination).ToArray();
        public IFileSystemInfo[] GetFileSystemInfos(string searchPattern, SearchOption searchOption) => _info.GetFileSystemInfos(searchPattern, searchOption).GetPassthroughFileSystemInfo(_source, _destination).ToArray();
        public IFileSystemInfo[] GetFileSystemInfos(string searchPattern, EnumerationOptions enumerationOptions) => _info.GetFileSystemInfos(searchPattern, enumerationOptions).GetPassthroughFileSystemInfo(_source, _destination).ToArray();
        public void MoveTo(string destDirName) => _info.MoveTo(Path.Combine(_destination, destDirName.Trim('/')));
        public void SetAccessControl(DirectorySecurity directorySecurity) => _info.SetAccessControl(directorySecurity);
        public IDirectoryInfo Parent => _info.Parent.GetPassthroughDirectoryInfo(_source, _destination);
        public IDirectoryInfo Root => _info.Root.GetPassthroughDirectoryInfo(_source, _destination);
        
        public string GetRealPath()
        {
            var passThroughPath = this.FullName.GetRealPath(_source, _destination);
            return UnixPath.GetRealPath(passThroughPath).GetMountedPath(_source, _destination);
        }
    }
}