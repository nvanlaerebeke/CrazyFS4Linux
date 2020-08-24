namespace StorageBackend {
    public interface IFileSystemHost {
        uint SectorSize { get; set; }
        uint SectorsPerAllocationUnit { get; set; }
        uint MaxComponentLength { get; set; }
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

        void Mount();
        void UMount();
    }
}
