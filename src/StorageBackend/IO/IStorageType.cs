using StorageBackend.IO;
using StorageBackend.Volume;
using System.IO;

namespace StorageBackend {

    public interface IStorageType {
        IVolume VolumeManager { get; }

        void Setup(string pSource);

        //Result GetSecurityByName(string pFileName, out FileAttributes pFileAttributes /* or ReparsePointIndex */, ref byte[] pSecurityDescriptor);

        Result Create(string pFileName, bool pIsFile, FileAccess pFileAccess, FileShare pShare, FileMode pMode, FileOptions pOptions, FileAttributes pAttributes, out IFSEntryPointer pNode);

        //Result Open(string pFileName, uint pGrantedAccess, out object pFileNode, out IFSEntryPointer pFileDesc, out IEntry pEntry, out string pNormalizedName);

        //Result OverWrite(IFSEntryPointer pFileDesc, FileAttributes pFileAttributes, bool pReplaceFileAttributes, out IEntry pEntry);

        //void Cleanup(IFSEntryPointer pFileDesc, bool pCleanupDelete);

        //void Close(IFSEntryPointer pFileDesc);

        //Result Read(IFSEntryPointer pFileDesc, out byte[] pBuffer, long pOffset, int pLength, out int pBytesTransferred);

        //Result Write(IFSEntryPointer pFileDesc, byte[] pBuffer, long pOffset, out int pBytesWritten);

        //Result Delete(IFSEntryPointer iFSEntryPointer, bool pRecursive);

        //Result Flush(IFSEntryPointer pFileDesc);

        IFSEntryPointer GetFileInfo(string filename);

        //Result GetFileInfo(string pPath, out IFSEntryPointer pFileDesc);

        //Result SetBasicInfo(IFSEntryPointer pFileDesc, FileAttributes pFileAttributes, DateTime pCreationTime, DateTime pLastAccessTime, DateTime pLastWriteTime, DateTime pChangeTime);

        //Result SetFileSize(IFSEntryPointer pFileDesc, long pNewSize);

        // Result CanDelete(IFSEntryPointer pFileDesc);

        //Result Rename(string pOldPath, string pNewPath, bool pReplaceIfExists);

        // Result GetSecurity(IFSEntryPointer pEntry, out FileSystemSecurity pFileSystemSecurity);

        //Result SetSecurity(IFSEntryPointer pFileDesc, AccessControlSections pSections, byte[] pSecurityDescriptor);

        // Result ReadDirectory(IFSEntryPointer pEntry, string pPattern, bool pCaseSensitive, string pMarker, out IFSEntryPointer[] pEntries);

        // Result Lock(IFSEntryPointer iFSEntryPointer, long offset, long length);

        // Result SetSecurity(IFSEntryPointer iFSEntryPointer, FileSystemSecurity security);

        // Result UnLock(IFSEntryPointer iFSEntryPointer, long offset, long length);

        // Result SetAllocationSize(string fileName, long length, IFSEntryPointer info);

        Result Move(string oldpath, string newpath, bool replace);

        Result DeleteDirectory(IFSDirectory iFSDirectory);
    }
}