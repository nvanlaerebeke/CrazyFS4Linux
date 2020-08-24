using Fsp;
using System;
using System.IO;

namespace StorageBackend {
    public static class ExceptionGenerator {
        public static void ThrowIoExceptionWithHResult(int HResult) {
            throw new IOException(null, HResult);
        }
        public static void ThrowIoExceptionWithWin32(int Error) {
            ThrowIoExceptionWithHResult(unchecked((int)(0x80070000 | Error)));
        }
        public static void ThrowIoExceptionWithNtStatus(int Status) {
            ThrowIoExceptionWithWin32(Status);
        }

        public static int Handle(Exception ex) {
            int HResult = ex.HResult; /* needs Framework 4.5 */
            if (0x80070000 == (HResult & 0xFFFF0000)) {
                return FileSystemBase.NtStatusFromWin32((uint)HResult & 0xFFFF);
            }
            return FileSystemStatus.STATUS_UNEXPECTED_IO_ERROR;
        }
    }
}