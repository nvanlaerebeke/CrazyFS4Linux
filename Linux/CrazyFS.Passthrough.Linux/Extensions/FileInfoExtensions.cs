using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;

// ReSharper disable once CheckNamespace
namespace CrazyFS.Passthrough.Linux
{
    public static class FileInfoExtensions
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