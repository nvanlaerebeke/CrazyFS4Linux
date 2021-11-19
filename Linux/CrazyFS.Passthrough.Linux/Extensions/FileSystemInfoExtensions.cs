using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using CrazyFS.Passthrough.Linux.Interfaces;
using Mono.Unix;

namespace CrazyFS.Passthrough.Linux.Extensions
{
    public static class FileSystemInfoExtensions
    {
        public static bool IsSymLink (this IFileSystemInfo fileSystemInfo)
        {
            if (fileSystemInfo is not ILinuxFileSystemInfo info) throw new Exception("IFileSystemInfo is not the linux version");
            return info.IsSymLink();
        }
    }
}