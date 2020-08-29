using System;
using System.IO;
using System.Security.AccessControl;

namespace StorageBackend {
    public interface IFileDescriptor {
        bool GetCrazyFSFileInfo(out ICrazyFSFileInfo pFileInfo);
        uint GetFileAttributes();
        byte[] GetSecurityDescriptor();
        void SetBasicInfo(uint FileAttributes, ulong CreationTime, ulong LastAccessTime, ulong LastWriteTime);
        void SetDisposition(bool Safe);
        void SetFileAttributes(uint FileAttributes);
        void SetSecurityDescriptor(AccessControlSections Sections, byte[] SecurityDescriptor);
        void Cleanup(uint pFlags, uint pCleanupDelete);
        void Close();
        void Flush();
        bool ReadDirectory(string pPattern, string pMarker, ref object pContext, out string pFileName, out ICrazyFSFileInfo pFileInfo);
        //int Read(IntPtr pBuffer, ulong pOffset, uint pLength, out uint pBytesTransferred);
        int Write(object pFileNode, IntPtr pBuffer, ulong pOffset, uint pLength, bool pWriteToEndOfFile, bool pConstrainedIo, out uint pBytesTransferred, out ICrazyFSFileInfo pFileInfo);
        int SetFileSize(ulong pNewSize, bool pSetAllocationSize, out ICrazyFSFileInfo pFileInfo);
        int OverWrite(uint pFileAttributes, bool pReplaceFileAttributes, out ICrazyFSFileInfo pFileInfo);

        FileStream GetStream();
    }
}