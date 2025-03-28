using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using streamlas;
using tests;

namespace Reading
{
    [TestClass]
    public class HeaderExceptions
    {
        [TestMethod]
        public void WrongFileSignature()
        {
            foreach (var file in TestData.ErrorFiles)
            {
                if (file.ErrorType == "WrongFileSignature")
                {
                    IOException ex = Assert.ThrowsException<IOException>(() => new lasStreamReader(file.FileName));
                    Assert.IsTrue(ex.Message.Contains("not a properly formatted LAS file"));
                    Assert.AreEqual(ex.Data["FileName"], Path.GetFileName(file.FileName));
                }
            }
        }


        [TestMethod]
        public void UnsupportedVersion()
        {
            foreach (var file in TestData.ErrorFiles)
            {
                if (file.ErrorType == "UnsupportedVersion")
                {
                    IOException ex = Assert.ThrowsException<IOException>(() => new lasStreamReader(file.FileName));
                    Assert.IsTrue(ex.Message.Contains("is unsupported or not yet defined."));
                    Assert.AreEqual(ex.Data["FileName"], Path.GetFileName(file.FileName));
                }
            }
        }

        [TestMethod]
        public void UnsupportedPointFormat()
        {
            foreach (var file in TestData.ErrorFiles)
            {
                if (file.ErrorType == "UnsupportedPointFormat")
                {
                    IOException ex = Assert.ThrowsException<IOException>(() => new lasStreamReader(file.FileName));
                    Assert.IsTrue(ex.Message.Contains("not supported in LAS v"));
                    Assert.AreEqual(ex.Data["FileName"], Path.GetFileName(file.FileName));
                }
            }
        }

        [TestMethod]
        public void IncorrectPointSize()
        {
            foreach (var file in TestData.ErrorFiles)
            {
                if (file.ErrorType == "IncorrectPointSize")
                {
                    IOException ex = Assert.ThrowsException<IOException>(() => new lasStreamReader(file.FileName));
                    Assert.IsTrue(ex.Message.Contains("Reported point record size smaller than minimum required for Point Format"));
                    Assert.AreEqual(ex.Data["FileName"], Path.GetFileName(file.FileName));
                }
            }
        }

        [TestMethod]
        public void IncorrectHeaderSize()
        {
            foreach (var file in TestData.ErrorFiles)
            {
                if (file.ErrorType == "IncorrectHeaderSize")
                {
                    IOException ex = Assert.Throws<IOException>(() => new lasStreamReader(file.FileName));
                    Assert.IsTrue(ex.Message.Contains("Reported header size incorrect for LAS v"));
                    Assert.AreEqual(ex.Data["FileName"], Path.GetFileName(file.FileName));
                }
            }
        }

        [TestMethod]
        public void InconsistentPointCounts()
        {
            foreach (var file in TestData.ErrorFiles)
            {
                if (file.ErrorType == "InconsistentPointCount")
                {
                    IOException ex = Assert.Throws<IOException>(() => new lasStreamReader(file.FileName));
                    Assert.IsTrue(ex.Message.Contains("Inconsistency between Point Count and Legacy Point Count Fields."));
                    Assert.AreEqual(ex.Data["FileName"], Path.GetFileName(file.FileName));
                }
            }
        }

        [TestMethod]
        public void ImproperLegacyCounts()
        {
            foreach (var file in TestData.ErrorFiles)
            {
                if (file.ErrorType == "ImproperLegacyUse")
                {
                    IOException ex = Assert.Throws<IOException>(() => new lasStreamReader(file.FileName));
                    Assert.IsTrue(ex.Message.Contains("Point Format > 5 but legacy point count fields have non-zero values."));
                    Assert.AreEqual(ex.Data["FileName"], Path.GetFileName(file.FileName));
                }
            }
        }

        [TestMethod]
        public void BadOffsetToPoints()
        {
            foreach (var file in TestData.ErrorFiles)
            {
                if (file.ErrorType == "BadOffsetToPoints")
                {
                    IOException ex = Assert.Throws<IOException>(() => new lasStreamReader(file.FileName));
                    Assert.IsTrue(ex.Message.Contains("Reported offset to points shorter than header size for LAS v"));
                    Assert.AreEqual(ex.Data["FileName"], Path.GetFileName(file.FileName));
                }
            }
        }

        [TestMethod]
        public void BadFileLength()
        {
            foreach (var file in TestData.ErrorFiles)
            {
                if (file.ErrorType == "BadFileLength")
                {
                    IOException ex = Assert.Throws<IOException>(() => new lasStreamReader(file.FileName));
                    Assert.IsTrue(ex.Message.Contains("length is inconsistent with point size and count."));
                    Assert.AreEqual(ex.Data["FileName"], Path.GetFileName(file.FileName));
                }
            }
        }
    }
}
