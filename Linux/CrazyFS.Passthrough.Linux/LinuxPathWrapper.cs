using System;
using System.IO.Abstractions;
using CrazyFS.FileSystem;

namespace CrazyFS.Linux
{
    public class LinuxPathWrapper : IPath
    {
        private readonly IFileSystem _fileSystem;
        private readonly string _source;
        private readonly string _destination;
        private readonly IPath _path;
        
        public LinuxPathWrapper(IFileSystem fileSystem, string source, string destination)
        {
            _fileSystem = fileSystem;
            _source = source;
            _destination = destination;
            _path = new PathWrapper(fileSystem);
        }
        
        public string ChangeExtension(string path, string extension)
        {
            return _path.ChangeExtension(path, extension);
        }

        public string Combine(params string[] paths)
        {
            return _path.Combine(paths);
        }

        public string Combine(string path1, string path2)
        {
            return _path.Combine(path1, path2);
        }

        public string Combine(string path1, string path2, string path3)
        {
            return _path.Combine(path1, path2, path3);
        }

        public string Combine(string path1, string path2, string path3, string path4)
        {
            return _path.Combine(path1, path2, path3, path4);
        }

        public string GetDirectoryName(string path)
        {
            return _path.GetDirectoryName(path);
        }

        public string GetExtension(string path)
        {
            return _path.GetExtension(path);
        }

        public string GetFileName(string path)
        {
            return _path.GetFileName(path);
        }

        public string GetFileNameWithoutExtension(string path)
        {
            return _path.GetFileNameWithoutExtension(path);
        }

        public string GetFullPath(string path)
        {
            return _path.GetFullPath(path.GetPath(_destination));
        }

        public string GetFullPath(string path, string basePath)
        {
            return _path.GetFullPath(path.GetPath(_destination), basePath);
        }

        public char[] GetInvalidFileNameChars()
        {
            return _path.GetInvalidFileNameChars();
        }

        public char[] GetInvalidPathChars()
        {
            return _path.GetInvalidPathChars();
        }

        public string GetPathRoot(string path)
        {
            return _path.GetPathRoot(path);
        }

        public string GetRandomFileName()
        {
            return _path.GetRandomFileName();
        }

        public string GetTempFileName()
        {
            return _path.GetTempFileName();
        }

        public string GetTempPath()
        {
            return _path.GetTempPath();
        }

        public bool HasExtension(string path)
        {
            return _path.HasExtension(path);
        }

        public bool IsPathRooted(string path)
        {
            return _path.IsPathRooted(path);
        }

        public bool IsPathFullyQualified(string path)
        {
            return _path.IsPathFullyQualified(path);
        }

        public string GetRelativePath(string relativeTo, string path)
        {
            return _path.GetRelativePath(relativeTo, path);
        }

        public string Join(ReadOnlySpan<char> path1, ReadOnlySpan<char> path2)
        {
            return _path.Join(path1, path2);
        }

        public string Join(ReadOnlySpan<char> path1, ReadOnlySpan<char> path2, ReadOnlySpan<char> path3)
        {
            return _path.Join(path1, path2, path3);
        }

        public bool TryJoin(ReadOnlySpan<char> path1, ReadOnlySpan<char> path2, ReadOnlySpan<char> path3, Span<char> destination, out int charsWritten)
        {
            return _path.TryJoin(path1, path2, path3, destination, out charsWritten);
        }

        public bool TryJoin(ReadOnlySpan<char> path1, ReadOnlySpan<char> path2, Span<char> destination, out int charsWritten)
        {
            return _path.TryJoin(path1, path2, destination, out charsWritten);
        }

        public char AltDirectorySeparatorChar => _path.AltDirectorySeparatorChar;
        public char DirectorySeparatorChar => _path.DirectorySeparatorChar;
        public IFileSystem FileSystem => _path.FileSystem;
        public char PathSeparator => _path.PathSeparator;
        public char VolumeSeparatorChar => _path.VolumeSeparatorChar;
        public char[] InvalidPathChars => _path.InvalidPathChars;
    }
}