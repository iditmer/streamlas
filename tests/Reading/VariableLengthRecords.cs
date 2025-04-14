using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text;
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

        [TestMethod]
        public void Description()
        {
            foreach (var info in TestData.BaseFiles)
            {
                using (lasStreamReader lr = new lasStreamReader(info.FileName))
                {
                    for (int i = 0; i < info.VariableLengthRecords.Length; i++)
                    {
                        Assert.AreEqual(info.VariableLengthRecords[i].Description, lr.VariableLengthRecords[i].Description);
                    }
                }
            }
        }

        [TestMethod]
        public void RecordLength()
        {
            foreach (var info in TestData.BaseFiles)
            {
                using (lasStreamReader lr = new lasStreamReader(info.FileName))
                {
                    for (int i = 0; i < info.VariableLengthRecords.Length; i++)
                    {
                        Assert.AreEqual(info.VariableLengthRecords[i].RecordLength, lr.VariableLengthRecords[i].Data.Length);
                    }
                }
            }
        }

        [TestMethod]
        public void RecordContent()
        {
            foreach (var info in TestData.BaseFiles)
            {
                using (lasStreamReader lr = new lasStreamReader(info.FileName))
                {
                    for (int i = 0; i < info.VariableLengthRecords.Length; i++)
                    {
                        Assert.AreEqual(info.VariableLengthRecords[i].DataString, Encoding.UTF8.GetString(lr.VariableLengthRecords[i].Data));
                    }
                }
            }
        }
    }
}