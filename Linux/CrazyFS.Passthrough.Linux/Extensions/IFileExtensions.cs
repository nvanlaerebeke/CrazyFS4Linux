using System;
using System.IO.Abstractions;
using CrazyFS.Passthrough.Linux.Interfaces;
using Mono.Unix.Native;

namespace CrazyFS.Passthrough.Linux.Extensions
{
    public static class FileExtensions
    {
        public static void CreateSpecialFile(this IFile file, string path, FilePermissions mode, ulong rdev)
        {
            if (file is not ILinuxFile fileWrapper) throw new Exception("IFile is not the linux version");
            fileWrapper.CreateSpecialFile(path, mode, rdev);
        }
    }
}