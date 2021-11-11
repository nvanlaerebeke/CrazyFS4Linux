using CrazyFS.CommandLine;
using Moq;
using NUnit.Framework;
using StorageBackend;
using System.Threading.Tasks;

namespace CrazyFS.Tests {

    [TestFixture]
    internal class CrazyFSServiceTests {

        [Test]
        public void TestStartStop() {
            //Arrange
            var fs = new Mock<IVolumeActions>();
            var o = new Options() {
                MountPoint = "E:",
                DebugFlags = 666,
                LogFile = "LOGFILEPATH"
            };
            _ = fs.Setup(s => s.Mount(o.MountPoint, null, true, o.DebugFlags, o.LogFile));
            _ = fs.Setup(s => s.UnMount());

            var s = new CrazyFSService(fs.Object, o);

            //Act
            int r = 0;
            _ = Task.Run(() => { r = s.Run(); });
            System.Threading.Thread.Sleep(200);
            s.Stop();

            //Assert
            Assert.AreEqual(0, r);
            fs.Verify(s => s.Mount(o.MountPoint, null, true, o.DebugFlags, o.LogFile), Times.Once);
            fs.Verify(s => s.UnMount(), Times.Once());
        }
    }
}