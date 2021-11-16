using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Security.AccessControl;
using Mono.Unix.Native;

namespace CrazyFS.Passthrough.Linux
{
    public class LinuxDirectoryWrapper : IDirectory
    {
        private readonly IFileSystem _fileSystem;
        private readonly string _source;
        private readonly IDirectory _directory;
        public LinuxDirectoryWrapper(IFileSystem fileSystem, string source)
        {
            _fileSystem = fileSystem;
            _source = source;
            _directory = new DirectoryWrapper(fileSystem);
        }

        public IDirectoryInfo CreateDirectory(string path)
        {
            return _directory.CreateDirectory(path.GetPath(_source));
        }

        public IDirectoryInfo CreateDirectory(string path, DirectorySecurity directorySecurity)
        {
            return _directory.CreateDirectory(path.GetPath(_source), directorySecurity);
        }
        public IDirectoryInfo CreateDirectory(string path, FilePermissions permissions)
        {
            if (Syscall.mkdir(path.GetPath(_source), permissions) != -1)
            {
                return _fileSystem.DirectoryInfo.FromDirectoryName(path.GetPath(_source));
            }
            throw new Exception();
        }

        public void CreateSpecialFile(string path, FilePermissions mode, ulong rdev)
        {
            if (Syscall.mknod(path.GetPath(_source), mode, rdev) != -1)
            {
                throw new Exception();
            }
        }
            
        public void Delete(string path)
        {
            _directory.Delete(path.GetPath(_source));
        }

        public void Delete(string path, bool recursive)
        {
            _directory.Delete(path.GetPath(_source), recursive);
        }

        public bool Exists(string path)
        {
            return _directory.Exists(path.GetPath(_source));
        }

        public DirectorySecurity GetAccessControl(string path)
        {
            return _directory.GetAccessControl(path.GetPath(_source));
        }

        public DirectorySecurity GetAccessControl(string path, AccessControlSections includeSections)
        {
            return _directory.GetAccessControl(path.GetPath(_source), includeSections);
        }

        public DateTime GetCreationTime(string path)
        {
            return _directory.GetCreationTime(path.GetPath(_source));
        }

        public DateTime GetCreationTimeUtc(string path)
        {
            return _directory.GetCreationTimeUtc(path.GetPath(_source));
        }

        public string GetCurrentDirectory()
        {
            return _directory.GetCurrentDirectory();
        }

        public string[] GetDirectories(string path)
        {
            return _directory.GetDirectories(path.GetPath(_source));
        }

        public string[] GetDirectories(string path, string searchPattern)
        {
            return _directory.GetDirectories(path.GetPath(_source), searchPattern);
        }

        public string[] GetDirectories(string path, string searchPattern, SearchOption searchOption)
        {
            return _directory.GetDirectories(path.GetPath(_source), searchPattern, searchOption);
        }

        public string[] GetDirectories(string path, string searchPattern, EnumerationOptions enumerationOptions)
        {
            return _directory.GetDirectories(path.GetPath(_source), searchPattern, enumerationOptions);
        }

        public string GetDirectoryRoot(string path)
        {
            return _directory.GetDirectoryRoot(path.GetPath(_source));
        }

        public string[] GetFiles(string path)
        {
            return _directory.GetFiles(path.GetPath(_source));
        }

        public string[] GetFiles(string path, string searchPattern)
        {
            return _directory.GetFiles(path.GetPath(_source), searchPattern);
        }

        public string[] GetFiles(string path, string searchPattern, SearchOption searchOption)
        {
            return _directory.GetFiles(path.GetPath(_source), searchPattern, searchOption);
        }

        public string[] GetFiles(string path, string searchPattern, EnumerationOptions enumerationOptions)
        {
            return _directory.GetFiles(path.GetPath(_source), searchPattern, enumerationOptions);
        }

        public string[] GetFileSystemEntries(string path)
        {
            return _directory.GetFileSystemEntries(path.GetPath(_source));
        }

        public string[] GetFileSystemEntries(string path, string searchPattern)
        {
            return _directory.GetFileSystemEntries(path.GetPath(_source), searchPattern);
        }

        public DateTime GetLastAccessTime(string path)
        {
            return _directory.GetLastAccessTime(path.GetPath(_source));
        }

        public DateTime GetLastAccessTimeUtc(string path)
        {
            return _directory.GetLastAccessTimeUtc(path.GetPath(_source));
        }

        public DateTime GetLastWriteTime(string path)
        {
            return _directory.GetLastWriteTime(path.GetPath(_source));
        }

        public DateTime GetLastWriteTimeUtc(string path)
        {
            return _directory.GetLastWriteTimeUtc(path.GetPath(_source));
        }

        public string[] GetLogicalDrives()
        {
            return _directory.GetLogicalDrives();
        }

        public IDirectoryInfo GetParent(string path)
        {
            return _directory.GetParent(path.GetPath(_source));
        }

        public void Move(string sourceDirName, string destDirName)
        {
            _directory.Move(sourceDirName.GetPath(_source), destDirName.GetPath(_source));
        }

        public void SetAccessControl(string path, DirectorySecurity directorySecurity)
        {
            _directory.SetAccessControl(path.GetPath(_source), directorySecurity);
        }

        public void SetCreationTime(string path, DateTime creationTime)
        {
            _directory.SetCreationTime(path.GetPath(_source), creationTime);
        }

        public void SetCreationTimeUtc(string path, DateTime creationTimeUtc)
        {
            _directory.SetCreationTimeUtc(path.GetPath(_source), creationTimeUtc);
        }

        public void SetCurrentDirectory(string path)
        {
            _directory.SetCurrentDirectory(path.GetPath(_source));
        }

        public void SetLastAccessTime(string path, DateTime lastAccessTime)
        {
            _directory.SetLastAccessTime(path.GetPath(_source), lastAccessTime);
        }

        public void SetLastAccessTimeUtc(string path, DateTime lastAccessTimeUtc)
        {
            _directory.SetLastAccessTimeUtc(path.GetPath(_source), lastAccessTimeUtc);
        }

        public void SetLastWriteTime(string path, DateTime lastWriteTime)
        {
            _directory.SetLastWriteTime(path.GetPath(_source), lastWriteTime);
        }

        public void SetLastWriteTimeUtc(string path, DateTime lastWriteTimeUtc)
        {
            _directory.SetLastWriteTimeUtc(path.GetPath(_source), lastWriteTimeUtc);
        }

        public IEnumerable<string> EnumerateDirectories(string path)
        {
            return _directory.EnumerateDirectories(path.GetPath(_source));
        }

        public IEnumerable<string> EnumerateDirectories(string path, string searchPattern)
        {
            return _directory.EnumerateDirectories(path.GetPath(_source), searchPattern);
        }

        public IEnumerable<string> EnumerateDirectories(string path, string searchPattern, SearchOption searchOption)
        {
            return _directory.EnumerateDirectories(path.GetPath(_source), searchPattern, searchOption);
        }

        public IEnumerable<string> EnumerateDirectories(string path, string searchPattern, EnumerationOptions enumerationOptions)
        {
            return _directory.EnumerateDirectories(path.GetPath(_source), searchPattern, enumerationOptions);
        }

        public IEnumerable<string> EnumerateFiles(string path)
        {
            return _directory.EnumerateFiles(path.GetPath(_source));
        }

        public IEnumerable<string> EnumerateFiles(string path, string searchPattern)
        {
            return _directory.EnumerateFiles(path.GetPath(_source), searchPattern);
        }

        public IEnumerable<string> EnumerateFiles(string path, string searchPattern, SearchOption searchOption)
        {
            return _directory.EnumerateFiles(path.GetPath(_source), searchPattern, searchOption);
        }

        public IEnumerable<string> EnumerateFiles(string path, string searchPattern, EnumerationOptions enumerationOptions)
        {
            return _directory.EnumerateFiles(path.GetPath(_source), searchPattern, enumerationOptions);
        }

        public IEnumerable<string> EnumerateFileSystemEntries(string path)
        {
            return _directory.EnumerateFileSystemEntries(path.GetPath(_source));
        }

        public IEnumerable<string> EnumerateFileSystemEntries(string path, string searchPattern)
        {
            return _directory.EnumerateFileSystemEntries(path.GetPath(_source), searchPattern);
        }

        public IEnumerable<string> EnumerateFileSystemEntries(string path, string searchPattern, SearchOption searchOption)
        {
            return _directory.EnumerateFileSystemEntries(path.GetPath(_source), searchPattern, searchOption);
        }

        public IEnumerable<string> EnumerateFileSystemEntries(string path, string searchPattern, EnumerationOptions enumerationOptions)
        {
            return _directory.EnumerateFileSystemEntries(path.GetPath(_source), searchPattern, enumerationOptions);
        }

        public IFileSystem FileSystem => _fileSystem;
    }
}