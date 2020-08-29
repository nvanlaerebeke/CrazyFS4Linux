namespace StorageBackend {
    public interface IFileSystemHost {
        ushort SectorSize { get; set; }
        ushort SectorsPerAllocationUnit { get; set; }
        ushort MaxComponentLength { get; set; }
        uint FileInfoTimeout { get; set; }
        bool CaseSensitiveSearch { get; set; }
        bool CasePreservedNames { get; set; }
        bool PostCleanupWhenModifiedOnly { get; set; }
        ulong VolumeCreationTime { get; set; }
        uint VolumeSerialNumber { get; set; }
        bool FlushAndPurgeOnCleanup { get; set; }
        bool PassQueryDirectoryPattern { get; set; }
        bool PersistentAcls { get; set; }
        bool UnicodeOnDisk { get; set; }
        int Mount(string MountPoint, byte[] SecurityDescriptor = null, bool Synchronized = false, uint DebugLog = 0);
        void UMount();
    }
}
