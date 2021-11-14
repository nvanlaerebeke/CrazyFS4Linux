using System;
using Mono.Unix.Native;

namespace CrazyFS.Linux
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