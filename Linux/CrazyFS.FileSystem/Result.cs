namespace CrazyFS.FileSystem {

    public class Result {
        public ResultStatus Status { get; }

        public Result(ResultStatus status)
        {
            Status = status;
        }
    }
}