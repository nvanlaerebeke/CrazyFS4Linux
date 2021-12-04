using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Runtime.CompilerServices;
using System.Security.AccessControl;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CrazyFS.Encryption;
using CrazyFS.FileSystem.Encrypted.Linux.Extensions;
using CrazyFS.Passthrough.Linux.Extensions;
using CrazyFS.Storage.Passthrough.Linux;
using Mono.Unix.Native;

namespace CrazyFS.FileSystem.Encrypted.Linux
{
    public class LinuxEncFileWrapper : LinuxFileWrapper
    {
        private readonly IEncryption _encryption;

        public LinuxEncFileWrapper(IFileSystem fileSystem, string source, IEncryption encryption) : base(fileSystem, source)
        {
            _encryption = encryption;
        }

        public override void Encrypt(string path)
        {
            base.Encrypt(path);
        }

        public override bool Exists(string path)
        {
            var encPath = FileSystem.Path.GetEncryptedPath(path, true);
            if (!string.IsNullOrEmpty(encPath))
            {
                return File.Exists(encPath.GetPath(_source));
            }

            return false;
        }

        public override FileSecurity GetAccessControl(string path)
        {
            return base.GetAccessControl(FileSystem.Path.GetEncryptedPath(path, true));
        }

        public override FileSecurity GetAccessControl(string path, AccessControlSections includeSections)
        {
            return base.GetAccessControl(FileSystem.Path.GetEncryptedPath(path, true), includeSections);
        }

        public override FileAttributes GetAttributes(string path)
        {
            return base.GetAttributes(FileSystem.Path.GetEncryptedPath(path, true));
        }

        public override DateTime GetCreationTime(string path)
        {
            return base.GetCreationTime(FileSystem.Path.GetEncryptedPath(path, true));
        }

        public override DateTime GetCreationTimeUtc(string path)
        {
            return base.GetCreationTimeUtc(FileSystem.Path.GetEncryptedPath(path, true));
        }

        public override DateTime GetLastAccessTime(string path)
        {
            return base.GetLastAccessTime(FileSystem.Path.GetEncryptedPath(path, true));
        }

        public override DateTime GetLastAccessTimeUtc(string path)
        {
            return base.GetLastAccessTimeUtc(FileSystem.Path.GetEncryptedPath(path, true));
        }

        public override DateTime GetLastWriteTime(string path)
        {
            return base.GetLastWriteTime(FileSystem.Path.GetEncryptedPath(path, true));
        }

        public override DateTime GetLastWriteTimeUtc(string path)
        {
            return base.GetLastWriteTimeUtc(FileSystem.Path.GetEncryptedPath(path, true));
        }

        public override void Move(string sourceFileName, string destFileName)
        {
            base.Move(FileSystem.Path.GetEncryptedPath(sourceFileName, true), FileSystem.Path.GetEncryptedPath(destFileName, false));
        }

        public override void Move(string sourceFileName, string destFileName, bool overwrite)
        {
            base.Move(FileSystem.Path.GetEncryptedPath(sourceFileName, true), FileSystem.Path.GetEncryptedPath(destFileName, false), overwrite);
        }

        public override Stream Open(string path, FileMode mode)
        {
            return base.Open(FileSystem.Path.GetEncryptedPath(path, true), mode);
        }

        public override Stream Open(string path, FileMode mode, FileAccess access)
        {
            return base.Open(FileSystem.Path.GetEncryptedPath(path, true), mode, access);
        }

        public override Stream Open(string path, FileMode mode, FileAccess access, FileShare share)
        {
            return base.Open(FileSystem.Path.GetEncryptedPath(path, true), mode, access, share);
        }

        public override Stream OpenRead(string path)
        {
            return base.OpenRead(FileSystem.Path.GetEncryptedPath(path, true));
        }

        public override StreamReader OpenText(string path)
        {
            return base.OpenText(FileSystem.Path.GetEncryptedPath(path, true));
        }

        public override Stream OpenWrite(string path)
        {
            return base.OpenWrite(FileSystem.Path.GetEncryptedPath(path, true));
        }

        public override byte[] ReadAllBytes(string path)
        {
            return base.ReadAllBytes(FileSystem.Path.GetEncryptedPath(path, true));
        }

        public override string[] ReadAllLines(string path)
        {
            return base.ReadAllLines(FileSystem.Path.GetEncryptedPath(path, true));
        }

        public override string[] ReadAllLines(string path, Encoding encoding)
        {
            return base.ReadAllLines(FileSystem.Path.GetEncryptedPath(path, true), encoding);
        }

        public override string ReadAllText(string path)
        {
            return base.ReadAllText(FileSystem.Path.GetEncryptedPath(path, true));
        }

        public override string ReadAllText(string path, Encoding encoding)
        {
            return base.ReadAllText(FileSystem.Path.GetEncryptedPath(path, true), encoding);
        }

        public override IEnumerable<string> ReadLines(string path)
        {
            return base.ReadLines(FileSystem.Path.GetEncryptedPath(path, true));
        }

        public override IEnumerable<string> ReadLines(string path, Encoding encoding)
        {
            return base.ReadLines(FileSystem.Path.GetEncryptedPath(path, true), encoding);
        }

        public override void Replace(string sourceFileName, string destinationFileName, string destinationBackupFileName)
        {
            base.Replace(FileSystem.Path.GetEncryptedPath(sourceFileName, true), FileSystem.Path.GetEncryptedPath(destinationFileName, false), FileSystem.Path.GetEncryptedPath(destinationFileName, false));
        }

        public override void Replace(string sourceFileName, string destinationFileName, string destinationBackupFileName, bool ignoreMetadataErrors)
        {
            base.Replace(FileSystem.Path.GetEncryptedPath(sourceFileName, true), FileSystem.Path.GetEncryptedPath(destinationFileName, false), FileSystem.Path.GetEncryptedPath(destinationBackupFileName, false), ignoreMetadataErrors);
        }

        public override void SetAccessControl(string path, FileSecurity fileSecurity)
        {
            base.SetAccessControl(FileSystem.Path.GetEncryptedPath(path, true), fileSecurity);
        }

        public override void SetAttributes(string path, FileAttributes fileAttributes)
        {
            base.SetAttributes(FileSystem.Path.GetEncryptedPath(path, true), fileAttributes);
        }

        public override void SetCreationTime(string path, DateTime creationTime)
        {
            base.SetCreationTime(FileSystem.Path.GetEncryptedPath(path, true), creationTime);
        }

        public override void SetCreationTimeUtc(string path, DateTime creationTimeUtc)
        {
            base.SetCreationTimeUtc(FileSystem.Path.GetEncryptedPath(path, true), creationTimeUtc);
        }

        public override void SetLastAccessTime(string path, DateTime lastAccessTime)
        {
            base.SetLastAccessTime(FileSystem.Path.GetEncryptedPath(path, true), lastAccessTime);
        }

        public override void SetLastAccessTimeUtc(string path, DateTime lastAccessTimeUtc)
        {
            base.SetLastAccessTimeUtc(FileSystem.Path.GetEncryptedPath(path, true), lastAccessTimeUtc);
        }

        public override void SetLastWriteTime(string path, DateTime lastWriteTime)
        {
            base.SetLastWriteTime(FileSystem.Path.GetEncryptedPath(path, true), lastWriteTime);
        }

        public override void SetLastWriteTimeUtc(string path, DateTime lastWriteTimeUtc)
        {
            base.SetLastWriteTimeUtc(FileSystem.Path.GetEncryptedPath(path, true), lastWriteTimeUtc);
        }

        public override void WriteAllBytes(string path, byte[] bytes)
        {
            base.WriteAllBytes(FileSystem.Path.GetEncryptedPath(path, true), bytes);
        }

        public override void WriteAllLines(string path, IEnumerable<string> contents)
        {
            base.WriteAllLines(FileSystem.Path.GetEncryptedPath(path, true), contents);
        }

        public override void WriteAllLines(string path, IEnumerable<string> contents, Encoding encoding)
        {
            base.WriteAllLines(FileSystem.Path.GetEncryptedPath(path, true), contents, encoding);
        }

        public override void WriteAllLines(string path, string[] contents)
        {
            base.WriteAllLines(FileSystem.Path.GetEncryptedPath(path, true), contents);
        }

        public override void WriteAllLines(string path, string[] contents, Encoding encoding)
        {
            base.WriteAllLines(FileSystem.Path.GetEncryptedPath(path, true), contents, encoding);
        }

        public override void WriteAllText(string path, string contents)
        {
            base.WriteAllText(FileSystem.Path.GetEncryptedPath(path, true), contents);
        }

        public override void WriteAllText(string path, string contents, Encoding encoding)
        {
            base.WriteAllText(FileSystem.Path.GetEncryptedPath(path, true), contents, encoding);
        }

        public override async Task AppendAllLinesAsync(string path, IEnumerable<string> contents, CancellationToken cancellationToken = new())
        {
            await base.AppendAllLinesAsync(FileSystem.Path.GetEncryptedPath(path, true), contents, cancellationToken);
        }

        public override async Task AppendAllLinesAsync(string path, IEnumerable<string> contents, Encoding encoding, CancellationToken cancellationToken = new())
        {
            await base.AppendAllLinesAsync(FileSystem.Path.GetEncryptedPath(path, true), contents, encoding, cancellationToken);
        }

        public override async Task AppendAllTextAsync(string path, string contents, CancellationToken cancellationToken = new())
        {
            await base.AppendAllTextAsync(FileSystem.Path.GetEncryptedPath(path, true), contents, cancellationToken);
        }

        public override async Task AppendAllTextAsync(string path, string contents, Encoding encoding, CancellationToken cancellationToken = new())
        {
            await base.AppendAllTextAsync(FileSystem.Path.GetEncryptedPath(path, true), contents, encoding, cancellationToken);
        }

        public override async Task<byte[]> ReadAllBytesAsync(string path, CancellationToken cancellationToken = new())
        {
            return await base.ReadAllBytesAsync(FileSystem.Path.GetEncryptedPath(path, true), cancellationToken);
        }

        public override async Task<string[]> ReadAllLinesAsync(string path, CancellationToken cancellationToken = new())
        {
            return await base.ReadAllLinesAsync(FileSystem.Path.GetEncryptedPath(path, true), cancellationToken);
        }

        public override async Task<string[]> ReadAllLinesAsync(string path, Encoding encoding, CancellationToken cancellationToken = new())
        {
            return await base.ReadAllLinesAsync(FileSystem.Path.GetEncryptedPath(path, true), encoding, cancellationToken);
        }

        public override async Task<string> ReadAllTextAsync(string path, CancellationToken cancellationToken = new())
        {
            return await base.ReadAllTextAsync(FileSystem.Path.GetEncryptedPath(path, true), cancellationToken);
        }

        public override async Task<string> ReadAllTextAsync(string path, Encoding encoding, CancellationToken cancellationToken = new())
        {
            return await base.ReadAllTextAsync(FileSystem.Path.GetEncryptedPath(path, true), encoding, cancellationToken);
        }

        public override async Task WriteAllLinesAsync(string path, IEnumerable<string> contents, CancellationToken cancellationToken = new())
        {
            await base.WriteAllLinesAsync(FileSystem.Path.GetEncryptedPath(path, true), contents, cancellationToken);
        }

        public override async Task WriteAllLinesAsync(string path, IEnumerable<string> contents, Encoding encoding, CancellationToken cancellationToken = new())
        {
            await base.WriteAllLinesAsync(FileSystem.Path.GetEncryptedPath(path, true), contents, encoding, cancellationToken);
        }

        public override async Task WriteAllLinesAsync(string path, string[] contents, CancellationToken cancellationToken = new())
        {
            await base.WriteAllLinesAsync(FileSystem.Path.GetEncryptedPath(path, true), contents, cancellationToken);
        }

        public override async Task WriteAllLinesAsync(string path, string[] contents, Encoding encoding, CancellationToken cancellationToken = new())
        {
            await base.WriteAllLinesAsync(FileSystem.Path.GetEncryptedPath(path, true), contents, encoding, cancellationToken);
        }

        public override async Task WriteAllTextAsync(string path, string contents, CancellationToken cancellationToken = new())
        {
            await base.WriteAllTextAsync(FileSystem.Path.GetEncryptedPath(path, true), contents, cancellationToken);
        }

        public override async Task WriteAllTextAsync(string path, string contents, Encoding encoding, CancellationToken cancellationToken = new())
        {
            await base.WriteAllTextAsync(FileSystem.Path.GetEncryptedPath(path, true), contents, encoding, cancellationToken);
        }

        public override async Task WriteAllBytesAsync(string path, byte[] bytes, CancellationToken cancellationToken = new())
        {
            await base.WriteAllBytesAsync(FileSystem.Path.GetEncryptedPath(path, true), bytes, cancellationToken);
        }

        public override void AppendAllLines(string path, IEnumerable<string> contents)
        {
            base.AppendAllLines(FileSystem.Path.GetEncryptedPath(path, true), contents);
        }

        public override void AppendAllLines(string path, IEnumerable<string> contents, Encoding encoding)
        {
            base.AppendAllLines(FileSystem.Path.GetEncryptedPath(path, true), contents, encoding);
        }

        public override void AppendAllText(string path, string contents)
        {
            base.AppendAllText(FileSystem.Path.GetEncryptedPath(path, true), contents);
        }

        public override void AppendAllText(string path, string contents, Encoding encoding)
        {
            base.AppendAllText(FileSystem.Path.GetEncryptedPath(path, true), contents, encoding);
        }

        public override StreamWriter AppendText(string path)
        {
            return base.AppendText(FileSystem.Path.GetEncryptedPath(path, true));
        }

        public override void Copy(string sourceFileName, string destFileName)
        {
            base.Copy(FileSystem.Path.GetEncryptedPath(sourceFileName, true), FileSystem.Path.GetEncryptedPath(destFileName, false));
        }

        public override void Copy(string sourceFileName, string destFileName, bool overwrite)
        {
            base.Copy(FileSystem.Path.GetEncryptedPath(sourceFileName, true), FileSystem.Path.GetEncryptedPath(destFileName, true), overwrite);
        }

        public override Stream Create(string path)
        {
            return base.Create(FileSystem.Path.GetEncryptedPath(path, true));
        }

        public override Stream Create(string path, int bufferSize)
        {
            return base.Create(FileSystem.Path.GetEncryptedPath(path, true), bufferSize);
        }

        public override Stream Create(string path, int bufferSize, FileOptions options)
        {
            return base.Create(FileSystem.Path.GetEncryptedPath(path, true), bufferSize, options);
        }

        public override StreamWriter CreateText(string path)
        {
            return base.CreateText(FileSystem.Path.GetEncryptedPath(path, true));
        }

        public override void Decrypt(string path)
        {
            base.Decrypt(FileSystem.Path.GetEncryptedPath(path, true));
        }

        public override void Delete(string path)
        {
            base.Delete(FileSystem.Path.GetEncryptedPath(path, true));
        }

        public override void CreateSpecialFile(string path, FilePermissions mode, ulong rdev)
        {
            var encPath = FileSystem.Path.GetEncryptedPath(path, false);
            base.CreateSpecialFile(encPath, mode, rdev);
        }
    }
}