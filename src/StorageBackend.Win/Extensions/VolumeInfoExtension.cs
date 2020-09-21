using StorageBackend.Volume;

namespace StorageBackend {

    public static class IVolumeInfoExtension {

        public static void GetStruct(
            this IVolumeInfo pVolumeInfo,
            out Fsp.Interop.VolumeInfo VolumeInfo
        ) {
            VolumeInfo = default;
            VolumeInfo.FreeSize = (ulong)pVolumeInfo.FreeSize;
            VolumeInfo.TotalSize = (ulong)pVolumeInfo.TotalSize;
        }
    }
}