using Microsoft.VisualStudio.TestTools.UnitTesting;
using streamlas;
using System;
using System.IO;
using tests;

namespace Modifying
{
    [TestClass]
    public class Points
    {
        private lasPointRecord[] point_per_format = new lasPointRecord[11];
        private PointInfo[] ref_points = new PointInfo[11];

        [TestInitialize]
        public void ReadPoints()
        {
            for (int pfmt = 0; pfmt < 11; pfmt++)
            {
                string las_file = Path.Combine(TestData.BasePath, string.Format("LAS_14_PF_{0:00}.las", pfmt));
                using (lasStreamReader lr = new lasStreamReader(las_file))
                {
                    point_per_format[pfmt] = new lasPointRecord(lr);
                    point_per_format[pfmt].ReadFrom(lr);
                }

                ref_points[pfmt] = TestData.BaseFilePoints[(byte)pfmt][0];
            }
        }

        [TestMethod]
        public void Classification()
        {
            for (int i = 0; i < 11; i++)
            {
                int num_classes = 32;
                if (i > 5) num_classes = 256;
                point_per_format[i].Classification = (byte)((point_per_format[i].Classification + 1) % num_classes);
                byte ref_class = (byte)((ref_points[i].Classification + 1) % num_classes);
                Assert.AreEqual(ref_class, point_per_format[i].Classification);
            }
        }

        [TestMethod]
        public void Intensity()
        {
            for (int i = 0; i < 11; i++)
            {
                UInt16 int_val = (UInt16)(UInt16.MaxValue * (i / 10.0));
                point_per_format[i].Intensity = int_val;
                Assert.AreEqual(int_val, point_per_format[i].Intensity);
            }
        }

        [TestMethod]
        public void UserData()
        {
            for (int i = 0; i < 11; i++)
            {
                byte ud = (byte)((i + 1) * 5);
                point_per_format[i].UserData = ud;
                Assert.AreEqual(ud, point_per_format[i].UserData);
            }
        }
    }
}
