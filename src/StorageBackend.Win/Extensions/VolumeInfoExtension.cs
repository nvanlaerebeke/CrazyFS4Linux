using StorageBackend.Volume;

namespace StorageBackend {

    public static class IVolumeInfoExtension {

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