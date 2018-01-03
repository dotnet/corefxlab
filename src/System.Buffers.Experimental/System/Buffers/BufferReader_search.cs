// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Sequences;

namespace System.Buffers
{
    public static partial class BufferReaderExtensions
    {
        public static bool TryReadUntill<TSequence>(ref this BufferReader<TSequence> reader, out ReadOnlyBuffer bytes, byte delimiter)
            where TSequence : ISequence<ReadOnlyMemory<byte>>
        {
            var position = reader.PositionOf(delimiter);
            if (position == null)
            {
                bytes = default;
                return false;
            }
            bytes = new ReadOnlyBuffer(reader.Position, position.Value);
            reader.SkipTo(position.Value);
            reader.Skip(1);
            return true;
        }

        public static bool TryReadUntill<TSequence>(ref this BufferReader<TSequence> reader, out ReadOnlyBuffer bytes, ReadOnlySpan<byte> delimiter)
            where TSequence : ISequence<ReadOnlyMemory<byte>>
        {
            var position = reader.PositionOf(delimiter);
            if(position == null)
            {
                bytes = default;
                return false;
            }
            bytes = new ReadOnlyBuffer(reader.Position, position.Value);
            reader.SkipTo(position.Value);
            reader.Skip(delimiter.Length);
            return true;
        }
    }
}
