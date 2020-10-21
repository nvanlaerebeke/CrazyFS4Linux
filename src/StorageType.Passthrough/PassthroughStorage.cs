using StorageBackend;
using StorageBackend.IO;
using StorageBackend.Volume;
using StorageType.Passthrough.IO;
using System;
using System.IO;
using System.IO.Abstractions;

namespace StorageType.Passthrough {

    public class PassthroughStorage : IStorageType {
        private string SourcePath;

        private readonly IFileSystem FileSystem;
        public IVolume VolumeManager { get; private set; }

        public PassthroughStorage() : this(new FileSystem()) {
        }

        public PassthroughStorage(IFileSystem pFileSystem) {
            FileSystem = pFileSystem;
        }

        public void Setup(string pSourcePath) {
            if (!Directory.Exists(pSourcePath)) {
                throw new ArgumentException(nameof(pSourcePath));
            }
            VolumeManager = new VolumeManager(pSourcePath);
            SourcePath = pSourcePath;
        }

        public Result Create(string fileName, bool isfile, FileAccess access, FileShare share, FileMode mode, FileOptions options, FileAttributes attributes, out IFSEntryPointer node) {
            return PassthroughFile.CreateFile(PathNormalizer.ConcatPath(SourcePath, fileName), access, share, mode, options, attributes, !isfile, out node);
        }

        public string GetPath() => SourcePath;

        public void Cleanup(IFSEntryPointer pFileDesc, bool pCleanupDelete) {
            if (pCleanupDelete) {
                (pFileDesc as PassthroughFile)?.Close();
            }
        }

        public void Close(IFSEntryPointer pFileDesc) => (pFileDesc as PassthroughFile)?.Close();

        public Result DeleteDirectory(IFSDirectory iFSDirectory) {
            try {
                if (!Directory.Exists(iFSDirectory.FullName)) {
                    return new Result(ResultStatus.PathNotFound);
                }
                if (iFSDirectory.HasContent()) {
                    return new Result(ResultStatus.DirectoryNotEmpty);
                }
                Directory.Delete(iFSDirectory.FullName);
                return new Result(ResultStatus.Success);
            } catch (UnauthorizedAccessException) {
                return new Result(ResultStatus.AccessDenied);
            } catch (Exception) {
                return new Result(ResultStatus.Unsuccessful);
            }
        }

        /*public Result SetSecurity(IFSEntryPointer pFileDesc, AccessControlSections pSections, byte[] pSecurityDescriptor) {
            if (pFileDesc is PassthroughFileSystemBase pointer) {
                return pointer.SetSecurityDescriptor(pSections, pSecurityDescriptor);
            }
            return new Result(ResultStatus.Success);
        }*/

        /* public Result CanDelete(IFSEntryPointer pFileDesc) {
             (pFileDesc as PassthroughFile).CanDelete(out var Status);
             return Status;
         }

         public Result Flush(IFSEntryPointer pFileDesc) {
             if (pFileDesc is PassthroughFile pointer) {
                 return pointer.Flush();
             } else {
                 return new Result(ResultStatus.Success);
             }
         }*/

        public IFSEntryPointer GetFileInfo(string filename) {
            var fullpath = PathNormalizer.ConcatPath(SourcePath, filename);
            if (File.Exists(fullpath)) {
                return new PassthroughFile(new FileInfo(fullpath));
            } else if (Directory.Exists(fullpath)) {
                return new PassthroughDirectory(new DirectoryInfo(fullpath));
            }
            return null;
        }

        /* public Result GetFileInfo(IFSEntryPointer pFileDesc, out IEntry pEntry) {
             if (pFileDesc is PassthroughFileSystemBase pointer) {
                 pEntry = pointer.GetEntry();
             } else {
                 pEntry = default;
             }
             return new Result(ResultStatus.Success);
         }

         public Result GetFileInfo(string pFilePath, out IFSEntryPointer pFileDesc) {
             pFileDesc = new PassthroughFile(FileSystem.FileInfo.FromFileName(pFilePath));
             return new Result(ResultStatus.Success);
         }*/

        /*public Result GetSecurity(IFSEntryPointer pFileDesc, out FileSystemSecurity pFileSystemSecurity) {
            pFileSystemSecurity = null;
            if (pFileDesc is PassthroughFileSystemBase pointer) {
                pFileSystemSecurity = pointer.GetSecurity();
            }
            return new Result(ResultStatus.Success);
        }

        public Result GetSecurityByName(string pFileName, out FileAttributes pFileAttributes, ref byte[] pSecurityDescriptor) {
            if (FileActions.Exists(pFileName)) {
                new PassthroughFile(
                    FileSystem.FileInfo.FromFileName(PathNormalizer.ConcatPath(SourcePath, pFileName))
                )
                .GetSecurityByName(
                    out pFileAttributes,
                    ref pSecurityDescriptor
                );
                return new Result(ResultStatus.Success);
            }
            pFileAttributes = default;
            return new Result(ResultStatus.Success);
        }*/

        /*public Result Open(string pFileName, uint pGrantedAccess, out object pFileNode, out IFSEntryPointer pFileDesc, out IEntry pEntry, out string pNormalizedName) {
            var FullPath = PathNormalizer.ConcatPath(SourcePath, pFileName);
            if (DirectoryActions.Exists(FullPath)) {
                pFileDesc = DirectoryActions.Open(FullPath, out pEntry);
            } else if (FileActions.Exists(FullPath)) {
                pFileDesc = FileActions.Open(FullPath, pGrantedAccess, out pEntry);
            } else {
                pFileDesc = default;
                pEntry = default;
            }
            pNormalizedName = default;
            pFileNode = default;
            return new Result(ResultStatus.Success);
        }*/

        /*public Result OverWrite(IFSEntryPointer pFileDesc, FileAttributes pFileAttributes, bool pReplaceFileAttributes, out IEntry pEntry) {
            if (pFileDesc is PassthroughFile pointer) {
                return pointer.OverWrite(pFileAttributes, pReplaceFileAttributes, out pEntry);
            } else {
                pEntry = default;
            }
            return new Result(ResultStatus.Success);
        }*/

        /*public Result Read(IFSEntryPointer pFileDesc, out byte[] pBuffer, long pOffset, int pLength, out int pBytesTransferred) {
            if (pFileDesc is PassthroughFile pointer) {
                return pointer.Read(out pBuffer, pOffset, pLength, out pBytesTransferred);
            } else {
                pBuffer = default;
                pBytesTransferred = 0;
            }
            return new Result(ResultStatus.Success);
        }*/

        /*public Result ReadDirectory(IFSEntryPointer pFileDesc, string pPattern, bool pCaseSensitive, string pMarker, out IFSEntryPointer[] pEntries) {
            pEntries = null;
            if (pFileDesc is PassthroughDirectory pointer) {
                pEntries = pointer.ReadDirectory(pPattern, pCaseSensitive, pMarker);
                return new Result(ResultStatus.Success);
            }
            return new Result(ResultStatus.NotADirectory);
        }*/

        /* public Result Rename(string pOldPath, string pNewPath, bool pReplaceIfExists) {
             if (FileActions.Exists(pOldPath)) {
                 FileActions.Move(pOldPath, pNewPath);
             } else if (DirectoryActions.Exists(pOldPath)) {
                 DirectoryActions.Move(pOldPath, pNewPath);
             } else {
                 throw new FileNotFoundException();
             }
             return new Result(ResultStatus.Success);
         }*/

        /*public Result SetBasicInfo(IFSEntryPointer pFileDesc, FileAttributes pFileAttributes, DateTime pCreationTime, DateTime pLastAccessTime, DateTime pLastWriteTime, DateTime pChangeTime) {
            if (pFileDesc is PassthroughFileSystemBase pointer) {
                return pointer.SetBasicInfo(pFileAttributes, pCreationTime, pLastAccessTime, pLastWriteTime);
            }
            return new Result(ResultStatus.Success);
        }

        public Result SetFileSize(IFSEntryPointer pFileDesc, long pNewSize) {
            if (pFileDesc is PassthroughFile pointer) {
                return pointer.SetFileSize(pNewSize);
            }
            return new Result(ResultStatus.FileNotFound);
        }*/

        /*public Result Write(IFSEntryPointer pFileDesc, byte[] pBuffer, long pOffset, out int pBytesTransferred) {
            if (pFileDesc is PassthroughFile pointer) {
                return pointer.Write(pBuffer, pOffset, out pBytesTransferred);
            } else {
                pBytesTransferred = 0;
                return new Result(ResultStatus.FileNotFound);
            }
        }*/

        //public Result GetCreationTimeUtc() => throw new NotImplementedException();

        /* public Result Delete(IFSEntryPointer pFSEntryPointer, bool pRecursive) {
             if (pFSEntryPointer is PassthroughFile f) {
                 return FileActions.Delete(f);
             } else if (pFSEntryPointer is PassthroughDirectory d) {
                 return DirectoryActions.Delete(d, pRecursive);
             }
             return new Result(ResultStatus.Success);
         }*/

        /*public Result Lock(IFSEntryPointer pFSEntryPointer, long offset, long length) {
            if (pFSEntryPointer is PassthroughFile pointer) {
                return pointer.Lock(offset, length);
            }
            return new Result(ResultStatus.FileNotFound);
        }*/

        /*public Result SetSecurity(IFSEntryPointer pFSEntryPointer, FileSystemSecurity pFileSystemSecurity) {
            if (pFSEntryPointer is PassthroughFileSystemBase pointer) {
                return pointer.SetSecurity(pFileSystemSecurity);
            }
            return new Result(ResultStatus.PathNotFound);
        }*/

        /*public Result UnLock(IFSEntryPointer pFSEntryPointer, long offset, long length) {
            if (pFSEntryPointer is PassthroughFile pointer) {
                return pointer.UnLock(offset, length);
            }
            return new Result(ResultStatus.FileNotFound);
        }

        public Result SetAllocationSize(string fileName, long length, IFSEntryPointer info) {
            return SetFileSize(info, length);
        }*/

        public Result Move(string oldpath, string newpath, bool replace) {
            if (Directory.Exists(oldpath)) {
                return PassthroughDirectory.Move(oldpath, newpath);
            } else if (File.Exists(newpath)) {
                return PassthroughFile.Move(oldpath, newpath, replace);
            } else {
                return new Result(ResultStatus.PathNotFound);
            }
        }
    }
}