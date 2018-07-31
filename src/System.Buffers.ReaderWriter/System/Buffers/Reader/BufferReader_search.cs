// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.


namespace System.Buffers.Reader
{
    // TODO: the TryReadUntill methods are very inneficient. We need to fix that.
    public static partial class BufferReaderExtensions
    {
        public static bool TryReadUntill(ref BufferReader reader, out ReadOnlySequence<byte> bytes, byte delimiter)
        {
            BufferReader copy = reader;
            while (!reader.End)
            {
                if (reader.Peek() == delimiter)
                {
                    bytes = reader.Sequence.Slice(copy.Position, reader.Position);
                    reader.Advance(1);
                    return true;
                }
                reader.Advance(1);
            }
            reader = copy;
            bytes = default;
            return false;
        }

        public static bool TryReadUntill(ref BufferReader reader, out ReadOnlySequence<byte> bytes, ReadOnlySpan<byte> delimiter)
        {
            if (delimiter.Length == 0)
            {
                bytes = default;
                return true;
            }

            int matched = 0;
            var copy = reader;
            var start = reader.Position;
            var end = reader.Position;
            while (!reader.End)
            {
                if (reader.Read() == delimiter[matched])
                {
                    matched++;
                }
                else
                {
                    end = reader.Position;
                    matched = 0;
                }
                if (matched >= delimiter.Length)
                {
                    bytes = reader.Sequence.Slice(start, end);
                    return true;
                }
            }
            reader = copy;
            bytes = default;
            return false;
        }
    }
}
