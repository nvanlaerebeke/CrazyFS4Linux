using StorageBackend;
using StorageBackend.IO;
using System;
using System.IO.Abstractions;
using System.Linq;
using System.Security.AccessControl;

namespace StorageType.Passthrough.IO {

    internal class PassthroughDirectory : PassthroughFileSystemBase {
        private readonly IDirectoryInfo DirectoryInfo;
        private readonly IFileSystem FileSystem;
        private IFSEntryPointer[] FileSystemInfos;

        public PassthroughDirectory(IDirectoryInfo pDirectoryInfo) : this(pDirectoryInfo, new FileSystem()) {
        }

        public PassthroughDirectory(IDirectoryInfo pDirectoryInfo, IFileSystem pFileSystem) {
            FileSystem = pFileSystem;
            DirectoryInfo = pDirectoryInfo;
        }

        public override Result SetBasicInfo(System.IO.FileAttributes Attributes, DateTime CreationTime, DateTime LastAccessTime, DateTime LastWriteTime) {
            if ((Attributes & System.IO.FileAttributes.Directory) == 0) {
                Attributes |= System.IO.FileAttributes.Directory;
            }
            DirectoryInfo.Attributes = Attributes;
            if (CreationTime != default) {
                DirectoryInfo.CreationTimeUtc = CreationTime;
            }
            if (LastAccessTime != default) {
                DirectoryInfo.LastAccessTimeUtc = LastAccessTime;
            }
            if (LastWriteTime != default) {
                DirectoryInfo.LastWriteTimeUtc = LastWriteTime;
            }
            return new Result(ResultStatus.Success);
        }

        public override byte[] GetSecurityDescriptor() => DirectoryInfo.GetAccessControl().GetSecurityDescriptorBinaryForm();

        public override Result SetSecurityDescriptor(AccessControlSections Sections, byte[] SecurityDescriptor) {
            DirectoryInfo.SetAccessControl(new DirectorySecurity(DirectoryInfo.FullName, Sections));
            return new Result(ResultStatus.Success);
        }

        public IFSEntryPointer[] ReadDirectory(string pPattern, bool pCaseSensitive, string pMarker) {
            if (FileSystemInfos == null) {
                FileSystemInfos = DirectoryInfo
                    .EnumerateFileSystemInfos()
                    .Where(finfo => NameMatcher.Match(pPattern, finfo.Name, pCaseSensitive))
                    .Select(finfo => finfo.GetIFSEntryPointer(FileSystem))
                    .ToArray();
            }
            return FileSystemInfos;
        }

        public override IEntry GetEntry() => new PassthroughEntry(DirectoryInfo);

        public override FileSystemSecurity GetSecurity() => FileSystem.Directory.GetAccessControl(DirectoryInfo.FullName);

        public override Result SetSecurity(FileSystemSecurity pFileSystemSecurity) {
            FileSystem.Directory.SetAccessControl(DirectoryInfo.FullName, (DirectorySecurity)pFileSystemSecurity);
            return new Result(ResultStatus.Success);
        }
    }
}