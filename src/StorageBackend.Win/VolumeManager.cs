using Fsp;

namespace StorageBackend.Win {
    public class VolumeManager {
        public FileSystemHost Mount(FileSystemBase pFileSystem, string pMountPoint, byte[] pSecurityDescriptor, bool pSynchonized, uint pDebugLog, string pLogFile) {
            var Host = new FileSystemHost(pFileSystem);
            _ = Host.Mount(pMountPoint, pSecurityDescriptor, pSynchonized, pDebugLog);
            //ToDo: move as a constructor param of Storage
            _ = FileSystemHost.SetDebugLogFile(pLogFile);
            return Host;
        }

        public void UMount(FileSystemHost pHost) {
            pHost.Unmount();
        }
    }
}
