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

            FileInfo.AllocationSize = pEntry.AllocationSize;
            FileInfo.ChangeTime = pEntry.ChangeTime;
            FileInfo.CreationTime = pEntry.CreationTime;
            FileInfo.EaSize = pEntry.EaSize;
            FileInfo.FileAttributes = pEntry.Attributes;
            FileInfo.FileSize = pEntry.FileSize;
            FileInfo.HardLinks = pEntry.HardLinks;
            FileInfo.IndexNumber = pEntry.IndexNumber;
            FileInfo.LastAccessTime = pEntry.LastAccessTime;
            FileInfo.LastWriteTime = pEntry.LastWriteTime;
            FileInfo.ReparseTag = pEntry.ReparseTag;
        }
    }
}