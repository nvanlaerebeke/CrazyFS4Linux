using System;
using System.Collections;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using CrazyFS.Encryption;
using CrazyFS.FileSystem.Encrypted.Linux.Extensions;
using CrazyFS.FileSystem.Encrypted.Linux.Interfaces;
using CrazyFS.Passthrough.Linux.Extensions;
using CrazyFS.Passthrough.Linux.Helpers;
using CrazyFS.Storage.Passthrough.Linux;
using Mono.Unix;
using Mono.Unix.Native;

namespace CrazyFS.FileSystem.Encrypted.Linux
{
    public class LinuxEncPathWrapper : LinuxPathWrapper, ILinuxEncPathWrapper
    {
        private readonly IEncryption _encryption;

        public LinuxEncPathWrapper(IFileSystem fileSystem, string source, string destination, IEncryption encryption) : base(fileSystem, source, destination)
        {
            _encryption = encryption;
        }

        /// <summary>
        ///     ToDo: benchmark with Linq ForEach
        /// </summary>
        /// <param name="path"></param>
        /// <param name="existing"></param>
        /// <returns>Fully encrypted path or empty string if the path cannot be found</returns>
        public string GetEncryptedPath(string path, bool existing)
        {
            var parts = path.Split(Path.DirectorySeparatorChar, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length.Equals(0)) return path;

            var found = true;
            var tmpPath = _source;
            for (byte i = 0; i < parts.Length; i++)
            {
                if (found)
                {
                    var currentListEncrypted = Directory.GetFileSystemEntries(tmpPath).Select(x => x.GetRelative(tmpPath).Trim('/')).ToArray(); // FileSystem.DirectoryInfo.FromDirectoryName(tmpPath).GetFileSystemInfos();
                    var currentListDecrypted = currentListEncrypted.ToList().ConvertAll(x => GetDecryptedPath(x)).ToArray();
                    var entry = currentListDecrypted.FirstOrDefault(x => x.Equals(parts[i]));
                    if (entry == null && existing) return "";

                    if (entry != null)
                    {
                        var part = currentListEncrypted[currentListDecrypted.ToList().IndexOf(entry)];
                        tmpPath = Path.Combine(tmpPath, part);
                        parts[i] = part;
                        continue;
                    }
                }
                parts[i] = _encryption.EncryptString(parts[i]);
            }
            return string.Join(Path.DirectorySeparatorChar, parts).GetRelative(_source);
        }

        public string GetDecryptedPath(string path)
        {
            var parts = path.Split(Path.DirectorySeparatorChar, StringSplitOptions.RemoveEmptyEntries);
            for (byte i = 0; i < parts.Length; i++)
            {
                parts[i] = _encryption.DecryptString(parts[i]);
            }
            return string.Join(Path.DirectorySeparatorChar, parts);
        }

        public override bool HasAccess(string path, AccessModes modes)
        {
            var path_enc = GetEncryptedPath(path, true);
            if (string.IsNullOrEmpty(path_enc)) return false;

            if (FileSystem.File.Exists(path))
            {
                return PermissionHelper.CheckPathAccessModes(
                    new UnixFileInfo(path_enc.GetPath(_source)).FileAccessPermissions,
                    modes
                );
            }
            return PermissionHelper.CheckPathAccessModes(
                new UnixDirectoryInfo(path_enc).FileAccessPermissions, 
                modes
            );
        }

        public override void GetExtendedAttribute(string path, string name, byte[] value, out int bytesWritten)
        {
            var encPath = FileSystem.Path.GetEncryptedPath(path, true);
            bytesWritten = (int) Syscall.lgetxattr(encPath.GetPath(_source), name, value, (ulong) (value?.Length ?? 0));
            if (bytesWritten != -1) return;

            var err = Stdlib.GetLastError();
            if (err != Errno.ENODATA) throw new NativeException((int) err);
        }
    }
}