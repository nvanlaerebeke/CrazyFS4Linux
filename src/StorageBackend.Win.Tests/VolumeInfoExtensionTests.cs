using Moq;
using NUnit.Framework;
using StorageBackend.Volume;

namespace StorageBackend.Win.Tests {

    [TestFixture]
    internal class VolumeInfoExtensionTests {

        [Test]
        public void TestGetStruct() {
            //Arrange
            var v = new Mock<IVolumeInfo>();
            _ = v.SetupGet(f => f.FreeSize).Returns(666);
            _ = v.SetupGet(f => f.TotalSize).Returns(999);

            //Act
            v.Object.GetStruct(out var i);

            //Assert
            Assert.AreEqual(666, i.FreeSize);
            Assert.AreEqual(999, i.TotalSize);
        }

        /*[Test]
        public void TestCrazyFSFileSystemHostMount() {
            //Arrange
            var fh = new Mock<IFileSystemHost>();

            //Act
            new CrazyFSFileSystemHost(fh.Object).Mount("MountPoint", new byte[] { 0, 1, 2, 3 }, true, 666);

            //Assert
            fh.Verify(x => x.Mount("MountPoint", new byte[] { 0, 1, 2, 3 }, true, 666), Times.Once());
        }

        [Test]
        public void TestCrazyFSFileSystemHostUnmount() {
            //Arrange
            var fh = new Mock<IFileSystemHost>();

            //Act
            new CrazyFSFileSystemHost(fh.Object).Unmount();

            //Assert
            fh.Verify(x => x.Unmount(), Times.Once());
        }*/
    }
}