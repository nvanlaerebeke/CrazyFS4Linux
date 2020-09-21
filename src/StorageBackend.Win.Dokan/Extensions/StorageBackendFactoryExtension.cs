namespace StorageBackend.Win.Dokan {

    public static class StorageBackendFactoryExtension {

        public static IVolumeActions CreateWindowsDokanStorageBackend<T>(
            this StorageBackendFactory pFactory,
            string pSource
        ) where T : IStorageType, new() {
            return new DokanFileSystem<T>(pSource);
        }
    }
}