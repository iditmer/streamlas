using System;
using System.IO;
using System.Text;

namespace streamlas
{
    public class lasStreamWriter : IDisposable
    {
        internal BinaryWriter writer;
        private byte point_format;
        private byte version_minor;
        private UInt32 offset_to_points;

        private UInt64 count = 0;
        private UInt64[] return_counts = new UInt64[15];

        public lasStreamWriter(lasStreamReader reader, lasPointRecord point, string path)
        {
            point_format = point.format;
            version_minor = reader.VersionMinor;
            offset_to_points = lasConstants.HeaderSize[reader.VersionMinor - 1];

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

            writer.Write(Encoding.ASCII.GetBytes("streamlas - .NET LAS IO Library"));
            while (writer.BaseStream.Position < 90) writer.Write('\0');

            writer.Write((UInt16)DateTime.Now.DayOfYear);
            writer.Write((UInt16)DateTime.Now.Year);

            writer.Write(lasConstants.HeaderSize[reader.VersionMinor - 1]);
            writer.Write(offset_to_points);

            for (int i = 0; i < 4; i++) writer.Write((byte)0);
            writer.Write(point.format);
            writer.Write(lasConstants.PointSize[point.format]);

            while (writer.BaseStream.Position != offset_to_points) writer.Write((byte)0);
        }

        public void WritePoint(lasPointRecord point)
        {
            writer.Write(point.raw_data);
            count++;
            return_counts[point.ReturnNumber - 1]++;
        }

        public void Dispose() 
        {
            if (point_format < 6)
            {
                writer.BaseStream.Position = 107;
                writer.Write((UInt32)count);

                for (int i = 0; i < 5; i++) writer.Write((UInt32)return_counts[i]);
            }
            
            if (version_minor > 3)
            {
                writer.BaseStream.Position = 247;
                writer.Write(count);

                for (int i = 0; i < 15; i++) writer.Write(return_counts[i]);
            }

            writer.Dispose(); 
        }
    }
}
