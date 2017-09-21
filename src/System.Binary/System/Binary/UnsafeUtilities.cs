// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace System.Runtime
{
    /// <summary>
    /// A collection of unsafe helper methods that we cannot implement in C#.
    /// NOTE: these can be used for VeryBadThings(tm), so tread with care...
    /// </summary>
    internal static class UnsafeUtilities
    {
        public static short ReverseEndianness(short value)
        {
            return (short)((value & 0x00FF) << 8 | (value & 0xFF00) >> 8);
        }

        public static int ReverseEndianness(int value)
        {
            return (value & 0x000000FF) << 24 |
                (value & 0x0000FF00) << 8 |
                (value & 0x00FF0000) >> 8 |
                (int)(((uint)value & 0xFF000000) >> 24);
        }

        public static long ReverseEndianness(long value)
        {
            return (value & 0x00000000000000FFL) << 56 |
                (value & 0x000000000000FF00L) << 40 |
                (value & 0x0000000000FF0000L) << 24 |
                (value & 0x00000000FF000000L) << 8 |
                (value & 0x000000FF00000000L) >> 8 |
                (value & 0x0000FF0000000000L) >> 24 |
                (value & 0x00FF000000000000L) >> 40 |
                (long)(((ulong)value & 0xFF00000000000000L) >> 56);
        }

        public static ushort ReverseEndianness(ushort value)
        {
            return (ushort)((value & 0x00FFU) << 8 | (value & 0xFF00U) >> 8);
        }

        public static uint ReverseEndianness(uint value)
        {
            return (value & 0x000000FFU) << 24 |
                (value & 0x0000FF00U) << 8 |
                (value & 0x00FF0000U) >> 8 |
                (value & 0xFF000000U) >> 24;
        }

        public static ulong ReverseEndianness(ulong value)
        {
            return (value & 0x00000000000000FFUL) << 56 |
                (value & 0x000000000000FF00UL) << 40 |
                (value & 0x0000000000FF0000UL) << 24 |
                (value & 0x00000000FF000000UL) << 8 |
                (value & 0x000000FF00000000UL) >> 8 |
                (value & 0x0000FF0000000000UL) >> 24 |
                (value & 0x00FF000000000000UL) >> 40 |
                (value & 0xFF00000000000000UL) >> 56;
        }
    }
}
