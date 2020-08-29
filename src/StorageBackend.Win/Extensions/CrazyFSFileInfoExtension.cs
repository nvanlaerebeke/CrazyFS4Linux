using Fsp.Interop;

namespace StorageBackend {
    public static class CrazyFSFileInfoExtension {
        public static void GetStruct(
            this ICrazyFSFileInfo pFileInfo,
            out FileInfo FileInfo
        ) {
            FileInfo = new FileInfo();

            if (pFileInfo == null) {
                return;
            }

            FileInfo.AllocationSize = pFileInfo.AllocationSize;
            FileInfo.ChangeTime = pFileInfo.ChangeTime;
            FileInfo.CreationTime = pFileInfo.CreationTime;
            FileInfo.EaSize = pFileInfo.EaSize;
            FileInfo.FileAttributes = pFileInfo.FileAttributes;
            FileInfo.FileSize = pFileInfo.FileSize;
            FileInfo.HardLinks = pFileInfo.HardLinks;
            FileInfo.IndexNumber = pFileInfo.IndexNumber;
            FileInfo.LastAccessTime = pFileInfo.LastAccessTime;
            FileInfo.LastWriteTime = pFileInfo.LastWriteTime;
            FileInfo.ReparseTag = pFileInfo.ReparseTag;
        }
    }
}
