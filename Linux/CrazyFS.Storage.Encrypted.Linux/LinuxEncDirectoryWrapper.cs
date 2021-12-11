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
    public class LinuxEncDirectoryWrapper : LinuxDirectoryWrapper
    {
        private readonly string _destination;
        private readonly IEncryption _encryption;
        private readonly IDirectory _directory;
        public LinuxEncDirectoryWrapper(IFileSystem fileSystem, string source, string destination, IEncryption encryption) : base(fileSystem, source)
        {
            _destination = destination;
            _encryption = encryption;
            _directory = new DirectoryWrapper(fileSystem);
        }
        public override IDirectoryInfo CreateDirectory(string path)
        {
            return FileSystem.DirectoryInfo.GetFromDirectoryInfo(base.CreateDirectory(FileSystem.Path.GetEncryptedPath(path, false)));
        }

        public override IDirectoryInfo CreateDirectory(string path, DirectorySecurity directorySecurity)
        {
            return FileSystem.DirectoryInfo.GetFromDirectoryInfo(base.CreateDirectory(FileSystem.Path.GetEncryptedPath(path, false), directorySecurity));
        }

        public override void CreateDirectory(string path, FilePermissions permissions)
        {
            if (Syscall.mkdir(FileSystem.Path.GetEncryptedPath(path, false).GetPath(_source), permissions) != -1) return;
            throw new NativeException((int)Stdlib.GetLastError());
        }

        public override void Delete(string path)
        {
            try
            {
                base.Delete(FileSystem.Path.GetEncryptedPath(path, true).GetPath(_source));
            }
            catch (FileNotFoundException)
            {
                throw new DirectoryNotFoundException(path);
            }
        }

        public override void Delete(string path, bool recursive)
        {
            try
            {
                base.Delete(FileSystem.Path.GetEncryptedPath(path, true).GetPath(_source), recursive);
            }
            catch (FileNotFoundException)
            {
                throw new DirectoryNotFoundException(path);
            }
        }

        public override bool Exists(string path)
        {
            try
            {
                return base.Exists(FileSystem.Path.GetEncryptedPath(path, true));
            }
            catch (FileNotFoundException)
            {
                throw new DirectoryNotFoundException(path);
            }
        }

        public override DirectorySecurity GetAccessControl(string path)
        {
            return base.GetAccessControl(FileSystem.Path.GetEncryptedPath(path, true).GetPath(_source));
        }

        public override DirectorySecurity GetAccessControl(string path, AccessControlSections includeSections)
        {
            return base.GetAccessControl(FileSystem.Path.GetEncryptedPath(path, true).GetPath(_source),
                includeSections);
        }

        public override DateTime GetCreationTime(string path)
        {
            return base.GetCreationTime(FileSystem.Path.GetEncryptedPath(path, true).GetPath(_source));
        }

        public override DateTime GetCreationTimeUtc(string path)
        {
            return base.GetCreationTimeUtc(FileSystem.Path.GetEncryptedPath(path, true).GetPath(_source));
        }

        public override string GetCurrentDirectory()
        {
            return FileSystem.Path.GetDecryptedPath(base.GetCurrentDirectory()).GetRelative(_source).GetPath(_destination);
        }

        public override string[] GetDirectories(string path)
        {
            return base.GetDirectories(
                FileSystem.Path.GetEncryptedPath(path, true).GetPath(_source)
            ).Select(x => 
                FileSystem.Path.GetDecryptedPath(x).GetRelative(_source).GetPath(_destination)
            ).ToArray();
        }

        public override string[] GetDirectories(string path, string searchPattern)
        {
            return base.GetDirectories(
                FileSystem.Path.GetEncryptedPath(path, true).GetPath(_source), 
                searchPattern
            ).Select(x => 
                FileSystem.Path.GetDecryptedPath(path).GetRelative(_source).GetPath(_destination)
            ).ToArray();
        }

        public override string[] GetDirectories(string path, string searchPattern, SearchOption searchOption)
        {
            return base.GetDirectories(
                FileSystem.Path.GetEncryptedPath(path, true).GetPath(_source), 
                searchPattern,
                searchOption
            ).Select(x => 
                FileSystem.Path.GetDecryptedPath(path).GetRelative(_source).GetPath(_destination)
            ).ToArray();
        }

        public override string[] GetDirectories(string path, string searchPattern, EnumerationOptions enumerationOptions)
        {
            return base.GetDirectories(
                FileSystem.Path.GetEncryptedPath(path, true).GetPath(_source), 
                searchPattern,
                enumerationOptions
            ).Select(x => 
                FileSystem.Path.GetDecryptedPath(path).GetRelative(_source).GetPath(_destination)
            ).ToArray();
        }

        public override string GetDirectoryRoot(string path)
        {
            return FileSystem.Path.GetDecryptedPath(
                base.GetDirectoryRoot(
                    FileSystem.Path.GetEncryptedPath(path, true).GetRelative(_source).GetPath(_destination)
                )
            );
        }

        public override string[] GetFiles(string path)
        {
            return base.GetFiles(
                FileSystem.Path.GetEncryptedPath(path, true).GetPath(_source)
            ).Select(x => 
                FileSystem.Path.GetDecryptedPath(x).GetRelative(_source).GetPath(_destination)
            ).ToArray();
        }

        public override string[] GetFiles(string path, string searchPattern)
        {
            return base.GetFiles(
                FileSystem.Path.GetEncryptedPath(path, true).GetPath(_source), 
                searchPattern
            ).Select(x => 
                FileSystem.Path.GetDecryptedPath(x).GetRelative(_source).GetPath(_destination)
            ).ToArray();
        }

        public override string[] GetFiles(string path, string searchPattern, SearchOption searchOption)
        {
            return base.GetFiles(
                FileSystem.Path.GetEncryptedPath(path, true).GetPath(_source), 
                searchPattern,
                searchOption
            ).Select(x => 
                FileSystem.Path.GetDecryptedPath(x).GetRelative(_source).GetPath(_destination)
            ).ToArray();
        }

        public override string[] GetFiles(string path, string searchPattern, EnumerationOptions enumerationOptions)
        {
            return base.GetFiles(
                FileSystem.Path.GetEncryptedPath(path, true).GetPath(_source), 
                searchPattern,
                enumerationOptions
            ).Select(x => 
                FileSystem.Path.GetDecryptedPath(x).GetRelative(_source).GetPath(_destination)
            ).ToArray();
        }

        public override string[] GetFileSystemEntries(string path)
        {
            return base.GetFileSystemEntries(
                FileSystem.Path.GetEncryptedPath(path, true).GetRelative(_source).GetPath(_destination)
            ).ToArray();
        }

        public override string[] GetFileSystemEntries(string path, string searchPattern)
        {
            return base.GetFileSystemEntries(
                FileSystem.Path.GetEncryptedPath(path, true).GetRelative(_source).GetPath(_destination),
                searchPattern
            ).ToArray();        
        }

        public override DateTime GetLastAccessTime(string path)
        {
            return base.GetLastAccessTime(FileSystem.Path.GetEncryptedPath(path, true).GetPath(_source));
        }

        public override DateTime GetLastAccessTimeUtc(string path)
        {
            return base.GetLastAccessTimeUtc(FileSystem.Path.GetEncryptedPath(path, true).GetPath(_source));
        }

        public override DateTime GetLastWriteTime(string path)
        {
            return base.GetLastWriteTime(FileSystem.Path.GetEncryptedPath(path, true).GetPath(_source));
        }

        public override DateTime GetLastWriteTimeUtc(string path)
        {
            return base.GetLastWriteTimeUtc(FileSystem.Path.GetEncryptedPath(path, true).GetPath(_source));
        }

        public override string[] GetLogicalDrives()
        {
            throw new NotImplementedException();
        }

        public override IDirectoryInfo GetParent(string path)
        {
            try
            {
                return FileSystem.DirectoryInfo.FromDirectoryName(
                    base.GetParent(
                        FileSystem.Path.GetEncryptedPath(path, true).GetPath(_source)
                    ).FullName
                );
            }
            catch (FileNotFoundException)
            {
                throw new DirectoryNotFoundException(path);
            }
        }

        public override void Move(string sourceDirName, string destDirName)
        {
            try
            {
                base.Move(
                    FileSystem.Path.GetEncryptedPath(sourceDirName, true), 
                    FileSystem.Path.GetEncryptedPath(destDirName, false)
                );
            }
            catch (FileNotFoundException)
            {
                throw new DirectoryNotFoundException(sourceDirName);
            }
        }

        public override void SetAccessControl(string path, DirectorySecurity directorySecurity)
        {
            base.SetAccessControl(FileSystem.Path.GetEncryptedPath(path, true), directorySecurity);
        }

        public override void SetCreationTime(string path, DateTime creationTime)
        {
            base.SetCreationTime(FileSystem.Path.GetEncryptedPath(path, true).GetPath(_source), creationTime);
        }

        public override void SetCreationTimeUtc(string path, DateTime creationTimeUtc)
        {
            base.SetCreationTimeUtc(FileSystem.Path.GetEncryptedPath(path, true).GetPath(_source), creationTimeUtc);
        }

        public override void SetLastAccessTime(string path, DateTime lastAccessTime)
        {
            base.SetLastAccessTime(FileSystem.Path.GetEncryptedPath(path, true).GetPath(_source), lastAccessTime);
        }

        public override void SetLastAccessTimeUtc(string path, DateTime lastAccessTimeUtc)
        {
            base.SetLastAccessTimeUtc(FileSystem.Path.GetEncryptedPath(path, true).GetPath(_source), lastAccessTimeUtc);
        }

        public override void SetLastWriteTime(string path, DateTime lastWriteTime)
        {
            base.SetLastWriteTime(FileSystem.Path.GetEncryptedPath(path, true).GetPath(_source), lastWriteTime);
        }

        public override void SetLastWriteTimeUtc(string path, DateTime lastWriteTimeUtc)
        {
            base.SetLastWriteTimeUtc(FileSystem.Path.GetEncryptedPath(path, true).GetPath(_source), lastWriteTimeUtc);
        }

        public override IEnumerable<string> EnumerateDirectories(string path)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<string> EnumerateDirectories(string path, string searchPattern)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<string> EnumerateDirectories(string path, string searchPattern, SearchOption searchOption)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<string> EnumerateDirectories(string path, string searchPattern, EnumerationOptions enumerationOptions)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<string> EnumerateFiles(string path)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<string> EnumerateFiles(string path, string searchPattern)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<string> EnumerateFiles(string path, string searchPattern, SearchOption searchOption)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<string> EnumerateFiles(string path, string searchPattern, EnumerationOptions enumerationOptions)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<string> EnumerateFileSystemEntries(string path)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<string> EnumerateFileSystemEntries(string path, string searchPattern)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<string> EnumerateFileSystemEntries(string path, string searchPattern, SearchOption searchOption)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<string> EnumerateFileSystemEntries(string path, string searchPattern, EnumerationOptions enumerationOptions)
        {
            throw new NotImplementedException();
        }

        public override void CreateSpecialFile(string path, FilePermissions mode, ulong rdev)
        {
            base.CreateSpecialFile(FileSystem.Path.GetEncryptedPath(path, false).GetPath(_source), mode, rdev);
        }
    }
}