// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Sequences;
using System.Runtime.CompilerServices;

namespace System.Buffers
{
    public readonly partial struct ReadOnlyBuffer<T>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal bool TryGetBuffer(SequenceIndex start, SequenceIndex end, out ReadOnlyMemory<T> data, out SequenceIndex next)
        {
            if (start.Segment == null)
            {
                data = default;
                next = default;
                return false;
            }

            var startIndex = start.Index;
            var endIndex = end.Index;
            var type = GetBufferType();

            startIndex = GetIndex(startIndex);
            endIndex = GetIndex(endIndex);

            switch (type)
            {
                case BufferType.MemoryList:
                    var bufferSegment = (IMemoryList<T>) start.Segment;
                    var currentEndIndex = bufferSegment.Memory.Length;

                    if (bufferSegment == end.Segment)
                    {
                        currentEndIndex = endIndex;
                        next = default;
                    }
                    else
                    {
                        var nextSegment = bufferSegment.Next;
                        if (nextSegment == null)
                        {
                            if (end.Segment != null)
                            {
                                ThrowHelper.ThrowInvalidOperationException(ExceptionResource.EndCursorNotReached);
                            }

                            next = default;
                        }
                        else
                        {
                            next = new SequenceIndex(nextSegment, 0);
                        }
                    }

                    data = bufferSegment.Memory.Slice(startIndex, currentEndIndex - startIndex);

                    return true;


                case BufferType.OwnedMemory:
                    var ownedMemory = (OwnedMemory<T>) start.Segment;
                    data = ownedMemory.Memory.Slice(startIndex, endIndex - startIndex);

                    if (ownedMemory != end.Segment)
                    {
                        ThrowHelper.ThrowInvalidOperationException(ExceptionResource.EndCursorNotReached);
                    }

                    next = default;
                    return true;

                case BufferType.Array:
                    var array = (T[]) start.Segment;
                    data = new Memory<T>(array, startIndex, endIndex - startIndex);

                    if (array != end.Segment)
                    {
                        ThrowHelper.ThrowInvalidOperationException(ExceptionResource.EndCursorNotReached);
                    }

                    next = default;
                    return true;
            }

            ThrowHelper.ThrowNotSupportedException();
            next = default;
            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal SequenceIndex Seek(SequenceIndex start, SequenceIndex end, int bytes, bool checkEndReachable = true)
        {
            var startIndex = start.Index;
            var endIndex = end.Index;
            var type = GetBufferType();

            startIndex = GetIndex(startIndex);
            endIndex = GetIndex(endIndex);

            switch (type)
            {
                case BufferType.MemoryList:
                    if (start.Segment == end.Segment && endIndex - startIndex >= bytes)
                    {
                        return new SequenceIndex(start.Segment, startIndex + bytes);
                    }
                    return SeekMultiSegment((IMemoryList<byte>) start.Segment, startIndex, (IMemoryList<byte>) end.Segment, endIndex, bytes, checkEndReachable);


                case BufferType.OwnedMemory:
                case BufferType.Array:
                    if (start.Segment != end.Segment)
                    {
                        ThrowHelper.ThrowInvalidOperationException(ExceptionResource.EndCursorNotReached);
                    }

                    if (endIndex - startIndex >= bytes)
                    {
                        return new SequenceIndex(start.Segment, startIndex + bytes);
                    }

                    ThrowHelper.ThrowCursorOutOfBoundsException();
                    return default;

                default:
                    ThrowHelper.ThrowInvalidOperationException(ExceptionResource.EndCursorNotReached);
                    return default;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal SequenceIndex Seek(SequenceIndex start, SequenceIndex end, long bytes, bool checkEndReachable = true)
        {
            var startIndex = start.Index;
            var endIndex = end.Index;
            var type = GetBufferType();

            startIndex = GetIndex(startIndex);
            endIndex = GetIndex(endIndex);

            switch (type)
            {

                case BufferType.MemoryList:
                    if (start.Segment == end.Segment && endIndex - startIndex >= bytes)
                    {
                        // end.Index >= bytes + Index and end.Index is int
                        return new SequenceIndex(start.Segment, startIndex + (int)bytes);
                    }
                    return SeekMultiSegment((IMemoryList<byte>) start.Segment, startIndex, (IMemoryList<byte>) end.Segment, endIndex, bytes, checkEndReachable);


                case BufferType.OwnedMemory:
                case BufferType.Array:
                    if (endIndex - startIndex >= bytes)
                    {
                        // end.Index >= bytes + Index and end.Index is int
                        return new SequenceIndex(start.Segment, startIndex + (int)bytes);
                    }

                    ThrowHelper.ThrowCursorOutOfBoundsException();
                    return default;
            }

            ThrowHelper.ThrowInvalidOperationException(ExceptionResource.EndCursorNotReached);
            return default;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static SequenceIndex SeekMultiSegment(IMemoryList<byte> start, int startIndex, IMemoryList<byte> end, int endPosition, long bytes, bool checkEndReachable)
        {
            SequenceIndex result = default;
            var foundResult = false;
            var current = start;
            var currentIndex = startIndex;

            while (current != null)
            {
                // We need to loop up until the end to make sure start and end are connected
                // if end is not trusted
                if (!foundResult)
                {
                    var memory = current.Memory;
                    var currentEnd = current == end ? endPosition : memory.Length;

                    memory = memory.Slice(0, currentEnd - currentIndex);
                    // We would prefer to put cursor in the beginning of next segment
                    // then past the end of previous one, but only if next exists

                    if (memory.Length > bytes ||
                       (memory.Length == bytes && current.Next == null))
                    {
                        result = new SequenceIndex(current, currentIndex + (int)bytes);
                        foundResult = true;
                        if (!checkEndReachable)
                        {
                            break;
                        }
                    }

                    bytes -= memory.Length;
                }

                if (current.Next == null && current != end)
                {
                    ThrowHelper.ThrowInvalidOperationException(ExceptionResource.EndCursorNotReached);
                }

                current = current.Next;
                currentIndex = 0;
            }

            if (!foundResult)
            {
                ThrowHelper.ThrowCursorOutOfBoundsException();
            }

            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private long GetLength(SequenceIndex start, SequenceIndex end)
        {
            var startIndex = start.Index;
            var endIndex = end.Index;
            var type = GetBufferType();

            startIndex = GetIndex(startIndex);
            endIndex = GetIndex(endIndex);

            switch (type)
            {
                case BufferType.MemoryList:
                    return GetLength((IMemoryList<T>) start.Segment, startIndex, (IMemoryList<T>)end.Segment, endIndex);

                case BufferType.OwnedMemory:
                case BufferType.Array:
                    return endIndex - startIndex;
            }

            ThrowHelper.ThrowInvalidOperationException(ExceptionResource.UnexpectedSegmentType);
            return default;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static long GetLength(
            IMemoryList<T> start,
            int startIndex,
            IMemoryList<T> endSegment,
            int endIndex)
        {
            if (start == endSegment)
            {
                return endIndex - startIndex;
            }

            return (endSegment.RunningIndex - start.Next.RunningIndex)
                   + (start.Memory.Length - startIndex)
                   + endIndex;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void BoundsCheck(SequenceIndex start, SequenceIndex newCursor)
        {
            var startIndex = start.Index;
            var endIndex = newCursor.Index;
            var type = GetBufferType();

            startIndex = GetIndex(startIndex);
            endIndex = GetIndex(endIndex);

            switch (type)
            {
                case BufferType.OwnedMemory:
                case BufferType.Array:
                    if (endIndex > startIndex)
                    {
                        ThrowHelper.ThrowCursorOutOfBoundsException();
                    }
                    return;
                case BufferType.MemoryList:
                    var segment = (IMemoryList<T>)newCursor.Segment;
                    var memoryList = (IMemoryList<T>) start.Segment;

                    if (segment.RunningIndex - startIndex > memoryList.RunningIndex - endIndex)
                    {
                        ThrowHelper.ThrowCursorOutOfBoundsException();
                    }
                    return;
                default:
                    ThrowHelper.ThrowCursorOutOfBoundsException();
                    return;
            }
        }

        private class ReadOnlyBufferSegment : IMemoryList<T>
        {
            public Memory<T> Memory { get; set; }
            public IMemoryList<T> Next { get; set; }
            public long RunningIndex { get; set; }
        }
    }
}
