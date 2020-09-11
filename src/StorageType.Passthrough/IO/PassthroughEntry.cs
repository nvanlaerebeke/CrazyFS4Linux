using StorageBackend.IO;
using System.IO;

namespace StorageType.Passthrough.IO {

    internal class PassthroughEntry : IEntry {
        private readonly bool _IsFile = false;

        internal PassthroughEntry(FileSystemInfo pFileSystemInfo) {
            FileSize = 0;
            AllocationSize = 0;

            ReparseTag = 0;
            IndexNumber = 0;
            HardLinks = 0;

            Attributes = (uint)pFileSystemInfo.Attributes;
            CreationTime = (ulong)pFileSystemInfo.CreationTimeUtc.ToFileTimeUtc();
            LastAccessTime = (ulong)pFileSystemInfo.LastAccessTimeUtc.ToFileTimeUtc();
            LastWriteTime = (ulong)pFileSystemInfo.LastWriteTimeUtc.ToFileTimeUtc();
            ChangeTime = (ulong)pFileSystemInfo.LastAccessTime.ToFileTimeUtc();

            if ((pFileSystemInfo.Attributes & FileAttributes.Directory) != FileAttributes.Directory) {
                _IsFile = true;
                FileSize = (ulong)(pFileSystemInfo as FileInfo).Length;
                AllocationSize = (FileSize + 4096 - 1) / 4096 * 4096;
            }
        }

        public uint Attributes { get; protected set; }
        public uint ReparseTag { get; protected set; }
        public ulong AllocationSize { get; protected set; }
        public ulong FileSize { get; protected set; }
        public ulong CreationTime { get; protected set; }
        public ulong LastAccessTime { get; protected set; }
        public ulong LastWriteTime { get; protected set; }
        public ulong ChangeTime { get; protected set; }
        public ulong IndexNumber { get; protected set; }
        public uint HardLinks { get; protected set; }
        public uint EaSize { get; protected set; }

        public bool IsFile() => _IsFile;
    }
}