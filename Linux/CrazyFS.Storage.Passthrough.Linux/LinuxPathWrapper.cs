using System;
using System.IO;
using System.IO.Abstractions;
using CrazyFS.FileSystem;
using CrazyFS.Passthrough.Linux.Extensions;
using CrazyFS.Passthrough.Linux.Helpers;
using CrazyFS.Passthrough.Linux.Interfaces;
using Mono.Unix;
using Mono.Unix.Native;

namespace CrazyFS.Storage.Passthrough.Linux
{
    public class LinuxPathWrapper : ILinuxPath
    {
        private readonly string _destination;
        private readonly IPath _path;
        private readonly string _source;

        public LinuxPathWrapper(IFileSystem fileSystem, string source, string destination)
        {
            _source = source;
            _destination = destination;
            _path = new PathWrapper(fileSystem);
        }

        public string ChangeExtension(string path, string extension)
        {
            return _path.ChangeExtension(path, extension);
        }

        public void Chmod(string path, FilePermissions permissions)
        {
            if (Syscall.chmod(path.GetPath(_source), permissions) == -1)
                throw new NativeException((int) Stdlib.GetLastError());
        }

        public void Chown(string path, uint uid, uint gid)
        {
            if (Syscall.lchown(path.GetPath(_source), uid, gid) == -1)
                throw new NativeException((int) Stdlib.GetLastError());
        }

        public void CreateHardLink(string from, string to)
        {
            if (Syscall.link(from.GetPath(_source), to.GetPath(_source)) == -1)
                throw new NativeException((int) Stdlib.GetLastError());
        }

        public void CreateSymLink(string from, string to)
        {
            if (Syscall.symlink(from, to.GetPath(_source)) == -1)
            {
                throw new NativeException((int) Stdlib.GetLastError());
            }
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

        public void GetExtendedAttribute(string path, string name, byte[] value, out int bytesWritten)
        {
            bytesWritten = (int) Syscall.lgetxattr(path.GetPath(_source), name, value, (ulong) (value?.Length ?? 0));
            if (bytesWritten != -1) return;

            var err = Stdlib.GetLastError();
            if (err != Errno.ENODATA) throw new NativeException((int) err);
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

        public bool HasAccess(string path, AccessModes modes)
        {
            if (FileSystem.File.Exists(path))
                return PermissionHelper.CheckPathAccessModes(
                    new UnixFileInfo(FileSystem.FileInfo.FromFileName(path).FullName).FileAccessPermissions, modes);

            if (!FileSystem.Directory.Exists(path)) throw new FileNotFoundException();

            return PermissionHelper.CheckPathAccessModes(
                new UnixDirectoryInfo(
                    FileSystem.DirectoryInfo.FromDirectoryName(path).FullName
                ).FileAccessPermissions,
                modes
            );
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

        public string[] ListExtendedAttributes(string path)
        {
            if ((int) Syscall.llistxattr(path.GetPath(_source), out var names) != -1) return names;
            throw new NativeException((int) Stdlib.GetLastError());
        }

        public void RemoveExtendedAttributes(string path, string name)
        {
            if (Syscall.lremovexattr(path.GetPath(_source), name) == -1)
                throw new NativeException((int) Stdlib.GetLastError());
        }

        public void SetExtendedAttributes(string path, string name, byte[] value, XattrFlags flags)
        {
            if (Syscall.lsetxattr(path.GetPath(_source), name, value, (ulong) value.Length, flags) == -1)
                throw new NativeException((int) Stdlib.GetLastError());
        }

        public bool TryJoin(ReadOnlySpan<char> path1, ReadOnlySpan<char> path2, ReadOnlySpan<char> path3,
            Span<char> destination, out int charsWritten)
        {
            return _path.TryJoin(path1, path2, path3, destination, out charsWritten);
        }

        public bool TryJoin(ReadOnlySpan<char> path1, ReadOnlySpan<char> path2, Span<char> destination,
            out int charsWritten)
        {
            return _path.TryJoin(path1, path2, destination, out charsWritten);
        }

        public char AltDirectorySeparatorChar => _path.AltDirectorySeparatorChar;
        public char DirectorySeparatorChar => _path.DirectorySeparatorChar;
        public IFileSystem FileSystem => _path.FileSystem;
        public char PathSeparator => _path.PathSeparator;
        public char VolumeSeparatorChar => _path.VolumeSeparatorChar;
        public char[] InvalidPathChars => _path.InvalidPathChars;

        public string GetSymlinkTarget(string path)
        {
            return new UnixFileInfo(path.GetPath(_source)).IsSymbolicLink ? GetFullPath(path) : path;
        }

        public void CreateSymlink(string from, string to)
        {
            if (Syscall.symlink(from, to.GetPath(_source)) != 1) return;
            throw new NativeException((int) Stdlib.GetLastError());
        }
    }
}