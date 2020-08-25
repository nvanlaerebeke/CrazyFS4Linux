﻿using Fsp;
using System;
using FileInfo = Fsp.Interop.FileInfo;
using VolumeInfo = Fsp.Interop.VolumeInfo;

namespace StorageBackend.Win {
    public class Storage : FileSystemBase {
        private readonly IStorageBackend Backend;
        private readonly string _Path;

        public Storage(IStorageBackend pBackend) {
            Backend = pBackend;
            _Path = Backend.GetOptions().SourcePath;
        }

        public override int ExceptionHandler(Exception ex) {
            int HResult = ex.HResult; /* needs Framework 4.5 */
            if (0x80070000 == (HResult & 0xFFFF0000)) {
                return NtStatusFromWin32((uint)HResult & 0xFFFF);
            }
            return FileSystemStatus.STATUS_UNEXPECTED_IO_ERROR;
        }
        public override int Init(object Host0) {
            return Backend.Init((FileSystemHost)Host0, _Path);
        }
        public override int GetVolumeInfo(out Fsp.Interop.VolumeInfo pVolumeInfo) {
            var r = Backend.GetVolumeInfo(_Path, out var VolumeInfo);
            VolumeInfo.GetStruct(out pVolumeInfo);
            return r;
        }
        public override int GetSecurityByName(string FileName, out uint FileAttributes /* or ReparsePointIndex */, ref byte[] SecurityDescriptor) {
            return Backend.GetSecurityByName(_Path, FileName, out FileAttributes, ref SecurityDescriptor);
        }
        public override int Create(string FileName, uint CreateOptions, uint GrantedAccess, uint FileAttributes, byte[] SecurityDescriptor, ulong AllocationSize, out object FileNode, out object FileDesc0, out FileInfo pFileInfo, out string NormalizedName) {
            var r = Backend.Create(_Path, FileName, CreateOptions, GrantedAccess, FileAttributes, SecurityDescriptor, out FileNode, out FileDesc0, out var FileInfo, out NormalizedName);
            FileInfo.GetStruct(out pFileInfo);
            return r;
        }
        public override int Open(string FileName, uint CreateOptions, uint GrantedAccess, out object FileNode, out object FileDesc0, out FileInfo FileInfo, out string NormalizedName) {
            var r = Backend.Open(_Path, FileName, GrantedAccess, out FileNode, out var objFileDesc, out var objFileInfo, out NormalizedName);
            objFileInfo.GetStruct(out FileInfo);
            FileDesc0 = objFileDesc;
            return r;
        }
        public override int Overwrite(object FileNode, object FileDesc0, uint FileAttributes, bool ReplaceFileAttributes, ulong AllocationSize, out FileInfo pFileInfo) {
            var r = Backend.OverWrite((FileDesc)FileDesc0, FileAttributes, ReplaceFileAttributes, out var FileInfo);
            FileInfo.GetStruct(out pFileInfo);
            return r;
        }
        public override void Cleanup(object FileNode, object FileDesc0, string FileName, uint Flags) {
            Backend.Cleanup((FileDesc)FileDesc0, FileName, Flags, CleanupDelete);
        }
        public override void Close(object FileNode, object FileDesc0) {
            Backend.Close((FileDesc)FileDesc0);
        }
        public override int Read(object FileNode, object FileDesc0, IntPtr Buffer, ulong Offset, uint Length, out uint PBytesTransferred) {
            return Backend.Read((FileDesc)FileDesc0, Buffer, Offset, Length, out PBytesTransferred);
        }
        public override int Write(object FileNode, object FileDesc0, IntPtr Buffer, ulong Offset, uint Length, bool WriteToEndOfFile, bool ConstrainedIo, out uint PBytesTransferred, out FileInfo pFileInfo) {
            var r = Backend.Write(FileNode, (FileDesc)FileDesc0, Buffer, Offset, Length, WriteToEndOfFile, ConstrainedIo, out PBytesTransferred, out var FileInfo);
            FileInfo.GetStruct(out pFileInfo);
            return r;
        }
        public override int Flush(object FileNode, object FileDesc0, out FileInfo pFileInfo) {
            var r = Backend.Flush(FileNode, (FileDesc)FileDesc0, out var FileInfo);
            FileInfo.GetStruct(out pFileInfo);
            return r;
        }
        public override int GetFileInfo(object FileNode, object FileDesc0, out FileInfo pFileInfo) {
            var r = Backend.GetFileInfo((FileDesc)FileDesc0, out var FileInfo);
            FileInfo.GetStruct(out pFileInfo);
            return r;
        }
        public override int SetBasicInfo(object FileNode, object FileDesc0, uint FileAttributes, ulong CreationTime, ulong LastAccessTime, ulong LastWriteTime, ulong ChangeTime, out FileInfo pFileInfo) {
            var r = Backend.SetBasicInfo((FileDesc)FileDesc0, FileAttributes, CreationTime, LastAccessTime, LastWriteTime, ChangeTime, out var FileInfo);
            FileInfo.GetStruct(out pFileInfo);
            return r;
        }
        public override int SetFileSize(object FileNode, object FileDesc0, ulong NewSize, bool SetAllocationSize, out FileInfo pFileInfo) {
            var r = Backend.SetFileSize(FileNode, (FileDesc)FileDesc0, NewSize, SetAllocationSize, out var FileInfo);
            FileInfo.GetStruct(out pFileInfo);
            return r;
        }
        public override int CanDelete(object FileNode, object FileDesc0, string FileName) {
            return Backend.CanDelete((FileDesc)FileDesc0);
        }

        public override int Rename(object FileNode, object FileDesc0, string FileName, string NewFileName, bool ReplaceIfExists) {
            return Backend.Rename(PathNormalizer.ConcatPath(_Path, FileName), PathNormalizer.ConcatPath(_Path, NewFileName), ReplaceIfExists);
        }

        public override int GetSecurity(object FileNode, object FileDesc0, ref byte[] SecurityDescriptor) {
            return Backend.GetSecurity((FileDesc)FileDesc0, ref SecurityDescriptor);
        }
        public override int SetSecurity(object FileNode, object FileDesc0, System.Security.AccessControl.AccessControlSections Sections, byte[] SecurityDescriptor) {
            return Backend.SetSecurity((FileDesc)FileDesc0, Sections, SecurityDescriptor);
        }
        public override bool ReadDirectoryEntry(object FileNode, object FileDesc0, string Pattern, string Marker, ref object Context, out string FileName, out FileInfo pFileInfo) {
            var r = Backend.ReadDirectory(FileNode, (FileDesc)FileDesc0, Pattern, Marker, ref Context, out FileName, out var FileInfo);
            if (FileInfo != null) {
                FileInfo.GetStruct(out pFileInfo);
            } else {
                pFileInfo = default;
            }
            return r;
        }
    }
}