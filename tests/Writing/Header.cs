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
        public void FileSourceID()
        {
            foreach (var info in TestData.BaseFiles)
            {
                string in_path = TestData.WriteTestPath(info);
                using (lasStreamReader lr = new lasStreamReader(in_path))
                {
                    Assert.AreEqual(info.SourceID, lr.FileSourceID);
                }
            }
        }

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
        public void SystemIdentifier()
        {
            foreach (var info in TestData.BaseFiles)
            {
                string in_path = TestData.WriteTestPath(info);
                using (lasStreamReader lr = new lasStreamReader(in_path))
                {
                    if (info.SystemIdentifier == "OTHER") Assert.AreEqual(info.SystemIdentifier, lr.SystemIdentifier);
                    else Assert.AreEqual("MODIFICATION", lr.SystemIdentifier);
                }
            }
        }

        [TestMethod]
        public void GeneratingSoftware()
        {
            foreach (var info in TestData.BaseFiles)
            {
                string in_path = TestData.WriteTestPath(info);
                using (lasStreamReader lr = new lasStreamReader(in_path))
                {
                    Assert.AreEqual("streamlas", lr.GeneratingSoftware);
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
        public void PointsByReturn()
        {
            foreach (var info in TestData.BaseFiles)
            {
                string in_path = TestData.WriteTestPath(info);
                using (lasStreamReader lr = new lasStreamReader(in_path))
                {
                    for (int i = 0; i < 15; i++)
                    {
                        Assert.AreEqual(info.NumberPointsByReturn[i], lr.NumberPointsByReturn[i]);
                    }
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

        [TestMethod]
        public void CoordinateExtents()
        {
            foreach (var info in TestData.BaseFiles)
            {
                string in_path = TestData.WriteTestPath(info);
                using (lasStreamReader lr = new lasStreamReader(in_path))
                {
                    for (int i = 0; i < 3; i++)
                    {
                        Assert.AreEqual(info.MinCoords[i], lr.MinimumXYZ[i], 1e-6);
                        Assert.AreEqual(info.MaxCoords[i], lr.MaximumXYZ[i], 1e-6);
                    }
                }
            }
        }
    }
}
