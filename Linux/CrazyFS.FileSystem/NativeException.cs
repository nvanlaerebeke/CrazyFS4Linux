using System;

namespace CrazyFS.FileSystem
{
    public class NativeException : Exception
    {
        public int Code { get; }

        public NativeException(int code)
        {
            Code = code;
        }
    }
}