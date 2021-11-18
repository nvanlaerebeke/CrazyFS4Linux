namespace CrazyFS.FileSystem {

    public class Result
    {
        public int? NativeCode { get; }
        
        public ResultStatus Status { get; }

        public Result(ResultStatus status)
        {
            Status = status;
        }

        public Result(int nativeCode)
        {
            NativeCode = nativeCode;
        }
    }
}