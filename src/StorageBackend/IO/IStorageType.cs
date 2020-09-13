using StorageBackend.IO;
using System;
using System.Security.AccessControl;

namespace StorageBackend {

    public interface IStorageType {

        void Setup(string pSource);

        int GetSecurityByName(string pFileName, out uint pFileAttributes /* or ReparsePointIndex */, ref byte[] pSecurityDescriptor);

        int Create(string pFileName, uint pCreateOptions, uint pGrantedAccess, uint pFileAttributes, byte[] pSecurityDescriptor, out object pFileNode, out IFSEntryPointer pFileDesc, out IEntry pEntry, out string pNormalizedName);

        int Open(string pFileName, uint pGrantedAccess, out object pFileNode, out IFSEntryPointer pFileDesc, out IEntry pEntry, out string pNormalizedName);

        int OverWrite(IFSEntryPointer pFileDesc, uint pFileAttributes, bool pReplaceFileAttributes, out IEntry pEntry);

        void Cleanup(IFSEntryPointer pFileDesc, string pFileName, uint pFlags, uint pCleanupDelete);

        void Close(IFSEntryPointer pFileDesc);

        int Read(IFSEntryPointer pFileDesc, IntPtr pBuffer, ulong pOffset, uint pLength, out uint pBytesTransferred);

        int Write(object pFileNode, IFSEntryPointer pFileDesc, IntPtr pBuffer, ulong pOffset, uint pLength, bool pWriteToEndOfFile, bool pConstrainedIo, out uint pBytesTransferred, out IEntry pEntry);

        int Flush(IFSEntryPointer pFileDesc, out IEntry pEntry);

        int GetFileInfo(IFSEntryPointer pFileDesc, out IEntry pEntry);

        int SetBasicInfo(IFSEntryPointer pFileDesc, uint pFileAttributes, ulong pCreationTime, ulong pLastAccessTime, ulong pLastWriteTime, ulong pChangeTime, out IEntry pEntry);

        int SetFileSize(object pFileNode, IFSEntryPointer pFileDesc, ulong pNewSize, bool pSetAllocationSize, out IEntry pEntry);

        int CanDelete(IFSEntryPointer pFileDesc);

        int Rename(string pOldPath, string pNewPath, bool pReplaceIfExists);

        int GetSecurity(IFSEntryPointer pFileDesc, ref byte[] pSecurityDescriptor);

        int SetSecurity(IFSEntryPointer pFileDesc, AccessControlSections pSections, byte[] pSecurityDescriptor);

        bool ReadDirectory(object pFileNode, IFSEntryPointer pFileDesc, string pPattern, string pMarker, ref object pContext, out string pFileName, out IEntry pEntry);
    }
}