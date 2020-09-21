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

        public override Result SetBasicInfo(FileAttributes FileAttributes, DateTime CreationTime, DateTime LastAccessTime, DateTime LastWriteTime) {
            if (FileAttributes == 0) {
                FileSystem.File.SetAttributes(Info.FullName, FileAttributes.Normal);
            } else {
                FileSystem.File.SetAttributes(Info.FullName, FileAttributes);
            }
            if (CreationTime != default) {
                FileSystem.File.SetCreationTimeUtc(Info.FullName, CreationTime);
            }
            if (LastAccessTime != default) {
                FileSystem.File.SetLastAccessTimeUtc(Info.FullName, LastAccessTime);
            }
            if (LastWriteTime != default) {
                FileSystem.File.SetLastWriteTimeUtc(Info.FullName, LastWriteTime);
            }
            return new Result(ResultStatus.Success);
        }

        internal void CanDelete(out Result Status) => Status = new Result(ResultStatus.Success);

        public override byte[] GetSecurityDescriptor() => FileSystem.File.GetAccessControl(Info.FullName).GetSecurityDescriptorBinaryForm();

        public override Result SetSecurityDescriptor(AccessControlSections Sections, byte[] SecurityDescriptor) {
            Info.SetAccessControl(new FileSecurity(Info.FullName, Sections));
            return new Result(ResultStatus.Success);
        }

        public Result SetFileSize(long pNewSize) {
            if (Stream == null) {
                Open(FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
            }
            (Stream as FileStream)?.SetLength(pNewSize);
            return new Result(ResultStatus.Success);
        }

        public Result OverWrite(FileAttributes pFileAttributes, bool pReplaceFileAttributes, out IEntry pEntry) {
            pEntry = new PassthroughEntry(Info);
            if (pReplaceFileAttributes) {
                _ = SetBasicInfo(pFileAttributes | FileAttributes.Archive, default, default, default);
            } else if (pFileAttributes != 0) {
                _ = SetBasicInfo(pEntry.Attributes | pFileAttributes | FileAttributes.Archive, default, default, default);
            }
            Stream.SetLength(0);
            return new Result(ResultStatus.Success);
        }

        public override IEntry GetEntry() => new PassthroughEntry(Info);

        internal void GetSecurityByName(out FileAttributes pFileAttributes, ref byte[] pSecurityDescriptor) {
            pFileAttributes = Info.Attributes;
            if (pSecurityDescriptor != null && Info.Exists) {
                pSecurityDescriptor = Info.GetAccessControl().GetSecurityDescriptorBinaryForm();
            }
        }

        public override FileSystemSecurity GetSecurity() => FileSystem.File.GetAccessControl(Info.FullName);

        internal Result Lock(long offset, long length) {
            if (Stream != null) {
                Open(FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
            }
            (Stream as FileStream)?.Lock(offset, length);
            return new Result(ResultStatus.Success);
        }

        public override Result SetSecurity(FileSystemSecurity pFileSystemSecurity) {
            FileSystem.File.SetAccessControl(Info.FullName, (FileSecurity)pFileSystemSecurity);
            return new Result(ResultStatus.Success);
        }

        internal Result UnLock(long offset, long length) {
            if (Stream != null) {
                Open(FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
            }
            (Stream as FileStream)?.Unlock(offset, length);
            return new Result(ResultStatus.Success);
        }
    }
}