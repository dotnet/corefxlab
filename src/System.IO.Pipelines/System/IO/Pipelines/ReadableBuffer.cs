// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Buffers;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace System.IO.Pipelines
{
    /// <summary>
    /// Represents a buffer that can read a sequential series of bytes.
    /// </summary>
    [DebuggerDisplay("")]
    public readonly struct ReadableBuffer
    {
        internal readonly ReadCursor BufferStart;
        internal readonly ReadCursor BufferEnd;

        /// <summary>
        /// Length of the <see cref="ReadableBuffer"/> in bytes.
        /// </summary>
        public long Length => BufferStart.GetLength(BufferEnd);

        /// <summary>
        /// Determines if the <see cref="ReadableBuffer"/> is empty.
        /// </summary>
        public bool IsEmpty => Length == 0;

        /// <summary>
        /// Determins if the <see cref="ReadableBuffer"/> is a single <see cref="Memory{Byte}"/>.
        /// </summary>
        public bool IsSingleSpan => BufferStart.Segment == BufferEnd.Segment;

        public Memory<byte> First
        {
            get
            {
                BufferStart.TryGetBuffer(BufferEnd, out Memory<byte> first);
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
            BufferStart = start;
            BufferEnd = end;
        }

        internal ReadableBuffer(BufferSegment startSegment, int startIndex, BufferSegment endSegment, int endIndex)
        {
            BufferStart = new ReadCursor(startSegment, startIndex);
            BufferEnd = new ReadCursor(endSegment, endIndex);
        }

        internal ReadableBuffer(byte[] startSegment, int startIndex, int lenght)
        {
            BufferStart = new ReadCursor(startSegment, startIndex);
            BufferEnd = new ReadCursor(startSegment, startIndex + lenght);
        }

        private ReadableBuffer Clone(in ReadableBuffer buffer)
        {
            if (buffer.Start.Segment is BufferSegment bufferSegment)
            {
                var segmentHead = BufferSegment.Clone(bufferSegment, buffer.BufferStart.Index, buffer.BufferEnd.GetSegment(), buffer.BufferEnd.Index, out var segmentTail);
                return new ReadableBuffer(segmentHead, segmentHead.Start, segmentTail, segmentTail.End);
            }

            if (buffer.Start.Segment is byte[] array)
            {
                var arrayClone = new byte[buffer.End.Index - buffer.Start.Index];
                Array.Copy(array, buffer.Start.Index, arrayClone, 0, arrayClone.Length);
                return new ReadableBuffer(arrayClone, 0, arrayClone.Length);
            }

            PipelinesThrowHelper.ThrowNotSupportedException();
            return default;
        }

        /// <summary>
        /// Forms a slice out of the given <see cref="ReadableBuffer"/>, beginning at 'start', and is at most length bytes
        /// </summary>
        /// <param name="start">The index at which to begin this slice.</param>
        /// <param name="length">The length of the slice</param>
        public ReadableBuffer Slice(long start, long length)
        {
            var begin = BufferStart.Seek(start, BufferEnd, false);
            var end = begin.Seek(length, BufferEnd, false);
            return new ReadableBuffer(begin, end);
        }

        /// <summary>
        /// Forms a slice out of the given <see cref="ReadableBuffer"/>, beginning at 'start', ending at 'end' (inclusive).
        /// </summary>
        /// <param name="start">The index at which to begin this slice.</param>
        /// <param name="end">The end (inclusive) of the slice</param>
        public ReadableBuffer Slice(long start, ReadCursor end)
        {
            BufferEnd.BoundsCheck(end);
            var begin = BufferStart.Seek(start, end);
            return new ReadableBuffer(begin, end);
        }

        /// <summary>
        /// Forms a slice out of the given <see cref="ReadableBuffer"/>, beginning at 'start', ending at 'end' (inclusive).
        /// </summary>
        /// <param name="start">The starting (inclusive) <see cref="ReadCursor"/> at which to begin this slice.</param>
        /// <param name="end">The ending (inclusive) <see cref="ReadCursor"/> of the slice</param>
        public ReadableBuffer Slice(ReadCursor start, ReadCursor end)
        {
            BufferEnd.BoundsCheck(end);
            end.BoundsCheck(start);

            return new ReadableBuffer(start, end);
        }

        /// <summary>
        /// Forms a slice out of the given <see cref="ReadableBuffer"/>, beginning at 'start', and is at most length bytes
        /// </summary>
        /// <param name="start">The starting (inclusive) <see cref="ReadCursor"/> at which to begin this slice.</param>
        /// <param name="length">The length of the slice</param>
        public ReadableBuffer Slice(ReadCursor start, long length)
        {
            BufferEnd.BoundsCheck(start);

            var end = start.Seek(length, BufferEnd, false);

            return new ReadableBuffer(start, end);
        }

        /// <summary>
        /// Forms a slice out of the given <see cref="ReadableBuffer"/>, beginning at 'start', ending at the existing <see cref="ReadableBuffer"/>'s end.
        /// </summary>
        /// <param name="start">The starting (inclusive) <see cref="ReadCursor"/> at which to begin this slice.</param>
        public ReadableBuffer Slice(ReadCursor start)
        {
            BufferEnd.BoundsCheck(start);

            return new ReadableBuffer(start, BufferEnd);
        }

        /// <summary>
        /// Forms a slice out of the given <see cref="ReadableBuffer"/>, beginning at 'start', ending at the existing <see cref="ReadableBuffer"/>'s end.
        /// </summary>
        /// <param name="start">The start index at which to begin this slice.</param>
        public ReadableBuffer Slice(long start)
        {
            if (start == 0) return this;

            var begin = BufferStart.Seek(start, BufferEnd, false);
            return new ReadableBuffer(begin, BufferEnd);
        }

        /// <summary>
        /// This transfers ownership of the buffer from the <see cref="IPipeReader"/> to the caller of this method. Preserved buffers must be disposed to avoid
        /// memory leaks.
        /// </summary>
        public PreservedBuffer Preserve()
        {
            var buffer = Clone(this);
            return new PreservedBuffer(buffer);
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
            return cursor.Seek(count, BufferEnd, false);
        }

        /// <summary>
        /// Create a <see cref="ReadableBuffer"/> over an <see cref="IEnumerable{OwnedMemory{Byte}}"/>.
        /// </summary>
        public static PreservedBuffer Create(IEnumerable<OwnedMemory<byte>> data)
        {
            if (data == null)
            {
                PipelinesThrowHelper.ThrowArgumentNullException(ExceptionArgument.data);
            }

            BufferSegment first = null;
            BufferSegment segment = null;
            foreach (var ownedMemory in data)
            {
                var previous = segment;

                segment = new BufferSegment();
                segment.SetMemory(ownedMemory, 0, ownedMemory.Length);

                if (previous == null)
                {
                    first = segment;
                }
                else
                {
                    previous.SetNext(segment);
                }
            }

            if (first == null)
            {
                return default;
            }

            return new PreservedBuffer(new ReadableBuffer(first, 0, segment, segment.End));
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
        public static PreservedBuffer Create(OwnedMemory<byte> data, int offset, int length)
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

            var segment = new BufferSegment();
            segment.SetMemory(data, offset, length);

            return new PreservedBuffer(new ReadableBuffer(segment, 0, segment, segment.End));
        }
    }
}
