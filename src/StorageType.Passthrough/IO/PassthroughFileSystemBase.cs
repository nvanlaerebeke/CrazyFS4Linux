using StorageBackend;
using StorageBackend.IO;
using System;
using System.Security.AccessControl;

namespace StorageType.Passthrough.IO {

    internal abstract class PassthroughFileSystemBase : IFSEntryPointer {

        public abstract Result SetBasicInfo(System.IO.FileAttributes FileAttributes, DateTime CreationTime, DateTime LastAccessTime, DateTime LastWriteTime);

        public abstract byte[] GetSecurityDescriptor();

        public abstract Result SetSecurityDescriptor(AccessControlSections Sections, byte[] SecurityDescriptor);

        public abstract IEntry GetEntry();

        public abstract FileSystemSecurity GetSecurity();

        public abstract Result SetSecurity(FileSystemSecurity pFileSystemSecurity);
    }
}