// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Buffers.Text;

namespace System.Buffers.Reader
{
    public static partial class BufferReaderExtensions 
    {
        public static bool TryParse(ref BufferReader reader, out bool value) 
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
            var copied = BufferReaderExtensions.Peek(reader, tempSpan);
            if (Utf8Parser.TryParse(tempSpan.Slice(0, copied), out value, out consumed))
            {
                reader.Advance(consumed);
                return true;
            }

            return false;
        }

        public static bool TryParse(ref BufferReader reader, out int value)
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
            var copied = BufferReaderExtensions.Peek(reader, tempSpan);
            if (Utf8Parser.TryParse(tempSpan.Slice(0, copied), out value, out consumed))
            {
                reader.Advance(consumed);
                return true;
            }

            return false;
        }

        public static bool TryParse(ref BufferReader reader, out ulong value)
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
            var copied = BufferReaderExtensions.Peek(reader, tempSpan);
            if (Utf8Parser.TryParse(tempSpan.Slice(0, copied), out value, out consumed))
            {
                reader.Advance(consumed);
                return true;
            }

            return false;
        }
    }
}
