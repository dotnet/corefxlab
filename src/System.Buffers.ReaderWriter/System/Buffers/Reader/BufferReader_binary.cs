// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Buffers.Binary;

namespace System.Buffers.Reader
{
    public static partial class BufferReaderExtensions
    {
        public static bool TryRead(ref BufferReader reader, out int value, bool littleEndian = false)
        {
            var unread = reader.UnreadSegment;
            if (littleEndian)
            {
                if (BinaryPrimitives.TryReadInt32LittleEndian(unread, out value))
                {
                    reader.Advance(sizeof(int));
                    return true;
                }
            }
            else if (BinaryPrimitives.TryReadInt32BigEndian(unread, out value))
            {
                reader.Advance(sizeof(int));
                return true;
            }

            Span<byte> tempSpan = stackalloc byte[4];
            var copied = BufferReaderExtensions.Peek(reader, tempSpan);
            if (copied < 4)
            {
                value = default;
                return false;
            }

            if (littleEndian)
            {
                value = BinaryPrimitives.ReadInt32LittleEndian(tempSpan);
            }
            else
            {
                value = BinaryPrimitives.ReadInt32BigEndian(tempSpan);
            }
            reader.Advance(sizeof(int));
            return true;
        }
    }

    public static partial class BufferReaderExtensions
    {
        public static int Peek(BufferReader reader, Span<byte> destination)
            => BufferReader.Peek(reader, destination);
    }
}
