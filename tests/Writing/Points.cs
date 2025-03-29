using Microsoft.VisualStudio.TestTools.UnitTesting;
using streamlas;
using tests;

namespace Writing
{
    [TestClass]
    public class Points
    {
        [TestMethod]
        public void Coordinates()
        {
            foreach (var info in TestData.BaseFiles)
            {
                string in_path = TestData.WriteTestPath(info);
                using (lasStreamReader lr = new lasStreamReader(in_path))
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
    }
}
