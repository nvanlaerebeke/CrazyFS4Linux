using System;
using System.IO.Abstractions;
using CrazyFS.Linux;
using Mono.Unix.Native;

// ReSharper disable once CheckNamespace
namespace CrazyFS.FileSystem
{
    public static class IDirectoryExtensions
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