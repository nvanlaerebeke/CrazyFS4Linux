using StorageBackend;
using StorageBackend.IO;
using StorageBackend.Volume;
using StorageType.Passthrough.IO;
using System;
using System.IO;
using System.Security.AccessControl;

namespace StorageType.Passthrough {

    public class PassthroughStorage : IStorageType {
        private string SourcePath;

        public void Setup(string pSourcePath) => SourcePath = pSourcePath;

        public string GetPath() => SourcePath;

        public void Cleanup(IFSEntryPointer pFileDesc, string pFileName, uint pFlags, uint pCleanupDelete) => (pFileDesc as PassthroughFile)?.Cleanup(pFlags, pCleanupDelete);

        public void Close(IFSEntryPointer pFileDesc) => (pFileDesc as PassthroughFile)?.Close();

        public int GetVolumeInfo(out IVolumeInfo pVolumeInfo) => FileSystem.GetVolumeInfo(SourcePath, out pVolumeInfo);

        public int Init(IFileSystemHost pHost) => FileSystem.Init(pHost, SourcePath);

        public int SetSecurity(IFSEntryPointer pFileDesc, AccessControlSections pSections, byte[] pSecurityDescriptor) {
            if (pFileDesc is PassthroughFileSystemBase pointer) {
                return pointer.SetSecurityDescriptor(pSections, pSecurityDescriptor);
            }
            return FileSystemStatus.STATUS_SUCCESS;
        }

        public int CanDelete(IFSEntryPointer pFileDesc) {
            (pFileDesc as PassthroughFile)?.SetDisposition(false);
            return FileSystemStatus.STATUS_SUCCESS;
        }

        public int Create(string pFileName, uint pCreateOptions, uint pGrantedAccess, uint pFileAttributes, byte[] pSecurityDescriptor, out object pFileNode, out IFSEntryPointer pFileDesc, out IEntry pEntry, out string pNormalizedName) {
            if ((pCreateOptions & FileSystemStatus.FILE_DIRECTORY_FILE) == 0) {
                pFileDesc = PassthroughFile.Create(pFileName, pGrantedAccess, pFileAttributes, pSecurityDescriptor, out pEntry);
            } else {
                pFileDesc = PassthroughDirectory.Create(pFileName, pFileAttributes, pSecurityDescriptor, out pEntry);
            }
            pFileNode = default;
            pNormalizedName = default;
            return FileSystemStatus.STATUS_SUCCESS;
        }

        public int Flush(IFSEntryPointer pFileDesc, out IEntry pEntry) {
            if (pFileDesc is PassthroughFile pointer) {
                return pointer.Flush(out pEntry);
            } else {
                pEntry = default;
                return FileSystemStatus.STATUS_SUCCESS;
            }
        }

        public int GetFileInfo(IFSEntryPointer pFileDesc, out IEntry pEntry) {
            if (pFileDesc is PassthroughFileSystemBase pointer) {
                pEntry = pointer.GetEntry();
            } else {
                pEntry = default;
            }
            return FileSystemStatus.STATUS_SUCCESS;
        }

        public int GetSecurity(IFSEntryPointer pFileDesc, ref byte[] pSecurityDescriptor) {
            if (pFileDesc is PassthroughFileSystemBase pointer) {
                pSecurityDescriptor = pointer.GetSecurityDescriptor();
            }
            return FileSystemStatus.STATUS_SUCCESS;
        }

        public int GetSecurityByName(string pFileName, out uint pFileAttributes, ref byte[] pSecurityDescriptor) {
            if (PassthroughFile.Exists(pFileName)) {
                return PassthroughFile.GetSecurityByName(PathNormalizer.ConcatPath(SourcePath, pFileName), out pFileAttributes, ref pSecurityDescriptor);
            }
            pFileAttributes = default;
            return FileSystemStatus.STATUS_SUCCESS;
        }

        public int Open(string pFileName, uint pGrantedAccess, out object pFileNode, out IFSEntryPointer pFileDesc, out IEntry pEntry, out string pNormalizedName) {
            var FullPath = PathNormalizer.ConcatPath(SourcePath, pFileName);
            if (PassthroughDirectory.Exists(FullPath)) {
                pFileDesc = PassthroughDirectory.Open(FullPath, out pEntry);
            } else {
                pFileDesc = PassthroughFile.Open(FullPath, pGrantedAccess, out pEntry);
            }
            pNormalizedName = default;
            pFileNode = default;
            return FileSystemStatus.STATUS_SUCCESS;
        }

        public int OverWrite(IFSEntryPointer pFileDesc, uint pFileAttributes, bool pReplaceFileAttributes, out IEntry pEntry) {
            if (pFileDesc is PassthroughFile pointer) {
                return pointer.OverWrite(pFileAttributes, pReplaceFileAttributes, out pEntry);
            } else {
                pEntry = default;
            }
            return FileSystemStatus.STATUS_SUCCESS;
        }

        public int Read(IFSEntryPointer pFileDesc, IntPtr pBuffer, ulong pOffset, uint pLength, out uint pBytesTransferred) {
            if (pFileDesc is PassthroughFile pointer) {
                return pointer.Read(pBuffer, pOffset, pLength, out pBytesTransferred);
            } else {
                pBytesTransferred = 0;
            }
            return FileSystemStatus.STATUS_SUCCESS;
        }

        public bool ReadDirectory(object pFileNode, IFSEntryPointer pFileDesc, string pPattern, string pMarker, ref object pContext, out string pFileName, out IEntry pEntry) {
            pFileName = default;
            pEntry = default;
            if (pFileDesc is PassthroughDirectory pointer) {
                return pointer.ReadDirectory(pPattern, pMarker, ref pContext, out pFileName, out pEntry);
            }
            return false;
        }

        public int Rename(string pOldPath, string pNewPath, bool pReplaceIfExists) {
            if (PassthroughFile.Exists(pOldPath)) {
                return PassthroughFile.Move(pOldPath, pNewPath);
            } else if (PassthroughDirectory.Exists(pOldPath)) {
                return PassthroughDirectory.Move(pOldPath, pNewPath);
            } else {
                throw new FileNotFoundException();
            }
        }

        public int SetBasicInfo(IFSEntryPointer pFileDesc, uint pFileAttributes, ulong pCreationTime, ulong pLastAccessTime, ulong pLastWriteTime, ulong pChangeTime, out IEntry pEntry) {
            if (pFileDesc is PassthroughFileSystemBase pointer) {
                return pointer.SetBasicInfo(pFileAttributes, pCreationTime, pLastAccessTime, pLastWriteTime, out pEntry);
            } else {
                pEntry = default;
            }
            return FileSystemStatus.STATUS_SUCCESS;
        }

        public int SetFileSize(object pFileNode, IFSEntryPointer pFileDesc, ulong pNewSize, bool pSetAllocationSize, out IEntry pEntry) {
            if (pFileDesc is PassthroughFile pointer) {
                return pointer.SetFileSize(pNewSize, pSetAllocationSize, out pEntry);
            }
            pEntry = default;
            return FileSystemStatus.STATUS_SUCCESS;
        }

        public int Write(object pFileNode, IFSEntryPointer pFileDesc, IntPtr pBuffer, ulong pOffset, uint pLength, bool pWriteToEndOfFile, bool pConstrainedIo, out uint pBytesTransferred, out IEntry pEntry) {
            if (pFileDesc is PassthroughFile pointer) {
                return pointer.Write(pBuffer, pOffset, pLength, pWriteToEndOfFile, pConstrainedIo, out pBytesTransferred, out pEntry);
            } else {
                pEntry = default;
                pBytesTransferred = 0;
                return FileSystemStatus.STATUS_SUCCESS;
            }
        }

        public int GetCreationTimeUtc() => throw new NotImplementedException();
    }
}