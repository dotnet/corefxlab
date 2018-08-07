// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Buffers.Binary;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace System.Buffers.Reader
{
    public ref partial struct BufferReader
    {
        /// <summary>
        /// Try to read the given type out of the buffer if possible.
        /// </summary>
        /// <remarks>
        /// The read is unaligned.
        /// </remarks>
        /// <returns>
        /// True if successful. <paramref name="value"/> will be default if failed.
        /// </returns>
        public unsafe bool TryRead<T>(out T value) where T : unmanaged
        {
            ReadOnlySpan<byte> span = UnreadSegment;
            if (span.Length < sizeof(T))
                return TryReadSlow(out value);

            value = MemoryMarshal.Read<T>(span);
            Advance(sizeof(T));
            return true;
        }

        private unsafe bool TryReadSlow<T>(out T value) where T : unmanaged
        {
            Debug.Assert(UnreadSegment.Length < sizeof(T));

            // Not enough data in the current segment, try to peek for the data we need.
            byte* buffer = stackalloc byte[sizeof(T)];
            Span<byte> tempSpan = new Span<byte>(buffer, sizeof(T));

            if (Peek(tempSpan).Length < sizeof(T))
            {
                value = default;
                return false;
            }

            value = MemoryMarshal.Read<T>(tempSpan);
            Advance(sizeof(T));
            return true;
        }

        public unsafe bool TryReadInt32LittleEndian(out int value)
        {
            if (BitConverter.IsLittleEndian)
            {
                return TryRead(out value);
            }

            return TryReadReverseEndianness(out value);
        }

        public unsafe bool TryReadInt32BigEndian(out int value)
        {
            if (!BitConverter.IsLittleEndian)
            {
                return TryRead(out value);
            }

            return TryReadReverseEndianness(out value);
        }

        private unsafe bool TryReadReverseEndianness(out int value)
        {
            if (TryRead(out value))
            {
                value = BinaryPrimitives.ReverseEndianness(value);
                return true;
            }

            return false;
        }
    }
}
