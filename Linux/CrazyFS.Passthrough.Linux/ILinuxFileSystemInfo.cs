using System.IO.Abstractions;
using Mono.Unix.Native;

namespace CrazyFS.Linux
{
    public interface ILinuxFileSystemInfo : IFileSystemInfo
    {
        ulong st_dev { get; }
        ulong st_ino { get; }
        FilePermissions st_mode { get; }
        ulong st_nlink { get; }
        uint st_uid { get; }
        uint st_gid { get; }
        ulong st_rdev { get; }
         long st_atime { get; }
         long st_mtime { get; }
         long st_ctime { get; }
         long st_atime_nsec { get; }
         long st_mtime_nsec { get; }
         long st_ctime_nsec { get; }    
         Timespec st_atim { get; }
         Timespec st_mtim  { get; }
         Timespec st_ctim  { get; }
    }
}