namespace StorageBackend.IO {

    public interface IEntry {
        ulong AllocationSize { get; }
        ulong ChangeTime { get; }
        ulong CreationTime { get; }
        uint EaSize { get; }
        uint Attributes { get; }
        ulong FileSize { get; }
        uint HardLinks { get; }
        ulong IndexNumber { get; }
        ulong LastAccessTime { get; }
        ulong LastWriteTime { get; }
        uint ReparseTag { get; }

        bool IsFile();
    }
}