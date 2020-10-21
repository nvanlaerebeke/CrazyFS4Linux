using StorageBackend;
using System;
using System.IO;

namespace StorageType.Passthrough.IO {

    internal partial class PassthroughDirectory : PassthroughFileSystemBase {

        internal static Result Move(string oldpath, string newpath) {
            try {
                if (!Directory.Exists(newpath)) {
                    Directory.Move(oldpath, newpath);
                    return new Result(ResultStatus.Success);
                } else {
                    return new Result(ResultStatus.AccessDenied);
                }
            } catch (UnauthorizedAccessException) {
                return new Result(ResultStatus.AccessDenied);
            }
        }
    }
}