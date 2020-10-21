using DokanNet;
using System;
using System.IO;
using System.Linq;

namespace StorageBackend.Win.Dokan.Volume {

    internal static class VolumeManager {

        public static NtStatus GetDiskFreeSpace(string path, out long freeBytesAvailable, out long totalNumberOfBytes, out long totalNumberOfFreeBytes) {
            var dinfo = DriveInfo.GetDrives().Single(di => string.Equals(di.RootDirectory.Name, Path.GetPathRoot(path + "\\"), StringComparison.OrdinalIgnoreCase));
            freeBytesAvailable = dinfo.TotalFreeSpace;
            totalNumberOfBytes = dinfo.TotalSize;
            totalNumberOfFreeBytes = dinfo.AvailableFreeSpace;
            return NtStatus.Success;
        }

        public static NtStatus GetVolumeInformation(out string volumeLabel, out FileSystemFeatures features, out string fileSystemName, out uint maximumComponentLength) {
            volumeLabel = "DOKAN";
            fileSystemName = "NTFS";
            maximumComponentLength = 256;

            features = FileSystemFeatures.CasePreservedNames | FileSystemFeatures.CaseSensitiveSearch | FileSystemFeatures.PersistentAcls | FileSystemFeatures.SupportsRemoteStorage | FileSystemFeatures.UnicodeOnDisk;
            return NtStatus.Success;
        }
    }
}