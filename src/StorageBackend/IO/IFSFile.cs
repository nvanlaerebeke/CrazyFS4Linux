using System;
using System.IO;
using System.Security.AccessControl;

namespace StorageBackend.IO {

    public interface IFSFile : IFSEntryPointer {

        Result SetAccessControl(FileSecurity security);

        Result Read(byte[] buffer, out int bytesRead, long offset);

        Result Write(byte[] buffer, out int bytesWritten, long offset);

        Result SetAttributes(FileAttributes attributes);

        void Flush();

        void SetCreationTime(DateTime value);

        void SetLastAccessTime(DateTime value);

        void SetLastWriteTime(DateTime value);

        void SetLength(long length);

        void UnLock(long offset, long length);

        void Lock(long offset, long length);
    }
}