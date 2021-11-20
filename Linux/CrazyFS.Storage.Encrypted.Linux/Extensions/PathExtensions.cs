using System;
using System.IO.Abstractions;
using CrazyFS.FileSystem.Encrypted.Linux.Interfaces;
using CrazyFS.Passthrough.Linux.Interfaces;

namespace CrazyFS.FileSystem.Encrypted.Linux.Extensions
{
    public static class PathExtensions
    {
        public static string GetDecryptedPath(this IPath pathInterface, string path)
        {
            if (pathInterface is not ILinuxEncPathWrapper pathWrapper) throw new Exception("IPath is not the encrypted linux version");
            return pathWrapper.GetDecryptedPath(path);
        }
        
        public static string GetEncryptedPath(this IPath pathInterface, string path)
        {
            if (pathInterface is not ILinuxEncPathWrapper pathWrapper) throw new Exception("IPath is not the encrypted linux version");
            return pathWrapper.GetEncryptedPath(path);
        }
    }
}