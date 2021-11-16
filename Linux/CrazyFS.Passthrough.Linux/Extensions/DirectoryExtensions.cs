using System;
using System.IO.Abstractions;
using Mono.Unix.Native;

// ReSharper disable once CheckNamespace
namespace CrazyFS.Passthrough.Linux
{
    public static class DirectoryExtensions
    {
        public static IDirectoryInfo CreateDirectory(this IDirectory directory, string path, FilePermissions mode)
        {
            if (directory is LinuxDirectoryWrapper dir)
            {
                return dir.CreateDirectory(path, mode);
            }

            throw new Exception("IDirectory is not the linux version");
        }
    }
}