using Microsoft.VisualStudio.TestTools.UnitTesting;
using streamlas;
using tests;

namespace Reading
{
    [TestClass]
    public class VariableLengthRecords
    {
        [TestMethod]
        public void NumberVariableLengthRecords()
        {
            foreach (var info in TestData.BaseFiles)
            {
                using (lasStreamReader lr = new lasStreamReader(info.FileName))
                {
                    Assert.AreEqual(info.VariableLengthRecords.Length, lr.VariableLengthRecords.Length);
                }
            }
        }
    }
}