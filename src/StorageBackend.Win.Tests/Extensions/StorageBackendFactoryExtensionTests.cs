using Moq;
using NUnit.Framework;
using StorageBackend.Win.Tests.TestObject;

namespace StorageBackend.Win.Tests.Extensions {

    [TestFixture]
    internal class StorageBackendFactoryExtensionTests {

        [Test]
        public void TestCreateWindowsStorageBackend() {
            //Arrange
            var b = new Mock<IStorageType>();
            //Act
            var fs = new StorageBackendFactory().CreateWindowsStorageBackend<TestStorageType>("Source");

            //Assert
            Assert.AreEqual(typeof(WindowsFileSystemBase<TestStorageType>), fs.GetType());
        }
    }
}