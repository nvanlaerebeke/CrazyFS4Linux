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
        
        public static IEnumerable<IFileSystemInfo> GetPassthroughFileSystemInfo(this IEnumerable<IFileSystemInfo> fileSystemInfos, string basePath)
        {
            return fileSystemInfos.ToList().ConvertAll<IFileSystemInfo>(x =>
            {
                if (x.Attributes.HasFlag(FileAttributes.Directory))
                {
                    return new LinuxDirectoryInfo(x.FileSystem, basePath, x);
                }
                else
                {
                    return new LinuxFileInfo(x.FileSystem, basePath, x);
                }
            });
        }
        
        public static IEnumerable<IDirectoryInfo> GetPassthroughDirectoryInfos(this IEnumerable<IDirectoryInfo> fileSystemInfos, string basePath)
        {
            return fileSystemInfos.ToList().ConvertAll<IDirectoryInfo>(x => new LinuxDirectoryInfo(x.FileSystem, basePath, x));
        }
        
        public static IEnumerable<IFileInfo> GetPassthroughFileInfos(this IEnumerable<IFileInfo> fileSystemInfos, string basePath)
        {
            return fileSystemInfos.ToList().ConvertAll<IFileInfo>(x => new LinuxFileInfo(x.FileSystem, basePath, x));
        }
        
        public static IDirectoryInfo GetPassthroughDirectoryInfo(this IDirectoryInfo fileSystemInfo, string basePath)
        {
            return new LinuxDirectoryInfo(fileSystemInfo.FileSystem, basePath, fileSystemInfo);
        }
        
        public static IFileInfo GetPassthroughFileInfo(this IFileInfo fileSystemInfo, string basePath)
        {
            return new LinuxFileInfo(fileSystemInfo.FileSystem, basePath, fileSystemInfo);
        }
    }
}