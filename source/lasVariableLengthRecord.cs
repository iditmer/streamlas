using System;
using System.IO;
using System.Text;

namespace streamlas
{
    public class lasVariableLengthRecord
    {
        public string UserID { get; private set; } = "";

        internal lasVariableLengthRecord(BinaryReader r)
        {
            r.ReadBytes(2); // reserved
            UserID = Encoding.UTF8.GetString(r.ReadBytes(16)).TrimEnd('\0');
            r.ReadBytes(2);
            UInt16 length_after_header = r.ReadUInt16();
            r.ReadBytes(32 + length_after_header);
        }
    }
}
