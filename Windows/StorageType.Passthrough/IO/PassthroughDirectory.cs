using StorageBackend;
using StorageBackend.IO;
using System;
using System.Collections.Generic;
using System.IO;

//using System.IO;
using System.IO.Abstractions;
using System.Linq;
using System.Security.AccessControl;

namespace StorageType.Passthrough.IO {

    internal partial class PassthroughDirectory : PassthroughFileSystemBase, IFSDirectory {

        protected PassthroughDirectory() : base() {
        }

        public PassthroughDirectory(IDirectoryInfo info) : base(info) {
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
                FileSystem.Directory.Delete(FullName);
            }
        }

        public override Result GetAccessControl(out FileSystemSecurity security) {
            try {
                security = FileSystem.DirectoryInfo.FromDirectoryName(FullName).GetAccessControl();
            } catch (UnauthorizedAccessException) {
                security = null;
                return new Result(ResultStatus.AccessDenied);
            }
            return new Result(ResultStatus.Success);
        }

        public Result SetAccessControl(DirectorySecurity security) {
            try {
                FileSystem.DirectoryInfo.FromDirectoryName(FullName).SetAccessControl(security);
                return new Result(ResultStatus.Success);
            } catch (UnauthorizedAccessException) {
                return new Result(ResultStatus.AccessDenied);
            }
        }

        public override void Close() {
        }

        public bool HasContent() {
            return FileSystem.DirectoryInfo.FromDirectoryName(FullName).GetFileSystemInfos().Length > 0;
        }

        public List<IFSEntryPointer> GetContent(string searchPattern = "") {
            var e = new List<IFSEntryPointer>();
            var list = new List<IFileSystemInfo>();

            if (string.IsNullOrEmpty(searchPattern)) {
                list = (FileSystem.DirectoryInfo.FromDirectoryName(FullName).GetFileSystemInfos()).ToList();
            } else {
                list = FileSystem.DirectoryInfo.FromDirectoryName(FullName).EnumerateFileSystemInfos().Where(finfo => NameComparer.IsNameInExpression(searchPattern, finfo.Name, true)).ToList();
            }
            list.ForEach(i => {
                if ((i.Attributes & FileAttributes.Directory) == FileAttributes.Directory) {
                    e.Add(new PassthroughDirectory(i as IDirectoryInfo));
                } else {
                    e.Add(new PassthroughFile(i as IFileInfo));
                }
            });
            return e;
        }
    }
}