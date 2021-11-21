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
        /// <returns>Fully encrypted path or empty string if the path cannot be found</returns>
        public string GetEncryptedPath(string path, bool existing)
        {
            var parts = path.Split(Path.DirectorySeparatorChar, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length.Equals(0)) return path;

            var found = true;
            var tmpPath = "";
            for (byte i = 0; i < parts.Length; i++)
            {
                if (found)
                {
                    var currentListEncrypted = Directory.GetFileSystemEntries(tmpPath.GetPath(_source)); // FileSystem.DirectoryInfo.FromDirectoryName(tmpPath).GetFileSystemInfos();
                    var currentListDecrypted = currentListEncrypted.ToList().ConvertAll(x => GetDecryptedPath(x)).ToArray();
                    var entry = currentListDecrypted.FirstOrDefault(x => x.Equals(parts[i]));
                    if (entry == null && existing) return "";

                    if (entry != null)
                    {
                        tmpPath = Path.Combine(tmpPath, entry);
                        parts[i] = currentListEncrypted[currentListDecrypted.ToList().IndexOf(entry)];
                    }
                    else
                    {
                        parts[i] = _encryption.EncryptString(parts[i]);
                    }
                }
                else
                {
                    parts[i] = _encryption.EncryptString(parts[i]);
                }
            }
            return string.Join(Path.DirectorySeparatorChar, parts);
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
    }
}