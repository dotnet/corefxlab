using System;

namespace LowAllocationServer
{
    public static class ByteSpanExtensions
    {
        internal static ByteSpan SliceTo(this ByteSpan buffer, char terminator, out int consumedBytes)
        {
            return buffer.SliceTo((byte) terminator, out consumedBytes);
        }

        internal static ByteSpan SliceTo(this ByteSpan buffer, int start, char terminator, out int consumedBytes)
        {
            return buffer.SliceTo(start, (byte)terminator, out consumedBytes);
        }

        internal static ByteSpan SliceTo(this ByteSpan buffer, byte terminator, out int consumedBytes)
        {
            return buffer.SliceTo(0, terminator, out consumedBytes);
        }

        internal static ByteSpan SliceTo(this ByteSpan buffer, int start, byte terminator, out int consumedBytes)
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

            return ByteSpan.Empty;
        }

        internal static ByteSpan SliceTo(this ByteSpan buffer, char terminatorFirst, char terminatorSecond, out int consumedBytes)
        {
            return buffer.SliceTo((byte) terminatorFirst, (byte) terminatorSecond, out consumedBytes);
        }

        internal static ByteSpan SliceTo(this ByteSpan buffer, int start, char terminatorFirst, char terminatorSecond, out int consumedBytes)
        {
            return buffer.SliceTo(start, (byte)terminatorFirst, (byte)terminatorSecond, out consumedBytes);
        }

        internal static ByteSpan SliceTo(this ByteSpan buffer, byte terminatorFirst, byte terminatorSecond, out int consumedBytes)
        {
            return buffer.SliceTo(0, terminatorFirst, terminatorSecond, out consumedBytes);
        }

        internal static ByteSpan SliceTo(this ByteSpan buffer, int start, byte terminatorFirst, byte terminatorSecond, out int consumedBytes)
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

            return ByteSpan.Empty;
        }
    }
}
