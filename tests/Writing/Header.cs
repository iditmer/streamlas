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
            foreach (var info in TestData.BaseFiles)
            {
                string in_path = TestData.WriteTestPath(info);
                using (lasStreamReader lr = new lasStreamReader(in_path))
                {
                    Assert.AreEqual(info.GUID1, lr.GUID1);
                    Assert.AreEqual(info.GUID2, lr.GUID2);
                    Assert.AreEqual(info.GUID3, lr.GUID3);
                    for (int i = 0; i < 8; i++) Assert.AreEqual(info.GUID4[i], lr.GUID4[i]);
                }
            }
        }

        [TestMethod]
        public void VersionMinor()
        {
            foreach (var info in TestData.BaseFiles)
            {
                string in_path = TestData.WriteTestPath(info);
                using (lasStreamReader lr = new lasStreamReader(in_path))
                {
                    Assert.AreEqual(info.VersionMinor, lr.VersionMinor);
                }
            }
        }

        [TestMethod]
        public void Timestamps()
        {
            foreach (var info in TestData.BaseFiles)
            {
                string in_path = TestData.WriteTestPath(info);
                using (lasStreamReader lr = new lasStreamReader(in_path))
                {
                    Assert.AreEqual(info.HasTimestamps, lr.HasTimestamps);
                    Assert.AreEqual(info.AdjustedGPSTime, lr.AdjustedGPSTime);
                }
            }
        }

        [TestMethod]
        public void SyntheticReturns()
        {
            foreach (var info in TestData.BaseFiles)
            {
                string in_path = TestData.WriteTestPath(info);
                using (lasStreamReader lr = new lasStreamReader(in_path))
                {
                    Assert.AreEqual(info.SyntheticReturns, lr.SyntheticReturns);
                }
            }
        }

        [TestMethod]
        public void PointFormat()
        {
            foreach (var info in TestData.BaseFiles)
            {
                string in_path = TestData.WriteTestPath(info);
                using (lasStreamReader lr = new lasStreamReader(in_path))
                {
                    Assert.AreEqual(info.PointFormat, lr.PointFormat);
                }
            }
        }

        [TestMethod]
        public void PointCounts()
        {
            foreach (var info in TestData.BaseFiles)
            {
                string in_path = TestData.WriteTestPath(info);
                using (lasStreamReader lr = new lasStreamReader(in_path))
                {
                    Assert.AreEqual(info.PointCount, lr.PointCount);
                }
            }
        }

        [TestMethod]
        public void CreationDate()
        {
            foreach (var info in TestData.BaseFiles)
            {
                string in_path = TestData.WriteTestPath(info);
                using (lasStreamReader lr = new lasStreamReader(in_path))
                {
                    Assert.AreEqual(DateTime.Now.DayOfYear, lr.FileCreationDayOfYear);
                    Assert.AreEqual(DateTime.Now.Year, lr.FileCreationYear);
                }
            }
        }
    }
}
