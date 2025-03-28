using Microsoft.VisualStudio.TestTools.UnitTesting;
using streamlas;
using tests;

namespace Writing
{
    [TestClass]
    public class Header
    {
        [TestMethod]
        public void VersionMinor()
        {
            foreach (var file in TestData.BaseFiles)
            {
                string test_path = Utility.WritePath(file);
                using (lasStreamReader lr = new lasStreamReader(test_path))
                {
                    Assert.AreEqual(file.VersionMinor, lr.VersionMinor);
                }
            }
        }

        [TestMethod]
        public void PointFormat()
        {
            foreach (var file in TestData.BaseFiles)
            {
                string test_path = Utility.WritePath(file);
                using (lasStreamReader lr = new lasStreamReader(test_path))
                {
                    Assert.AreEqual(file.PointFormat, lr.PointFormat);
                }
            }
        }

        [TestMethod]
        public void PointCounts()
        {
            foreach (var file in TestData.BaseFiles)
            {
                string test_path = Utility.WritePath(file);
                using (lasStreamReader lr = new lasStreamReader(test_path))
                {
                    Assert.AreEqual(file.PointCount, lr.PointCount);
                }
            }
        }
    }
}
