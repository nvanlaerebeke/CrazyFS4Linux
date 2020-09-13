using StorageBackend.Win.Winfsp;

namespace StorageBackend {

    public static class StorageBackendFactoryExtension {

        public static IVolumeActions CreateWindowsStorageBackend<T>(
            this StorageBackendFactory pFactory,
            string pSource
        ) where T : IStorageType, new() {
            return new WindowsFileSystemBase<T>(pSource);
        }
    }
}