using Moq;
using NUnit.Framework;

namespace StorageBackend.Win.Tests {

    [TestFixture]
    internal class WindowsExceptionGeneratorTests {

        [Test]
        public void TestGetIOExceptionFromNTException() {
            //Arrange
            var NTException = new Mock<INTException>();
            _ = NTException.Setup(e => e.GetCode()).Returns(FileSystemStatus.STATUS_SUCCESS);

            //Act
            var Exception = WindowsExceptionGenerator.GetIOException(NTException.Object);

            //Assert
            Assert.NotNull(Exception);
            Assert.AreEqual(-2147024896, Exception.HResult);
        }

        [Test]
        public void TestGetIOExceptionFromWin32Exception() {
            //Arrange
            var Win32Exception = new Mock<IWin32Exception>();
            _ = Win32Exception.Setup(e => e.GetCode()).Returns(FileSystemStatus.STATUS_SUCCESS);

            //Act
            var Exception = WindowsExceptionGenerator.GetIOException(Win32Exception.Object);

            //Assert
            Assert.NotNull(Exception);
            Assert.AreEqual(-2147024896, Exception.HResult);
        }
    }
}