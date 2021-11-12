using System.IO;
using System.IO.Abstractions;
using Mono.Unix.Native;

namespace CrazyFS.Linux
{
    class LinuxDirectoryInfo : DirectoryInfoWrapper, ILinuxFileSystemInfo
    {
        private Stat _stat;
        public LinuxDirectoryInfo(IFileSystem fileSystem, DirectoryInfo info) : base(fileSystem, info)
        {
            _ = Syscall.stat(info.FullName, out _stat);
        }
        
        public ulong st_dev => _stat.st_dev;
        public ulong st_ino => _stat.st_ino;
        public FilePermissions st_mode => _stat.st_mode;
        public ulong st_nlink => _stat.st_nlink;
        public uint st_uid => _stat.st_uid;
        public uint st_gid => _stat.st_gid;
        public ulong st_rdev => _stat.st_rdev;
        public long st_atime => _stat.st_atime;
        public long st_mtime => _stat.st_mtime;
        public long st_ctime => _stat.st_ctime;
        public long st_atime_nsec => _stat.st_atime_nsec;
        public long st_mtime_nsec => _stat.st_mtime_nsec;
        public long st_ctime_nsec => _stat.st_ctime_nsec;        
        public Timespec st_atim => _stat.st_atim;
        public Timespec st_mtim => _stat.st_mtim;
        public Timespec st_ctim => _stat.st_ctim;
        
        public override void Refresh()
        {
            base.Refresh();
            _ = Syscall.stat(FullName, out _stat);
        }
    }
}