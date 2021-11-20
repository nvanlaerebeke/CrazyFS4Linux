using System;
using System.IO.Abstractions;
using CrazyFS.FileSystem.Encrypted.Linux.Interfaces;

namespace CrazyFS.FileSystem.Encrypted.Linux.Extensions
{
    public static class FileSystemInfoExtension
    {
        public static string GetEncryptedName(this IFileSystemInfo fileSystemInfo)
        {
            if (fileSystemInfo is not ILinuxEncFileSystemInfo linuxFileSystemInfo) throw new Exception("IFileSystemInfo is not the encrypted linux version");
            return linuxFileSystemInfo.GetEncryptedName();
        }
    }
}