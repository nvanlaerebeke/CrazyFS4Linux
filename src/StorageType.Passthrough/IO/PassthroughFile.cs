using StorageBackend;
using StorageBackend.IO;
using System;
using System.IO;
using System.Security.AccessControl;

namespace StorageType.Passthrough.IO {

    internal partial class PassthroughFile : PassthroughFileSystemBase, IFSFile {

        protected PassthroughFile() {
        }

        public PassthroughFile(FileInfo info) : base(info) {
        }

        public override void Cleanup(bool deleteOnClose) {
            if (deleteOnClose) {
                File.Delete(FullName);
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
            using (var s = new FileStream(FullName, FileMode.Open, FileAccess.Read)) {
                s.Position = offset;
                bytesRead = s.Read(buffer, 0, buffer.Length);
            }
            return new Result(ResultStatus.Success);
        }

        public Result Write(byte[] buffer, out int bytesWritten, long offset) {
            using (var s = new FileStream(FullName, FileMode.Open, FileAccess.Write)) {
                s.Position = offset;
                s.Write(buffer, 0, buffer.Length);
                bytesWritten = buffer.Length;
            }
            return new Result(ResultStatus.Success);
        }

        public void Flush() {
        }

        public Result SetAttributes(FileAttributes attributes) {
            try {
                File.SetAttributes(FullName, attributes);
                return new Result(ResultStatus.Success);
            } catch (UnauthorizedAccessException) {
                return new Result(ResultStatus.AccessDenied);
            } catch (FileNotFoundException) {
                return new Result(ResultStatus.FileNotFound);
            } catch (DirectoryNotFoundException) {
                return new Result(ResultStatus.PathNotFound);
            }
        }

        public void SetCreationTime(DateTime creationTime) {
            File.SetCreationTime(FullName, creationTime);
            CreationTime = creationTime;
        }

        public void SetLastAccessTime(DateTime lastAccessTime) {
            File.SetLastAccessTime(FullName, lastAccessTime);
            LastAccessTime = lastAccessTime;
        }

        public void SetLastWriteTime(DateTime lastWriteTime) {
            File.SetLastWriteTime(FullName, lastWriteTime);
            LastWriteTime = lastWriteTime;
        }

        public void SetLength(long length) {
            using (var stream = new FileStream(FullName, FileMode.Open, FileAccess.ReadWrite, FileShare.None, 4096, FileOptions.None)) {
                stream.SetLength(length);
            }
            FileSize = length;
        }

        public void Lock(long offset, long length) {
            using (var stream = new FileStream(FullName, FileMode.Open, FileAccess.ReadWrite, FileShare.None, 4096, FileOptions.None)) {
                stream.Lock(offset, length);
            }
        }

        public void UnLock(long offset, long length) {
            using (var stream = new FileStream(FullName, FileMode.Open, FileAccess.ReadWrite, FileShare.None, 4096, FileOptions.None)) {
                stream.Unlock(offset, length);
            }
        }

        internal void Open(FileMode mode, FileAccess access, FileShare share, int buffersize, FileOptions options) {
        }

        public override Result GetAccessControl(out FileSystemSecurity security) {
            try {
                security = new FileInfo(FullName).GetAccessControl();
            } catch (UnauthorizedAccessException) {
                security = null;
                return new Result(ResultStatus.AccessDenied);
            }
            return new Result(ResultStatus.Success);
        }

        public Result SetAccessControl(FileSecurity security) {
            try {
                new FileInfo(FullName).SetAccessControl(security);
                return new Result(ResultStatus.Success);
            } catch (UnauthorizedAccessException) {
                return new Result(ResultStatus.AccessDenied);
            }
        }
    }
}