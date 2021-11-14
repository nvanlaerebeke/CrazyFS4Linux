using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Security.AccessControl;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CrazyFS.FileSystem;
using Mono.Unix.Native;

namespace CrazyFS.Linux
{
    public class LinuxFileWrapper : IFile
    {
        private readonly IFileSystem _fileSystem;
        private readonly string _source;
        private readonly IFile _file;
        public LinuxFileWrapper(IFileSystem fileSystem, string source, string destination)
        {
            _fileSystem = fileSystem;
            _source = source;
            _file = new FileWrapper(fileSystem);
        }

        public Task AppendAllLinesAsync(string path, IEnumerable<string> contents, CancellationToken cancellationToken = new CancellationToken())
        {
            return _file.AppendAllLinesAsync(path.GetPath(_source), contents, cancellationToken);
        }

        public Task AppendAllLinesAsync(string path, IEnumerable<string> contents, Encoding encoding, CancellationToken cancellationToken = new CancellationToken())
        {
            return _file.AppendAllLinesAsync(path.GetPath(_source), contents, encoding, cancellationToken);
        }

        public Task AppendAllTextAsync(string path, string contents, CancellationToken cancellationToken = new CancellationToken())
        {
            return _file.AppendAllTextAsync(path.GetPath(_source), contents, cancellationToken);
        }

        public Task AppendAllTextAsync(string path, string contents, Encoding encoding, CancellationToken cancellationToken = new CancellationToken())
        {
            return _file.AppendAllTextAsync(path.GetPath(_source), contents, encoding, cancellationToken);
        }

        public Task<byte[]> ReadAllBytesAsync(string path, CancellationToken cancellationToken = new CancellationToken())
        {
            return _file.ReadAllBytesAsync(path.GetPath(_source), cancellationToken);
        }

        public Task<string[]> ReadAllLinesAsync(string path, CancellationToken cancellationToken = new CancellationToken())
        {
            return _file.ReadAllLinesAsync(path.GetPath(_source), cancellationToken);
        }

        public Task<string[]> ReadAllLinesAsync(string path, Encoding encoding, CancellationToken cancellationToken = new CancellationToken())
        {
            return _file.ReadAllLinesAsync(path.GetPath(_source), encoding, cancellationToken);
        }

        public Task<string> ReadAllTextAsync(string path, CancellationToken cancellationToken = new CancellationToken())
        {
            return _file.ReadAllTextAsync(path.GetPath(_source), cancellationToken);
        }

        public Task<string> ReadAllTextAsync(string path, Encoding encoding, CancellationToken cancellationToken = new CancellationToken())
        {
            return _file.ReadAllTextAsync(path.GetPath(_source), encoding, cancellationToken);
        }

        public Task WriteAllLinesAsync(string path, IEnumerable<string> contents, CancellationToken cancellationToken = new CancellationToken())
        {
            return _file.WriteAllLinesAsync(path.GetPath(_source), contents, cancellationToken);
        }

        public Task WriteAllLinesAsync(string path, IEnumerable<string> contents, Encoding encoding, CancellationToken cancellationToken = new CancellationToken())
        {
            return _file.WriteAllLinesAsync(path.GetPath(_source), contents, encoding, cancellationToken);
        }

        public Task WriteAllLinesAsync(string path, string[] contents, CancellationToken cancellationToken = new CancellationToken())
        {
            return _file.WriteAllLinesAsync(path.GetPath(_source), contents, cancellationToken);
        }

        public Task WriteAllLinesAsync(string path, string[] contents, Encoding encoding, CancellationToken cancellationToken = new CancellationToken())
        {
            return _file.WriteAllLinesAsync(path.GetPath(_source), contents, encoding, cancellationToken);
        }

        public Task WriteAllTextAsync(string path, string contents, CancellationToken cancellationToken = new CancellationToken())
        {
            return _file.WriteAllTextAsync(path.GetPath(_source), contents, cancellationToken);
        }

        public Task WriteAllTextAsync(string path, string contents, Encoding encoding, CancellationToken cancellationToken = new CancellationToken())
        {
            return _file.WriteAllTextAsync(path.GetPath(_source), contents, encoding, cancellationToken);
        }

        public Task WriteAllBytesAsync(string path, byte[] bytes, CancellationToken cancellationToken = new CancellationToken())
        {
            return _file.WriteAllBytesAsync(path.GetPath(_source), bytes, cancellationToken);
        }

        public void AppendAllLines(string path, IEnumerable<string> contents)
        {
            _file.AppendAllLines(path.GetPath(_source), contents);
        }

        public void AppendAllLines(string path, IEnumerable<string> contents, Encoding encoding)
        {
            _file.AppendAllLines(path.GetPath(_source), contents, encoding);
        }

        public void AppendAllText(string path, string contents)
        {
            _file.AppendAllText(path.GetPath(_source), contents);
        }

        public void AppendAllText(string path, string contents, Encoding encoding)
        {
            _file.AppendAllText(path.GetPath(_source), contents, encoding);
        }

        public StreamWriter AppendText(string path)
        {
            return _file.AppendText(path.GetPath(_source));
        }

        public void Copy(string sourceFileName, string destFileName)
        {
            _file.Copy(sourceFileName.GetPath(_source), destFileName.GetPath(_source));
        }

        public void Copy(string sourceFileName, string destFileName, bool overwrite)
        {
            _file.Copy(sourceFileName.GetPath(_source), destFileName.GetPath(_source), overwrite);
        }

        public Stream Create(string path)
        {
            return _file.Create(path.GetPath(_source));
        }

        public Stream Create(string path, int bufferSize)
        {
            return _file.Create(path.GetPath(_source), bufferSize);
        }

        public Stream Create(string path, int bufferSize, FileOptions options)
        {
            return _file.Create(path.GetPath(_source), bufferSize, options);
        }
        
        public IFileInfo CreateSpecialFile(string path, FilePermissions mode, ulong rdev)
        {
            if (Syscall.mknod(path.GetPath(_source), mode, rdev) != -1)
            {
                return _fileSystem.FileInfo.FromFileName(path.GetPath(_source));
            }
            throw new Exception();
        }
        public StreamWriter CreateText(string path)
        {
            return _file.CreateText(path.GetPath(_source));
        }

        public void Decrypt(string path)
        {
            _file.Decrypt(path.GetPath(_source));
        }

        public void Delete(string path)
        {
            _file.Delete(path.GetPath(_source));
        }

        public void Encrypt(string path)
        {
            _file.Encrypt(path.GetPath(_source));
        }

        public bool Exists(string path)
        {
            return _file.Exists(path.GetPath(_source));
        }

        public FileSecurity GetAccessControl(string path)
        {
            throw new PlatformNotSupportedException();
        }

        public FileSecurity GetAccessControl(string path, AccessControlSections includeSections)
        {
            throw new PlatformNotSupportedException();
        }

        public FileAttributes GetAttributes(string path)
        {
            return _file.GetAttributes(path.GetPath(_source));
        }

        public DateTime GetCreationTime(string path)
        {
            return _file.GetCreationTime(path.GetPath(_source));
        }

        public DateTime GetCreationTimeUtc(string path)
        {
            return _file.GetCreationTimeUtc(path.GetPath(_source));
        }

        public DateTime GetLastAccessTime(string path)
        {
            return _file.GetLastAccessTime(path.GetPath(_source));
        }

        public DateTime GetLastAccessTimeUtc(string path)
        {
            return _file.GetLastAccessTimeUtc(path.GetPath(_source));
        }

        public DateTime GetLastWriteTime(string path)
        {
            return _file.GetLastWriteTime(path.GetPath(_source));
        }

        public DateTime GetLastWriteTimeUtc(string path)
        {
            return _file.GetLastWriteTimeUtc(path.GetPath(_source));
        }

        public void Move(string sourceFileName, string destFileName)
        {
            _file.Move(sourceFileName.GetPath(_source), destFileName.GetPath(_source));
        }

        public void Move(string sourceFileName, string destFileName, bool overwrite)
        {
             _file.Move(sourceFileName.GetPath(_source), destFileName.GetPath(_source), overwrite);
        }

        public Stream Open(string path, FileMode mode)
        {
            return _file.Open(path.GetPath(_source), mode);
        }

        public Stream Open(string path, FileMode mode, FileAccess access)
        {
            return _file.Open(path.GetPath(_source), mode, access);
        }

        public Stream Open(string path, FileMode mode, FileAccess access, FileShare share)
        {
            return _file.Open(path.GetPath(_source), mode, access, share);
        }

        public Stream OpenRead(string path)
        {
            return _file.OpenRead(path.GetPath(_source));
        }

        public StreamReader OpenText(string path)
        {
            return _file.OpenText(path.GetPath(_source));
        }

        public Stream OpenWrite(string path)
        {
            return _file.OpenWrite(path.GetPath(_source));
        }

        public byte[] ReadAllBytes(string path)
        {
            return _file.ReadAllBytes(path.GetPath(_source));
        }

        public string[] ReadAllLines(string path)
        {
            return _file.ReadAllLines(path.GetPath(_source));
        }

        public string[] ReadAllLines(string path, Encoding encoding)
        {
            return _file.ReadAllLines(path.GetPath(_source), encoding);
        }

        public string ReadAllText(string path)
        {
            return _file.ReadAllText(path.GetPath(_source));
        }

        public string ReadAllText(string path, Encoding encoding)
        {
            return _file.ReadAllText(path.GetPath(_source), encoding);
        }

        public IEnumerable<string> ReadLines(string path)
        {
            return _file.ReadLines(path.GetPath(_source));
        }

        public IEnumerable<string> ReadLines(string path, Encoding encoding)
        {
            return _file.ReadLines(path.GetPath(_source), encoding);
        }

        public void Replace(string sourceFileName, string destinationFileName, string destinationBackupFileName)
        {
            _file.Replace(sourceFileName.GetPath(_source), destinationFileName.GetPath(_source), destinationBackupFileName);
        }

        public void Replace(string sourceFileName, string destinationFileName, string destinationBackupFileName, bool ignoreMetadataErrors)
        {
            _file.Replace(sourceFileName.GetPath(_source), destinationFileName.GetPath(_source), destinationBackupFileName.GetPath(_source), ignoreMetadataErrors);
        }

        public void SetAccessControl(string path, FileSecurity fileSecurity)
        {
            throw new PlatformNotSupportedException();
        }

        public void SetAttributes(string path, FileAttributes fileAttributes)
        {
            _file.SetAttributes(path.GetPath(_source), fileAttributes);
        }

        public void SetCreationTime(string path, DateTime creationTime)
        {
            _file.SetCreationTime(path.GetPath(_source), creationTime);
        }

        public void SetCreationTimeUtc(string path, DateTime creationTimeUtc)
        {
            _file.SetCreationTimeUtc(path.GetPath(_source), creationTimeUtc);
        }

        public void SetLastAccessTime(string path, DateTime lastAccessTime)
        {
            _file.SetLastAccessTime(path.GetPath(_source), lastAccessTime);
        }

        public void SetLastAccessTimeUtc(string path, DateTime lastAccessTimeUtc)
        {
            _file.SetLastAccessTimeUtc(path.GetPath(_source), lastAccessTimeUtc);
        }

        public void SetLastWriteTime(string path, DateTime lastWriteTime)
        {
            _file.SetLastWriteTime(path.GetPath(_source), lastWriteTime);
        }

        public void SetLastWriteTimeUtc(string path, DateTime lastWriteTimeUtc)
        {
            _file.SetLastWriteTimeUtc(path.GetPath(_source), lastWriteTimeUtc);
        }

        public void WriteAllBytes(string path, byte[] bytes)
        {
            _file.WriteAllBytes(path.GetPath(_source), bytes);
        }

        public void WriteAllLines(string path, IEnumerable<string> contents)
        {
            _file.WriteAllLines(path.GetPath(_source), contents);
        }

        public void WriteAllLines(string path, IEnumerable<string> contents, Encoding encoding)
        {
            _file.WriteAllLines(path.GetPath(_source), contents, encoding);
        }

        public void WriteAllLines(string path, string[] contents)
        {
            _file.WriteAllLines(path.GetPath(_source), contents);
        }

        public void WriteAllLines(string path, string[] contents, Encoding encoding)
        {
            _file.WriteAllLines(path.GetPath(_source), contents, encoding);
        }

        public void WriteAllText(string path, string contents)
        {
            _file.WriteAllText(path.GetPath(_source), contents);
        }

        public void WriteAllText(string path, string contents, Encoding encoding)
        {
            _file.WriteAllText(path.GetPath(_source), contents, encoding);
        }

        public IFileSystem FileSystem => _fileSystem;
    }
}