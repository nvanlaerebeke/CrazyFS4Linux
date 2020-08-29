using StorageBackend;
using System.IO;
using System.Security.AccessControl;

namespace StorageType.Passthrough {
    internal static class CrazyFSDirectory {
        internal static FileDesc Create(string pFileName, uint pFileAttributes, byte[] pSecurityDescriptor) {
            FileDesc objFileDesc = null;
            try {
                if (Directory.Exists(pFileName)) {
                    throw new NTException(FileSystemStatus.STATUS_OBJECT_NAME_COLLISION);
                }
                DirectorySecurity Security = null;
                if (null != pSecurityDescriptor) {
                    Security = new DirectorySecurity();
                    Security.SetSecurityDescriptorBinaryForm(pSecurityDescriptor);
                }
                objFileDesc = new FileDesc(Directory.CreateDirectory(pFileName));
                objFileDesc.SetFileAttributes(pFileAttributes);
                new DirectoryInfo(pFileName).SetAccessControl(Security);
                return objFileDesc;
            } catch {
                if (objFileDesc != null && objFileDesc.Stream != null) {
                    objFileDesc.Stream.Dispose();
                }
                throw;
            }
        }

        internal static FileDesc Open(string pFileName) {
            FileDesc objFileDesc = null;
            try {
                objFileDesc = new FileDesc(new DirectoryInfo(pFileName));
            } catch {
                if (objFileDesc != null && objFileDesc.Stream != null) {
                    objFileDesc.Stream.Dispose();
                }
                throw;
            }
            return objFileDesc;
        }

        internal static bool ReadDirectory(object pFileNode, IFileDescriptor pFileDesc, string pPattern, string pMarker, ref object pContext, out string pFileName, out ICrazyFSFileInfo pFileInfo) {
            return pFileDesc.ReadDirectory(pPattern, pMarker, ref pContext, out pFileName, out pFileInfo);
        }

        internal static int Move(string pOldPath, string pNewPath) {
            Directory.Move(pOldPath, pNewPath);
            return FileSystemStatus.STATUS_SUCCESS;
        }
    }
}
