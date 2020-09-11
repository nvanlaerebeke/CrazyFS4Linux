using System.IO.Abstractions;

namespace StorageBackend.Volume {

    public class VolumeInfo : IVolumeInfo {

        public VolumeInfo(IDriveInfo driveInfo) {
            TotalSize = (ulong)driveInfo.TotalSize;
            FreeSize = (ulong)driveInfo.AvailableFreeSpace;
        }

        public ulong TotalSize { get; }
        public ulong FreeSize { get; }
    }
}