namespace StorageBackend.Volume {

    public interface IVolumeInfo {
        string Label { get; }
        long FreeSize { get; }
        long TotalSize { get; }
    }
}