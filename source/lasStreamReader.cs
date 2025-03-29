using System;
using System.IO;
using System.Text;

namespace streamlas
{
    internal enum lasStreamResult { OK, BadFileSig, InconsistentCounts, ImproperLegacy }

    public class lasStreamReader : IDisposable
    {
        internal BinaryReader reader;
        private string path;        

        public UInt16 FileSourceID { get; private set; }
        
        internal UInt16 global_encoding;
        public bool HasTimestamps { get { return PointFormat != 0 && PointFormat != 2; } }
        public bool AdjustedGPSTime { get { return (global_encoding & 1) == 1; } }
        public bool SyntheticReturns { get { return (global_encoding & 8) == 8; } }
        
        public UInt32 GUID1 { get; private set; }
        public UInt16 GUID2 { get; private set; }
        public UInt16 GUID3 { get; private set; }
        public byte[] GUID4 { get; private set; } = new byte[8];

        private byte version_major;
        public byte VersionMinor { get; private set; }

        public string SystemIdentifier { get; private set; } = "";
        public string GeneratingSoftware { get; private set; } = "";
        
        public UInt16 FileCreationDayOfYear { get;private set; }
        public UInt16 FileCreationYear { get; private set; }

        private UInt16 header_size;
        private UInt32 offset_to_points;
                
        public byte PointFormat { get; private set; }
        internal UInt16 point_size;
        public UInt64 PointCount { get; private set; }
        public UInt64[] NumberPointsByReturn { get; private set; } = new UInt64[15];

        internal double[] scale = new double[3];
        internal double[] offset = new double[3];

        public double[] MinimumXYZ { get; private set; } = new double[3];
        public double[] MaximumXYZ { get; private set; } = new double[3];

        public lasStreamReader(string las_path)
        {
            reader = new BinaryReader(File.OpenRead(las_path));
            path = las_path;

            lasStreamResult result = ReadHeader();
            ValidateHeader(result);
        }

        private lasStreamResult ReadHeader()
        {
            if (new string(reader.ReadChars(4)) != "LASF") return lasStreamResult.BadFileSig;

            FileSourceID = reader.ReadUInt16();
            global_encoding = reader.ReadUInt16();

            GUID1 = reader.ReadUInt32();
            GUID2 = reader.ReadUInt16();
            GUID3 = reader.ReadUInt16();
            for (int i = 0; i < 8; i++) GUID4[i] = reader.ReadByte();
            
            version_major = reader.ReadByte();
            VersionMinor = reader.ReadByte();

            SystemIdentifier = Encoding.ASCII.GetString(reader.ReadBytes(32)).Trim('\0');
            GeneratingSoftware = Encoding.ASCII.GetString(reader.ReadBytes(32)).Trim('\0');
            FileCreationDayOfYear = reader.ReadUInt16();
            FileCreationYear = reader.ReadUInt16();

            header_size = reader.ReadUInt16();
            offset_to_points = reader.ReadUInt32();
            reader.ReadBytes(4);

            PointFormat = reader.ReadByte();
            point_size = reader.ReadUInt16();

            UInt32 temp_count = reader.ReadUInt32();
            if (PointFormat > 5 && temp_count > 0) return lasStreamResult.ImproperLegacy;
            UInt32[] temp_counts_by_return = new UInt32[5];
            bool legacy_return_counts = false;
            for (int i = 0; i < 5; i++)
            {
                temp_counts_by_return[i] = reader.ReadUInt32();
                if (PointFormat > 5 && temp_counts_by_return[i] > 0) return lasStreamResult.ImproperLegacy;
                if (temp_counts_by_return[i] > 0) legacy_return_counts = true;
            }

            for (int i = 0; i < 3; i++) scale[i] = reader.ReadDouble();
            for (int i = 0; i < 3; i++) offset[i] = reader.ReadDouble();
            for (int i = 0; i < 3; i++)
            {
                MaximumXYZ[i] = reader.ReadDouble();
                MinimumXYZ[i] = reader.ReadDouble();
            }

            if (VersionMinor > 3)
            {
                reader.ReadBytes(20);
                PointCount = reader.ReadUInt64();
                for (int i = 0; i < 15; i++) NumberPointsByReturn[i] = reader.ReadUInt64();

                if (temp_count > 0 && temp_count != PointCount) return lasStreamResult.InconsistentCounts;
                for (int i = 0; i < 5; i++)
                {
                    if (legacy_return_counts && temp_counts_by_return[i] != NumberPointsByReturn[i])
                    {
                        return lasStreamResult.InconsistentCounts;
                    }
                }
            }
            else
            {
                PointCount = temp_count;
                for (int i = 0; i < 5; i++) NumberPointsByReturn[i] = temp_counts_by_return[i];
            }

            reader.BaseStream.Position = offset_to_points;
            return lasStreamResult.OK;
        }

        private void ValidateHeader(lasStreamResult read_result)
        {
            if (read_result == lasStreamResult.BadFileSig)
            {
                IOException ex = new IOException("Input file " + Path.GetFileName(path)
                    + " is not a properly formatted LAS file.");
                ex.Data["FileName"] = Path.GetFileName(path);
                DisposeWithException(ex);
            }

            if (version_major == 0 || version_major > 1 || VersionMinor == 0 || VersionMinor > 4)
            {
                IOException ex = new IOException("LAS v" + version_major + "." + VersionMinor +
                    " is unsupported or not yet defined.");
                ex.Data["FileName"] = Path.GetFileName(path);
                DisposeWithException(ex);
            }

            if (PointFormat > lasConstants.MaxPointFormat[VersionMinor - 1])
            {
                IOException ex = new IOException("Point format " + PointFormat +
                    " is not supported in LAS v" + version_major + "." + VersionMinor);
                ex.Data["FileName"] = Path.GetFileName(path);
                DisposeWithException(ex);
            }

            if (header_size != lasConstants.HeaderSize[VersionMinor - 1])
            {
                IOException ex = new IOException("Reported header size incorrect for LAS v" + version_major +
                    "." + VersionMinor);
                ex.Data["FileName"] = Path.GetFileName(path);
                DisposeWithException(ex);
            }

            if (offset_to_points < lasConstants.HeaderSize[VersionMinor - 1])
            {
                IOException ex = new IOException("Reported offset to points shorter than header size for LAS v" +
                    version_major + "." + VersionMinor);
                ex.Data["FileName"] = Path.GetFileName(path);
                DisposeWithException(ex);
            }

            if (point_size < lasConstants.PointSize[PointFormat])
            {
                IOException ex = new IOException("Reported point record size smaller than minimum required for Point Format " + PointFormat);
                ex.Data["FileName"] = Path.GetFileName(path);
                DisposeWithException(ex);
            }

            if (read_result == lasStreamResult.InconsistentCounts)
            {
                IOException ex = new IOException("Inconsistency between Point Count and Legacy Point Count Fields.");
                ex.Data["FileName"] = Path.GetFileName(path);
                DisposeWithException(ex);
            }

            if (read_result == lasStreamResult.ImproperLegacy)
            {
                IOException ex = new IOException("Point Format > 5 but legacy point count fields have non-zero values.");
                ex.Data["FileName"] = Path.GetFileName(path);
                DisposeWithException(ex);
            }

            if (PointCount * point_size != (UInt64)(reader.BaseStream.Length - offset_to_points))
            {
                IOException ex = new IOException(Path.GetFileName(path) + " length is inconsistent with point size and count.");
                ex.Data["FileName"] = Path.GetFileName(path);
                DisposeWithException(ex);
            }
        }

        public void Dispose() { reader.Dispose(); }
        private void DisposeWithException(Exception ex)
        {
            Dispose();
            throw ex;
        }
    }
}