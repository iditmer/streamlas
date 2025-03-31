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
            }
        }

        [TestMethod]
        public void Coordinates()
        {
            for (int i = 0; i < point_per_format.Length; i++)
            {
                double x = point_per_format[i].X + 1500.0;
                double y = point_per_format[i].Y + 2750.0;
                double z = point_per_format[i].Z + 310.0;

                point_per_format[i].X = x;
                point_per_format[i].Y = y;
                point_per_format[i].Z = z;

                Assert.AreEqual(x, point_per_format[i].X, 1e-6);
                Assert.AreEqual(y, point_per_format[i].Y, 1e-6);
                Assert.AreEqual(z, point_per_format[i].Z, 1e-6);
            }
        }

        [TestMethod]
        public void Classification()
        {
            for (int i = 0; i < point_per_format.Length; i++)
            {
                int num_classes = 32;
                if (i > 5) num_classes = 256;

                byte class_val = (byte)(num_classes * (i + 1) / 10.0);
                point_per_format[i].Classification = class_val;
                Assert.AreEqual(class_val, point_per_format[i].Classification);
            }
        }

        [TestMethod]
        public void BitFlags()
        {
            for (int i = 0; i < point_per_format.Length; i++)
            {
                bool kp_flag = !point_per_format[i].KeypointFlag;
                bool wh_flag = !point_per_format[i].WithheldFlag;

                point_per_format[i].KeypointFlag = kp_flag;
                point_per_format[i].WithheldFlag = wh_flag;

                Assert.AreEqual(kp_flag, point_per_format[i].KeypointFlag);
                Assert.AreEqual(wh_flag, point_per_format[i].WithheldFlag);

                if (i < 6)
                {
                    var ex = Assert.Throws<InvalidOperationException>(() => point_per_format[i].OverlapFlag(true));
                    Assert.IsTrue(ex.Message.Contains("Overlap flag not defined for point format"));
                }

                if (i > 5)
                {
                    bool over_flag = !point_per_format[i].OverlapFlag();
                    point_per_format[i].OverlapFlag(over_flag);
                    Assert.AreEqual(over_flag, point_per_format[i].OverlapFlag());
                }
            }
        }

        [TestMethod]
        public void Intensity()
        {
            for (int i = 0; i < point_per_format.Length; i++)
            {
                UInt16 int_val = (UInt16)(UInt16.MaxValue * (i / 10.0));
                point_per_format[i].Intensity = int_val;
                Assert.AreEqual(int_val, point_per_format[i].Intensity);
            }
        }

        [TestMethod]
        public void UserData()
        {
            for (int i = 0; i < point_per_format.Length; i++)
            {
                byte ud = (byte)((i + 1) * 5);
                point_per_format[i].UserData = ud;
                Assert.AreEqual(ud, point_per_format[i].UserData);
            }
        }

        [TestMethod]
        public void PointSourceID()
        {
            for (int i = 0; i < point_per_format.Length; i++)
            {
                UInt16 src_id = (UInt16)((i + 1) * 100);
                point_per_format[i].SourceID = src_id;
                Assert.AreEqual(src_id, point_per_format[i].SourceID);
            }
        }
    }
}
