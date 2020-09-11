using StorageBackend;
using StorageBackend.IO;
using System;
using System.Collections;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.AccessControl;

namespace StorageType.Passthrough.IO {

    internal class PassthroughDirectory : PassthroughFileSystemBase {
        private static readonly DirectoryEntryComparer _DirectoryEntryComparer = new DirectoryEntryComparer();
        private readonly DirectoryInfo DirectoryInfo;
        private DictionaryEntry[] FileSystemInfos;

        public PassthroughDirectory(DirectoryInfo pDirectoryInfo) => DirectoryInfo = pDirectoryInfo;

        public static int Move(string pOldPath, string pNewPath) {
            new DirectoryInfo(pOldPath).MoveTo(pNewPath);
            return FileSystemStatus.STATUS_SUCCESS;
        }

        public static PassthroughDirectory Open(string pPath, out IEntry pEntry) {
            if (!Directory.Exists(pPath)) {
                throw new DirectoryNotFoundException($"{pPath} not found");
            }
            var d = new PassthroughDirectory(new DirectoryInfo(pPath));
            pEntry = d.GetEntry();
            return d;
        }

        public override int SetBasicInfo(uint FileAttributes, ulong CreationTime, ulong LastAccessTime, ulong LastWriteTime, out IEntry pEntry) {
            if (FileAttributes == 0) {
                FileAttributes = (uint)System.IO.FileAttributes.Normal;
            }
            if (unchecked((uint)-1) != FileAttributes) {
                DirectoryInfo.Attributes = (FileAttributes)FileAttributes;
            }
            if (CreationTime != 0) {
                DirectoryInfo.CreationTimeUtc = DateTime.FromFileTimeUtc((long)CreationTime);
            }
            if (LastAccessTime != 0) {
                DirectoryInfo.LastAccessTimeUtc = DateTime.FromFileTimeUtc((long)LastAccessTime);
            }
            if (LastWriteTime != 0) {
                DirectoryInfo.LastWriteTimeUtc = DateTime.FromFileTimeUtc((long)LastWriteTime);
            }
            pEntry = GetEntry();
            return FileSystemStatus.STATUS_SUCCESS;
        }

        public override byte[] GetSecurityDescriptor() => DirectoryInfo.GetAccessControl().GetSecurityDescriptorBinaryForm();

        public void SetDisposition(bool Safe) {
            try {
                DirectoryInfo.Delete();
            } catch (Exception ex) {
                if (!Safe) {
                    throw new IOException(null, ex.HResult);
                }
            }
        }

        public override int SetSecurityDescriptor(AccessControlSections Sections, byte[] SecurityDescriptor) {
            var SecurityInformation = 0;
            if (0 != (Sections & AccessControlSections.Owner)) {
                SecurityInformation |= 1/*OWNER_SECURITY_INFORMATION*/;
            }
            if (0 != (Sections & AccessControlSections.Group)) {
                SecurityInformation |= 2/*GROUP_SECURITY_INFORMATION*/;
            }
            if (0 != (Sections & AccessControlSections.Access)) {
                SecurityInformation |= 4/*DACL_SECURITY_INFORMATION*/;
            }
            if (0 != (Sections & AccessControlSections.Audit)) {
                SecurityInformation |= 8/*SACL_SECURITY_INFORMATION*/;
            }

            if (!SetFileSecurityW(DirectoryInfo.FullName, SecurityInformation, SecurityDescriptor)) {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }
            return FileSystemStatus.STATUS_SUCCESS;
        }

        public bool ReadDirectory(string pPattern, string pMarker, ref object pContext, out string pFileName, out IEntry pEntry) {
            if (FileSystemInfos == null) {
                if (pPattern != null) {
                    pPattern = pPattern.Replace('<', '*').Replace('>', '?').Replace('"', '.');
                } else {
                    pPattern = "*";
                }
                var Enum = DirectoryInfo.EnumerateFileSystemInfos(pPattern);
                var lstItems = new SortedList();
                if (DirectoryInfo?.Parent != null) {
                    lstItems.Add(".", DirectoryInfo);
                    lstItems.Add("..", DirectoryInfo.Parent);
                }
                foreach (var Info in Enum) {
                    lstItems.Add(Info.Name, Info);
                }
                FileSystemInfos = new DictionaryEntry[lstItems.Count];
                lstItems.CopyTo(FileSystemInfos, 0);
            }
            int Index;
            if (pContext == null) {
                Index = 0;
                if (pMarker != null) {
                    Index = Array.BinarySearch(FileSystemInfos, new DictionaryEntry(pMarker, null), _DirectoryEntryComparer);
                    if (0 <= Index) {
                        Index++;
                    } else {
                        Index = ~Index;
                    }
                }
            } else {
                Index = (int)pContext;
            }

            if (FileSystemInfos.Length > Index) {
                pContext = Index + 1;
                pFileName = (string)FileSystemInfos[Index].Key;
                pEntry = new PassthroughEntry((FileSystemInfo)FileSystemInfos[Index].Value);
                return true;
            } else {
                pFileName = default;
                pEntry = default;
                return false;
            }
        }

        public static bool Exists(string pPath) => Directory.Exists(pPath);

        public override IEntry GetEntry() => new PassthroughEntry(DirectoryInfo);

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool SetFileSecurityW([MarshalAs(UnmanagedType.LPWStr)] string FileName, int SecurityInformation, byte[] SecurityDescriptor);

        internal static PassthroughDirectory Create(string pFileName, uint pFileAttributes, byte[] pSecurityDescriptor, out IEntry pEntry) {
            if (Directory.Exists(pFileName)) {
                throw new NTException(FileSystemStatus.STATUS_OBJECT_NAME_COLLISION);
            }
            DirectorySecurity Security = null;
            if (pSecurityDescriptor != null) {
                Security = new DirectorySecurity();
                Security.SetSecurityDescriptorBinaryForm(pSecurityDescriptor);
            }
            var objFileDesc = new PassthroughDirectory(Directory.CreateDirectory(pFileName));
            _ = objFileDesc.SetBasicInfo(pFileAttributes, 0, 0, 0, out pEntry);
            new DirectoryInfo(pFileName).SetAccessControl(Security);
            return objFileDesc;
        }
    }
}