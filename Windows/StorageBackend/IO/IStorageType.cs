using StorageBackend.IO;
using System.IO;

namespace StorageBackend {

    public interface IStorageType {

        void Setup(string pSource);

        Result Create(string pFileName, bool pIsFile, FileAccess pFileAccess, FileShare pShare, FileMode pMode, FileOptions pOptions, FileAttributes pAttributes, out IFSEntryPointer pNode);

        IFSEntryPointer GetFileInfo(string filename);

        Result Move(string oldpath, string newpath, bool replace);

        Result DeleteDirectory(IFSDirectory iFSDirectory);
    }
}