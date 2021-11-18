using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using CrazyFS.Passthrough.Win;
using DokanNet;
using Microsoft.Win32.SafeHandles;

namespace CrazyFS.Win
{
    /// <summary>
    /// Implementation of IDokanOperationsUnsafe to demonstrate usage.
    /// </summary>
    internal class CrazyFSFileSystem : IDokanOperationsUnsafe
    {

        private readonly WinPassthroughFileSystem FileSystem;
        /// <summary>
        /// Constructs a new unsafe mirror for the specified root path.
        /// </summary>
        /// <param name="path">Root path of mirror.</param>
        public CrazyFSFileSystem(string from, string to) {
            FileSystem = new WinPassthroughFileSystem(from, to);
        }

        public void Cleanup(string fileName, IDokanFileInfo info)
        {
            FileSystem.Cleanup(fileName, info);
        }

        public void CloseFile(string fileName, IDokanFileInfo info)
        {
            FileSystem.CloseFile(fileName, info);
        }

        public NtStatus CreateFile(string fileName, DokanNet.FileAccess access, FileShare share, FileMode mode, FileOptions options, FileAttributes attributes, IDokanFileInfo info)
        {
            return FileSystem.CreateFile(fileName, access, share, mode, options, attributes, info).ToNtStatus();
        }

        public NtStatus DeleteDirectory(string fileName, IDokanFileInfo info)
        {
            return FileSystem.DeleteDirectory(fileName, info).ToNtStatus();
        }

        public NtStatus DeleteFile(string fileName, IDokanFileInfo info)
        {
            return FileSystem.DeleteFile(fileName, info).ToNtStatus();
        }

        public NtStatus FindFiles(string fileName, out IList<FileInformation> files, IDokanFileInfo info)
        {
            return FileSystem.FindFiles(fileName, out files, info).ToNtStatus();
        }

        public NtStatus FindFilesWithPattern(string fileName, string searchPattern, out IList<FileInformation> files, IDokanFileInfo info)
        {
            return FileSystem.FindFilesWithPattern(fileName, searchPattern, out files, info).ToNtStatus(); 
        }

        public NtStatus FindStreams(string fileName, out IList<FileInformation> streams, IDokanFileInfo info)
        {
            return FileSystem.FindStreams(fileName, out streams, info).ToNtStatus();  
        }

        public NtStatus FlushFileBuffers(string fileName, IDokanFileInfo info)
        {
            return FileSystem.FlushFileBuffers(fileName, info).ToNtStatus();
        }

        public NtStatus GetDiskFreeSpace(out long freeBytesAvailable, out long totalNumberOfBytes, out long totalNumberOfFreeBytes, IDokanFileInfo info)
        {
            return FileSystem.GetDiskFreeSpace(out freeBytesAvailable, out totalNumberOfBytes, out totalNumberOfFreeBytes, info).ToNtStatus();
        }

        public NtStatus GetFileInformation(string fileName, out FileInformation fileInfo, IDokanFileInfo info)
        {
            return FileSystem.GetFileInformation(fileName, out fileInfo, info).ToNtStatus();
        }

        public NtStatus GetFileSecurity(string fileName, out FileSystemSecurity security, AccessControlSections sections, IDokanFileInfo info)
        {
            return FileSystem.GetFileSecurity(fileName, out security, sections, info).ToNtStatus();
        }

        public NtStatus GetVolumeInformation(out string volumeLabel, out FileSystemFeatures features, out string fileSystemName, out uint maximumComponentLength, IDokanFileInfo info)
        {
            return FileSystem.GetVolumeInformation(out volumeLabel, out features, out fileSystemName, out maximumComponentLength, info).ToNtStatus();
        }

        public NtStatus LockFile(string fileName, long offset, long length, IDokanFileInfo info)
        {
            return FileSystem.LockFile(fileName, offset, length, info).ToNtStatus();
        }

        public NtStatus Mounted(IDokanFileInfo info)
        {
            return FileSystem.Mounted(info).ToNtStatus();
        }

        public NtStatus MoveFile(string oldName, string newName, bool replace, IDokanFileInfo info)
        {
            return FileSystem.MoveFile(oldName, newName, replace, info).ToNtStatus();
        }

        public NtStatus ReadFile(string fileName, IntPtr buffer, uint bufferLength, out int bytesRead, long offset, IDokanFileInfo info)
        {
            return FileSystem.ReadFile(fileName, buffer, bufferLength, out bytesRead, offset, info).ToNtStatus();
        }

        public NtStatus ReadFile(string fileName, byte[] buffer, out int bytesRead, long offset, IDokanFileInfo info)
        {
            return FileSystem.ReadFile(fileName, buffer, out bytesRead, offset, info).ToNtStatus();
        }

        public NtStatus SetAllocationSize(string fileName, long length, IDokanFileInfo info)
        {
            return FileSystem.SetAllocationSize(fileName, length, info).ToNtStatus();
        }

        public NtStatus SetEndOfFile(string fileName, long length, IDokanFileInfo info)
        {
            return FileSystem.SetEndOfFile(fileName, length, info).ToNtStatus();
        }

        public NtStatus SetFileAttributes(string fileName, FileAttributes attributes, IDokanFileInfo info)
        {
            return FileSystem.SetFileAttributes(fileName, attributes, info).ToNtStatus();
        }

        public NtStatus SetFileSecurity(string fileName, FileSystemSecurity security, AccessControlSections sections, IDokanFileInfo info)
        {
            return FileSystem.SetFileSecurity(fileName, security, sections, info).ToNtStatus();
        }

        public NtStatus SetFileTime(string fileName, DateTime? creationTime, DateTime? lastAccessTime, DateTime? lastWriteTime, IDokanFileInfo info)
        {
            return FileSystem.SetFileTime(fileName, creationTime, lastAccessTime, lastWriteTime, info).ToNtStatus();
        }

        public NtStatus UnlockFile(string fileName, long offset, long length, IDokanFileInfo info)
        {
            return FileSystem.UnlockFile(fileName, offset, length, info).ToNtStatus();
        }

        public NtStatus Unmounted(IDokanFileInfo info)
        {
            return FileSystem.Unmounted(info).ToNtStatus();
        }

        public NtStatus WriteFile(string fileName, IntPtr buffer, uint bufferLength, out int bytesWritten, long offset, IDokanFileInfo info)
        {
            return FileSystem.WriteFile(fileName, buffer, bufferLength, out bytesWritten, offset, info).ToNtStatus();
        }

        public NtStatus WriteFile(string fileName, byte[] buffer, out int bytesWritten, long offset, IDokanFileInfo info)
        {
            return FileSystem.WriteFile(fileName, buffer, out bytesWritten, offset, info).ToNtStatus();
        }
    }
}
