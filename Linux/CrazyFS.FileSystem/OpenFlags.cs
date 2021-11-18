using System;
// ReSharper disable UnusedMember.Global

namespace CrazyFS.FileSystem
{
    [Flags]
    public enum OpenFlags
    {
        O_RDONLY = 0,
        O_WRONLY = 1,
        O_RDWR = 2,
        O_CREAT = 64, // 0x00000040
        O_EXCL = 128, // 0x00000080
        O_NOCTTY = 256, // 0x00000100
        O_TRUNC = 512, // 0x00000200
        O_APPEND = 1024, // 0x00000400
        O_NONBLOCK = 2048, // 0x00000800
        O_SYNC = 4096, // 0x00001000
        O_NOFOLLOW = 131072, // 0x00020000
        O_DIRECTORY = 65536, // 0x00010000
        O_DIRECT = 16384, // 0x00004000
        O_ASYNC = 8192, // 0x00002000
        O_LARGEFILE = 32768, // 0x00008000
        O_CLOEXEC = 524288, // 0x00080000
        O_PATH = 2097152, // 0x00200000
    }
}