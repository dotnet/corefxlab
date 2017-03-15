// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Sequences;
using System.Diagnostics;
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int IndexOf(this ReadOnlyMemory<byte> memory, ReadOnlySpan<byte> values)
        {
            return SpanExtensions.IndexOf(memory.Span, values);
        }
    }
}