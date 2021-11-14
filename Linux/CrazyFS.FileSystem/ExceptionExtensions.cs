using System;
using System.IO;
using CrazyFS.Linux;

namespace CrazyFS.FileSystem
{
    internal static class ExceptionExtensions
    {
        public static Result GetResult(this Exception exception)
        {
            return exception switch
            {
                UnauthorizedAccessException => new Result(ResultStatus.AccessDenied),
                FileNotFoundException => new Result(ResultStatus.PathNotFound),
                LinuxException ex => new Result(ex.Code),
                _ => new Result(ResultStatus.Error)
            };
        }
    }
}