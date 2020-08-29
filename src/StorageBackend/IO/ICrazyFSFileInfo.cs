namespace StorageBackend {
    public interface ICrazyFSFileInfo {
        ulong AllocationSize { get; }
        ulong ChangeTime { get; }
        ulong CreationTime { get; }
        uint EaSize { get; }
        uint FileAttributes { get; }
        ulong FileSize { get; }
        uint HardLinks { get; }
        ulong IndexNumber { get; }
        ulong LastAccessTime { get; }
        ulong LastWriteTime { get; }
        uint ReparseTag { get; }
    }
}