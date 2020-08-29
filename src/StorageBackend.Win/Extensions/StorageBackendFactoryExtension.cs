using Fsp;
using StorageBackend.Win;

namespace StorageBackend {
    public static class StorageBackendFactoryExtension {
        public static FileSystemBase CreateWindowsStorageBackend(
            this StorageBackendFactory pFactory,
            IStorageBackend pStorageBackend
        ) {
            return new Storage(pStorageBackend);
        }
    }
}
