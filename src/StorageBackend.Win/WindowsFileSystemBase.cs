using Fsp;
using System;
using FileInfo = Fsp.Interop.FileInfo;

namespace StorageBackend.Win.Winfsp {

    internal class WindowsFileSystemBase<T> : FileSystemBase, IVolumeActions where T : IStorageType, new() {
        private readonly IWindowsFileSystemBaseWrapper Wrapper;

        public WindowsFileSystemBase(IWindowsFileSystemBaseWrapper pWrapper) => Wrapper = pWrapper;

        public WindowsFileSystemBase(string pSource) => Wrapper = new WindowsFileSystemBaseWrapper<T>(pSource, new VolumeManager(this, pSource));

        public override int ExceptionHandler(Exception ex) => Wrapper.ExceptionHandler(ex);

        public override int Init(object Host0) => Wrapper.Init();

        public override int GetVolumeInfo(out Fsp.Interop.VolumeInfo pVolumeInfo) => Wrapper.GetVolumeInfo(out pVolumeInfo);

        public override int GetSecurityByName(string FileName, out uint FileAttributes /* or ReparsePointIndex */, ref byte[] SecurityDescriptor) => Wrapper.GetSecurityByName(FileName, out FileAttributes, ref SecurityDescriptor);

        public override int Create(string FileName, uint CreateOptions, uint GrantedAccess, uint FileAttributes, byte[] SecurityDescriptor, ulong AllocationSize, out object FileNode, out object FileDesc0, out FileInfo pFileInfo, out string NormalizedName) => Wrapper.Create(FileName, CreateOptions, GrantedAccess, FileAttributes, SecurityDescriptor, AllocationSize, out FileNode, out FileDesc0, out pFileInfo, out NormalizedName);

        public override int Open(string FileName, uint CreateOptions, uint GrantedAccess, out object FileNode, out object FileDesc0, out FileInfo FileInfo, out string NormalizedName) => Wrapper.Open(FileName, CreateOptions, GrantedAccess, out FileNode, out FileDesc0, out FileInfo, out NormalizedName);

        public override int Overwrite(object FileNode, object FileDesc0, uint FileAttributes, bool ReplaceFileAttributes, ulong AllocationSize, out FileInfo pFileInfo) => Wrapper.Overwrite(FileNode, FileDesc0, FileAttributes, ReplaceFileAttributes, AllocationSize, out pFileInfo);

        public override void Cleanup(object FileNode, object FileDesc0, string FileName, uint Flags) => Wrapper.Cleanup(FileNode, FileDesc0, FileName, Flags);

        public override void Close(object FileNode, object FileDesc0) => Wrapper.Close(FileNode, FileDesc0);

        public override int Read(object FileNode, object FileDesc0, IntPtr Buffer, ulong Offset, uint Length, out uint PBytesTransferred) => Wrapper.Read(FileNode, FileDesc0, Buffer, Offset, Length, out PBytesTransferred);

        public override int Write(object FileNode, object FileDesc0, IntPtr Buffer, ulong Offset, uint Length, bool WriteToEndOfFile, bool ConstrainedIo, out uint PBytesTransferred, out FileInfo pFileInfo) => Wrapper.Write(FileNode, FileDesc0, Buffer, Offset, Length, WriteToEndOfFile, ConstrainedIo, out PBytesTransferred, out pFileInfo);

        public override int Flush(object FileNode, object FileDesc0, out FileInfo pFileInfo) => Wrapper.Flush(FileNode, FileDesc0, out pFileInfo);

        public override int GetFileInfo(object FileNode, object FileDesc0, out FileInfo pFileInfo) => Wrapper.GetFileInfo(FileNode, FileDesc0, out pFileInfo);

        public override int SetBasicInfo(object FileNode, object FileDesc0, uint FileAttributes, ulong CreationTime, ulong LastAccessTime, ulong LastWriteTime, ulong ChangeTime, out FileInfo pFileInfo) => Wrapper.SetBasicInfo(FileNode, FileDesc0, FileAttributes, CreationTime, LastAccessTime, LastWriteTime, ChangeTime, out pFileInfo);

        public override int SetFileSize(object FileNode, object FileDesc0, ulong NewSize, bool SetAllocationSize, out FileInfo pFileInfo) => Wrapper.SetFileSize(FileNode, FileDesc0, NewSize, SetAllocationSize, out pFileInfo);

        public override int CanDelete(object FileNode, object FileDesc0, string FileName) => Wrapper.CanDelete(FileNode, FileDesc0, FileName);

        public override int Rename(object FileNode, object FileDesc0, string FileName, string NewFileName, bool ReplaceIfExists) => Wrapper.Rename(FileNode, FileDesc0, FileName, NewFileName, ReplaceIfExists);

        public override int GetSecurity(object FileNode, object FileDesc0, ref byte[] SecurityDescriptor) => Wrapper.GetSecurity(FileNode, FileDesc0, ref SecurityDescriptor);

        public override int SetSecurity(object FileNode, object FileDesc0, System.Security.AccessControl.AccessControlSections Sections, byte[] SecurityDescriptor) => Wrapper.SetSecurity(FileNode, FileDesc0, Sections, SecurityDescriptor);

        public override bool ReadDirectoryEntry(object FileNode, object FileDesc0, string Pattern, string Marker, ref object Context, out string FileName, out FileInfo pFileInfo) => Wrapper.ReadDirectoryEntry(FileNode, FileDesc0, Pattern, Marker, ref Context, out FileName, out pFileInfo);

        public void Mount(string pMountPoint, byte[] pSecurityDescriptor, bool pSynchronized, uint pDebugLog, string pLogFile) => Wrapper.Mount(pMountPoint, pSecurityDescriptor, pSynchronized, pDebugLog, pLogFile);

        public void UnMount() => Wrapper.UnMount();
    }
}