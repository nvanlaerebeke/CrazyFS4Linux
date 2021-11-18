using System;
using System.IO;
using System.IO.Abstractions;
using CrazyFS.Passthrough.Linux;
using Mono.Unix.Native;

namespace CrazyFS.Linux
{
    /// <summary>
    /// ToDo: Move to Linux implementation
    /// </summary>
    public static class FileSystemInfoExtension
    {
        public static Stat ToStat(this IFileSystemInfo fileSystemInfo)
        {
            if (fileSystemInfo is not ILinuxFileSystemInfo info) throw new Exception("Unable to convert path information");
            const int blockSize = 4096;
            var o = new Stat
            {
                st_dev = info.st_dev,
                st_ino = info.st_ino,
                st_mode = info.st_mode,
                st_nlink = info.st_nlink,
                st_uid = info.st_uid,
                st_gid = info.st_gid,
                st_rdev = info.st_rdev,
                //st_size = 0,
                st_blksize = blockSize,
                //st_blocks = 0,
                st_atime =  info.st_atime,
                st_mtime = info.st_mtime,
                st_ctime = info.st_ctime,
                st_atime_nsec = info.st_atime_nsec,
                st_mtime_nsec = info.st_mtime_nsec,
                st_ctime_nsec = info.st_ctime_nsec,
                st_atim = info.st_atim,
                st_mtim = info.st_mtim,
                st_ctim = info.st_ctim
            };

            if ((fileSystemInfo.Attributes & FileAttributes.Directory) != 0) return o;
                
            o.st_size = ((IFileInfo) fileSystemInfo).Length;
            o.st_blocks = (long)Math.Ceiling(new decimal(o.st_size / blockSize));
            return o;
        }
    }
}