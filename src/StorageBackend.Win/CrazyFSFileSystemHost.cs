using Fsp;

namespace StorageBackend.Win {
    public class CrazyFSFileSystemHost : IFileSystemHost {
        private readonly FileSystemHost Host;
        public CrazyFSFileSystemHost(FileSystemHost pHost) {
            Host = pHost;
        }
        public ushort SectorSize {
            get { return Host.SectorSize; }
            set { Host.SectorSize = value; }
        }

        public ushort SectorsPerAllocationUnit {
            get { return Host.SectorsPerAllocationUnit; }
            set { Host.SectorsPerAllocationUnit = value; }
        }

        public ushort MaxComponentLength {
            get { return Host.MaxComponentLength; }
            set { Host.MaxComponentLength = value; }
        }

        public uint FileInfoTimeout {
            get { return Host.FileInfoTimeout; }
            set { Host.FileInfoTimeout = value; }
        }

        public bool CaseSensitiveSearch {
            get { return Host.CaseSensitiveSearch; }
            set { Host.CaseSensitiveSearch = value; }
        }

        public bool CasePreservedNames {
            get { return Host.CasePreservedNames; }
            set { Host.CasePreservedNames = value; }
        }

        public bool PostCleanupWhenModifiedOnly {
            get { return Host.PostCleanupWhenModifiedOnly; }
            set { Host.PostCleanupWhenModifiedOnly = value; }
        }

        public ulong VolumeCreationTime {
            get { return Host.VolumeCreationTime; }
            set { Host.VolumeCreationTime = value; }
        }

        public uint VolumeSerialNumber {
            get { return Host.VolumeSerialNumber; }
            set { Host.VolumeSerialNumber = value; }
        }

        public bool FlushAndPurgeOnCleanup {
            get { return Host.FlushAndPurgeOnCleanup; }
            set { Host.FlushAndPurgeOnCleanup = value; }
        }

        public bool PassQueryDirectoryPattern {
            get { return Host.PassQueryDirectoryPattern; }
            set { Host.PassQueryDirectoryPattern = value; }
        }

        public bool PersistentAcls {
            get { return Host.PersistentAcls; }
            set { Host.PersistentAcls = value; }
        }

        public bool UnicodeOnDisk {
            get { return Host.UnicodeOnDisk; }
            set { Host.UnicodeOnDisk = value; }
        }

        public int Mount(string MountPoint, byte[] SecurityDescriptor = null, bool Synchronized = false, uint DebugLog = 0) {
            return Host.Mount(MountPoint, SecurityDescriptor, Synchronized, DebugLog);
        }

        public void UMount() {
            Host.Unmount();
        }
    }
}
