using DokanNet;
using StorageBackend.IO;

namespace StorageBackend.Win.Dokan {

    public static class IFSEntryPointExtension {

        public static FileInformation ToFileInformation(
            this IFSEntryPointer entry
        ) => new FileInformation() {
            Attributes = entry.Attributes,
            CreationTime = entry.CreationTime,
            FileName = entry.Name,
            LastAccessTime = entry.LastAccessTime,
            LastWriteTime = entry.LastWriteTime,
            Length = entry.FileSize
        };
    }
}