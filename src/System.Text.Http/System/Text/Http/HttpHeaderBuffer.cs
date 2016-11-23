using System.Text;

namespace System.Text.Http
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
            newValue.TryEncode(_bytes, out bytesWritten, _encoding.TextEncoding);            

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
            var index = start;
            var count = 0;
            while (index < buffer.Length)
            {
                if (buffer[index] == terminator)
                {
                    consumedBytes = count + 1;
                    return buffer.Slice(start, count);
                }
                count++;
                index++;
            }
            consumedBytes = 0;

            return Span<byte>.Empty;
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
            var index = start;
            var count = 0;
            while (index < buffer.Length)
            {
                if (buffer[index] == terminatorFirst && buffer.Length > index + 1 && buffer[index + 1] == terminatorSecond)
                {
                    consumedBytes = count + 2;
                    return buffer.Slice(start, count);
                }
                count++;
                index++;
            }

            consumedBytes = 0;

            return Span<byte>.Empty;
        }
    }
}
