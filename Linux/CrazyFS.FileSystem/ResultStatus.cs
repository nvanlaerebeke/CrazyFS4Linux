namespace CrazyFS.FileSystem {

    public enum ResultStatus {

        /// <summary>
        /// Success - The operation completed successfully.
        /// </summary>
        Success,

        /// <summary>
        /// Error - Incorrect function.
        /// </summary>
        Error,

        /// <summary>
        /// Error - The system cannot find the file specified.
        /// </summary>
        FileNotFound,

        /// <summary>
        /// Error - The system cannot find the path specified.
        /// </summary>
        PathNotFound,

        /// <summary>
        /// Error - Access is denied.
        /// </summary>
        AccessDenied,

        /// <summary>
        /// Error - The handle is invalid.
        /// </summary>
        InvalidHandle,

        /// <summary>
        /// Warning - The device is not ready.
        /// </summary>
        NotReady,

        /// <summary>
        /// Error - The process cannot access the file because it is being used
        /// by another process.
        /// </summary>
        SharingViolation,

        /// <summary>
        /// Error - The file exists.
        /// </summary>
        FileExists,

        /// <summary>
        /// Error - There is not enough space on the disk.
        /// </summary>
        DiskFull,

        /// <summary>
        /// Error - This function is not supported on this system.
        /// </summary>
        NotImplemented,

        /// <summary>
        /// Error - The data area passed to a system call is too small.
        /// </summary>
        BufferTooSmall,

        /// <summary>
        /// Warning - The data area passed to a system call is too small.
        /// </summary>
        BufferOverflow,

        /// <summary>
        /// Error - The filename, directory name, or volume label syntax is
        /// incorrect.
        /// </summary>
        InvalidName,

        /// <summary>
        /// Error - The directory is not empty.
        /// </summary>
        DirectoryNotEmpty,

        /// <summary>
        /// Error - Cannot create a file when that file already exists.
        /// </summary>
        AlreadyExists,

        /// <summary>
        /// Error - An exception occurred in the service when handling the
        /// control request.
        /// </summary>
        InternalError,

        /// <summary>
        /// Error - A required privilege is not held by the client.
        /// </summary>
        PrivilegeNotHeld,

        /// <summary>
        /// Error - The requested operation was unsuccessful.
        /// </summary>
        Unsuccessful,

        /// <summary>
        /// Error - A directory semantics call was made but the accessed file was not a directory.
        /// </summary>
        NotADirectory,

        /// <summary>
        /// Error - The parameter is incorrect.
        /// </summary>
        InvalidParameter,

        /// <summary>
        /// Error - End of File reached
        /// </summary>
        EndOfFile
    }
}