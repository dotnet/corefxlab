// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Sequences;

namespace System.Buffers
{
    // TODO: the TryReadUntill methods are very inneficient. We need to fix that.
    public static partial class BufferReaderExtensions
    {
        public static bool TryReadUntill<TSequence>(ref this BufferReader<TSequence> reader, out ReadOnlyBuffer bytes, byte delimiter)
            where TSequence : ISequence<ReadOnlyMemory<byte>>
        {
            var copy = reader;
            var start = reader.Position;
            while (!reader.End) {
                Position end = reader.Position;
                if(reader.Take() == delimiter)
                {
                    bytes = new ReadOnlyBuffer(start, end);
                    return true;
                }
            }
            reader = copy;
            bytes = default;
            return false;
        }

        public static bool TryReadUntill<TSequence>(ref this BufferReader<TSequence> reader, out ReadOnlyBuffer bytes, ReadOnlySpan<byte> delimiter)
            where TSequence : ISequence<ReadOnlyMemory<byte>>
        {
            if (delimiter.Length == 0)
            {
                bytes = ReadOnlyBuffer.Empty;
                return true;
            }

            int matched = 0;
            var copy = reader;
            var start = reader.Position;
            var end = reader.Position;
            while (!reader.End)
            {
                if (reader.Take() == delimiter[matched]) {
                    matched++;
                }
                else
                {
                    end = reader.Position;
                    matched = 0;
                }
                if(matched >= delimiter.Length)
                {
                    bytes = new ReadOnlyBuffer(start, end);
                    return true;
                }
            }
            reader = copy;
            bytes = default;
            return false;
        }
    }
}
