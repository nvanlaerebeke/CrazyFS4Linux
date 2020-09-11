using StorageBackend;
using StorageBackend.IO;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.AccessControl;

namespace StorageType.Passthrough.IO {

    internal partial class PassthroughFile : PassthroughFileSystemBase {
        private readonly FileStream Stream;

        public PassthroughFile(FileStream pStream) => Stream = pStream;

        public override int SetBasicInfo(uint FileAttributes, ulong CreationTime, ulong LastAccessTime, ulong LastWriteTime, out IEntry pEntry) {
            if (FileAttributes == 0) {
                FileAttributes = (uint)System.IO.FileAttributes.Normal;
            }

            FILE_BASIC_INFO Info = default;
            if (unchecked((uint)-1) != FileAttributes)
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
            pEntry = GetEntry();
            return FileSystemStatus.STATUS_SUCCESS;
        }

        public override byte[] GetSecurityDescriptor() => Stream.GetAccessControl().GetSecurityDescriptorBinaryForm();

        public void SetDisposition(bool Safe) {
            FILE_DISPOSITION_INFO Info;
            Info.DeleteFile = true;
            if (!SetFileInformationByHandle(Stream.SafeFileHandle.DangerousGetHandle(), 4/*FileDispositionInfo*/, ref Info, (uint)Marshal.SizeOf(Info)) && !Safe) {
                throw new Win32Exception(Marshal.GetLastWin32Error());
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
            if (!SetKernelObjectSecurity(Stream.SafeFileHandle.DangerousGetHandle(), SecurityInformation, SecurityDescriptor)) {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }
            return FileSystemStatus.STATUS_SUCCESS;
        }

        public static int GetSecurityByName(string pPath, out uint pFileAttributes, ref byte[] pSecurityDescriptor) {
            var Info = new FileInfo(pPath);
            pFileAttributes = (uint)Info.Attributes;
            if (pSecurityDescriptor != null && Info.Exists) {
                pSecurityDescriptor = Info.GetAccessControl().GetSecurityDescriptorBinaryForm();
            }
            return FileSystemStatus.STATUS_SUCCESS;
        }

        public int SetFileSize(ulong pNewSize, bool pSetAllocationSize, out IEntry pEntry) {
            if (!pSetAllocationSize || (ulong)Stream.Length > pNewSize) {
                /*
                 * "FileInfo.FileSize > NewSize" explanation:
                 * Ptfs does not support allocation size. However if the new AllocationSize
                 * is less than the current FileSize we must truncate the file.
                 */
                Stream.SetLength((long)pNewSize);
            }
            pEntry = GetEntry();
            return FileSystemStatus.STATUS_SUCCESS;
        }

        public int OverWrite(uint pFileAttributes, bool pReplaceFileAttributes, out IEntry pEntry) {
            pEntry = new PassthroughEntry(new FileInfo(Stream.Name));
            if (pReplaceFileAttributes) {
                _ = SetBasicInfo(pFileAttributes | (uint)FileAttributes.Archive, 0, 0, 0, out pEntry);
            } else if (pFileAttributes != 0) {
                _ = SetBasicInfo(pEntry.Attributes | pFileAttributes | (uint)FileAttributes.Archive, 0, 0, 0, out pEntry);
            }
            Stream.SetLength(0);
            return FileSystemStatus.STATUS_SUCCESS;
        }

        public static bool Exists(string pPath) => File.Exists(pPath);

        public override IEntry GetEntry() => new PassthroughEntry(new FileInfo(Stream.Name));

        [StructLayout(LayoutKind.Sequential)]
        private struct FILE_BASIC_INFO {
            public ulong CreationTime;
            public ulong LastAccessTime;
            public ulong LastWriteTime;
            public ulong ChangeTime;
            public uint FileAttributes;
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool SetFileInformationByHandle(IntPtr hFile, int FileInformationClass, ref FILE_BASIC_INFO lpFileInformation, uint dwBufferSize);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool SetFileInformationByHandle(IntPtr hFile, int FileInformationClass, ref FILE_DISPOSITION_INFO lpFileInformation, uint dwBufferSize);

        [StructLayout(LayoutKind.Sequential)]
        private struct FILE_DISPOSITION_INFO {
            public bool DeleteFile;
        }

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool SetKernelObjectSecurity(IntPtr Handle, int SecurityInformation, byte[] SecurityDescriptor);
    }
}