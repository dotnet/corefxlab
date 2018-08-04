// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers.Text;
using System.Collections.Sequences;

namespace System.Buffers
{
    public static class Sequence
    {
        public static ReadOnlySpan<byte> ToSpan(this ReadOnlySequence<byte> sequence)
        {
            SequencePosition position = sequence.Start;
            ResizableArray<byte> array = new ResizableArray<byte>(1024);
            while (sequence.TryGet(ref position, out ReadOnlyMemory<byte> buffer))
            {
                array.AddAll(buffer.Span);
            }
            return array.Span;
        }

        public static ReadOnlySpan<byte> ToSpan<T>(this T sequence) where T : ISequence<ReadOnlyMemory<byte>>
        {
            SequencePosition position = sequence.Start;
            ResizableArray<byte> array = new ResizableArray<byte>(1024);
            while (sequence.TryGet(ref position, out ReadOnlyMemory<byte> buffer))
            {
                array.AddAll(buffer.Span);
            }
            array.Resize(array.Count);
            return array.Span.Slice(0, array.Count);
        }

        // TODO: this cannot be an extension method (as I would like it to be).
        // If I make it an extensions method, the compiler complains Span<T> cannot
        // be used as a type parameter.
        public static long IndexOf(ReadOnlySequence<byte> sequence, byte value)
        {
            SequencePosition position = sequence.Start;
            int totalIndex = 0;
            while (sequence.TryGet(ref position, out ReadOnlyMemory<byte> memory))
            {
                var index = memory.Span.IndexOf(value);
                if (index != -1) return index + totalIndex;
                totalIndex += memory.Length;
            }
            return -1;
        }

        public static long IndexOf(ReadOnlySequence<byte> sequence, byte v1, byte v2)
        {
            SequencePosition position = sequence.Start;
            int totalIndex = 0;
            while (sequence.TryGet(ref position, out ReadOnlyMemory<byte> memory))
            {
                var span = memory.Span;
                var index = span.IndexOf(v1);
                if (index != -1)
                {
                    if (span.Length > index + 1)
                    {
                        if (span[index + 1] == v2) return index + totalIndex;
                        else throw new NotImplementedException(); // need to check farther in the span
                    }
                    else
                    {
                        if (sequence.TryGet(ref position, out var next, false))
                        {
                            var nextSpan = next.Span;
                            if (nextSpan.Length > 0)
                            {
                                if (next.Span[0] == v2) return totalIndex + index;
                            }
                        }
                    }
                }
                totalIndex += memory.Length;
            }
            return -1;
        }

        public static SequencePosition? PositionOf(this ReadOnlySequence<byte> sequence, byte value)
        {
            SequencePosition position = sequence.Start;
            SequencePosition result = position;
            while (sequence.TryGet(ref position, out ReadOnlyMemory<byte> memory))
            {
                var index = MemoryExtensions.IndexOf(memory.Span, value);
                if (index != -1)
                {
                    result = sequence.GetPosition(index, result);
                    return result;
                }
                result = position;
            }
            return null;
        }

        public static SequencePosition? PositionAt(this ReadOnlySequence<byte> sequence, long index)
        {
            SequencePosition position = sequence.Start;
            SequencePosition result = position;
            while (sequence.TryGet(ref position, out ReadOnlyMemory<byte> memory))
            {
                var span = memory.Span;
                if (span.Length > index)
                {
                    result = sequence.GetPosition(index, result);
                    return result;
                }
                index -= span.Length;
                result = position;
            }

            return null;
        }

        public static int Copy(ReadOnlySequence<byte> sequence, Span<byte> buffer)
        {
            int copied = 0;
            var position = sequence.Start;
            while (sequence.TryGet(ref position, out ReadOnlyMemory<byte> memory, true))
            {
                var span = memory.Span;
                var toCopy = Math.Min(span.Length, buffer.Length - copied);
                span.Slice(0, toCopy).CopyTo(buffer.Slice(copied));
                copied += toCopy;
                if (copied >= buffer.Length) break;
            }
            return copied;
        }

        public static int Copy(ReadOnlySequence<byte> sequence, SequencePosition from, Span<byte> buffer)
        {
            int copied = 0;
            while (sequence.TryGet(ref from, out ReadOnlyMemory<byte> memory, true))
            {
                var span = memory.Span;
                var toCopy = Math.Min(span.Length, buffer.Length - copied);
                span.Slice(0, toCopy).CopyTo(buffer.Slice(copied));
                copied += toCopy;
                if (copied >= buffer.Length) break;
            }
            return copied;
        }

        public static bool TryParse(ReadOnlySequence<byte> sequence, out int value, out int consumed)
        {
            var position = sequence.Start;
            if (sequence.TryGet(ref position, out ReadOnlyMemory<byte> memory))
            {
                var span = memory.Span;
                if (Utf8Parser.TryParse(span, out value, out consumed) && consumed < span.Length)
                {
                    return true;
                }

                Span<byte> temp = stackalloc byte[11]; // TODO: it would be good to have APIs to return constants related to sizes of needed buffers
                var copied = Copy(sequence, temp);
                // we need to slice temp, as we might stop zeroing stack allocated buffers
                if (Utf8Parser.TryParse(temp.Slice(0, copied), out value, out consumed))
                {
                    return true;
                }
            }

            value = default;
            consumed = default;
            return false;
        }

        public static bool TryParse(ReadOnlySequence<byte> sequence, out int value, out SequencePosition consumed)
        {
            if (!TryParse(sequence, out value, out int consumedBytes))
            {
                consumed = default;
                return false;
            }

            consumed = sequence.PositionAt(consumedBytes).GetValueOrDefault();
            return true;
        }
    }
}
