using System.IO;

namespace StorageBackend {
    public class VolumeInfo : IVolumeInfo {
        public VolumeInfo(DriveInfo driveInfo) {
            TotalSize = (ulong)driveInfo.TotalSize;
            FreeSize = (ulong)driveInfo.AvailableFreeSpace;
        }

        public ulong TotalSize { get; private set; }
        public ulong FreeSize { get; private set; }
    }
}
