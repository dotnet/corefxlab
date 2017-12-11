// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Buffers;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
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
        public long Length => GetLength(BufferStart, BufferEnd);

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
                TryGetBuffer(BufferStart, BufferEnd, out var first, out _);
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
            var begin = Seek(BufferStart, BufferEnd, start, false);
            var end = Seek(begin, BufferEnd, length, false);
            return new ReadableBuffer(begin, end);
        }

        /// <summary>
        /// Forms a slice out of the given <see cref="ReadableBuffer"/>, beginning at 'start', ending at 'end' (inclusive).
        /// </summary>
        /// <param name="start">The index at which to begin this slice.</param>
        /// <param name="end">The end (inclusive) of the slice</param>
        public ReadableBuffer Slice(long start, ReadCursor end)
        {
            BoundsCheck(BufferEnd, end);
            var begin = Seek(BufferStart, end, start);
            return new ReadableBuffer(begin, end);
        }

        /// <summary>
        /// Forms a slice out of the given <see cref="ReadableBuffer"/>, beginning at 'start', ending at 'end' (inclusive).
        /// </summary>
        /// <param name="start">The starting (inclusive) <see cref="ReadCursor"/> at which to begin this slice.</param>
        /// <param name="end">The ending (inclusive) <see cref="ReadCursor"/> of the slice</param>
        public ReadableBuffer Slice(ReadCursor start, ReadCursor end)
        {
            BoundsCheck(BufferEnd, end);
            BoundsCheck(end, start);

            return new ReadableBuffer(start, end);
        }

        /// <summary>
        /// Forms a slice out of the given <see cref="ReadableBuffer"/>, beginning at 'start', and is at most length bytes
        /// </summary>
        /// <param name="start">The starting (inclusive) <see cref="ReadCursor"/> at which to begin this slice.</param>
        /// <param name="length">The length of the slice</param>
        public ReadableBuffer Slice(ReadCursor start, long length)
        {
            BoundsCheck(BufferEnd, start);

            var end = Seek(start, BufferEnd, length, false);

            return new ReadableBuffer(start, end);
        }

        /// <summary>
        /// Forms a slice out of the given <see cref="ReadableBuffer"/>, beginning at 'start', ending at the existing <see cref="ReadableBuffer"/>'s end.
        /// </summary>
        /// <param name="start">The starting (inclusive) <see cref="ReadCursor"/> at which to begin this slice.</param>
        public ReadableBuffer Slice(ReadCursor start)
        {
            BoundsCheck(BufferEnd, start);

            return new ReadableBuffer(start, BufferEnd);
        }

        /// <summary>
        /// Forms a slice out of the given <see cref="ReadableBuffer"/>, beginning at 'start', ending at the existing <see cref="ReadableBuffer"/>'s end.
        /// </summary>
        /// <param name="start">The start index at which to begin this slice.</param>
        public ReadableBuffer Slice(long start)
        {
            if (start == 0) return this;

            var begin = Seek(BufferStart, BufferEnd, start, false);
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
            return Seek(cursor, BufferEnd, count, false);
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static long GetLength(ReadCursor begin, ReadCursor end)
        {
            if (begin.IsDefault)
            {
                return 0;
            }

            var segment = begin.Segment;
            switch (segment)
            {
                case IMemoryList<byte> bufferSegment:
                    return GetLength(bufferSegment, begin.Index, end.Get<IMemoryList<byte>>(), end.Index);
                case byte[] _:
                case OwnedMemory<byte> _:
                    return end.Index - begin.Index;
            }

            PipelinesThrowHelper.ThrowInvalidOperationException(ExceptionResource.UnexpectedSegmentType);
            return default;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static long GetLength(
            IMemoryList<byte> start,
            int startIndex,
            IMemoryList<byte> endSegment,
            int endIndex)
        {
            if (start == endSegment)
            {
                return endIndex - startIndex;
            }

            return (endSegment.VirtualIndex - start.Next.VirtualIndex)
                   + (start.Memory.Length - startIndex)
                   + endIndex;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static ReadCursor Seek(ReadCursor begin, ReadCursor end, long bytes, bool checkEndReachable = true)
        {
            ReadCursor cursor;
            if (begin.Segment == end.Segment && end.Index - begin.Index >= bytes)
            {
                // end.Index >= bytes + Index and end.Index is int
                cursor = new ReadCursor(begin.Segment, begin.Index + (int)bytes);
            }
            else
            {
                cursor = SeekMultiSegment(begin, end, bytes, checkEndReachable);
            }

            return cursor;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static ReadCursor SeekMultiSegment(ReadCursor begin, ReadCursor end, long bytes,  bool checkEndReachable)
        {
            ReadCursor result = default;
            var foundResult = false;
            var current = begin;
            while (TryGetBuffer(current, end, out var memory, out begin))
            {
                // We need to loop up until the end to make sure start and end are connected
                // if end is not trusted
                if (!foundResult)
                {
                    // We would prefer to put cursor in the beginning of next segment
                    // then past the end of previous one, but only if next exists

                    if (memory.Length > bytes ||
                       (memory.Length == bytes && begin.IsDefault))
                    {

                        result = new ReadCursor(current.Segment, current.Index + (int)bytes);
                        foundResult = true;
                        if (!checkEndReachable)
                        {
                            break;
                        }
                    }

                    bytes -= memory.Length;
                }
                current = begin;
            }

            if (!foundResult)
            {
                PipelinesThrowHelper.ThrowCursorOutOfBoundsException();
            }

            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void BoundsCheck(ReadCursor end, ReadCursor newCursor)
        {
            switch (end.Segment)
            {
                case byte[] _ :
                case OwnedMemory<byte> _ :
                    if (newCursor.Index > end.Index)
                    {
                        PipelinesThrowHelper.ThrowCursorOutOfBoundsException();
                    }
                    return;
                case IMemoryList<byte> memoryList:
                    if (!GreaterOrEqual(memoryList, end.Index, newCursor.Get<IMemoryList<byte>>(), newCursor.Index))
                    {
                        PipelinesThrowHelper.ThrowCursorOutOfBoundsException();
                    }
                    return;
                default:
                    PipelinesThrowHelper.ThrowCursorOutOfBoundsException();
                    return;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static bool TryGetBuffer(ReadCursor begin, ReadCursor end, out ReadOnlyMemory<byte> data, out ReadCursor next)
        {
            var segment = begin.Segment;

            switch (segment)
            {
                case null:
                    data = default;
                    next = default;
                    return false;

                case IMemoryList<byte> bufferSegment:
                    var startIndex = begin.Index;
                    var endIndex = bufferSegment.Memory.Length;

                    if (segment == end.Segment)
                    {
                        endIndex = end.Index;
                        next = default;
                    }
                    else
                    {
                        var nextSegment = bufferSegment.Next;
                        if (nextSegment == null)
                        {
                            if (end.Segment != null)
                            {
                                PipelinesThrowHelper.ThrowInvalidOperationException(ExceptionResource.EndCursorNotReached);
                            }

                            next = default;
                        }
                        else
                        {
                            next = new ReadCursor(nextSegment, 0);
                        }
                    }

                    data = bufferSegment.Memory.Slice(startIndex, endIndex - startIndex);

                    return true;


                case OwnedMemory<byte> ownedMemory:
                    data = ownedMemory.Memory.Slice(begin.Index, end.Index - begin.Index);

                    if (segment != end.Segment)
                    {
                         PipelinesThrowHelper.ThrowInvalidOperationException(ExceptionResource.EndCursorNotReached);
                    }

                    next = default;
                    return true;

                case byte[] array:
                    data = new Memory<byte>(array, begin.Index, end.Index - begin.Index);

                    if (segment != end.Segment)
                    {
                        PipelinesThrowHelper.ThrowInvalidOperationException(ExceptionResource.EndCursorNotReached);
                    }
                    next = default;
                    return true;
            }

            PipelinesThrowHelper.ThrowNotSupportedException();
            next = default;
            return false;
        }

        internal static bool GreaterOrEqual(IMemoryList<byte> start, int startIndex, IMemoryList<byte> end, int endIndex)
        {
            // other.Segment.RunningLength + other.Index  - other.Segment.Start <= Segment.RunningLength + Index- Segment.Start
            // fliped to avoid overflows

            return end.VirtualIndex - startIndex <= start.VirtualIndex - endIndex;
        }
    }
}
