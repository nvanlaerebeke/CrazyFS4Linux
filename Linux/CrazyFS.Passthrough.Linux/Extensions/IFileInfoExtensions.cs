using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using CrazyFS.Linux;

// ReSharper disable once CheckNamespace
namespace CrazyFS.FileSystem
{
    public static class IFileInfoExtensions
    {
        public static IEnumerable<IFileInfo> GetPassthroughFileInfos(this IEnumerable<IFileInfo> fileSystemInfos, string source, string destination)
        {
            return fileSystemInfos.ToList().ConvertAll<IFileInfo>(x => new LinuxFileInfo(x.FileSystem, source, destination, x));
        }

        public static IFileInfo GetPassthroughFileInfo(this IFileInfo fileSystemInfo, string source, string destination)
        {
            return new LinuxFileInfo(fileSystemInfo.FileSystem, source, destination, fileSystemInfo);
        }
    }
}