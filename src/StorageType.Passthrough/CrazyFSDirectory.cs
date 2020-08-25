using StorageBackend;
using System;
using System.Collections;
using System.IO;
using System.Security.AccessControl;

namespace StorageType.Passthrough {
    internal static class CrazyFSDirectory {
        private static readonly DirectoryEntryComparer _DirectoryEntryComparer = new DirectoryEntryComparer();

        internal static FileDesc Create(string pFileName, uint pFileAttributes, byte[] pSecurityDescriptor) {
            FileDesc objFileDesc = null;
            try {
                if (Directory.Exists(pFileName)) {
                    ExceptionGenerator.ThrowIoExceptionWithNtStatus(FileSystemStatus.STATUS_OBJECT_NAME_COLLISION);
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

        internal static bool ReadDirectory(object pFileNode, FileDesc pFileDesc, string pPattern, string pMarker, ref object pContext, out string pFileName, out ICrazyFSFileInfo pFileInfo) {
            if (null == pFileDesc.FileSystemInfos) {
                if (null != pPattern) {
                    pPattern = pPattern.Replace('<', '*').Replace('>', '?').Replace('"', '.');
                } else {
                    pPattern = "*";
                }
                var Enum = pFileDesc.DirInfo.EnumerateFileSystemInfos(pPattern);
                var lstItems = new SortedList();
                if (null != pFileDesc.DirInfo && null != pFileDesc.DirInfo.Parent) {
                    lstItems.Add(".", pFileDesc.DirInfo);
                    lstItems.Add("..", pFileDesc.DirInfo.Parent);
                }
                foreach (FileSystemInfo Info in Enum) {
                    lstItems.Add(Info.Name, Info);
                }
                pFileDesc.FileSystemInfos = new DictionaryEntry[lstItems.Count];
                lstItems.CopyTo(pFileDesc.FileSystemInfos, 0);
            }
            int Index;
            if (null == pContext) {
                Index = 0;
                if (null != pMarker) {
                    Index = Array.BinarySearch(pFileDesc.FileSystemInfos, new DictionaryEntry(pMarker, null), _DirectoryEntryComparer);
                    if (0 <= Index) {
                        Index++;
                    } else {
                        Index = ~Index;
                    }
                }
            } else
                Index = (int)pContext;
            if (pFileDesc.FileSystemInfos.Length > Index) {
                pContext = Index + 1;
                pFileName = (string)pFileDesc.FileSystemInfos[Index].Key;
                FileDesc.GetCrazyFSFileInfoFromFileSystemInfo((FileSystemInfo)pFileDesc.FileSystemInfos[Index].Value, out pFileInfo);
                return true;
            } else {
                pFileName = default;
                pFileInfo = default;
                return false;
            }
        }

        internal static int Move(string pOldPath, string pNewPath) {
            Directory.Move(pOldPath, pNewPath);
            return FileSystemStatus.STATUS_SUCCESS;
        }
    }
}
