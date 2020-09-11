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
    }
}