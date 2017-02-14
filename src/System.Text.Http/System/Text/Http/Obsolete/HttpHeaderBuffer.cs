// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace System.Text.Http.SingleSegment
{ 
    public struct HttpHeaderBuffer
    {
        //TODO: Issue #390: Switch HttpHeaderBuffer to use Slices.Span.
        private Span<byte> _bytes;
        private readonly EncodingData _encoding;

        public HttpHeaderBuffer(Span<byte> bytes, EncodingData encoding)
        {
            _bytes = bytes;
            _encoding = encoding;
        }

        public void UpdateValue(string newValue)
        {
            if (newValue.Length > _bytes.Length)
            {
                throw new ArgumentException("newValue");
            }

            int bytesWritten;
            _encoding.TextEncoder.TryEncode(newValue, _bytes, out bytesWritten);            

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

        internal static ReadOnlySpan<byte> SliceTo(this ReadOnlySpan<byte> buffer, char terminator, out int consumedBytes)
        {
            return buffer.SliceTo((byte)terminator, out consumedBytes);
        }

        internal static ReadOnlySpan<byte> SliceTo(this ReadOnlySpan<byte> buffer, int start, char terminator, out int consumedBytes)
        {
            return buffer.SliceTo(start, (byte)terminator, out consumedBytes);
        }

        internal static ReadOnlySpan<byte> SliceTo(this ReadOnlySpan<byte> buffer, byte terminator, out int consumedBytes)
        {
            return buffer.SliceTo(0, terminator, out consumedBytes);
        }

        internal static ReadOnlySpan<byte> SliceTo(this ReadOnlySpan<byte> buffer, int start, byte terminator, out int consumedBytes)
        {
            var slice = buffer.Slice(start);
            var index = System.SpanExtensions.IndexOf(slice, terminator);
            if (index == -1) {
                consumedBytes = 0;
                return Span<byte>.Empty;
            }
            consumedBytes = index;
            return slice.Slice(0, index);
        }

        internal static ReadOnlySpan<byte> SliceTo(this ReadOnlySpan<byte> buffer, char terminatorFirst, char terminatorSecond, out int consumedBytes)
        {
            return buffer.SliceTo((byte)terminatorFirst, (byte)terminatorSecond, out consumedBytes);
        }

        internal static ReadOnlySpan<byte> SliceTo(this ReadOnlySpan<byte> buffer, int start, char terminatorFirst, char terminatorSecond, out int consumedBytes)
        {
            return buffer.SliceTo(start, (byte)terminatorFirst, (byte)terminatorSecond, out consumedBytes);
        }

        internal static ReadOnlySpan<byte> SliceTo(this ReadOnlySpan<byte> buffer, byte terminatorFirst, byte terminatorSecond, out int consumedBytes)
        {
            return buffer.SliceTo(0, terminatorFirst, terminatorSecond, out consumedBytes);
        }

        internal static ReadOnlySpan<byte> SliceTo(this ReadOnlySpan<byte> buffer, int start, byte terminatorFirst, byte terminatorSecond, out int consumedBytes)
        {
            int offset = 0;
            while (true)
            {
                var slice = buffer.Slice(start + offset);
                var index = System.SpanExtensions.IndexOf(slice, terminatorFirst);
                if (index == -1 || index == slice.Length - 1)
                {
                    consumedBytes = 0;
                    return Span<byte>.Empty;
                }
                if (slice[index + 1] == terminatorSecond)
                {
                    consumedBytes = index;
                    return slice.Slice(0, index + offset);
                }
                else
                {
                    offset += index;
                }
            }
        }
    }
}
