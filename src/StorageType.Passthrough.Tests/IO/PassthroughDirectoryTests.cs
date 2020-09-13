using Moq;
using NUnit.Framework;
using StorageBackend.IO;
using StorageType.Passthrough.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using System.Linq;

namespace StorageType.Passthrough.Tests.IO {

    [TextFixture]
    internal class PassthroughDirectoryTests {

        [Test]
        public void TestSetBasicInfo() {
            //Arrange
            var di = new Mock<IDirectoryInfo>();
            var ct = DateTime.UtcNow.Subtract(new TimeSpan(0, 0, 10));
            var lw = ct.AddSeconds(3);
            var la = ct.AddSeconds(5);

            _ = di.SetupGet(x => x.Attributes).Returns(System.IO.FileAttributes.Directory);
            _ = di.SetupGet(x => x.CreationTime).Returns(ct);
            _ = di.SetupGet(x => x.CreationTimeUtc).Returns(ct);
            _ = di.SetupGet(x => x.LastAccessTime).Returns(la);
            _ = di.SetupGet(x => x.LastAccessTimeUtc).Returns(la);
            _ = di.SetupGet(x => x.LastWriteTime).Returns(lw);
            _ = di.SetupGet(x => x.LastWriteTimeUtc).Returns(lw);

            //Act
            var d = new PassthroughDirectory(di.Object);
            _ = d.SetBasicInfo(0, (ulong)ct.ToFileTimeUtc(), (ulong)la.ToFileTimeUtc(), (ulong)lw.ToFileTimeUtc(), out var e);

            //Assert
            Assert.AreEqual(0, e.AllocationSize);
            Assert.AreEqual((uint)System.IO.FileAttributes.Directory, e.Attributes);
            Assert.AreEqual(lw.ToFileTimeUtc(), e.ChangeTime);
            Assert.AreEqual(ct.ToFileTimeUtc(), e.CreationTime);
            Assert.AreEqual(0, e.EaSize);
            Assert.AreEqual(0, e.FileSize);
            Assert.AreEqual(0, e.HardLinks);
            Assert.AreEqual(0, e.IndexNumber);
            Assert.AreEqual(la.ToFileTimeUtc(), e.LastAccessTime);
            Assert.AreEqual(lw.ToFileTimeUtc(), e.LastWriteTime);
            Assert.AreEqual(0, e.ReparseTag);
            Assert.IsFalse(e.IsFile());
        }

        //[Test]
        public void TestReadDirectory() {
            //Arrange
            var fs = new MockFileSystem();
            var dirs = new string[] {
                @"c:\My",
                @"c:\My\Dir1",
                @"c:\My\Dir2",
                @"c:\My\Dir3",
                @"c:\My\Dir4"
            }.ToList();
            dirs.ForEach(d => fs.Directory.CreateDirectory(d));

            var files = new string[] {
                @"c:\My\file1.txt",
                @"c:\My\file2.txt",
                @"c:\My\file3.txt",
                @"c:\My\file4.txt"
            }.ToList();
            files.ForEach(f => fs.File.Create(f));

            var di = fs.DirectoryInfo.FromDirectoryName(@"c:\My");
            var items = new Dictionary<string, IEntry>();

            //Act
            var d = new PassthroughDirectory(di);
            object c = 0;
            while (d.ReadDirectory(null, null, ref c, out var n, out var e)) {
                items.Add(n, e.Clone() as IEntry);
            }

            static bool comp(FileSystemInfoBase x, IEntry y) {
                if (!x.Attributes.Equals((FileAttributes)y.Attributes)) {
                    return false;
                }
                if (!x.CreationTimeUtc.ToFileTimeUtc().Equals((long)y.CreationTime)) {
                    return false;
                }
                if (!x.LastAccessTimeUtc.ToFileTimeUtc().Equals((long)y.LastAccessTime)) {
                    return false;
                }
                if (!x.LastWriteTimeUtc.ToFileTimeUtc().Equals((long)y.LastWriteTime)) {
                    return false;
                }

                return (
                    x.Attributes.Equals((FileAttributes)y.Attributes) &&
                    x.CreationTimeUtc.ToFileTimeUtc().Equals((long)y.CreationTime) &&
                    x.LastAccessTimeUtc.ToFileTimeUtc().Equals((long)y.LastAccessTime) &&
                    x.LastWriteTimeUtc.ToFileTimeUtc().Equals((long)y.LastWriteTime)
                );
            }

            //Assert
            Assert.AreEqual((4 + files.Count + 2), (int)c);
            foreach (var n in items) {
                var entry = n.Value;
                FileSystemInfoBase real;

                if (n.Equals(".")) {
                    real = fs.DirectoryInfo.FromDirectoryName(@"c:\My") as FileSystemInfoBase;
                } else if (n.Equals("..")) {
                    real = fs.DirectoryInfo.FromDirectoryName(@"c:\") as FileSystemInfoBase;
                } else {
                    real = fs.DirectoryInfo.FromDirectoryName(n.Key) as FileSystemInfoBase;
                }

                if (!comp(real, entry)) {
                    throw new Exception("Not the same");
                }
            }
        }
    }
}