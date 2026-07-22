using System;
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

        internal void Write(BinaryWriter w)
        {
            w.Write((byte)0);
            w.Write((byte)0);

            for (int i = 0; i < Math.Min(16, UserID.Length); i++) w.Write(Convert.ToByte(UserID[i]));
            if (UserID.Length < 16) for (int i = 0; i < 16 - UserID.Length; i++) w.Write((byte)0);

            w.Write(RecordID);
            w.Write((UInt16)Data.Length);

            for (int i = 0; i < Math.Min(32, Description.Length); i++) w.Write(Convert.ToByte(Description[i]));
            if (Description.Length < 32) for (int i = 0; i < 32 - Description.Length; i++) w.Write((byte)0);

            w.Write(Data);
        }
    }
}
