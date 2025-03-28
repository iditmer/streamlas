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
            writer.Write(Encoding.ASCII.GetBytes("LASF"));            
            
            for (int i = 0; i < 20; i++) writer.Write((byte)0);
            writer.Write((byte)1);
            writer.Write(reader.VersionMinor);

            for (int i = 0; i < 68; i++) writer.Write((byte)0);
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
