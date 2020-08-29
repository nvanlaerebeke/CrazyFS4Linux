using System;

namespace StorageBackend {
    public class Win32Exception : Exception {
        private readonly int Code;

        public Win32Exception() : base() { }
        public Win32Exception(int pCode) : base() => Code = pCode;

        public Win32Exception(int pCode, string message) : base(message) => Code = pCode;

        public Win32Exception(int pCode, string message, Exception innerException) : base(message, innerException) => Code = pCode;

        public int GetCode() => Code;
    }
}
