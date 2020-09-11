using NUnit.Framework;

namespace StorageBackend.Win.Tests {

    [TestFixture]
    internal class CrazyFSFileSystemHostTests {
        /*
                [Test]
                public void TestCrazyFSFileSystemHostProps() {
                    //Arrange
                    var fh = new Mock<IFileSystemHost>();
                    _ = fh.SetupGet(f => f.SectorSize).Returns(1);
                    _ = fh.SetupGet(f => f.SectorsPerAllocationUnit).Returns(2);
                    _ = fh.SetupGet(f => f.MaxComponentLength).Returns(3);
                    _ = fh.SetupGet(f => f.FileInfoTimeout).Returns(4);
                    _ = fh.SetupGet(f => f.CaseSensitiveSearch).Returns(true);
                    _ = fh.SetupGet(f => f.CasePreservedNames).Returns(true);
                    _ = fh.SetupGet(f => f.PostCleanupWhenModifiedOnly).Returns(true);
                    _ = fh.SetupGet(f => f.VolumeCreationTime).Returns(5);
                    _ = fh.SetupGet(f => f.VolumeSerialNumber).Returns(6);
                    _ = fh.SetupGet(f => f.FlushAndPurgeOnCleanup).Returns(true);
                    _ = fh.SetupGet(f => f.PassQueryDirectoryPattern).Returns(true);
                    _ = fh.SetupGet(f => f.PersistentAcls).Returns(true);
                    _ = fh.SetupGet(f => f.UnicodeOnDisk).Returns(true);
                    _ = fh.SetupGet(f => f.StreamInfoTimeout).Returns(7);

                    //Act
                    var fshost = new CrazyFSFileSystemHost(fh.Object);

                    //Assert
                    Assert.AreEqual(1, fshost.SectorSize);
                    Assert.AreEqual(2, fshost.SectorsPerAllocationUnit);
                    Assert.AreEqual(3, fshost.MaxComponentLength);
                    Assert.AreEqual(4, fshost.FileInfoTimeout);
                    Assert.AreEqual(true, fshost.CaseSensitiveSearch);
                    Assert.AreEqual(true, fshost.CasePreservedNames);
                    Assert.AreEqual(true, fshost.PostCleanupWhenModifiedOnly);
                    Assert.AreEqual(5, fshost.VolumeCreationTime);
                    Assert.AreEqual(6, fshost.VolumeSerialNumber);
                    Assert.AreEqual(true, fshost.FlushAndPurgeOnCleanup);
                    Assert.AreEqual(true, fshost.PassQueryDirectoryPattern);
                    Assert.AreEqual(true, fshost.PersistentAcls);
                    Assert.AreEqual(true, fshost.UnicodeOnDisk);
                    Assert.AreEqual(7, fshost.StreamInfoTimeout);
                }

                [Test]
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