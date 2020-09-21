using StorageBackend;
using StorageBackend.IO;
using System.IO.Abstractions;
using System.Security.AccessControl;

namespace StorageType.Passthrough.IO {

    internal class DirectoryActions {
        private readonly IFileSystem FileSystem;

        public DirectoryActions(IFileSystem pFileSystem) => FileSystem = pFileSystem;

        public DirectoryActions() : this(new FileSystem()) {
        }

        public void Move(string pOldPath, string pNewPath) {
            FileSystem.Directory.Move(pOldPath, pNewPath);
        }

        public Directory Open(string pPath, out IEntry pEntry) {
            if (!FileSystem.Directory.Exists(pPath)) {
                throw new System.IO.DirectoryNotFoundException($"{pPath} not found");
            }
            var d = new Directory(FileSystem.DirectoryInfo.FromDirectoryName(pPath));
            pEntry = d.GetEntry();
            return d;
        }

        public bool Exists(string pPath) => FileSystem.Directory.Exists(pPath);

        internal Directory Create(string pFileName, uint pFileAttributes, byte[] pSecurityDescriptor, out IEntry pEntry) {
            if (FileSystem.Directory.Exists(pFileName)) {
                throw new NTException(FileSystemStatus.STATUS_OBJECT_NAME_COLLISION);
            }
            DirectorySecurity Security = null;
            if (pSecurityDescriptor != null) {
                Security = new DirectorySecurity();
                Security.SetSecurityDescriptorBinaryForm(pSecurityDescriptor);
            }
            var objFileDesc = new Directory(FileSystem.Directory.CreateDirectory(pFileName));
            _ = objFileDesc.SetBasicInfo(pFileAttributes, 0, 0, 0, out pEntry);
            FileSystem.DirectoryInfo.FromDirectoryName(pFileName).SetAccessControl(Security);
            return objFileDesc;
        }
    }
}