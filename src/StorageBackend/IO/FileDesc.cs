using System;
using System.Collections;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.AccessControl;

namespace StorageBackend {
    public class FileDesc : IFileDescriptor {
        private static readonly DirectoryEntryComparer _DirectoryEntryComparer = new DirectoryEntryComparer();

        public FileStream Stream;
        public DirectoryInfo DirInfo;
        public DictionaryEntry[] FileSystemInfos;

        public FileDesc(FileStream Stream) {
            this.Stream = Stream;
        }
        public FileDesc(DirectoryInfo DirInfo) {
            this.DirInfo = DirInfo;
        }

        public static void GetCrazyFSFileInfoFromFileSystemInfo(FileSystemInfo Info, out ICrazyFSFileInfo pFileInfo) {
            pFileInfo = new CrazyFSFileInfo(Info);
        }

        public bool GetCrazyFSFileInfo(out ICrazyFSFileInfo pFileInfo) {
            if (Stream != null) {
                pFileInfo = new CrazyFSFileInfo(new FileInfo(Stream.Name));
            } else {
                pFileInfo = new CrazyFSFileInfo(DirInfo);
            }
            return true;
        }

        public void SetBasicInfo(
            uint FileAttributes,
            ulong CreationTime,
            ulong LastAccessTime,
            ulong LastWriteTime
        ) {
            if (FileAttributes == 0) {
                FileAttributes = (uint)System.IO.FileAttributes.Normal;
            }
            if (Stream != null) {
                FILE_BASIC_INFO Info = default;
                if (unchecked((uint)(-1)) != FileAttributes)
                    Info.FileAttributes = FileAttributes;
                if (0 != CreationTime)
                    Info.CreationTime = CreationTime;
                if (0 != LastAccessTime)
                    Info.LastAccessTime = LastAccessTime;
                if (0 != LastWriteTime)
                    Info.LastWriteTime = LastWriteTime;
                if (!SetFileInformationByHandle(Stream.SafeFileHandle.DangerousGetHandle(), 0/*FileBasicInfo*/, ref Info, (uint)Marshal.SizeOf(Info))) {
                    throw new Win32Exception(Marshal.GetLastWin32Error());
                }
            } else {
                if (unchecked((uint)(-1)) != FileAttributes)
                    DirInfo.Attributes = (FileAttributes)FileAttributes;
                if (0 != CreationTime)
                    DirInfo.CreationTimeUtc = DateTime.FromFileTimeUtc((long)CreationTime);
                if (0 != LastAccessTime)
                    DirInfo.LastAccessTimeUtc = DateTime.FromFileTimeUtc((long)LastAccessTime);
                if (0 != LastWriteTime)
                    DirInfo.LastWriteTimeUtc = DateTime.FromFileTimeUtc((long)LastWriteTime);
            }
        }
        public uint GetFileAttributes() {
            _ = GetCrazyFSFileInfo(out ICrazyFSFileInfo FileInfo);
            return FileInfo.FileAttributes;
        }
        public void SetFileAttributes(uint FileAttributes) {
            SetBasicInfo(FileAttributes, 0, 0, 0);
        }
        public byte[] GetSecurityDescriptor() {
            if (Stream != null) {
                return Stream.GetAccessControl().GetSecurityDescriptorBinaryForm();
            } else {
                return DirInfo.GetAccessControl().GetSecurityDescriptorBinaryForm();
            }
        }
        public void SetSecurityDescriptor(AccessControlSections Sections, byte[] SecurityDescriptor) {
            var SecurityInformation = 0;
            if (0 != (Sections & AccessControlSections.Owner))
                SecurityInformation |= 1/*OWNER_SECURITY_INFORMATION*/;
            if (0 != (Sections & AccessControlSections.Group))
                SecurityInformation |= 2/*GROUP_SECURITY_INFORMATION*/;
            if (0 != (Sections & AccessControlSections.Access))
                SecurityInformation |= 4/*DACL_SECURITY_INFORMATION*/;
            if (0 != (Sections & AccessControlSections.Audit))
                SecurityInformation |= 8/*SACL_SECURITY_INFORMATION*/;
            if (Stream != null) {
                if (!SetKernelObjectSecurity(Stream.SafeFileHandle.DangerousGetHandle(), SecurityInformation, SecurityDescriptor)) {
                    throw new Win32Exception(Marshal.GetLastWin32Error());
                }
            } else {
                if (!SetFileSecurityW(DirInfo.FullName, SecurityInformation, SecurityDescriptor)) {
                    throw new Win32Exception(Marshal.GetLastWin32Error());
                }
            }
        }
        public void SetDisposition(bool Safe) {
            if (Stream != null) {
                FILE_DISPOSITION_INFO Info;
                Info.DeleteFile = true;
                if (!SetFileInformationByHandle(Stream.SafeFileHandle.DangerousGetHandle(), 4/*FileDispositionInfo*/, ref Info, (uint)Marshal.SizeOf(Info)) && !Safe) {
                    throw new Win32Exception(Marshal.GetLastWin32Error());
                }
            } else {
                try {
                    DirInfo.Delete();
                } catch (Exception ex) {
                    if (!Safe) {
                        throw new IOException(null, ex.HResult);
                    }
                }
            }
        }

        public void Cleanup(uint pFlags, uint pCleanupDelete) {
            if ((pFlags & pCleanupDelete) != 0) {
                SetDisposition(true);
                Stream?.Dispose();
            }
        }

        public void Close() {
            Stream?.Dispose();
        }

        public void Flush() {
            Stream?.Flush(true);
        }

        /* interop */
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        private struct BY_HANDLE_FILE_INFORMATION {
            public uint dwFileAttributes;
            public ulong ftCreationTime;
            public ulong ftLastAccessTime;
            public ulong ftLastWriteTime;
            public uint dwVolumeSerialNumber;
            public uint nFileSizeHigh;
            public uint nFileSizeLow;
            public uint nNumberOfLinks;
            public uint nFileIndexHigh;
            public uint nFileIndexLow;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct FILE_BASIC_INFO {
            public ulong CreationTime;
            public ulong LastAccessTime;
            public ulong LastWriteTime;
            public ulong ChangeTime;
            public uint FileAttributes;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct FILE_DISPOSITION_INFO {
            public bool DeleteFile;
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool GetFileInformationByHandle(IntPtr hFile, out BY_HANDLE_FILE_INFORMATION lpFileInformation);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool SetFileInformationByHandle(IntPtr hFile, int FileInformationClass, ref FILE_BASIC_INFO lpFileInformation, uint dwBufferSize);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool SetFileInformationByHandle(IntPtr hFile, int FileInformationClass, ref FILE_DISPOSITION_INFO lpFileInformation, uint dwBufferSize);

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool SetFileSecurityW([MarshalAs(UnmanagedType.LPWStr)] string FileName, int SecurityInformation, byte[] SecurityDescriptor);
        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool SetKernelObjectSecurity(IntPtr Handle, int SecurityInformation, byte[] SecurityDescriptor);
        public bool ReadDirectory(string pPattern, string pMarker, ref object pContext, out string pFileName, out ICrazyFSFileInfo pFileInfo) {
            if (FileSystemInfos == null) {
                if (pPattern != null) {
                    pPattern = pPattern.Replace('<', '*').Replace('>', '?').Replace('"', '.');
                } else {
                    pPattern = "*";
                }
                var Enum = DirInfo.EnumerateFileSystemInfos(pPattern);
                var lstItems = new SortedList();
                if (DirInfo != null && null != DirInfo.Parent) {
                    lstItems.Add(".", DirInfo);
                    lstItems.Add("..", DirInfo.Parent);
                }
                foreach (FileSystemInfo Info in Enum) {
                    lstItems.Add(Info.Name, Info);
                }
                FileSystemInfos = new DictionaryEntry[lstItems.Count];
                lstItems.CopyTo(FileSystemInfos, 0);
            }
            int Index;
            if (pContext == null) {
                Index = 0;
                if (null != pMarker) {
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
                GetCrazyFSFileInfoFromFileSystemInfo((FileSystemInfo)FileSystemInfos[Index].Value, out pFileInfo);
                return true;
            } else {
                pFileName = default;
                pFileInfo = default;
                return false;
            }
        }

        public FileStream GetStream() {
            return Stream;
        }

        /// <summary>
        /// Does not work, gives exception for some reason
        /// </summary>
        /*public int Read(IntPtr pBuffer, ulong pOffset, uint pLength, out uint pBytesTransferred) {
            var FileDesc = this;
            if (pOffset >= (ulong)FileDesc.Stream.Length)
                ExceptionGenerator.ThrowIoExceptionWithNtStatus(FileSystemStatus.STATUS_END_OF_FILE);
            byte[] Bytes = new byte[pLength];
            FileDesc.Stream.Seek((long)pOffset, SeekOrigin.Begin);
            pBytesTransferred = (uint)FileDesc.Stream.Read(Bytes, 0, Bytes.Length);
            Marshal.Copy(Bytes, 0, pBuffer, Bytes.Length);
            return FileSystemStatus.STATUS_SUCCESS;
        }*/

        public int Write(object pFileNode, IntPtr pBuffer, ulong pOffset, uint pLength, bool pWriteToEndOfFile, bool pConstrainedIo, out uint pBytesTransferred, out ICrazyFSFileInfo pFileInfo) {
            if (pConstrainedIo) {
                if (pOffset >= (ulong)Stream.Length) {
                    pBytesTransferred = default;
                    pFileInfo = default;
                    return FileSystemStatus.STATUS_SUCCESS;
                }
                if (pOffset + pLength > (ulong)Stream.Length) {
                    pLength = (uint)((ulong)Stream.Length - pOffset);
                }
            }
            byte[] Bytes = new byte[pLength];
            Marshal.Copy(pBuffer, Bytes, 0, Bytes.Length);
            if (!pWriteToEndOfFile) {
                _ = Stream.Seek((long)pOffset, SeekOrigin.Begin);
            }
            Stream.Write(Bytes, 0, Bytes.Length);
            pBytesTransferred = (uint)Bytes.Length;
            _ = GetCrazyFSFileInfo(out pFileInfo);
            return FileSystemStatus.STATUS_SUCCESS;
        }

        public int SetFileSize(ulong pNewSize, bool pSetAllocationSize, out ICrazyFSFileInfo pFileInfo) {
            if (!pSetAllocationSize || (ulong)Stream.Length > pNewSize) {
                /*
                 * "FileInfo.FileSize > NewSize" explanation:
                 * Ptfs does not support allocation size. However if the new AllocationSize
                 * is less than the current FileSize we must truncate the file.
                 */
                Stream.SetLength((long)pNewSize);
            }
            _ = GetCrazyFSFileInfo(out pFileInfo);
            return FileSystemStatus.STATUS_SUCCESS;
        }

        public int OverWrite(uint pFileAttributes, bool pReplaceFileAttributes, out ICrazyFSFileInfo pFileInfo) {
            if (pReplaceFileAttributes) {
                SetFileAttributes(pFileAttributes | (uint)FileAttributes.Archive);
            } else if (0 != pFileAttributes) {
                SetFileAttributes(GetFileAttributes() | pFileAttributes | (uint)FileAttributes.Archive);
            }
            Stream.SetLength(0);
            _ = GetCrazyFSFileInfo(out pFileInfo);
            return FileSystemStatus.STATUS_SUCCESS;
        }
    }
}
