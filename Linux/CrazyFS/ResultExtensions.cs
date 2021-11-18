using System;
using CrazyFS.FileSystem;
using Mono.Unix.Native;

namespace CrazyFS.Linux
{
    public static class ResultExtensions
    {
        public static Errno ToErrno(this Result result)
        {
            try
            {
                if (result.NativeCode.HasValue) return (Errno) result.NativeCode.Value;
            }
            catch (Exception)
            {
                //fall back to result.Status
            }
            
            return result.Status switch
            {
                ResultStatus.Success => 0,
                ResultStatus.Error => Errno.EIO,
                ResultStatus.FileNotFound => Errno.ENOENT,
                ResultStatus.NotSet => Errno.ENODATA,
                ResultStatus.PathNotFound => Errno.ENOENT,
                ResultStatus.AccessDenied => Errno.EPERM,
                ResultStatus.InvalidHandle => Errno.EBADR, //Not sure about this
                ResultStatus.NotReady => Errno.ENXIO, //Not sure about this
                ResultStatus.SharingViolation => Errno.EACCES, //not sure about this
                ResultStatus.FileExists => Errno.EEXIST,
                ResultStatus.DiskFull => Errno.ENOSPC,
                ResultStatus.NotImplemented => Errno.ENOSYS,
                ResultStatus.BufferTooSmall => Errno.ENOBUFS,
                ResultStatus.BufferOverflow => Errno.ENOBUFS, //not sure, used the same as buffer to small
                ResultStatus.InvalidName => Errno.EINVAL, //not sure, used 'invalid argument' 
                ResultStatus.DirectoryNotEmpty => Errno.ENOTEMPTY,
                ResultStatus.AlreadyExists => Errno.EEXIST,
                ResultStatus.InternalError => Errno.EIO, //not sure, same as a regular error?
                ResultStatus.PrivilegeNotHeld => Errno.EACCES, //not sure, just returning an access violation
                ResultStatus.Unsuccessful => Errno.EIO, //not sure, generic I/O error?
                ResultStatus.NotADirectory => Errno.ENOTDIR,
                ResultStatus.InvalidParameter => Errno.EINVAL,
                ResultStatus.EndOfFile => Errno.EIO, //not sure, same as a regular error?
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}