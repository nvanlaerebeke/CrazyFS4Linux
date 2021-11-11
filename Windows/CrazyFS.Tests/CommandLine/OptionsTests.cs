using CrazyFS.CommandLine;
using NUnit.Framework;

namespace CrazyFS.Tests.CommandLine {
    [TestFixture]
    public class OptionsTests {
        [Test]
        public void testFields() {
            var o = new Options() {
                DebugFlags = 4294967295,
                LogFile = "logfile",
                MountPoint = "mountpoint",
                SourcePath = "source",
                UNCPrefix = "unc"
            };

            Assert.AreEqual(4294967295, o.DebugFlags);
            Assert.AreEqual("logfile", o.LogFile);
            Assert.AreEqual("mountpoint", o.MountPoint);
            Assert.AreEqual("source", o.SourcePath);
            Assert.AreEqual("unc", o.UNCPrefix);
        }
    }
}
