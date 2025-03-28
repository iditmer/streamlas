﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using streamlas;
using tests;
using System;

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

        [TestMethod]
        public void HeaderSizeAndOffset()
        {
            foreach (var file in TestData.BaseFiles)
            {
                UInt16[] HeaderSize = { 227, 227, 235, 375 };

                string test_path = Utility.WritePath(file);
                using (BinaryReader r = new BinaryReader(File.OpenRead(test_path)))
                {
                    r.ReadBytes(94);
                    Assert.AreEqual(HeaderSize[file.VersionMinor - 1], r.ReadUInt16());
                    Assert.AreEqual(HeaderSize[file.VersionMinor - 1], r.ReadUInt32());
                }
            }
        }
    }
}
