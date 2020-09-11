using Moq;
using NUnit.Framework;

namespace StorageBackend.Win.Tests.Extensions {

    [TestFixture]
    internal class StorageBackendFactoryExtensionTests {

        [Test]
        public void TestCreateWindowsStorageBackend() {
            //Arrange
            var b = new Mock<IStorageType>();

            //Act
            var fs = new StorageBackendFactory().CreateWindowsStorageBackend(b.Object);

            //Assert
            Assert.AreEqual(typeof(WindowsFileSystemBase), fs.GetType());
        }
    }
}