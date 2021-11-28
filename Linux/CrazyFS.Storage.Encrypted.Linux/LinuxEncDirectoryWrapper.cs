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
using CrazyFS.Storage.Passthrough.Linux;
using Mono.Unix.Native;

namespace CrazyFS.FileSystem.Encrypted.Linux
{
    public class LinuxEncDirectoryWrapper : ILinuxEncDirectoryWrapper
    {
        private readonly string _source;
        private readonly IEncryption _encryption;
        private readonly IDirectory _directory;
        public LinuxEncDirectoryWrapper(IFileSystem fileSystem, string source, IEncryption encryption)
        {
            FileSystem = fileSystem;
            _source = source;
            _encryption = encryption;
            _directory = new DirectoryWrapper(fileSystem);
        }
        public IDirectoryInfo CreateDirectory(string path)
        {
            return FileSystem.DirectoryInfo.FromDirectoryName(
                Directory.CreateDirectory(
                    FileSystem.Path.GetEncryptedPath(path, false).GetPath(_source)
                ).FullName
            );
        }

        public IDirectoryInfo CreateDirectory(string path, DirectorySecurity directorySecurity)
        {
            var encPath = FileSystem.Path.GetEncryptedPath(path, false);
            return _directory.CreateDirectory(encPath.GetPath(_source), directorySecurity);
        }
        public virtual void CreateDirectory(string path, FilePermissions permissions)
        {
            var encPath = FileSystem.Path.GetEncryptedPath(path, false);
            if (Syscall.mkdir(encPath.GetPath(_source), permissions) != -1) return;
            throw new NativeException((int)Stdlib.GetLastError());
        }

        public void Delete(string path)
        {
            var pathEnc = FileSystem.Path.GetEncryptedPath(path, true);
            if(!string.IsNullOrEmpty(pathEnc)) Directory.Delete(pathEnc.GetPath(_source));
        }

        public void Delete(string path, bool recursive)
        {
            var pathEnc = FileSystem.Path.GetEncryptedPath(path, true);
            if(!string.IsNullOrEmpty(pathEnc)) Directory.Delete(pathEnc.GetPath(_source), true);
        }

        public bool Exists(string path)
        {
            var encPath = FileSystem.Path.GetEncryptedPath(path, true);
            if (!string.IsNullOrEmpty(encPath))
            {
                return Directory.Exists(encPath.GetPath(_source));
            }
            return false;        
        }

        public DirectorySecurity GetAccessControl(string path)
        {
            throw new NotImplementedException();
        }

        public DirectorySecurity GetAccessControl(string path, AccessControlSections includeSections)
        {
            throw new NotImplementedException();
        }

        public DateTime GetCreationTime(string path)
        {
            throw new NotImplementedException();
        }

        public DateTime GetCreationTimeUtc(string path)
        {
            throw new NotImplementedException();
        }

        public string GetCurrentDirectory()
        {
            return Directory.GetCurrentDirectory();
        }

        public string[] GetDirectories(string path)
        {
            var paths = Directory.GetDirectories(FileSystem.Path.GetEncryptedPath(path, true).GetPath(_source));
            for (var i = 0; i < paths.Length; i++) paths[i] = FileSystem.Path.GetDecryptedPath(paths[i]);
            return paths;
        }

        public string[] GetDirectories(string path, string searchPattern)
        {
            throw new NotImplementedException();
        }

        public string[] GetDirectories(string path, string searchPattern, SearchOption searchOption)
        {
            throw new NotImplementedException();
        }

        public string[] GetDirectories(string path, string searchPattern, EnumerationOptions enumerationOptions)
        {
            throw new NotImplementedException();
        }

        public new string GetDirectoryRoot(string path)
        {
            return FileSystem.Path.GetDecryptedPath(Directory.GetDirectoryRoot(FileSystem.Path.GetEncryptedPath(path, true).GetPath(_source)).GetRelative(_source));
        }

        public new string[] GetFiles(string path)
        {
            var paths = Directory.GetFiles(FileSystem.Path.GetEncryptedPath(path, true).GetPath(_source));
            for (var i = 0; i < paths.Length; i++) paths[i] = FileSystem.Path.GetDecryptedPath(paths[i]);
            return paths;
        }

        public string[] GetFiles(string path, string searchPattern)
        {
            throw new NotImplementedException();
        }

        public string[] GetFiles(string path, string searchPattern, SearchOption searchOption)
        {
            throw new NotImplementedException();
        }

        public string[] GetFiles(string path, string searchPattern, EnumerationOptions enumerationOptions)
        {
            throw new NotImplementedException();
        }

        public new string[] GetFileSystemEntries(string path)
        {
            var paths = Directory.GetFileSystemEntries(FileSystem.Path.GetEncryptedPath(path, true).GetPath(_source));
            for (var i = 0; i < paths.Length; i++) paths[i] = FileSystem.Path.GetDecryptedPath(paths[i]);
            return paths;
        }

        public string[] GetFileSystemEntries(string path, string searchPattern)
        {
            throw new NotImplementedException();
        }

        public new DateTime GetLastAccessTime(string path)
        {
            return Directory.GetLastAccessTime(FileSystem.Path.GetEncryptedPath(path, true).GetPath(_source));
        }

        public new DateTime GetLastAccessTimeUtc(string path)
        {
            return Directory.GetLastAccessTimeUtc(FileSystem.Path.GetEncryptedPath(path, true).GetPath(_source));
        }

        public new DateTime GetLastWriteTime(string path)
        {
            return Directory.GetLastWriteTime(FileSystem.Path.GetEncryptedPath(path, true).GetPath(_source));
        }

        public new DateTime GetLastWriteTimeUtc(string path)
        {
            return Directory.GetLastWriteTimeUtc(FileSystem.Path.GetEncryptedPath(path, true).GetPath(_source));
        }

        public string[] GetLogicalDrives()
        {
            throw new NotImplementedException();
        }

        public IDirectoryInfo GetParent(string path)
        {
            var pathEnc = FileSystem.Path.GetEncryptedPath(path, true).GetPath(_source);
            return FileSystem.DirectoryInfo.FromDirectoryName(Directory.GetParent(pathEnc)?.FullName);
        }

        public void Move(string sourceDirName, string destDirName)
        {
            var from = FileSystem.Path.GetEncryptedPath(sourceDirName, true);
            var to = FileSystem.Path.GetEncryptedPath(destDirName, false);
            Directory.Move(from, to);
        }

        public void SetAccessControl(string path, DirectorySecurity directorySecurity)
        {
            throw new NotImplementedException();
        }

        public void SetCreationTime(string path, DateTime creationTime)
        {
            Directory.SetCreationTime(FileSystem.Path.GetEncryptedPath(path, true).GetPath(_source), creationTime);
        }

        public void SetCreationTimeUtc(string path, DateTime creationTimeUtc)
        {
            Directory.SetCreationTimeUtc(FileSystem.Path.GetEncryptedPath(path, true).GetPath(_source), creationTimeUtc);
        }

        public void SetCurrentDirectory(string path)
        {
            throw new NotImplementedException();
        }

        public void SetLastAccessTime(string path, DateTime lastAccessTime)
        {
            Directory.SetLastAccessTime(FileSystem.Path.GetEncryptedPath(path, true).GetPath(_source), lastAccessTime);
        }

        public void SetLastAccessTimeUtc(string path, DateTime lastAccessTimeUtc)
        {
            Directory.SetLastAccessTimeUtc(FileSystem.Path.GetEncryptedPath(path, true).GetPath(_source), lastAccessTimeUtc);
        }

        public void SetLastWriteTime(string path, DateTime lastWriteTime)
        {
            throw new NotImplementedException();
        }

        public void SetLastWriteTimeUtc(string path, DateTime lastWriteTimeUtc)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<string> EnumerateDirectories(string path)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<string> EnumerateDirectories(string path, string searchPattern)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<string> EnumerateDirectories(string path, string searchPattern, SearchOption searchOption)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<string> EnumerateDirectories(string path, string searchPattern, EnumerationOptions enumerationOptions)
        {
            throw new NotImplementedException();
        }

        public new IEnumerable<string> EnumerateFiles(string path)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<string> EnumerateFiles(string path, string searchPattern)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<string> EnumerateFiles(string path, string searchPattern, SearchOption searchOption)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<string> EnumerateFiles(string path, string searchPattern, EnumerationOptions enumerationOptions)
        {
            throw new NotImplementedException();
        }

        public new IEnumerable<string> EnumerateFileSystemEntries(string path)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<string> EnumerateFileSystemEntries(string path, string searchPattern)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<string> EnumerateFileSystemEntries(string path, string searchPattern, SearchOption searchOption)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<string> EnumerateFileSystemEntries(string path, string searchPattern, EnumerationOptions enumerationOptions)
        {
            throw new NotImplementedException();
        }

        public IFileSystem FileSystem { get; }
    }
}