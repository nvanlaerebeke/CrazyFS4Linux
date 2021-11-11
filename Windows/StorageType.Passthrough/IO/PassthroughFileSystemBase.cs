using StorageBackend;
using StorageBackend.IO;
using System;
using System.IO;
using System.IO.Abstractions;
using System.Security.AccessControl;

namespace StorageType.Passthrough.IO {

    internal abstract class PassthroughFileSystemBase : IFSEntryPointer {
        private readonly bool _IsFile = false;
        protected FileSystem FileSystem;

        protected PassthroughFileSystemBase() {
            FileSystem = new FileSystem();
        }

        protected PassthroughFileSystemBase(IFileSystemInfo pFileSystemInfo) : this() {
            FullName = pFileSystemInfo.FullName;
            Name = pFileSystemInfo.Name;
            FileSize = 0;
            AllocationSize = 0;

            ReparseTag = 0;
            IndexNumber = 0;
            HardLinks = 0;
            EaSize = 0;

            Attributes = pFileSystemInfo.Attributes;
            CreationTime = pFileSystemInfo.CreationTimeUtc;
            LastAccessTime = pFileSystemInfo.LastAccessTimeUtc;
            LastWriteTime = pFileSystemInfo.LastWriteTimeUtc;
            ChangeTime = LastWriteTime;

            if ((Attributes & FileAttributes.Directory) == 0) {
                _IsFile = true;
                FileSize = (pFileSystemInfo as IFileInfo).Length;
                AllocationSize = (FileSize + 4096 - 1) / 4096 * 4096;
            }
        }

        public string FullName { get; protected set; }
        public string Name { get; protected set; }
        public FileAttributes Attributes { get; protected set; }
        public uint ReparseTag { get; protected set; }
        public long AllocationSize { get; protected set; }
        public long FileSize { get; protected set; }
        public DateTime CreationTime { get; protected set; }
        public DateTime LastAccessTime { get; protected set; }
        public DateTime LastWriteTime { get; protected set; }
        public DateTime ChangeTime { get; protected set; }
        public ulong IndexNumber { get; protected set; }
        public uint HardLinks { get; protected set; }
        public uint EaSize { get; protected set; }

        public bool IsFile() => _IsFile;

        public abstract object Clone();

        public abstract void Cleanup(bool deleteOnClose);

        public abstract void Close();

        public abstract Result GetAccessControl(out FileSystemSecurity security);
    }
}