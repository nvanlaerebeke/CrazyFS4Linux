using StorageBackend.IO;
using StorageBackend.Volume;
using System;
using System.IO;
using System.Security.AccessControl;

namespace StorageBackend.Win.Tests.TestObject {

    internal class TestStorageType : IStorageType {
        public IVolume VolumeManager => throw new NotImplementedException();

        public Result CanDelete(IFSEntryPointer pFileDesc) => throw new NotImplementedException();

        public void Cleanup(IFSEntryPointer pFileDesc, bool pCleanupDelete) => throw new NotImplementedException();

        public void Close(IFSEntryPointer pFileDesc) => throw new NotImplementedException();

        public Result Create(string pFileName, bool pIsFile, FileAccess pFileAccess, FileShare pShare, FileMode pMode, FileOptions pOptions, FileAttributes pFileAttributes, out IFSEntryPointer pNode) => throw new NotImplementedException();

        public Result Delete(IFSEntryPointer pEntry, bool pRecursive) => throw new NotImplementedException();

        public Result Flush(IFSEntryPointer pFileDesc) => throw new NotImplementedException();

        public Result GetFileInfo(IFSEntryPointer pFileDesc, out IEntry pEntry) => throw new NotImplementedException();

        public Result GetFileInfo(string pPath, out IFSEntryPointer pFileDesc) => throw new NotImplementedException();

        public Result GetSecurity(IFSEntryPointer pFileDesc, out FileSystemSecurity pSecurity) => throw new NotImplementedException();

        public Result GetSecurityByName(string pFileName, out FileAttributes pFileAttributes, ref byte[] pSecurityDescriptor) => throw new NotImplementedException();

        public Result GetVolumeInfo(out IVolumeInfo pVolumeInfo) => throw new NotImplementedException();

        public Result Init(IFileSystemHost pHost) => throw new NotImplementedException();

        public Result Lock(IFSEntryPointer iFSEntryPointer, long offset, long length) => throw new NotImplementedException();

        public Result Open(string pFileName, uint pGrantedAccess, out object pFileNode, out IFSEntryPointer pFileDesc, out IEntry pEntry, out string pNormalizedName) => throw new NotImplementedException();

        public Result OverWrite(IFSEntryPointer pFileDesc, FileAttributes pFileAttributes, bool pReplaceFileAttributes, out IEntry pEntry) => throw new NotImplementedException();

        public Result Read(IFSEntryPointer pEntry, out byte[] pBuffer, long pOffset, int pLength, out int pBytesTransferred) => throw new NotImplementedException();

        public Result ReadDirectory(IFSEntryPointer pFileDesc, string pPattern, bool pCaseSensitive, string pMarker, out IFSEntryPointer[] pEntries) => throw new NotImplementedException();

        public Result Rename(string pOldPath, string pNewPath, bool pReplaceIfExists) => throw new NotImplementedException();

        public Result SetAllocationSize(string fileName, long length, IFSEntryPointer info) => throw new NotImplementedException();

        public Result SetBasicInfo(IFSEntryPointer pFileDesc, FileAttributes pFileAttributes, DateTime pCreationTime, DateTime pLastAccessTime, DateTime pLastWriteTime, DateTime pChangeTime) => throw new NotImplementedException();

        public Result SetFileSize(IFSEntryPointer pEntry, long pNewSize) => throw new NotImplementedException();

        public Result SetSecurity(IFSEntryPointer pFileDesc, AccessControlSections pSections, byte[] pSecurityDescriptor) => throw new NotImplementedException();

        public Result SetSecurity(IFSEntryPointer iFSEntryPointer, FileSystemSecurity security) => throw new NotImplementedException();

        public void Setup(string pSource) {
        }

        public Result UnLock(IFSEntryPointer iFSEntryPointer, long offset, long length) => throw new NotImplementedException();

        public Result Write(IFSEntryPointer pFileDesc, byte[] pBuffer, long pOffset, out int pBytesTransferred) => throw new NotImplementedException();
    }
}