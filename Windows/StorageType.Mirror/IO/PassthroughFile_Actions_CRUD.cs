using StorageBackend;
using StorageBackend.IO;
using System;
using System.IO;
using System.Runtime.InteropServices;

namespace StorageType.Passthrough.IO {

    internal partial class PassthroughFile {

        public int Flush(out IEntry pEntry) {
            pEntry = GetEntry();
            Stream?.Flush();
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
    }
}