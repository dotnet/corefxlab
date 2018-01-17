// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers;
using System.Collections.Sequences;
using System.Runtime.CompilerServices;

namespace System.Buffers
{
    public readonly partial struct ReadOnlyBuffer<T>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static bool TryGetBuffer(SequencePosition begin, SequencePosition end, out ReadOnlyMemory<T> data, out SequencePosition next)
        {
            var segment = begin.Segment;

            switch (segment)
            {
                case null:
                    data = default;
                    next = default;
                    return false;

                case IBufferList<T> bufferSegment:
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
                                ThrowHelper.ThrowInvalidOperationException(ExceptionResource.EndPositionNotReached);
                            }

                            next = default;
                        }
                        else
                        {
                            next = new SequencePosition(nextSegment, 0);
                        }
                    }

                    data = bufferSegment.Memory.Slice(startIndex, endIndex - startIndex);

                    return true;


                case OwnedMemory<T> ownedMemory:
                    data = ownedMemory.Memory.Slice(begin.Index, end.Index - begin.Index);

                    if (segment != end.Segment)
                    {
                        ThrowHelper.ThrowInvalidOperationException(ExceptionResource.EndPositionNotReached);
                    }

                    next = default;
                    return true;

                case T[] array:
                    data = new Memory<T>(array, begin.Index, end.Index - begin.Index);

                    if (segment != end.Segment)
                    {
                        ThrowHelper.ThrowInvalidOperationException(ExceptionResource.EndPositionNotReached);
                    }
                    next = default;
                    return true;
            }

            ThrowHelper.ThrowNotSupportedException();
            next = default;
            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static SequencePosition Seek(SequencePosition begin, SequencePosition end, long bytes, bool checkEndReachable = true)
        {
            SequencePosition position;
            if (begin.Segment == end.Segment && end.Index - begin.Index >= bytes)
            {
                // end.Index >= bytes + Index and end.Index is int
                position = new SequencePosition(begin.Segment, begin.Index + (int)bytes);
            }
            else
            {
                position = SeekMultiSegment(begin, end, bytes, checkEndReachable);
            }

            return position;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static SequencePosition SeekMultiSegment(SequencePosition begin, SequencePosition end, long bytes, bool checkEndReachable)
        {
            SequencePosition result = default;
            var foundResult = false;
            var current = begin;
            while (TryGetBuffer(begin, end, out var memory, out begin))
            {
                // We need to loop up until the end to make sure start and end are connected
                // if end is not trusted
                if (!foundResult)
                {
                    // We would prefer to put position in the beginning of next segment
                    // then past the end of previous one, but only if next exists

                    if (memory.Length > bytes ||
                       (memory.Length == bytes && begin.Segment == null))
                    {
                        result = new SequencePosition(current.Segment, current.Index + (int)bytes);
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
                ThrowHelper.ThrowPositionOutOfBoundsException();
            }

            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static long GetLength(SequencePosition begin, SequencePosition end)
        {
            if (begin.Segment == null)
            {
                return 0;
            }

            var segment = begin.Segment;
            switch (segment)
            {
                case IBufferList<T> bufferSegment:
                    return GetLength(bufferSegment, begin.Index, (IBufferList<T>)end.Segment, end.Index);
                case T[] _:
                case OwnedMemory<T> _:
                    return end.Index - begin.Index;
            }

            ThrowHelper.ThrowInvalidOperationException(ExceptionResource.UnexpectedSegmentType);
            return default;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static long GetLength(
            IBufferList<T> start,
            int startIndex,
            IBufferList<T> endSegment,
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
        private static void BoundsCheck(SequencePosition end, SequencePosition newPosition)
        {
            switch (end.Segment)
            {
                case byte[] _:
                case OwnedMemory<byte> _:
                    if (newPosition.Index > end.Index)
                    {
                        ThrowHelper.ThrowPositionOutOfBoundsException();
                    }
                    return;
                case IBufferList<T> memoryList:
                    var segment = (IBufferList<T>)newPosition.Segment;
                    if (segment.RunningIndex - end.Index > memoryList.RunningIndex - newPosition.Index)
                    {
                        ThrowHelper.ThrowPositionOutOfBoundsException();
                    }
                    return;
                default:
                    ThrowHelper.ThrowPositionOutOfBoundsException();
                    return;
            }
        }

        private class ReadOnlyBufferSegment : IBufferList<T>
        {
            public Memory<T> Memory { get; set; }
            public IBufferList<T> Next { get; set; }
            public long RunningIndex { get; set; }
        }
    }
}
