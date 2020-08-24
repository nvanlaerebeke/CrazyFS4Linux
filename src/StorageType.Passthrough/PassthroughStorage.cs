using Fsp;
using Fsp.Interop;
using StorageBackend;
using System;
using System.Security.AccessControl;
using FileInfo = Fsp.Interop.FileInfo;

namespace StorageType.Passthrough {
    public class PassthroughStorage : IStorageBackend {
        private readonly Options Options;
        //private readonly IFileSystemHost Host;

        public PassthroughStorage(Options pOptions) {
            Options = pOptions;
            FileSystemHost.SetDebugLogFile(Options.LogFile);
        }

        /*public IFileSystemHost GetFileSystemHost() {
            return Host;
        }*/

        public int CanDelete(FileDesc pFileDesc) {
            return CrazyFSFile.CanDelete(pFileDesc);
        }
        public void Cleanup(FileDesc pFileDesc, string pFileName, uint pFlags, uint pCleanupDelete) {
            CrazyFSPath.Cleanup(pFileDesc, pFlags, pCleanupDelete);
        }
        public void Close(FileDesc pFileDesc) {
            CrazyFSPath.Close(pFileDesc);
        }
        public int Create(string pPath, string pFileName, uint pCreateOptions, uint pGrantedAccess, uint pFileAttributes, byte[] pSecurityDescriptor, out object pFileNode, out object pFileDesc, out FileInfo pFileInfo, out string pNormalizedName) {
            return CrazyFSPath.Create(pPath, pFileName, pCreateOptions, pGrantedAccess, pFileAttributes, pSecurityDescriptor, out pFileNode, out pFileDesc, out pFileInfo, out pNormalizedName);
        }
        public int Flush(object pFileNode, FileDesc pFileDesc, out FileInfo pFileInfo) {
            return CrazyFSPath.Flush(pFileNode, pFileDesc, out pFileInfo);
        }
        public int GetFileInfo(FileDesc pFileDesc, out FileInfo pFileInfo) {
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

        public VolumeInfo GetVolumeInfo(string pPath) {
            return FileSystem.GetVolumeInfo(pPath);
        }

        public int Init(FileSystemHost pHost, string pPath) {
            return FileSystem.Init(pHost, pPath);
        }
        public int Open(string pPath, string pFileName, uint pGrantedAccess, out object pFileNode, out FileDesc pFileDesc, out FileInfo pFileInfo, out string pNormalizedName) {
            return CrazyFSPath.Open(pPath, pFileName, pGrantedAccess, out pFileNode, out pFileDesc, out pFileInfo, out pNormalizedName);
        }

        public int OverWrite(FileDesc pFileDesc, uint pFileAttributes, bool pReplaceFileAttributes, out FileInfo pFileInfo) {
            return CrazyFSFile.OverWrite(pFileDesc, pFileAttributes, pReplaceFileAttributes, out pFileInfo);
        }
        public int Read(FileDesc pFileDesc, IntPtr pBuffer, ulong pOffset, uint pLength, out uint pBytesTransferred) {
            return CrazyFSFile.Read(pFileDesc, pBuffer, pOffset, pLength, out pBytesTransferred);
        }
        public bool ReadDirectory(object pFileNode, FileDesc pFileDesc, string pPattern, string pMarker, ref object pContext, out string pFileName, out FileInfo pFileInfo) {
            return CrazyFSDirectory.ReadDirectory(pFileNode, pFileDesc, pPattern, pMarker, ref pContext, out pFileName, out pFileInfo);
        }

        public int Rename(string pOldPath, string pNewPath, bool pReplaceIfExists) {
            return CrazyFSPath.Rename(pOldPath, pNewPath);
        }

        public int SetBasicInfo(FileDesc pFileDesc, uint pFileAttributes, ulong pCreationTime, ulong pLastAccessTime, ulong pLastWriteTime, ulong pChangeTime, out FileInfo pFileInfo) {
            return CrazyFSPath.SetBasicInfo(pFileDesc, pFileAttributes, pCreationTime, pLastAccessTime, pLastWriteTime, pChangeTime, out pFileInfo);
        }
        public int SetFileSize(object pFileNode, FileDesc pFileDesc, ulong pNewSize, bool pSetAllocationSize, out FileInfo pFileInfo) {
            return CrazyFSFile.SetFileSize(pFileNode, pFileDesc, pNewSize, pSetAllocationSize, out pFileInfo);
        }
        public int SetSecurity(FileDesc pFileDesc, AccessControlSections pSections, byte[] pSecurityDescriptor) {
            return CrazyFSPath.SetSecurity(pFileDesc, pSections, pSecurityDescriptor);
        }
        public int Write(object pFileNode, FileDesc pFileDesc, IntPtr pBuffer, ulong pOffset, uint pLength, bool pWriteToEndOfFile, bool pConstrainedIo, out uint pBytesTransferred, out FileInfo pFileInfo) {
            return CrazyFSFile.Write(pFileNode, pFileDesc, pBuffer, pOffset, pLength, pWriteToEndOfFile, pConstrainedIo, out pBytesTransferred, out pFileInfo);
        }
    }
}
