using System;
using CrazyFS.FileSystem;
using DokanNet;

namespace CrazyFS
{
    public static class ResultExtensions
    {
        public static NtStatus ToNtStatus(this Result result)
        {
            try
            {
                if (result.NativeCode.HasValue) return (NtStatus) result.NativeCode.Value;
            }
            catch (Exception)
            {
                //fall back to result.Status
            }
            
            return result.Status switch
            {
                ResultStatus.Success => DokanResult.Success,
                ResultStatus.Error => DokanResult.Error,
                ResultStatus.FileNotFound => DokanResult.FileNotFound,
                ResultStatus.NotSet => DokanResult.Error,
                ResultStatus.PathNotFound => DokanResult.PathNotFound,
                ResultStatus.AccessDenied => DokanResult.AccessDenied,
                ResultStatus.InvalidHandle => DokanResult.InvalidHandle, 
                ResultStatus.NotReady => DokanResult.NotReady, 
                ResultStatus.SharingViolation => DokanResult.SharingViolation,
                ResultStatus.FileExists => DokanResult.FileExists,
                ResultStatus.DiskFull => DokanResult.DiskFull,
                ResultStatus.NotImplemented => DokanResult.NotImplemented,
                ResultStatus.BufferTooSmall => DokanResult.BufferTooSmall,
                ResultStatus.BufferOverflow => DokanResult.BufferOverflow, 
                ResultStatus.InvalidName => DokanResult.InvalidName,
                ResultStatus.DirectoryNotEmpty => DokanResult.DirectoryNotEmpty,
                ResultStatus.AlreadyExists => DokanResult.AlreadyExists,
                ResultStatus.InternalError => DokanResult.InternalError,
                ResultStatus.PrivilegeNotHeld => DokanResult.PrivilegeNotHeld,
                ResultStatus.Unsuccessful => DokanResult.Unsuccessful,
                ResultStatus.NotADirectory => DokanResult.NotADirectory,
                ResultStatus.InvalidParameter => DokanResult.InvalidParameter,
                ResultStatus.EndOfFile => DokanResult.Error, //not sure, same as a regular error?
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}