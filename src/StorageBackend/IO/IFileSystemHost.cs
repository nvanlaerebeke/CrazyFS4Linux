namespace StorageBackend {

    public interface IFileSystemHost {
        bool CasePreservedNames { get; set; }
        bool CaseSensitiveSearch { get; set; }
        uint FileInfoTimeout { get; set; }
        bool FlushAndPurgeOnCleanup { get; set; }
        ushort MaxComponentLength { get; set; }
        bool PassQueryDirectoryPattern { get; set; }
        bool PersistentAcls { get; set; }
        bool PostCleanupWhenModifiedOnly { get; set; }
        ushort SectorSize { get; set; }
        uint StreamInfoTimeout { get; set; }
        bool UnicodeOnDisk { get; set; }
        ulong VolumeCreationTime { get; set; }
        uint VolumeSerialNumber { get; set; }
        ushort SectorsPerAllocationUnit { get; set; }

        int Mount(string MountPoint, byte[] SecurityDescriptor = null, bool Synchronized = false, uint DebugLog = 0);

        void Unmount();
    }
}