using Microsoft.VisualStudio.TestTools.UnitTesting;
using streamlas;
using tests;

namespace Reading
{
    [TestClass]
    public class Header
    {
        [TestMethod]
        public void FileSourceID()
        {
            foreach (var info in TestData.BaseFiles)
            {
                using (lasStreamReader lr = new lasStreamReader(info.FileName))
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
                using (lasStreamReader lr = new lasStreamReader(info.FileName))
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
                using (lasStreamReader lr = new lasStreamReader(info.FileName))
                {
                    Assert.AreEqual(info.SystemIdentifier, lr.SystemIdentifier);
                }
            }
        }

        [TestMethod]
        public void GeneratingSoftware()
        {
            foreach (var info in TestData.BaseFiles)
            {
                using (lasStreamReader lr = new lasStreamReader(info.FileName))
                {
                    Assert.AreEqual(info.GeneratingSoftware, lr.GeneratingSoftware);
                }
            }
        }

        [TestMethod]
        public void CreationDate()
        {
            foreach (var info in TestData.BaseFiles)
            {
                using (lasStreamReader lr = new lasStreamReader(info.FileName))
                {
                    Assert.AreEqual(info.FileCreationDay, lr.FileCreationDayOfYear);
                    Assert.AreEqual(info.FileCreationYear, lr.FileCreationYear);
                }
            }
        }

        [TestMethod]
        public void VersionMinor()
        {
            foreach (var info in TestData.BaseFiles)
            {
                using (lasStreamReader lr = new lasStreamReader(info.FileName))
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
                using (lasStreamReader lr = new lasStreamReader(info.FileName))
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
                using (lasStreamReader lr = new lasStreamReader(info.FileName))
                {
                    Assert.AreEqual(info.SyntheticReturns, lr.SyntheticReturns);
                }
            }
        }

        [TestMethod]
        public void SpatialReferenceWKT()
        {
            foreach (var info in TestData.BaseFiles)
            {
                using (lasStreamReader lr = new lasStreamReader(info.FileName))
                {
                    Assert.AreEqual(info.CRSisWKT, lr.SpatialReferenceIsWKT);
                }
            }
        }

        [TestMethod]
        public void NumberVariableLengthRecords()
        {
            foreach (var info in TestData.BaseFiles)
            {
                using (lasStreamReader lr = new lasStreamReader(info.FileName))
                {
                    Assert.AreEqual((uint)info.VariableLengthRecords.Length, lr.NumberVariableLengthRecords);
                }
            }
        }

        [TestMethod]
        public void PointFormat()
        {
            foreach (var info in TestData.BaseFiles)
            {
                using (lasStreamReader lr = new lasStreamReader(info.FileName))
                {
                    Assert.AreEqual(info.PointFormat, lr.PointFormat);
                }
            }
        }

        [TestMethod]
        public void PointCount()
        {
            foreach (var info in TestData.BaseFiles)
            {
                using (lasStreamReader lr = new lasStreamReader(info.FileName))
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
                using (lasStreamReader lr = new lasStreamReader(info.FileName))
                {
                    for (int i = 0; i < 15; i++)
                    {
                        Assert.AreEqual(info.NumberPointsByReturn[i], lr.NumberPointsByReturn[i]);
                    }
                }
            }
        }

        [TestMethod]
        public void CoordinateExtents()
        {
            foreach (var info in TestData.BaseFiles)
            {
                using (lasStreamReader lr = new lasStreamReader(info.FileName))
                {
                    for (int i = 0; i < 3; i++)
                    {
                        Assert.AreEqual(info.MinCoords[i], lr.MinimumXYZ[i]);
                        Assert.AreEqual(info.MaxCoords[i], lr.MaximumXYZ[i]);
                    }
                }
            }
        }
    }
}