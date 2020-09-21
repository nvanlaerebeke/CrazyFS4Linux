using System;
using System.IO;

namespace StorageBackend.IO {

    public interface IEntry : ICloneable {
        string FullName { get; }
        string Name { get; }
        long AllocationSize { get; }
        DateTime ChangeTime { get; }
        DateTime CreationTime { get; }
        uint EaSize { get; }
        FileAttributes Attributes { get; }
        long FileSize { get; }
        uint HardLinks { get; }
        ulong IndexNumber { get; }
        DateTime LastAccessTime { get; }
        DateTime LastWriteTime { get; }
        uint ReparseTag { get; }

        bool IsFile();
    }
}