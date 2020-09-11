using System;

namespace StorageBackend {

    public class NTException : Exception, INTException {
        private readonly int Code;

        public NTException(int pCode) : base() => Code = pCode;

        public int GetCode() => Code;
    }
}