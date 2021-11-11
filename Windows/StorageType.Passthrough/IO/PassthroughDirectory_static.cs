using StorageBackend;
using System;
using System.IO.Abstractions;

namespace StorageType.Passthrough.IO {

    internal partial class PassthroughDirectory : PassthroughFileSystemBase {

        internal static Result Move(FileSystem filesystem, string oldpath, string newpath) {
            try {
                if (!filesystem.Directory.Exists(newpath)) {
                    filesystem.Directory.Move(oldpath, newpath);
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