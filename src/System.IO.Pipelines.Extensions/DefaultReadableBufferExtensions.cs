using System.Buffers;
using System.Collections.Sequences;
using System.Numerics;
using System.Runtime.CompilerServices;

using static System.Buffers.Binary.BinaryPrimitives;

namespace System.IO.Pipelines
{
    public static class DefaultReadableBufferExtensions
    {
        private static readonly int VectorWidth = Vector<byte>.Count;

        /// <summary>
        /// Searches for 2 sequential bytes in the <see cref="ReadOnlyBuffer"/> and returns a sliced <see cref="ReadOnlyBuffer"/> that
        /// contains all data up to and excluding the first byte, and a <see cref="Position"/> that points to the second byte.
        /// </summary>
        /// <param name="b1">The first byte to search for</param>
        /// <param name="b2">The second byte to search for</param>
        /// <param name="slice">A <see cref="ReadOnlyBuffer"/> slice that contains all data up to and excluding the first byte.</param>
        /// <param name="cursor">A <see cref="Position"/> that points to the second byte</param>
        /// <returns>True if the byte sequence was found, false if not found</returns>
        public static unsafe bool TrySliceTo(this ReadOnlyBuffer<byte> buffer, byte b1, byte b2, out ReadOnlyBuffer<byte> slice, out Position cursor)
        {
            // use address of ushort rather than stackalloc as the inliner won't inline functions with stackalloc
            ushort twoBytes;
            byte* byteArray = (byte*)&twoBytes;
            byteArray[0] = b1;
            byteArray[1] = b2;
            return buffer.TrySliceTo(new Span<byte>(byteArray, 2), out slice, out cursor);
        }

        /// <summary>
        /// Searches for a span of bytes in the <see cref="ReadOnlyBuffer"/> and returns a sliced <see cref="ReadOnlyBuffer"/> that
        /// contains all data up to and excluding the first byte of the span, and a <see cref="Position"/> that points to the last byte of the span.
        /// </summary>
        /// <param name="span">The <see cref="Span{Byte}"/> byte to search for</param>
        /// <param name="slice">A <see cref="ReadOnlyBuffer"/> that matches all data up to and excluding the first byte</param>
        /// <param name="cursor">A <see cref="Position"/> that points to the second byte</param>
        /// <returns>True if the byte sequence was found, false if not found</returns>
        public static bool TrySliceTo(this ReadOnlyBuffer<byte> buffer, Span<byte> span, out ReadOnlyBuffer<byte> slice, out Position cursor)
        {
            var result = false;
            var subBuffer = buffer;
            do
            {
                // Find the first byte
                if (!subBuffer.TrySliceTo(span[0], out slice, out cursor))
                {
                    break;
                }

                // Move the buffer to where you fonud the first byte then search for the next byte
                subBuffer = buffer.Slice(cursor);

                if (subBuffer.StartsWith(span))
                {
                    slice = buffer.Slice(buffer.Start, cursor);
                    result = true;
                    break;
                }

                // REVIEW: We need to check the performance of Slice in a loop like this
                // Not a match so skip(1) 
                subBuffer = subBuffer.Slice(1);
            } while (!subBuffer.IsEmpty);

            return result;
        }

        /// <summary>
        /// Searches for a byte in the <see cref="ReadOnlyBuffer"/> and returns a sliced <see cref="ReadOnlyBuffer"/> that
        /// contains all data up to and excluding the byte, and a <see cref="Position"/> that points to the byte.
        /// </summary>
        /// <param name="b1">The first byte to search for</param>
        /// <param name="slice">A <see cref="ReadOnlyBuffer"/> slice that contains all data up to and excluding the first byte.</param>
        /// <param name="cursor">A <see cref="Position"/> that points to the second byte</param>
        /// <returns>True if the byte sequence was found, false if not found</returns>
        public static bool TrySliceTo(this ReadOnlyBuffer<byte> buffer, byte b1, out ReadOnlyBuffer<byte> slice, out Position cursor)
        {
            if (buffer.IsEmpty)
            {
                slice = default;
                cursor = default;
                return false;
            }

            var byte0Vector = GetVector(b1);

            var seek = 0;

            foreach (var memory in buffer)
            {
                var currentSpan = memory.Span;
                var found = false;

                if (Vector.IsHardwareAccelerated)
                {
                    while (currentSpan.Length >= VectorWidth)
                    {
                        var data = ReadMachineEndian<Vector<byte>>(currentSpan);
                        var byte0Equals = Vector.Equals(data, byte0Vector);

                        if (byte0Equals.Equals(Vector<byte>.Zero))
                        {
                            currentSpan = currentSpan.Slice(VectorWidth);
                            seek += VectorWidth;
                        }
                        else
                        {
                            var index = FindFirstEqualByte(ref byte0Equals);
                            seek += index;
                            found = true;
                            break;
                        }
                    }
                }

                if (!found)
                {
                    // Slow search
                    for (int i = 0; i < currentSpan.Length; i++)
                    {
                        if (currentSpan[i] == b1)
                        {
                            found = true;
                            break;
                        }
                        seek++;
                    }
                }

                if (found)
                {
                    cursor = buffer.Seek(buffer.Start, seek);
                    slice = buffer.Slice(buffer.Start, cursor);
                    return true;
                }
            }

            slice = default;
            cursor = default;
            return false;
        }

        /// <summary>
        /// Find first byte
        /// </summary>
        /// <param  name="byteEquals"></param >
        /// <returns>The first index of the result vector</returns>
        /// <exception cref="InvalidOperationException">byteEquals = 0</exception>
        internal static int FindFirstEqualByte(ref Vector<byte> byteEquals)
        {
            if (!BitConverter.IsLittleEndian) return FindFirstEqualByteSlow(ref byteEquals);

            // Quasi-tree search
            var vector64 = Vector.AsVectorInt64(byteEquals);
            for (var i = 0; i < Vector<long>.Count; i++)
            {
                var longValue = vector64[i];
                if (longValue == 0) continue;

                return (i << 3) +
                       ((longValue & 0x00000000ffffffff) > 0
                           ? (longValue & 0x000000000000ffff) > 0
                               ? (longValue & 0x00000000000000ff) > 0 ? 0 : 1
                               : (longValue & 0x0000000000ff0000) > 0 ? 2 : 3
                           : (longValue & 0x0000ffff00000000) > 0
                               ? (longValue & 0x000000ff00000000) > 0 ? 4 : 5
                               : (longValue & 0x00ff000000000000) > 0 ? 6 : 7);
            }
            throw new InvalidOperationException();
        }

        // Internal for testing
        internal static int FindFirstEqualByteSlow(ref Vector<byte> byteEquals)
        {
            // Quasi-tree search
            var vector64 = Vector.AsVectorInt64(byteEquals);
            for (var i = 0; i < Vector<long>.Count; i++)
            {
                var longValue = vector64[i];
                if (longValue == 0) continue;

                var shift = i << 1;
                var offset = shift << 2;
                var vector32 = Vector.AsVectorInt32(byteEquals);
                if (vector32[shift] != 0)
                {
                    if (byteEquals[offset] != 0) return offset;
                    if (byteEquals[offset + 1] != 0) return offset + 1;
                    if (byteEquals[offset + 2] != 0) return offset + 2;
                    return offset + 3;
                }
                if (byteEquals[offset + 4] != 0) return offset + 4;
                if (byteEquals[offset + 5] != 0) return offset + 5;
                if (byteEquals[offset + 6] != 0) return offset + 6;
                return offset + 7;
            }
            throw new InvalidOperationException();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector<byte> GetVector(byte vectorByte)
        {
            // Vector<byte> .ctor is a bit fussy to get working; however this always seems to work
            // https://github.com/dotnet/coreclr/issues/7459#issuecomment-253965670
            return Vector.AsVectorByte(new Vector<ulong>(vectorByte * 0x0101010101010101ul));
        }


        /// <summary>
        /// Checks to see if the <see cref="ReadOnlyBuffer"/> starts with the specified <see cref="Span{Byte}"/>.
        /// </summary>
        /// <param name="value">The <see cref="Span{Byte}"/> to compare to</param>
        /// <returns>True if the bytes StartsWith, false if not</returns>
        public static bool StartsWith(this ReadOnlyBuffer<byte> buffer, Span<byte> value)
        {
            if (buffer.Length < value.Length)
            {
                // just nope
                return false;
            }

            return buffer.Slice(0, value.Length).EqualsTo(value);
        }

        /// <summary>
        /// Checks to see if the <see cref="ReadOnlyBuffer"/> is Equal to the specified <see cref="Span{Byte}"/>.
        /// </summary>
        /// <param name="value">The <see cref="Span{Byte}"/> to compare to</param>
        /// <returns>True if the bytes are equal, false if not</returns>
        public static bool EqualsTo(this ReadOnlyBuffer<byte> buffer, Span<byte> value)
        {
            if (value.Length != buffer.Length)
            {
                return false;
            }

            if (buffer.IsSingleSegment)
            {
                return buffer.First.Span.SequenceEqual(value);
            }

            foreach (var memory in buffer)
            {
                var compare = value.Slice(0, memory.Length);
                if (!memory.Span.SequenceEqual(compare))
                {
                    return false;
                }

                value = value.Slice(memory.Length);
            }
            return true;
        }
    }
}
