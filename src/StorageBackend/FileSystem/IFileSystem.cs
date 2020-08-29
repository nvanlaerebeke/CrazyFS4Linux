namespace StorageBackend {
    public interface IFileSystem {
        void Mount(string pMountPoint, byte[] pSecurityDescriptor, bool pSynchronized, uint pDebugLog, string pLogFile);
        void UnMount();
    }
}
