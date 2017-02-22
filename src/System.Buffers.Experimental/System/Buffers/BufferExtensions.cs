// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Sequences;
using System.Diagnostics;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace System.Buffers
{
    public static class BufferExtensions
    {
        public static ReadOnlySpan<byte> ToSpan<T>(this T memorySequence) where T : ISequence<ReadOnlyMemory<byte>>
        {
            Position position = Position.First;
            ReadOnlyMemory<byte> memory;
            ResizableArray<byte> array = new ResizableArray<byte>(memorySequence.Length.GetValueOrDefault(1024)); 
            while (memorySequence.TryGet(ref position, out memory))
            {
                array.AddAll(memory.Span);
            }
            array.Resize(array.Count);
            return array.Items.Slice(0, array.Count);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int IndexOf(this IReadOnlyMemoryList<byte> sequence, ReadOnlySpan<byte> value)
        {
            var first = sequence.First.Span;
            var index = first.IndexOf(value);
            if (index != -1) return index;

            var rest = sequence.Rest;
            if (rest == null) return -1;

            return IndexOfStraddling(first, sequence.Rest, value);
        }

        public static int IndexOf(this IReadOnlyMemoryList<byte> sequence, byte value)
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
        internal static int IndexOfStraddling(this ReadOnlySpan<byte> first, IReadOnlyMemoryList<byte> rest, ReadOnlySpan<byte> value)
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

        static readonly int s_longSize = Vector<ulong>.Count;
        static readonly int s_byteSize = Vector<byte>.Count;

        public unsafe static int IndexOfVectorized(this Span<byte> buffer, byte value)
        {
            var index = -1;
            var length = buffer.Length;
            if (length == 0)
            {
                goto exit;
            }

            fixed (byte* pHaystack = &buffer.DangerousGetPinnableReference())
            {
                var haystack = pHaystack;
                index = 0;

                if (Vector.IsHardwareAccelerated)
                {
                    if (length - Vector<byte>.Count >= index)
                    {
                        Vector<byte> needles = GetVector(value);
                        do
                        {
                            var flaggedMatches = Vector.Equals(Unsafe.Read<Vector<byte>>(haystack + index), needles);
                            if (flaggedMatches.Equals(Vector<byte>.Zero))
                            {
                                index += Vector<byte>.Count;
                                continue;
                            }

                            index += LocateFirstFoundByte(flaggedMatches);
                            goto exitFixed;

                        } while (length - Vector<byte>.Count >= index);
                    }
                }

                while (length - sizeof(ulong) >= index)
                {
                    var flaggedMatches = SetLowBitsForByteMatch(*(ulong*)(haystack + index), value);
                    if (flaggedMatches == 0)
                    {
                        index += sizeof(ulong);
                        continue;
                    }

                    index += LocateFirstFoundByte(flaggedMatches);
                    goto exitFixed;
                }

                for (; index < length; index++)
                {
                    if (*(haystack + index) == value)
                    {
                        goto exitFixed;
                    }
                }
                // No Matches
                index = -1;
                // Don't goto out of fixed block
        exitFixed:;
            }
        exit:
            return index;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public unsafe static int IndexOfVectorized(this ReadOnlySpan<byte> buffer, byte value)
        {
            Debug.Assert(s_longSize == 4 || s_longSize == 2);

            var index = -1;
            var length = buffer.Length;
            if (length == 0)
            {
                goto exit;
            }

            fixed (byte* pHaystack = &buffer.DangerousGetPinnableReference())
            {
                var haystack = pHaystack;
                index = 0;

                if (Vector.IsHardwareAccelerated)
                {
                    if (length - Vector<byte>.Count >= index)
                    {
                        Vector<byte> needles = GetVector(value);
                        do
                        {
                            var flaggedMatches = Vector.Equals(Unsafe.Read<Vector<byte>>(haystack + index), needles);
                            if (flaggedMatches.Equals(Vector<byte>.Zero))
                            {
                                index += Vector<byte>.Count;
                                continue;
                            }

                            index += LocateFirstFoundByte(flaggedMatches);
                            goto exitFixed;

                        } while (length - Vector<byte>.Count >= index);
                    }
                }

                while (length - sizeof(ulong) >= index)
                {
                    var flaggedMatches = SetLowBitsForByteMatch(*(ulong*)(haystack + index), value);
                    if (flaggedMatches == 0)
                    {
                        index += sizeof(ulong);
                        continue;
                    }

                    index += LocateFirstFoundByte(flaggedMatches);
                    goto exitFixed;
                }

                for (; index < length; index++)
                {
                    if (*(haystack + index) == value)
                    {
                        goto exitFixed;
                    }
                }
                // No Matches
                index = -1;
                // Don't goto out of fixed block
        exitFixed:;
            }
        exit:
            return index;
        }

        public static int ValidateCompileTimeEnumeratorUse(ReadOnlySpan<byte> input, int lastOffset, int maxBytesRemaining)
        {
            var upperBound = lastOffset + maxBytesRemaining;
            int headerStart = lastOffset;
            foreach (var headerEnd in input.MatchIndicies((byte)'\r', lastOffset, Math.Min(input.Length - lastOffset, maxBytesRemaining)))
            {
                if (headerEnd == upperBound)
                {
                    throw new Exception("Bad...");
                }

                if (headerEnd == input.Length)
                {
                    // Incomplete
                    return headerStart;
                }

                if (input[headerEnd + 1] != (byte)'\n')
                {
                    throw new Exception("Bad...");
                }

                if (headerEnd == headerStart)
                {
                    // End of headers
                    return -1;
                }

                string headerName = null;
                string headerValue = null;
                var isFirst = true;
                foreach (var valueStart in input.MatchIndicies((byte)':', headerStart, headerEnd - 1))
                {
                    if (!isFirst)
                    {
                        throw new Exception("Bad...");
                    }
                    else
                    {
                        isFirst = false;
                        headerName = input.ToAsciiString(headerStart, valueStart - headerStart);
                        headerValue = input.ToAsciiString(valueStart + 1, headerEnd - 1 - valueStart);
                    }
                }

                if (string.IsNullOrEmpty(headerName) || string.IsNullOrEmpty(headerValue))
                {
                    throw new Exception("Bad...");
                }

                // Do something with headers

                headerStart = headerEnd + 1;
            }

            if (input.Length >= upperBound)
            {
                throw new Exception("Bad...");
            }

            // Incomplete
            return headerStart;
        }

        public static string ToAsciiString(this ReadOnlySpan<byte> buffer)
        {
            if (buffer.Length == 0)
            {
                return string.Empty;
            }

            return string.Empty;
        }

        public static string ToAsciiString(this ReadOnlySpan<byte> buffer, int start)
        {
            if (buffer.Length - start == 0)
            {
                return string.Empty;
            }

            return string.Empty;
        }

        public static string ToAsciiString(this ReadOnlySpan<byte> buffer, int start, int length)
        {
            if (length == 0)
            {
                return string.Empty;
            }

            return string.Empty;
        }

        public static MatchesEnumerable MatchIndicies(this ReadOnlySpan<byte> buffer, byte value)
        {
            return new MatchesEnumerable(buffer, value);
        }

        public static MatchesEnumerable MatchIndicies(this ReadOnlySpan<byte> buffer, byte value, int start)
        {
            return new MatchesEnumerable(buffer, value, start);
        }

        public static MatchesEnumerable MatchIndicies(this ReadOnlySpan<byte> buffer, byte value, int start, int length)
        {
            return new MatchesEnumerable(buffer, value, start, length);
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
                if (candidate == 0) continue;
                break;
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
#if !NETCOREAPP1_2
            // Vector<byte> .ctor doesn't become an intrinsic due to detection issue
            // However this does cause it to become an intrinsic (with additional multiply and reg->reg copy)
            // https://github.com/dotnet/coreclr/issues/7459#issuecomment-253965670
            return Vector.AsVectorByte(new Vector<uint>(vectorByte * 0x01010101u));
#else
            return new Vector<byte>(vectorByte);
#endif
        }

        private const ulong xorPowerOfTwoToHighByte = (0x07ul       |
                                                       0x06ul << 8  |
                                                       0x05ul << 16 |
                                                       0x04ul << 24 |
                                                       0x03ul << 32 |
                                                       0x02ul << 40 |
                                                       0x01ul << 48 ) + 1;
        private const ulong flagsToHighByte =         (0x01ul       |
                                                       0x01ul << 7  |
                                                       0x01ul << 14 |
                                                       0x01ul << 21 |
                                                       0x01ul << 28 |
                                                       0x01ul << 35 |
                                                       0x01ul << 42 ) + 1;
        private const ulong byteBroadcastToUlong = ~0UL / byte.MaxValue;
        private const ulong filterByteHighBitsInUlong = (byteBroadcastToUlong >> 1) | (byteBroadcastToUlong << (sizeof(ulong) * 8 - 1));

        public struct MatchesEnumerable
        {
            private readonly ReadOnlySpan<byte> _buffer;
            private readonly byte _value;
            private readonly int _start;
            private readonly int _upperBound;

            internal MatchesEnumerable(ReadOnlySpan<byte> buffer, byte value)
            {
                _buffer = buffer;
                _value = value;
                _start = 0;
                _upperBound = buffer.Length;
            }

            internal MatchesEnumerable(ReadOnlySpan<byte> buffer, byte value, int start)
            {
                _buffer = buffer;
                _value = value;
                _start = start;
                _upperBound = buffer.Length;
            }

            internal MatchesEnumerable(ReadOnlySpan<byte> buffer, byte value, int start, int length)
            {
                _buffer = buffer;
                _value = value;
                _start = start;
                _upperBound = length + start;
            }

            public MatchesEnumerator GetEnumerator()
            {
                return new MatchesEnumerator(_buffer, _value, _start, _upperBound);
            }
        }

        public struct MatchesEnumerator
        {
            private ReadOnlySpan<byte> _buffer; // don't make readonly, methods called on it
            private readonly byte _value;
            private readonly int _upperBound;

            private ulong _currentMatches;
            private int _examinedIndex;
            private int _index;

            internal MatchesEnumerator(ReadOnlySpan<byte> buffer, byte value, int start, int upperBound)
            {
                _buffer = buffer;
                _value = value;
                _upperBound = upperBound;
                _currentMatches = 0;
                _examinedIndex = start - 1;
                _index = -1;
            }

            public bool MoveNext()
            {
                if (_currentMatches > 0)
                {
                    var location = _currentMatches ^ (_currentMatches - 1);
                    _currentMatches -= location;
                    // TODO: set _index to bitscanreverse(location)
                    return true;
                }
                else
                {
                    if (_examinedIndex == _upperBound)
                    {
                        return false;
                    }

                    return MoveNextSeek();
                }
            }

            private unsafe bool MoveNextSeek()
            {
                // if length < sizeof(ulong) don't fix? 
                var offset = _examinedIndex;
                fixed (byte* pSearchSpace = &_buffer.DangerousGetPinnableReference())
                {
                    var searchStart = pSearchSpace;

                    var upperBound = _upperBound;
                    var value = _value;

                    if (Vector.IsHardwareAccelerated)
                    {
                        if (upperBound - Vector<byte>.Count >= offset)
                        {
                            Vector<byte> values = GetVector(value);
                            do
                            {
                                var flaggedMatches = Vector.Equals(Unsafe.Read<Vector<byte>>(searchStart + offset), values);
                                if (flaggedMatches.Equals(Vector<byte>.Zero))
                                {
                                    offset += Vector<byte>.Count;
                                    continue;
                                }

                                _currentMatches = FlagFoundBytes(flaggedMatches);
                                _examinedIndex += offset + Vector<byte>.Count;
                                offset += LocateFirstFoundByte(flaggedMatches); // something better?
                                goto exitFixed;

                            } while (upperBound - Vector<byte>.Count >= offset);
                        }
                    }

                    while (upperBound - sizeof(ulong) >= offset)
                    {
                        var flaggedMatches = SetLowBitsForByteMatch(*(ulong*)(searchStart + offset), value);
                        if (flaggedMatches == 0)
                        {
                            offset += sizeof(ulong);
                            continue;
                        }

                        _currentMatches = (ulong)FlagFoundBytes(flaggedMatches) << 56;
                        _examinedIndex += offset + sizeof(ulong);
                        offset += LocateFirstFoundByte(flaggedMatches); // something better?
                        goto exitFixed;
                    }

                    for (; offset < upperBound; offset++)
                    {
                        if (*(searchStart + offset) == value)
                        {
                            _examinedIndex = offset;
                            goto exitFixed;
                        }
                    }
                    // No Matches
                    offset = -1;
                    _examinedIndex = _upperBound;
                    // Don't goto out of fixed block
                    exitFixed:;
                }

                _index = offset;
                return offset >= 0;
            }

            public int Current => _index;

            private static ulong FlagFoundBytes(Vector<byte> match)
            {
                var vector64 = Vector.AsVectorUInt64(match);
                ulong result = 0;
                // Pattern unrolled by jit https://github.com/dotnet/coreclr/pull/8001
                for (var i = 0; i < Vector<ulong>.Count; i++)
                {
                    var candidate = vector64[i];
                    if (candidate == 0) continue;
                    result |= (ulong)FlagFoundBytes(candidate) << ((Vector<ulong>.Count - i) * 8);
                }
                
                return result;
            }

            private static byte FlagFoundBytes(ulong match)
            {
                return (byte)(((match & filterByteHighBitsInUlong) * flagsToHighByte) >> 56);
            }
        }
    }
}