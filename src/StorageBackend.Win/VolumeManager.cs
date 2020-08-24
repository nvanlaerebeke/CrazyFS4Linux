using Fsp;

namespace StorageBackend.Win {
    public static class VolumeManager {
        public static FileSystemHost Mount(FileSystemBase pFileSystem, string pMountPoint, byte[] pSecurityDescriptor, bool pSynchonized, uint pDebugLog) {
            var Host = new FileSystemHost(pFileSystem);
            _ = Host.Mount(pMountPoint, pSecurityDescriptor, pSynchonized, pDebugLog);
            return Host;
        }

        public static void UMount(FileSystemHost pHost) {
            pHost.Unmount();
        }
    }
}
