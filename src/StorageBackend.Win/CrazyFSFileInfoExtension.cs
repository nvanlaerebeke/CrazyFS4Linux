using Fsp.Interop;

namespace StorageBackend {
    public static class CrazyFSFileInfoExtension {
        public static FileInfo GetStruct(
            this ICrazyFSFileInfo pFileInfo
        ) {
            return new FileInfo();
        }
    }
}
