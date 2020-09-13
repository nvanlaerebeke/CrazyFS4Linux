using NUnit.Framework;
using StorageType.Passthrough.IO;
using System.IO;
using System.IO.Abstractions.TestingHelpers;
using System.Security.AccessControl;

namespace StorageType.Passthrough.Tests.IO {

    [TestFixture]
    internal class FileActionTests {

        [Test]
        public void TestExists() {
            //Arrange
            var fs = new MockFileSystem();
            var file1 = @"c:\my\file1.txt";
            var file2 = @"c:\my\other\folder\file2.txt";
            fs.Directory.CreateDirectory(@"c:\my");
            fs.File.Create(file1);

            //Act
            var a = new FileActions(fs);
            var r1 = a.Exists(file1);
            var r2 = a.Exists(file2);

            //Assert
            Assert.IsTrue(r1);
            Assert.IsFalse(r2);
        }

        [Test]
        public void TestCreate() {
            //Arrange
            var fs = new MockFileSystem();
            var file = @"C:\my\file.txt";
            fs.Directory.CreateDirectory(@"C:\my");

            //Act
            var s = new FileSecurity();
            var fsecurity = s.GetSecurityDescriptorBinaryForm();
            _ = new FileActions(fs).Create(file, 1, (uint)FileAttributes.Normal, fsecurity, out var e);

            //Assert
            Assert.IsTrue(fs.File.Exists(file));
            Assert.AreEqual(0, e.AllocationSize);
            Assert.AreEqual((uint)fs.DirectoryInfo.FromDirectoryName(file).Attributes, e.Attributes);
            Assert.AreEqual(fs.DirectoryInfo.FromDirectoryName(file).LastWriteTimeUtc.ToFileTimeUtc(), e.ChangeTime);
            Assert.AreEqual(fs.DirectoryInfo.FromDirectoryName(file).CreationTimeUtc.ToFileTimeUtc(), e.CreationTime);
            Assert.AreEqual(0, e.EaSize);
            Assert.AreEqual(0, e.FileSize);
            Assert.AreEqual(0, e.HardLinks);
            Assert.AreEqual(0, e.IndexNumber);
            Assert.AreEqual(fs.DirectoryInfo.FromDirectoryName(file).LastAccessTimeUtc.ToFileTimeUtc(), e.LastAccessTime);
            Assert.AreEqual(fs.DirectoryInfo.FromDirectoryName(file).LastWriteTimeUtc.ToFileTimeUtc(), e.LastWriteTime);
            Assert.AreEqual(0, e.ReparseTag);
            Assert.IsTrue(e.IsFile());
        }

        [Test]
        public void TestOpen() {
            //Arrange
            var fs = new MockFileSystem();
            const string file = @"C:\my\file.txt";
            _ = fs.Directory.CreateDirectory(@"C:\my");
            _ = fs.File.Create(file);

            //Act
            var f = new FileActions(fs).Open(file, 1, out var e);

            //Assert
            Assert.IsTrue(fs.File.Exists(file));
            Assert.AreEqual(0, e.AllocationSize);
            Assert.AreEqual((uint)fs.DirectoryInfo.FromDirectoryName(file).Attributes, e.Attributes);
            Assert.AreEqual(fs.DirectoryInfo.FromDirectoryName(file).LastWriteTimeUtc.ToFileTimeUtc(), e.ChangeTime);
            Assert.AreEqual(fs.DirectoryInfo.FromDirectoryName(file).CreationTimeUtc.ToFileTimeUtc(), e.CreationTime);
            Assert.AreEqual(0, e.EaSize);
            Assert.AreEqual(0, e.FileSize);
            Assert.AreEqual(0, e.HardLinks);
            Assert.AreEqual(0, e.IndexNumber);
            Assert.AreEqual(fs.DirectoryInfo.FromDirectoryName(file).LastAccessTimeUtc.ToFileTimeUtc(), e.LastAccessTime);
            Assert.AreEqual(fs.DirectoryInfo.FromDirectoryName(file).LastWriteTimeUtc.ToFileTimeUtc(), e.LastWriteTime);
            Assert.AreEqual(0, e.ReparseTag);
            Assert.IsTrue(e.IsFile());
        }
    }
}