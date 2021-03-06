﻿using NUnit.Framework;
using StorageBackend.Win.Tests.TestObject;
using StorageBackend.Win.Winfsp;

namespace StorageBackend.Win.Tests.Extensions {

    [TestFixture]
    internal class StorageBackendFactoryExtensionTests {

        [Test]
        public void TestCreateWindowsStorageBackend() {
            //Act
            var fs = new StorageBackendFactory().CreateWindowsWinfspStorageBackend<TestStorageType>("Source");

            //Assert
            Assert.AreEqual(typeof(WindowsFileSystemBase<TestStorageType>), fs.GetType());
        }
    }
}