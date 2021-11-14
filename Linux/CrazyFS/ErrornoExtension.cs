using System;
using CrazyFS.FileSystem;
using Mono.Unix.Native;

namespace CrazyFS
{
    public static class ErrornoExtension
    {
        public static Errno ToErrno(this Result result)
        {
            if (result.NativeCode != null)
            {
                return result.NativeCode.Value;
            }
            
            switch (result.Status)
            {
                case ResultStatus.Success:
                    return 0;
                case ResultStatus.Error:
                    return Errno.EIO;
                case ResultStatus.FileNotFound:
                    return Errno.ENOENT;
                case ResultStatus.NotSet:
                    return Errno.ENODATA;
                case ResultStatus.PathNotFound:
                    return Errno.ENOENT;
                case ResultStatus.AccessDenied:
                    return Errno.EPERM;
                case ResultStatus.InvalidHandle:
                    return Errno.EBADR; //Not sure about this
                case ResultStatus.NotReady:
                    return Errno.ENXIO; //Not sure about this
                case ResultStatus.SharingViolation:
                    return Errno.EACCES; //not sure about this
                case ResultStatus.FileExists:
                    return Errno.EEXIST;
                case ResultStatus.DiskFull:
                    return Errno.ENOSPC;
                case ResultStatus.NotImplemented:
                    return Errno.ENOSYS;
                case ResultStatus.BufferTooSmall:
                    return Errno.ENOBUFS;
                case ResultStatus.BufferOverflow:
                    return Errno.ENOBUFS;  //not sure, used the same as buffertosmall
                case ResultStatus.InvalidName:
                    return Errno.EINVAL; //not sure, used 'invalid argument' 
                case ResultStatus.DirectoryNotEmpty:
                    return Errno.ENOTEMPTY;
                case ResultStatus.AlreadyExists:
                    return Errno.EEXIST;
                case ResultStatus.InternalError:
                    return Errno.EIO; //not sure, same as a regular error?
                case ResultStatus.PrivilegeNotHeld:
                    return Errno.EACCES; //not sure, just returning an access violation
                case ResultStatus.Unsuccessful:
                    return Errno.EIO; //not sure, generic I/O error?
                case ResultStatus.NotADirectory:
                    return Errno.ENOTDIR;
                case ResultStatus.InvalidParameter:
                    return Errno.EINVAL;
                case ResultStatus.EndOfFile:
                    return Errno.EIO; //not sure, same as a regular error?
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}