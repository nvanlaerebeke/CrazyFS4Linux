using System;
using System.IO.Abstractions;
using CrazyFS.Passthrough.Linux.Interfaces;
using Mono.Unix.Native;

namespace CrazyFS.Passthrough.Linux.Extensions
{
    public static class DirectoryExtensions
    {
        public static void CreateDirectory(this IDirectory directory, string path, FilePermissions mode)
        {
            if (directory is not ILinuxDirectory dir) throw new Exception("IDirectory is not the linux version");
            dir.CreateDirectory(path, mode);
        }
    }
}