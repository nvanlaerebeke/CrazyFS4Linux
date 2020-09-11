using Fsp;

namespace StorageBackend.Win {

    public class FileSystemHostWrapper : IFileSystemHost {
        private readonly FileSystemHost FileSystemHost;

        public FileSystemHostWrapper(FileSystemHost pFSHost) => FileSystemHost = pFSHost;

        public ushort SectorSize {
            get => FileSystemHost.SectorSize;
            set => FileSystemHost.SectorSize = value;
        }

        public uint StreamInfoTimeout {
            get => FileSystemHost.StreamInfoTimeout;
            set => FileSystemHost.StreamInfoTimeout = value;
        }

        public ushort SectorsPerAllocationUnit {
            get => FileSystemHost.SectorsPerAllocationUnit;
            set => FileSystemHost.SectorsPerAllocationUnit = value;
        }

        public ushort MaxComponentLength {
            get => FileSystemHost.MaxComponentLength;
            set => FileSystemHost.MaxComponentLength = value;
        }

        public uint FileInfoTimeout {
            get => FileSystemHost.FileInfoTimeout;
            set => FileSystemHost.FileInfoTimeout = value;
        }

        public bool CaseSensitiveSearch {
            get => FileSystemHost.CaseSensitiveSearch;
            set => FileSystemHost.CaseSensitiveSearch = value;
        }

        public bool CasePreservedNames {
            get => FileSystemHost.CasePreservedNames;
            set => FileSystemHost.CasePreservedNames = value;
        }

        public bool PostCleanupWhenModifiedOnly {
            get => FileSystemHost.PostCleanupWhenModifiedOnly;
            set => FileSystemHost.PostCleanupWhenModifiedOnly = value;
        }

        public ulong VolumeCreationTime {
            get => FileSystemHost.VolumeCreationTime;
            set => FileSystemHost.VolumeCreationTime = value;
        }

        public uint VolumeSerialNumber {
            get => FileSystemHost.VolumeSerialNumber;
            set => FileSystemHost.VolumeSerialNumber = value;
        }

        public bool FlushAndPurgeOnCleanup {
            get => FileSystemHost.FlushAndPurgeOnCleanup;
            set => FileSystemHost.FlushAndPurgeOnCleanup = value;
        }

        public bool PassQueryDirectoryPattern {
            get => FileSystemHost.PassQueryDirectoryPattern;
            set => FileSystemHost.PassQueryDirectoryPattern = value;
        }

        public bool PersistentAcls {
            get => FileSystemHost.PersistentAcls;
            set => FileSystemHost.PersistentAcls = value;
        }

        public bool UnicodeOnDisk {
            get => FileSystemHost.UnicodeOnDisk;
            set => FileSystemHost.UnicodeOnDisk = value;
        }

        public int Mount(string MountPoint, byte[] SecurityDescriptor = null, bool Synchronized = false, uint DebugLog = 0) => FileSystemHost.Mount(MountPoint, SecurityDescriptor, Synchronized, DebugLog);

        public void Unmount() => FileSystemHost.Unmount();
    }
}