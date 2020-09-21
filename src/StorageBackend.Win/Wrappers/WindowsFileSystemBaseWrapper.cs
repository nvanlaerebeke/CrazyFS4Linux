using Fsp;
using Fsp.Interop;
using StorageBackend.IO;
using System;
using System.Runtime.InteropServices;

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
            return FileSystemBase.STATUS_SUCCESS;
        }

        public int Init() => HandleResult(VolumeManager.Initialize(DateTime.UtcNow.ToFileTimeUtc())).GetNtStatus();

        public void Mount(string pMountPoint, byte[] pSecurityDescriptor, bool pSynchronized, uint pDebugLog, string pLogFile) => VolumeManager.Mount(pMountPoint, pSecurityDescriptor, pSynchronized, pDebugLog, pLogFile);

        public void UnMount() => VolumeManager.UnMount();

        public int GetVolumeInfo(out VolumeInfo pVolumeInfo) {
            try {
                VolumeManager.GetVolumeInfo().GetStruct(out pVolumeInfo);
                return HandleResult(new Result(ResultStatus.Success)).GetNtStatus();
            } catch (Win32Exception ex) {
                throw WindowsExceptionGenerator.GetIOException(ex);
            } catch (NTException ex) {
                throw WindowsExceptionGenerator.GetIOException(ex);
            }
        }

        public int GetSecurityByName(string FileName, out uint FileAttributes /* or ReparsePointIndex */, ref byte[] SecurityDescriptor) {
            try {
                var r = HandleResult(Storage.GetSecurityByName(FileName, out var attr, ref SecurityDescriptor)).GetNtStatus();
                FileAttributes = (uint)attr;
                return r;
            } catch (Win32Exception ex) {
                throw WindowsExceptionGenerator.GetIOException(ex);
            } catch (NTException ex) {
                throw WindowsExceptionGenerator.GetIOException(ex);
            }
        }

        public int Create(string pFileName, uint pCreateOptions, uint GrantedAccess, uint pFileAttributes, byte[] SecurityDescriptor, ulong AllocationSize, out object FileNode, out object FileDesc0, out FileInfo pFileInfo, out string NormalizedName) {
            try {
                var r = Storage.Create(
                    pFileName,
                    (pCreateOptions & FileSystemBase.FILE_DIRECTORY_FILE) == 0,
                    System.IO.FileAccess.ReadWrite,
                    System.IO.FileShare.Read | System.IO.FileShare.Write | System.IO.FileShare.Delete,
                    System.IO.FileMode.CreateNew,
                    System.IO.FileOptions.None,
                    (System.IO.FileAttributes)pFileAttributes,
                    out var Node
                );
                if (r.Status == ResultStatus.Success) {
                    FileDesc0 = Node;

                    var e = Node.GetEntry();
                    FileNode = e;
                    e.GetStruct(out pFileInfo);
                    NormalizedName = e.Name;
                } else {
                    FileDesc0 = default;
                    FileNode = default;
                    NormalizedName = default;
                    pFileInfo = default;
                }
                return HandleResult(r).GetNtStatus();
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
                return HandleResult(r).GetNtStatus();
            } catch (Win32Exception ex) {
                throw WindowsExceptionGenerator.GetIOException(ex);
            } catch (NTException ex) {
                throw WindowsExceptionGenerator.GetIOException(ex);
            }
        }

        public int Overwrite(object FileNode, object FileDesc0, uint FileAttributes, bool ReplaceFileAttributes, ulong AllocationSize, out FileInfo pFileInfo) {
            try {
                var r = Storage.OverWrite((IFSEntryPointer)FileDesc0, (System.IO.FileAttributes)FileAttributes, ReplaceFileAttributes, out var FileInfo);
                FileInfo.GetStruct(out pFileInfo);
                return HandleResult(r).GetNtStatus();
            } catch (Win32Exception ex) {
                throw WindowsExceptionGenerator.GetIOException(ex);
            } catch (NTException ex) {
                throw WindowsExceptionGenerator.GetIOException(ex);
            }
        }

        public void Cleanup(object FileNode, object FileDesc0, string FileName, uint Flags) {
            try {
                Storage.Cleanup((IFSEntryPointer)FileDesc0, FileSystemBase.CleanupDelete == 1);
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

        public int Read(object FileNode, object FileDesc0, IntPtr Buffer, ulong Offset, uint Length, out uint pBytesTransferred) {
            try {
                var r = HandleResult(Storage.Read((IFSEntryPointer)FileDesc0, out var buffer, (long)Offset, (int)Length, out var BytesTransfered)).GetNtStatus();
                pBytesTransferred = (uint)BytesTransfered;
                Marshal.Copy(buffer, 0, Buffer, buffer.Length);
                return r;
            } catch (Win32Exception ex) {
                throw WindowsExceptionGenerator.GetIOException(ex);
            } catch (NTException ex) {
                throw WindowsExceptionGenerator.GetIOException(ex);
            }
        }

        public int Write(object FileNode, object FileDesc0, IntPtr Buffer, ulong Offset, uint Length, bool WriteToEndOfFile, bool ConstrainedIo, out uint PBytesTransferred, out FileInfo pFileInfo) {
            try {
                var p = (IFSEntryPointer)FileDesc0;
                var Bytes = new byte[Length];
                Marshal.Copy(Buffer, Bytes, 0, Bytes.Length);
                var r = Storage.Write(p, Bytes, (long)Offset, out var Transfered);
                PBytesTransferred = (uint)Transfered;
                p.GetEntry().GetStruct(out pFileInfo);
                return HandleResult(r).GetNtStatus();
            } catch (Win32Exception ex) {
                throw WindowsExceptionGenerator.GetIOException(ex);
            } catch (NTException ex) {
                throw WindowsExceptionGenerator.GetIOException(ex);
            }
        }

        public int Flush(object FileNode, object FileDesc0, out FileInfo pFileInfo) {
            try {
                var r = Storage.Flush((IFSEntryPointer)FileDesc0);
                ((IFSEntryPointer)FileDesc0).GetEntry().GetStruct(out pFileInfo);
                return HandleResult(r).GetNtStatus();
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
                return HandleResult(r).GetNtStatus();
            } catch (Win32Exception ex) {
                throw WindowsExceptionGenerator.GetIOException(ex);
            } catch (NTException ex) {
                throw WindowsExceptionGenerator.GetIOException(ex);
            }
        }

        public int SetBasicInfo(object FileNode, object FileDesc0, uint FileAttributes, ulong CreationTime, ulong LastAccessTime, ulong LastWriteTime, ulong ChangeTime, out FileInfo pFileInfo) {
            try {
                var p = (IFSEntryPointer)FileDesc0;
                var r = Storage.SetBasicInfo(
                    p,
                    (System.IO.FileAttributes)FileAttributes,
                    DateTime.FromFileTimeUtc((long)CreationTime),
                    DateTime.FromFileTimeUtc((long)LastAccessTime),
                    DateTime.FromFileTimeUtc((long)LastWriteTime),
                    DateTime.FromFileTimeUtc((long)ChangeTime)
                );
                p.GetEntry().GetStruct(out pFileInfo);
                return HandleResult(r).GetNtStatus();
            } catch (Win32Exception ex) {
                throw WindowsExceptionGenerator.GetIOException(ex);
            } catch (NTException ex) {
                throw WindowsExceptionGenerator.GetIOException(ex);
            }
        }

        public int SetFileSize(object FileNode, object FileDesc0, ulong NewSize, bool SetAllocationSize, out FileInfo pFileInfo) {
            try {
                var r = Storage.SetFileSize((IFSEntryPointer)FileDesc0, (long)NewSize);
                ((IFSEntryPointer)FileDesc0).GetEntry().GetStruct(out pFileInfo);
                return HandleResult(r).GetNtStatus();
            } catch (Win32Exception ex) {
                throw WindowsExceptionGenerator.GetIOException(ex);
            } catch (NTException ex) {
                throw WindowsExceptionGenerator.GetIOException(ex);
            }
        }

        public int CanDelete(object FileNode, object FileDesc0, string FileName) {
            try {
                return HandleResult(Storage.CanDelete((IFSEntryPointer)FileDesc0)).GetNtStatus();
            } catch (Win32Exception ex) {
                throw WindowsExceptionGenerator.GetIOException(ex);
            } catch (NTException ex) {
                throw WindowsExceptionGenerator.GetIOException(ex);
            }
        }

        public int Rename(object FileNode, object FileDesc0, string FileName, string NewFileName, bool ReplaceIfExists) {
            try {
                return HandleResult(Storage.Rename(FileName, NewFileName, ReplaceIfExists)).GetNtStatus();
            } catch (Win32Exception ex) {
                throw WindowsExceptionGenerator.GetIOException(ex);
            } catch (NTException ex) {
                throw WindowsExceptionGenerator.GetIOException(ex);
            }
        }

        public int GetSecurity(object FileNode, object FileDesc0, ref byte[] SecurityDescriptor) {
            try {
                var r = HandleResult(Storage.GetSecurity((IFSEntryPointer)FileDesc0, out var s)).GetNtStatus();
                SecurityDescriptor = s.GetSecurityDescriptorBinaryForm();
                return r;
            } catch (Win32Exception ex) {
                throw WindowsExceptionGenerator.GetIOException(ex);
            } catch (NTException ex) {
                throw WindowsExceptionGenerator.GetIOException(ex);
            }
        }

        public int SetSecurity(object FileNode, object FileDesc0, System.Security.AccessControl.AccessControlSections Sections, byte[] SecurityDescriptor) {
            try {
                return HandleResult(Storage.SetSecurity((IFSEntryPointer)FileDesc0, Sections, SecurityDescriptor)).GetNtStatus();
            } catch (Win32Exception ex) {
                throw WindowsExceptionGenerator.GetIOException(ex);
            } catch (NTException ex) {
                throw WindowsExceptionGenerator.GetIOException(ex);
            }
        }

        public bool ReadDirectoryEntry(object FileNode, object FileDesc0, string Pattern, string Marker, ref object Context, out string FileName, out FileInfo pFileInfo) {
            try {
                var r = Storage.ReadDirectory((IFSEntryPointer)FileDesc0, Pattern, true, Marker, out var pEntries);
                /*if (FileInfo != null) {
                    FileInfo.GetStruct(out pFileInfo);
                } else {
                    pFileInfo = default;
                }*/
                FileName = default;
                pFileInfo = default;
                return r.Status == ResultStatus.Success;
            } catch (Win32Exception ex) {
                throw WindowsExceptionGenerator.GetIOException(ex);
            } catch (NTException ex) {
                throw WindowsExceptionGenerator.GetIOException(ex);
            }
        }

        private Result HandleResult(Result pResult) {
            try {
                switch (pResult.Status) {
                    case ResultStatus.EndOfFile:
                        throw new NTException(FileSystemBase.STATUS_END_OF_FILE);
                }
            } catch (Win32Exception ex) {
                throw WindowsExceptionGenerator.GetIOException(ex);
            } catch (NTException ex) {
                throw WindowsExceptionGenerator.GetIOException(ex);
            }
            return pResult;
        }
    }
}