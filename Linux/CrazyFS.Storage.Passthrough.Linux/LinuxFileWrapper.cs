using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Security.AccessControl;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CrazyFS.FileSystem;
using CrazyFS.Passthrough.Linux.Extensions;
using CrazyFS.Passthrough.Linux.Interfaces;
using Mono.Unix.Native;

namespace CrazyFS.Storage.Passthrough.Linux
{
    public class LinuxFileWrapper : ILinuxFile
    {
        protected readonly string _source;
        private readonly IFile _file;
        public LinuxFileWrapper(IFileSystem fileSystem, string source)
        {
            FileSystem = fileSystem;
            _source = source;
            _file = new FileWrapper(fileSystem);
        }

        public virtual Task AppendAllLinesAsync(string path, IEnumerable<string> contents, CancellationToken cancellationToken = new())
        {
            return _file.AppendAllLinesAsync(path.GetPath(_source), contents, cancellationToken);
        }

        public virtual Task AppendAllLinesAsync(string path, IEnumerable<string> contents, Encoding encoding, CancellationToken cancellationToken = new())
        {
            return _file.AppendAllLinesAsync(path.GetPath(_source), contents, encoding, cancellationToken);
        }

        public virtual Task AppendAllTextAsync(string path, string contents, CancellationToken cancellationToken = new())
        {
            return _file.AppendAllTextAsync(path.GetPath(_source), contents, cancellationToken);
        }

        public virtual Task AppendAllTextAsync(string path, string contents, Encoding encoding, CancellationToken cancellationToken = new())
        {
            return _file.AppendAllTextAsync(path.GetPath(_source), contents, encoding, cancellationToken);
        }

        public virtual Task<byte[]> ReadAllBytesAsync(string path, CancellationToken cancellationToken = new())
        {
            return _file.ReadAllBytesAsync(path.GetPath(_source), cancellationToken);
        }

        public virtual Task<string[]> ReadAllLinesAsync(string path, CancellationToken cancellationToken = new())
        {
            return _file.ReadAllLinesAsync(path.GetPath(_source), cancellationToken);
        }

        public virtual Task<string[]> ReadAllLinesAsync(string path, Encoding encoding, CancellationToken cancellationToken = new())
        {
            return _file.ReadAllLinesAsync(path.GetPath(_source), encoding, cancellationToken);
        }

        public virtual Task<string> ReadAllTextAsync(string path, CancellationToken cancellationToken = new())
        {
            return _file.ReadAllTextAsync(path.GetPath(_source), cancellationToken);
        }

        public virtual Task<string> ReadAllTextAsync(string path, Encoding encoding, CancellationToken cancellationToken = new())
        {
            return _file.ReadAllTextAsync(path.GetPath(_source), encoding, cancellationToken);
        }

        public virtual Task WriteAllLinesAsync(string path, IEnumerable<string> contents, CancellationToken cancellationToken = new())
        {
            return _file.WriteAllLinesAsync(path.GetPath(_source), contents, cancellationToken);
        }

        public virtual Task WriteAllLinesAsync(string path, IEnumerable<string> contents, Encoding encoding, CancellationToken cancellationToken = new())
        {
            return _file.WriteAllLinesAsync(path.GetPath(_source), contents, encoding, cancellationToken);
        }

        public virtual Task WriteAllLinesAsync(string path, string[] contents, CancellationToken cancellationToken = new())
        {
            return _file.WriteAllLinesAsync(path.GetPath(_source), contents, cancellationToken);
        }

        public virtual Task WriteAllLinesAsync(string path, string[] contents, Encoding encoding, CancellationToken cancellationToken = new())
        {
            return _file.WriteAllLinesAsync(path.GetPath(_source), contents, encoding, cancellationToken);
        }

        public virtual Task WriteAllTextAsync(string path, string contents, CancellationToken cancellationToken = new())
        {
            return _file.WriteAllTextAsync(path.GetPath(_source), contents, cancellationToken);
        }

        public virtual Task WriteAllTextAsync(string path, string contents, Encoding encoding, CancellationToken cancellationToken = new())
        {
            return _file.WriteAllTextAsync(path.GetPath(_source), contents, encoding, cancellationToken);
        }

        public virtual Task WriteAllBytesAsync(string path, byte[] bytes, CancellationToken cancellationToken = new())
        {
            return _file.WriteAllBytesAsync(path.GetPath(_source), bytes, cancellationToken);
        }

        public virtual void AppendAllLines(string path, IEnumerable<string> contents)
        {
            _file.AppendAllLines(path.GetPath(_source), contents);
        }

        public virtual void AppendAllLines(string path, IEnumerable<string> contents, Encoding encoding)
        {
            _file.AppendAllLines(path.GetPath(_source), contents, encoding);
        }

        public virtual void AppendAllText(string path, string contents)
        {
            _file.AppendAllText(path.GetPath(_source), contents);
        }

        public virtual void AppendAllText(string path, string contents, Encoding encoding)
        {
            _file.AppendAllText(path.GetPath(_source), contents, encoding);
        }

        public virtual StreamWriter AppendText(string path)
        {
            return _file.AppendText(path.GetPath(_source));
        }

        public virtual void Copy(string sourceFileName, string destFileName)
        {
            _file.Copy(sourceFileName.GetPath(_source), destFileName.GetPath(_source));
        }

        public virtual void Copy(string sourceFileName, string destFileName, bool overwrite)
        {
            _file.Copy(sourceFileName.GetPath(_source), destFileName.GetPath(_source), overwrite);
        }

        public virtual Stream Create(string path)
        {
            return _file.Create(path.GetPath(_source));
        }

        public virtual Stream Create(string path, int bufferSize)
        {
            return _file.Create(path.GetPath(_source), bufferSize);
        }

        public virtual Stream Create(string path, int bufferSize, FileOptions options)
        {
            return _file.Create(path.GetPath(_source), bufferSize, options);
        }
        
        public virtual void CreateSpecialFile(string path, FilePermissions mode, ulong rdev)
        {
            if (Syscall.mknod(path.GetPath(_source), mode, rdev) != -1) return;
            throw new NativeException((int)Stdlib.GetLastError());
        }
        
        public virtual StreamWriter CreateText(string path)
        {
            return _file.CreateText(path.GetPath(_source));
        }

        public virtual void Decrypt(string path)
        {
            _file.Decrypt(path.GetPath(_source));
        }

        public virtual void Delete(string path)
        {
            _file.Delete(path.GetPath(_source));
        }

        public virtual void Encrypt(string path)
        {
            _file.Encrypt(path.GetPath(_source));
        }

        public virtual bool Exists(string path)
        {
            return _file.Exists(path.GetPath(_source));
        }

        public virtual FileSecurity GetAccessControl(string path)
        {
            throw new PlatformNotSupportedException();
        }

        public virtual FileSecurity GetAccessControl(string path, AccessControlSections includeSections)
        {
            throw new PlatformNotSupportedException();
        }

        public virtual FileAttributes GetAttributes(string path)
        {
            return _file.GetAttributes(path.GetPath(_source));
        }

        public virtual DateTime GetCreationTime(string path)
        {
            return _file.GetCreationTime(path.GetPath(_source));
        }

        public virtual DateTime GetCreationTimeUtc(string path)
        {
            return _file.GetCreationTimeUtc(path.GetPath(_source));
        }

        public virtual DateTime GetLastAccessTime(string path)
        {
            return _file.GetLastAccessTime(path.GetPath(_source));
        }

        public virtual DateTime GetLastAccessTimeUtc(string path)
        {
            return _file.GetLastAccessTimeUtc(path.GetPath(_source));
        }

        public virtual DateTime GetLastWriteTime(string path)
        {
            return _file.GetLastWriteTime(path.GetPath(_source));
        }

        public virtual DateTime GetLastWriteTimeUtc(string path)
        {
            return _file.GetLastWriteTimeUtc(path.GetPath(_source));
        }

        public virtual void Move(string sourceFileName, string destFileName)
        {
            _file.Move(sourceFileName.GetPath(_source), destFileName.GetPath(_source));
        }

        public virtual void Move(string sourceFileName, string destFileName, bool overwrite)
        {
             _file.Move(sourceFileName.GetPath(_source), destFileName.GetPath(_source), overwrite);
        }

        public virtual Stream Open(string path, FileMode mode)
        {
            return _file.Open(path.GetPath(_source), mode);
        }

        public virtual Stream Open(string path, FileMode mode, FileAccess access)
        {
            return _file.Open(path.GetPath(_source), mode, access);
        }

        public virtual Stream Open(string path, FileMode mode, FileAccess access, FileShare share)
        {
            return _file.Open(path.GetPath(_source), mode, access, share);
        }

        public virtual Stream OpenRead(string path)
        {
            return _file.OpenRead(path.GetPath(_source));
        }

        public virtual StreamReader OpenText(string path)
        {
            return _file.OpenText(path.GetPath(_source));
        }

        public virtual Stream OpenWrite(string path)
        {
            return _file.OpenWrite(path.GetPath(_source));
        }

        public virtual byte[] ReadAllBytes(string path)
        {
            return _file.ReadAllBytes(path.GetPath(_source));
        }

        public virtual string[] ReadAllLines(string path)
        {
            return _file.ReadAllLines(path.GetPath(_source));
        }

        public virtual string[] ReadAllLines(string path, Encoding encoding)
        {
            return _file.ReadAllLines(path.GetPath(_source), encoding);
        }

        public virtual string ReadAllText(string path)
        {
            return _file.ReadAllText(path.GetPath(_source));
        }

        public virtual string ReadAllText(string path, Encoding encoding)
        {
            return _file.ReadAllText(path.GetPath(_source), encoding);
        }

        public virtual IEnumerable<string> ReadLines(string path)
        {
            return _file.ReadLines(path.GetPath(_source));
        }

        public virtual IEnumerable<string> ReadLines(string path, Encoding encoding)
        {
            return _file.ReadLines(path.GetPath(_source), encoding);
        }

        public virtual void Replace(string sourceFileName, string destinationFileName, string destinationBackupFileName)
        {
            _file.Replace(sourceFileName.GetPath(_source), destinationFileName.GetPath(_source), destinationBackupFileName);
        }

        public virtual void Replace(string sourceFileName, string destinationFileName, string destinationBackupFileName, bool ignoreMetadataErrors)
        {
            _file.Replace(sourceFileName.GetPath(_source), destinationFileName.GetPath(_source), destinationBackupFileName.GetPath(_source), ignoreMetadataErrors);
        }

        public virtual void SetAccessControl(string path, FileSecurity fileSecurity)
        {
            throw new PlatformNotSupportedException();
        }

        public virtual void SetAttributes(string path, FileAttributes fileAttributes)
        {
            _file.SetAttributes(path.GetPath(_source), fileAttributes);
        }

        public virtual void SetCreationTime(string path, DateTime creationTime)
        {
            _file.SetCreationTime(path.GetPath(_source), creationTime);
        }

        public virtual void SetCreationTimeUtc(string path, DateTime creationTimeUtc)
        {
            _file.SetCreationTimeUtc(path.GetPath(_source), creationTimeUtc);
        }

        public virtual void SetLastAccessTime(string path, DateTime lastAccessTime)
        {
            _file.SetLastAccessTime(path.GetPath(_source), lastAccessTime);
        }

        public virtual void SetLastAccessTimeUtc(string path, DateTime lastAccessTimeUtc)
        {
            _file.SetLastAccessTimeUtc(path.GetPath(_source), lastAccessTimeUtc);
        }

        public virtual void SetLastWriteTime(string path, DateTime lastWriteTime)
        {
            _file.SetLastWriteTime(path.GetPath(_source), lastWriteTime);
        }

        public virtual void SetLastWriteTimeUtc(string path, DateTime lastWriteTimeUtc)
        {
            _file.SetLastWriteTimeUtc(path.GetPath(_source), lastWriteTimeUtc);
        }

        public virtual void WriteAllBytes(string path, byte[] bytes)
        {
            _file.WriteAllBytes(path.GetPath(_source), bytes);
        }

        public virtual void WriteAllLines(string path, IEnumerable<string> contents)
        {
            _file.WriteAllLines(path.GetPath(_source), contents);
        }

        public virtual void WriteAllLines(string path, IEnumerable<string> contents, Encoding encoding)
        {
            _file.WriteAllLines(path.GetPath(_source), contents, encoding);
        }

        public virtual void WriteAllLines(string path, string[] contents)
        {
            _file.WriteAllLines(path.GetPath(_source), contents);
        }

        public virtual void WriteAllLines(string path, string[] contents, Encoding encoding)
        {
            _file.WriteAllLines(path.GetPath(_source), contents, encoding);
        }

        public virtual void WriteAllText(string path, string contents)
        {
            _file.WriteAllText(path.GetPath(_source), contents);
        }

        public virtual void WriteAllText(string path, string contents, Encoding encoding)
        {
            _file.WriteAllText(path.GetPath(_source), contents, encoding);
        }

        public IFileSystem FileSystem { get; }
    }
}