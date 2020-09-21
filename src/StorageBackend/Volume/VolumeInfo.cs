using System.IO.Abstractions;

namespace StorageBackend.Volume {

    public class VolumeInfo : IVolumeInfo {

        public VolumeInfo(IDriveInfo driveInfo, string pLabel) {
            Label = pLabel;
            TotalSize = driveInfo.TotalSize;
            FreeSize = driveInfo.AvailableFreeSpace;
        }

        public string Label { get; }
        public long TotalSize { get; }
        public long FreeSize { get; }
    }
}