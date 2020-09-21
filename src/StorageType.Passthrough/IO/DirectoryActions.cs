using StorageBackend;
using StorageBackend.IO;
using System;
using System.IO.Abstractions;

namespace StorageType.Passthrough.IO {

    internal class DirectoryActions {
        private readonly IFileSystem FileSystem;
        private readonly FileActions FileActions;

        public DirectoryActions(IFileSystem pFileSystem) {
            FileSystem = pFileSystem;
            FileActions = new FileActions(FileSystem);
        }

        public DirectoryActions() : this(new FileSystem()) {
        }

        public void Move(string pOldPath, string pNewPath) {
            FileSystem.Directory.Move(pOldPath, pNewPath);
        }

        public PassthroughDirectory Open(string pPath, out IEntry pEntry) {
            if (!FileSystem.Directory.Exists(pPath)) {
                throw new System.IO.DirectoryNotFoundException($"{pPath} not found");
            }
            var d = new PassthroughDirectory(FileSystem.DirectoryInfo.FromDirectoryName(pPath));
            pEntry = d.GetEntry();
            return d;
        }

        public bool Exists(string pPath) => FileSystem.Directory.Exists(pPath);

        internal Result Create(string pFileName, System.IO.FileMode pMode, System.IO.FileAttributes pFileAttributes, out IFSEntryPointer pNode) {
            try {
                switch (pMode) {
                    case System.IO.FileMode.Open:
                        if (FileSystem.Directory.Exists(pFileName)) {
                            pNode = new PassthroughDirectory(FileSystem.DirectoryInfo.FromDirectoryName(pFileName));
                            return new Result(ResultStatus.Success);
                        } else {
                            pNode = default;
                            return new Result(ResultStatus.PathNotFound);
                        }
                    case System.IO.FileMode.CreateNew:
                        if (FileSystem.Directory.Exists(pFileName)) {
                            pNode = new PassthroughDirectory(FileSystem.DirectoryInfo.FromDirectoryName(pFileName));
                            return new Result(ResultStatus.AlreadyExists);
                        } else {
                            pNode = new PassthroughDirectory(FileSystem.Directory.CreateDirectory(pFileName));
                            return new Result(ResultStatus.Success);
                        }
                    default:
                        throw new NotSupportedException($"Mode {pMode} is not supported in directory create");
                }
            } catch (UnauthorizedAccessException) {
                pNode = default;
                return new Result(ResultStatus.AccessDenied);
            }
        }

        internal Result Delete(PassthroughDirectory pDirectory, bool pRecursive) {
            var path = pDirectory.GetEntry().FullName;
            if (FileSystem.Directory.Exists(path)) {
                var c = FileSystem.Directory.GetDirectories(path);
                if (c.Length > 0 && !pRecursive) {
                    return new Result(ResultStatus.DirectoryNotEmpty);
                }
                foreach (var d in c) {
                    var r = Delete(new PassthroughDirectory(FileSystem.DirectoryInfo.FromDirectoryName(d)), true);
                    if (r.Status != ResultStatus.Success) {
                        return r;
                    }
                }
                c = FileSystem.Directory.GetFiles(path);
                foreach (var f in c) {
                    var r = FileActions.Delete(new PassthroughFile(FileSystem.FileInfo.FromFileName(f)));
                    if (r.Status != ResultStatus.Success) {
                        return r;
                    }
                }
            }
            return new Result(ResultStatus.Success);
        }
    }
}