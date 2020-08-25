namespace StorageBackend {
    public interface IVolumeInfo {
        ulong FreeSize { get; }
        ulong TotalSize { get; }
    }
}
