using Fsp;
using StorageBackend.Win;
using System;

namespace StorageBackend {
    public static class StorageBackendFactoryExtension {
        public static FileSystemBase CreateWindowsStorageBackend<T>(
            this StorageBackendFactory pFactory,
            Options pOptions
        ) where T : IStorageBackend {
            var o = (T)Activator.CreateInstance(typeof(T), pOptions);
            return new Storage(o);
        }
    }
}
