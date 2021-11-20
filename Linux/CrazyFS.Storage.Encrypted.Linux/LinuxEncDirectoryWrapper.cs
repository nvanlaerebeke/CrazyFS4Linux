using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using System.Security.AccessControl;
using CrazyFS.Encryption;
using CrazyFS.FileSystem.Encrypted.Linux.Extensions;
using CrazyFS.FileSystem.Encrypted.Linux.Interfaces;
using CrazyFS.Storage.Passthrough.Linux;
using Mono.Unix.Native;

namespace CrazyFS.FileSystem.Encrypted.Linux
{
    public class LinuxEncDirectoryWrapper : LinuxDirectoryWrapper, ILinuxEncDirectoryWrapper
    {
        private readonly IEncryption _encryption;

        public LinuxEncDirectoryWrapper(IFileSystem fileSystem, string source, IEncryption encryption) : base(fileSystem, source)
        {
            _encryption = encryption;
        }
        public new IDirectoryInfo CreateDirectory(string path)
        {
            return base.CreateDirectory(FileSystem.Path.GetEncryptedPath(path));
        }

        public new IDirectoryInfo CreateDirectory(string path, DirectorySecurity directorySecurity)
        {
            return base.CreateDirectory(FileSystem.Path.GetEncryptedPath(path), directorySecurity);
        }

        public override void CreateDirectory(string path, FilePermissions permissions)
        {
            base.CreateDirectory(FileSystem.Path.GetEncryptedPath(path), permissions);
        }
        public void Delete(string path)
        {
            base.Delete(FileSystem.Path.GetEncryptedPath(path));
        }

        public void Delete(string path, bool recursive)
        {
            base.Delete(FileSystem.Path.GetEncryptedPath(path), recursive);
        }

        public override bool Exists(string path)
        {
            return !string.IsNullOrEmpty(FileSystem.Path.GetEncryptedPath(path));
        }

        public DirectorySecurity GetAccessControl(string path)
        {
            return base.GetAccessControl(FileSystem.Path.GetEncryptedPath(path));
        }

        public DirectorySecurity GetAccessControl(string path, AccessControlSections includeSections)
        {
            return base.GetAccessControl(FileSystem.Path.GetEncryptedPath(path), includeSections);
        }

        public DateTime GetCreationTime(string path)
        {
            return base.GetCreationTime(FileSystem.Path.GetEncryptedPath(path));
        }

        public DateTime GetCreationTimeUtc(string path)
        {
            return base.GetCreationTimeUtc(FileSystem.Path.GetEncryptedPath(path));
        }

        public string GetCurrentDirectory()
        {
            return FileSystem.Path.GetDecryptedPath(base.GetCurrentDirectory());
        }

        public string[] GetDirectories(string path)
        {
            var paths = base.GetDirectories(FileSystem.Path.GetEncryptedPath(path));
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
            return FileSystem.Path.GetDecryptedPath(base.GetDirectoryRoot(FileSystem.Path.GetEncryptedPath(path)));
        }

        public new string[] GetFiles(string path)
        {
            var paths = base.GetFiles(FileSystem.Path.GetEncryptedPath(path));
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
            var paths = base.GetFileSystemEntries(FileSystem.Path.GetEncryptedPath(path));
            for (var i = 0; i < paths.Length; i++) paths[i] = FileSystem.Path.GetDecryptedPath(paths[i]);
            return paths;
        }

        public string[] GetFileSystemEntries(string path, string searchPattern)
        {
            throw new NotImplementedException();
        }

        public new DateTime GetLastAccessTime(string path)
        {
            return base.GetLastAccessTime(FileSystem.Path.GetEncryptedPath(path));
        }

        public new DateTime GetLastAccessTimeUtc(string path)
        {
            return base.GetLastAccessTimeUtc(FileSystem.Path.GetEncryptedPath(path));
        }

        public new DateTime GetLastWriteTime(string path)
        {
            return base.GetLastWriteTime(FileSystem.Path.GetEncryptedPath(path));
        }

        public new DateTime GetLastWriteTimeUtc(string path)
        {
            return base.GetLastWriteTimeUtc(FileSystem.Path.GetEncryptedPath(path));
        }

        public new IDirectoryInfo GetParent(string path)
        {
            return base.GetParent(FileSystem.Path.GetEncryptedPath(path));
        }

        public new void Move(string sourceDirName, string destDirName)
        {
            base.Move(FileSystem.Path.GetEncryptedPath(sourceDirName), FileSystem.Path.GetEncryptedPath(destDirName));
        }

        public new void SetAccessControl(string path, DirectorySecurity directorySecurity)
        {
            base.SetAccessControl(FileSystem.Path.GetEncryptedPath(path), directorySecurity);
        }

        public new void SetCreationTime(string path, DateTime creationTime)
        {
            base.SetCreationTime(FileSystem.Path.GetEncryptedPath(path), creationTime);
        }

        public new void SetCreationTimeUtc(string path, DateTime creationTimeUtc)
        {
            base.SetCreationTimeUtc(FileSystem.Path.GetEncryptedPath(path), creationTimeUtc);
        }

        public new void SetCurrentDirectory(string path)
        {
            base.SetCurrentDirectory(FileSystem.Path.GetEncryptedPath(path));
        }

        public new void SetLastAccessTime(string path, DateTime lastAccessTime)
        {
            base.SetLastAccessTime(FileSystem.Path.GetEncryptedPath(path), lastAccessTime);
        }

        public new void SetLastAccessTimeUtc(string path, DateTime lastAccessTimeUtc)
        {
            base.SetLastAccessTimeUtc(FileSystem.Path.GetEncryptedPath(path), lastAccessTimeUtc);
        }

        public new void SetLastWriteTime(string path, DateTime lastWriteTime)
        {
            base.SetLastWriteTime(FileSystem.Path.GetEncryptedPath(path), lastWriteTime);
        }

        public new void SetLastWriteTimeUtc(string path, DateTime lastWriteTimeUtc)
        {
            base.SetLastWriteTimeUtc(FileSystem.Path.GetEncryptedPath(path), lastWriteTimeUtc);
        }

        public new IEnumerable<string> EnumerateDirectories(string path)
        {
            return  base.EnumerateDirectories(FileSystem.Path.GetEncryptedPath(path)).ToList().ConvertAll(x => FileSystem.Path.GetDecryptedPath(x));
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
            return  base.EnumerateFiles(FileSystem.Path.GetEncryptedPath(path)).ToList().ConvertAll(x => FileSystem.Path.GetDecryptedPath(x));
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
            return  base.EnumerateFileSystemEntries(FileSystem.Path.GetEncryptedPath(path)).ToList().ConvertAll(x => FileSystem.Path.GetDecryptedPath(x));
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

    }
}