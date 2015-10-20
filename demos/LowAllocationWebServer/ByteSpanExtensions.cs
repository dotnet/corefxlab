using System;

namespace LowAllocationServer
{
    public static class ByteSpanExtensions
    {
        internal static ByteSpan SliceTo(this ByteSpan buffer, char terminator, out int consumedBytes)
        {
            return buffer.SliceTo((byte) terminator, out consumedBytes);
        }

        internal static ByteSpan SliceTo(this ByteSpan buffer, byte terminator, out int consumedBytes)
        {
            var index = 0;
            while (index < buffer.Length)
            {
                if (buffer[index] == terminator)
                {
                    consumedBytes = index + 1;
                    return buffer.Slice(0, index);
                }
                index++;
            }
            consumedBytes = 0;

            return ByteSpan.Empty;
        }

        internal static ByteSpan SliceTo(this ByteSpan buffer, char terminatorFirst, char terminatorSecond, out int consumedBytes)
        {
            return buffer.SliceTo((byte) terminatorFirst, (byte) terminatorSecond, out consumedBytes);
        }

        internal static ByteSpan SliceTo(this ByteSpan buffer, byte terminatorFirst, byte terminatorSecond, out int consumedBytes)
        {
            int index = 0;
            while (index < buffer.Length)
            {
                if (buffer[index] == terminatorFirst && buffer.Length > index + 1 && buffer[index + 1] == terminatorSecond)
                {
                    consumedBytes = index + 2;
                    return buffer.Slice(0, index);
                }
                index++;
            }

            consumedBytes = 0;

            return ByteSpan.Empty;
        }
    }
}
