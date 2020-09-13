using Fsp;
using Fsp.Interop;
using StorageBackend.IO;
using System;

namespace StorageBackend.Win.Winfsp {

    internal class WindowsFileSystemBaseWrapper<T> : IWindowsFileSystemBaseWrapper where T : IStorageType, new() {
        private readonly IStorageType Storage;
        private readonly VolumeManager VolumeManager;

        public WindowsFileSystemBaseWrapper(string pSource, VolumeManager pVolumeManager) {
            Storage = new T();
            Storage.Setup(pSource);
            VolumeManager = pVolumeManager;
        }

        public int ExceptionHandler(Exception ex) {
            var HResult = ex.HResult;
            if (0x80070000 == (HResult & 0xFFFF0000)) {
                return FileSystemBase.NtStatusFromWin32((uint)HResult & 0xFFFF);
            }
            return FileSystemStatus.STATUS_UNEXPECTED_IO_ERROR;
        }

        public int Init() => VolumeManager.Initialize(DateTime.UtcNow.ToFileTimeUtc());

        public void Mount(string pMountPoint, byte[] pSecurityDescriptor, bool pSynchronized, uint pDebugLog, string pLogFile) => VolumeManager.Mount(pMountPoint, pSecurityDescriptor, pSynchronized, pDebugLog, pLogFile);

        public void UnMount() => VolumeManager.UnMount();

        public int GetVolumeInfo(out VolumeInfo pVolumeInfo) {
            try {
                VolumeManager.GetVolumeInfo().GetStruct(out pVolumeInfo);
                return FileSystemStatus.STATUS_SUCCESS;
            } catch (Win32Exception ex) {
                throw WindowsExceptionGenerator.GetIOException(ex);
            } catch (NTException ex) {
                throw WindowsExceptionGenerator.GetIOException(ex);
            }
        }

        public int GetSecurityByName(string FileName, out uint FileAttributes /* or ReparsePointIndex */, ref byte[] SecurityDescriptor) {
            try {
                return Storage.GetSecurityByName(FileName, out FileAttributes, ref SecurityDescriptor);
            } catch (Win32Exception ex) {
                throw WindowsExceptionGenerator.GetIOException(ex);
            } catch (NTException ex) {
                throw WindowsExceptionGenerator.GetIOException(ex);
            }
        }

        public int Create(string FileName, uint CreateOptions, uint GrantedAccess, uint FileAttributes, byte[] SecurityDescriptor, ulong AllocationSize, out object FileNode, out object FileDesc0, out FileInfo pFileInfo, out string NormalizedName) {
            try {
                var r = Storage.Create(FileName, CreateOptions, GrantedAccess, FileAttributes, SecurityDescriptor, out FileNode, out var Pointer, out var FileInfo, out NormalizedName);
                FileDesc0 = Pointer;
                FileInfo.GetStruct(out pFileInfo);
                return r;
            } catch (Win32Exception ex) {
                throw WindowsExceptionGenerator.GetIOException(ex);
            } catch (NTException ex) {
                throw WindowsExceptionGenerator.GetIOException(ex);
            }
        }

        public int Open(string FileName, uint CreateOptions, uint GrantedAccess, out object FileNode, out object FileDesc0, out FileInfo FileInfo, out string NormalizedName) {
            try {
                var r = Storage.Open(FileName, GrantedAccess, out FileNode, out var objFileDesc, out var objFileInfo, out NormalizedName);
                objFileInfo.GetStruct(out FileInfo);
                FileDesc0 = objFileDesc;
                return r;
            } catch (Win32Exception ex) {
                throw WindowsExceptionGenerator.GetIOException(ex);
            } catch (NTException ex) {
                throw WindowsExceptionGenerator.GetIOException(ex);
            }
        }

        public int Overwrite(object FileNode, object FileDesc0, uint FileAttributes, bool ReplaceFileAttributes, ulong AllocationSize, out FileInfo pFileInfo) {
            try {
                var r = Storage.OverWrite((IFSEntryPointer)FileDesc0, FileAttributes, ReplaceFileAttributes, out var FileInfo);
                FileInfo.GetStruct(out pFileInfo);
                return r;
            } catch (Win32Exception ex) {
                throw WindowsExceptionGenerator.GetIOException(ex);
            } catch (NTException ex) {
                throw WindowsExceptionGenerator.GetIOException(ex);
            }
        }

        public void Cleanup(object FileNode, object FileDesc0, string FileName, uint Flags) {
            try {
                Storage.Cleanup((IFSEntryPointer)FileDesc0, FileName, Flags, FileSystemBase.CleanupDelete);
            } catch (Win32Exception ex) {
                throw WindowsExceptionGenerator.GetIOException(ex);
            } catch (NTException ex) {
                throw WindowsExceptionGenerator.GetIOException(ex);
            }
        }

        public void Close(object FileNode, object FileDesc0) {
            try {
                Storage.Close((IFSEntryPointer)FileDesc0);
            } catch (Win32Exception ex) {
                throw WindowsExceptionGenerator.GetIOException(ex);
            } catch (NTException ex) {
                throw WindowsExceptionGenerator.GetIOException(ex);
            }
        }

        public int Read(object FileNode, object FileDesc0, IntPtr Buffer, ulong Offset, uint Length, out uint PBytesTransferred) {
            try {
                return Storage.Read((IFSEntryPointer)FileDesc0, Buffer, Offset, Length, out PBytesTransferred);
            } catch (Win32Exception ex) {
                throw WindowsExceptionGenerator.GetIOException(ex);
            } catch (NTException ex) {
                throw WindowsExceptionGenerator.GetIOException(ex);
            }
        }

        public int Write(object FileNode, object FileDesc0, IntPtr Buffer, ulong Offset, uint Length, bool WriteToEndOfFile, bool ConstrainedIo, out uint PBytesTransferred, out FileInfo pFileInfo) {
            try {
                var r = Storage.Write(FileNode, (IFSEntryPointer)FileDesc0, Buffer, Offset, Length, WriteToEndOfFile, ConstrainedIo, out PBytesTransferred, out var FileInfo);
                FileInfo.GetStruct(out pFileInfo);
                return r;
            } catch (Win32Exception ex) {
                throw WindowsExceptionGenerator.GetIOException(ex);
            } catch (NTException ex) {
                throw WindowsExceptionGenerator.GetIOException(ex);
            }
        }

        public int Flush(object FileNode, object FileDesc0, out FileInfo pFileInfo) {
            try {
                var r = Storage.Flush((IFSEntryPointer)FileDesc0, out var FileInfo);
                FileInfo.GetStruct(out pFileInfo);
                return r;
            } catch (Win32Exception ex) {
                throw WindowsExceptionGenerator.GetIOException(ex);
            } catch (NTException ex) {
                throw WindowsExceptionGenerator.GetIOException(ex);
            }
        }

        public int GetFileInfo(object FileNode, object FileDesc0, out FileInfo pFileInfo) {
            try {
                var r = Storage.GetFileInfo((IFSEntryPointer)FileDesc0, out var FileInfo);
                FileInfo.GetStruct(out pFileInfo);
                return r;
            } catch (Win32Exception ex) {
                throw WindowsExceptionGenerator.GetIOException(ex);
            } catch (NTException ex) {
                throw WindowsExceptionGenerator.GetIOException(ex);
            }
        }

        public int SetBasicInfo(object FileNode, object FileDesc0, uint FileAttributes, ulong CreationTime, ulong LastAccessTime, ulong LastWriteTime, ulong ChangeTime, out FileInfo pFileInfo) {
            try {
                var r = Storage.SetBasicInfo((IFSEntryPointer)FileDesc0, FileAttributes, CreationTime, LastAccessTime, LastWriteTime, ChangeTime, out var FileInfo);
                FileInfo.GetStruct(out pFileInfo);
                return r;
            } catch (Win32Exception ex) {
                throw WindowsExceptionGenerator.GetIOException(ex);
            } catch (NTException ex) {
                throw WindowsExceptionGenerator.GetIOException(ex);
            }
        }

        public int SetFileSize(object FileNode, object FileDesc0, ulong NewSize, bool SetAllocationSize, out FileInfo pFileInfo) {
            try {
                var r = Storage.SetFileSize(FileNode, (IFSEntryPointer)FileDesc0, NewSize, SetAllocationSize, out var FileInfo);
                FileInfo.GetStruct(out pFileInfo);
                return r;
            } catch (Win32Exception ex) {
                throw WindowsExceptionGenerator.GetIOException(ex);
            } catch (NTException ex) {
                throw WindowsExceptionGenerator.GetIOException(ex);
            }
        }

        public int CanDelete(object FileNode, object FileDesc0, string FileName) {
            try {
                return Storage.CanDelete((IFSEntryPointer)FileDesc0);
            } catch (Win32Exception ex) {
                throw WindowsExceptionGenerator.GetIOException(ex);
            } catch (NTException ex) {
                throw WindowsExceptionGenerator.GetIOException(ex);
            }
        }

        public int Rename(object FileNode, object FileDesc0, string FileName, string NewFileName, bool ReplaceIfExists) {
            try {
                return Storage.Rename(FileName, NewFileName, ReplaceIfExists);
            } catch (Win32Exception ex) {
                throw WindowsExceptionGenerator.GetIOException(ex);
            } catch (NTException ex) {
                throw WindowsExceptionGenerator.GetIOException(ex);
            }
        }

        public int GetSecurity(object FileNode, object FileDesc0, ref byte[] SecurityDescriptor) {
            try {
                return Storage.GetSecurity((IFSEntryPointer)FileDesc0, ref SecurityDescriptor);
            } catch (Win32Exception ex) {
                throw WindowsExceptionGenerator.GetIOException(ex);
            } catch (NTException ex) {
                throw WindowsExceptionGenerator.GetIOException(ex);
            }
        }

        public int SetSecurity(object FileNode, object FileDesc0, System.Security.AccessControl.AccessControlSections Sections, byte[] SecurityDescriptor) {
            try {
                return Storage.SetSecurity((IFSEntryPointer)FileDesc0, Sections, SecurityDescriptor);
            } catch (Win32Exception ex) {
                throw WindowsExceptionGenerator.GetIOException(ex);
            } catch (NTException ex) {
                throw WindowsExceptionGenerator.GetIOException(ex);
            }
        }

        public bool ReadDirectoryEntry(object FileNode, object FileDesc0, string Pattern, string Marker, ref object Context, out string FileName, out FileInfo pFileInfo) {
            try {
                var r = Storage.ReadDirectory(FileNode, (IFSEntryPointer)FileDesc0, Pattern, Marker, ref Context, out FileName, out var FileInfo);
                if (FileInfo != null) {
                    FileInfo.GetStruct(out pFileInfo);
                } else {
                    pFileInfo = default;
                }
                return r;
            } catch (Win32Exception ex) {
                throw WindowsExceptionGenerator.GetIOException(ex);
            } catch (NTException ex) {
                throw WindowsExceptionGenerator.GetIOException(ex);
            }
        }
    }
}