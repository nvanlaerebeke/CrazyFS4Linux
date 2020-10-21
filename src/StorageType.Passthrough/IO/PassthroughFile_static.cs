using StorageBackend;
using StorageBackend.IO;
using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace StorageType.Passthrough.IO {

    internal partial class PassthroughFile : PassthroughFileSystemBase {

        public static Result CreateFile(string filePath, FileAccess access, FileShare share, FileMode mode, FileOptions options, FileAttributes attributes, bool isdirectory, out IFSEntryPointer entry) {
            var result = ResultStatus.Success;

            if (isdirectory) {
                try {
                    switch (mode) {
                        case FileMode.Open:
                            if (!Directory.Exists(filePath)) {
                                try {
                                    if (!File.GetAttributes(filePath).HasFlag(FileAttributes.Directory)) {
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

                            new DirectoryInfo(filePath).EnumerateFileSystemInfos().Any();
                            // you can't list the directory
                            break;

                        case FileMode.CreateNew:
                            if (Directory.Exists(filePath)) {
                                entry = null;
                                return new Result(ResultStatus.FileExists);
                            }

                            try {
                                _ = File.GetAttributes(filePath).HasFlag(FileAttributes.Directory);
                                entry = null;
                                return new Result(ResultStatus.AlreadyExists);
                            } catch (IOException) {
                            }

                            _ = Directory.CreateDirectory(filePath);
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
                    pathExists = (Directory.Exists(filePath) || File.Exists(filePath));
                    pathIsDirectory = pathExists ? File.GetAttributes(filePath).HasFlag(FileAttributes.Directory) : false;
                } catch (IOException) {
                }

                switch (mode) {
                    case FileMode.Open:

                        if (pathExists) {
                            // must set it to something if you return DokanError.Success
                            if (pathIsDirectory) {
                                entry = new PassthroughDirectory(new DirectoryInfo(filePath));
                            } else {
                                entry = new PassthroughFile(new FileInfo(filePath));
                            }
                            return new Result(ResultStatus.Success);
                        } else {
                            entry = null;
                            return new Result(ResultStatus.FileNotFound);
                        }
                    case FileMode.CreateNew:
                        if (pathExists) {
                            entry = null;
                            return new Result(ResultStatus.FileExists);
                        }
                        break;

                    case FileMode.Truncate:
                        if (!pathExists) {
                            entry = null;
                            return new Result(ResultStatus.FileNotFound);
                        }
                        break;
                }

                try {
                    entry = new PassthroughFile(new FileInfo(filePath));

                    if (pathExists && (mode == FileMode.OpenOrCreate || mode == FileMode.Create)) {
                        result = ResultStatus.AlreadyExists;
                    }

                    if (mode == FileMode.CreateNew || mode == FileMode.Create) //Files are always created as Archive
                        attributes |= FileAttributes.Archive;
                    File.SetAttributes(filePath, attributes);
                } catch (UnauthorizedAccessException) { // don't have access rights
                    entry = null;
                    return new Result(ResultStatus.AccessDenied);
                } catch (DirectoryNotFoundException) {
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

        internal static Result Move(string oldpath, string newpath, bool replace) {
            try {
                if (File.Exists(newpath)) {
                    File.Move(oldpath, newpath);
                    return new Result(ResultStatus.Success);
                } else if (replace) {
                    File.Delete(newpath);
                    File.Move(oldpath, newpath);
                    return new Result(ResultStatus.Success);
                }
            } catch (UnauthorizedAccessException) {
                return new Result(ResultStatus.AccessDenied);
            }
            return new Result(ResultStatus.FileExists);
        }
    }
}