using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using Mono.Unix;

// ReSharper disable once CheckNamespace
namespace CrazyFS.Passthrough.Linux
{
    public static class FileSystemInfoExtensions
    {
        public static bool IsSymlink (this IFileSystemInfo fileSystemInfo)
        {
            return new UnixSymbolicLinkInfo(fileSystemInfo.FullName).FileType switch
            {
                FileTypes.SymbolicLink => true,
                _ => false
            };
        }
       
        public static IEnumerable<IFileSystemInfo> GetPassthroughFileSystemInfo(this IEnumerable<IFileSystemInfo> fileSystemInfos, string source, string destination)
        {
            return fileSystemInfos.ToList().ConvertAll<IFileSystemInfo>(x =>
            {
                if (x.Attributes.HasFlag(FileAttributes.Directory))
                {
                    return new LinuxDirectoryInfo(x.FileSystem, source, destination, x);
                }
                else
                {
                    return new LinuxFileInfo(x.FileSystem, source, destination, x);
                }
            });
        }
    }
}