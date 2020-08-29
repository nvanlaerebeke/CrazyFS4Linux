using System;

namespace StorageBackend {
    public class NTException : Exception {
        private readonly int Code;

        public NTException() : base() { }
        public NTException(int pCode) : base() => Code = pCode;

        public NTException(int pCode, string message) : base(message) => Code = pCode;

        public NTException(int pCode, string message, Exception innerException) : base(message, innerException) => Code = pCode;

        public int GetCode() => Code;
    }
}
