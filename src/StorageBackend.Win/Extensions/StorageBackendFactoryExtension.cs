using StorageBackend.Win;

namespace StorageBackend {

    public static class StorageBackendFactoryExtension {

        public static IFileSystem CreateWindowsStorageBackend<T>(
            this StorageBackendFactory pFactory,
            string pSource
        ) where T : IStorageType, new() {
            return new WindowsFileSystemBase<T>(pSource);
        }
    }
}