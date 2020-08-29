using StorageBackend.Win;

namespace StorageBackend {
    public static class StorageBackendFactoryExtension {
        public static IFileSystem CreateWindowsStorageBackend(
            this StorageBackendFactory pFactory,
            IStorageType pStorageBackend
        ) {
            return new WindowsBackend(pStorageBackend);
        }
    }
}
