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

        [TestMethod]
        public void UserID()
        {
            foreach (var info in TestData.BaseFiles)
            {
                using (lasStreamReader lr = new lasStreamReader(info.FileName))
                {
                    for (int i = 0; i < info.VariableLengthRecords.Length; i++)
                    {
                        Assert.AreEqual(info.VariableLengthRecords[i].UserID, lr.VariableLengthRecords[i].UserID);
                    }
                }
            }
        }

        [TestMethod]
        public void RecordID()
        {
            foreach (var info in TestData.BaseFiles)
            {
                using (lasStreamReader lr = new lasStreamReader(info.FileName))
                {
                    for (int i = 0; i < info.VariableLengthRecords.Length; i++)
                    {
                        Assert.AreEqual(info.VariableLengthRecords[i].RecordID, lr.VariableLengthRecords[i].RecordID);
                    }
                }
            }
        }
    }
}