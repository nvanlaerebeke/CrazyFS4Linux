using StorageBackend;
using StorageBackend.IO;
using System;
using System.IO.Abstractions;
using System.Security.AccessControl;

namespace StorageType.Passthrough.IO {

    internal partial class PassthroughFile : PassthroughFileSystemBase, IFSFile {

        protected PassthroughFile() : base() {
        }

        public PassthroughFile(IFileInfo info) : base(info) {
        }

        public override void Cleanup(bool deleteOnClose) {
            if (deleteOnClose) {
                FileSystem.File.Delete(FullName);
            }
        }

        public override void Close() {
        }

        public override object Clone() {
            return new PassthroughFile() {
                AllocationSize = AllocationSize,
                Attributes = Attributes,
                ChangeTime = ChangeTime,
                CreationTime = CreationTime,
                EaSize = EaSize,
                FileSize = FileSize,
                FullName = FullName,
                HardLinks = HardLinks,
                IndexNumber = IndexNumber,
                LastAccessTime = LastAccessTime,
                LastWriteTime = LastWriteTime,
                Name = Name,
                ReparseTag = ReparseTag
            };
        }

        public Result Read(byte[] buffer, out int bytesRead, long offset) {
            using (var s = FileSystem.FileStream.Create(FullName, System.IO.FileMode.Open, System.IO.FileAccess.Read)) {
                s.Position = offset;
                bytesRead = s.Read(buffer, 0, buffer.Length);
            }
            return new Result(ResultStatus.Success);
        }

        public Result Write(byte[] buffer, out int bytesWritten, long offset) {
            using (var s = FileSystem.FileStream.Create(FullName, System.IO.FileMode.Open, System.IO.FileAccess.Write)) {
                s.Position = offset;
                s.Write(buffer, 0, buffer.Length);
                bytesWritten = buffer.Length;
            }
            return new Result(ResultStatus.Success);
        }

        public void Flush() {
        }

        public Result SetAttributes(System.IO.FileAttributes attributes) {
            try {
                FileSystem.File.SetAttributes(FullName, attributes);
                return new Result(ResultStatus.Success);
            } catch (UnauthorizedAccessException) {
                return new Result(ResultStatus.AccessDenied);
            } catch (System.IO.FileNotFoundException) {
                return new Result(ResultStatus.FileNotFound);
            } catch (System.IO.DirectoryNotFoundException) {
                return new Result(ResultStatus.PathNotFound);
            }
        }

        public void SetCreationTime(DateTime creationTime) {
            FileSystem.File.SetCreationTime(FullName, creationTime);
            CreationTime = creationTime;
        }

        public void SetLastAccessTime(DateTime lastAccessTime) {
            FileSystem.File.SetLastAccessTime(FullName, lastAccessTime);
            LastAccessTime = lastAccessTime;
        }

        public void SetLastWriteTime(DateTime lastWriteTime) {
            FileSystem.File.SetLastWriteTime(FullName, lastWriteTime);
            LastWriteTime = lastWriteTime;
        }

        public void SetLength(long length) {
            using (var s = FileSystem.FileStream.Create(FullName, System.IO.FileMode.Open, System.IO.FileAccess.ReadWrite)) {
                s.SetLength(length);
            }
            FileSize = length;
        }

        public void Lock(long offset, long length) {
            using (var s = FileSystem.FileStream.Create(FullName, System.IO.FileMode.Open, System.IO.FileAccess.ReadWrite)) {
                (s as System.IO.FileStream).Lock(offset, length);
            }
        }

        public void UnLock(long offset, long length) {
            using (var s = FileSystem.FileStream.Create(FullName, System.IO.FileMode.Open, System.IO.FileAccess.ReadWrite)) {
                (s as System.IO.FileStream).Unlock(offset, length);
            }
        }

        public override Result GetAccessControl(out FileSystemSecurity security) {
            try {
                security = FileSystem.FileInfo.FromFileName(FullName).GetAccessControl();
            } catch (UnauthorizedAccessException) {
                security = null;
                return new Result(ResultStatus.AccessDenied);
            }
            return new Result(ResultStatus.Success);
        }

        public Result SetAccessControl(FileSecurity security) {
            try {
                FileSystem.FileInfo.FromFileName(FullName).SetAccessControl(security);
                return new Result(ResultStatus.Success);
            } catch (UnauthorizedAccessException) {
                return new Result(ResultStatus.AccessDenied);
            }
        }
    }
}