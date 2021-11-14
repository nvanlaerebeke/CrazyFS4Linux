using System;
using System.IO;

namespace CrazyFS.FileSystem
{
    internal static class ExceptionExtensions
    {
        public static Result GetResult(this Exception exception)
        {
            if(exception is UnauthorizedAccessException)
            {
                return new Result(ResultStatus.AccessDenied);
            } 
            if (exception is FileNotFoundException)
            {
                return new Result(ResultStatus.PathNotFound);
            }
            return new Result(ResultStatus.Error);
        }
    }
}