using StorageBackend;
using System;
using System.Security.AccessControl;

namespace StorageType.Passthrough {
    public class PassthroughStorage : IStorageBackend {
        private readonly string SourcePath;
        public PassthroughStorage(string pSourcePath) {
            SourcePath = pSourcePath;
        }

        public int CanDelete(IFileDescriptor pFileDesc) {
            return CrazyFSFile.CanDelete(pFileDesc);
        }
        public void Cleanup(IFileDescriptor pFileDesc, string pFileName, uint pFlags, uint pCleanupDelete) {
            CrazyFSPath.Cleanup(pFileDesc, pFlags, pCleanupDelete);
        }
        public void Close(IFileDescriptor pFileDesc) {
            CrazyFSPath.Close(pFileDesc);
        }
        public int Create(string pFileName, uint pCreateOptions, uint pGrantedAccess, uint pFileAttributes, byte[] pSecurityDescriptor, out object pFileNode, out object pFileDesc, out ICrazyFSFileInfo pFileInfo, out string pNormalizedName) {
            return CrazyFSPath.Create(SourcePath, pFileName, pCreateOptions, pGrantedAccess, pFileAttributes, pSecurityDescriptor, out pFileNode, out pFileDesc, out pFileInfo, out pNormalizedName);
        }
        public int Flush(IFileDescriptor pFileDesc, out ICrazyFSFileInfo pFileInfo) {
            return CrazyFSPath.Flush(pFileDesc, out pFileInfo);
        }
        public int GetFileInfo(IFileDescriptor pFileDesc, out ICrazyFSFileInfo pFileInfo) {
            return CrazyFSFile.GetFileInfo(pFileDesc, out pFileInfo);
        }

        public int GetSecurity(IFileDescriptor pFileDesc, ref byte[] pSecurityDescriptor) {
            return CrazyFSPath.GetSecurity(pFileDesc, ref pSecurityDescriptor);
        }

        public int GetSecurityByName(string pFileName, out uint pFileAttributes, ref byte[] pSecurityDescriptor) {
            return CrazyFSFile.GetSecurityByName(SourcePath, pFileName, out pFileAttributes, ref pSecurityDescriptor);
        }

        public int GetVolumeInfo(out IVolumeInfo pVolumeInfo) {
            return FileSystem.GetVolumeInfo(SourcePath, out pVolumeInfo);
        }

        public int Init(IFileSystemHost pHost) {
            return FileSystem.Init(pHost, SourcePath);
        }
        public int Open(string pFileName, uint pGrantedAccess, out object pFileNode, out IFileDescriptor pFileDesc, out ICrazyFSFileInfo pFileInfo, out string pNormalizedName) {
            return CrazyFSPath.Open(SourcePath, pFileName, pGrantedAccess, out pFileNode, out pFileDesc, out pFileInfo, out pNormalizedName);
        }

        public int OverWrite(IFileDescriptor pFileDesc, uint pFileAttributes, bool pReplaceFileAttributes, out ICrazyFSFileInfo pFileInfo) {
            return CrazyFSFile.OverWrite(pFileDesc, pFileAttributes, pReplaceFileAttributes, out pFileInfo);
        }
        public int Read(IFileDescriptor pFileDesc, IntPtr pBuffer, ulong pOffset, uint pLength, out uint pBytesTransferred) {
            return CrazyFSFile.Read(pFileDesc, pBuffer, pOffset, pLength, out pBytesTransferred);
        }
        public bool ReadDirectory(object pFileNode, IFileDescriptor pFileDesc, string pPattern, string pMarker, ref object pContext, out string pFileName, out ICrazyFSFileInfo pFileInfo) {
            return CrazyFSDirectory.ReadDirectory(pFileNode, pFileDesc, pPattern, pMarker, ref pContext, out pFileName, out pFileInfo);
        }

        public int Rename(string pOldPath, string pNewPath, bool pReplaceIfExists) {
            return CrazyFSPath.Rename(PathNormalizer.ConcatPath(SourcePath, pOldPath), PathNormalizer.ConcatPath(SourcePath, pNewPath));
        }

        public int SetBasicInfo(IFileDescriptor pFileDesc, uint pFileAttributes, ulong pCreationTime, ulong pLastAccessTime, ulong pLastWriteTime, ulong pChangeTime, out ICrazyFSFileInfo pFileInfo) {
            return CrazyFSPath.SetBasicInfo(pFileDesc, pFileAttributes, pCreationTime, pLastAccessTime, pLastWriteTime, pChangeTime, out pFileInfo);
        }
        public int SetFileSize(object pFileNode, IFileDescriptor pFileDesc, ulong pNewSize, bool pSetAllocationSize, out ICrazyFSFileInfo pFileInfo) {
            return CrazyFSFile.SetFileSize(pFileNode, pFileDesc, pNewSize, pSetAllocationSize, out pFileInfo);
        }
        public int SetSecurity(IFileDescriptor pFileDesc, AccessControlSections pSections, byte[] pSecurityDescriptor) {
            return CrazyFSPath.SetSecurity(pFileDesc, pSections, pSecurityDescriptor);
        }
        public int Write(object pFileNode, IFileDescriptor pFileDesc, IntPtr pBuffer, ulong pOffset, uint pLength, bool pWriteToEndOfFile, bool pConstrainedIo, out uint pBytesTransferred, out ICrazyFSFileInfo pFileInfo) {
            return CrazyFSFile.Write(pFileNode, pFileDesc, pBuffer, pOffset, pLength, pWriteToEndOfFile, pConstrainedIo, out pBytesTransferred, out pFileInfo);
        }
    }
}
