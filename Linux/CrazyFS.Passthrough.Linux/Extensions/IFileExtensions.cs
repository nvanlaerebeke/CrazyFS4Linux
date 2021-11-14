using System;
using System.IO.Abstractions;
using CrazyFS.Linux;
using Mono.Unix.Native;

// ReSharper disable once CheckNamespace
namespace CrazyFS.FileSystem
{
    public static class IFileExtensions
    {
        public static IFileInfo CreateSpecialFile(this IFile file, string path, FilePermissions mode, ulong rdev)
        {
            if (file is LinuxFileWrapper fileWrapper)
            {
                return fileWrapper.CreateSpecialFile(path, mode, rdev);
            }

            throw new Exception("IFile is not the linux version");
        }
    }
}