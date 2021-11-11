using Moq;
using NUnit.Framework;
using StorageBackend.IO;
using System;

namespace StorageBackend.Win.Tests.Extensions {

    [TestFixture]
    internal class IEntryExtensionTests {

        [Test]
        public void TestGetStruct() {
            //Arrange
            var createtime = DateTime.Now.AddSeconds(-10);
            var changetime = DateTime.Now.AddSeconds(-30);
            var lastaccesstime = DateTime.Now.AddSeconds(-60);
            var lastwritetime = DateTime.Now.AddSeconds(-120);

            var e = new Mock<IEntry>();
            _ = e.SetupGet(e => e.AllocationSize).Returns(1);
            _ = e.SetupGet(e => e.Attributes).Returns(System.IO.FileAttributes.Normal);
            _ = e.SetupGet(e => e.ChangeTime).Returns(changetime);
            _ = e.SetupGet(e => e.CreationTime).Returns(createtime);
            _ = e.SetupGet(e => e.EaSize).Returns(5);
            _ = e.SetupGet(e => e.FileSize).Returns(6);
            _ = e.SetupGet(e => e.HardLinks).Returns(7);
            _ = e.SetupGet(e => e.IndexNumber).Returns(8);
            _ = e.SetupGet(e => e.LastAccessTime).Returns(lastaccesstime);
            _ = e.SetupGet(e => e.LastWriteTime).Returns(lastwritetime);
            _ = e.SetupGet(e => e.ReparseTag).Returns(11);

            //Act
            e.Object.GetStruct(out var f);

            //Assert
            Assert.AreEqual(1, f.AllocationSize);
            Assert.AreEqual(2, f.FileAttributes);
            Assert.AreEqual(changetime, f.ChangeTime);
            Assert.AreEqual(createtime, f.CreationTime);
            Assert.AreEqual(5, f.EaSize);
            Assert.AreEqual(6, f.FileSize);
            Assert.AreEqual(7, f.HardLinks);
            Assert.AreEqual(8, f.IndexNumber);
            Assert.AreEqual(lastaccesstime, f.LastAccessTime);
            Assert.AreEqual(lastwritetime, f.LastWriteTime);
            Assert.AreEqual(11, f.ReparseTag);
        }
    }
}