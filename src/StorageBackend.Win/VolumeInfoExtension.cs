namespace StorageBackend {
    public static class VolumeInfoExtension {
        public static void GetStruct(
            this IVolumeInfo pVolumeInfo,
            out Fsp.Interop.VolumeInfo VolumeInfo
        ) {
            VolumeInfo = default;
            VolumeInfo.FreeSize = pVolumeInfo.FreeSize;
            VolumeInfo.TotalSize = pVolumeInfo.TotalSize;
        }
    }
}
