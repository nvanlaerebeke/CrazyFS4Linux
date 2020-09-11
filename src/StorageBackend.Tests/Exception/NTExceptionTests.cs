using NUnit.Framework;

namespace StorageBackend.Tests.Exception {

    [TestFixture]
    internal class NTExceptionTests {

        [Test]
        public void TestGetCode() {
            var ex = new NTException(666);
            Assert.AreEqual(666, ex.GetCode());
        }
    }
}