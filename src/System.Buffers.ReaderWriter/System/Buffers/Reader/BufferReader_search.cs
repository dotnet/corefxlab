// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Runtime.CompilerServices;

namespace System.Buffers.Reader
{
    // TODO: the TryReadUntil methods are very inneficient. We need to fix that.
    public static partial class BufferReaderExtensions
    {
        public static bool TryReadUntil(ref BufferReader reader, out ReadOnlySpan<byte> bytes, byte delimiter)
        {
            int idx = reader.UnreadSegment.IndexOf(delimiter);
            if (idx != -1)
            {
                bytes = reader.UnreadSegment.Slice(0, idx);
                reader.Advance(idx + 1);
                return true;
            }
            else
            {
                bool result = TryReadUntil(ref reader, out ReadOnlySequence<byte> sequence, delimiter);

                if (sequence.IsSingleSegment)
                {
                    bytes = sequence.First.Span;
                }
                else
                {
                    bytes = sequence.ToArray();
                }

                return result;
            }
        }

        public static bool TryReadUntil(ref BufferReader reader, out ReadOnlySequence<byte> bytes, byte delimiter)
        {
            // Option A
            /*SequencePosition start = reader.Position;
            int idx = reader.Sequence.Slice(start).PositionOf(delimiter);
            if (idx != -1)
            {
                bytes = reader.Sequence.Slice(start, idx);
                reader.Advance(idx + 1);
                return true;
            }
            else
            {
                bytes = default;
                return false;
            }*/

            // Option B
            /*SequencePosition start = reader.Position;
            SequencePosition? position = reader.Sequence.Slice(start).PositionOf(delimiter);
            if (position != null)
            {
                bytes = reader.Sequence.Slice(start, (SequencePosition)position);
                reader.Advance((int)bytes.Length + 1);
                return true;
            }
            else
            {
                bytes = default;
                return false;
            }*/

            BufferReader copy = reader;
            while (!reader.End)
            {
                if (reader.Peek() == delimiter)
                {
                    bytes = reader.Sequence.Slice(copy.Position, reader.Position);
                    reader.Advance(1);
                    return true;
                }
                reader.Advance(1);
            }
            reader = copy;
            bytes = default;
            return false;
        }

        public static bool TryReadUntilAny(ref BufferReader reader, out ReadOnlySpan<byte> bytes, ReadOnlySpan<byte> delimiters)
        {
            int idx = reader.UnreadSegment.IndexOfAny(delimiters);
            if (idx != -1)
            {
                bytes = reader.UnreadSegment.Slice(0, idx);
                reader.Advance(idx);
                return true;
            }
            else
            {
                bool result = TryReadUntil(ref reader, out ReadOnlySequence<byte> sequence, delimiters);

                if (sequence.IsSingleSegment)
                {
                    bytes = sequence.First.Span;
                }
                else
                {
                    bytes = sequence.ToArray();
                }

                return result;
            }
        }

        private static bool TryReadUntilAny(ref BufferReader reader, out ReadOnlySequence<byte> bytes, ReadOnlySpan<byte> delimiters)
        {
            BufferReader copy = reader;
            while (!reader.End)
            {
                byte b = (byte)reader.Peek();
                if (delimiters.IndexOf(b) != -1)
                {
                    bytes = reader.Sequence.Slice(copy.Position, reader.Position);
                    return true;
                }
                reader.Advance(1);
            }
            reader = copy;
            bytes = default;
            return false;
        }

        /*[MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int PositionOf<T>(in this ReadOnlySequence<T> source, T value) where T : IEquatable<T>
        {
            if (source.IsSingleSegment)
            {
                return source.First.Span.IndexOf(value);
            }
            else
            {
                return PositionOfMultiSegment(source, value);
            }
        }

        private static int PositionOfMultiSegment<T>(in ReadOnlySequence<T> source, T value) where T : IEquatable<T>
        {
            SequencePosition position = source.Start;
            SequencePosition result = position;
            int index = 0;
            while (source.TryGet(ref position, out ReadOnlyMemory<T> memory))
            {
                int localIndex = memory.Span.IndexOf(value);
                if (localIndex != -1)
                {
                    return index + localIndex;
                }
                else if (position.GetObject() == null)
                {
                    break;
                }
                else if (localIndex == -1)
                {
                    index += memory.Span.Length;
                }

                result = position;
            }

            return -1;
        }*/

        public static bool TryReadUntil(ref BufferReader reader, out ReadOnlySequence<byte> bytes, ReadOnlySpan<byte> delimiter)
        {
            if (delimiter.Length == 0)
            {
                bytes = default;
                return true;
            }

            int matched = 0;
            var copy = reader;
            var start = reader.Position;
            var end = reader.Position;
            while (!reader.End)
            {
                if (reader.Read() == delimiter[matched])
                {
                    matched++;
                }
                else
                {
                    end = reader.Position;
                    matched = 0;
                }
                if (matched >= delimiter.Length)
                {
                    bytes = reader.Sequence.Slice(start, end);
                    return true;
                }
            }
            reader = copy;
            bytes = default;
            return false;
        }
    }
}
