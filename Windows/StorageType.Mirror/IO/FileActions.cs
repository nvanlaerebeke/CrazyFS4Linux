using StorageBackend.IO;
using System.IO;
using System.IO.Abstractions;

namespace StorageType.Passthrough.IO {

    internal class FileActions {
        public readonly IFileSystem FileSystem;

        public FileActions(IFileSystem pFileSystem) => FileSystem = pFileSystem;

        public FileActions() : this(new FileSystem()) {
        }

        public bool Exists(string pPath) => FileSystem.File.Exists(pPath);

        public void Move(string pOldPath, string pNewPath) => FileSystem.File.Move(pOldPath, pNewPath);

        /// <summary>
        /// ToDo: SetSecurity was removed for .net standard support
        /// </summary>
        internal PassthroughFile Create(string pFileName, uint pGrantedAccess, uint pFileAttributes, byte[] pSecurityDescriptor, out IEntry pEntry) {
            using (var s = FileSystem.File.Open(pFileName, FileMode.CreateNew, FileAccess.ReadWrite, FileShare.Read | FileShare.Write | FileShare.Delete)) { }
            var objFileDesc = new PassthroughFile(FileSystem.FileInfo.FromFileName(pFileName), FileSystem);
            //objFileDesc.SetSecurity((FileSystemRights)pGrantedAccess | FileSystemRights.WriteAttributes, pSecurityDescriptor);
            _ = objFileDesc.SetBasicInfo(pFileAttributes | (uint)FileAttributes.Archive, 0, 0, 0, out pEntry);
            return objFileDesc;
        }

        internal PassthroughFile Open(string pFileName, uint pGrantedAccess, out IEntry pEntry) {
            var objFileDesc = new PassthroughFile(FileSystem.FileInfo.FromFileName(pFileName), FileSystem);
            objFileDesc.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.Read | FileShare.Write | FileShare.Delete);
            //SetSecurity(pFileName, (FileSystemRights)pGrantedAccess);
            pEntry = objFileDesc.GetEntry();
            return objFileDesc;
        }
    }
}