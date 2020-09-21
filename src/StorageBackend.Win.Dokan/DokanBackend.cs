using DokanNet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.AccessControl;

namespace StorageBackend.Win.Dokan {

    internal class DokanBackend<T> : IDokanOperations where T : IStorageType, new() {
        private readonly IStorageType Storage;

        public DokanBackend(string pSource) {
            Storage = new T();
            Storage.Setup(pSource);
        }

        public void Cleanup(string fileName, IDokanFileInfo info)
            => Storage.Cleanup(info.GetFSEntryPointer(), info.DeleteOnClose);

        public void CloseFile(string fileName, IDokanFileInfo info)
            => Storage.Close(info.GetFSEntryPointer());

        public NtStatus CreateFile(string pFileName, DokanNet.FileAccess pAccess, FileShare pShare, FileMode pMode, FileOptions pOptions, FileAttributes pAttributes, IDokanFileInfo pInfo) {
            var r = Storage.Create(pFileName, !pInfo.IsDirectory, pAccess.GetFileAccess(), pShare, pMode, pOptions, pAttributes, out var e).GetNtStatus();
            if (e != null) {
                pInfo.IsDirectory = !e.GetEntry().IsFile();
                pInfo.Context = e;
            }
            return r;
        }

        public NtStatus DeleteDirectory(string fileName, IDokanFileInfo info)
            => Storage.Delete(info.GetFSEntryPointer(), false).GetNtStatus();

        public NtStatus DeleteFile(string fileName, IDokanFileInfo info)
            => Storage.Delete(info.GetFSEntryPointer(), false).GetNtStatus();

        public NtStatus FindFiles(string fileName, out IList<FileInformation> files, IDokanFileInfo info)
            => FindFilesWithPattern(fileName, "*", out files, info);

        public NtStatus FindFilesWithPattern(string fileName, string searchPattern, out IList<FileInformation> files, IDokanFileInfo info) {
            var r = Storage.ReadDirectory(info.GetFSEntryPointer(), searchPattern, false, "", out var Entries);
            files = new List<FileInformation>();
            if (Entries != null) {
                foreach (var e in Entries) {
                    var entry = e.GetEntry();
                    files.Add(new FileInformation() {
                        Attributes = entry.Attributes,
                        CreationTime = entry.CreationTime,
                        FileName = entry.Name,
                        LastAccessTime = entry.LastAccessTime,
                        LastWriteTime = entry.LastWriteTime,
                        Length = entry.FileSize
                    });
                }
            }
            return r.GetNtStatus();
        }

        public NtStatus FindStreams(string fileName, out IList<FileInformation> streams, IDokanFileInfo info) {
            var entry = info.GetFSEntryPointer().GetEntry();
            streams = new List<FileInformation>() {
                new FileInformation() {
                    Attributes = entry.Attributes,
                    CreationTime = entry.CreationTime,
                    FileName = entry.Name,
                    LastAccessTime = entry.LastAccessTime,
                    LastWriteTime = entry.LastWriteTime,
                    Length = entry.FileSize
                }
            };
            return NtStatus.Success;
        }

        public NtStatus FlushFileBuffers(string fileName, IDokanFileInfo info)
            => Storage.Flush(info.GetFSEntryPointer()).GetNtStatus();

        public NtStatus GetDiskFreeSpace(out long freeBytesAvailable, out long totalNumberOfBytes, out long totalNumberOfFreeBytes, IDokanFileInfo info) {
            var v = Storage.VolumeManager.GetVolumeInfo();
            freeBytesAvailable = v.FreeSize;
            totalNumberOfBytes = v.TotalSize;
            totalNumberOfFreeBytes = v.TotalSize - v.FreeSize;
            return NtStatus.Success;
        }

        public NtStatus GetFileInformation(string fileName, out FileInformation fileInfo, IDokanFileInfo info) {
            var entry = info.GetFSEntryPointer().GetEntry();
            fileInfo = new FileInformation() {
                Attributes = entry.Attributes,
                CreationTime = entry.CreationTime,
                FileName = entry.Name,
                LastAccessTime = entry.LastAccessTime,
                LastWriteTime = entry.LastWriteTime,
                Length = entry.FileSize
            };
            return NtStatus.Success;
        }

        public NtStatus GetFileSecurity(string fileName, out FileSystemSecurity security, AccessControlSections sections, IDokanFileInfo info)
            => Storage.GetSecurity(info.GetFSEntryPointer(), out security).GetNtStatus();

        public NtStatus GetVolumeInformation(out string volumeLabel, out FileSystemFeatures features, out string fileSystemName, out uint maximumComponentLength, IDokanFileInfo info) {
            var v = Storage.VolumeManager.GetVolumeInfo();
            features = (FileSystemFeatures)(int)Storage.VolumeManager.GetFeatures();
            volumeLabel = v.Label;
            fileSystemName = "CrazyFS";
            maximumComponentLength = 256;
            return NtStatus.Success;
        }

        public NtStatus LockFile(string fileName, long offset, long length, IDokanFileInfo info)
            => Storage.Lock(info.GetFSEntryPointer(), offset, length).GetNtStatus();

        public NtStatus Mounted(IDokanFileInfo info)
            => NtStatus.Success;

        public NtStatus MoveFile(string oldName, string newName, bool replace, IDokanFileInfo info)
            => Storage.Rename(oldName, newName, replace).GetNtStatus();

        public NtStatus ReadFile(string fileName, byte[] buffer, out int bytesRead, long offset, IDokanFileInfo info)
            => Storage.Read(info.GetFSEntryPointer(), out buffer, offset, buffer.Length, out bytesRead).GetNtStatus();

        public NtStatus SetAllocationSize(string fileName, long length, IDokanFileInfo info)
            => Storage.SetFileSize(info.GetFSEntryPointer(), length).GetNtStatus();

        public NtStatus SetEndOfFile(string fileName, long length, IDokanFileInfo info)
            => Storage.SetFileSize(info.GetFSEntryPointer(), length).GetNtStatus();

        public NtStatus SetFileAttributes(string fileName, FileAttributes attributes, IDokanFileInfo info) {
            var p = info.GetFSEntryPointer();
            var e = p.GetEntry();
            return Storage.SetBasicInfo(p, attributes, e.CreationTime, e.LastAccessTime, e.LastWriteTime, e.ChangeTime).GetNtStatus();
        }

        public NtStatus SetFileSecurity(string fileName, FileSystemSecurity security, AccessControlSections sections, IDokanFileInfo info)
            => Storage.SetSecurity(info.GetFSEntryPointer(), security).GetNtStatus();

        public NtStatus SetFileTime(string fileName, DateTime? creationTime, DateTime? lastAccessTime, DateTime? lastWriteTime, IDokanFileInfo info) {
            var p = info.GetFSEntryPointer();
            return Storage.SetBasicInfo(
                p,
                p.GetEntry().Attributes,
                (creationTime != null) ? (DateTime)creationTime : default,
                (lastAccessTime != null) ? (DateTime)lastAccessTime : default,
                (lastWriteTime != null) ? (DateTime)lastWriteTime : default,
                default
            ).GetNtStatus();
        }

        public NtStatus UnlockFile(string fileName, long offset, long length, IDokanFileInfo info)
            => Storage.UnLock(info.GetFSEntryPointer(), offset, length).GetNtStatus();

        public NtStatus Unmounted(IDokanFileInfo info)
            => NtStatus.Success;

        public NtStatus WriteFile(string fileName, byte[] buffer, out int bytesWritten, long offset, IDokanFileInfo info)
            => Storage.Write(info.GetFSEntryPointer(), buffer, offset, out bytesWritten).GetNtStatus();
    }
}