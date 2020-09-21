﻿using StorageBackend.IO;
using System;
using System.IO;
using System.IO.Abstractions;

namespace StorageType.Passthrough.IO {

    internal class PassthroughEntry : IEntry {
        private readonly bool _IsFile = false;

        private PassthroughEntry(IEntry pEntry) {
            FullName = pEntry.FullName;
            Name = pEntry.Name;
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

        internal PassthroughEntry(IFileSystemInfo pFileSystemInfo) {
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

            if ((pFileSystemInfo.Attributes & System.IO.FileAttributes.Directory) != System.IO.FileAttributes.Directory) {
                _IsFile = true;
                FileSize = (pFileSystemInfo as FileInfoBase).Length;
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

        public object Clone() => new PassthroughEntry(this);
    }
}