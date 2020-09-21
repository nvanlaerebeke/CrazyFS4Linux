using Fsp.Interop;
using StorageBackend.IO;

namespace StorageBackend.Win {

    public static class IEntryExtension {

        public static void GetStruct(
            this IEntry pEntry,
            out FileInfo FileInfo
        ) {
            FileInfo = new FileInfo();

            if (pEntry == null) {
                return;
            }
            FileInfo.AllocationSize = (ulong)pEntry.AllocationSize;
            FileInfo.ChangeTime = (ulong)pEntry.ChangeTime.ToFileTimeUtc();
            FileInfo.CreationTime = (ulong)pEntry.CreationTime.ToFileTimeUtc();
            FileInfo.EaSize = pEntry.EaSize;
            FileInfo.FileAttributes = (uint)pEntry.Attributes;
            FileInfo.FileSize = (ulong)pEntry.FileSize;
            FileInfo.HardLinks = pEntry.HardLinks;
            FileInfo.IndexNumber = pEntry.IndexNumber;
            FileInfo.LastAccessTime = (ulong)pEntry.LastAccessTime.ToFileTimeUtc();
            FileInfo.LastWriteTime = (ulong)pEntry.LastWriteTime.ToFileTimeUtc();
            FileInfo.ReparseTag = pEntry.ReparseTag;
        }
    }
}