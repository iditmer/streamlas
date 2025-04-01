using System;
using System.Runtime.InteropServices;

namespace streamlas
{
    [StructLayout(LayoutKind.Explicit)]
    internal unsafe struct PointBase
    {
        [FieldOffset(0)]
        internal int X;

        [FieldOffset(4)]
        internal int Y;

        [FieldOffset(8)]
        internal int Z;

        [FieldOffset(12)]
        internal ushort Intensity;

        [FieldOffset(14)]
        internal byte BitGroupOne;

        [FieldOffset(15)]
        internal byte BitGroupTwo;
    }

    [StructLayout(LayoutKind.Explicit)]
    internal unsafe struct PointBlockLegacy
    {
        [FieldOffset(0)]
        internal sbyte ScanAngleRank;

        [FieldOffset(1)]
        internal byte UserData;

        [FieldOffset(2)]
        internal UInt16 PointSourceID;
    }

    [StructLayout(LayoutKind.Explicit)]
    internal unsafe struct PointBlockModern
    {
        [FieldOffset(0)]
        internal byte Classification;

        [FieldOffset(1)]
        internal byte UserData;

        [FieldOffset(2)]
        internal Int16 ScanAngle;

        [FieldOffset(4)]
        internal UInt16 PointSourceID;

        [FieldOffset(6)]
        internal double Timestamp;
    }
}
