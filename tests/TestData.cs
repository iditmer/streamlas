using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace tests
{
    public class ErrorFileInfo
    {
        public string FileName { get; set; } = string.Empty;
        public string ErrorType { get; set; } = string.Empty;
    }

    public class BaseFileInfo
    {
        public string FileName { get; set; } = "";
        public UInt16 SourceID { get; set; }
        public bool HasTimestamps { get; set; }
        public bool AdjustedGPSTime { get; set; }
        public bool SyntheticReturns { get; set; }
        public UInt32 GUID1 { get; set; }
        public UInt16 GUID2 { get; set; }
        public UInt16 GUID3 { get; set; }
        public int[] GUID4 { get; set; } = new int[8];
        public string GeneratingSoftware { get; set; } = "";
        public string SystemIdentifier { get; set; } = "";
        public UInt16 FileCreationDay { get; set; }        
        public UInt16 FileCreationYear { get; set; }
        public byte VersionMinor { get; set; }
        public byte PointFormat { get; set; }
        public UInt32 PointCount { get; set; }
        public double[] MinCoords { get; set; } = new double[3];
        public double[] MaxCoords { get; set; } = new double[3];
        public UInt64[] NumberPointsByReturn { get; set; } = new UInt64[15];
    }

    public class PointInfo
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
        public UInt16 Intensity { get; set; }
        public byte Classification { get; set; }
        public byte Return { get; set; }
        public byte NumberReturns { get; set; }
        public bool SyntheticFlag { get; set; }
        public bool KeypointFlag { get; set; }
        public bool WithheldFlag { get; set; }
        public bool OverlapFlag { get; set; }
        public bool ScanDirectionFlag { get; set; }
        public bool EdgeOfFlightLineFlag { get; set; }
        public byte ScannerChannel { get; set; }
        public UInt16 SourceID { get; set; }
        public byte UserData { get; set; }
        public Int16 ScanAngle { get; set; }
        public double Timestamp { get; set; }
    }

    internal static class TestData
    {
        internal static readonly List<ErrorFileInfo> ErrorFiles;
        internal static readonly List<BaseFileInfo> BaseFiles;
        internal static readonly Dictionary<byte, List<PointInfo>> BaseFilePoints;
        internal static readonly string WritePath;

        internal static List<ErrorFileInfo> GetErrorFileInfo(string data_dir, string json_file)
        {
            string in_data = File.ReadAllText(json_file);
            List<ErrorFileInfo> files = JsonSerializer.Deserialize<List<ErrorFileInfo>>(in_data);
            foreach (ErrorFileInfo file in files)
            {
                file.FileName = data_dir + file.FileName;
            }
            return files;
        }

        internal static List<BaseFileInfo> GetBaseFileInfo(string data_dir, string json_file)
        {
            string in_data = File.ReadAllText(json_file);
            List<BaseFileInfo> files = JsonSerializer.Deserialize<List<BaseFileInfo>>(in_data);
            foreach (BaseFileInfo file in files)
            {
                file.FileName = data_dir + file.FileName;
            }
            return files;
        }

        internal static List<PointInfo> GetPointInfo(string json_file)
        {
            string in_data = File.ReadAllText(json_file);
            return JsonSerializer.Deserialize<List<PointInfo>>(in_data);
        }

        static TestData()
        {
            string data_dir = "../../../Data/ErrorFiles/";
            ErrorFiles = GetErrorFileInfo(data_dir, data_dir + "ErrorFiles.json");

            data_dir = "../../../Data/BaseFiles/";
            BaseFiles = GetBaseFileInfo(data_dir, data_dir + "BaseFiles.json");

            BaseFilePoints = new Dictionary<byte, List<PointInfo>>();
            for (byte pfmt = 0; pfmt < 11; pfmt++)
            {
                string point_file = data_dir + string.Format("PointFormat_{0:00}.json", pfmt);
                BaseFilePoints.Add(pfmt, GetPointInfo(point_file));
            }

            WritePath = "../../../Data/Writing/";
        }

        internal static string WriteTestPath(BaseFileInfo test_file)
        {
            return Path.Combine(WritePath, Path.GetFileName(test_file.FileName));
        }
    }
}
