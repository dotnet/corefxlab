// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Buffers.Operations;

namespace System.Buffers
{
    public static class MemoryListExtensions
    {
        // span creation helpers:
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long IndexOf(this ReadOnlySequenceSegment<byte> list, ReadOnlySpan<byte> value)
        {
            var first = list.Memory.Span;
            var index = first.IndexOf(value);
            if (index != -1) return index;

            var rest = list.Next;
            if (rest == null) return -1;

            return IndexOfStraddling(first, list.Next, value);
        }

        public static int CopyTo(this ReadOnlySequenceSegment<byte> list, Span<byte> destination)
        {
            var current = list.Memory.Span;
            int copied = 0;

            while (destination.Length > 0)
            {
                if (current.Length >= destination.Length)
                {
                    current.Slice(0, destination.Length).CopyTo(destination);
                    copied += destination.Length;
                    return copied;
                }
                else
                {
                    current.CopyTo(destination);
                    copied += current.Length;
                    destination = destination.Slice(current.Length);
                }
            }
            return copied;
        }

        public static SequencePosition? PositionOf(this ReadOnlySequenceSegment<byte> list, byte value)
        {
            while (list != null)
            {
                var current = list.Memory.Span;
                var index = current.IndexOf(value);
                if (index != -1) return new SequencePosition(list, index);
                list = list.Next;
            }
            return null;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static (T segment, int index) Get<T>(this SequencePosition position)
        {
            var segment = position.GetObject() == null ? default : (T)position.GetObject();
            return (segment, position.GetInteger());
        }

        // TODO (pri 3): I am pretty sure this whole routine can be written much better

        // searches values that potentially straddle between first and rest
        internal static long IndexOfStraddling(this ReadOnlySpan<byte> first, ReadOnlySequenceSegment<byte> rest, ReadOnlySpan<byte> value)
        {
            Debug.Assert(first.IndexOf(value) == -1);
            if (rest == null) return -1;

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

            long index;

            // now combine the bytes from the end of the first buffer with bytes in the rest, and serarch the combined buffer
            // this check is a small optimization: if the first byte from the value does not exist in the bytesToSearchAgain, there is no reason to combine
            if (bytesToSearchAgain.IndexOf(value[0]) != -1)
            {
                var combinedBufferLength = value.Length << 1;
                var combined = combinedBufferLength < 128 ?
                                        stackalloc byte[combinedBufferLength] :
                                        // TODO (pri 3): I think this could be eliminated by chunking values
                                        new byte[combinedBufferLength];

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
    }

    public static class BufferExtensions
    {
        const int stackLength = 32;

        public static void Pipe(this IBufferOperation transformation, ReadOnlySequence<byte> source, IBufferWriter<byte> destination)
        {
            int afterMergeSlice = 0;

            // Assign 'remainder' to something formally stack-referring.
            // The default classification is "returnable, not referring to stack", we want the opposite in this case.
            ReadOnlySpan<byte> remainder = stackalloc byte[0];
            Span<byte> stackSpan = stackalloc byte[stackLength];

            SequencePosition poisition = default;
            while (source.TryGet(ref poisition, out var sourceBuffer))
            {
                Span<byte> outputSpan = destination.GetSpan();
                ReadOnlySpan<byte> sourceSpan = sourceBuffer.Span;

                if (!remainder.IsEmpty)
                {
                    int leftOverBytes = remainder.Length;
                    remainder.CopyTo(stackSpan);
                    int amountToCopy = Math.Min(sourceSpan.Length, stackSpan.Length - leftOverBytes);
                    sourceSpan.Slice(0, amountToCopy).CopyTo(stackSpan.Slice(leftOverBytes));
                    int amountOfData = leftOverBytes + amountToCopy;

                    Span<byte> spanToTransform = stackSpan.Slice(0, amountOfData);

                    TryTransformWithRemainder:
                    OperationStatus status = transformation.Execute(spanToTransform, outputSpan, out int bytesConsumed, out int bytesWritten);
                    if (status != OperationStatus.Done)
                    {
                        destination.Advance(bytesWritten);
                        spanToTransform = spanToTransform.Slice(bytesConsumed);

                        if (status == OperationStatus.DestinationTooSmall)
                        {
                            outputSpan = destination.GetSpan();

                            if (outputSpan.Length - bytesWritten < 3)
                            {
                                return; // no more output space, user decides what to do.
                            }
                            goto TryTransformWithRemainder;
                        }
                        else
                        {
                            if (status == OperationStatus.InvalidData)
                            {
                                continue; // source buffer contains invalid bytes, user decides what to do for fallback
                            }

                            // at this point, status = TransformationStatus.NeedMoreSourceData
                            // left over bytes in stack span
                            remainder = spanToTransform;
                        }
                        continue;
                    }
                    else    // success
                    {
                        afterMergeSlice = bytesConsumed - remainder.Length;
                        remainder = Span<byte>.Empty;
                        destination.Advance(bytesWritten);
                        outputSpan = destination.GetSpan();
                    }
                }

                TryTransform:
                OperationStatus result = transformation.Execute(sourceSpan.Slice(afterMergeSlice), outputSpan, out int consumed, out int written);
                afterMergeSlice = 0;
                destination.Advance(written);
                sourceSpan = sourceSpan.Slice(consumed);

                if (result == OperationStatus.Done) continue;

                // Not successful
                if (result == OperationStatus.DestinationTooSmall)
                {
                    destination.GetMemory();  // output buffer is too small
                    outputSpan = destination.GetSpan();
                    if (outputSpan.Length - written < 3)
                    {
                        return; // no more output space, user decides what to do.
                    }
                    goto TryTransform;
                }
                else
                {
                    if (result == OperationStatus.InvalidData)
                    {
                        continue; // source buffer contains invalid bytes, user decides what to do for fallback
                    }

                    // at this point, result = TransformationStatus.NeedMoreSourceData
                    // left over bytes in source span
                    remainder = sourceSpan;
                }
            }
            return;
        }

        public static bool SequenceEqual<T>(this Memory<T> first, Memory<T> second) where T : struct, IEquatable<T>
        {
            return first.Span.SequenceEqual(second.Span);
        }

        public static bool SequenceEqual<T>(this ReadOnlyMemory<T> first, ReadOnlyMemory<T> second) where T : struct, IEquatable<T>
        {
            return first.Span.SequenceEqual(second.Span);
        }

        public static int SequenceCompareTo(this Span<byte> left, ReadOnlySpan<byte> right)
        {
            return SequenceCompareTo((ReadOnlySpan<byte>)left, right);
        }

        public static int SequenceCompareTo(this ReadOnlySpan<byte> left, ReadOnlySpan<byte> right)
        {
            var minLength = left.Length;
            if (minLength > right.Length) minLength = right.Length;
            for (int i = 0; i < minLength; i++)
            {
                var result = left[i].CompareTo(right[i]);
                if (result != 0) return result;
            }
            return left.Length.CompareTo(right.Length);
        }

        public static bool TryIndicesOf(this Span<byte> buffer, byte value, Span<int> indices, out int numberOfIndices)
        {
            var length = buffer.Length;
            if (length == 0 || indices.Length == 0)
            {
                numberOfIndices = 0;
                return false;
            }

            return TryIndicesOf(ref MemoryMarshal.GetReference(buffer), value, length, indices, out numberOfIndices);
        }

        public static bool TryIndicesOf(this ReadOnlySpan<byte> buffer, byte value, Span<int> indices, out int numberOfIndices)
        {
            var length = buffer.Length;
            if (length == 0 || indices.Length == 0)
            {
                numberOfIndices = 0;
                return false;
            }

            return TryIndicesOf(ref MemoryMarshal.GetReference(buffer), value, length, indices, out numberOfIndices);
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
        private const ulong byteBroadcastToUlong = ~0UL / Byte.MaxValue;
        private const ulong filterByteHighBitsInUlong = (byteBroadcastToUlong >> 1) | (byteBroadcastToUlong << (sizeof(ulong) * 8 - 1));
    }
}
