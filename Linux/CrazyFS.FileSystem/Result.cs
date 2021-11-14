using Mono.Unix.Native;

namespace CrazyFS.FileSystem {

    public class Result
    {
        public Errno? NativeCode { get; }
        
        public ResultStatus Status { get; }

        public Result(ResultStatus status)
        {
            Status = status;
        }

        public Result(Errno nativeCode)
        {
            NativeCode = nativeCode;
        }
    }
}