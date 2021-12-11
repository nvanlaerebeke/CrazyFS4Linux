using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Security.AccessControl;
using CrazyFS.FileSystem;
using CrazyFS.Passthrough.Linux.Extensions;
using CrazyFS.Passthrough.Linux.Interfaces;
using Mono.Unix.Native;

namespace CrazyFS.Storage.Passthrough.Linux
{
    public class LinuxDirectoryWrapper : ILinuxDirectory
    {
        protected readonly IDirectory _directory;
        protected readonly string _source;

        public LinuxDirectoryWrapper(IFileSystem fileSystem, string source)
        {
            FileSystem = fileSystem;
            _source = source;
            _directory = new DirectoryWrapper(fileSystem);
        }

        public virtual IDirectoryInfo CreateDirectory(string path)
        {
            return _directory.CreateDirectory(path.GetPath(_source));
        }

        public virtual IDirectoryInfo CreateDirectory(string path, DirectorySecurity directorySecurity)
        {
            return _directory.CreateDirectory(path.GetPath(_source), directorySecurity);
        }

        public virtual void CreateDirectory(string path, FilePermissions permissions)
        {
            if (Syscall.mkdir(path.GetPath(_source), permissions) != -1)
                FileSystem.DirectoryInfo.FromDirectoryName(path.GetPath(_source));
            throw new NativeException((int) Stdlib.GetLastError());
        }

        public virtual void Delete(string path)
        {
            _directory.Delete(path.GetPath(_source));
        }

        public virtual void Delete(string path, bool recursive)
        {
            _directory.Delete(path.GetPath(_source), recursive);
        }

        public virtual bool Exists(string path)
        {
            return _directory.Exists(path.GetPath(_source));
        }

        public virtual DirectorySecurity GetAccessControl(string path)
        {
            return _directory.GetAccessControl(path.GetPath(_source));
        }

        public virtual DirectorySecurity GetAccessControl(string path, AccessControlSections includeSections)
        {
            return _directory.GetAccessControl(path.GetPath(_source), includeSections);
        }

        public virtual DateTime GetCreationTime(string path)
        {
            return _directory.GetCreationTime(path.GetPath(_source));
        }

        public virtual DateTime GetCreationTimeUtc(string path)
        {
            return _directory.GetCreationTimeUtc(path.GetPath(_source));
        }

        public virtual string GetCurrentDirectory()
        {
            return _directory.GetCurrentDirectory();
        }

        public virtual string[] GetDirectories(string path)
        {
            return _directory.GetDirectories(path.GetPath(_source));
        }

        public virtual string[] GetDirectories(string path, string searchPattern)
        {
            return _directory.GetDirectories(path.GetPath(_source), searchPattern);
        }

        public virtual string[] GetDirectories(string path, string searchPattern, SearchOption searchOption)
        {
            return _directory.GetDirectories(path.GetPath(_source), searchPattern, searchOption);
        }

        public virtual string[] GetDirectories(string path, string searchPattern, EnumerationOptions enumerationOptions)
        {
            return _directory.GetDirectories(path.GetPath(_source), searchPattern, enumerationOptions);
        }

        public virtual string GetDirectoryRoot(string path)
        {
            return _directory.GetDirectoryRoot(path.GetPath(_source));
        }

        public virtual string[] GetFiles(string path)
        {
            return _directory.GetFiles(path.GetPath(_source));
        }

        public virtual string[] GetFiles(string path, string searchPattern)
        {
            return _directory.GetFiles(path.GetPath(_source), searchPattern);
        }

        public virtual string[] GetFiles(string path, string searchPattern, SearchOption searchOption)
        {
            return _directory.GetFiles(path.GetPath(_source), searchPattern, searchOption);
        }

        public virtual string[] GetFiles(string path, string searchPattern, EnumerationOptions enumerationOptions)
        {
            return _directory.GetFiles(path.GetPath(_source), searchPattern, enumerationOptions);
        }

        public virtual string[] GetFileSystemEntries(string path)
        {
            return _directory.GetFileSystemEntries(path.GetPath(_source));
        }

        public virtual string[] GetFileSystemEntries(string path, string searchPattern)
        {
            return _directory.GetFileSystemEntries(path.GetPath(_source), searchPattern);
        }

        public virtual DateTime GetLastAccessTime(string path)
        {
            return _directory.GetLastAccessTime(path.GetPath(_source));
        }

        public virtual DateTime GetLastAccessTimeUtc(string path)
        {
            return _directory.GetLastAccessTimeUtc(path.GetPath(_source));
        }

        public virtual DateTime GetLastWriteTime(string path)
        {
            return _directory.GetLastWriteTime(path.GetPath(_source));
        }

        public virtual DateTime GetLastWriteTimeUtc(string path)
        {
            return _directory.GetLastWriteTimeUtc(path.GetPath(_source));
        }

        public virtual string[] GetLogicalDrives()
        {
            return _directory.GetLogicalDrives();
        }

        public virtual IDirectoryInfo GetParent(string path)
        {
            return _directory.GetParent(path.GetPath(_source));
        }

        public virtual void Move(string sourceDirName, string destDirName)
        {
            _directory.Move(sourceDirName.GetPath(_source), destDirName.GetPath(_source));
        }

        public virtual void SetAccessControl(string path, DirectorySecurity directorySecurity)
        {
            _directory.SetAccessControl(path.GetPath(_source), directorySecurity);
        }

        public virtual void SetCreationTime(string path, DateTime creationTime)
        {
            _directory.SetCreationTime(path.GetPath(_source), creationTime);
        }

        public virtual void SetCreationTimeUtc(string path, DateTime creationTimeUtc)
        {
            _directory.SetCreationTimeUtc(path.GetPath(_source), creationTimeUtc);
        }

        public virtual void SetCurrentDirectory(string path)
        {
            _directory.SetCurrentDirectory(path.GetPath(_source));
        }

        public virtual void SetLastAccessTime(string path, DateTime lastAccessTime)
        {
            _directory.SetLastAccessTime(path.GetPath(_source), lastAccessTime);
        }

        public virtual void SetLastAccessTimeUtc(string path, DateTime lastAccessTimeUtc)
        {
            _directory.SetLastAccessTimeUtc(path.GetPath(_source), lastAccessTimeUtc);
        }

        public virtual void SetLastWriteTime(string path, DateTime lastWriteTime)
        {
            _directory.SetLastWriteTime(path.GetPath(_source), lastWriteTime);
        }

        public virtual void SetLastWriteTimeUtc(string path, DateTime lastWriteTimeUtc)
        {
            _directory.SetLastWriteTimeUtc(path.GetPath(_source), lastWriteTimeUtc);
        }

        public virtual IEnumerable<string> EnumerateDirectories(string path)
        {
            return _directory.EnumerateDirectories(path.GetPath(_source));
        }

        public virtual IEnumerable<string> EnumerateDirectories(string path, string searchPattern)
        {
            return _directory.EnumerateDirectories(path.GetPath(_source), searchPattern);
        }

        public virtual IEnumerable<string> EnumerateDirectories(string path, string searchPattern,
            SearchOption searchOption)
        {
            return _directory.EnumerateDirectories(path.GetPath(_source), searchPattern, searchOption);
        }

        public virtual IEnumerable<string> EnumerateDirectories(string path, string searchPattern,
            EnumerationOptions enumerationOptions)
        {
            return _directory.EnumerateDirectories(path.GetPath(_source), searchPattern, enumerationOptions);
        }

        public virtual IEnumerable<string> EnumerateFiles(string path)
        {
            return _directory.EnumerateFiles(path.GetPath(_source));
        }

        public virtual IEnumerable<string> EnumerateFiles(string path, string searchPattern)
        {
            return _directory.EnumerateFiles(path.GetPath(_source), searchPattern);
        }

        public virtual IEnumerable<string> EnumerateFiles(string path, string searchPattern, SearchOption searchOption)
        {
            return _directory.EnumerateFiles(path.GetPath(_source), searchPattern, searchOption);
        }

        public virtual IEnumerable<string> EnumerateFiles(string path, string searchPattern,
            EnumerationOptions enumerationOptions)
        {
            return _directory.EnumerateFiles(path.GetPath(_source), searchPattern, enumerationOptions);
        }

        public virtual IEnumerable<string> EnumerateFileSystemEntries(string path)
        {
            return _directory.EnumerateFileSystemEntries(path.GetPath(_source));
        }

        public virtual IEnumerable<string> EnumerateFileSystemEntries(string path, string searchPattern)
        {
            return _directory.EnumerateFileSystemEntries(path.GetPath(_source), searchPattern);
        }

        public virtual IEnumerable<string> EnumerateFileSystemEntries(string path, string searchPattern,
            SearchOption searchOption)
        {
            return _directory.EnumerateFileSystemEntries(path.GetPath(_source), searchPattern, searchOption);
        }

        public virtual IEnumerable<string> EnumerateFileSystemEntries(string path, string searchPattern,
            EnumerationOptions enumerationOptions)
        {
            return _directory.EnumerateFileSystemEntries(path.GetPath(_source), searchPattern, enumerationOptions);
        }

        public IFileSystem FileSystem { get; }

        public virtual void CreateSpecialFile(string path, FilePermissions mode, ulong rdev)
        {
            if (Syscall.mknod(path.GetPath(_source), mode, rdev) != -1)
                throw new NativeException((int) Stdlib.GetLastError());
        }
    }
}