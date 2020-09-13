using StorageBackend;
using StorageBackend.IO;
using System;
using System.IO;
using System.IO.Abstractions;
using System.Security.AccessControl;

namespace StorageType.Passthrough.IO {

    internal partial class PassthroughFile : PassthroughFileSystemBase {
        private readonly IFileInfo Info;
        private readonly IFileSystem FileSystem;
        private Stream Stream;

        public PassthroughFile(IFileInfo pInfo) : this(pInfo, new FileSystem()) {
        }

        public PassthroughFile(IFileInfo pInfo, IFileSystem pFileSystem) {
            Info = pInfo;
            FileSystem = pFileSystem;
        }

        public void Open(FileMode pFileMode, FileAccess pFileAccess, FileShare pFileShare) {
            if (Stream != null) { Close(); }
            Stream = FileSystem.File.Open(Info.FullName, pFileMode, pFileAccess, pFileShare);
        }

        public void Close() {
            Stream?.Close();
            Stream?.Dispose();
            Stream = null;
        }

        public override int SetBasicInfo(uint FileAttributes, ulong CreationTime, ulong LastAccessTime, ulong LastWriteTime, out IEntry pEntry) {
            if (FileAttributes == 0) {
                FileSystem.File.SetAttributes(Info.FullName, System.IO.FileAttributes.Normal);
            } else {
                FileSystem.File.SetAttributes(Info.FullName, (System.IO.FileAttributes)FileAttributes);
            }
            if (CreationTime != 0) {
                FileSystem.File.SetCreationTimeUtc(Info.FullName, DateTime.FromFileTimeUtc((long)CreationTime));
            }
            if (LastAccessTime != 0) {
                FileSystem.File.SetLastAccessTimeUtc(Info.FullName, DateTime.FromFileTimeUtc((long)LastAccessTime));
            }
            if (LastWriteTime != 0) {
                FileSystem.File.SetLastWriteTimeUtc(Info.FullName, DateTime.FromFileTimeUtc((long)LastWriteTime));
            }
            pEntry = GetEntry();
            return FileSystemStatus.STATUS_SUCCESS;
        }

        internal void CanDelete(out int Status) => Status = FileSystemStatus.STATUS_SUCCESS;

        public override byte[] GetSecurityDescriptor() => FileSystem.File.GetAccessControl(Info.FullName).GetSecurityDescriptorBinaryForm();

        public override int SetSecurityDescriptor(AccessControlSections Sections, byte[] SecurityDescriptor) {
            Info.SetAccessControl(new FileSecurity(Info.FullName, Sections));
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
            pEntry = new PassthroughEntry(Info);
            if (pReplaceFileAttributes) {
                _ = SetBasicInfo(pFileAttributes | (uint)FileAttributes.Archive, 0, 0, 0, out pEntry);
            } else if (pFileAttributes != 0) {
                _ = SetBasicInfo(pEntry.Attributes | pFileAttributes | (uint)FileAttributes.Archive, 0, 0, 0, out pEntry);
            }
            Stream.SetLength(0);
            return FileSystemStatus.STATUS_SUCCESS;
        }

        public override IEntry GetEntry() => new PassthroughEntry(Info);

        internal void GetSecurityByName(out uint pFileAttributes, ref byte[] pSecurityDescriptor) {
            pFileAttributes = (uint)Info.Attributes;
            if (pSecurityDescriptor != null && Info.Exists) {
                pSecurityDescriptor = Info.GetAccessControl().GetSecurityDescriptorBinaryForm();
            }
        }
    }
}