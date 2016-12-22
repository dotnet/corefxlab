// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Buffers;
using System.Collections.Sequences;
using System.Numerics;
using System.Text;

namespace System.IO.Pipelines
{
    /// <summary>
    /// Represents a buffer that can read a sequential series of bytes.
    /// </summary>
    public struct ReadableBuffer : ISequence<ReadOnlyMemory<byte>>
    {
        private static readonly int VectorWidth = Vector<byte>.Count;

        private Memory<byte> _first;

        private ReadCursor _start;
        private ReadCursor _end;
        private int _length;

        /// <summary>
        /// Length of the <see cref="ReadableBuffer"/> in bytes.
        /// </summary>
        public int Length => _length >= 0 ? _length : GetLength();

        int? ISequence<ReadOnlyMemory<byte>>.Length => Length;

        /// <summary>
        /// Determines if the <see cref="ReadableBuffer"/> is empty.
        /// </summary>
        public bool IsEmpty => Length == 0;

        /// <summary>
        /// Determins if the <see cref="ReadableBuffer"/> is a single <see cref="Memory{Byte}"/>.
        /// </summary>
        public bool IsSingleSpan => _start.Segment == _end.Segment;

        /// <summary>
        /// The first <see cref="Memory{Byte}"/> in the <see cref="ReadableBuffer"/>.
        /// </summary>
        public Memory<byte> First => _first;

        /// <summary>
        /// A cursor to the start of the <see cref="ReadableBuffer"/>.
        /// </summary>
        public ReadCursor Start => _start;

        /// <summary>
        /// A cursor to the end of the <see cref="ReadableBuffer"/>
        /// </summary>
        public ReadCursor End => _end;

        internal ReadableBuffer(ReadCursor start, ReadCursor end)
        {
            _start = start;
            _end = end;

            start.TryGetBuffer(end, out _first, out start);

            _length = -1;
        }

        private ReadableBuffer(ref ReadableBuffer buffer)
        {
            var begin = buffer._start;
            var end = buffer._end;

            BufferSegment segmentTail;
            var segmentHead = BufferSegment.Clone(begin, end, out segmentTail);

            begin = new ReadCursor(segmentHead);
            end = new ReadCursor(segmentTail, segmentTail.End);

            _start = begin;
            _end = end;

            _length = buffer._length;

            begin.TryGetBuffer(end, out _first, out begin);
        }

        /// <summary>
        /// Searches for 2 sequential bytes in the <see cref="ReadableBuffer"/> and returns a sliced <see cref="ReadableBuffer"/> that
        /// contains all data up to and excluding the first byte, and a <see cref="ReadCursor"/> that points to the second byte.
        /// </summary>
        /// <param name="b1">The first byte to search for</param>
        /// <param name="b2">The second byte to search for</param>
        /// <param name="slice">A <see cref="ReadableBuffer"/> slice that contains all data up to and excluding the first byte.</param>
        /// <param name="cursor">A <see cref="ReadCursor"/> that points to the second byte</param>
        /// <returns>True if the byte sequence was found, false if not found</returns>
        public unsafe bool TrySliceTo(byte b1, byte b2, out ReadableBuffer slice, out ReadCursor cursor)
        {
            // use address of ushort rather than stackalloc as the inliner won't inline functions with stackalloc
            ushort twoBytes;
            byte* byteArray = (byte*)&twoBytes;
            byteArray[0] = b1;
            byteArray[1] = b2;
            return TrySliceTo(new Span<byte>(byteArray, 2), out slice, out cursor);
        }

        /// <summary>
        /// Searches for a span of bytes in the <see cref="ReadableBuffer"/> and returns a sliced <see cref="ReadableBuffer"/> that
        /// contains all data up to and excluding the first byte of the span, and a <see cref="ReadCursor"/> that points to the last byte of the span.
        /// </summary>
        /// <param name="span">The <see cref="Span{Byte}"/> byte to search for</param>
        /// <param name="slice">A <see cref="ReadableBuffer"/> that matches all data up to and excluding the first byte</param>
        /// <param name="cursor">A <see cref="ReadCursor"/> that points to the second byte</param>
        /// <returns>True if the byte sequence was found, false if not found</returns>
        public bool TrySliceTo(Span<byte> span, out ReadableBuffer slice, out ReadCursor cursor)
        {
            var result = false;
            var buffer = this;
            do
            {
                // Find the first byte
                if (!buffer.TrySliceTo(span[0], out slice, out cursor))
                {
                    break;
                }

                // Move the buffer to where you fonud the first byte then search for the next byte
                buffer = buffer.Slice(cursor);

                if (buffer.StartsWith(span))
                {
                    slice = Slice(_start, cursor);
                    result = true;
                    break;
                }

                // REVIEW: We need to check the performance of Slice in a loop like this
                // Not a match so skip(1) 
                buffer = buffer.Slice(1);
            } while (!buffer.IsEmpty);

            return result;
        }

        /// <summary>
        /// Searches for a byte in the <see cref="ReadableBuffer"/> and returns a sliced <see cref="ReadableBuffer"/> that
        /// contains all data up to and excluding the byte, and a <see cref="ReadCursor"/> that points to the byte.
        /// </summary>
        /// <param name="b1">The first byte to search for</param>
        /// <param name="slice">A <see cref="ReadableBuffer"/> slice that contains all data up to and excluding the first byte.</param>
        /// <param name="cursor">A <see cref="ReadCursor"/> that points to the second byte</param>
        /// <returns>True if the byte sequence was found, false if not found</returns>
        public bool TrySliceTo(byte b1, out ReadableBuffer slice, out ReadCursor cursor)
        {
            if (IsEmpty)
            {
                slice = default(ReadableBuffer);
                cursor = default(ReadCursor);
                return false;
            }

            var byte0Vector = CommonVectors.GetVector(b1);

            var seek = 0;

            foreach (var memory in this)
            {
                var currentSpan = memory.Span;
                var found = false;

                if (Vector.IsHardwareAccelerated)
                {
                    while (currentSpan.Length >= VectorWidth)
                    {
                        var data = currentSpan.Read<Vector<byte>>();
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
                    cursor = _start.Seek(seek);
                    slice = Slice(_start, cursor);
                    return true;
                }
            }

            slice = default(ReadableBuffer);
            cursor = default(ReadCursor);
            return false;
        }

        /// <summary>
        /// Forms a slice out of the given <see cref="ReadableBuffer"/>, beginning at 'start', and is at most length bytes
        /// </summary>
        /// <param name="start">The index at which to begin this slice.</param>
        /// <param name="length">The length of the slice</param>
        public ReadableBuffer Slice(int start, int length)
        {
            var begin = _start.Seek(start);
            return Slice(begin, begin.Seek(length));
        }

        /// <summary>
        /// Forms a slice out of the given <see cref="ReadableBuffer"/>, beginning at 'start', ending at 'end' (inclusive).
        /// </summary>
        /// <param name="start">The index at which to begin this slice.</param>
        /// <param name="end">The end (inclusive) of the slice</param>
        public ReadableBuffer Slice(int start, ReadCursor end)
        {
            return Slice(_start.Seek(start), end);
        }

        /// <summary>
        /// Forms a slice out of the given <see cref="ReadableBuffer"/>, beginning at 'start', ending at 'end' (inclusive).
        /// </summary>
        /// <param name="start">The starting (inclusive) <see cref="ReadCursor"/> at which to begin this slice.</param>
        /// <param name="end">The ending (inclusive) <see cref="ReadCursor"/> of the slice</param>
        public ReadableBuffer Slice(ReadCursor start, ReadCursor end)
        {
            return new ReadableBuffer(start, end);
        }

        /// <summary>
        /// Forms a slice out of the given <see cref="ReadableBuffer"/>, beginning at 'start', and is at most length bytes
        /// </summary>
        /// <param name="start">The starting (inclusive) <see cref="ReadCursor"/> at which to begin this slice.</param>
        /// <param name="length">The length of the slice</param>
        public ReadableBuffer Slice(ReadCursor start, int length)
        {
            return Slice(start, start.Seek(length));
        }

        /// <summary>
        /// Forms a slice out of the given <see cref="ReadableBuffer"/>, beginning at 'start', ending at the existing <see cref="ReadableBuffer"/>'s end.
        /// </summary>
        /// <param name="start">The starting (inclusive) <see cref="ReadCursor"/> at which to begin this slice.</param>
        public ReadableBuffer Slice(ReadCursor start)
        {
            return new ReadableBuffer(start, _end);
        }

        /// <summary>
        /// Forms a slice out of the given <see cref="ReadableBuffer"/>, beginning at 'start', ending at the existing <see cref="ReadableBuffer"/>'s end.
        /// </summary>
        /// <param name="start">The start index at which to begin this slice.</param>
        public ReadableBuffer Slice(int start)
        {
            if (start == 0) return this;

            return new ReadableBuffer(_start.Seek(start), _end);
        }

        /// <summary>
        /// Returns the first byte in the <see cref="ReadableBuffer"/>.
        /// </summary>
        /// <returns>-1 if the buffer is empty, the first byte otherwise.</returns>
        public int Peek()
        {
            if (IsEmpty)
            {
                return -1;
            }

            var span = First.Span;
            return span[0];
        }

        /// <summary>
        /// This transfers ownership of the buffer from the <see cref="IPipelineReader"/> to the caller of this method. Preserved buffers must be disposed to avoid
        /// memory leaks.
        /// </summary>
        public PreservedBuffer Preserve()
        {
            var buffer = new ReadableBuffer(ref this);
            return new PreservedBuffer(ref buffer);
        }

        /// <summary>
        /// Copy the <see cref="ReadableBuffer"/> to the specified <see cref="Span{Byte}"/>.
        /// </summary>
        /// <param name="destination">The destination <see cref="Span{Byte}"/>.</param>
        public void CopyTo(Span<byte> destination)
        {
            if (Length > destination.Length)
            {
                ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.destination);
            }

            foreach (var memory in this)
            {
                memory.Span.CopyTo(destination);
                destination = destination.Slice(memory.Length);
            }
        }

        /// <summary>
        /// Converts the <see cref="ReadableBuffer"/> to a <see cref="T:byte[]"/>
        /// </summary>
        public byte[] ToArray()
        {
            var buffer = new byte[Length];
            CopyTo(buffer);
            return buffer;
        }

        private int GetLength()
        {
            var begin = _start;
            var length = begin.GetLength(_end);
            _length = length;
            return length;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            foreach (var memory in this)
            {
                int length = memory.Span.Length;
                for (int i = 0; i < length; i++)
                {
                    char c = (char)memory.Span[i];
                    AppendCharLiteral(c, sb);
                }
            }
            return sb.ToString();
        }

        internal static void AppendCharLiteral(char c, StringBuilder sb)
        {
            switch (c)
            {
                case '\'':
                    sb.Append(@"\'");
                    break;
                case '\"':
                    sb.Append("\\\"");
                    break;
                case '\\':
                    sb.Append(@"\\");
                    break;
                case '\0':
                    sb.Append(@"\0");
                    break;
                case '\a':
                    sb.Append(@"\a");
                    break;
                case '\b':
                    sb.Append(@"\b");
                    break;
                case '\f':
                    sb.Append(@"\f");
                    break;
                case '\n':
                    sb.Append(@"\n");
                    break;
                case '\r':
                    sb.Append(@"\r");
                    break;
                case '\t':
                    sb.Append(@"\t");
                    break;
                case '\v':
                    sb.Append(@"\v");
                    break;
                default:
                    // ASCII printable character
                    if (!char.IsControl(c))
                    {
                        sb.Append(c);
                        // As UTF16 escaped character
                    }
                    else
                    {
                        sb.Append(@"\u");
                        sb.Append(((int) c).ToString("x4"));
                    }
                    break;
            }
        }

        /// <summary>
        /// Returns an enumerator over the <see cref="ReadableBuffer"/>
        /// </summary>
        public MemoryEnumerator GetEnumerator()
        {
            return new MemoryEnumerator(_start, _end);
        }

        /// <summary>
        /// Checks to see if the <see cref="ReadableBuffer"/> starts with the specified <see cref="Span{Byte}"/>.
        /// </summary>
        /// <param name="value">The <see cref="Span{Byte}"/> to compare to</param>
        /// <returns>True if the bytes StartsWith, false if not</returns>
        public bool StartsWith(Span<byte> value)
        {
            if (Length < value.Length)
            {
                // just nope
                return false;
            }

            return Slice(0, value.Length).Equals(value);
        }

        /// <summary>
        /// Checks to see if the <see cref="ReadableBuffer"/> is Equal to the specified <see cref="Span{Byte}"/>.
        /// </summary>
        /// <param name="value">The <see cref="Span{Byte}"/> to compare to</param>
        /// <returns>True if the bytes are equal, false if not</returns>
        public bool Equals(Span<byte> value)
        {
            if (value.Length != Length)
            {
                return false;
            }

            if (IsSingleSpan)
            {
                return First.Span.BlockEquals(value);
            }

            foreach (var memory in this)
            {
                var compare = value.Slice(0, memory.Length);
                if (!memory.Span.BlockEquals(compare))
                {
                    return false;
                }

                value = value.Slice(memory.Length);
            }
            return true;
        }

        internal void ClearCursors()
        {
            _start = default(ReadCursor);
            _end = default(ReadCursor);
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

        /// <summary>
        /// Create a <see cref="ReadableBuffer"/> over an array.
        /// </summary>
        public static ReadableBuffer Create(byte[] data)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            return Create(data, 0, data.Length);
        }

        /// <summary>
        /// Create a <see cref="ReadableBuffer"/> over an array.
        /// </summary>
        public static ReadableBuffer Create(byte[] data, int offset, int length)
        {
            if (data == null)
            {
                ThrowHelper.ThrowArgumentNullException(ExceptionArgument.data);
            }

            if (offset < 0)
            {
                ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.offset);
            }

            if (length < 0 || (offset + length) > data.Length)
            {
                ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.length);
            }

            var buffer = new OwnedArray<byte>(data);
            var segment = new BufferSegment(buffer);
            segment.Start = offset;
            segment.End = offset + length;
            return new ReadableBuffer(new ReadCursor(segment, offset), new ReadCursor(segment, offset + length));
        }

        bool ISequence<ReadOnlyMemory<byte>>.TryGet(ref Position position, out ReadOnlyMemory<byte> item, bool advance)
        {
            if (position == Position.First)
            {
                item = First.Slice(_start.Index);
                if (advance)
                {
                    if (_start.IsEnd)
                    {
                        position = Position.AfterLast;
                    }
                    else
                    {
                        position.ObjectPosition = _start.Segment.Next;
                    }
                }
                return true;
            }
            else if (position == Position.BeforeFirst)
            {
                if (advance) position = Position.First;
                item = default(ReadOnlyMemory<byte>);
                return false;
            }
            else if (position == Position.AfterLast)
            {
                item = default(ReadOnlyMemory<byte>);
                return false;
            }

            var currentSegment = (BufferSegment)position.ObjectPosition;
            if (advance)
            {
                position.ObjectPosition = currentSegment.Next;
                if (position.ObjectPosition == null)
                {
                    position = Position.AfterLast;
                }
            }
            if (currentSegment == _end.Segment)
            {
                item = currentSegment.Memory.Slice(currentSegment.Start, _end.Index);
            }
            else
            {
                item = currentSegment.Memory.Slice(currentSegment.Start, currentSegment.End);
            }
            return true;
        }

        SequenceEnumerator<ReadOnlyMemory<byte>> ISequence<ReadOnlyMemory<byte>>.GetEnumerator()
        {
            return new SequenceEnumerator<ReadOnlyMemory<byte>>(this);
        }
    }
}
