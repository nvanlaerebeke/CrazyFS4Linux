using Fsp;

namespace StorageBackend.Win {

    internal class VolumeManager {
        private readonly IFileSystemHost Host;

        public VolumeManager(IFileSystemHost pHost) => Host = pHost;

        public VolumeManager(FileSystemBase pFileSystem) => Host = new FileSystemHostWrapper(new FileSystemHost(pFileSystem));

        public void Mount(string pMountPoint, byte[] pSecurityDescriptor, bool pSynchonized, uint pDebugLog, string pLogFile) {
            _ = Host.Mount(pMountPoint, pSecurityDescriptor, pSynchonized, pDebugLog);
            //ToDo: move as a constructor param of Storage
            _ = FileSystemHost.SetDebugLogFile(pLogFile);
        }

        public void UnMount() => Host.Unmount();

        public int Initialize(long pCreationTimeUtc) {
            try {
                Host.SectorSize = 4096;
                Host.SectorsPerAllocationUnit = 1;
                Host.MaxComponentLength = 255;
                Host.FileInfoTimeout = 1000;
                Host.CaseSensitiveSearch = false;
                Host.CasePreservedNames = true;
                Host.UnicodeOnDisk = true;
                Host.PersistentAcls = true;
                Host.PostCleanupWhenModifiedOnly = true;
                Host.PassQueryDirectoryPattern = true;
                Host.FlushAndPurgeOnCleanup = true;
                Host.VolumeCreationTime = (ulong)pCreationTimeUtc;
                Host.VolumeSerialNumber = 0;
                return FileSystemStatus.STATUS_SUCCESS;
            } catch (Win32Exception ex) {
                throw WindowsExceptionGenerator.GetIOException(ex);
            } catch (NTException ex) {
                throw WindowsExceptionGenerator.GetIOException(ex);
            }
        }
    }
}