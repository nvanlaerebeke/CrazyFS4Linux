using Fsp;
using StorageBackend;
using System;
using System.Security.AccessControl;

namespace StorageType.Passthrough {
    public class PassthroughStorage : IStorageBackend {
        private readonly Options Options;

        public PassthroughStorage(Options pOptions) {
            Options = pOptions;
            FileSystemHost.SetDebugLogFile(Options.LogFile);
        }

        public int CanDelete(FileDesc pFileDesc) {
            return CrazyFSFile.CanDelete(pFileDesc);
        }
        public void Cleanup(FileDesc pFileDesc, string pFileName, uint pFlags, uint pCleanupDelete) {
            CrazyFSPath.Cleanup(pFileDesc, pFlags, pCleanupDelete);
        }
        public void Close(FileDesc pFileDesc) {
            CrazyFSPath.Close(pFileDesc);
        }
        public int Create(string pPath, string pFileName, uint pCreateOptions, uint pGrantedAccess, uint pFileAttributes, byte[] pSecurityDescriptor, out object pFileNode, out object pFileDesc, out ICrazyFSFileInfo pFileInfo, out string pNormalizedName) {
            return CrazyFSPath.Create(pPath, pFileName, pCreateOptions, pGrantedAccess, pFileAttributes, pSecurityDescriptor, out pFileNode, out pFileDesc, out pFileInfo, out pNormalizedName);
        }
        public int Flush(object pFileNode, FileDesc pFileDesc, out ICrazyFSFileInfo pFileInfo) {
            return CrazyFSPath.Flush(pFileNode, pFileDesc, out pFileInfo);
        }
        public int GetFileInfo(FileDesc pFileDesc, out ICrazyFSFileInfo pFileInfo) {
            return CrazyFSFile.GetFileInfo(pFileDesc, out pFileInfo);
        }
        public Options GetOptions() {
            return Options;
        }
        public int GetSecurity(FileDesc pFileDesc, ref byte[] pSecurityDescriptor) {
            return CrazyFSPath.GetSecurity(pFileDesc, ref pSecurityDescriptor);
        }

        public int GetSecurityByName(string pPath, string pFileName, out uint pFileAttributes, ref byte[] pSecurityDescriptor) {
            return CrazyFSFile.GetSecurityByName(pPath, pFileName, out pFileAttributes, ref pSecurityDescriptor);
        }

        public int GetVolumeInfo(string pPath, out IVolumeInfo pVolumeInfo) {
            return FileSystem.GetVolumeInfo(pPath, out pVolumeInfo);
        }

        public int Init(FileSystemHost pHost, string pPath) {
            return FileSystem.Init(pHost, pPath);
        }
        public int Open(string pPath, string pFileName, uint pGrantedAccess, out object pFileNode, out FileDesc pFileDesc, out ICrazyFSFileInfo pFileInfo, out string pNormalizedName) {
            return CrazyFSPath.Open(pPath, pFileName, pGrantedAccess, out pFileNode, out pFileDesc, out pFileInfo, out pNormalizedName);
        }

        public int OverWrite(FileDesc pFileDesc, uint pFileAttributes, bool pReplaceFileAttributes, out ICrazyFSFileInfo pFileInfo) {
            return CrazyFSFile.OverWrite(pFileDesc, pFileAttributes, pReplaceFileAttributes, out pFileInfo);
        }
        public int Read(FileDesc pFileDesc, IntPtr pBuffer, ulong pOffset, uint pLength, out uint pBytesTransferred) {
            return CrazyFSFile.Read(pFileDesc, pBuffer, pOffset, pLength, out pBytesTransferred);
        }
        public bool ReadDirectory(object pFileNode, FileDesc pFileDesc, string pPattern, string pMarker, ref object pContext, out string pFileName, out ICrazyFSFileInfo pFileInfo) {
            return CrazyFSDirectory.ReadDirectory(pFileNode, pFileDesc, pPattern, pMarker, ref pContext, out pFileName, out pFileInfo);
        }

        public int Rename(string pOldPath, string pNewPath, bool pReplaceIfExists) {
            return CrazyFSPath.Rename(pOldPath, pNewPath);
        }

        public int SetBasicInfo(FileDesc pFileDesc, uint pFileAttributes, ulong pCreationTime, ulong pLastAccessTime, ulong pLastWriteTime, ulong pChangeTime, out ICrazyFSFileInfo pFileInfo) {
            return CrazyFSPath.SetBasicInfo(pFileDesc, pFileAttributes, pCreationTime, pLastAccessTime, pLastWriteTime, pChangeTime, out pFileInfo);
        }
        public int SetFileSize(object pFileNode, FileDesc pFileDesc, ulong pNewSize, bool pSetAllocationSize, out ICrazyFSFileInfo pFileInfo) {
            return CrazyFSFile.SetFileSize(pFileNode, pFileDesc, pNewSize, pSetAllocationSize, out pFileInfo);
        }
        public int SetSecurity(FileDesc pFileDesc, AccessControlSections pSections, byte[] pSecurityDescriptor) {
            return CrazyFSPath.SetSecurity(pFileDesc, pSections, pSecurityDescriptor);
        }
        public int Write(object pFileNode, FileDesc pFileDesc, IntPtr pBuffer, ulong pOffset, uint pLength, bool pWriteToEndOfFile, bool pConstrainedIo, out uint pBytesTransferred, out ICrazyFSFileInfo pFileInfo) {
            return CrazyFSFile.Write(pFileNode, pFileDesc, pBuffer, pOffset, pLength, pWriteToEndOfFile, pConstrainedIo, out pBytesTransferred, out pFileInfo);
        }
    }
}
