using System.Collections.Generic;
using System.Security.AccessControl;

namespace StorageBackend.IO {

    public interface IFSDirectory : IFSEntryPointer {

        Result SetAccessControl(DirectorySecurity security);

        bool HasContent();

        List<IFSEntryPointer> GetContent(string searchPattern);
    }
}