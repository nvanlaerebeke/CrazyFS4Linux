using System;
using System.IO.Abstractions;
using Mono.Unix.Native;

// ReSharper disable once CheckNamespace
namespace CrazyFS.Passthrough.Linux
{
    public static class FileExtensions
    {
        public static void CreateSpecialFile(this IFile file, string path, FilePermissions mode, ulong rdev)
        {
            if (file is not LinuxFileWrapper fileWrapper) throw new Exception("IFile is not the linux version");
            fileWrapper.CreateSpecialFile(path, mode, rdev);
        }
    }
}