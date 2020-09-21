using Fsp;

namespace StorageBackend.Win.Winfsp {

    public static class ResultExtension {

        public static int GetNtStatus(
            this Result pResult
        ) {
            switch (pResult.Status) {
                case ResultStatus.AccessDenied:
                    return FileSystemBase.STATUS_ACCESS_DENIED;

                case ResultStatus.AlreadyExists:
                    return FileSystemBase.STATUS_OBJECT_NAME_COLLISION;

                case ResultStatus.BufferOverflow:
                    return FileSystemBase.STATUS_BUFFER_OVERFLOW;

                case ResultStatus.BufferTooSmall:
                    return FileSystemBase.STATUS_BUFFER_TOO_SMALL;

                case ResultStatus.DirectoryNotEmpty:
                    return FileSystemBase.STATUS_DIRECTORY_NOT_EMPTY;

                case ResultStatus.DiskFull:
                    return FileSystemBase.STATUS_DISK_FULL;

                case ResultStatus.Error:
                    return FileSystemBase.STATUS_UNEXPECTED_IO_ERROR;

                case ResultStatus.FileExists:
                    return FileSystemBase.STATUS_OBJECT_NAME_COLLISION;

                case ResultStatus.FileNotFound:
                    return FileSystemBase.STATUS_OBJECT_NAME_NOT_FOUND;

                case ResultStatus.InternalError:
                    return FileSystemBase.STATUS_INTERNAL_ERROR;

                case ResultStatus.InvalidHandle:
                    return FileSystemBase.STATUS_INVALID_HANDLE;

                case ResultStatus.InvalidName:
                    return FileSystemBase.STATUS_OBJECT_NAME_INVALID;

                case ResultStatus.InvalidParameter:
                    return FileSystemBase.STATUS_INVALID_PARAMETER;

                case ResultStatus.NotADirectory:
                    return FileSystemBase.STATUS_NOT_A_DIRECTORY;

                case ResultStatus.NotImplemented:
                    return FileSystemBase.STATUS_NOT_IMPLEMENTED;

                case ResultStatus.NotReady:
                    return FileSystemBase.STATUS_DEVICE_BUSY;

                case ResultStatus.PathNotFound:
                    return FileSystemBase.STATUS_OBJECT_PATH_NOT_FOUND;

                case ResultStatus.PrivilegeNotHeld:
                    return FileSystemBase.STATUS_PRIVILEGE_NOT_HELD;

                case ResultStatus.SharingViolation:
                    return FileSystemBase.STATUS_SHARING_VIOLATION;

                case ResultStatus.Success:
                    return FileSystemBase.STATUS_SUCCESS;

                case ResultStatus.Unsuccessful:
                    return FileSystemBase.STATUS_UNSUCCESSFUL;

                default:
                    throw new System.Exception($"Unable to convert {pResult.Status.ToString()} to Winfsp STATUS");
            }
        }
    }
}