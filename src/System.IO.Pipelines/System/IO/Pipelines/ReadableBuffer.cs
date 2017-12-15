// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Buffers;
using System.Collections.Sequences;
using System.Diagnostics;
using System.Text;

namespace System.IO.Pipelines
{
    /// <summary>
    /// Represents a buffer that can read a sequential series of bytes.
    /// </summary>
    public readonly struct ReadableBuffer
    {
        internal readonly ReadCursor BufferStart;
        internal readonly ReadCursor BufferEnd;

        /// <summary>
        /// Length of the <see cref="ReadableBuffer"/> in bytes.
        /// </summary>
        public long Length => ReadCursorOperations.GetLength(BufferStart, BufferEnd);

        /// <summary>
        /// Determines if the <see cref="ReadableBuffer"/> is empty.
        /// </summary>
        public bool IsEmpty => Length == 0;

        /// <summary>
        /// Determins if the <see cref="ReadableBuffer"/> is a single <see cref="Memory{Byte}"/>.
        /// </summary>
        public bool IsSingleSpan => BufferStart.Segment == BufferEnd.Segment;

        public ReadOnlyMemory<byte> First
        {
            get
            {
                ReadCursorOperations.TryGetBuffer(BufferStart, BufferEnd, out var first, out _);
                return first;
            }
        }

        /// <summary>
        /// A cursor to the start of the <see cref="ReadableBuffer"/>.
        /// </summary>
        public ReadCursor Start => BufferStart;

        /// <summary>
        /// A cursor to the end of the <see cref="ReadableBuffer"/>
        /// </summary>
        public ReadCursor End => BufferEnd;

        internal ReadableBuffer(ReadCursor start, ReadCursor end)
        {
            Debug.Assert(start.Segment != null);
            Debug.Assert(end.Segment != null);

            BufferStart = start;
            BufferEnd = end;
        }

        internal ReadableBuffer(IMemoryList<byte> startSegment, int startIndex, IMemoryList<byte> endSegment, int endIndex)
        {
            Debug.Assert(startSegment != null);
            Debug.Assert(endSegment != null);
            Debug.Assert(startSegment.Memory.Length >= startIndex);
            Debug.Assert(endSegment.Memory.Length >= endIndex);

            BufferStart = new ReadCursor(startSegment, startIndex);
            BufferEnd = new ReadCursor(endSegment, endIndex);
        }

        internal ReadableBuffer(byte[] startSegment, int startIndex, int length)
        {
            BufferStart = new ReadCursor(startSegment, startIndex);
            BufferEnd = new ReadCursor(startSegment, startIndex + length);
        }

        internal ReadableBuffer(OwnedMemory<byte> startSegment, int startIndex, int length)
        {
            BufferStart = new ReadCursor(startSegment, startIndex);
            BufferEnd = new ReadCursor(startSegment, startIndex + length);
        }

        /// <summary>
        /// Forms a slice out of the given <see cref="ReadableBuffer"/>, beginning at 'start', and is at most length bytes
        /// </summary>
        /// <param name="start">The index at which to begin this slice.</param>
        /// <param name="length">The length of the slice</param>
        public ReadableBuffer Slice(long start, long length)
        {
            var begin = ReadCursorOperations.Seek(BufferStart, BufferEnd, start, false);
            var end = ReadCursorOperations.Seek(begin, BufferEnd, length, false);
            return new ReadableBuffer(begin, end);
        }

        /// <summary>
        /// Forms a slice out of the given <see cref="ReadableBuffer"/>, beginning at 'start', ending at 'end' (inclusive).
        /// </summary>
        /// <param name="start">The index at which to begin this slice.</param>
        /// <param name="end">The end (inclusive) of the slice</param>
        public ReadableBuffer Slice(long start, ReadCursor end)
        {
            ReadCursorOperations.BoundsCheck(BufferEnd, end);
            var begin = ReadCursorOperations.Seek(BufferStart, end, start);
            return new ReadableBuffer(begin, end);
        }

        /// <summary>
        /// Forms a slice out of the given <see cref="ReadableBuffer"/>, beginning at 'start', ending at 'end' (inclusive).
        /// </summary>
        /// <param name="start">The starting (inclusive) <see cref="ReadCursor"/> at which to begin this slice.</param>
        /// <param name="end">The ending (inclusive) <see cref="ReadCursor"/> of the slice</param>
        public ReadableBuffer Slice(ReadCursor start, ReadCursor end)
        {
            ReadCursorOperations.BoundsCheck(BufferEnd, end);
            ReadCursorOperations.BoundsCheck(end, start);

            return new ReadableBuffer(start, end);
        }

        /// <summary>
        /// Forms a slice out of the given <see cref="ReadableBuffer"/>, beginning at 'start', and is at most length bytes
        /// </summary>
        /// <param name="start">The starting (inclusive) <see cref="ReadCursor"/> at which to begin this slice.</param>
        /// <param name="length">The length of the slice</param>
        public ReadableBuffer Slice(ReadCursor start, long length)
        {
            ReadCursorOperations.BoundsCheck(BufferEnd, start);

            var end = ReadCursorOperations.Seek(start, BufferEnd, length, false);

            return new ReadableBuffer(start, end);
        }

        /// <summary>
        /// Forms a slice out of the given <see cref="ReadableBuffer"/>, beginning at 'start', ending at the existing <see cref="ReadableBuffer"/>'s end.
        /// </summary>
        /// <param name="start">The starting (inclusive) <see cref="ReadCursor"/> at which to begin this slice.</param>
        public ReadableBuffer Slice(ReadCursor start)
        {
            ReadCursorOperations.BoundsCheck(BufferEnd, start);

            return new ReadableBuffer(start, BufferEnd);
        }

        /// <summary>
        /// Forms a slice out of the given <see cref="ReadableBuffer"/>, beginning at 'start', ending at the existing <see cref="ReadableBuffer"/>'s end.
        /// </summary>
        /// <param name="start">The start index at which to begin this slice.</param>
        public ReadableBuffer Slice(long start)
        {
            if (start == 0) return this;

            var begin = ReadCursorOperations.Seek(BufferStart, BufferEnd, start, false);
            return new ReadableBuffer(begin, BufferEnd);
        }

        /// <summary>
        /// Copy the <see cref="ReadableBuffer"/> to the specified <see cref="Span{Byte}"/>.
        /// </summary>
        /// <param name="destination">The destination <see cref="Span{Byte}"/>.</param>
        public void CopyTo(Span<byte> destination)
        {
            if (Length > destination.Length)
            {
                PipelinesThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.destination);
            }

            foreach (var buffer in this)
            {
                buffer.Span.CopyTo(destination);
                destination = destination.Slice(buffer.Length);
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

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            foreach (var buffer in this)
            {
                SpanLiteralExtensions.AppendAsLiteral(buffer.Span, sb);
            }
            return sb.ToString();
        }

        /// <summary>
        /// Returns an enumerator over the <see cref="ReadableBuffer"/>
        /// </summary>
        public BufferEnumerator GetEnumerator()
        {
            return new BufferEnumerator(BufferStart, BufferEnd);
        }

        public ReadCursor Move(ReadCursor cursor, long count)
        {
            if (count < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(count));
            }
            return ReadCursorOperations.Seek(cursor, BufferEnd, count, false);
        }

        /// <summary>
        /// Create a <see cref="ReadableBuffer"/> over an array.
        /// </summary>
        public static ReadableBuffer Create(byte[] data, int offset, int length)
        {
            if (data == null)
            {
                PipelinesThrowHelper.ThrowArgumentNullException(ExceptionArgument.data);
            }

            return new ReadableBuffer(data, offset, length);
        }

        /// <summary>
        /// Create a <see cref="ReadableBuffer"/> over an array.
        /// </summary>
        public static ReadableBuffer Create(byte[] data)
        {
            if (data == null)
            {
                PipelinesThrowHelper.ThrowArgumentNullException(ExceptionArgument.data);
            }

            return new ReadableBuffer(data, 0, data.Length);
        }

        /// <summary>
        /// Create a <see cref="ReadableBuffer"/> over an <see cref="OwnedMemory{Byte}"/>.
        /// </summary>
        public static ReadableBuffer Create(OwnedMemory<byte> data, int offset, int length)
        {
            if (data == null)
            {
                PipelinesThrowHelper.ThrowArgumentNullException(ExceptionArgument.data);
            }

            if (offset < 0)
            {
                PipelinesThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.offset);
            }

            if (length < 0 || length > data.Length - offset)
            {
                PipelinesThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.length);
            }

            return new ReadableBuffer(data, offset, length);
        }
    }
}
