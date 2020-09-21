using Fsp;
using StorageBackend.Volume;
using System.IO;
using System.IO.Abstractions;

namespace StorageBackend.Win.Winfsp {

    internal class VolumeManager {
        private readonly IFileSystemHost Host;
        private readonly string Source;

        public VolumeManager(IFileSystemHost pHost, string pSource) {
            Host = pHost;
            Source = pSource;
        }

        public VolumeManager(FileSystemBase pFileSystem, string pSource) {
            Host = new FileSystemHostWrapper(new FileSystemHost(pFileSystem));
            Source = pSource;
        }

        public void Mount(string pMountPoint, byte[] pSecurityDescriptor, bool pSynchonized, uint pDebugLog, string pLogFile) {
            _ = Host.Mount(pMountPoint, pSecurityDescriptor, pSynchonized, pDebugLog);
            //ToDo: move as a constructor param of Storage
            _ = FileSystemHost.SetDebugLogFile(pLogFile);
        }

        public void UnMount() => Host.Unmount();

        public Result Initialize(long pCreationTimeUtc) {
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
                return new Result(ResultStatus.Success);
            } catch (Win32Exception ex) {
                throw WindowsExceptionGenerator.GetIOException(ex);
            } catch (NTException ex) {
                throw WindowsExceptionGenerator.GetIOException(ex);
            }
        }

        public IVolumeInfo GetVolumeInfo() {
            return new VolumeInfo(new DriveInfoWrapper(new System.IO.Abstractions.FileSystem(), new DriveInfo(Source)), "MyLabel");
        }
    }
}