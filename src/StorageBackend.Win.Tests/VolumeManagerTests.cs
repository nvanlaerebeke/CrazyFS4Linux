using Moq;
using NUnit.Framework;
using StorageBackend.Win.Winfsp;

namespace StorageBackend.Win.Tests {

    [TestFixture]
    internal class VolumeManagerTests {

        [Test]
        public void TestMount() {
            //Arrange
            var h = new Mock<IFileSystemHost>();

            //Act
            new VolumeManager(h.Object, "Source").Mount("MountPoint", new byte[] { 1, 2, 3 }, true, 0, "LogFile");

            //Assert
            h.Verify(x => x.Mount("MountPoint", new byte[] { 1, 2, 3 }, true, 0), Times.Once());
        }

        [Test]
        public void TestUnMount() {
            //Arrange
            var h = new Mock<IFileSystemHost>();

            //Act
            new VolumeManager(h.Object, "Source").UnMount();

            //Assert
            h.Verify(x => x.Unmount(), Times.Once());
        }

        [Test]
        public void TestInitialize() {
            //Arrange
            var h = new Mock<IFileSystemHost>();
            h.SetupSet(x => x.SectorSize = It.IsAny<ushort>()).Verifiable();
            h.SetupSet(x => x.SectorsPerAllocationUnit = It.IsAny<ushort>()).Verifiable();
            h.SetupSet(x => x.MaxComponentLength = It.IsAny<ushort>()).Verifiable();
            h.SetupSet(x => x.FileInfoTimeout = It.IsAny<ushort>()).Verifiable();
            h.SetupSet(x => x.CaseSensitiveSearch = It.IsAny<bool>()).Verifiable();
            h.SetupSet(x => x.CasePreservedNames = It.IsAny<bool>()).Verifiable();
            h.SetupSet(x => x.UnicodeOnDisk = It.IsAny<bool>()).Verifiable();
            h.SetupSet(x => x.PersistentAcls = It.IsAny<bool>()).Verifiable();
            h.SetupSet(x => x.PostCleanupWhenModifiedOnly = It.IsAny<bool>()).Verifiable();
            h.SetupSet(x => x.PassQueryDirectoryPattern = It.IsAny<bool>()).Verifiable();
            h.SetupSet(x => x.FlushAndPurgeOnCleanup = It.IsAny<bool>()).Verifiable();
            h.SetupSet(x => x.VolumeCreationTime = It.IsAny<ulong>()).Verifiable();
            h.SetupSet(x => x.VolumeSerialNumber = It.IsAny<uint>()).Verifiable();

            //Act
            var r = new VolumeManager(h.Object, "Source").Initialize(1);

            //Assert
            Assert.AreEqual(0, r);
            h.VerifySet(x => x.SectorSize = 4096);
            h.VerifySet(x => x.SectorsPerAllocationUnit = 1);
            h.VerifySet(x => x.MaxComponentLength = 255);
            h.VerifySet(x => x.FileInfoTimeout = 1000);
            h.VerifySet(x => x.CaseSensitiveSearch = false);
            h.VerifySet(x => x.CasePreservedNames = true);
            h.VerifySet(x => x.UnicodeOnDisk = true);
            h.VerifySet(x => x.PersistentAcls = true);
            h.VerifySet(x => x.PostCleanupWhenModifiedOnly = true);
            h.VerifySet(x => x.PassQueryDirectoryPattern = true);
            h.VerifySet(x => x.FlushAndPurgeOnCleanup = true);
            h.VerifySet(x => x.VolumeCreationTime = 1);
            h.VerifySet(x => x.VolumeSerialNumber = 0);
        }
    }
}