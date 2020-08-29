using System.IO;

namespace StorageBackend {
    public class CrazyFSFileInfo : ICrazyFSFileInfo {

        public CrazyFSFileInfo(FileInfo pFileInfo) {
            FileAttributes = (uint)pFileInfo.Attributes;
            ReparseTag = 0;
            FileSize = pFileInfo is FileInfo ? (ulong)pFileInfo.Length : 0;
            AllocationSize = (ulong)(pFileInfo.Length + 4096 - 1) / 4096 * 4096;
            CreationTime = (ulong)pFileInfo.CreationTimeUtc.ToFileTimeUtc();
            LastAccessTime = (ulong)pFileInfo.LastAccessTimeUtc.ToFileTimeUtc();
            LastWriteTime = (ulong)pFileInfo.LastWriteTimeUtc.ToFileTimeUtc();
            ChangeTime = (ulong)pFileInfo.LastWriteTime.ToFileTimeUtc();
            IndexNumber = 0;
            HardLinks = 0;
        }

        public CrazyFSFileInfo(FileSystemInfo pFileSystemInfo) {
            // get the file attributes for file or directory
            var attr = pFileSystemInfo.Attributes;
            //detect whether its a directory or file
            if ((attr & System.IO.FileAttributes.Directory) == System.IO.FileAttributes.Directory) {
                FileSize = 0;
                AllocationSize = 0;
            } else {
                FileSize = (ulong)((System.IO.FileInfo)pFileSystemInfo).Length;
                AllocationSize = (FileSize + 4096 - 1) / 4096 * 4096;
            }
            ReparseTag = 0;
            IndexNumber = 0;
            HardLinks = 0;

            FileAttributes = (uint)attr;
            CreationTime = (ulong)pFileSystemInfo.CreationTimeUtc.ToFileTimeUtc();
            LastAccessTime = (ulong)pFileSystemInfo.LastAccessTimeUtc.ToFileTimeUtc();
            LastWriteTime = (ulong)pFileSystemInfo.LastWriteTimeUtc.ToFileTimeUtc();
            ChangeTime = (ulong)pFileSystemInfo.LastAccessTime.ToFileTimeUtc();
        }

        public uint FileAttributes { get; private set; }
        public uint ReparseTag { get; private set; }
        public ulong AllocationSize { get; private set; }
        public ulong FileSize { get; private set; }
        public ulong CreationTime { get; private set; }
        public ulong LastAccessTime { get; private set; }
        public ulong LastWriteTime { get; private set; }
        public ulong ChangeTime { get; private set; }
        public ulong IndexNumber { get; private set; }
        public uint HardLinks { get; private set; }
        public uint EaSize { get; private set; }
    }
}
