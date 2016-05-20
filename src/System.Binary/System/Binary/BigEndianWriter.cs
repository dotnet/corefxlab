// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace System.Binary.BigEndian
{
    /// <summary>
    /// Writes big-endian (network order) primitives.
    /// </summary>
    /// <remarks>
    /// Use these helpers when you need to read specific endinaness.
    /// </remarks>
    public static class BigEndianWriter
    {
        public static void WriteUInt32(this Span<byte> data, uint value)
        {
            uint bigEndianValue = (value >> 24) | (value << 24) | ((value >> 8) & 0x0000FF00) | ((value << 8) & 0x00FF0000);
            data.Write(bigEndianValue);
        }
    }
}