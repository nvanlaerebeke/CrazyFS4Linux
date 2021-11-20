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
        public string GetEncryptedPath(string path)
        {
            var parts = path.Split(Path.DirectorySeparatorChar, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length.Equals(0)) return path;
            
            var tmpPath = "";
            for (byte i = 0; i < parts.Length; i++)
            {
                var currentList = FileSystem.DirectoryInfo.FromDirectoryName(tmpPath).GetFileSystemInfos();
                var entry = currentList.FirstOrDefault(x => x.Name.Equals(parts[i]));
                if (entry == null) return "";
                tmpPath = Path.Combine(tmpPath, entry.Name);
                parts[i] = entry.GetEncryptedName();
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
            var path_enc = GetEncryptedPath(path);
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