using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using streamlas;
using tests;

namespace Writing
{
    [TestClass]
    public class Header
    {
        [TestMethod]
        public void FileSignature()
        {
            foreach (var file in TestData.BaseFiles)
            {
                string test_path = Utility.WritePath(file);
                using (BinaryReader r = new BinaryReader(File.OpenRead(test_path)))
                {
                    Assert.AreEqual("LASF", new string(r.ReadChars(4)));
                }
            }
        }

        [TestMethod]
        public void Version()
        {
            foreach (var file in TestData.BaseFiles)
            {
                string test_path = Utility.WritePath(file);
                using (BinaryReader r = new BinaryReader(File.OpenRead(test_path)))
                {
                    r.ReadBytes(24);
                    Assert.AreEqual((byte)1, r.ReadByte());
                    Assert.AreEqual(file.VersionMinor, r.ReadByte());
                }
            }
        }

        [TestMethod]
        public void PointFormat()
        {
            foreach (var file in TestData.BaseFiles)
            {
                string test_path = Utility.WritePath(file);
                using (BinaryReader r = new BinaryReader(File.OpenRead(test_path)))
                {
                    r.ReadBytes(104);
                    Assert.AreEqual(file.PointFormat, r.ReadByte());
                }
            }
        }
    }
}
