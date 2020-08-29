using System;

namespace StorageBackend {
    public class Win32Exception : Exception {
        private readonly int Code;

        public Win32Exception(int pCode) : base() => Code = pCode;

        public int GetCode() => Code;
    }
}
