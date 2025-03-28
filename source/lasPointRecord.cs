using System;

namespace streamlas
{
    internal delegate bool get_bool();
    internal delegate byte get_byte();
    internal delegate UInt16 get_UInt16();
    internal delegate double get_double();
    internal delegate void void_method();

    public class lasPointRecord
    {
        private byte format;
        private double[] scale;
        private double[] offset;
        private byte[] raw_data;

        private PointBase point_base;
        private PointBlockLegacy point_block_legacy;
        private PointBlockModern point_block_modern;
        void_method assign_point_block;

        public double X { get { return point_base.X * scale[0] + offset[0]; } }
        public double Y { get { return point_base.Y * scale[1] + offset[1]; } }
        public double Z { get { return point_base.Z * scale[2] + offset[2]; } }
        public ushort Intensity { get { return point_base.Intensity; } }

        public byte ReturnNumber { get { return return_number(); } }
        get_byte return_number;
        byte return_number_legacy() { return (byte)(point_base.BitGroupOne & 7); }
        byte return_number_modern() { return (byte)(point_base.BitGroupOne & 15); }

        public byte NumberReturns { get { return number_returns(); } }
        get_byte number_returns;
        byte number_returns_legacy() { return (byte)((point_base.BitGroupOne & 56) >> 3); }
        byte number_returns_modern() { return (byte)((point_base.BitGroupOne & 240) >> 4); }

        public byte Classification { get { return classification(); } }
        get_byte classification;
        byte classification_legacy() { return (byte)(point_base.BitGroupTwo & 31); }
        byte classification_modern() { return point_block_modern.Classification; }

        public bool SyntheticFlag { get { return synthetic(); } }
        get_bool synthetic;
        bool synthetic_legacy() { return (point_base.BitGroupTwo & 32) == 32; }
        bool synthetic_modern() { return (point_base.BitGroupTwo & 1) == 1; }

        public bool KeypointFlag { get { return keypoint(); } }
        get_bool keypoint;
        bool keypoint_legacy() { return (point_base.BitGroupTwo & 64) == 64; }
        bool keypoint_modern() { return (point_base.BitGroupTwo & 2) == 2; }

        public bool WithheldFlag { get { return withheld(); } }
        get_bool withheld;
        bool withheld_legacy() { return (point_base.BitGroupTwo & 128) == 128; }
        bool withheld_modern() { return (point_base.BitGroupTwo & 4) == 4; }

        public bool OverlapFlag() {  return overlap(); } 
        get_bool overlap;
        bool overlap_legacy() { throw new InvalidOperationException("Overlap flag not defined for point format " + format); }
        bool overlap_modern() { return (point_base.BitGroupTwo & 8) == 8; }

        public bool ScanDirectionFlag { get { return scan_direction(); } }
        get_bool scan_direction;
        bool scan_direction_legacy() { return (point_base.BitGroupOne & 64) == 64; }
        bool scan_direction_modern() { return (point_base.BitGroupTwo & 64) == 64; }

        public bool EdgeOfFlightLineFlag { get { return edge_line(); } }
        get_bool edge_line;
        bool edge_line_legacy() { return (point_base.BitGroupOne & 128) == 128; }
        bool edge_line_modern() { return (point_base.BitGroupTwo & 128) == 128; }

        public byte ScannerChannel() { return scanner_channel(); }
        get_byte scanner_channel;
        byte scanner_channel_legacy() { throw new InvalidOperationException("Scanner Channel field not defined for point format " + format); }
        byte scanner_channel_modern() { return (byte)((point_base.BitGroupTwo & 48) >> 4); }

        public byte UserData { get { return user_data(); } }
        get_byte user_data;
        byte user_data_legacy() {  return point_block_legacy.UserData; }    
        byte user_data_modern() { return point_block_modern.UserData; }

        public double ScanAngle { get { return scan_angle(); } }
        get_double scan_angle;
        double scan_angle_legacy() { return point_block_legacy.ScanAngleRank; }
        double scan_angle_modern() { return point_block_modern.ScanAngle * 0.006; }

        public UInt16 SourceID { get { return source_id(); } }
        get_UInt16 source_id;
        UInt16 source_id_legacy() { return point_block_legacy.PointSourceID; }
        UInt16 source_id_modern() { return point_block_modern.PointSourceID; }
        
        public double Timestamp() { return get_time(); }
        private int time_index;
        private double time;
        get_double get_time;
        double no_timestamp() { throw new InvalidOperationException("Timestamp not stored in point format " + format); }
        double timestamp() { return time; }

        public lasPointRecord(lasStreamReader input)
        {
            format = input.PointFormat;
            scale = input.scale;
            offset = input.offset;
            raw_data = new byte[input.point_size];

            if (input.PointFormat < 6)
            {
                return_number = return_number_legacy;
                number_returns = number_returns_legacy;
                classification = classification_legacy;
                synthetic = synthetic_legacy;
                keypoint = keypoint_legacy;
                withheld = withheld_legacy;
                overlap = overlap_legacy;
                scan_direction = scan_direction_legacy;
                edge_line = edge_line_legacy;
                scanner_channel = scanner_channel_legacy;
                scan_angle = scan_angle_legacy;
                user_data = user_data_legacy;
                source_id = source_id_legacy;

                if (input.PointFormat == 0 || input.PointFormat == 2)
                {
                    assign_point_block = assign_point_block_legacy;
                    get_time = no_timestamp;
                }
                else
                {
                    assign_point_block = assign_point_block_legacy_with_time;
                    get_time = timestamp;
                }
                time_index = 20;
            }
            else
            {
                return_number = return_number_modern;
                number_returns = number_returns_modern;
                classification = classification_modern;
                synthetic = synthetic_modern;
                keypoint = keypoint_modern;
                withheld = withheld_modern;
                overlap = overlap_modern;
                scan_direction = scan_direction_modern;
                edge_line = edge_line_modern;
                scanner_channel = scanner_channel_modern;
                scan_angle = scan_angle_modern;
                user_data = user_data_modern;
                source_id = source_id_modern;
                assign_point_block = assign_point_block_modern;                
                get_time = timestamp;
                time_index = 22;
            }
        }

        public unsafe void ReadFrom(lasStreamReader input)
        {
            input.reader.Read(raw_data, 0, raw_data.Length);
            fixed (byte* b = &raw_data[0]) point_base = *(PointBase*)b;
            assign_point_block();
        }

        private unsafe void assign_point_block_legacy()
        {
            fixed (byte* b = &raw_data[16]) point_block_legacy = *(PointBlockLegacy*)b;
        }

        private unsafe void assign_point_block_legacy_with_time()
        {
            fixed (byte* b = &raw_data[16]) point_block_legacy = *(PointBlockLegacy*)b;
            fixed (byte* b = &raw_data[time_index]) time = *(double*)b;
        }

        private unsafe void assign_point_block_modern()
        {
            fixed (byte* b = &raw_data[16]) point_block_modern = *(PointBlockModern*)b;
            fixed (byte* b = &raw_data[time_index]) time = *(double*)b;
        }
    }
}
