// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Buffers.Text;
using System.Collections.Sequences;

namespace System.Buffers
{
    public static partial class BufferReaderExtensions 
    {
        public static bool TryParse<TSequence>(ref BufferReader<TSequence> reader, out bool value) 
            where TSequence : ISequence<ReadOnlyMemory<byte>>
        {
            var unread = reader.UnreadSegment;
            if (Utf8Parser.TryParse(unread, out value, out int consumed))
            {
                if (unread.Length > consumed)
                {
                    reader.Advance(consumed);
                    return true;
                }
            }

            Span<byte> tempSpan = stackalloc byte[5];
            var copied = BufferReader.Peek(reader, tempSpan);
            if (Utf8Parser.TryParse(tempSpan.Slice(0, copied), out value, out consumed))
            {
                reader.Advance(consumed);
                return true;
            }

            return false;
        }

        public static bool TryParse<TSequence>(ref BufferReader<TSequence> reader, out int value)
            where TSequence : ISequence<ReadOnlyMemory<byte>>
        {
            var unread = reader.UnreadSegment;
            if (Utf8Parser.TryParse(unread, out value, out int consumed))
            {
                if (unread.Length > consumed)
                {
                    reader.Advance(consumed);
                    return true;
                }
            }

            Span<byte> tempSpan = stackalloc byte[15];
            var copied = BufferReader.Peek(reader, tempSpan);
            if (Utf8Parser.TryParse(tempSpan.Slice(0, copied), out value, out consumed))
            {
                reader.Advance(consumed);
                return true;
            }

            return false;
        }

        public static bool TryParse<TSequence>(ref BufferReader<TSequence> reader, out ulong value)
            where TSequence : ISequence<ReadOnlyMemory<byte>>
        {
            var unread = reader.UnreadSegment;
            if (Utf8Parser.TryParse(unread, out value, out int consumed))
            {
                if (unread.Length > consumed)
                {
                    reader.Advance(consumed);
                    return true;
                }
            }

            Span<byte> tempSpan = stackalloc byte[30];
            var copied = BufferReader.Peek(reader, tempSpan);
            if (Utf8Parser.TryParse(tempSpan.Slice(0, copied), out value, out consumed))
            {
                reader.Advance(consumed);
                return true;
            }

            return false;
        }
    }
}
