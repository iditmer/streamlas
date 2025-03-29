using Microsoft.VisualStudio.TestTools.UnitTesting;
using streamlas;
using System;
using tests;

namespace Reading
{
    [TestClass]
    public class Points
    {
        [TestMethod]
        public void Coordinates()
        {
            foreach (var info in TestData.BaseFiles)
            {
                using (lasStreamReader lr = new lasStreamReader(info.FileName))
                {
                    var ref_pts = TestData.BaseFilePoints[lr.PointFormat];
                    lasPointRecord pt = new lasPointRecord(lr);

                    for (int i = 0; i < ref_pts.Count; i++)
                    {
                        pt.ReadFrom(lr);
                        Assert.AreEqual(ref_pts[i].X, pt.X, 1e-6);
                        Assert.AreEqual(ref_pts[i].Y, pt.Y, 1e-6);
                        Assert.AreEqual(ref_pts[i].Z, pt.Z, 1e-6);
                    }
                }
            }
        }

        [TestMethod]
        public void Intensity()
        {
            foreach (var info in TestData.BaseFiles)
            {
                using (lasStreamReader lr = new lasStreamReader(info.FileName))
                {
                    var ref_pts = TestData.BaseFilePoints[lr.PointFormat];
                    lasPointRecord pt = new lasPointRecord(lr);

                    for (int i = 0; i < ref_pts.Count; i++)
                    {
                        pt.ReadFrom(lr);
                        Assert.AreEqual(ref_pts[i].Intensity, pt.Intensity);
                    }
                }
            }
        }

        [TestMethod]
        public void Returns()
        {
            foreach (var info in TestData.BaseFiles)
            {
                using (lasStreamReader lr = new lasStreamReader(info.FileName))
                {
                    var ref_pts = TestData.BaseFilePoints[lr.PointFormat];
                    lasPointRecord pt = new lasPointRecord(lr);

                    for (int i = 0; i < ref_pts.Count; i++)
                    {
                        pt.ReadFrom(lr);
                        Assert.AreEqual(ref_pts[i].Return, pt.ReturnNumber);
                        Assert.AreEqual(ref_pts[i].NumberReturns, pt.NumberReturns);
                    }
                }
            }
        }

        [TestMethod]
        public void Classification()
        {
            foreach (var info in TestData.BaseFiles)
            {
                using (lasStreamReader lr = new lasStreamReader(info.FileName))
                {
                    var ref_pts = TestData.BaseFilePoints[lr.PointFormat];
                    lasPointRecord pt = new lasPointRecord(lr);

                    for (int i = 0; i < ref_pts.Count; i++)
                    {
                        pt.ReadFrom(lr);
                        Assert.AreEqual(ref_pts[i].Classification, pt.Classification);
                    }
                }
            }
        }

        [TestMethod]
        public void BitFlags()
        {
            foreach (var info in TestData.BaseFiles)
            {
                using (lasStreamReader lr = new lasStreamReader(info.FileName))
                {
                    var ref_pts = TestData.BaseFilePoints[lr.PointFormat];
                    lasPointRecord pt = new lasPointRecord(lr);

                    for (int i = 0; i < ref_pts.Count; i++)
                    {
                        pt.ReadFrom(lr);
                        Assert.AreEqual(ref_pts[i].SyntheticFlag, pt.SyntheticFlag);
                        Assert.AreEqual(ref_pts[i].KeypointFlag, pt.KeypointFlag);
                        Assert.AreEqual(ref_pts[i].WithheldFlag, pt.WithheldFlag);
                        Assert.AreEqual(ref_pts[i].ScanDirectionFlag, pt.ScanDirectionFlag);
                        Assert.AreEqual(ref_pts[i].EdgeOfFlightLineFlag, pt.EdgeOfFlightLineFlag);

                        if (lr.PointFormat < 6)
                        {
                            var ex = Assert.Throws<InvalidOperationException>(() => pt.OverlapFlag());
                            Assert.IsTrue(ex.Message.Contains("Overlap flag not defined for point format"));
                        }
                        else Assert.AreEqual(ref_pts[i].OverlapFlag, pt.OverlapFlag());
                    }
                }
            }
        }

        [TestMethod]
        public void ScannerChannel()
        {
            foreach (var info in TestData.BaseFiles)
            {
                using (lasStreamReader lr = new lasStreamReader(info.FileName))
                {
                    var ref_pts = TestData.BaseFilePoints[lr.PointFormat];
                    lasPointRecord pt = new lasPointRecord(lr);

                    for (int i = 0; i < ref_pts.Count; i++)
                    {
                        pt.ReadFrom(lr);
                        if (lr.PointFormat < 6)
                        {
                            var ex = Assert.Throws<InvalidOperationException>(() => pt.ScannerChannel());
                            Assert.IsTrue(ex.Message.Contains("Scanner Channel field not defined for point format"));
                        }
                        else Assert.AreEqual(ref_pts[i].ScannerChannel, pt.ScannerChannel());
                    }
                }
            }
        }

        [TestMethod]
        public void UserData()
        {
            foreach (var info in TestData.BaseFiles)
            {
                using (lasStreamReader lr = new lasStreamReader(info.FileName))
                {
                    var ref_pts = TestData.BaseFilePoints[lr.PointFormat];
                    lasPointRecord pt = new lasPointRecord(lr);

                    for (int i = 0; i < ref_pts.Count; i++)
                    {
                        pt.ReadFrom(lr);
                        Assert.AreEqual(ref_pts[i].UserData, pt.UserData);
                    }
                }
            }
        }

        [TestMethod]
        public void ScanAngle()
        {
            foreach (var info in TestData.BaseFiles)
            {
                using (lasStreamReader lr = new lasStreamReader(info.FileName))
                {
                    var ref_pts = TestData.BaseFilePoints[lr.PointFormat];
                    lasPointRecord pt = new lasPointRecord(lr);

                    for (int i = 0; i < ref_pts.Count; i++)
                    {
                        pt.ReadFrom(lr);
                        double test_val = ref_pts[i].ScanAngle * 0.006;
                        if (lr.PointFormat < 6) Assert.AreEqual(Math.Round(test_val), pt.ScanAngle);
                        else Assert.AreEqual(test_val, pt.ScanAngle);
                    }
                }
            }
        }

        [TestMethod]
        public void SourceID()
        {
            foreach (var info in TestData.BaseFiles)
            {
                using (lasStreamReader lr = new lasStreamReader(info.FileName))
                {
                    var ref_pts = TestData.BaseFilePoints[lr.PointFormat];
                    lasPointRecord pt = new lasPointRecord(lr);

                    for (int i = 0; i < ref_pts.Count; i++)
                    {
                        pt.ReadFrom(lr);
                        Assert.AreEqual(ref_pts[i].SourceID, pt.SourceID);
                    }
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
                    var ref_pts = TestData.BaseFilePoints[lr.PointFormat];
                    lasPointRecord pt = new lasPointRecord(lr);

                    for (int i = 0; i < ref_pts.Count; i++)
                    {
                        pt.ReadFrom(lr);
                        if (!lr.HasTimestamps)
                        {
                            var ex = Assert.Throws<InvalidOperationException>(() => pt.Timestamp());
                            Assert.IsTrue(ex.Message.Contains("Timestamp not stored in point format"));
                        }
                        else Assert.AreEqual(ref_pts[i].Timestamp, pt.Timestamp());
                    }
                }
            }
        }
    }
}
