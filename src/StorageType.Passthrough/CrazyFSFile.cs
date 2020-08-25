using StorageBackend;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Security.Principal;

namespace StorageType.Passthrough {
    internal static class CrazyFSFile {
        public static int Move(string pOldPath, string pNewPath) {
            File.Move(pOldPath, pNewPath);
            return FileSystemStatus.STATUS_SUCCESS;
        }

        internal static int GetSecurityByName(string pPath, string pFileName, out uint pFileAttributes, ref byte[] pSecurityDescriptor) {
            pFileName = PathNormalizer.ConcatPath(pPath, pFileName);
            var Info = new System.IO.FileInfo(pFileName);
            pFileAttributes = (uint)Info.Attributes;
            if (null != pSecurityDescriptor && Info.Exists) {
                pSecurityDescriptor = Info.GetAccessControl().GetSecurityDescriptorBinaryForm();
            }
            return FileSystemStatus.STATUS_SUCCESS;
        }

        /// <summary>
        /// ToDo: SetSecurity was removed for .net standard support
        /// </summary>
        internal static FileDesc Create(string pFileName, uint pGrantedAccess, uint pFileAttributes, byte[] pSecurityDescriptor) {
            FileDesc objFileDesc = null;
            try {
                objFileDesc = new FileDesc(
                    new FileStream(pFileName, FileMode.CreateNew, FileAccess.ReadWrite, FileShare.Read | FileShare.Write | FileShare.Delete, 4096, FileOptions.SequentialScan)
                );
                //objFileDesc.SetSecurity((FileSystemRights)pGrantedAccess | FileSystemRights.WriteAttributes, pSecurityDescriptor);
                objFileDesc.SetFileAttributes(pFileAttributes | (uint)FileAttributes.Archive);
                return objFileDesc;
            } catch {
                if (objFileDesc != null && objFileDesc.Stream != null) {
                    objFileDesc.Stream.Dispose();
                }
                throw;
            }
        }

        internal static FileDesc Open(string pFileName, uint pGrantedAccess) {
            FileDesc objFileDesc = null;
            try {
                //SetSecurity(pFileName, (FileSystemRights)pGrantedAccess);
                objFileDesc = new FileDesc(
                    new FileStream(pFileName, FileMode.Open, FileAccess.ReadWrite, FileShare.Read | FileShare.Write | FileShare.Delete, 4096, false)
                );
            } catch (System.IO.FileNotFoundException) {
                //File not found can be safely ignored
                //this will let w/e is doing the listing know the file/dir does not exist
                throw;
            } catch {
                if (null != objFileDesc && null != objFileDesc.Stream) {
                    objFileDesc.Stream.Dispose();
                }
                throw;
            }
            return objFileDesc;
        }

        internal static int Read(FileDesc pFileDesc, IntPtr pBuffer, ulong pOffset, uint pLength, out uint pBytesTransferred) {
            if (pOffset >= (ulong)pFileDesc.Stream.Length) {
                ExceptionGenerator.ThrowIoExceptionWithNtStatus(FileSystemStatus.STATUS_END_OF_FILE);
            }
            byte[] Bytes = new byte[pLength];
            pFileDesc.Stream.Seek((long)pOffset, SeekOrigin.Begin);
            pBytesTransferred = (uint)pFileDesc.Stream.Read(Bytes, 0, Bytes.Length);
            Marshal.Copy(Bytes, 0, pBuffer, Bytes.Length);
            return FileSystemStatus.STATUS_SUCCESS;
        }

        internal static int GetFileInfo(FileDesc pFileDesc, out ICrazyFSFileInfo pFileInfo) {
            return pFileDesc.GetCrazyFSFileInfo(out pFileInfo);
        }

        internal static int Write(object pFileNode, FileDesc pFileDesc, IntPtr pBuffer, ulong pOffset, uint pLength, bool pWriteToEndOfFile, bool pConstrainedIo, out uint pBytesTransferred, out ICrazyFSFileInfo pFileInfo) {
            if (pConstrainedIo) {
                if (pOffset >= (ulong)pFileDesc.Stream.Length) {
                    pBytesTransferred = default;
                    pFileInfo = default;
                    return FileSystemStatus.STATUS_SUCCESS;
                }
                if (pOffset + pLength > (ulong)pFileDesc.Stream.Length) {
                    pLength = (uint)((ulong)pFileDesc.Stream.Length - pOffset);
                }
            }
            byte[] Bytes = new byte[pLength];
            Marshal.Copy(pBuffer, Bytes, 0, Bytes.Length);
            if (!pWriteToEndOfFile) {
                pFileDesc.Stream.Seek((long)pOffset, SeekOrigin.Begin);
            }
            pFileDesc.Stream.Write(Bytes, 0, Bytes.Length);
            pBytesTransferred = (uint)Bytes.Length;
            return pFileDesc.GetCrazyFSFileInfo(out pFileInfo);
        }

        internal static int SetFileSize(object pFileNode, FileDesc pFileDesc, ulong pNewSize, bool pSetAllocationSize, out ICrazyFSFileInfo pFileInfo) {
            if (!pSetAllocationSize || (ulong)pFileDesc.Stream.Length > pNewSize) {
                /*
                 * "FileInfo.FileSize > NewSize" explanation:
                 * Ptfs does not support allocation size. However if the new AllocationSize
                 * is less than the current FileSize we must truncate the file.
                 */
                pFileDesc.Stream.SetLength((long)pNewSize);
            }
            return pFileDesc.GetCrazyFSFileInfo(out pFileInfo);
        }

        internal static int OverWrite(FileDesc pFileDesc, uint pFileAttributes, bool pReplaceFileAttributes, out ICrazyFSFileInfo pFileInfo) {
            FileDesc objFileDesc = pFileDesc;
            if (pReplaceFileAttributes) {
                objFileDesc.SetFileAttributes(pFileAttributes | (uint)FileAttributes.Archive);
            } else if (0 != pFileAttributes) {
                objFileDesc.SetFileAttributes(objFileDesc.GetFileAttributes() | pFileAttributes | (uint)System.IO.FileAttributes.Archive);
            }
            objFileDesc.Stream.SetLength(0);
            return objFileDesc.GetCrazyFSFileInfo(out pFileInfo);
        }

        internal static int CanDelete(FileDesc pFileDesc) {
            pFileDesc.SetDisposition(false);
            return FileSystemStatus.STATUS_SUCCESS;
        }

        private static void SetSecurity(string pFileName, FileSystemRights pFileSystemRights, byte[] pSecurityDescriptor = null) {
            var i = new FileInfo(pFileName);

            FileSecurity Security = null;
            if (pSecurityDescriptor != null) {
                Security = new FileSecurity();
                Security.SetSecurityDescriptorBinaryForm(pSecurityDescriptor);
            } else {
                Security = i.GetAccessControl();
            }

            Security.AddAccessRule(
                new FileSystemAccessRule(
                    new SecurityIdentifier(WellKnownSidType.AuthenticatedUserSid, null),
                    pFileSystemRights,
                    AccessControlType.Allow
                )
            );
            i.SetAccessControl(Security);
        }
    }
}