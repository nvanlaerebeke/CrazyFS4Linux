using Fsp;
using System;
using FileInfo = Fsp.Interop.FileInfo;
using VolumeInfo = Fsp.Interop.VolumeInfo;

namespace StorageBackend.Win {
    public class WindowsBackend : FileSystemBase, IFileSystem {
        private readonly IStorageType Backend;
        private FileSystemHost Host = null;

        public WindowsBackend(IStorageType pBackend) {
            Backend = pBackend;
        }

        public override int ExceptionHandler(Exception ex) {
            int HResult = ex.HResult; /* needs Framework 4.5 */
            if (0x80070000 == (HResult & 0xFFFF0000)) {
                return NtStatusFromWin32((uint)HResult & 0xFFFF);
            }
            return FileSystemStatus.STATUS_UNEXPECTED_IO_ERROR;
        }
        public override int Init(object Host0) {
            try {
                return Backend.Init(new CrazyFSFileSystemHost((FileSystemHost)Host0));
            } catch (Win32Exception ex) {
                throw WindowsExceptionGenerator.GetIOException(ex);
            } catch (NTException ex) {
                throw WindowsExceptionGenerator.GetIOException(ex);
            }
        }
        public override int GetVolumeInfo(out Fsp.Interop.VolumeInfo pVolumeInfo) {
            try {
                var r = Backend.GetVolumeInfo(out var VolumeInfo);
                VolumeInfo.GetStruct(out pVolumeInfo);
                return r;
            } catch (Win32Exception ex) {
                throw WindowsExceptionGenerator.GetIOException(ex);
            } catch (NTException ex) {
                throw WindowsExceptionGenerator.GetIOException(ex);
            }
        }
        public override int GetSecurityByName(string FileName, out uint FileAttributes /* or ReparsePointIndex */, ref byte[] SecurityDescriptor) {
            try {
                return Backend.GetSecurityByName(FileName, out FileAttributes, ref SecurityDescriptor);
            } catch (Win32Exception ex) {
                throw WindowsExceptionGenerator.GetIOException(ex);
            } catch (NTException ex) {
                throw WindowsExceptionGenerator.GetIOException(ex);
            }
        }
        public override int Create(string FileName, uint CreateOptions, uint GrantedAccess, uint FileAttributes, byte[] SecurityDescriptor, ulong AllocationSize, out object FileNode, out object FileDesc0, out FileInfo pFileInfo, out string NormalizedName) {
            try {
                var r = Backend.Create(FileName, CreateOptions, GrantedAccess, FileAttributes, SecurityDescriptor, out FileNode, out FileDesc0, out var FileInfo, out NormalizedName);
                FileInfo.GetStruct(out pFileInfo);
                return r;
            } catch (Win32Exception ex) {
                throw WindowsExceptionGenerator.GetIOException(ex);
            } catch (NTException ex) {
                throw WindowsExceptionGenerator.GetIOException(ex);
            }
        }
        public override int Open(string FileName, uint CreateOptions, uint GrantedAccess, out object FileNode, out object FileDesc0, out FileInfo FileInfo, out string NormalizedName) {
            try {
                var r = Backend.Open(FileName, GrantedAccess, out FileNode, out var objFileDesc, out var objFileInfo, out NormalizedName);
                objFileInfo.GetStruct(out FileInfo);
                FileDesc0 = objFileDesc;
                return r;
            } catch (Win32Exception ex) {
                throw WindowsExceptionGenerator.GetIOException(ex);
            } catch (NTException ex) {
                throw WindowsExceptionGenerator.GetIOException(ex);
            }
        }
        public override int Overwrite(object FileNode, object FileDesc0, uint FileAttributes, bool ReplaceFileAttributes, ulong AllocationSize, out FileInfo pFileInfo) {
            try {
                var r = Backend.OverWrite((IFileDescriptor)FileDesc0, FileAttributes, ReplaceFileAttributes, out var FileInfo);
                FileInfo.GetStruct(out pFileInfo);
                return r;
            } catch (Win32Exception ex) {
                throw WindowsExceptionGenerator.GetIOException(ex);
            } catch (NTException ex) {
                throw WindowsExceptionGenerator.GetIOException(ex);
            }
        }
        public override void Cleanup(object FileNode, object FileDesc0, string FileName, uint Flags) {
            try {
                Backend.Cleanup((IFileDescriptor)FileDesc0, FileName, Flags, CleanupDelete);
            } catch (Win32Exception ex) {
                throw WindowsExceptionGenerator.GetIOException(ex);
            } catch (NTException ex) {
                throw WindowsExceptionGenerator.GetIOException(ex);
            }
        }
        public override void Close(object FileNode, object FileDesc0) {
            try {
                Backend.Close((IFileDescriptor)FileDesc0);
            } catch (Win32Exception ex) {
                throw WindowsExceptionGenerator.GetIOException(ex);
            } catch (NTException ex) {
                throw WindowsExceptionGenerator.GetIOException(ex);
            }
        }
        public override int Read(object FileNode, object FileDesc0, IntPtr Buffer, ulong Offset, uint Length, out uint PBytesTransferred) {
            try {
                return Backend.Read((IFileDescriptor)FileDesc0, Buffer, Offset, Length, out PBytesTransferred);
            } catch (Win32Exception ex) {
                throw WindowsExceptionGenerator.GetIOException(ex);
            } catch (NTException ex) {
                throw WindowsExceptionGenerator.GetIOException(ex);
            }
        }
        public override int Write(object FileNode, object FileDesc0, IntPtr Buffer, ulong Offset, uint Length, bool WriteToEndOfFile, bool ConstrainedIo, out uint PBytesTransferred, out FileInfo pFileInfo) {
            try {
                var r = Backend.Write(FileNode, (IFileDescriptor)FileDesc0, Buffer, Offset, Length, WriteToEndOfFile, ConstrainedIo, out PBytesTransferred, out var FileInfo);
                FileInfo.GetStruct(out pFileInfo);
                return r;
            } catch (Win32Exception ex) {
                throw WindowsExceptionGenerator.GetIOException(ex);
            } catch (NTException ex) {
                throw WindowsExceptionGenerator.GetIOException(ex);
            }
        }
        public override int Flush(object FileNode, object FileDesc0, out FileInfo pFileInfo) {
            try {
                var r = Backend.Flush((IFileDescriptor)FileDesc0, out var FileInfo);
                FileInfo.GetStruct(out pFileInfo);
                return r;
            } catch (Win32Exception ex) {
                throw WindowsExceptionGenerator.GetIOException(ex);
            } catch (NTException ex) {
                throw WindowsExceptionGenerator.GetIOException(ex);
            }
        }
        public override int GetFileInfo(object FileNode, object FileDesc0, out FileInfo pFileInfo) {
            try {
                var r = Backend.GetFileInfo((IFileDescriptor)FileDesc0, out var FileInfo);
                FileInfo.GetStruct(out pFileInfo);
                return r;
            } catch (Win32Exception ex) {
                throw WindowsExceptionGenerator.GetIOException(ex);
            } catch (NTException ex) {
                throw WindowsExceptionGenerator.GetIOException(ex);
            }
        }
        public override int SetBasicInfo(object FileNode, object FileDesc0, uint FileAttributes, ulong CreationTime, ulong LastAccessTime, ulong LastWriteTime, ulong ChangeTime, out FileInfo pFileInfo) {
            try {
                var r = Backend.SetBasicInfo((IFileDescriptor)FileDesc0, FileAttributes, CreationTime, LastAccessTime, LastWriteTime, ChangeTime, out var FileInfo);
                FileInfo.GetStruct(out pFileInfo);
                return r;
            } catch (Win32Exception ex) {
                throw WindowsExceptionGenerator.GetIOException(ex);
            } catch (NTException ex) {
                throw WindowsExceptionGenerator.GetIOException(ex);
            }
        }
        public override int SetFileSize(object FileNode, object FileDesc0, ulong NewSize, bool SetAllocationSize, out FileInfo pFileInfo) {
            try {
                var r = Backend.SetFileSize(FileNode, (IFileDescriptor)FileDesc0, NewSize, SetAllocationSize, out var FileInfo);
                FileInfo.GetStruct(out pFileInfo);
                return r;
            } catch (Win32Exception ex) {
                throw WindowsExceptionGenerator.GetIOException(ex);
            } catch (NTException ex) {
                throw WindowsExceptionGenerator.GetIOException(ex);
            }
        }
        public override int CanDelete(object FileNode, object FileDesc0, string FileName) {
            try {
                return Backend.CanDelete((IFileDescriptor)FileDesc0);
            } catch (Win32Exception ex) {
                throw WindowsExceptionGenerator.GetIOException(ex);
            } catch (NTException ex) {
                throw WindowsExceptionGenerator.GetIOException(ex);
            }
        }

        public override int Rename(object FileNode, object FileDesc0, string FileName, string NewFileName, bool ReplaceIfExists) {
            try {
                return Backend.Rename(FileName, NewFileName, ReplaceIfExists);
            } catch (Win32Exception ex) {
                throw WindowsExceptionGenerator.GetIOException(ex);
            } catch (NTException ex) {
                throw WindowsExceptionGenerator.GetIOException(ex);
            }
        }

        public override int GetSecurity(object FileNode, object FileDesc0, ref byte[] SecurityDescriptor) {
            try {
                return Backend.GetSecurity((IFileDescriptor)FileDesc0, ref SecurityDescriptor);
            } catch (Win32Exception ex) {
                throw WindowsExceptionGenerator.GetIOException(ex);
            } catch (NTException ex) {
                throw WindowsExceptionGenerator.GetIOException(ex);
            }
        }
        public override int SetSecurity(object FileNode, object FileDesc0, System.Security.AccessControl.AccessControlSections Sections, byte[] SecurityDescriptor) {
            try {
                return Backend.SetSecurity((IFileDescriptor)FileDesc0, Sections, SecurityDescriptor);
            } catch (Win32Exception ex) {
                throw WindowsExceptionGenerator.GetIOException(ex);
            } catch (NTException ex) {
                throw WindowsExceptionGenerator.GetIOException(ex);
            }
        }
        public override bool ReadDirectoryEntry(object FileNode, object FileDesc0, string Pattern, string Marker, ref object Context, out string FileName, out FileInfo pFileInfo) {
            try {
                var r = Backend.ReadDirectory(FileNode, (IFileDescriptor)FileDesc0, Pattern, Marker, ref Context, out FileName, out var FileInfo);
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

        public void Mount(string pMountPoint, byte[] pSecurityDescriptor, bool pSynchronized, uint pDebugLog, string pLogFile) {
            Host = new VolumeManager().Mount(this, pMountPoint, pSecurityDescriptor, pSynchronized, pDebugLog, pLogFile);
        }
        public void UnMount() {
            Host.Unmount();
        }
    }

}