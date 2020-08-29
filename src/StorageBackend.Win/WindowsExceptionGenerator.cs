using Fsp;
using System.IO;

namespace StorageBackend {
    public static class WindowsExceptionGenerator {
        private static IOException GetIOExceptionWithHResult(int HResult) {
            return new IOException(null, HResult);
        }
        private static IOException GetIOExceptionWithWin32(int Error) {
            return GetIOExceptionWithHResult(unchecked((int)(0x80070000 | Error)));
        }

        public static IOException GetIOException(NTException ex) {
            return GetIOExceptionWithWin32((int)FileSystemBase.Win32FromNtStatus(ex.GetCode()));
        }

        public static IOException GetIOException(Win32Exception ex) {
            return GetIOExceptionWithWin32(ex.GetCode());
        }
    }
}
