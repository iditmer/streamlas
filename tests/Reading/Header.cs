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
            foreach (var file in TestData.BaseFiles)
            {
                using (lasStreamReader lr = new lasStreamReader(file.FileName))
                {
                    Assert.AreEqual(file.SourceID, lr.FileSourceID);
                }
            }
        }

        [TestMethod]
        public void ProjectID()
        {
            foreach (var file in TestData.BaseFiles)
            {
                using (lasStreamReader lr = new lasStreamReader(file.FileName))
                {
                    Assert.AreEqual(file.GUID1, lr.GUID1);
                    Assert.AreEqual(file.GUID2, lr.GUID2);
                    Assert.AreEqual(file.GUID3, lr.GUID3);
                    for (int i = 0; i < 8; i++) Assert.AreEqual(file.GUID4[i], lr.GUID4[i]);
                }
            }
        }

        [TestMethod]
        public void SystemIdentifier()
        {
            foreach (var file in TestData.BaseFiles)
            {
                using (lasStreamReader lr = new lasStreamReader(file.FileName))
                {
                    Assert.AreEqual(file.SystemIdentifier, lr.SystemIdentifier);
                }
            }
        }

        [TestMethod]
        public void GeneratingSoftware()
        {
            foreach (var file in TestData.BaseFiles)
            {
                using (lasStreamReader lr = new lasStreamReader(file.FileName))
                {
                    Assert.AreEqual(file.GeneratingSoftware, lr.GeneratingSoftware);
                }
            }
        }

        [TestMethod]
        public void CreationDate()
        {
            foreach (var file in TestData.BaseFiles)
            {
                using (lasStreamReader lr = new lasStreamReader(file.FileName))
                {
                    Assert.AreEqual(file.FileCreationDay, lr.FileCreationDayOfYear);
                    Assert.AreEqual(file.FileCreationYear, lr.FileCreationYear);
                }
            }
        }

        [TestMethod]
        public void VersionMinor()
        {
            foreach (var file in TestData.BaseFiles)
            {
                using (lasStreamReader lr = new lasStreamReader(file.FileName))
                {
                    Assert.AreEqual(file.VersionMinor, lr.VersionMinor);
                }
            }
        }

        [TestMethod]
        public void Timestamps()
        {
            foreach (var file in TestData.BaseFiles)
            {
                using (lasStreamReader lr = new lasStreamReader(file.FileName))
                {
                    Assert.AreEqual(file.HasTimestamps, lr.HasTimestamps);
                    Assert.AreEqual(file.AdjustedGPSTime, lr.AdjustedGPSTime);
                }
            }
        }

        [TestMethod]
        public void SyntheticReturns()
        {
            foreach (var file in TestData.BaseFiles)
            {
                using (lasStreamReader lr = new lasStreamReader(file.FileName))
                {
                    Assert.AreEqual(file.SyntheticReturns, lr.SyntheticReturns);
                }
            }
        }

        [TestMethod]
        public void PointFormat()
        {
            foreach (var file in TestData.BaseFiles)
            {
                using (lasStreamReader lr = new lasStreamReader(file.FileName))
                {
                    Assert.AreEqual(file.PointFormat, lr.PointFormat);
                }
            }
        }

        [TestMethod]
        public void PointCount()
        {
            foreach (var file in TestData.BaseFiles)
            {
                using (lasStreamReader lr = new lasStreamReader(file.FileName))
                {
                    Assert.AreEqual(file.PointCount, lr.PointCount);
                }
            }
        }

        [TestMethod]
        public void PointsByReturn()
        {
            foreach (var file in TestData.BaseFiles)
            {
                using (lasStreamReader lr = new lasStreamReader(file.FileName))
                {
                    for (int i = 0; i < 15; i++)
                    {
                        Assert.AreEqual(file.NumberPointsByReturn[i], lr.NumberPointsByReturn[i]);
                    }
                }
            }
        }

        [TestMethod]
        public void CoordinateExtents()
        {
            foreach (var file in TestData.BaseFiles)
            {
                using (lasStreamReader lr = new lasStreamReader(file.FileName))
                {
                    for (int i = 0; i < 3; i++)
                    {
                        Assert.AreEqual(lr.MinimumXYZ[i], file.MinCoords[i]);
                        Assert.AreEqual(lr.MaximumXYZ[i], file.MaxCoords[i]);
                    }
                }
            }
        }
    }
}