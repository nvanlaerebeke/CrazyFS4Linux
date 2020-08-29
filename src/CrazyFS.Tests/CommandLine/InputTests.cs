using CrazyFS.CommandLine;
using NUnit.Framework;

namespace CrazyFS.Tests.CommandLine {
    [TestFixture]
    internal class InputTests {

        [Test]
        public void testUsageInput() {
            //Act
            var ex = false;
            try {
                _ = new Input().Get(new string[] { "?" });
            } catch (UsageException) {
                ex = true;
            }

            //Assert
            Assert.IsTrue(ex);
        }

        [Test]
        public void testInvalidInput() {
            //Act
            var ex = false;
            try {
                _ = new Input().Get(new string[] { "foo" });
            } catch (UsageException) {
                ex = true;
            }

            //Assert
            Assert.IsTrue(ex);
        }

        [Test]
        public void testAllValidParams() {
            //Act
            var o = new Input().Get(new string[] { "-d", "-1", "-D", @"C:\LogFile.log", "-m", "E:", "-p", @"C:\Source", "-u", "\\?" });

            //Assert
            Assert.NotNull(o);
            Assert.AreEqual(4294967295, o.DebugFlags);
            Assert.AreEqual(@"C:\LogFile.log", o.LogFile);
            Assert.AreEqual("E:", o.MountPoint);
            Assert.AreEqual(@"C:\Source", o.SourcePath);
            Assert.AreEqual("\\?", o.UNCPrefix);
        }

    }
}