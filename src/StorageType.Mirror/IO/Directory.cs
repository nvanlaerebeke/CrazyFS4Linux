using System;
using System.Collections;

namespace StorageType.Passthrough.IO {

    internal class Directory : PassthroughFileSystemBase {
        private static readonly DirectoryEntryComparer _DirectoryEntryComparer = new DirectoryEntryComparer();
        private readonly IDirectoryInfo DirectoryInfo;
        private DictionaryEntry[] FileSystemInfos;

        public Directory(IDirectoryInfo pDirectoryInfo) => DirectoryInfo = pDirectoryInfo;

        public override int SetBasicInfo(uint Attributes, ulong CreationTime, ulong LastAccessTime, ulong LastWriteTime, out IEntry pEntry) {
            if (Attributes == 0) {
                Attributes = (uint)System.IO.FileAttributes.Directory;
            }
            if (unchecked((uint)-1) != Attributes) {
                DirectoryInfo.Attributes = Attributes;
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

        public override int SetSecurityDescriptor(AccessControlSections Sections, byte[] SecurityDescriptor) {
            DirectoryInfo.SetAccessControl(new DirectorySecurity(DirectoryInfo.FullName, Sections));
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
                pEntry = new Entry((FileSystemInfoBase)FileSystemInfos[Index].Value);
                return true;
            } else {
                pFileName = default;
                pEntry = default;
                return false;
            }
        }

        public override IEntry GetEntry() => new Entry(DirectoryInfo);
    }
}