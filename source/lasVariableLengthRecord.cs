﻿using System;
using System.IO;
using System.Text;

namespace streamlas
{
    public class lasVariableLengthRecord
    {
        public string UserID { get; private set; }
        public UInt16 RecordID { get; private set; }
        public string Description { get; private set; }
        public byte[] Data { get; private set; }

        internal lasVariableLengthRecord(BinaryReader r)
        {
            r.ReadBytes(2); // reserved
            UserID = Encoding.UTF8.GetString(r.ReadBytes(16)).TrimEnd('\0');
            RecordID = r.ReadUInt16();
            UInt16 length_after_header = r.ReadUInt16();
            Description = Encoding.UTF8.GetString(r.ReadBytes(32)).TrimEnd('\0');
            Data = r.ReadBytes(length_after_header);
        }
    }
}
