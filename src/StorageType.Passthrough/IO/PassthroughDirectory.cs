using StorageBackend;
using StorageBackend.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.AccessControl;

namespace StorageType.Passthrough.IO {

    internal partial class PassthroughDirectory : PassthroughFileSystemBase, IFSDirectory {

        protected PassthroughDirectory() {
        }

        public PassthroughDirectory(DirectoryInfo info) : base(info) {
        }

        public override object Clone() {
            return new PassthroughDirectory() {
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

        public override void Cleanup(bool deleteOnClose) {
            if (deleteOnClose) {
                Directory.Delete(FullName);
            }
        }

        public override Result GetAccessControl(out FileSystemSecurity security) {
            try {
                security = new DirectoryInfo(FullName).GetAccessControl();
            } catch (UnauthorizedAccessException) {
                security = null;
                return new Result(ResultStatus.AccessDenied);
            }
            return new Result(ResultStatus.Success);
        }

        public Result SetAccessControl(DirectorySecurity security) {
            try {
                new DirectoryInfo(FullName).SetAccessControl(security);
                return new Result(ResultStatus.Success);
            } catch (UnauthorizedAccessException) {
                return new Result(ResultStatus.AccessDenied);
            }
        }

        public override void Close() {
        }

        public bool HasContent() {
            return (new DirectoryInfo(FullName).GetFileSystemInfos().Length > 0);
        }

        public List<IFSEntryPointer> GetContent(string searchPattern = "") {
            var e = new List<IFSEntryPointer>();
            var list = new List<FileSystemInfo>();

            if (string.IsNullOrEmpty(searchPattern)) {
                list = (new DirectoryInfo(FullName).GetFileSystemInfos()).ToList();
            } else {
                list = new DirectoryInfo(FullName).EnumerateFileSystemInfos().Where(finfo => NameComparer.IsNameInExpression(searchPattern, finfo.Name, true)).ToList();
            }
            list.ForEach(i => {
                if ((i.Attributes & FileAttributes.Directory) == FileAttributes.Directory) {
                    e.Add(new PassthroughDirectory(i as DirectoryInfo));
                } else {
                    e.Add(new PassthroughFile(i as FileInfo));
                }
            });
            return e;
        }
    }
}