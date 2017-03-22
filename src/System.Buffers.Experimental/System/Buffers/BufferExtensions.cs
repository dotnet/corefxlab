// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Sequences;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Numerics;

namespace System.Buffers
{
    public static class BufferExtensions
    {
        public static ReadOnlySpan<byte> ToSpan<T>(this T bufferSequence) where T : ISequence<ReadOnlyBuffer<byte>>
        {
            Position position = Position.First;
            ReadOnlyBuffer<byte> buffer;
            ResizableArray<byte> array = new ResizableArray<byte>(1024); 
            while (bufferSequence.TryGet(ref position, out buffer))
            {
                array.AddAll(buffer.Span);
            }
            array.Resize(array.Count);
            return array.Items.Slice(0, array.Count);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int IndexOf(this IReadOnlyBufferList<byte> sequence, ReadOnlySpan<byte> value)
        {
            var first = sequence.First.Span;
            var index = first.IndexOf(value);
            if (index != -1) return index;

            var rest = sequence.Rest;
            if (rest == null) return -1;

            return IndexOfStraddling(first, sequence.Rest, value);
        }

        public static int IndexOf(this IReadOnlyBufferList<byte> sequence, byte value)
        {
            var first = sequence.First.Span;
            var index = first.IndexOf(value);
            if (index != -1) return index;

            var rest = sequence.Rest;
            if (rest == null) return -1;

            index = rest.IndexOf(value);
            if (index != -1) return first.Length + index;

            return -1;
        }

        // TODO (pri 3): I am pretty sure this whole routine can be written much better

        // searches values that potentially straddle between first and rest
        internal static int IndexOfStraddling(this ReadOnlySpan<byte> first, IReadOnlyBufferList<byte> rest, ReadOnlySpan<byte> value)
        {
            Debug.Assert(rest != null);

            // we only need to search the end of the first buffer. More precisely, only up to value.Length - 1 bytes in the first buffer
            // The other bytes in first, were already search and presumably did not match
            int bytesToSkipFromFirst = 0; 
            if (first.Length > value.Length - 1)
            {
                bytesToSkipFromFirst = first.Length - value.Length - 1;
            }

            // now that we know how many bytes we need to skip, create slice of first buffer with bytes that need to be searched.
            ReadOnlySpan<byte> bytesToSearchAgain;
            if (bytesToSkipFromFirst > 0)
            {
                bytesToSearchAgain = first.Slice(bytesToSkipFromFirst);
            }
            else
            {
                bytesToSearchAgain = first;
            }

            int index;

            // now combine the bytes from the end of the first buffer with bytes in the rest, and serarch the combined buffer
            // this check is a small optimization: if the first byte from the value does not exist in the bytesToSearchAgain, there is no reason to combine
            if (bytesToSearchAgain.IndexOf(value[0]) != -1)
            {
                Span<byte> combined;
                var combinedBufferLength = value.Length << 1;
                if (combinedBufferLength < 128)
                {
                    unsafe
                    {
                        byte* temp = stackalloc byte[combinedBufferLength];
                        combined = new Span<byte>(temp, combinedBufferLength);
                    }
                }
                else
                {
                    // TODO (pri 3): I think this could be eliminated by chunking values
                    combined = new byte[combinedBufferLength];
                }

                bytesToSearchAgain.CopyTo(combined);
                int combinedLength = bytesToSearchAgain.Length + rest.CopyTo(combined.Slice(bytesToSearchAgain.Length));
                combined = combined.Slice(0, combinedLength);

                if (combined.Length < value.Length) return -1;

                index = combined.IndexOf(value);
                if (index != -1)
                {
                    return index + bytesToSkipFromFirst;
                }
            }

            // try to find the bytes in _rest
            index = rest.IndexOf(value);
            if (index != -1) return first.Length + index;

            return -1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int IndexOf(this ReadOnlyBuffer<byte> buffer, ReadOnlySpan<byte> values)
        {
            return SpanExtensions.IndexOf(buffer.Span, values);
        }

        public static int SequentialIndexOf(this Span<byte> span, byte value)
        {
            return SequentialIndexOf(ref span.DangerousGetPinnableReference(), value, span.Length);
        }

        public static int SequentialIndexOf(this ReadOnlySpan<byte> span, byte value)
        {
            return SequentialIndexOf(ref span.DangerousGetPinnableReference(), value, span.Length);
        }

        private static unsafe int SequentialIndexOf(ref byte searchSpace, byte value, int length)
        {
            Debug.Assert(length >= 0);

            IntPtr index = (IntPtr)0; // Use IntPtr for arithmetic to avoid unnecessary 64->32->64 truncations
            while (length >= 8)
            {
                length -= 8;

                if (value == Unsafe.Add(ref searchSpace, index))
                    goto Found;
                if (value == Unsafe.Add(ref searchSpace, index + 1))
                    goto Found1;
                if (value == Unsafe.Add(ref searchSpace, index + 2))
                    goto Found2;
                if (value == Unsafe.Add(ref searchSpace, index + 3))
                    goto Found3;
                if (value == Unsafe.Add(ref searchSpace, index + 4))
                    goto Found4;
                if (value == Unsafe.Add(ref searchSpace, index + 5))
                    goto Found5;
                if (value == Unsafe.Add(ref searchSpace, index + 6))
                    goto Found6;
                if (value == Unsafe.Add(ref searchSpace, index + 7))
                    goto Found7;

                index += 8;
            }

            if (length >= 4)
            {
                length -= 4;

                if (value == Unsafe.Add(ref searchSpace, index))
                    goto Found;
                if (value == Unsafe.Add(ref searchSpace, index + 1))
                    goto Found1;
                if (value == Unsafe.Add(ref searchSpace, index + 2))
                    goto Found2;
                if (value == Unsafe.Add(ref searchSpace, index + 3))
                    goto Found3;

                index += 4;
            }

            while (length > 0)
            {
                if (value == Unsafe.Add(ref searchSpace, index))
                    goto Found;

                index += 1;
                length--;
            }
            return -1;

            Found: // Workaround for https://github.com/dotnet/coreclr/issues/9692
            return (int)(byte*)index;
            Found1:
            return (int)(byte*)(index + 1);
            Found2:
            return (int)(byte*)(index + 2);
            Found3:
            return (int)(byte*)(index + 3);
            Found4:
            return (int)(byte*)(index + 4);
            Found5:
            return (int)(byte*)(index + 5);
            Found6:
            return (int)(byte*)(index + 6);
            Found7:
            return (int)(byte*)(index + 7);
        }

        public static bool TryIndicesOf(this Span<byte> buffer, byte value, Span<int> indices, out int numberOfIndices)
        {
            var length = buffer.Length;
            if (length == 0 || indices.Length == 0)
            {
                numberOfIndices = 0;
                return false;
            }

            return TryIndicesOf(ref buffer.DangerousGetPinnableReference(), value, length, indices, out numberOfIndices);
        }

        public static bool TryIndicesOf(this ReadOnlySpan<byte> buffer, byte value, Span<int> indices, out int numberOfIndices)
        {
            var length = buffer.Length;
            if (length == 0 || indices.Length == 0)
            {
                numberOfIndices = 0;
                return false;
            }

            return TryIndicesOf(ref buffer.DangerousGetPinnableReference(), value, length, indices, out numberOfIndices);
        }

        private unsafe static bool TryIndicesOf(ref byte searchSpace, byte value, int length, Span<int> indices, out int numberOfIndices)
        {
            var result = false;
            numberOfIndices = 0;

            fixed (byte* pSearchSpace = &searchSpace)
            {
                var searchStart = pSearchSpace;
                var offset = 0;

                while (true)
                {
                    if (Vector.IsHardwareAccelerated)
                    {
                        // Check Vector lengths
                        if (length - Vector<byte>.Count >= offset)
                        {
                            Vector<byte> values = GetVector(value);
                            do
                            {
                                var vFlaggedMatches = Vector.Equals(Unsafe.Read<Vector<byte>>(searchStart + offset), values);
                                if (!vFlaggedMatches.Equals(Vector<byte>.Zero))
                                {
                                    // Found match, reuse Vector values to keep register pressure low
                                    values = vFlaggedMatches;
                                    break;
                                }

                                offset += Vector<byte>.Count;
                            } while (length - Vector<byte>.Count >= offset);

                            // Found match? Perform secondary search outside out of loop, so above loop body is small
                            if (length - Vector<byte>.Count >= offset)
                            {
                                // Find offset of first match
                                offset += LocateFirstFoundByte(values);
                                // goto rather than inline return to keep function smaller
                                goto exitFixed;
                            }
                        }
                    }

                    ulong flaggedMatches = 0;
                    // Check ulong length
                    while (length - sizeof(ulong) >= offset)
                    {
                        flaggedMatches = SetLowBitsForByteMatch(*(ulong*)(searchStart + offset), value);
                        if (flaggedMatches != 0)
                        {
                            // Found match
                            break;
                        }

                        offset += sizeof(ulong);
                    }

                    // Found match? Perform secondary search outside out of loop, so above loop body is small
                    if (length - sizeof(ulong) >= offset)
                    {
                        // Find offset of first match
                        offset += LocateFirstFoundByte(flaggedMatches);
                        // goto rather than inline return to keep function smaller
                        goto exitFixed;
                    }

                    // Haven't found match, scan through remaining
                    for (; offset < length; offset++)
                    {
                        if (*(searchStart + offset) == value)
                        {
                            // goto rather than inline return to keep loop body small
                            goto exitFixed;
                        }
                    }

                    // No Matches
                    result = true;
                    break;

                    exitFixed:;
                    indices[numberOfIndices++] = offset++;
                    if (numberOfIndices >= indices.Length)
                        break;
                }
            }

            return result && numberOfIndices < indices.Length;
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int LocateFirstFoundByte(Vector<byte> match)
        {
            var vector64 = Vector.AsVectorUInt64(match);
            ulong candidate = 0;
            var i = 0;
            // Pattern unrolled by jit https://github.com/dotnet/coreclr/pull/8001
            for (; i < Vector<ulong>.Count; i++)
            {
                candidate = vector64[i];
                if (candidate != 0)
                {
                    break;
                }
            }

            // Single LEA instruction with jitted const (using function result)
            return i * 8 + LocateFirstFoundByte(candidate);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int LocateFirstFoundByte(ulong match)
        {
            unchecked
            {
                // Flag least significant power of two bit
                var powerOfTwoFlag = match ^ (match - 1);
                // Shift all powers of two into the high byte and extract
                return (int)((powerOfTwoFlag * xorPowerOfTwoToHighByte) >> 57);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static ulong SetLowBitsForByteMatch(ulong potentialMatch, byte search)
        {
            unchecked
            {
                var flaggedValue = potentialMatch ^ (byteBroadcastToUlong * search);
                return (
                        (flaggedValue - byteBroadcastToUlong) &
                        ~(flaggedValue) &
                        filterByteHighBitsInUlong
                       ) >> 7;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Vector<byte> GetVector(byte vectorByte)
        {
#if !NETCOREAPP
            // Vector<byte> .ctor doesn't become an intrinsic due to detection issue
            // However this does cause it to become an intrinsic (with additional multiply and reg->reg copy)
            // https://github.com/dotnet/coreclr/issues/7459#issuecomment-253965670
            return Vector.AsVectorByte(new Vector<uint>(vectorByte * 0x01010101u));
#else
            return new Vector<byte>(vectorByte);
#endif
        }

        private const ulong xorPowerOfTwoToHighByte = (0x07ul |
                                                       0x06ul << 8 |
                                                       0x05ul << 16 |
                                                       0x04ul << 24 |
                                                       0x03ul << 32 |
                                                       0x02ul << 40 |
                                                       0x01ul << 48) + 1;
        private const ulong byteBroadcastToUlong = ~0UL / byte.MaxValue;
        private const ulong filterByteHighBitsInUlong = (byteBroadcastToUlong >> 1) | (byteBroadcastToUlong << (sizeof(ulong) * 8 - 1));
    }
}