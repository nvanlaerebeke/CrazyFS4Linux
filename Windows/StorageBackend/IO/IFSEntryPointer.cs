using System.Security.AccessControl;

namespace StorageBackend.IO {

    public interface IFSEntryPointer : IEntry {

        Result GetAccessControl(out FileSystemSecurity security);

        void Cleanup(bool deleteOnCleanup);

        void Close();
    }
}