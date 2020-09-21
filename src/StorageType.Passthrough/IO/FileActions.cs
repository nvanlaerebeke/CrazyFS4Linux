using StorageBackend;
using StorageBackend.IO;
using System;
using System.IO;
using System.IO.Abstractions;
using System.Runtime.InteropServices;

namespace StorageType.Passthrough.IO {

    internal class FileActions {
        public readonly IFileSystem FileSystem;

        public FileActions(IFileSystem pFileSystem) => FileSystem = pFileSystem;

        public FileActions() : this(new FileSystem()) {
        }

        public bool Exists(string pPath) => FileSystem.File.Exists(pPath);

        public void Move(string pOldPath, string pNewPath) => FileSystem.File.Move(pOldPath, pNewPath);

        /// <summary>
        /// ToDo: SetSecurity was removed for .net standard support
        /// </summary>
        internal Result Create(string pFileName, FileAccess pFileAccess, FileShare pShare, FileMode pFileMode, FileAttributes pFileAttributes, out IFSEntryPointer pNode) {
            if (!pFileAttributes.HasFlag(FileAttributes.Archive)) {
                pFileAttributes |= FileAttributes.Archive;
            }
            pNode = default;
            switch (pFileMode) {
                case FileMode.Open:
                    if (!FileSystem.File.Exists(pFileName)) {
                        return new Result(ResultStatus.FileNotFound);
                    }
                    break;

                case FileMode.CreateNew:
                    if (FileSystem.File.Exists(pFileName)) {
                        return new Result(ResultStatus.FileNotFound);
                    }
                    break;

                case FileMode.Truncate:
                    if (!FileSystem.File.Exists(pFileName)) {
                        return new Result(ResultStatus.FileNotFound);
                    }
                    break;

                case FileMode.Append:
                    break;

                case FileMode.Create:
                case FileMode.OpenOrCreate:
                    if (FileSystem.File.Exists(pFileName)) {
                        return new Result(ResultStatus.AlreadyExists);
                    }
                    break;
            }
            try {
                using (var s = FileSystem.File.Open(pFileName, pFileMode, pFileAccess, pShare)) { }
                FileSystem.File.SetAttributes(pFileName, pFileAttributes);
                pNode = new PassthroughFile(FileSystem.FileInfo.FromFileName(pFileName), FileSystem);
                return new Result(ResultStatus.Success);
            } catch (UnauthorizedAccessException) {
                return new Result(ResultStatus.AccessDenied);
            } catch (DirectoryNotFoundException) {
                return new Result(ResultStatus.PathNotFound);
            } catch (Exception ex) {
                switch ((uint)Marshal.GetHRForException(ex)) {
                    case 0x80070020: //Sharing violation
                        return new Result(ResultStatus.SharingViolation);

                    default:
                        throw;
                }
            }
        }

        internal PassthroughFile Open(string pFileName, uint pGrantedAccess, out IEntry pEntry) {
            var objFileDesc = new PassthroughFile(FileSystem.FileInfo.FromFileName(pFileName), FileSystem);
            objFileDesc.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.Read | FileShare.Write | FileShare.Delete);
            pEntry = objFileDesc.GetEntry();
            return objFileDesc;
        }

        internal Result Delete(PassthroughFile f) {
            var path = f.GetEntry().FullName;
            if (FileSystem.File.Exists(path)) {
                FileSystem.File.Delete(path);
            }
            return new Result(ResultStatus.Success);
        }
    }
}