using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using CrazyFS.Linux;
using Mono.Unix.Native;
using Mono.Unix;

namespace CrazyFS.FileSystem
{
    public static class IFileSystemInfoExtensions
    {
        public static bool IsSymlink (this IFileSystemInfo fileSystemInfo)
        {
            UnixSymbolicLinkInfo i = new UnixSymbolicLinkInfo(fileSystemInfo.FullName);
            switch( i.FileType )
            {
                case FileTypes.SymbolicLink:
                    return true;
                case FileTypes.Fifo:
                case FileTypes.Socket:
                case FileTypes.BlockDevice:
                case FileTypes.CharacterDevice:
                case FileTypes.Directory:
                case FileTypes.RegularFile:
                default:
                    return false;
            }
        }
        public static string GetRealPath(this IFileSystemInfo fileSystemInfo)
        {
            return UnixPath.GetRealPath(fileSystemInfo.FullName);
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
        
        public static IEnumerable<IDirectoryInfo> GetPassthroughDirectoryInfos(this IEnumerable<IDirectoryInfo> fileSystemInfos, string source, string destination)
        {
            return fileSystemInfos.ToList().ConvertAll<IDirectoryInfo>(x => new LinuxDirectoryInfo(x.FileSystem, source, destination, x));
        }
        
        public static IEnumerable<IFileInfo> GetPassthroughFileInfos(this IEnumerable<IFileInfo> fileSystemInfos, string source, string destination)
        {
            return fileSystemInfos.ToList().ConvertAll<IFileInfo>(x => new LinuxFileInfo(x.FileSystem, source, destination, x));
        }
        
        public static IDirectoryInfo GetPassthroughDirectoryInfo(this IDirectoryInfo fileSystemInfo, string source, string destination)
        {
            return new LinuxDirectoryInfo(fileSystemInfo.FileSystem, source, destination, fileSystemInfo);
        }
        
        public static IFileInfo GetPassthroughFileInfo(this IFileInfo fileSystemInfo, string source, string destination)
        {
            return new LinuxFileInfo(fileSystemInfo.FileSystem, source, destination, fileSystemInfo);
        }
    }
}