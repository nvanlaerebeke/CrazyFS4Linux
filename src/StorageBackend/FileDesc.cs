using System;
using System.Collections;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using FileInfo = Fsp.Interop.FileInfo;

namespace StorageBackend {
    public class FileDesc {
        public FileStream Stream;
        public DirectoryInfo DirInfo;
        public DictionaryEntry[] FileSystemInfos;

        public FileDesc(FileStream Stream) {
            this.Stream = Stream;
        }
        public FileDesc(DirectoryInfo DirInfo) {
            this.DirInfo = DirInfo;
        }
        public static void GetFileInfoFromFileSystemInfo(FileSystemInfo Info, out FileInfo FileInfo) {
            FileInfo.FileAttributes = (uint)Info.Attributes;
            FileInfo.ReparseTag = 0;
            FileInfo.FileSize = Info is System.IO.FileInfo ? (ulong)((System.IO.FileInfo)Info).Length : 0;
            FileInfo.AllocationSize = (FileInfo.FileSize + 4096 - 1) / 4096 * 4096;
            FileInfo.CreationTime = (ulong)Info.CreationTimeUtc.ToFileTimeUtc();
            FileInfo.LastAccessTime = (ulong)Info.LastAccessTimeUtc.ToFileTimeUtc();
            FileInfo.LastWriteTime = (ulong)Info.LastWriteTimeUtc.ToFileTimeUtc();
            FileInfo.ChangeTime = FileInfo.LastWriteTime;
            FileInfo.IndexNumber = 0;
            FileInfo.HardLinks = 0;
        }
        public int GetFileInfo(out FileInfo FileInfo) {
            if (null != Stream) {
                if (!GetFileInformationByHandle(Stream.SafeFileHandle.DangerousGetHandle(), out var Info)) {
                    ExceptionGenerator.ThrowIoExceptionWithWin32(Marshal.GetLastWin32Error());
                }
                FileInfo.FileAttributes = Info.dwFileAttributes;
                FileInfo.ReparseTag = 0;
                FileInfo.FileSize = (ulong)Stream.Length;
                FileInfo.AllocationSize = (FileInfo.FileSize + 4096 - 1) / 4096 * 4096;
                FileInfo.CreationTime = Info.ftCreationTime;
                FileInfo.LastAccessTime = Info.ftLastAccessTime;
                FileInfo.LastWriteTime = Info.ftLastWriteTime;
                FileInfo.ChangeTime = FileInfo.LastWriteTime;
                FileInfo.IndexNumber = 0;
                FileInfo.HardLinks = 0;
            } else
                GetFileInfoFromFileSystemInfo(DirInfo, out FileInfo);
            return FileSystemStatus.STATUS_SUCCESS;
        }

        public static void GetCrazyFSFileInfoFromFileSystemInfo(FileSystemInfo Info, out ICrazyFSFileInfo pFileInfo) {
            pFileInfo = new CrazyFSFileInfo(Info);
        }

        public int GetCrazyFSFileInfo(out ICrazyFSFileInfo pFileInfo) {
            if (null != Stream) {
                pFileInfo = new CrazyFSFileInfo(new System.IO.FileInfo(Stream.Name));
            } else {
                pFileInfo = new CrazyFSFileInfo(DirInfo);
            }
            return FileSystemStatus.STATUS_SUCCESS;
        }

        public void SetBasicInfo(
            uint FileAttributes,
            ulong CreationTime,
            ulong LastAccessTime,
            ulong LastWriteTime) {
            if (0 == FileAttributes)
                FileAttributes = (uint)System.IO.FileAttributes.Normal;
            if (null != Stream) {
                FILE_BASIC_INFO Info = default(FILE_BASIC_INFO);
                if (unchecked((uint)(-1)) != FileAttributes)
                    Info.FileAttributes = FileAttributes;
                if (0 != CreationTime)
                    Info.CreationTime = CreationTime;
                if (0 != LastAccessTime)
                    Info.LastAccessTime = LastAccessTime;
                if (0 != LastWriteTime)
                    Info.LastWriteTime = LastWriteTime;
                if (!SetFileInformationByHandle(Stream.SafeFileHandle.DangerousGetHandle(),
                    0/*FileBasicInfo*/, ref Info, (uint)Marshal.SizeOf(Info)))
                    ExceptionGenerator.ThrowIoExceptionWithWin32(Marshal.GetLastWin32Error());
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
            GetFileInfo(out FileInfo FileInfo);
            return FileInfo.FileAttributes;
        }
        public void SetFileAttributes(uint FileAttributes) {
            SetBasicInfo(FileAttributes, 0, 0, 0);
        }
        public byte[] GetSecurityDescriptor() {
            if (null != Stream)
                return Stream.GetAccessControl().GetSecurityDescriptorBinaryForm();
            else
                return DirInfo.GetAccessControl().GetSecurityDescriptorBinaryForm();
        }
        public void SetSecurityDescriptor(AccessControlSections Sections, byte[] SecurityDescriptor) {
            int SecurityInformation = 0;
            if (0 != (Sections & AccessControlSections.Owner))
                SecurityInformation |= 1/*OWNER_SECURITY_INFORMATION*/;
            if (0 != (Sections & AccessControlSections.Group))
                SecurityInformation |= 2/*GROUP_SECURITY_INFORMATION*/;
            if (0 != (Sections & AccessControlSections.Access))
                SecurityInformation |= 4/*DACL_SECURITY_INFORMATION*/;
            if (0 != (Sections & AccessControlSections.Audit))
                SecurityInformation |= 8/*SACL_SECURITY_INFORMATION*/;
            if (null != Stream) {
                if (!SetKernelObjectSecurity(Stream.SafeFileHandle.DangerousGetHandle(),
                    SecurityInformation, SecurityDescriptor))
                    ExceptionGenerator.ThrowIoExceptionWithWin32(Marshal.GetLastWin32Error());
            } else {
                if (!SetFileSecurityW(DirInfo.FullName,
                    SecurityInformation, SecurityDescriptor))
                    ExceptionGenerator.ThrowIoExceptionWithWin32(Marshal.GetLastWin32Error());
            }
        }
        public void SetDisposition(bool Safe) {
            if (null != Stream) {
                FILE_DISPOSITION_INFO Info;
                Info.DeleteFile = true;
                if (!SetFileInformationByHandle(Stream.SafeFileHandle.DangerousGetHandle(), 4/*FileDispositionInfo*/, ref Info, (uint)Marshal.SizeOf(Info)))
                    if (!Safe) {
                        ExceptionGenerator.ThrowIoExceptionWithWin32(Marshal.GetLastWin32Error());
                    }
            } else
                try {
                    DirInfo.Delete();
                } catch (Exception ex) {
                    if (!Safe) {
                        ExceptionGenerator.ThrowIoExceptionWithHResult(ex.HResult);
                    }
                }
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
    }
}
