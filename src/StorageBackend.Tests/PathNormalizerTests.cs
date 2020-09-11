using NUnit.Framework;

namespace StorageBackend.Tests {

    [TestFixture]
    internal class PathNormalizerTests {

        [Test]
        public void TestGetDriveLetter() {
            Assert.AreEqual(@"E:\", PathNormalizer.GetDriveLetter(@"E:\My\Sub\Folder"));
            Assert.AreEqual(@"E:\", PathNormalizer.GetDriveLetter(@"E:\My\Sub\Folder\myfile.txt"));
            Assert.AreEqual(@"E:", PathNormalizer.GetDriveLetter(@"E:"));
            Assert.AreEqual(@"E:\", PathNormalizer.GetDriveLetter(@"E:\My\Sub\Fo:lder"));
            Assert.AreEqual(@"E:\", PathNormalizer.GetDriveLetter(@"E:\My\Sub\Fo/lder"));
        }

        [Test]
        public void TestConcatPath() {
            Assert.AreEqual(@"E:\My\Sub\Folder", PathNormalizer.ConcatPath(@"E:\My\", @"Sub\Folder\"));
            Assert.AreEqual(@"\Sub\Folder", PathNormalizer.ConcatPath(@"E:\My\", @"\Sub\Folder\"));
        }
    }
}