using Moq;
using NUnit.Framework;
using System;
using System.Security.AccessControl;

namespace StorageBackend.Win.Tests {

    [TestFixture]
    internal class WindowsFileSystemBaseTests {

        [Test]
        public void TestExceptionHandler() {
            //Arrange
            var ex = new Exception("MyMessage");
            var s = new Mock<IStorageBackendWrapper>();
            _ = s.Setup(x => x.ExceptionHandler(ex)).Returns(666);

            //Act
            var fs = new WindowsFileSystemBase(s.Object);
            var r = fs.ExceptionHandler(ex);

            //Assert
            Assert.AreEqual(666, r);
            s.Verify(f => f.ExceptionHandler(ex), Times.Once());
        }

        [Test]
        public void TestInit() {
            //Arrange
            var o = new object();
            var s = new Mock<IStorageBackendWrapper>();
            _ = s.Setup(x => x.Init(o)).Returns(666);

            //Act
            var fs = new WindowsFileSystemBase(s.Object);
            var r = fs.Init(o);

            //Assert
            Assert.AreEqual(666, r);
            s.Verify(f => f.Init(o), Times.Once());
        }

        [Test]
        public void TestGetVolumeInfo() {
            //Arrange
            var v = new Fsp.Interop.VolumeInfo();
            var s = new Mock<IStorageBackendWrapper>();
            _ = s.Setup(x => x.GetVolumeInfo(out v)).Returns(666);

            //Act
            var fs = new WindowsFileSystemBase(s.Object);
            var r = fs.GetVolumeInfo(out v);

            //Assert
            Assert.AreEqual(666, r);
            s.Verify(f => f.GetVolumeInfo(out v), Times.Once());
        }

        [Test]
        public void TestGetSecurityByName() {
            //Arrange
            byte[] SecurityDescription = null;
            uint FileAttributes = 123;
            var s = new Mock<IStorageBackendWrapper>();
            _ = s.Setup(x => x.GetSecurityByName("FileName", out FileAttributes, ref SecurityDescription)).Returns(666);

            //Act
            var fs = new WindowsFileSystemBase(s.Object);
            var r = fs.GetSecurityByName("FileName", out FileAttributes, ref SecurityDescription);

            //Assert
            Assert.AreEqual(666, r);
            s.Verify(f => f.GetSecurityByName("FileName", out FileAttributes, ref SecurityDescription), Times.Once());
        }

        [Test]
        public void TestCreate() {
            //Arrange
            var FileNode = new object();
            var FileDesc = new object();
            Fsp.Interop.FileInfo FileInfo;
            var NormalizedName = "";

            var s = new Mock<IStorageBackendWrapper>();
            _ = s.Setup(x => x.Create("FileName", 1, 2, 3, new byte[] { 1, 2, 3, 4 }, 4, out FileNode, out FileDesc, out FileInfo, out NormalizedName)).Returns(666);
            //Act
            var fs = new WindowsFileSystemBase(s.Object);
            var r = fs.Create("FileName", 1, 2, 3, new byte[] { 1, 2, 3, 4 }, 4, out FileNode, out FileDesc, out FileInfo, out NormalizedName);

            //Assert
            Assert.AreEqual(666, r);
            s.Verify(f => f.Create("FileName", 1, 2, 3, new byte[] { 1, 2, 3, 4 }, 4, out FileNode, out FileDesc, out FileInfo, out NormalizedName), Times.Once());
        }

        [Test]
        public void TestOpen() {
            //Arrange
            var s = new Mock<IStorageBackendWrapper>();
            var FileNode = new object();
            var FileDesc = new object();
            var NormalizedName = "";
            Fsp.Interop.FileInfo FileInfo;

            _ = s.Setup(x => x.Open("FileName", 1, 2, out FileNode, out FileDesc, out FileInfo, out NormalizedName)).Returns(666);
            //Act
            var fs = new WindowsFileSystemBase(s.Object);
            var r = fs.Open("FileName", 1, 2, out FileNode, out FileDesc, out FileInfo, out NormalizedName);

            //Assert
            Assert.AreEqual(666, r);
            s.Verify(f => f.Open("FileName", 1, 2, out FileNode, out FileDesc, out FileInfo, out NormalizedName), Times.Once());
        }

        [Test]
        public void TestOverwrite() {
            //Arrange
            var s = new Mock<IStorageBackendWrapper>();
            var FileNode = new object();
            var FileDesc = new object();
            Fsp.Interop.FileInfo FileInfo;
            s.Setup(x => x.Overwrite(FileNode, FileDesc, 1, true, 2, out FileInfo)).Returns(666);
            //Act
            var fs = new WindowsFileSystemBase(s.Object);
            var r = fs.Overwrite(FileNode, FileDesc, 1, true, 2, out FileInfo);

            //Assert
            Assert.AreEqual(666, r);
            s.Verify(f => f.Overwrite(FileNode, FileDesc, 1, true, 2, out FileInfo), Times.Once());
        }

        [Test]
        public void TestCleanup() {
            //Arrange
            var s = new Mock<IStorageBackendWrapper>();
            var FileNode = new object();
            var FileDesc = new object();

            //Act
            var fs = new WindowsFileSystemBase(s.Object);
            fs.Cleanup(FileNode, FileDesc, "FileName", 1);

            //Assert
            s.Verify(f => f.Cleanup(FileNode, FileDesc, "FileName", 1), Times.Once());
        }

        [Test]
        public void TestClose() {
            //Arrange
            var s = new Mock<IStorageBackendWrapper>();
            var FileNode = new object();
            var FileDesc = new object();

            //Act
            var fs = new WindowsFileSystemBase(s.Object);
            fs.Close(FileNode, FileDesc);

            //Assert
            s.Verify(f => f.Close(FileNode, FileDesc), Times.Once());
        }

        [Test]
        public void TestRead() {
            //Arrange
            var s = new Mock<IStorageBackendWrapper>();
            var FileNode = new object();
            var FileDesc = new object();
            var Buffer = new IntPtr();
            uint BytesTransfered = 0;
            _ = s.Setup(x => x.Read(FileNode, FileDesc, Buffer, 1, 2, out BytesTransfered)).Returns(666);

            //Act
            var fs = new WindowsFileSystemBase(s.Object);
            var r = fs.Read(FileNode, FileDesc, Buffer, 1, 2, out BytesTransfered);

            //Assert
            Assert.AreEqual(666, r);
            s.Verify(f => f.Close(FileNode, FileDesc), Times.Once());
        }

        [Test]
        public void TestWrite() {
            //Arrange
            var s = new Mock<IStorageBackendWrapper>();
            var FileNode = new object();
            var FileDesc = new object();
            var Buffer = new IntPtr();
            uint BytesTransfered = 0;
            Fsp.Interop.FileInfo FileInfo;
            _ = s.Setup(x => x.Write(FileNode, FileDesc, Buffer, 1, 2, true, true, out BytesTransfered, out FileInfo)).Returns(666);

            //Act
            var fs = new WindowsFileSystemBase(s.Object);
            var r = fs.Write(FileNode, FileDesc, Buffer, 1, 2, true, true, out BytesTransfered, out FileInfo);

            //Assert
            Assert.AreEqual(666, r);
            s.Verify(f => f.Write(FileNode, FileDesc, Buffer, 1, 2, true, true, out BytesTransfered, out FileInfo), Times.Once());
        }

        [Test]
        public void TestFlush() {
            //Arrange
            var s = new Mock<IStorageBackendWrapper>();
            var FileNode = new object();
            var FileDesc = new object();
            Fsp.Interop.FileInfo FileInfo;
            _ = s.Setup(x => x.Flush(FileNode, FileDesc, out FileInfo)).Returns(666);

            //Act
            var fs = new WindowsFileSystemBase(s.Object);
            var r = fs.Flush(FileNode, FileDesc, out FileInfo);

            //Assert
            Assert.AreEqual(666, r);
            s.Verify(f => f.Flush(FileNode, FileDesc, out FileInfo), Times.Once());
        }

        [Test]
        public void TestGetFileInfo() {
            //Arrange
            var s = new Mock<IStorageBackendWrapper>();
            var FileNode = new object();
            var FileDesc = new object();
            Fsp.Interop.FileInfo FileInfo;
            _ = s.Setup(x => x.GetFileInfo(FileNode, FileDesc, out FileInfo)).Returns(666);

            //Act
            var fs = new WindowsFileSystemBase(s.Object);
            var r = fs.GetFileInfo(FileNode, FileDesc, out FileInfo);

            //Assert
            Assert.AreEqual(666, r);
            s.Verify(f => f.GetFileInfo(FileNode, FileDesc, out FileInfo), Times.Once());
        }

        [Test]
        public void TestSetBasicInfo() {
            //Arrange
            var s = new Mock<IStorageBackendWrapper>();
            var FileNode = new object();
            var FileDesc = new object();
            Fsp.Interop.FileInfo FileInfo;
            _ = s.Setup(x => x.SetBasicInfo(FileNode, FileDesc, 1, 2, 3, 5, 6, out FileInfo)).Returns(666);

            //Act
            var fs = new WindowsFileSystemBase(s.Object);
            var r = fs.SetBasicInfo(FileNode, FileDesc, 1, 2, 3, 5, 6, out FileInfo);

            //Assert
            Assert.AreEqual(666, r);
            s.Verify(f => f.SetBasicInfo(FileNode, FileDesc, 1, 2, 3, 5, 6, out FileInfo), Times.Once());
        }

        [Test]
        public void TestSetFileSize() {
            //Arrange
            var s = new Mock<IStorageBackendWrapper>();
            var FileNode = new object();
            var FileDesc = new object();
            Fsp.Interop.FileInfo FileInfo;
            _ = s.Setup(x => x.SetFileSize(FileNode, FileDesc, 1, true, out FileInfo)).Returns(666);

            //Act
            var fs = new WindowsFileSystemBase(s.Object);
            var r = fs.SetFileSize(FileNode, FileDesc, 1, true, out FileInfo);

            //Assert
            Assert.AreEqual(666, r);
            s.Verify(f => f.SetFileSize(FileNode, FileDesc, 1, true, out FileInfo), Times.Once());
        }

        [Test]
        public void TestCanDelete() {
            //Arrange
            var s = new Mock<IStorageBackendWrapper>();
            var FileNode = new object();
            var FileDesc = new object();
            _ = s.Setup(x => x.CanDelete(FileNode, FileDesc, "FileName")).Returns(666);

            //Act
            var fs = new WindowsFileSystemBase(s.Object);
            var r = fs.CanDelete(FileNode, FileDesc, "FileName");

            //Assert
            Assert.AreEqual(666, r);
            s.Verify(f => f.CanDelete(FileNode, FileDesc, "FileName"), Times.Once());
        }

        [Test]
        public void TestRename() {
            //Arrange
            var s = new Mock<IStorageBackendWrapper>();
            var FileNode = new object();
            var FileDesc = new object();
            _ = s.Setup(x => x.Rename(FileNode, FileDesc, "OldFileName", "NewFileName", true)).Returns(666);

            //Act
            var fs = new WindowsFileSystemBase(s.Object);
            var r = fs.Rename(FileNode, FileDesc, "OldFileName", "NewFileName", true);

            //Assert
            Assert.AreEqual(666, r);
            s.Verify(f => f.Rename(FileNode, FileDesc, "OldFileName", "NewFileName", true), Times.Once());
        }

        [Test]
        public void TestGetSecurity() {
            //Arrange
            var s = new Mock<IStorageBackendWrapper>();
            var FileNode = new object();
            var FileDesc = new object();
            byte[] SecurityDescriptor = null;

            _ = s.Setup(x => x.GetSecurity(FileNode, FileDesc, ref SecurityDescriptor)).Returns(666);

            //Act
            var fs = new WindowsFileSystemBase(s.Object);
            var r = fs.GetSecurity(FileNode, FileDesc, ref SecurityDescriptor);

            //Assert
            Assert.AreEqual(666, r);
            s.Verify(f => f.GetSecurity(FileNode, FileDesc, ref SecurityDescriptor), Times.Once());
        }

        [Test]
        public void TestSetSecurity() {
            //Arrange
            var s = new Mock<IStorageBackendWrapper>();
            var FileNode = new object();
            var FileDesc = new object();
            byte[] SecurityDescriptor = null;
            var Sections = AccessControlSections.All;

            _ = s.Setup(x => x.SetSecurity(FileNode, FileDesc, Sections, SecurityDescriptor)).Returns(666);

            //Act
            var fs = new WindowsFileSystemBase(s.Object);
            var r = fs.SetSecurity(FileNode, FileDesc, Sections, SecurityDescriptor);

            //Assert
            Assert.AreEqual(666, r);
            s.Verify(f => f.SetSecurity(FileNode, FileDesc, Sections, SecurityDescriptor), Times.Once());
        }

        [Test]
        public void TestReadDirectoryEntry() {
            //Arrange
            var s = new Mock<IStorageBackendWrapper>();
            var FileNode = new object();
            var FileDesc = new object();
            var Context = new object();
            var FileName = "";
            Fsp.Interop.FileInfo FileInfo;
            _ = s.Setup(x => x.ReadDirectoryEntry(FileNode, FileDesc, "Pattern", "Marker", ref Context, out FileName, out FileInfo)).Returns(true);

            //Act
            var fs = new WindowsFileSystemBase(s.Object);
            var r = fs.ReadDirectoryEntry(FileNode, FileDesc, "Pattern", "Marker", ref Context, out FileName, out FileInfo);

            //Assert
            Assert.AreEqual(true, r);
            s.Verify(f => f.ReadDirectoryEntry(FileNode, FileDesc, "Pattern", "Marker", ref Context, out FileName, out FileInfo), Times.Once());
        }
    }
}