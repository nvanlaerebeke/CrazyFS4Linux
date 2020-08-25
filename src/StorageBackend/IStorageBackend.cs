using Fsp;
using System;
using System.Security.AccessControl;

namespace StorageBackend {
    public interface IStorageBackend {
        Options GetOptions();
        int Init(FileSystemHost pHost, string pPath);
        int GetVolumeInfo(string pPath, out IVolumeInfo pVolumeInfo);
        int GetSecurityByName(string pPath, string pFileName, out uint pFileAttributes /* or ReparsePointIndex */, ref byte[] pSecurityDescriptor);
        int Create(string pPath, string pFileName, uint pCreateOptions, uint pGrantedAccess, uint pFileAttributes, byte[] pSecurityDescriptor, out object pFileNode, out object pFileDesc, out ICrazyFSFileInfo pFileInfo, out string pNormalizedName);
        int Open(string pPath, string pFileName, uint pGrantedAccess, out object pFileNode, out FileDesc pFileDesc, out ICrazyFSFileInfo pFileInfo, out string pNormalizedName);
        int OverWrite(FileDesc pFileDesc, uint pFileAttributes, bool pReplaceFileAttributes, out ICrazyFSFileInfo pFileInfo);
        void Cleanup(FileDesc pFileDesc, string pFileName, uint pFlags, uint pCleanupDelete);
        void Close(FileDesc pFileDesc);
        int Read(FileDesc pFileDesc, IntPtr pBuffer, ulong pOffset, uint pLength, out uint pBytesTransferred);
        int Write(object pFileNode, FileDesc pFileDesc, IntPtr pBuffer, ulong pOffset, uint pLength, bool pWriteToEndOfFile, bool pConstrainedIo, out uint pBytesTransferred, out ICrazyFSFileInfo pFileInfo);
        int Flush(object pFileNode, FileDesc pFileDesc, out ICrazyFSFileInfo pFileInfo);
        int GetFileInfo(FileDesc pFileDesc, out ICrazyFSFileInfo pFileInfo);
        int SetBasicInfo(FileDesc pFileDesc, uint pFileAttributes, ulong pCreationTime, ulong pLastAccessTime, ulong pLastWriteTime, ulong pChangeTime, out ICrazyFSFileInfo pFileInfo);
        int SetFileSize(object pFileNode, FileDesc pFileDesc, ulong pNewSize, bool pSetAllocationSize, out ICrazyFSFileInfo pFileInfo);
        int CanDelete(FileDesc pFileDesc);
        int Rename(string pOldPath, string pNewPath, bool pReplaceIfExists);
        int GetSecurity(FileDesc pFileDesc, ref byte[] pSecurityDescriptor);
        int SetSecurity(FileDesc pFileDesc, AccessControlSections pSections, byte[] pSecurityDescriptor);
        bool ReadDirectory(object pFileNode, FileDesc pFileDesc, string pPattern, string pMarker, ref object pContext, out string pFileName, out ICrazyFSFileInfo pFileInfo);
    }
}
