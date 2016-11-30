using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace System.IO.Pipelines
{
    using Diagnostics;
    using Numerics;
    using Runtime.CompilerServices;

    public static class SeekExtensions
    {
        private const ulong _xorPowerOfTwoToHighByte = (0x07ul |
                                                        0x06ul << 8 |
                                                        0x05ul << 16 |
                                                        0x04ul << 24 |
                                                        0x03ul << 32 |
                                                        0x02ul << 40 |
                                                        0x01ul << 48) + 1;

        private static readonly int _vectorSpan = Vector<byte>.Count;

        public static unsafe int Seek(ReadCursor begin, ReadCursor end, out ReadCursor result, byte byte0, out int bytesScanned, int limit = int.MaxValue)
        {
            bytesScanned = 0;
            result = default(ReadCursor);

            var block = begin.Segment;
            if (block == null || limit <= 0)
            {
                return -1;
            }

            var index = begin.Index;
            var wasLastBlock = block.Next == null;
            var following = block.End - index;
            var byte0Vector = GetVector(byte0);

            while (true)
            {
                while (following == 0)
                {
                    if (bytesScanned >= limit || wasLastBlock)
                    {
                        return -1;
                    }

                    block = block.Next;
                    index = block.Start;
                    wasLastBlock = block.Next == null;
                    following = block.End - index;
                }
                ArraySegment<byte> array;
                Debug.Assert(block.Memory.TryGetArray(out array));
                while (following > 0)
                {
                    // Need unit tests to test Vector path
#if !DEBUG
                    // Check will be Jitted away https://github.com/dotnet/coreclr/issues/1079
                    if (Vector.IsHardwareAccelerated)
                    {
#endif
                    if (following >= _vectorSpan)
                    {
                        var byte0Equals = Vector.Equals(new Vector<byte>(array.Array, array.Offset + index), byte0Vector);

                        if (byte0Equals.Equals(Vector<byte>.Zero))
                        {
                            if (bytesScanned + _vectorSpan >= limit)
                            {
                                bytesScanned = limit;
                                return -1;
                            }

                            bytesScanned += _vectorSpan;
                            following -= _vectorSpan;
                            index += _vectorSpan;
                            continue;
                        }
                        var firstEqualByteIndex = LocateFirstFoundByte(byte0Equals);
                        var vectorBytesScanned = firstEqualByteIndex + 1;

                        if (bytesScanned + vectorBytesScanned > limit)
                        {
                            // Ensure iterator is left at limit position
                            bytesScanned = limit;
                            return -1;
                        }

                        bytesScanned += vectorBytesScanned;

                        result = new ReadCursor(block, index + firstEqualByteIndex);
                        return byte0;
                    }
                    // Need unit tests to test Vector path
#if !DEBUG
                    }
#endif
                    fixed (byte* pCurrentFixed = array.Array)
                    {
                        var pCurrent = pCurrentFixed + array.Offset + index;

                        var pEnd = pCurrent + Math.Min(following, limit - bytesScanned);
                        do
                        {
                            bytesScanned++;
                            if (*pCurrent == byte0)
                            {
                                result = new ReadCursor(block, index);
                                return byte0;
                            }
                            pCurrent++;
                            index++;
                        } while (pCurrent < pEnd);
                    }
                    following = 0;
                    break;
                }
            }
        }

        public static unsafe int Seek(ReadCursor begin, ReadCursor end, out ReadCursor result, byte byte0)
        {
            result = default(ReadCursor);
            var block = begin.Segment;
            if (block == null)
            {
                return -1;
            }

            var index = begin.Index;
            var wasLastBlock = block.Next == null;
            var following = block.End - index;

            while (true)
            {
                while (following == 0)
                {
                    if ((block == end.Segment && index > end.Index) ||
                        wasLastBlock)
                    {
                        return -1;
                    }

                    block = block.Next;
                    index = block.Start;
                    wasLastBlock = block.Next == null;
                    following = block.End - index;
                }
                ArraySegment<byte> array;
                Debug.Assert(block.Memory.TryGetArray(out array));
                while (following > 0)
                {
                    // Need unit tests to test Vector path
#if !DEBUG
                    // Check will be Jitted away https://github.com/dotnet/coreclr/issues/1079
                    if (Vector.IsHardwareAccelerated)
                    {
#endif
                    if (following >= _vectorSpan)
                    {
                        var byte0Equals = Vector.Equals(new Vector<byte>(array.Array, array.Offset + index), GetVector(byte0));

                        if (byte0Equals.Equals(Vector<byte>.Zero))
                        {
                            if (block == end.Segment && index + _vectorSpan > end.Index)
                            {
                                return -1;
                            }

                            following -= _vectorSpan;
                            index += _vectorSpan;
                            continue;
                        }

                        var byteIndex = LocateFirstFoundByte(byte0Equals);

                        if (block == end.Segment && index + byteIndex > end.Index)
                        {
                            // Ensure iterator is left at limit position
                            return -1;
                        }

                        result = new ReadCursor(block, index + byteIndex);
                        return byte0;
                    }
                    // Need unit tests to test Vector path
#if !DEBUG
                    }
#endif
                    fixed (byte* pCurrentFixed = array.Array)
                    {
                        var pCurrent = pCurrentFixed + array.Offset + index;

                        var pEnd = block == end.Segment ? pCurrentFixed + end.Index + 1 : pCurrent + following;
                        do
                        {
                            if (*pCurrent == byte0)
                            {
                                result = new ReadCursor(block, index);
                                return byte0;
                            }
                            pCurrent++;
                            index++;
                        } while (pCurrent < pEnd);
                    }
                    following = 0;
                    break;
                }
            }
        }

        public static unsafe int Seek(ReadCursor begin, ReadCursor end, out ReadCursor result, byte byte0, byte byte1)
        {
            result = default(ReadCursor);
            var block = begin.Segment;
            if (block == null)
            {
                return -1;
            }

            var index = begin.Index;
            var wasLastBlock = block.Next == null;
            var following = block.End - index;
            int byteIndex = int.MaxValue;

            while (true)
            {
                while (following == 0)
                {
                    if ((block == end.Segment && index > end.Index) ||
                        wasLastBlock)
                    {
                        return -1;
                    }
                    block = block.Next;
                    index = block.Start;
                    wasLastBlock = block.Next == null;
                    following = block.End - index;
                }
                ArraySegment<byte> array;
                Debug.Assert(block.Memory.TryGetArray(out array));

                while (following > 0)
                {

                    // Need unit tests to test Vector path
#if !DEBUG
                    // Check will be Jitted away https://github.com/dotnet/coreclr/issues/1079
                    if (Vector.IsHardwareAccelerated)
                    {
#endif
                    if (following >= _vectorSpan)
                    {
                        var data = new Vector<byte>(array.Array, index + array.Offset);

                        var byteEquals = Vector.Equals(data, GetVector(byte0));
                        byteEquals = Vector.ConditionalSelect(byteEquals, byteEquals, Vector.Equals(data, GetVector(byte1)));

                        if (!byteEquals.Equals(Vector<byte>.Zero))
                        {
                            byteIndex = LocateFirstFoundByte(byteEquals);
                        }

                        if (byteIndex == int.MaxValue)
                        {
                            following -= _vectorSpan;
                            index += _vectorSpan;

                            if (block == end.Segment && index > end.Index)
                            {
                                return -1;
                            }

                            continue;
                        }

                        if (block == end.Segment && index + byteIndex > end.Index)
                        {
                            // Ensure iterator is left at limit position
                            return -1;
                        }

                        result = new ReadCursor(block, index + byteIndex);
                        return array.Array[array.Offset + index + byteIndex];
                    }
                    // Need unit tests to test Vector path
#if !DEBUG
                    }
#endif
                    fixed (byte* pCurrentFixed = array.Array)
                    {
                        var pCurrent = pCurrentFixed + array.Offset + index;

                        var pEnd = block == end.Segment ? pCurrentFixed + end.Index + 1 : pCurrent + following;
                        do
                        {
                            if (*pCurrent == byte0)
                            {
                                result = new ReadCursor(block, index);
                                return byte0;
                            }
                            if (*pCurrent == byte1)
                            {
                                result = new ReadCursor(block, index);
                                return byte1;
                            }
                            pCurrent++;
                            index++;
                        } while (pCurrent != pEnd);
                    }

                    following = 0;
                    break;
                }
            }
        }

        public static unsafe int Seek(ReadCursor begin, ReadCursor end, out ReadCursor result, byte byte0, byte byte1, byte byte2)
        {
            result = default(ReadCursor);
            var block = begin.Segment;
            if (block == null)
            {
                return -1;
            }

            var index = begin.Index;
            var wasLastBlock = block.Next == null;
            var following = block.End - index;
            int byteIndex = int.MaxValue;

            while (true)
            {
                while (following == 0)
                {
                    if ((block == end.Segment && index > end.Index) ||
                        wasLastBlock)
                    {
                        return -1;
                    }
                    block = block.Next;
                    index = block.Start;
                    wasLastBlock = block.Next == null;
                    following = block.End - index;
                }
                ArraySegment<byte> array;
                Debug.Assert(block.Memory.TryGetArray(out array));

                while (following > 0)
                {
                    // Need unit tests to test Vector path
#if !DEBUG
                    // Check will be Jitted away https://github.com/dotnet/coreclr/issues/1079
                    if (Vector.IsHardwareAccelerated)
                    {
#endif
                    if (following >= _vectorSpan)
                    {
                        var data = new Vector<byte>(array.Array, array.Offset + index);

                        var byteEquals = Vector.Equals(data, GetVector(byte0));
                        byteEquals = Vector.ConditionalSelect(byteEquals, byteEquals, Vector.Equals(data, GetVector(byte1)));
                        byteEquals = Vector.ConditionalSelect(byteEquals, byteEquals, Vector.Equals(data, GetVector(byte2)));

                        if (!byteEquals.Equals(Vector<byte>.Zero))
                        {
                            byteIndex = LocateFirstFoundByte(byteEquals);
                        }

                        if (byteIndex == int.MaxValue)
                        {
                            following -= _vectorSpan;
                            index += _vectorSpan;

                            if (block == end.Segment && index > end.Index)
                            {
                                return -1;
                            }

                            continue;
                        }

                        if (block == end.Segment && index + byteIndex > end.Index)
                        {
                            // Ensure iterator is left at limit position
                            return -1;
                        }

                        result = new ReadCursor(block, index + byteIndex);
                        return array.Array[array.Offset + index + byteIndex];
                    }
                    // Need unit tests to test Vector path
#if !DEBUG
                    }
#endif

                    fixed (byte* pCurrentFixed = array.Array)
                    {
                        var pCurrent = pCurrentFixed + array.Offset + index;

                        var pEnd = block == end.Segment ? pCurrentFixed + end.Index + 1 : pCurrent + following;
                        do
                        {
                            if (*pCurrent == byte0)
                            {
                                result = new ReadCursor(block, index);
                                return byte0;
                            }
                            if (*pCurrent == byte1)
                            {
                                result = new ReadCursor(block, index);
                                return byte1;
                            }
                            if (*pCurrent == byte2)
                            {
                                result = new ReadCursor(block, index);
                                return byte2;
                            }
                            pCurrent++;
                            index++;
                        } while (pCurrent != pEnd);
                    }
                    following = 0;
                    break;
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector<byte> GetVector(byte vectorByte)
        {
            // Vector<byte> .ctor doesn't become an intrinsic due to detection issue
            // However this does cause it to become an intrinsic (with additional multiply and reg->reg copy)
            // https://github.com/dotnet/coreclr/issues/7459#issuecomment-253965670
            return Vector.AsVectorByte(new Vector<uint>(vectorByte * 0x01010101u));
        }

        /// <summary>
        /// Locate the first of the found bytes
        /// </summary>
        /// <param  name="byteEquals"></param >
        /// <returns>The first index of the result vector</returns>
        // Force inlining (64 IL bytes, 91 bytes asm) Issue: https://github.com/dotnet/coreclr/issues/7386
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static int LocateFirstFoundByte(Vector<byte> byteEquals)
        {
            var vector64 = Vector.AsVectorUInt64(byteEquals);
            ulong longValue = 0;
            var i = 0;
            // Pattern unrolled by jit https://github.com/dotnet/coreclr/pull/8001
            for (; i < Vector<ulong>.Count; i++)
            {
                longValue = vector64[i];
                if (longValue == 0) continue;
                break;
            }

            // Flag least significant power of two bit
            var powerOfTwoFlag = (longValue ^ (longValue - 1));
            // Shift all powers of two into the high byte and extract
            var foundByteIndex = (int)((powerOfTwoFlag * _xorPowerOfTwoToHighByte) >> 57);
            // Single LEA instruction with jitted const (using function result)
            return i * 8 + foundByteIndex;
        }
    }
}
