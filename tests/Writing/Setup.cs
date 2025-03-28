using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using streamlas;
using tests;

namespace Writing
{
    [TestClass]
    public class Setup
    {
        [AssemblyInitialize]
        public static void WriteBaseFiles(TestContext c)
        {
            if (!Directory.Exists(TestData.WritePath)) Directory.CreateDirectory(TestData.WritePath);
            foreach (var file in TestData.BaseFiles)
            {
                using (lasStreamReader lr = new lasStreamReader(file.FileName))
                using (lasPointRecord pt = new lasPointRecord(lr))                
                using (lasStreamWriter lw = new lasStreamWriter(lr, pt, Utility.WritePath(file)))
                {
                    for (ulong i = 0; i < lr.PointCount; i++)
                    {
                        pt.ReadFrom(lr);
                        lw.WritePoint(pt);
                    }
                }
            }
        }

        [AssemblyCleanup]
        public static void ClearWriteTests(TestContext c) 
        {
            Directory.Delete(TestData.WritePath, true);
        }
    }

    internal class Utility
    {
        internal static string WritePath(BaseFileInfo test_file)
        {
            return Path.Combine(TestData.WritePath, Path.GetFileName(test_file.FileName));
        }
    }
}
