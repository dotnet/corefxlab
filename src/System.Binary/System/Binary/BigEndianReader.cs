// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace System.Binary.BigEndian
{
    /// <summary>
    /// Reads big-endian (network order) bytes as primitives.
    /// </summary>
    /// <remarks>
    /// For native formats, SpanExtensions.Read<T> should be used.
    /// Use these helpers when you need to read specific endinaness.
    /// </remarks>
    public static class BigEndianReader
    {
        public static ushort ReadUInt16(this ReadOnlySpan<byte> data)
        {
            ushort result = data[0];
            result <<= 8;
            result |= data[1];
            return result;
        }

        public static ushort ReadUInt16(this Span<byte> data)
        {
            ushort result = data[0];
            result <<= 8;
            result |= data[1];
            return result;
        }

        public static short ReadInt16(this ReadOnlySpan<byte> data)
        {
            short result = data[0];
            result <<= 8;
            result |= (short)data[1];
            return result;
        }

        public static short ReadInt16(this Span<byte> data)
        {
            short result = data[0];
            result <<= 8;
            result |= (short)data[1];
            return result;
        }

        public static uint ReadUInt32(this ReadOnlySpan<byte> data)
        {
            uint result = data[0];
            result <<= 8;
            result |= data[1];
            result <<= 8;
            result |= data[2];
            result <<= 8;
            result |= data[3];
            return result;
        }

        public static uint ReadUInt32(this Span<byte> data)
        {
            uint result = data[0];
            result <<= 8;
            result |= data[1];
            result <<= 8;
            result |= data[2];
            result <<= 8;
            result |= data[3];
            return result;
        }

        public static int ReadInt32(this ReadOnlySpan<byte> data)
        {
            int result = data[0];
            result <<= 8;
            result |= data[1];
            result <<= 8;
            result |= data[2];
            result <<= 8;
            result |= data[3];
            return result;
        }

        public static int ReadInt32(this Span<byte> data)
        {
            int result = data[0];
            result <<= 8;
            result |= data[1];
            result <<= 8;
            result |= data[2];
            result <<= 8;
            result |= data[3];
            return result;
        }

        public static ulong ReadUInt64(this ReadOnlySpan<byte> data)
        {
            ulong result = data[0];
            result <<= 8;
            result |= data[1];
            result <<= 8;
            result |= data[2];
            result <<= 8;
            result |= data[3];
            result <<= 8;
            result |= data[4];
            result <<= 8;
            result |= data[5];
            result <<= 8;
            result |= data[6];
            result <<= 8;
            result |= data[7];
            return result;
        }

        public static ulong ReadUInt64(this Span<byte> data)
        {
            ulong result = data[0];
            result <<= 8;
            result |= data[1];
            result <<= 8;
            result |= data[2];
            result <<= 8;
            result |= data[3];
            result <<= 8;
            result |= data[4];
            result <<= 8;
            result |= data[5];
            result <<= 8;
            result |= data[6];
            result <<= 8;
            result |= data[7];
            return result;
        }

        public static long ReadInt64(this ReadOnlySpan<byte> data)
        {
            long result = data[0];
            result <<= 8;
            result |= data[1];
            result <<= 8;
            result |= data[2];
            result <<= 8;
            result |= data[3];
            result <<= 8;
            result |= data[4];
            result <<= 8;
            result |= data[5];
            result <<= 8;
            result |= data[6];
            result <<= 8;
            result |= data[7];
            return result;
        }

        public static long ReadInt64(this Span<byte> data)
        {
            long result = data[0];
            result <<= 8;
            result |= data[1];
            result <<= 8;
            result |= data[2];
            result <<= 8;
            result |= data[3];
            result <<= 8;
            result |= data[4];
            result <<= 8;
            result |= data[5];
            result <<= 8;
            result |= data[6];
            result <<= 8;
            result |= data[7];
            return result;
        }
    }
}