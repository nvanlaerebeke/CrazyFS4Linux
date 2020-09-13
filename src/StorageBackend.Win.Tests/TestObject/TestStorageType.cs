using StorageBackend.IO;
using StorageBackend.Volume;
using System;
using System.Security.AccessControl;

namespace StorageBackend.Win.Tests.TestObject {

    internal class TestStorageType : IStorageType {

        public int CanDelete(IFSEntryPointer pFileDesc) => throw new NotImplementedException();

        public void Cleanup(IFSEntryPointer pFileDesc, string pFileName, uint pFlags, uint pCleanupDelete) => throw new NotImplementedException();

        public void Close(IFSEntryPointer pFileDesc) => throw new NotImplementedException();

        public int Create(string pFileName, uint pCreateOptions, uint pGrantedAccess, uint pFileAttributes, byte[] pSecurityDescriptor, out object pFileNode, out IFSEntryPointer pFileDesc, out IEntry pEntry, out string pNormalizedName) => throw new NotImplementedException();

        public int Flush(IFSEntryPointer pFileDesc, out IEntry pEntry) => throw new NotImplementedException();

        public int GetFileInfo(IFSEntryPointer pFileDesc, out IEntry pEntry) => throw new NotImplementedException();

        public int GetSecurity(IFSEntryPointer pFileDesc, ref byte[] pSecurityDescriptor) => throw new NotImplementedException();

        public int GetSecurityByName(string pFileName, out uint pFileAttributes, ref byte[] pSecurityDescriptor) => throw new NotImplementedException();

        public int GetVolumeInfo(out IVolumeInfo pVolumeInfo) => throw new NotImplementedException();

        public int Init(IFileSystemHost pHost) => throw new NotImplementedException();

        public int Open(string pFileName, uint pGrantedAccess, out object pFileNode, out IFSEntryPointer pFileDesc, out IEntry pEntry, out string pNormalizedName) => throw new NotImplementedException();

        public int OverWrite(IFSEntryPointer pFileDesc, uint pFileAttributes, bool pReplaceFileAttributes, out IEntry pEntry) => throw new NotImplementedException();

        public int Read(IFSEntryPointer pFileDesc, IntPtr pBuffer, ulong pOffset, uint pLength, out uint pBytesTransferred) => throw new NotImplementedException();

        public bool ReadDirectory(object pFileNode, IFSEntryPointer pFileDesc, string pPattern, string pMarker, ref object pContext, out string pFileName, out IEntry pEntry) => throw new NotImplementedException();

        public int Rename(string pOldPath, string pNewPath, bool pReplaceIfExists) => throw new NotImplementedException();

        public int SetBasicInfo(IFSEntryPointer pFileDesc, uint pFileAttributes, ulong pCreationTime, ulong pLastAccessTime, ulong pLastWriteTime, ulong pChangeTime, out IEntry pEntry) => throw new NotImplementedException();

        public int SetFileSize(object pFileNode, IFSEntryPointer pFileDesc, ulong pNewSize, bool pSetAllocationSize, out IEntry pEntry) => throw new NotImplementedException();

        public int SetSecurity(IFSEntryPointer pFileDesc, AccessControlSections pSections, byte[] pSecurityDescriptor) => throw new NotImplementedException();

        public void Setup(string pSource) {
        }

        public int Write(object pFileNode, IFSEntryPointer pFileDesc, IntPtr pBuffer, ulong pOffset, uint pLength, bool pWriteToEndOfFile, bool pConstrainedIo, out uint pBytesTransferred, out IEntry pEntry) => throw new NotImplementedException();
    }
}