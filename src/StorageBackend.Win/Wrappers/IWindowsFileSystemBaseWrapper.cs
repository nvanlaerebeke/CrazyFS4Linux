using Fsp.Interop;
using System;
using System.Security.AccessControl;

namespace StorageBackend.Win.Winfsp {

    internal interface IWindowsFileSystemBaseWrapper {

        int CanDelete(object FileNode, object FileDesc0, string FileName);

        void Cleanup(object FileNode, object FileDesc0, string FileName, uint Flags);

        void Close(object FileNode, object FileDesc0);

        int Create(string FileName, uint CreateOptions, uint GrantedAccess, uint FileAttributes, byte[] SecurityDescriptor, ulong AllocationSize, out object FileNode, out object FileDesc0, out FileInfo pFileInfo, out string NormalizedName);

        int ExceptionHandler(Exception ex);

        int Flush(object FileNode, object FileDesc0, out FileInfo pFileInfo);

        int GetFileInfo(object FileNode, object FileDesc0, out FileInfo pFileInfo);

        int GetSecurity(object FileNode, object FileDesc0, ref byte[] SecurityDescriptor);

        int GetSecurityByName(string FileName, out uint FileAttributes, ref byte[] SecurityDescriptor);

        int GetVolumeInfo(out VolumeInfo pVolumeInfo);

        int Init();

        void Mount(string pMountPoint, byte[] pSecurityDescriptor, bool pSynchronized, uint pDebugLog, string pLogFile);

        int Open(string FileName, uint CreateOptions, uint GrantedAccess, out object FileNode, out object FileDesc0, out FileInfo FileInfo, out string NormalizedName);

        int Overwrite(object FileNode, object FileDesc0, uint FileAttributes, bool ReplaceFileAttributes, ulong AllocationSize, out FileInfo pFileInfo);

        int Read(object FileNode, object FileDesc0, IntPtr Buffer, ulong Offset, uint Length, out uint PBytesTransferred);

        bool ReadDirectoryEntry(object FileNode, object FileDesc0, string Pattern, string Marker, ref object Context, out string FileName, out FileInfo pFileInfo);

        int Rename(object FileNode, object FileDesc0, string FileName, string NewFileName, bool ReplaceIfExists);

        int SetBasicInfo(object FileNode, object FileDesc0, uint FileAttributes, ulong CreationTime, ulong LastAccessTime, ulong LastWriteTime, ulong ChangeTime, out FileInfo pFileInfo);

        int SetFileSize(object FileNode, object FileDesc0, ulong NewSize, bool SetAllocationSize, out FileInfo pFileInfo);

        int SetSecurity(object FileNode, object FileDesc0, AccessControlSections Sections, byte[] SecurityDescriptor);

        void UnMount();

        int Write(object FileNode, object FileDesc0, IntPtr Buffer, ulong Offset, uint Length, bool WriteToEndOfFile, bool ConstrainedIo, out uint PBytesTransferred, out FileInfo pFileInfo);
    }
}