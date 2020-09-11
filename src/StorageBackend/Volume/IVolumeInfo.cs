namespace StorageBackend.Volume {

    public interface IVolumeInfo {
        ulong FreeSize { get; }
        ulong TotalSize { get; }
    }
}