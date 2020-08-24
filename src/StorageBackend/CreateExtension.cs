using System;

namespace StorageBackend {
    public static class CreateExtension {
        public static IStorageBackend Create<T>(Options pOptions) where T : IStorageBackend {
            return (T)Activator.CreateInstance(typeof(T), pOptions);
        }
    }
}
