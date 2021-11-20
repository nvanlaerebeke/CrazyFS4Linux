using System;
using System.IO.Abstractions;
using CrazyFS.FileSystem.Encrypted.Linux.Interfaces;
using CrazyFS.Passthrough.Linux.Interfaces;
using Mono.Unix.Native;

namespace CrazyFS.FileSystem.Encrypted.Linux.Extensions
{
    public static class DirectoryExtensions
    {
        public static string GetEncryptedName(this IDirectory directory)
        {
            if (directory is not ILinuxEncDirectoryWrapper dir) throw new Exception("IDirectory is not the linux version");
            return directory.GetEncryptedName();
        }
    }
}