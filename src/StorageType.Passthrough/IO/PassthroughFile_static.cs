using StorageBackend;
using StorageBackend.IO;
using System;
using System.IO.Abstractions;
using System.Runtime.InteropServices;

namespace StorageType.Passthrough.IO {

    internal partial class PassthroughFile : PassthroughFileSystemBase {

        public static Result CreateFile(FileSystem FileSystem, string filePath, System.IO.FileAccess access, System.IO.FileShare share, System.IO.FileMode mode, System.IO.FileOptions options, System.IO.FileAttributes attributes, bool isdirectory, out IFSEntryPointer entry) {
            var result = ResultStatus.Success;

            if (isdirectory) {
                try {
                    switch (mode) {
                        case System.IO.FileMode.Open:
                            if (!FileSystem.Directory.Exists(filePath)) {
                                try {
                                    if (!FileSystem.File.GetAttributes(filePath).HasFlag(System.IO.FileAttributes.Directory)) {
                                        entry = null;
                                        return new Result(ResultStatus.NotADirectory);
                                    }
                                } catch (Exception) {
                                    entry = null;
                                    return new Result(ResultStatus.FileNotFound);
                                }
                                entry = null;
                                return new Result(ResultStatus.PathNotFound);
                            }
                            // you can't list the directory
                            break;

                        case System.IO.FileMode.CreateNew:
                            if (FileSystem.Directory.Exists(filePath)) {
                                entry = null;
                                return new Result(ResultStatus.FileExists);
                            }

                            try {
                                _ = FileSystem.File.GetAttributes(filePath).HasFlag(System.IO.FileAttributes.Directory);
                                entry = null;
                                return new Result(ResultStatus.AlreadyExists);
                            } catch (System.IO.IOException) {
                            }

                            _ = FileSystem.Directory.CreateDirectory(filePath);
                            break;
                    }
                } catch (UnauthorizedAccessException) {
                    entry = null;
                    return new Result(ResultStatus.AccessDenied);
                }
            } else {
                var pathExists = true;
                var pathIsDirectory = false;

                try {
                    pathExists = (FileSystem.Directory.Exists(filePath) || FileSystem.File.Exists(filePath));
                    pathIsDirectory = pathExists ? FileSystem.File.GetAttributes(filePath).HasFlag(System.IO.FileAttributes.Directory) : false;
                } catch (System.IO.IOException) {
                }

                switch (mode) {
                    case System.IO.FileMode.Open:

                        if (pathExists) {
                            // must set it to something if you return DokanError.Success
                            if (pathIsDirectory) {
                                entry = new PassthroughDirectory(FileSystem.DirectoryInfo.FromDirectoryName(filePath));
                            } else {
                                entry = new PassthroughFile(FileSystem.FileInfo.FromFileName(filePath));
                            }
                            return new Result(ResultStatus.Success);
                        } else {
                            entry = null;
                            return new Result(ResultStatus.FileNotFound);
                        }
                    case System.IO.FileMode.CreateNew:
                        if (pathExists) {
                            entry = null;
                            return new Result(ResultStatus.FileExists);
                        }
                        break;

                    case System.IO.FileMode.Truncate:
                        if (!pathExists) {
                            entry = null;
                            return new Result(ResultStatus.FileNotFound);
                        }
                        break;
                }

                try {
                    entry = new PassthroughFile(FileSystem.FileInfo.FromFileName(filePath));

                    if (pathExists && (mode == System.IO.FileMode.OpenOrCreate || mode == System.IO.FileMode.Create)) {
                        result = ResultStatus.AlreadyExists;
                    }

                    if (mode == System.IO.FileMode.CreateNew || mode == System.IO.FileMode.Create) { //Files are always created as Archive
                        attributes |= System.IO.FileAttributes.Archive;
                    }
                    FileSystem.File.SetAttributes(filePath, attributes);
                } catch (UnauthorizedAccessException) { // don't have access rights
                    entry = null;
                    return new Result(ResultStatus.AccessDenied);
                } catch (System.IO.DirectoryNotFoundException) {
                    entry = null;
                    return new Result(ResultStatus.PathNotFound);
                } catch (Exception ex) {
                    var hr = (uint)Marshal.GetHRForException(ex);
                    switch (hr) {
                        case 0x80070020: //Sharing violation
                            entry = null;
                            return new Result(ResultStatus.SharingViolation);

                        default:
                            throw;
                    }
                }
            }
            entry = null;
            return new Result(result);
        }

        internal static Result Move(FileSystem filesystem, string oldpath, string newpath, bool replace) {
            try {
                if (filesystem.File.Exists(newpath)) {
                    filesystem.File.Move(oldpath, newpath);
                    return new Result(ResultStatus.Success);
                } else if (replace) {
                    filesystem.File.Delete(newpath);
                    filesystem.File.Move(oldpath, newpath);
                    return new Result(ResultStatus.Success);
                }
            } catch (UnauthorizedAccessException) {
                return new Result(ResultStatus.AccessDenied);
            }
            return new Result(ResultStatus.FileExists);
        }
    }
}