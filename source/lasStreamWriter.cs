using System;
using System.IO;
using System.Text;

namespace streamlas
{
    public class lasStreamWriter : IDisposable
    {
        internal BinaryWriter writer;
        private UInt64 count = 0;
        private byte point_format;
        private byte version_minor;

        public lasStreamWriter(lasStreamReader reader, lasPointRecord point, string path)
        {
            point_format = point.format;
            version_minor = reader.VersionMinor;

            writer = new BinaryWriter(File.Create(path));
            WriteHeader(reader, point);
        }

        private void WriteHeader(lasStreamReader reader, lasPointRecord point)
        {
            writer.Write(Encoding.ASCII.GetBytes("LASF"));

            for (int i = 0; i < 2; i++) writer.Write((byte)0);

            writer.Write(reader.global_encoding);

            writer.Write(reader.GUID1);
            writer.Write(reader.GUID2);
            writer.Write(reader.GUID3);
            writer.Write(reader.GUID4);

            writer.Write((byte)1);
            writer.Write(reader.VersionMinor);

            writer.Write(Encoding.ASCII.GetBytes(reader.SystemIdentifier));
            while (writer.BaseStream.Position < 58) writer.Write('\0');

            for (int i = 0; i < 32; i++) writer.Write((byte)0);
            writer.Write((UInt16)DateTime.Now.DayOfYear);
            writer.Write((UInt16)DateTime.Now.Year);

            writer.Write(lasConstants.HeaderSize[reader.VersionMinor - 1]);
            writer.Write((UInt32)lasConstants.HeaderSize[reader.VersionMinor - 1]);

            for (int i = 0; i < 4; i++) writer.Write((byte)0);
            writer.Write(point.format);
            writer.Write(lasConstants.PointSize[point.format]);

            while (writer.BaseStream.Position < lasConstants.HeaderSize[reader.VersionMinor - 1]) writer.Write((byte)0);
        }

        public void WritePoint(lasPointRecord point)
        {
            writer.Write(point.raw_data);
            count++;
        }

        public void Dispose() 
        {
            if (point_format < 6)
            {
                writer.BaseStream.Position = 107;
                writer.Write((uint)count);
            }
            
            if (version_minor > 3)
            {
                writer.BaseStream.Position = 247;
                writer.Write(count);
            }

            writer.Dispose(); 
        }
    }
}
