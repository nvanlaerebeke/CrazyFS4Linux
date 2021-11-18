using System;
using System.IO.Abstractions;
using Mono.Unix.Native;

// ReSharper disable once CheckNamespace
namespace CrazyFS.Passthrough.Linux
{
    public static class DirectoryExtensions
    {
        public static void CreateDirectory(this IDirectory directory, string path, FilePermissions mode)
        {
            if (directory is not LinuxDirectoryWrapper dir) throw new Exception("IDirectory is not the linux version");
            dir.CreateDirectory(path, mode);
        }
    }
}