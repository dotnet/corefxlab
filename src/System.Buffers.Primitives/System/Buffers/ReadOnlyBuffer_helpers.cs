// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers;
using System.Collections.Sequences;
using System.Runtime.CompilerServices;

namespace System.Buffers
{
    public readonly partial struct ReadOnlyBuffer 
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static bool TryGetBuffer(Position begin, Position end, out ReadOnlyMemory<byte> data, out Position next)
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
                                ThrowHelper.ThrowInvalidOperationException(ExceptionResource.EndCursorNotReached);
                            }

                            next = default;
                        }
                        else
                        {
                            next = new Position(nextSegment, 0);
                        }
                    }

                    data = bufferSegment.Memory.Slice(startIndex, endIndex - startIndex);

                    return true;


                case OwnedMemory<byte> ownedMemory:
                    data = ownedMemory.Memory.Slice(begin.Index, end.Index - begin.Index);

                    if (segment != end.Segment)
                    {
                        ThrowHelper.ThrowInvalidOperationException(ExceptionResource.EndCursorNotReached);
                    }

                    next = default;
                    return true;

                case byte[] array:
                    data = new Memory<byte>(array, begin.Index, end.Index - begin.Index);

                    if (segment != end.Segment)
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
        internal static Position Seek(Position begin, Position end, long bytes, bool checkEndReachable = true)
        {
            Position cursor;
            if (begin.Segment == end.Segment && end.Index - begin.Index >= bytes)
            {
                // end.Index >= bytes + Index and end.Index is int
                cursor = new Position(begin.Segment, begin.Index + (int)bytes);
            }
            else
            {
                cursor = SeekMultiSegment(begin, end, bytes, checkEndReachable);
            }

            return cursor;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static Position SeekMultiSegment(Position begin, Position end, long bytes, bool checkEndReachable)
        {
            Position result = default;
            var foundResult = false;
            var current = begin;
            while (TryGetBuffer(begin, end, out var memory, out begin))
            {
                // We need to loop up until the end to make sure start and end are connected
                // if end is not trusted
                if (!foundResult)
                {
                    // We would prefer to put cursor in the beginning of next segment
                    // then past the end of previous one, but only if next exists

                    if (memory.Length > bytes ||
                       (memory.Length == bytes && begin.Segment == null))
                    {
                        result = new Position(current.Segment, current.Index + (int)bytes);
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
                ThrowHelper.ThrowCursorOutOfBoundsException();
            }

            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static long GetLength(Position begin, Position end)
        {
            if (begin.Segment == null)
            {
                return 0;
            }

            var segment = begin.Segment;
            switch (segment)
            {
                case IMemoryList<byte> bufferSegment:
                    return GetLength(bufferSegment, begin.Index, end.GetSegment<IMemoryList<byte>>(), end.Index);
                case byte[] _:
                case OwnedMemory<byte> _:
                    return end.Index - begin.Index;
            }

            ThrowHelper.ThrowInvalidOperationException(ExceptionResource.UnexpectedSegmentType);
            return default;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static long GetLength(
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
        private static void BoundsCheck(Position end, Position newCursor)
        {
            switch (end.Segment)
            {
                case byte[] _:
                case OwnedMemory<byte> _:
                    if (newCursor.Index > end.Index)
                    {
                        ThrowHelper.ThrowCursorOutOfBoundsException();
                    }
                    return;
                case IMemoryList<byte> memoryList:
                    if(newCursor.GetSegment<IMemoryList<byte>>().VirtualIndex - end.Index > memoryList.VirtualIndex - newCursor.Index)
                    {
                        ThrowHelper.ThrowCursorOutOfBoundsException();
                    }
                    return;
                default:
                    ThrowHelper.ThrowCursorOutOfBoundsException();
                    return;
            }
        }

        private class ReadOnlyBufferSegment: IMemoryList<byte>
        {
            public Memory<byte> Memory { get; set; }
            public IMemoryList<byte> Next { get; set; }
            public long VirtualIndex { get; set; }
        }
    }
}
