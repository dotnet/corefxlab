// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers;
using System.Collections.Sequences;
using System.Runtime.CompilerServices;

namespace System.IO.Pipelines
{
    public static class ReadCursorOperations
    {
        public static int Seek(ReadCursor begin, ReadCursor end, out ReadCursor result, byte byte0)
        {
            var enumerator = new ReadableBuffer(begin, end).GetEnumerator();
            while (enumerator.MoveNext())
            {
                var span = enumerator.Current.Span;

                int index = span.IndexOf(byte0);
                if (index != -1)
                {
                    result = enumerator.CreateCursor(index);
                    return span[index];
                }
            }

            result = end;
            return -1;
        }

        public static int Seek(ReadCursor begin, ReadCursor end, out ReadCursor result, byte byte0, byte byte1)
        {
            var enumerator = new ReadableBuffer(begin, end).GetEnumerator();
            while (enumerator.MoveNext())
            {
                var span = enumerator.Current.Span;
                int index = span.IndexOfAny(byte0, byte1);

                if (index != -1)
                {
                    result = enumerator.CreateCursor(index);
                    return span[index];
                }
            }

            result = end;
            return -1;
        }

        public static int Seek(ReadCursor begin, ReadCursor end, out ReadCursor result, byte byte0, byte byte1, byte byte2)
        {
            var enumerator = new ReadableBuffer(begin, end).GetEnumerator();
            while (enumerator.MoveNext())
            {
                var span = enumerator.Current.Span;
                int index = span.IndexOfAny(byte0, byte1, byte2);

                if (index != -1)
                {
                    result = enumerator.CreateCursor(index);
                    return span[index];
                }
            }

            result = end;
            return -1;
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
            while (TryGetBuffer(begin, end, out var memory, out begin))
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
