using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;

// ReSharper disable once CheckNamespace
namespace CrazyFS.Passthrough.Linux
{
    public static class DirectoryInfoExtensions
    {
        public static IEnumerable<IDirectoryInfo> GetPassthroughDirectoryInfos(this IEnumerable<IDirectoryInfo> fileSystemInfos, string source, string destination)
        {
            return fileSystemInfos.ToList().ConvertAll<IDirectoryInfo>(x => new LinuxDirectoryInfo(x.FileSystem, source, destination, x));
        }

        public static IDirectoryInfo GetPassthroughDirectoryInfo(this IDirectoryInfo fileSystemInfo, string source, string destination)
        {
            return new LinuxDirectoryInfo(fileSystemInfo.FileSystem, source, destination, fileSystemInfo);
        }
    }
}