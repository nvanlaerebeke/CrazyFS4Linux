namespace StorageBackend {
    public interface IVolumeActions {
        void Mount(string pMountPoint, byte[] pSecurityDescriptor, bool pSynchronized, uint pDebugLog, string pLogFile);
        void UnMount();
    }
}
