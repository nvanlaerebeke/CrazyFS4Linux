using StorageBackend;
using StorageBackend.IO;
using System;
using System.IO;
using System.Runtime.InteropServices;

namespace StorageType.Passthrough.IO {

    internal partial class PassthroughFile {

        public int Flush(out IEntry pEntry) {
            pEntry = GetEntry();
            Stream?.Flush(true);
            return FileSystemStatus.STATUS_SUCCESS;
        }

        public void Close() => Stream?.Dispose();

        /// <summary>
        /// ToDo: SetSecurity was removed for .net standard support
        /// </summary>
        internal static PassthroughFile Create(string pFileName, uint pGrantedAccess, uint pFileAttributes, byte[] pSecurityDescriptor, out IEntry pEntry) {
            PassthroughFile objFileDesc = null;
            try {
                objFileDesc = new PassthroughFile(
                    new FileStream(pFileName, FileMode.CreateNew, FileAccess.ReadWrite, FileShare.Read | FileShare.Write | FileShare.Delete, 4096, FileOptions.SequentialScan)
                );
                //objFileDesc.SetSecurity((FileSystemRights)pGrantedAccess | FileSystemRights.WriteAttributes, pSecurityDescriptor);
                _ = objFileDesc.SetBasicInfo(pFileAttributes | (uint)FileAttributes.Archive, 0, 0, 0, out pEntry);
                return objFileDesc;
            } catch {
                if (objFileDesc?.Stream != null) {
                    objFileDesc.Stream.Dispose();
                }
                throw;
            }
        }

        internal static PassthroughFile Open(string pFileName, uint pGrantedAccess, out IEntry pEntry) {
            PassthroughFile objFileDesc = null;
            try {
                //SetSecurity(pFileName, (FileSystemRights)pGrantedAccess);
                objFileDesc = new PassthroughFile(
                    new FileStream(pFileName, FileMode.Open, FileAccess.ReadWrite, FileShare.Read | FileShare.Write | FileShare.Delete, 4096, false)
                );
            } catch (FileNotFoundException) {
                //File not found can be safely ignored
                //this will let w/e is doing the listing know the file/dir does not exist
                throw;
            } catch {
                if (objFileDesc?.Stream != null) {
                    objFileDesc.Stream.Dispose();
                }
                throw;
            }
            pEntry = objFileDesc.GetEntry();
            return objFileDesc;
        }

        public static int Move(string pOldPath, string pNewPath) {
            new FileInfo(pOldPath).MoveTo(pNewPath);
            return FileSystemStatus.STATUS_SUCCESS;
        }

        public int Read(IntPtr pBuffer, ulong pOffset, uint pLength, out uint pBytesTransferred) {
            if (pOffset >= (ulong)Stream.Length) {
                throw new NTException(FileSystemStatus.STATUS_END_OF_FILE);
            }
            var Bytes = new byte[pLength];
            _ = Stream.Seek((long)pOffset, SeekOrigin.Begin);
            pBytesTransferred = (uint)Stream.Read(Bytes, 0, Bytes.Length);
            Marshal.Copy(Bytes, 0, pBuffer, Bytes.Length);
            return FileSystemStatus.STATUS_SUCCESS;
        }

        public int Write(IntPtr Buffer, ulong Offset, uint Length, bool WriteToEndOfFile, bool ConstrainedIo, out uint PBytesTransferred, out IEntry pEntry) {
            if (ConstrainedIo) {
                if (Offset >= (ulong)Stream.Length) {
                    PBytesTransferred = default;
                    pEntry = default;
                    return FileSystemStatus.STATUS_SUCCESS;
                }
                if (Offset + Length > (ulong)Stream.Length) {
                    Length = (uint)((ulong)Stream.Length - Offset);
                }
            }
            var Bytes = new byte[Length];
            Marshal.Copy(Buffer, Bytes, 0, Bytes.Length);
            if (!WriteToEndOfFile) {
                _ = Stream.Seek((long)Offset, SeekOrigin.Begin);
            }
            Stream.Write(Bytes, 0, Bytes.Length);
            PBytesTransferred = (uint)Bytes.Length;
            pEntry = GetEntry();
            return FileSystemStatus.STATUS_SUCCESS;
        }

        public void Cleanup(uint pFlags, uint pCleanupDelete) {
            if ((pFlags & pCleanupDelete) != 0) {
                SetDisposition(true);
                Stream?.Dispose();
            }
        }
    }
}