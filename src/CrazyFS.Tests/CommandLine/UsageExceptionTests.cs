using CrazyFS.CommandLine;
using NUnit.Framework;

namespace CrazyFS.Tests.CommandLine {
    [TestFixture]
    internal class UsageExceptionTests {
        [Test]
        public void testWithoutMessage() {
            UsageException ex = new UsageException();
            Assert.IsFalse(ex.HasMessage);
        }

        [Test]
        public void testWithMessage() {
            UsageException ex = new UsageException("message");
            Assert.IsTrue(ex.HasMessage);
            Assert.AreEqual("message", ex.Message);
        }
    }
}
