using DokanNet;

namespace StorageBackend.Win.Dokan {

    public static class ResultExtension {

        public static NtStatus GetNtStatus(
            this Result pResult
        ) {
            switch (pResult.Status) {
                case ResultStatus.AccessDenied:
                    return NtStatus.AccessDenied;

                case ResultStatus.AlreadyExists:
                    return NtStatus.ObjectNameCollision;

                case ResultStatus.BufferOverflow:
                    return NtStatus.BufferOverflow;

                case ResultStatus.BufferTooSmall:
                    return NtStatus.BufferTooSmall;

                case ResultStatus.DirectoryNotEmpty:
                    return NtStatus.DirectoryNotEmpty;

                case ResultStatus.DiskFull:
                    return NtStatus.DiskFull;

                case ResultStatus.Error:
                    return NtStatus.Error;

                case ResultStatus.FileExists:
                    return NtStatus.ObjectNameCollision;

                case ResultStatus.FileNotFound:
                    return NtStatus.ObjectNameNotFound;

                case ResultStatus.InternalError:
                    return NtStatus.InternalError;

                case ResultStatus.InvalidHandle:
                    return NtStatus.InvalidHandle;

                case ResultStatus.InvalidName:
                    return NtStatus.ObjectNameInvalid;

                case ResultStatus.InvalidParameter:
                    return NtStatus.InvalidParameter;

                case ResultStatus.NotADirectory:
                    return NtStatus.NotADirectory;

                case ResultStatus.NotImplemented:
                    return NtStatus.NotImplemented;

                case ResultStatus.NotReady:
                    return NtStatus.DeviceBusy;

                case ResultStatus.PathNotFound:
                    return NtStatus.ObjectPathNotFound;

                case ResultStatus.PrivilegeNotHeld:
                    return NtStatus.PrivilegeNotHeld;

                case ResultStatus.SharingViolation:
                    return NtStatus.SharingViolation;

                case ResultStatus.Success:
                    return NtStatus.Success;

                case ResultStatus.Unsuccessful:
                    return NtStatus.Unsuccessful;

                default:
                    throw new System.Exception($"Unable to convert {pResult.Status.ToString()} to Dokan NtStatus");
            }
        }
    }
}