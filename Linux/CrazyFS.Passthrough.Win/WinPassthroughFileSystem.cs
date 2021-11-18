using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using CrazyFS.FileSystem;
using DokanNet;
using Microsoft.Win32.SafeHandles;
using FileAccess = DokanNet.FileAccess;

namespace CrazyFS.Passthrough.Win
{
    public class WinPassthroughFileSystem
    {
        private readonly string path;

        private const FileAccess DataAccess = FileAccess.ReadData | FileAccess.WriteData | FileAccess.AppendData |
                                      FileAccess.Execute |
                                      FileAccess.GenericExecute | FileAccess.GenericWrite |
                                      FileAccess.GenericRead;

        private const FileAccess DataWriteAccess = FileAccess.WriteData | FileAccess.AppendData |
                                                   FileAccess.Delete |
                                                   FileAccess.GenericWrite;

        public WinPassthroughFileSystem(string from, string to)
        {
            if (!Directory.Exists(from)) throw new ArgumentException(nameof(from));
            this.path = from;
        }

        protected string GetPath(string fileName)
        {
            return path + fileName;
        }


        public Result CreateFile(string fileName, DokanNet.FileAccess access, FileShare share, FileMode mode, FileOptions options, FileAttributes attributes, IDokanFileInfo info)
        {
            var result = ResultStatus.Success;
            var filePath = GetPath(fileName);

            if (info.IsDirectory)
            {
                try
                {
                    switch (mode)
                    {
                        case FileMode.Open:
                            if (!Directory.Exists(filePath))
                            {
                                try
                                {
                                    if (!File.GetAttributes(filePath).HasFlag(FileAttributes.Directory))
                                        return new Result(ResultStatus.NotADirectory);
                                }
                                catch (Exception)
                                {
                                    return new Result(ResultStatus.FileNotFound);
                                }
                                return new Result(ResultStatus.PathNotFound);
                            }

                            _ = new DirectoryInfo(filePath).EnumerateFileSystemInfos().Any();
                            // you can't list the directory
                            break;

                        case FileMode.CreateNew:
                            if (Directory.Exists(filePath))
                                return new Result(ResultStatus.FileExists);

                            try
                            {
                                File.GetAttributes(filePath).HasFlag(FileAttributes.Directory);
                                return new Result(ResultStatus.AlreadyExists);
                            }
                            catch (IOException)
                            {
                            }

                            Directory.CreateDirectory(GetPath(fileName));
                            break;
                    }
                }
                catch (UnauthorizedAccessException)
                {
                    return new Result(ResultStatus.AccessDenied);
                }
            }
            else
            {
                var pathExists = true;
                var pathIsDirectory = false;

                var readWriteAttributes = (access & DataAccess) == 0;
                var readAccess = (access & DataWriteAccess) == 0;

                try
                {
                    pathExists = (Directory.Exists(filePath) || File.Exists(filePath));
                    pathIsDirectory = pathExists ? File.GetAttributes(filePath).HasFlag(FileAttributes.Directory) : false;
                }
                catch (IOException)
                {
                }

                switch (mode)
                {
                    case FileMode.Open:

                        if (pathExists)
                        {
                            // check if driver only wants to read attributes, security info, or open directory
                            if (readWriteAttributes || pathIsDirectory)
                            {
                                if (pathIsDirectory && (access & FileAccess.Delete) == FileAccess.Delete
                                    && (access & FileAccess.Synchronize) != FileAccess.Synchronize)
                                    //It is a DeleteFile request on a directory
                                    return new Result(ResultStatus.AccessDenied);

                                info.IsDirectory = pathIsDirectory;
                                info.Context = new object();
                                // must set it to something if you return DokanError.Success

                                return new Result(ResultStatus.Success);
                            }
                        }
                        else
                        {
                            return new Result(ResultStatus.FileNotFound);
                        }
                        break;

                    case FileMode.CreateNew:
                        if (pathExists)
                            return new Result(ResultStatus.FileExists);
                        break;

                    case FileMode.Truncate:
                        if (!pathExists)
                            return new Result(ResultStatus.FileNotFound);
                        break;
                }

                try
                {
                    info.Context = new FileStream(filePath, mode,
                        readAccess ? System.IO.FileAccess.Read : System.IO.FileAccess.ReadWrite, share, 4096, options);

                    if (pathExists && (mode == FileMode.OpenOrCreate || mode == FileMode.Create))
                        result = ResultStatus.AlreadyExists;

                    bool fileCreated = mode == FileMode.CreateNew || mode == FileMode.Create || (!pathExists && mode == FileMode.OpenOrCreate);
                    if (fileCreated)
                    {
                        FileAttributes new_attributes = attributes;
                        new_attributes |= FileAttributes.Archive; // Files are always created as Archive
                        // FILE_ATTRIBUTE_NORMAL is override if any other attribute is set.
                        new_attributes &= ~FileAttributes.Normal;
                        File.SetAttributes(filePath, new_attributes);
                    }
                }
                catch (UnauthorizedAccessException) // don't have access rights
                {
                    if (info.Context is FileStream fileStream)
                    {
                        // returning AccessDenied cleanup and close won't be called,
                        // so we have to take care of the stream now
                        fileStream.Dispose();
                        info.Context = null;
                    }
                    return new Result(ResultStatus.AccessDenied);
                }
                catch (DirectoryNotFoundException)
                {
                    return new Result(ResultStatus.PathNotFound);
                }
                catch (Exception ex)
                {
                    var hr = (uint)Marshal.GetHRForException(ex);
                    switch (hr)
                    {
                        case 0x80070020: //Sharing violation
                            return new Result(ResultStatus.SharingViolation);
                        default:
                            throw;
                    }
                }
            }
            return new Result(result);
        }

        public void Cleanup(string fileName, IDokanFileInfo info)
        {
            (info.Context as FileStream)?.Dispose();
            info.Context = null;

            if (info.DeleteOnClose)
            {
                if (info.IsDirectory)
                {
                    Directory.Delete(GetPath(fileName));
                }
                else
                {
                    File.Delete(GetPath(fileName));
                }
            }
        }

        public void CloseFile(string fileName, IDokanFileInfo info)
        {
            (info.Context as FileStream)?.Dispose();
            info.Context = null;
            // could recreate cleanup code here but this is not called sometimes
        }

        public Result ReadFile(string fileName, byte[] buffer, out int bytesRead, long offset, IDokanFileInfo info)
        {
            if (info.Context == null) // memory mapped read
            {
                using (var stream = new FileStream(GetPath(fileName), FileMode.Open, System.IO.FileAccess.Read))
                {
                    stream.Position = offset;
                    bytesRead = stream.Read(buffer, 0, buffer.Length);
                }
            }
            else // normal read
            {
                var stream = info.Context as FileStream;
                lock (stream) //Protect from overlapped read
                {
                    stream.Position = offset;
                    bytesRead = stream.Read(buffer, 0, buffer.Length);
                }
            }
            return new Result(ResultStatus.Success);
        }

        public Result WriteFile(string fileName, byte[] buffer, out int bytesWritten, long offset, IDokanFileInfo info)
        {
            var append = offset == -1;
            if (info.Context == null)
            {
                using (var stream = new FileStream(GetPath(fileName), append ? FileMode.Append : FileMode.Open, System.IO.FileAccess.Write))
                {
                    if (!append) // Offset of -1 is an APPEND: https://docs.microsoft.com/en-us/windows/win32/api/fileapi/nf-fileapi-writefile
                    {
                        stream.Position = offset;
                    }
                    stream.Write(buffer, 0, buffer.Length);
                    bytesWritten = buffer.Length;
                }
            }
            else
            {
                var stream = info.Context as FileStream;
                lock (stream) //Protect from overlapped write
                {
                    if (append)
                    {
                        if (stream.CanSeek)
                        {
                            stream.Seek(0, SeekOrigin.End);
                        }
                        else
                        {
                            bytesWritten = 0;
                            return new Result(ResultStatus.Error);
                        }
                    }
                    else
                    {
                        stream.Position = offset;
                    }
                    stream.Write(buffer, 0, buffer.Length);
                }
                bytesWritten = buffer.Length;
            }
            return new Result(ResultStatus.Success);
        }

        public Result FlushFileBuffers(string fileName, IDokanFileInfo info)
        {
            try
            {
                ((FileStream)(info.Context)).Flush();
                return new Result(ResultStatus.Success);
            }
            catch (IOException)
            {
                return new Result(ResultStatus.DiskFull);
            }
        }

        public Result GetFileInformation(string fileName, out FileInformation fileInfo, IDokanFileInfo info)
        {
            // may be called with info.Context == null, but usually it isn't
            var filePath = GetPath(fileName);
            FileSystemInfo finfo = new FileInfo(filePath);
            if (!finfo.Exists)
                finfo = new DirectoryInfo(filePath);

            fileInfo = new FileInformation
            {
                FileName = fileName,
                Attributes = finfo.Attributes,
                CreationTime = finfo.CreationTime,
                LastAccessTime = finfo.LastAccessTime,
                LastWriteTime = finfo.LastWriteTime,
                Length = (finfo as FileInfo)?.Length ?? 0,
            };
            return new Result(ResultStatus.Success);
        }

        public Result FindFiles(string fileName, out IList<FileInformation> files, IDokanFileInfo info)
        {
            // This function is not called because FindFilesWithPattern is implemented
            // Return DokanResult.NotImplemented in FindFilesWithPattern to make FindFiles called
            files = FindFilesHelper(fileName, "*");
            return new Result(ResultStatus.Success);
        }

        public Result SetFileAttributes(string fileName, FileAttributes attributes, IDokanFileInfo info)
        {
            try
            {
                // MS-FSCC 2.6 File Attributes : There is no file attribute with the value 0x00000000
                // because a value of 0x00000000 in the FileAttributes field means that the file attributes for this file MUST NOT be changed when setting basic information for the file
                if (attributes != 0)
                    File.SetAttributes(GetPath(fileName), attributes);
                return new Result(ResultStatus.Success);
            }
            catch (UnauthorizedAccessException)
            {
                return new Result(ResultStatus.AccessDenied);
            }
            catch (FileNotFoundException)
            {
                return new Result(ResultStatus.FileNotFound);
            }
            catch (DirectoryNotFoundException)
            {
                return new Result(ResultStatus.PathNotFound);
            }
        }

        public Result SetFileTime(string fileName, DateTime? creationTime, DateTime? lastAccessTime, DateTime? lastWriteTime, IDokanFileInfo info)
        {
            try
            {
                if (info.Context is FileStream stream)
                {
                    var ct = creationTime?.ToFileTime() ?? 0;
                    var lat = lastAccessTime?.ToFileTime() ?? 0;
                    var lwt = lastWriteTime?.ToFileTime() ?? 0;

                    if (NativeMethods.SetFileTime(stream.SafeFileHandle, ref ct, ref lat, ref lwt))
                        return new Result(ResultStatus.Success);

                    throw Marshal.GetExceptionForHR(Marshal.GetLastWin32Error());
                }

                var filePath = GetPath(fileName);

                if (creationTime.HasValue)
                    File.SetCreationTime(filePath, creationTime.Value);

                if (lastAccessTime.HasValue)
                    File.SetLastAccessTime(filePath, lastAccessTime.Value);

                if (lastWriteTime.HasValue)
                    File.SetLastWriteTime(filePath, lastWriteTime.Value);

                return new Result(ResultStatus.Success);
            }
            catch (UnauthorizedAccessException)
            {
                return new Result(ResultStatus.AccessDenied);
            }
            catch (FileNotFoundException)
            {
                return new Result(ResultStatus.FileNotFound);
            }
        }

        public Result DeleteFile(string fileName, IDokanFileInfo info)
        {
            var filePath = GetPath(fileName);

            if (Directory.Exists(filePath))
                return new Result(ResultStatus.AccessDenied);

            if (!File.Exists(filePath))
                return new Result(ResultStatus.FileNotFound);

            if (File.GetAttributes(filePath).HasFlag(FileAttributes.Directory))
                return new Result(ResultStatus.AccessDenied);

            return new Result(ResultStatus.Success);
            // we just check here if we could delete the file - the true deletion is in Cleanup
        }

        public Result DeleteDirectory(string fileName, IDokanFileInfo info)
        {
            return new Result(Directory.EnumerateFileSystemEntries(GetPath(fileName)).Any() ? ResultStatus.DirectoryNotEmpty : ResultStatus.Success); 
        }

        public Result MoveFile(string oldName, string newName, bool replace, IDokanFileInfo info)
        {
            var oldpath = GetPath(oldName);
            var newpath = GetPath(newName);

            (info.Context as FileStream)?.Dispose();
            info.Context = null;

            var exist = info.IsDirectory ? Directory.Exists(newpath) : File.Exists(newpath);

            try
            {

                if (!exist)
                {
                    info.Context = null;
                    if (info.IsDirectory)
                        Directory.Move(oldpath, newpath);
                    else
                        File.Move(oldpath, newpath);
                    
                    return new Result(ResultStatus.Success);
                }
                else if (replace)
                {
                    info.Context = null;

                    if (info.IsDirectory) //Cannot replace directory destination - See MOVEFILE_REPLACE_EXISTING
                        return new Result(ResultStatus.AccessDenied);


                    File.Delete(newpath);
                    File.Move(oldpath, newpath);
                    return new Result(ResultStatus.Success);
                }
            }
            catch (UnauthorizedAccessException)
            {
                return new Result(ResultStatus.AccessDenied);
            }
            return new Result(ResultStatus.FileExists);
        }

        public Result SetEndOfFile(string fileName, long length, IDokanFileInfo info)
        {
            try
            {
                ((FileStream)(info.Context)).SetLength(length);
                return new Result(ResultStatus.Success);

            }
            catch (IOException)
            {
                return new Result(ResultStatus.DiskFull);
            }
        }

        public Result SetAllocationSize(string fileName, long length, IDokanFileInfo info)
        {
            try
            {
                ((FileStream)(info.Context)).SetLength(length);
                return new Result(ResultStatus.Success);
            }
            catch (IOException)
            {
                return new Result(ResultStatus.DiskFull);
            }
        }

        public Result LockFile(string fileName, long offset, long length, IDokanFileInfo info)
        {
            try
            {
                ((FileStream)(info.Context)).Lock(offset, length);
                return new Result(ResultStatus.Success);
            }
            catch (IOException)
            {
                return new Result(ResultStatus.AccessDenied);
            }

        }

        public Result UnlockFile(string fileName, long offset, long length, IDokanFileInfo info)
        {
            try
            {
                ((FileStream)(info.Context)).Unlock(offset, length);
                return new Result(ResultStatus.Success);
            }
            catch (IOException)
            {
                return new Result(ResultStatus.AccessDenied);
            }
        }

        public Result GetDiskFreeSpace(out long freeBytesAvailable, out long totalNumberOfBytes, out long totalNumberOfFreeBytes, IDokanFileInfo info)
        {
            var dinfo = DriveInfo.GetDrives().Single(di => string.Equals(di.RootDirectory.Name, Path.GetPathRoot(path + "\\"), StringComparison.OrdinalIgnoreCase));

            freeBytesAvailable = dinfo.TotalFreeSpace;
            totalNumberOfBytes = dinfo.TotalSize;
            totalNumberOfFreeBytes = dinfo.AvailableFreeSpace;

            return new Result(ResultStatus.Success);
        }

        public Result GetVolumeInformation(out string volumeLabel, out FileSystemFeatures features, out string fileSystemName, out uint maximumComponentLength, IDokanFileInfo info)
        {
            volumeLabel = "DOKAN";
            fileSystemName = "NTFS";
            maximumComponentLength = 256;

            features = FileSystemFeatures.CasePreservedNames | FileSystemFeatures.CaseSensitiveSearch |
                       FileSystemFeatures.PersistentAcls | FileSystemFeatures.SupportsRemoteStorage |
                       FileSystemFeatures.UnicodeOnDisk;

            return new Result(ResultStatus.Success);
        }

        public Result GetFileSecurity(string fileName, out FileSystemSecurity security, AccessControlSections sections, IDokanFileInfo info)
        {
            try
            {
                security = new FileSecurity(GetPath(fileName), AccessControlSections.Owner | AccessControlSections.Group | AccessControlSections.Access);
                return new Result(ResultStatus.Success);
            }
            catch (UnauthorizedAccessException)
            {
                security = null;
                return new Result(ResultStatus.AccessDenied);
            }
        }

        public Result SetFileSecurity(string fileName, FileSystemSecurity security, AccessControlSections sections, IDokanFileInfo info)
        {
            return new Result(ResultStatus.NotImplemented);
        }

        public Result Mounted(IDokanFileInfo info)
        {
            return new Result(ResultStatus.Success);
        }

        public Result Unmounted(IDokanFileInfo info)
        {
            return new Result(ResultStatus.Success);
        }

        public Result FindStreams(string fileName, IntPtr enumContext, out string streamName, out long streamSize, IDokanFileInfo info)
        {
            streamName = string.Empty;
            streamSize = 0;
            return new Result(ResultStatus.NotImplemented);
        }

        public Result FindStreams(string fileName, out IList<FileInformation> streams, IDokanFileInfo info)
        {
            streams = new FileInformation[0];
            return new Result(ResultStatus.NotImplemented);
        }

        public IList<FileInformation> FindFilesHelper(string fileName, string searchPattern)
        {
            IList<FileInformation> files = new DirectoryInfo(GetPath(fileName))
                .EnumerateFileSystemInfos()
                .Where(finfo => DokanHelper.DokanIsNameInExpression(searchPattern, finfo.Name, true))
                .Select(finfo => new FileInformation
                {
                    Attributes = finfo.Attributes,
                    CreationTime = finfo.CreationTime,
                    LastAccessTime = finfo.LastAccessTime,
                    LastWriteTime = finfo.LastWriteTime,
                    Length = (finfo as FileInfo)?.Length ?? 0,
                    FileName = finfo.Name
                }).ToArray();

            return files;
        }

        public Result FindFilesWithPattern(string fileName, string searchPattern, out IList<FileInformation> files, IDokanFileInfo info)
        {
            files = FindFilesHelper(fileName, searchPattern);
            return new Result(ResultStatus.Success);
        }


        /**
         * Optional implementation for performance
         * 
         */
        /// <summary>
        /// Read from file using unmanaged buffers.
        /// </summary>
        public Result ReadFile(string fileName, IntPtr buffer, uint bufferLength, out int bytesRead, long offset, IDokanFileInfo info)
        {
            if (info.Context == null) // memory mapped read
            {
                using (var stream = new FileStream(GetPath(fileName), FileMode.Open, System.IO.FileAccess.Read))
                {
                    DoRead(stream, buffer, bufferLength, out bytesRead, offset);
                }
            }
            else // normal read
            {
                var stream = info.Context as FileStream;
                lock (stream) //Protect from overlapped read
                {
                    DoRead(stream, buffer, bufferLength, out bytesRead, offset);
                }
            }
            return new Result(ResultStatus.Success);

            void DoRead(FileStream stream, IntPtr innerBuffer, uint innerBufferLength, out int innerBytesRead, long innerOffset)
            {
                NativeMethods.SetFilePointer(stream.SafeFileHandle, innerOffset);
                NativeMethods.ReadFile(stream.SafeFileHandle, innerBuffer, innerBufferLength, out innerBytesRead);
            }
        }

        /// <summary>
        /// Write to file using unmanaged buffers.
        /// </summary>
        public Result WriteFile(string fileName, IntPtr buffer, uint bufferLength, out int bytesWritten, long offset, IDokanFileInfo info)
        {
            if (info.Context == null)
            {
                using (var stream = new FileStream(GetPath(fileName), FileMode.Open, System.IO.FileAccess.Write))
                {
                    DoWrite(stream, buffer, bufferLength, out bytesWritten, offset);
                }
            }
            else
            {
                var stream = info.Context as FileStream;
                lock (stream) //Protect from overlapped write
                {
                    DoWrite(stream, buffer, bufferLength, out bytesWritten, offset);
                }
            }

            return new Result(ResultStatus.Success);
            
            void DoWrite(FileStream stream, IntPtr innerBuffer, uint innerBufferLength, out int innerBytesWritten, long innerOffset)
            {
                NativeMethods.SetFilePointer(stream.SafeFileHandle, innerOffset);
                NativeMethods.WriteFile(stream.SafeFileHandle, innerBuffer, innerBufferLength, out innerBytesWritten);
            }
        }

        /// <summary>
        /// kernel32 file method wrappers.
        /// </summary>
        private class NativeMethods
        {
            /// <summary>
            /// Sets the date and time that the specified file or directory was created, last accessed, or last modified.
            /// </summary>
            /// <param name="hFile">A <see cref="SafeFileHandle"/> to the file or directory. 
            /// To get the handler, <see cref="System.IO.FileStream.SafeFileHandle"/> can be used.</param>
            /// <param name="lpCreationTime">A Windows File Time that contains the new creation date and time 
            /// for the file or directory. 
            /// If the application does not need to change this information, set this parameter to 0.</param>
            /// <param name="lpLastAccessTime">A Windows File Time that contains the new last access date and time 
            /// for the file or directory. The last access time includes the last time the file or directory 
            /// was written to, read from, or (in the case of executable files) run. 
            /// If the application does not need to change this information, set this parameter to 0.</param>
            /// <param name="lpLastWriteTime">A Windows File Time that contains the new last modified date and time 
            /// for the file or directory. If the application does not need to change this information, 
            /// set this parameter to 0.</param>
            /// <returns>If the function succeeds, the return value is <c>true</c>.</returns>
            /// \see <a href="https://msdn.microsoft.com/en-us/library/windows/desktop/ms724933">SetFileTime function (MSDN)</a>
            [DllImport("kernel32", SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool SetFileTime(SafeFileHandle hFile, ref long lpCreationTime, ref long lpLastAccessTime, ref long lpLastWriteTime);

            public static void SetFilePointer(SafeFileHandle fileHandle, long offset)
            {
                if (!SetFilePointerEx(fileHandle, offset, IntPtr.Zero, FILE_BEGIN))
                {
                    throw new Win32Exception();
                }
            }

            public static void ReadFile(SafeFileHandle fileHandle, IntPtr buffer, uint bytesToRead, out int bytesRead)
            {
                if (!ReadFile(fileHandle, buffer, bytesToRead, out bytesRead, IntPtr.Zero))
                {
                    throw new Win32Exception();
                }
            }

            public static void WriteFile(SafeFileHandle fileHandle, IntPtr buffer, uint bytesToWrite, out int bytesWritten)
            {
                if (!WriteFile(fileHandle, buffer, bytesToWrite, out bytesWritten, IntPtr.Zero))
                {
                    throw new Win32Exception();
                }
            }

            private const uint FILE_BEGIN = 0;

            [DllImport("kernel32.dll", SetLastError = true)]
            private static extern bool SetFilePointerEx(SafeFileHandle hFile, long liDistanceToMove, IntPtr lpNewFilePointer, uint dwMoveMethod);

            [DllImport("kernel32.dll", SetLastError = true)]
            private static extern bool ReadFile(SafeFileHandle hFile, IntPtr lpBuffer, uint nNumberOfBytesToRead, out int lpNumberOfBytesRead, IntPtr lpOverlapped);

            [DllImport("kernel32.dll", SetLastError = true)]
            private static extern bool WriteFile(SafeFileHandle hFile, IntPtr lpBuffer, uint nNumberOfBytesToWrite, out int lpNumberOfBytesWritten, IntPtr lpOverlapped);
        }
    }
}
