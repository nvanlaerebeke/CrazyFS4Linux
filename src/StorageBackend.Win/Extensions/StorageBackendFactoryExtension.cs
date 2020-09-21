using StorageBackend.Win.Winfsp;

namespace StorageBackend {

    public static class StorageBackendFactoryExtension {

        public static IVolumeActions CreateWindowsWinfspStorageBackend<T>(
            this StorageBackendFactory pFactory,
            string pSource
        ) where T : IStorageType, new() {
            return new WindowsFileSystemBase<T>(pSource);
        }
    }
}