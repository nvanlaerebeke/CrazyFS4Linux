using StorageBackend;
using StorageBackend.IO;
using StorageType.Passthrough.IO;
using System;
using System.IO.Abstractions;

namespace StorageType.Passthrough {

    public class PassthroughStorage : IStorageType {
        private string SourcePath;
        private FileSystem FileSystem;

        public void Setup(string pSourcePath) {
            FileSystem = new FileSystem();
            if (!FileSystem.Directory.Exists(pSourcePath)) {
                throw new ArgumentException(nameof(pSourcePath));
            }
            SourcePath = pSourcePath;
        }

        public Result Create(string fileName, bool isfile, System.IO.FileAccess access, System.IO.FileShare share, System.IO.FileMode mode, System.IO.FileOptions options, System.IO.FileAttributes attributes, out IFSEntryPointer node) {
            return PassthroughFile.CreateFile(FileSystem, PathNormalizer.ConcatPath(SourcePath, fileName), access, share, mode, options, attributes, !isfile, out node);
        }

        public Result DeleteDirectory(IFSDirectory iFSDirectory) {
            try {
                if (!FileSystem.Directory.Exists(iFSDirectory.FullName)) {
                    return new Result(ResultStatus.PathNotFound);
                }
                if (iFSDirectory.HasContent()) {
                    return new Result(ResultStatus.DirectoryNotEmpty);
                }
                FileSystem.Directory.Delete(iFSDirectory.FullName);
                return new Result(ResultStatus.Success);
            } catch (UnauthorizedAccessException) {
                return new Result(ResultStatus.AccessDenied);
            } catch (Exception) {
                return new Result(ResultStatus.Unsuccessful);
            }
        }

        public IFSEntryPointer GetFileInfo(string filename) {
            var fullpath = PathNormalizer.ConcatPath(SourcePath, filename);
            if (FileSystem.File.Exists(fullpath)) {
                return new PassthroughFile(FileSystem.FileInfo.FromFileName(fullpath));
            } else if (FileSystem.Directory.Exists(fullpath)) {
                return new PassthroughDirectory(FileSystem.DirectoryInfo.FromDirectoryName(fullpath));
            }
            return null;
        }

        public Result Move(string oldpath, string newpath, bool replace) {
            if (FileSystem.Directory.Exists(oldpath)) {
                return PassthroughDirectory.Move(FileSystem, oldpath, newpath);
            } else if (FileSystem.File.Exists(newpath)) {
                return PassthroughFile.Move(FileSystem, oldpath, newpath, replace);
            } else {
                return new Result(ResultStatus.PathNotFound);
            }
        }
    }
}