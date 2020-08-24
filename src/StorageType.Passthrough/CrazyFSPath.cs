using StorageBackend;
using System.IO;
using System.Security.AccessControl;

namespace StorageType.Passthrough {
    internal static class CrazyFSPath {

        internal static int Create(string pPath, string pFileName, uint pCreateOptions, uint pGrantedAccess, uint pFileAttributes, byte[] pSecurityDescriptor, out object pFileNode, out object pFileDesc0, out Fsp.Interop.FileInfo pFileInfo, out string pNormalizedName) {
            pFileName = PathNormalizer.ConcatPath(pPath, pFileName);
            FileDesc objFileDesc;
            if (0 == (pCreateOptions & FileSystemStatus.FILE_DIRECTORY_FILE)) {
                objFileDesc = CrazyFSFile.Create(pFileName, pGrantedAccess, pFileAttributes, pSecurityDescriptor);
            } else {
                objFileDesc = CrazyFSDirectory.Create(pFileName, pFileAttributes, pSecurityDescriptor);
            }
            pFileNode = default;
            pFileDesc0 = objFileDesc;
            pNormalizedName = default;
            return objFileDesc.GetFileInfo(out pFileInfo);
        }

        internal static string Normalize(string pPath) {
            _ = Path.GetFullPath(pPath);
            return (pPath.EndsWith("\\")) ? pPath.Substring(0, pPath.Length - 1) : pPath;
        }

        internal static int Open(string pPath, string pFileName, uint pGrantedAccess, out object pFileNode, out FileDesc pFileDesc, out Fsp.Interop.FileInfo pFileInfo, out string pNormalizedName) {
            pFileName = PathNormalizer.ConcatPath(pPath, pFileName);
            var objFileDesc = (!Directory.Exists(pFileName)) ? CrazyFSFile.Open(pFileName, pGrantedAccess) : CrazyFSDirectory.Open(pFileName);
            pFileNode = default;
            pFileDesc = objFileDesc;
            pNormalizedName = default;
            return objFileDesc.GetFileInfo(out pFileInfo);
        }

        internal static void Cleanup(FileDesc pFileDesc, uint pFlags, uint pCleanupDelete) {
            if (0 != (pFlags & pCleanupDelete)) {
                pFileDesc.SetDisposition(true);
                if (null != pFileDesc.Stream) {
                    pFileDesc.Stream.Dispose();
                }
            }
        }

        internal static void Close(FileDesc pFileDesc) {
            if (null != pFileDesc.Stream) {
                pFileDesc.Stream.Dispose();
            }
        }

        internal static int SetSecurity(FileDesc pFileDesc, AccessControlSections pSections, byte[] pSecurityDescriptor) {
            pFileDesc.SetSecurityDescriptor(pSections, pSecurityDescriptor);
            return FileSystemStatus.STATUS_SUCCESS;
        }

        internal static int GetSecurity(FileDesc pFileDesc, ref byte[] pSecurityDescriptor) {
            pSecurityDescriptor = pFileDesc.GetSecurityDescriptor();
            return FileSystemStatus.STATUS_SUCCESS;
        }

        internal static int Flush(object pFileNode, FileDesc pFileDesc, out Fsp.Interop.FileInfo pFileInfo) {
            if (null == pFileDesc) {
                /* we do not flush the whole volume, so just return SUCCESS */
                pFileInfo = default(Fsp.Interop.FileInfo);
                return FileSystemStatus.STATUS_SUCCESS;
            }
            pFileDesc.Stream.Flush(true);
            return pFileDesc.GetFileInfo(out pFileInfo);
        }

        internal static int SetBasicInfo(FileDesc pFileDesc, uint pFileAttributes, ulong pCreationTime, ulong pLastAccessTime, ulong pLastWriteTime, ulong pChangeTime, out Fsp.Interop.FileInfo pFileInfo) {
            pFileDesc.SetBasicInfo(pFileAttributes, pCreationTime, pLastAccessTime, pLastWriteTime);
            return pFileDesc.GetFileInfo(out pFileInfo);
        }

        internal static int Rename(string pOldPath, string pNewPath) {
            FileAttributes attr = File.GetAttributes(pOldPath);
            if ((attr & FileAttributes.Directory) == FileAttributes.Directory) {
                return CrazyFSDirectory.Move(pOldPath, pNewPath);
            } else {
                return CrazyFSFile.Move(pOldPath, pNewPath);
            }
        }
    }
}
