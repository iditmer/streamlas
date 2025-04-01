using Microsoft.VisualStudio.TestTools.UnitTesting;
using streamlas;
using System;
using System.Collections.Generic;
using System.IO;
using tests;

namespace Modifying
{
    [TestClass]
    public class Export
    {
        [TestMethod, TestCategory("Integration")]
        public void ModifiedPoints()
        {
            foreach (var info in TestData.BaseFiles)
            {
                string out_file = info.FileName.Replace(".las", "_TEMP.las");
                var ref_pts = TestData.BaseFilePoints[info.PointFormat];
                List<PointInfo> modified_pts = new List<PointInfo>();                

                using (lasStreamReader lr = new lasStreamReader(info.FileName))
                using (lasPointRecord pt = new lasPointRecord(lr))
                using (lasStreamWriter lw = new lasStreamWriter(lr, pt, out_file))
                {
                    for (int i = 0; i < ref_pts.Count; i++)
                    {
                        pt.ReadFrom(lr);
                        modified_pts.Add(ref_pts[i].Copy());

                        pt.X += 150.0;
                        modified_pts[i].X += 150.0;
                        pt.Y -= 250.0;
                        modified_pts[i].Y -= 250.0;
                        pt.Z += 300.0;
                        modified_pts[i].Z += 300.0;
                        pt.Intensity = (UInt16)(((lr.PointFormat + 1) / 10) * 65000);
                        modified_pts[i].Intensity = (UInt16)(((lr.PointFormat + 1) / 10) * 65000);
                        
                        pt.KeypointFlag = !ref_pts[i].KeypointFlag;
                        modified_pts[i].KeypointFlag = !ref_pts[i].KeypointFlag;
                        pt.WithheldFlag = !ref_pts[i].WithheldFlag;
                        modified_pts[i].WithheldFlag = !ref_pts[i].WithheldFlag;
                        
                        if (lr.PointFormat < 6)
                        {
                            pt.Classification = (byte)((ref_pts[i].Classification + 1) % 32);
                            modified_pts[i].Classification = (byte)((ref_pts[i].Classification + 1) % 32);
                        }
                        else
                        {
                            pt.Classification = (byte)((ref_pts[i].Classification + 1) % 256);
                            modified_pts[i].Classification = (byte)((ref_pts[i].Classification + 1) % 256);
                            pt.OverlapFlag(!ref_pts[i].OverlapFlag);
                            modified_pts[i].OverlapFlag = !ref_pts[i].OverlapFlag;
                        }
                        
                        lw.WritePoint(pt);
                    }
                }

                using (lasStreamReader lr = new lasStreamReader(out_file))
                using (lasPointRecord pt = new lasPointRecord(lr))
                {
                    for (int i = 0; i < modified_pts.Count; i++)
                    {
                        pt.ReadFrom(lr);
                        Assert.AreEqual(modified_pts[i].X, pt.X, 1e-6);
                        Assert.AreEqual(modified_pts[i].Y, pt.Y, 1e-6);
                        Assert.AreEqual(modified_pts[i].Z, pt.Z, 1e-6);
                        Assert.AreEqual(modified_pts[i].Intensity, pt.Intensity);
                        Assert.AreEqual(modified_pts[i].Classification, pt.Classification);
                        Assert.AreEqual(modified_pts[i].KeypointFlag, pt.KeypointFlag);
                        Assert.AreEqual(modified_pts[i].WithheldFlag, pt.WithheldFlag);
                        if (lr.PointFormat > 5)
                        {
                            Assert.AreEqual(modified_pts[i].OverlapFlag, pt.OverlapFlag());
                        }
                    }
                }

                File.Delete(out_file);
            }
        }
    }
}
