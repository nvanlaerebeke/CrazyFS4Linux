using Moq;
using NUnit.Framework;
using StorageBackend.Volume;
using System.IO.Abstractions;

namespace StorageBackend.Tests.Exception {

    [TestFixture]
    internal class VolumeInfoTests {

        [Test]
        public void TestCreate() {
            //Arrange
            var vinfo = new Mock<IDriveInfo>();
            _ = vinfo.SetupGet(i => i.AvailableFreeSpace).Returns(666);
            _ = vinfo.SetupGet(i => i.TotalSize).Returns(999);

            //Act
            var o = new VolumeInfo(vinfo.Object, "MyLabel");

            //Assert
            Assert.AreEqual(666, o.FreeSize);
            Assert.AreEqual(999, o.TotalSize);
        }
    }
}