using System;

namespace CrazyFS.CommandLine {
    internal class UsageException : Exception {
        public UsageException(string Message = null) : base(Message) {
            HasMessage = null != Message;
        }

        public UsageException() : base() {
        }

        public UsageException(string message, Exception innerException) : base(message, innerException) {
        }

        public bool HasMessage;
    }
}
