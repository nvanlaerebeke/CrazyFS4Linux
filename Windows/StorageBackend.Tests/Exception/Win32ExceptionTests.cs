using NUnit.Framework;

namespace StorageBackend.Tests.Exception {
    [TestFixture]
    internal class Win32ExceptionTests {

        [Test]
        public void testGetCode() {
            var ex = new Win32Exception(666);
            Assert.AreEqual(666, ex.GetCode());
        }
    }
}
