using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using streamlas;
using tests;

namespace Writing
{
    [TestClass]
    public class Header
    {
        [TestMethod]
        public void ProjectID()
        {
            foreach (var file in TestData.BaseFiles)
            {
                string test_path = TestData.WriteTestPath(file);
                using (lasStreamReader lr = new lasStreamReader(test_path))
                {
                    Assert.AreEqual(file.GUID1, lr.GUID1);
                    Assert.AreEqual(file.GUID2, lr.GUID2);
                    Assert.AreEqual(file.GUID3, lr.GUID3);
                    for (int i = 0; i < 8; i++) Assert.AreEqual(file.GUID4[i], lr.GUID4[i]);
                }
            }
        }

        [TestMethod]
        public void VersionMinor()
        {
            foreach (var file in TestData.BaseFiles)
            {
                string test_path = TestData.WriteTestPath(file);
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
                string test_path = TestData.WriteTestPath(file);
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
                string test_path = TestData.WriteTestPath(file);
                using (lasStreamReader lr = new lasStreamReader(test_path))
                {
                    Assert.AreEqual(file.PointCount, lr.PointCount);
                }
            }
        }

        [TestMethod]
        public void CreationDate()
        {
            foreach (var file in TestData.BaseFiles)
            {
                string test_path = TestData.WriteTestPath(file);
                using (lasStreamReader lr = new lasStreamReader(test_path))
                {
                    Assert.AreEqual(DateTime.Now.DayOfYear, lr.FileCreationDayOfYear);
                    Assert.AreEqual(DateTime.Now.Year, lr.FileCreationYear);
                }
            }
        }
    }
}
