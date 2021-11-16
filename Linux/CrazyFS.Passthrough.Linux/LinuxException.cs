using System;
using Mono.Unix.Native;

namespace CrazyFS.Passthrough.Linux
{
    public class LinuxException : Exception
    {
        public Errno Code { get; }

        public LinuxException(Errno code)
        {
            Code = code;
        }
    }
}