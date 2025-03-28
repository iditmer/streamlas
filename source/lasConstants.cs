using System;

namespace streamlas
{
    internal static class lasConstants
    {
        internal static UInt16[] HeaderSize = { 227, 227, 235, 375 };
        internal static byte[] MaxPointFormat = { 1, 3, 5, 10 };
        internal static UInt16[] PointSize = { 20, 28, 26, 34, 57, 63, 30, 36, 38, 59, 67 };
    }
}