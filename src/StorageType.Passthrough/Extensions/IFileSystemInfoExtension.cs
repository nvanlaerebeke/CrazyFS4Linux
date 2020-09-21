using StorageBackend.IO;
using System.IO;
using System.IO.Abstractions;

namespace StorageType.Passthrough.IO {

    internal static class IFileSystemInfoExtension {

        public static IFSEntryPointer GetIFSEntryPointer(
            this IFileSystemInfo pInfo,
            IFileSystem pFileSystem
        ) {
            if ((pInfo.Attributes & FileAttributes.Directory) == FileAttributes.Directory) {
                return new PassthroughDirectory(pFileSystem.DirectoryInfo.FromDirectoryName(pInfo.FullName));
            } else {
                return new PassthroughFile(pFileSystem.FileInfo.FromFileName(pInfo.FullName));
            }
        }
    }
}