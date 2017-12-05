// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;

namespace System.IO.Pipelines
{
    [DebuggerDisplay("{Segment}[{Index}]")]
    public readonly struct ReadCursor : IEquatable<ReadCursor>
    {
        internal readonly object Segment;
        internal readonly int Index;

        internal ReadCursor(BufferSegment segment)
        {
            Segment = segment;
            Index = segment?.Start ?? 0;
        }

        internal ReadCursor(object segment, int index)
        {
            Segment = segment;
            Index = index;
        }

        internal ReadCursor(BufferSegment segment, int index)
        {
            Segment = segment;
            Index = index;
        }

        internal ReadCursor(byte[] array, int index)
        {
            Segment = array;
            Index = index;
        }

        internal bool IsDefault => Segment == null;

        internal bool IsEnd
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                var segment = Segment;

                if (segment == null)
                {
                    return true;
                }

                if (segment is BufferSegment bufferSegment)
                {
                    if (Index < bufferSegment.End)
                    {
                        return false;
                    }

                    return bufferSegment.Next == null ||
                           IsEndMultiSegment(bufferSegment);
                }

                if (segment is byte[] array)
                {
                    return Index >= array.Length;
                }

                ThrowWrongType();
                return default;
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private void ThrowWrongType()
        {
            throw new InvalidOperationException();
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static bool IsEndMultiSegment(BufferSegment segment)
        {
            segment = segment.Next;
            while (segment != null)
            {
                if (segment.Start < segment.End)
                {
                    return false; // subsequent block has data - IsEnd is false
                }
                segment = segment.Next;
            }
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal long GetLength(ReadCursor end)
        {
            if (IsDefault)
            {
                return 0;
            }

            var segment = Segment;
            if (segment is BufferSegment bufferSegment)
            {
                return GetLength(bufferSegment, Index, (BufferSegment)end.Segment, end.Index);
            }

            if (segment is byte[])
            {
                return end.Index - Index;
            }

            ThrowWrongType();
            return default;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static long GetLength(
            BufferSegment start,
            int startIndex,
            BufferSegment endSegment,
            int endIndex)
        {
            if (start == endSegment)
            {
                return endIndex - startIndex;
            }

            return (endSegment.RunningLength - start.Next.RunningLength)
                   + (start.End - startIndex)
                   + (endIndex - endSegment.Start);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal ReadCursor Seek(long bytes, ReadCursor end, bool checkEndReachable = true)
        {
            ReadCursor cursor;
            if (Segment == end.Segment && end.Index - Index >= bytes)
            {
                // end.Index >= bytes + Index and end.Index is int
                cursor = new ReadCursor(Segment, Index + (int)bytes);
            }
            else
            {
                cursor = SeekMultiSegment(bytes, end, checkEndReachable);
            }

            return cursor;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private ReadCursor SeekMultiSegment(long bytes, ReadCursor end, bool checkEndReachable)
        {
            ReadCursor result = default;
            bool foundResult = false;

            var enumerator = new BufferEnumerator(this, end);
            Memory<byte> memory;
            while (enumerator.MoveNext())
            {
                memory = enumerator.Current;
                // We need to loop up until the end to make sure start and end are connected
                // if end is not trusted
                if (!foundResult)
                {
                    if (memory.Length >= bytes)
                    {
                        result = enumerator.CreateCursor((int)bytes);
                        foundResult = true;
                        if (!checkEndReachable)
                        {
                            break;
                        }
                    }
                    else
                    {
                        bytes -= memory.Length;
                    }
                }

            }
            if (!foundResult)
            {
                ThrowOutOfBoundsException();
            }

            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void BoundsCheck(ReadCursor newCursor)
        {
            if (ReferenceEquals(Segment, newCursor.Segment))
            {
                if (newCursor.Index > Index)
                {
                    ThrowOutOfBoundsException();
                }
            }
            else
            {
                if (!GreaterOrEqual(GetSegment(), Index, newCursor.GetSegment(), newCursor.Index))
                {
                    ThrowOutOfBoundsException();
                }
            }
        }

        private static void ThrowOutOfBoundsException()
        {
            throw new InvalidOperationException("Cursor is out of bounds");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal bool TryGetBuffer(ReadCursor end, out Memory<byte> data)
        {
            if (IsDefault)
            {
                data = Memory<byte>.Empty;
                return false;
            }

            var segment = Segment;
            var index = Index;

            if (segment is BufferSegment bufferSegment)
            {
                if (end.Segment == segment)
                {
                    var following = end.Index - index;

                    if (following > 0)
                    {
                        data = bufferSegment.Memory.Slice(index, following);
                        return true;
                    }

                    data = Memory<byte>.Empty;
                    return false;
                }
                else
                {
                    return TryGetBufferMultiBlock(bufferSegment, Index, (BufferSegment)end.Segment, end.Index, out data);
                }
            }

            if (segment is byte[] array)
            {
                data = ((Memory<byte>) array).Slice(Index, end.Index - Index);
                return true;
            }

            ThrowWrongType();
            return default;
        }

        private static bool TryGetBufferMultiBlock(BufferSegment start, int startIndex, BufferSegment end, int endIndex, out Memory<byte> data)
        {
            var segment = start;

            // Determine if we might attempt to copy data from segment.Next before
            // calculating "following" so we don't risk skipping data that could
            // be added after segment.End when we decide to copy from segment.Next.
            // segment.End will always be advanced before segment.Next is set.

            int following = 0;

            while (true)
            {
                var wasLastSegment = segment.Next == null || end == segment;

                if (end == segment)
                {
                    following = endIndex - startIndex;
                }
                else
                {
                    following = segment.End - startIndex;
                }

                if (following > 0)
                {
                    break;
                }

                if (wasLastSegment)
                {
                    data = Memory<byte>.Empty;
                    return false;
                }
                else
                {
                    segment = segment.Next;
                    startIndex = segment.Start;
                }
            }

            data = segment.Memory.Slice(startIndex, following);
            return true;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            return sb.ToString();
        }

        public static bool operator ==(ReadCursor c1, ReadCursor c2)
        {
            return c1.Equals(c2);
        }

        public static bool operator !=(ReadCursor c1, ReadCursor c2)
        {
            return !c1.Equals(c2);
        }

        public bool Equals(ReadCursor other)
        {
            return other.Segment == Segment && other.Index == Index;
        }

        public override bool Equals(object obj)
        {
            return Equals((ReadCursor)obj);
        }

        public override int GetHashCode()
        {
            var h1 = Segment?.GetHashCode() ?? 0;
            var h2 = Index.GetHashCode();

            var shift5 = ((uint)h1 << 5) | ((uint)h1 >> 27);
            return ((int)shift5 + h1) ^ h2;
        }

        internal static bool GreaterOrEqual(BufferSegment start, int startIndex, BufferSegment end, int endIndex)
        {
            // other.Segment.RunningLength + other.Index  - other.Segment.Start <= Segment.RunningLength + Index- Segment.Start
            // fliped to avoid overflows

            return end.RunningLength - startIndex - end.Start <= start.RunningLength - endIndex - start.Start;
        }

        internal BufferSegment GetSegment()
        {
            if (Segment == null)
            {
                return null;
            }
            if (Segment is BufferSegment bufferSegment)
            {
                return bufferSegment;
            }

            ThrowWrongType();
            return default;
        }
    }
}
