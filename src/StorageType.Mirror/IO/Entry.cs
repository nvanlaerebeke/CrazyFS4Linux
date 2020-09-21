using StorageBackend.IO;
using System.IO.Abstractions;

namespace StorageType.Passthrough.IO {

    internal class Entry : IEntry {
        private readonly bool _IsFile = false;

        private Entry(IEntry pEntry) {
            FileSize = pEntry.FileSize;
            AllocationSize = pEntry.AllocationSize;

            ReparseTag = pEntry.ReparseTag;
            IndexNumber = pEntry.IndexNumber;
            HardLinks = pEntry.HardLinks;
            EaSize = pEntry.EaSize;

            Attributes = pEntry.Attributes;
            CreationTime = pEntry.CreationTime;
            LastAccessTime = pEntry.LastAccessTime;
            LastWriteTime = pEntry.LastWriteTime;
            ChangeTime = pEntry.ChangeTime;

            _IsFile = pEntry.IsFile();
        }

        internal Entry(IFileSystemInfo pFileSystemInfo) {
            FileSize = 0;
            AllocationSize = 0;

            ReparseTag = 0;
            IndexNumber = 0;
            HardLinks = 0;
            EaSize = 0;

            Attributes = (uint)pFileSystemInfo.Attributes;
            CreationTime = (ulong)pFileSystemInfo.CreationTimeUtc.ToFileTimeUtc();
            LastAccessTime = (ulong)pFileSystemInfo.LastAccessTimeUtc.ToFileTimeUtc();
            LastWriteTime = (ulong)pFileSystemInfo.LastWriteTimeUtc.ToFileTimeUtc();
            ChangeTime = LastWriteTime;

            if ((pFileSystemInfo.Attributes & System.IO.FileAttributes.Directory) != System.IO.FileAttributes.Directory) {
                _IsFile = true;
                FileSize = (ulong)(pFileSystemInfo as FileInfoBase).Length;
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

        public object Clone() => new Entry(this);
    }
}