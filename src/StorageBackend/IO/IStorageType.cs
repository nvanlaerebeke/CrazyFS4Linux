using System;
using System.Security.AccessControl;

namespace StorageBackend {
    public interface IStorageType {
        int Init(IFileSystemHost pHost);
        int GetVolumeInfo(out IVolumeInfo pVolumeInfo);
        int GetSecurityByName(string pFileName, out uint pFileAttributes /* or ReparsePointIndex */, ref byte[] pSecurityDescriptor);
        int Create(string pFileName, uint pCreateOptions, uint pGrantedAccess, uint pFileAttributes, byte[] pSecurityDescriptor, out object pFileNode, out object pFileDesc, out ICrazyFSFileInfo pFileInfo, out string pNormalizedName);
        int Open(string pFileName, uint pGrantedAccess, out object pFileNode, out IFileDescriptor pFileDesc, out ICrazyFSFileInfo pFileInfo, out string pNormalizedName);
        int OverWrite(IFileDescriptor pFileDesc, uint pFileAttributes, bool pReplaceFileAttributes, out ICrazyFSFileInfo pFileInfo);
        void Cleanup(IFileDescriptor pFileDesc, string pFileName, uint pFlags, uint pCleanupDelete);
        void Close(IFileDescriptor pFileDesc);
        int Read(IFileDescriptor pFileDesc, IntPtr pBuffer, ulong pOffset, uint pLength, out uint pBytesTransferred);
        int Write(object pFileNode, IFileDescriptor pFileDesc, IntPtr pBuffer, ulong pOffset, uint pLength, bool pWriteToEndOfFile, bool pConstrainedIo, out uint pBytesTransferred, out ICrazyFSFileInfo pFileInfo);
        int Flush(IFileDescriptor pFileDesc, out ICrazyFSFileInfo pFileInfo);
        int GetFileInfo(IFileDescriptor pFileDesc, out ICrazyFSFileInfo pFileInfo);
        int SetBasicInfo(IFileDescriptor pFileDesc, uint pFileAttributes, ulong pCreationTime, ulong pLastAccessTime, ulong pLastWriteTime, ulong pChangeTime, out ICrazyFSFileInfo pFileInfo);
        int SetFileSize(object pFileNode, IFileDescriptor pFileDesc, ulong pNewSize, bool pSetAllocationSize, out ICrazyFSFileInfo pFileInfo);
        int CanDelete(IFileDescriptor pFileDesc);
        int Rename(string pOldPath, string pNewPath, bool pReplaceIfExists);
        int GetSecurity(IFileDescriptor pFileDesc, ref byte[] pSecurityDescriptor);
        int SetSecurity(IFileDescriptor pFileDesc, AccessControlSections pSections, byte[] pSecurityDescriptor);
        bool ReadDirectory(object pFileNode, IFileDescriptor pFileDesc, string pPattern, string pMarker, ref object pContext, out string pFileName, out ICrazyFSFileInfo pFileInfo);
    }
}
