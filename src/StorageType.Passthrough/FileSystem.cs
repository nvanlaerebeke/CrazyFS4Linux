using Fsp;
using Fsp.Interop;
using System;
using System.IO;

namespace StorageType.Passthrough {
    internal class FileSystem {
        protected readonly ushort AllocationSize;
        public FileSystem() : this(4096) { }
        public FileSystem(ushort pDefaultAllocationSize) {
            AllocationSize = pDefaultAllocationSize;
        }
        public static int Init(FileSystemHost pHost, string pPath) {
            pHost.SectorSize = 4096;
            pHost.SectorsPerAllocationUnit = 1;
            pHost.MaxComponentLength = 255;
            pHost.FileInfoTimeout = 1000;
            pHost.CaseSensitiveSearch = false;
            pHost.CasePreservedNames = true;
            pHost.UnicodeOnDisk = true;
            pHost.PersistentAcls = true;
            pHost.PostCleanupWhenModifiedOnly = true;
            pHost.PassQueryDirectoryPattern = true;
            pHost.FlushAndPurgeOnCleanup = true;
            pHost.VolumeCreationTime = (ulong)File.GetCreationTimeUtc(pPath).ToFileTimeUtc();
            pHost.VolumeSerialNumber = 0;
            return FileSystemStatus.STATUS_SUCCESS;
        }

        internal static VolumeInfo GetVolumeInfo(string pPath) {
            try {
                DriveInfo Info = new DriveInfo(pPath);
                return new VolumeInfo() {
                    TotalSize = (ulong)Info.TotalSize,
                    FreeSize = (ulong)Info.TotalFreeSpace
                };
            } catch (ArgumentException) {
                /*
                 * DriveInfo only supports drives and does not support UNC paths.
                 * It would be better to use GetDiskFreeSpaceEx here.
                 */
                throw;
            }
        }
    }
}