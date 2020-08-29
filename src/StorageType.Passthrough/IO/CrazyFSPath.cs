using StorageBackend;
using System.IO;
using System.Security.AccessControl;

namespace StorageType.Passthrough {
    internal static class CrazyFSPath {

        internal static int Create(string pPath, string pFileName, uint pCreateOptions, uint pGrantedAccess, uint pFileAttributes, byte[] pSecurityDescriptor, out object pFileNode, out object pFileDesc0, out ICrazyFSFileInfo pFileInfo, out string pNormalizedName) {
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
            _ = objFileDesc.GetCrazyFSFileInfo(out pFileInfo);
            return FileSystemStatus.STATUS_SUCCESS;
        }

        internal static string Normalize(string pPath) {
            _ = Path.GetFullPath(pPath);
            return (pPath.EndsWith("\\")) ? pPath.Substring(0, pPath.Length - 1) : pPath;
        }

        internal static int Open(string pPath, string pFileName, uint pGrantedAccess, out object pFileNode, out IFileDescriptor pFileDesc, out ICrazyFSFileInfo pFileInfo, out string pNormalizedName) {
            pFileName = PathNormalizer.ConcatPath(pPath, pFileName);
            var objFileDesc = (!Directory.Exists(pFileName)) ? CrazyFSFile.Open(pFileName, pGrantedAccess) : CrazyFSDirectory.Open(pFileName);
            pFileNode = default;
            pFileDesc = objFileDesc;
            pNormalizedName = default;
            _ = objFileDesc.GetCrazyFSFileInfo(out pFileInfo);
            return FileSystemStatus.STATUS_SUCCESS;
        }

        internal static void Cleanup(IFileDescriptor pFileDesc, uint pFlags, uint pCleanupDelete) {
            pFileDesc.Cleanup(pFlags, pCleanupDelete);
        }

        internal static void Close(IFileDescriptor pFileDesc) {
            pFileDesc.Close();
        }

        internal static int SetSecurity(IFileDescriptor pFileDesc, AccessControlSections pSections, byte[] pSecurityDescriptor) {
            pFileDesc.SetSecurityDescriptor(pSections, pSecurityDescriptor);
            return FileSystemStatus.STATUS_SUCCESS;
        }

        internal static int GetSecurity(IFileDescriptor pFileDesc, ref byte[] pSecurityDescriptor) {
            pSecurityDescriptor = pFileDesc.GetSecurityDescriptor();
            return FileSystemStatus.STATUS_SUCCESS;
        }

        internal static int Flush(IFileDescriptor pFileDesc, out ICrazyFSFileInfo pFileInfo) {
            /* we do not flush the whole volume, so just return SUCCESS */
            if (pFileDesc != null) {
                pFileInfo = default;
                return FileSystemStatus.STATUS_SUCCESS;
            }
            pFileDesc.Flush();
            _ = pFileDesc.GetCrazyFSFileInfo(out pFileInfo);
            return FileSystemStatus.STATUS_SUCCESS;
        }

        internal static int SetBasicInfo(IFileDescriptor pFileDesc, uint pFileAttributes, ulong pCreationTime, ulong pLastAccessTime, ulong pLastWriteTime, ulong pChangeTime, out ICrazyFSFileInfo pFileInfo) {
            pFileDesc.SetBasicInfo(pFileAttributes, pCreationTime, pLastAccessTime, pLastWriteTime);
            _ = pFileDesc.GetCrazyFSFileInfo(out pFileInfo);
            return FileSystemStatus.STATUS_SUCCESS;
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
