using Moq;
using NUnit.Framework;
using StorageBackend.IO;

namespace StorageBackend.Win.Tests.Extensions {

    [TestFixture]
    internal class IEntryExtensionTests {

        [Test]
        public void TestGetStruct() {
            //Arrange
            var e = new Mock<IEntry>();
            _ = e.SetupGet(e => e.AllocationSize).Returns(1);
            _ = e.SetupGet(e => e.Attributes).Returns(2);
            _ = e.SetupGet(e => e.ChangeTime).Returns(3);
            _ = e.SetupGet(e => e.CreationTime).Returns(4);
            _ = e.SetupGet(e => e.EaSize).Returns(5);
            _ = e.SetupGet(e => e.FileSize).Returns(6);
            _ = e.SetupGet(e => e.HardLinks).Returns(7);
            _ = e.SetupGet(e => e.IndexNumber).Returns(8);
            _ = e.SetupGet(e => e.LastAccessTime).Returns(9);
            _ = e.SetupGet(e => e.LastWriteTime).Returns(10);
            _ = e.SetupGet(e => e.ReparseTag).Returns(11);

            //Act
            e.Object.GetStruct(out var f);

            //Assert
            Assert.AreEqual(1, f.AllocationSize);
            Assert.AreEqual(2, f.FileAttributes);
            Assert.AreEqual(3, f.ChangeTime);
            Assert.AreEqual(4, f.CreationTime);
            Assert.AreEqual(5, f.EaSize);
            Assert.AreEqual(6, f.FileSize);
            Assert.AreEqual(7, f.HardLinks);
            Assert.AreEqual(8, f.IndexNumber);
            Assert.AreEqual(9, f.LastAccessTime);
            Assert.AreEqual(10, f.LastWriteTime);
            Assert.AreEqual(11, f.ReparseTag);
        }
    }
}