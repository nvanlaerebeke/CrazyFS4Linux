using System;
using System.IO;

namespace CrazyFS.FileSystem
{
    public static class ExceptionExtensions
    {
        public static Result GetResult(this Exception exception)
        {
            return exception switch
            {
                UnauthorizedAccessException => new Result(ResultStatus.AccessDenied),
                FileNotFoundException => new Result(ResultStatus.PathNotFound),
                NativeException ex => new Result(ex.Code),
                _ => new Result(ResultStatus.Error)
            };
        }
    }
}