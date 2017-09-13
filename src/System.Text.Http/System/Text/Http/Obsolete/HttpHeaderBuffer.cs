// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Buffers.Text;

namespace System.Text.Http.SingleSegment
{
    public ref struct HttpHeaderBuffer
    {
        //TODO: Issue #390: Switch HttpHeaderBuffer to use Slices.Span.
        private Span<byte> _bytes;
        private readonly SymbolTable _symbolTable;

        public HttpHeaderBuffer(Span<byte> bytes, SymbolTable symbolTable)
        {
            _bytes = bytes;
            _symbolTable = symbolTable;
        }

        public void UpdateValue(string newValue)
        {
            if (newValue.Length > _bytes.Length)
            {
                throw new ArgumentException("newValue");
            }

            _symbolTable.TryEncode(newValue.AsReadOnlySpan(), _bytes, out int consumed, out int written);
            _bytes.SetFromRestOfSpanToEmpty(newValue.Length);
        }
    }

    public static class SpanExtensions
    {
        private const byte EmptyCharacter = 32;

        public static void SetFromRestOfSpanToEmpty(this Span<byte> span, int startingFrom)
        {
            for (var i = startingFrom; i < span.Length; i++)
            {
                span[i] = EmptyCharacter;
            }
        }

        internal static int SliceTo(this ReadOnlySpan<byte> buffer, char terminator, out ReadOnlySpan<byte> slice)
        {
            return buffer.SliceTo((byte)terminator, out slice);
        }

        internal static int SliceTo(this ReadOnlySpan<byte> buffer, int start, char terminator, out ReadOnlySpan<byte> slice)
        {
            return buffer.SliceTo(start, (byte)terminator, out slice);
        }

        internal static int SliceTo(this ReadOnlySpan<byte> buffer, byte terminator, out ReadOnlySpan<byte> slice)
        {
            return buffer.SliceTo(0, terminator, out slice);
        }

        internal static int SliceTo(this ReadOnlySpan<byte> buffer, int start, byte terminator, out ReadOnlySpan<byte> slice)
        {
            slice = buffer.Slice(start);
            var index = System.SpanExtensions.IndexOf(slice, terminator);
            if (index == -1) {
                return 0;
            }
            slice = slice.Slice(0, index);
            return index;
        }

        internal static int SliceTo(this ReadOnlySpan<byte> buffer, char terminatorFirst, char terminatorSecond, out ReadOnlySpan<byte> slice)
        {
            return buffer.SliceTo((byte)terminatorFirst, (byte)terminatorSecond, out slice);
        }

        internal static int SliceTo(this ReadOnlySpan<byte> buffer, int start, char terminatorFirst, char terminatorSecond, out ReadOnlySpan<byte> slice)
        {
            return buffer.SliceTo(start, (byte)terminatorFirst, (byte)terminatorSecond, out slice);
        }

        internal static int SliceTo(this ReadOnlySpan<byte> buffer, byte terminatorFirst, byte terminatorSecond, out ReadOnlySpan<byte> slice)
        {
            return buffer.SliceTo(0, terminatorFirst, terminatorSecond, out slice);
        }

        internal static int SliceTo(this ReadOnlySpan<byte> buffer, int start, byte terminatorFirst, byte terminatorSecond, out ReadOnlySpan<byte> slice)
        {
            int offset = 0;
            while (true)
            {
                slice = buffer.Slice(start + offset);
                var index = System.SpanExtensions.IndexOf(slice, terminatorFirst);
                if (index == -1 || index == slice.Length - 1)
                {
                    return 0;
                }
                if (slice[index + 1] == terminatorSecond)
                {
                    slice = slice.Slice(0, index + offset);
                    return index;
                }
                else
                {
                    offset += index;
                }
            }
        }
    }
}
