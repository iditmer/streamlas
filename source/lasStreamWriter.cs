using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace streamlas
{
    internal delegate void lasPointMethod(lasPointRecord p);

    public class lasStreamWriter : IDisposable
    {        
        internal BinaryWriter writer;
        internal string file_name;
        
        private byte point_format;
        private int excess_point_bytes;
        private byte version_minor;
        private HashSet<UInt16> src_ids = new HashSet<ushort>();

        private UInt64 count = 0;
        private UInt64[] return_counts = new UInt64[15];
        private double[] min_coords = { double.MaxValue, double.MaxValue, double.MaxValue };
        private double[] max_coords = { double.MinValue, double.MinValue, double.MinValue };

        public string SystemIdentifier { get; set; } = "MODIFICATION";

        public lasStreamWriter(lasStreamReader reader, lasPointRecord point, string path)
        {
            point_format = point.format;
            version_minor = reader.VersionMinor;
            if (point_format < 6)
            {
                write_point_block = write_point_block_legacy;
                excess_point_bytes = point.raw_data.Length - 20;
                if (excess_point_bytes > 0) write_point_block = write_point_block_legacy_excess;
            }
            else
            {
                write_point_block = write_point_block_modern;
                excess_point_bytes = point.raw_data.Length - 30;
                if (excess_point_bytes > 0) write_point_block = write_point_block_modern_excess;
            }

            file_name = path;
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

            // write SYSTEM IDENTIFIER on disposal to allow changes
            while (writer.BaseStream.Position < 58) writer.Write('\0');

            writer.Write(Encoding.ASCII.GetBytes("streamlas"));
            while (writer.BaseStream.Position < 90) writer.Write('\0');

            writer.Write((UInt16)DateTime.Now.DayOfYear);
            writer.Write((UInt16)DateTime.Now.Year);

            writer.Write(lasConstants.HeaderSize[reader.VersionMinor - 1]);
            writer.Write((UInt32)lasConstants.HeaderSize[reader.VersionMinor - 1]);

            for (int i = 0; i < 4; i++) writer.Write((byte)0);
            writer.Write(point.format);
            writer.Write((UInt16)point.raw_data.Length);

            for (int i = 0; i < 24; i++) writer.Write((byte)0);
            for (int i = 0; i < 3; i++) writer.Write(reader.scale[i]);
            for (int i = 0; i < 3; i++) writer.Write(reader.offset[i]);

            while (writer.BaseStream.Position != lasConstants.HeaderSize[reader.VersionMinor - 1]) writer.Write((byte)0);
        }

        public void WritePoint(lasPointRecord point)
        {
            count++;
            return_counts[point.ReturnNumber - 1]++;
            src_ids.Add(point.SourceID);

            min_coords[0] = (min_coords[0] < point.X) ? min_coords[0] : point.X;
            min_coords[1] = (min_coords[1] < point.Y) ? min_coords[1] : point.Y;
            min_coords[2] = (min_coords[2] < point.Z) ? min_coords[2] : point.Z;

            max_coords[0] = (max_coords[0] > point.X) ? max_coords[0] : point.X;
            max_coords[1] = (max_coords[1] > point.Y) ? max_coords[1] : point.Y;
            max_coords[2] = (max_coords[2] > point.Z) ? max_coords[2] : point.Z;

            writer.Write(point.point_base.X);
            writer.Write(point.point_base.Y);
            writer.Write(point.point_base.Z);
            writer.Write(point.point_base.Intensity);
            writer.Write(point.point_base.BitGroupOne);
            writer.Write(point.point_base.BitGroupTwo);
            write_point_block(point);
        }

        lasPointMethod write_point_block;

        void write_point_block_legacy(lasPointRecord p)
        {
            writer.Write(p.point_block_legacy.ScanAngleRank);
            writer.Write(p.point_block_legacy.UserData);
            writer.Write(p.point_block_legacy.PointSourceID);
        }

        void write_point_block_legacy_excess(lasPointRecord p)
        {
            writer.Write(p.point_block_legacy.ScanAngleRank);
            writer.Write(p.point_block_legacy.UserData);
            writer.Write(p.point_block_legacy.PointSourceID);
            for (int i = 0; i < excess_point_bytes; i++) writer.Write(p.raw_data[20 + i]);
        }

        void write_point_block_modern(lasPointRecord p)
        {
            writer.Write(p.point_block_modern.Classification);
            writer.Write(p.point_block_modern.UserData);
            writer.Write(p.point_block_modern.ScanAngle);
            writer.Write(p.point_block_modern.PointSourceID);
            writer.Write(p.point_block_modern.Timestamp);
        }

        void write_point_block_modern_excess(lasPointRecord p)
        {
            writer.Write(p.point_block_modern.Classification);
            writer.Write(p.point_block_modern.UserData);
            writer.Write(p.point_block_modern.ScanAngle);
            writer.Write(p.point_block_modern.PointSourceID);
            writer.Write(p.point_block_modern.Timestamp);
            for (int i = 0; i < excess_point_bytes; i++) writer.Write(p.raw_data[30 + i]);
        }

        public void Dispose()
        {
            if (src_ids.Count == 1)
            {
                writer.BaseStream.Position = 4;
                writer.Write(src_ids.First());
            }

            writer.BaseStream.Position = 26;
            byte[] sys_id = Encoding.ASCII.GetBytes(SystemIdentifier);
            for (int i = 0; i < Math.Min(sys_id.Length, 32); i++) writer.Write(sys_id[i]);

            if (point_format < 6)
            {
                if (count > UInt32.MaxValue)
                {
                    writer.Dispose();
                    File.Delete(file_name);
                    throw new IOException("More points than UINT32_MAX written to file;" +
                        " unsupported in legacy mode using Point Format " + point_format);
                }

                writer.BaseStream.Position = 107;
                writer.Write((UInt32)count);
                for (int i = 0; i < 5; i++) writer.Write((UInt32)return_counts[i]);
            }

            if (count > 0)
            {
                writer.BaseStream.Position = 179;
                for (int i = 0; i < 3; i++)
                {
                    writer.Write(max_coords[i]);
                    writer.Write(min_coords[i]);
                }
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
