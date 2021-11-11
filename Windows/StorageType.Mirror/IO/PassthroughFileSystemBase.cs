using StorageBackend.IO;
using System.Security.AccessControl;

namespace StorageType.Passthrough.IO {

    internal abstract class PassthroughFileSystemBase : IFSEntryPointer {

        public abstract int SetBasicInfo(uint FileAttributes, ulong CreationTime, ulong LastAccessTime, ulong LastWriteTime, out IEntry pEntry);

        public abstract byte[] GetSecurityDescriptor();

        public abstract int SetSecurityDescriptor(AccessControlSections Sections, byte[] SecurityDescriptor);

        public abstract IEntry GetEntry();
    }
}