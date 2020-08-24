using Fsp;
using System.IO;

namespace StorageType.Passthrough {
    internal static class ExceptionGenerator {
        public static void ThrowIoExceptionWithHResult(int HResult) {
            throw new IOException(null, HResult);
        }
        public static void ThrowIoExceptionWithWin32(int Error) {
            ThrowIoExceptionWithHResult(unchecked((int)(0x80070000 | Error)));
        }
        public static void ThrowIoExceptionWithNtStatus(int Status) {
            ThrowIoExceptionWithWin32((int)FileSystemBase.Win32FromNtStatus(Status));
        }
    }
}
