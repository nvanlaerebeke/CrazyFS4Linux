using DokanNet;
using StorageBackend.IO;
using StorageBackend.Win.Dokan.Enum;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.AccessControl;
using FileAccess = DokanNet.FileAccess;

namespace StorageBackend.Win.Dokan {

    internal class DokanBackend<T> : IDokanOperations where T : IStorageType, new() {
        private readonly string path;
        private readonly IStorageType Storage;

        public DokanBackend(string pSource) {
            Storage = new T();
            Storage.Setup(pSource);

            path = pSource;
        }

        public NtStatus CreateFile(string fileName, FileAccess access, FileShare share, FileMode mode, FileOptions options, FileAttributes attributes, IDokanFileInfo info) {
            var r = Storage.Create(fileName, !info.IsDirectory, FileAccessConverter.Get(access), share, mode, options, attributes, out var node);
            info.Context = node;
            if (node != null) {
                info.IsDirectory = !node.IsFile();
            }
            return r.GetNtStatus();
        }

        public NtStatus GetFileSecurity(string fileName, out FileSystemSecurity security, AccessControlSections sections, IDokanFileInfo info) {
            if (info.Context == null) {
                info.Context = Storage.GetFileInfo(fileName);
            }
            return info.GetFSEntryPointer().GetAccessControl(out security).GetNtStatus();
        }

        public NtStatus SetFileSecurity(string fileName, FileSystemSecurity security, AccessControlSections sections, IDokanFileInfo info) {
            if (info.Context == null) {
                info.Context = Storage.GetFileInfo(fileName);
            }
            var e = info.GetFSEntryPointer();
            if (e.IsFile()) {
                return (e as IFSFile).SetAccessControl((FileSecurity)security).GetNtStatus();
            } else {
                return (e as IFSDirectory).SetAccessControl((DirectorySecurity)security).GetNtStatus();
            }
        }

        public void Cleanup(string fileName, IDokanFileInfo info) {
            if (info.Context == null) {
                info.Context = Storage.GetFileInfo(fileName);
            }
            var e = info.GetFSEntryPointer();
            ;
            if (e != null) {
                e.Cleanup(info.DeleteOnClose);
            }
            info.Context = null;
        }

        public void CloseFile(string fileName, IDokanFileInfo info) {
            if (info.Context != null) {
                var e = info.GetFSEntryPointer();
                if (e != null) {
                    e.Close();
                }
            }
            info.Context = null;
        }

        public NtStatus ReadFile(string fileName, byte[] buffer, out int bytesRead, long offset, IDokanFileInfo info) {
            if (info.Context == null) {
                info.Context = Storage.GetFileInfo(fileName);
            }
            var e = info.GetFSEntryPointer();
            if (e?.IsFile() == true) {
                return (e as IFSFile).Read(buffer, out bytesRead, offset).GetNtStatus();
            }
            bytesRead = 0;
            return NtStatus.Success;
        }

        public NtStatus WriteFile(string fileName, byte[] buffer, out int bytesWritten, long offset, IDokanFileInfo info) {
            if (info.Context == null) {
                info.Context = Storage.GetFileInfo(fileName);
            }
            var e = info.GetFSEntryPointer();
            if (e?.IsFile() == true) {
                return (e as IFSFile).Write(buffer, out bytesWritten, offset).GetNtStatus();
            }
            bytesWritten = 0;
            return NtStatus.Success;
        }

        public NtStatus FlushFileBuffers(string fileName, IDokanFileInfo info) {
            var e = info.GetFSEntryPointer();
            if (e?.IsFile() == true) {
                (e as IFSFile)?.Flush();
            }
            return NtStatus.Success;
        }

        public NtStatus GetFileInformation(string fileName, out FileInformation fileInfo, IDokanFileInfo info) {
            if (info.Context == null) {
                info.Context = Storage.GetFileInfo(fileName);
            }
            var e = info.GetFSEntryPointer();
            if (e != null) {
                fileInfo = e.ToFileInformation();
                return NtStatus.Success;
            }
            fileInfo = default;
            return NtStatus.ObjectNameNotFound;
        }

        public NtStatus SetFileAttributes(string fileName, FileAttributes attributes, IDokanFileInfo info) {
            if (info.Context == null) {
                info.Context = Storage.GetFileInfo(fileName);
            }
            var e = info.GetFSEntryPointer();
            if (e?.IsFile() == true) {
                return (e as IFSFile).SetAttributes(attributes).GetNtStatus();
            } else {
                return NtStatus.ObjectTypeMismatch;
            }
        }

        public NtStatus SetFileTime(string fileName, DateTime? creationTime, DateTime? lastAccessTime, DateTime? lastWriteTime, IDokanFileInfo info) {
            if (info.Context == null) {
                info.Context = Storage.GetFileInfo(fileName);
            }
            var e = info.GetFSEntryPointer();
            if (e?.IsFile() == true) {
                var f = (e as IFSFile);
                if (f != null) {
                    try {
                        if (creationTime.HasValue) {
                            f.SetCreationTime(creationTime.Value);
                        }
                        if (lastAccessTime.HasValue) {
                            f.SetLastAccessTime(lastAccessTime.Value);
                        }
                        if (lastWriteTime.HasValue) {
                            f.SetLastWriteTime(lastWriteTime.Value);
                        }
                        return NtStatus.Success;
                    } catch (UnauthorizedAccessException) {
                        return NtStatus.AccessDenied;
                    }
                }
            }
            return NtStatus.ObjectNameNotFound;
        }

        /// <summary>
        /// we just check here if we could delete the file - the true deletion is in Cleanup
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        public NtStatus DeleteFile(string fileName, IDokanFileInfo info) {
            if (info.Context == null) {
                info.Context = Storage.GetFileInfo(fileName);
            }
            var e = info.GetFSEntryPointer();
            if (e?.IsFile() == false) {
                return NtStatus.AccessDenied;
            }
            return NtStatus.Success;
        }

        public NtStatus MoveFile(string oldName, string newName, bool replace, IDokanFileInfo info) {
            return Storage.Move(PathNormalizer.ConcatPath(path, oldName), PathNormalizer.ConcatPath(path, newName), replace).GetNtStatus();
        }

        public NtStatus SetEndOfFile(string fileName, long length, IDokanFileInfo info) {
            try {
                if (info.Context == null) {
                    info.Context = Storage.GetFileInfo(fileName);
                }
                var e = info.GetFSEntryPointer();
                if (e?.IsFile() == true) {
                    (e as IFSFile).SetLength(length);
                }
                return NtStatus.Success;
            } catch (IOException) {
                return NtStatus.DiskFull;
            }
        }

        public NtStatus SetAllocationSize(string fileName, long length, IDokanFileInfo info) {
            return SetEndOfFile(fileName, length, info);
        }

        public NtStatus LockFile(string fileName, long offset, long length, IDokanFileInfo info) {
            try {
                if (info.Context == null) {
                    info.Context = Storage.GetFileInfo(fileName);
                }
                var e = info.GetFSEntryPointer();
                if (e?.IsFile() == true) {
                    (e as IFSFile).Lock(offset, length);
                    return NtStatus.Success;
                }
                return NtStatus.ObjectTypeMismatch;
            } catch (IOException) {
                return NtStatus.AccessDenied;
            }
        }

        public NtStatus UnlockFile(string fileName, long offset, long length, IDokanFileInfo info) {
            try {
                if (info.Context == null) {
                    info.Context = Storage.GetFileInfo(fileName);
                }
                var e = info.GetFSEntryPointer();
                if (e?.IsFile() == true) {
                    (e as IFSFile).UnLock(offset, length);
                    return NtStatus.Success;
                }
                return NtStatus.ObjectTypeMismatch;
            } catch (IOException) {
                return NtStatus.AccessDenied;
            }
        }

        /// <summary>
        /// if dir is not empty it can't be deleted
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        public NtStatus DeleteDirectory(string fileName, IDokanFileInfo info) {
            var e = info.GetFSEntryPointer();
            if (e == null) {
                e = Storage.GetFileInfo(fileName);
            }
            if (e != null && !e.IsFile()) {
                return Storage.DeleteDirectory(e as IFSDirectory).GetNtStatus();
            }
            return NtStatus.ObjectNameNotFound;
        }

        public NtStatus FindFilesWithPattern(string fileName, string searchPattern, out IList<FileInformation> files, IDokanFileInfo info) {
            files = new List<FileInformation>();
            if (info.Context == null) {
                info.Context = Storage.GetFileInfo(fileName);
            }
            var e = info.GetFSEntryPointer();
            if (e?.IsFile() == false) {
                foreach (var i in (e as IFSDirectory).GetContent(searchPattern)) {
                    files.Add(i.ToFileInformation());
                }
            }
            return NtStatus.Success;
        }

        /// <summary>
        /// This function is not called because FindFilesWithPattern is implemented
        /// Return DokanResult.NotImplemented in FindFilesWithPattern to make FindFiles called
        /// </summary>
        public NtStatus FindFiles(string fileName, out IList<FileInformation> files, IDokanFileInfo info) {
            return FindFilesWithPattern(fileName, "*", out files, info);
        }

        /// <summary>
        /// Currently alternative streams are not implemented, see here for more info about alt streams:
        /// http://ntfs.com/ntfs-multiple.htm
        /// </summary>
        public NtStatus FindStreams(string fileName, IntPtr enumContext, out string streamName, out long streamSize, IDokanFileInfo info) {
            streamName = string.Empty;
            streamSize = 0;
            return NtStatus.NotImplemented;
        }

        /// <summary>
        /// Currently alternative streams are not implemented, see here for more info about alt streams:
        /// http://ntfs.com/ntfs-multiple.htm
        /// </summary>
        public NtStatus FindStreams(string fileName, out IList<FileInformation> streams, IDokanFileInfo info) {
            streams = new FileInformation[0];
            return NtStatus.NotImplemented;
        }

        public NtStatus GetDiskFreeSpace(out long freeBytesAvailable, out long totalNumberOfBytes, out long totalNumberOfFreeBytes, IDokanFileInfo info) {
            return Volume.VolumeManager.GetDiskFreeSpace(path, out freeBytesAvailable, out totalNumberOfBytes, out totalNumberOfFreeBytes);
        }

        public NtStatus GetVolumeInformation(out string volumeLabel, out FileSystemFeatures features, out string fileSystemName, out uint maximumComponentLength, IDokanFileInfo info) {
            return Volume.VolumeManager.GetVolumeInformation(out volumeLabel, out features, out fileSystemName, out maximumComponentLength);
        }

        public NtStatus Mounted(IDokanFileInfo info) {
            return NtStatus.Success;
        }

        public NtStatus Unmounted(IDokanFileInfo info) {
            return NtStatus.Success;
        }
    }
}