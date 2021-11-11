using NUnit.Framework;
using StorageType.Passthrough.IO;
using System.IO.Abstractions.TestingHelpers;
using System.Security.AccessControl;

namespace StorageType.Passthrough.Tests.IO {

    [TextFixture]
    internal class DirectoryActionsTests {

        [Test]
        public void TestMove() {
            //Arrange
            var fs = new MockFileSystem();
            fs.Directory.CreateDirectory(@"c:\my\dir");

            //Act
            new DirectoryActions(fs).Move(@"c:\my\dir", @"c:\my\dir2");

            //Assert
            Assert.IsTrue(fs.Directory.Exists(@"c:\my\dir2"));
            Assert.IsFalse(fs.Directory.Exists(@"c:\my\dir"));
        }

        [Test]
        public void TestOpen() {
            //Arrange
            var fs = new MockFileSystem();
            var dir = @"c:\my\dir";

            _ = fs.Directory.CreateDirectory(dir);

            //Act
            _ = new DirectoryActions(fs).Open(dir, out var e);

            //Assert
            Assert.IsTrue(fs.Directory.Exists(dir));
            Assert.AreEqual(0, e.AllocationSize);
            Assert.AreEqual((uint)fs.DirectoryInfo.FromDirectoryName(dir).Attributes, e.Attributes);
            Assert.AreEqual(fs.DirectoryInfo.FromDirectoryName(dir).LastWriteTimeUtc.ToFileTimeUtc(), e.ChangeTime);
            Assert.AreEqual(fs.DirectoryInfo.FromDirectoryName(dir).CreationTimeUtc.ToFileTimeUtc(), e.CreationTime);
            Assert.AreEqual(0, e.EaSize);
            Assert.AreEqual(0, e.FileSize);
            Assert.AreEqual(0, e.HardLinks);
            Assert.AreEqual(0, e.IndexNumber);
            Assert.AreEqual(fs.DirectoryInfo.FromDirectoryName(dir).LastAccessTimeUtc.ToFileTimeUtc(), e.LastAccessTime);
            Assert.AreEqual(fs.DirectoryInfo.FromDirectoryName(dir).LastWriteTimeUtc.ToFileTimeUtc(), e.LastWriteTime);
            Assert.AreEqual(0, e.ReparseTag);
            Assert.IsFalse(e.IsFile());
        }

        [Test]
        public void TestExists() {
            //Arrange
            var fs = new MockFileSystem();
            var dir1 = @"c:\my\dir";
            var dir2 = @"C:\my\dir2";
            _ = fs.Directory.CreateDirectory(dir1);

            //Act
            var dir1_exists = new DirectoryActions(fs).Exists(dir1);
            var dir2_exists = new DirectoryActions(fs).Exists(dir2);

            //Assert
            Assert.IsTrue(dir1_exists);
            Assert.IsFalse(dir2_exists);
        }

        [Test]
        public void TestCreate() {
            //Arrange
            var fs = new MockFileSystem();
            var dir = @"c:\my\dir";

            //Act
            var s = new DirectorySecurity();
            var d = s.GetSecurityDescriptorBinaryForm();
            _ = new DirectoryActions(fs).Create(dir, 0, d, out var e);

            //Assert
            Assert.IsTrue(fs.Directory.Exists(@"c:\my\dir"));
            Assert.IsTrue(fs.Directory.Exists(@"c:\my\dir"));
            Assert.AreEqual(0, e.AllocationSize);
            Assert.AreEqual((uint)fs.DirectoryInfo.FromDirectoryName(dir).Attributes, e.Attributes);
            Assert.AreEqual(fs.DirectoryInfo.FromDirectoryName(dir).LastWriteTimeUtc.ToFileTimeUtc(), e.ChangeTime);
            Assert.AreEqual(fs.DirectoryInfo.FromDirectoryName(dir).CreationTimeUtc.ToFileTimeUtc(), e.CreationTime);
            Assert.AreEqual(0, e.EaSize);
            Assert.AreEqual(0, e.FileSize);
            Assert.AreEqual(0, e.HardLinks);
            Assert.AreEqual(0, e.IndexNumber);
            Assert.AreEqual(fs.DirectoryInfo.FromDirectoryName(dir).LastAccessTimeUtc.ToFileTimeUtc(), e.LastAccessTime);
            Assert.AreEqual(fs.DirectoryInfo.FromDirectoryName(dir).LastWriteTimeUtc.ToFileTimeUtc(), e.LastWriteTime);
            Assert.AreEqual(0, e.ReparseTag);
            Assert.IsFalse(e.IsFile());
        }
    }
}