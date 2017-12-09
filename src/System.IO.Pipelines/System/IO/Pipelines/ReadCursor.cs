// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Buffers;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;

namespace System.IO.Pipelines
{
    [DebuggerDisplay("{Segment}[{Index}]")]
    public readonly struct ReadCursor : IEquatable<ReadCursor>
    {
        internal readonly object Segment;
        internal readonly int Index;

        internal ReadCursor(object segment, int index)
        {
            Segment = segment;
            Index = index;
        }

        internal bool IsDefault => Segment == null;

        [MethodImpl(MethodImplOptions.NoInlining)]
        private void ThrowWrongType()
        {
            throw new InvalidOperationException();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal long GetLength(ReadCursor end)
        {
            if (IsDefault)
            {
                return 0;
            }

            var segment = Segment;
            switch (segment)
            {
                case IMemoryList<byte> bufferSegment:
                    return GetLength(bufferSegment, Index, (BufferSegment)end.Segment, end.Index);
                case byte[] _:
                case OwnedMemory<byte> _:
                    return end.Index - Index;
            }

            ThrowWrongType();
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
            while (enumerator.MoveNext())
            {
                var memory = enumerator.Current;
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
                if (!GreaterOrEqual(Get<IMemoryList<byte>>(), Index, newCursor.Get<IMemoryList<byte>>(), newCursor.Index))
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
        internal bool TryGetBuffer(ReadCursor end, out ReadOnlyMemory<byte> data, out ReadCursor next)
        {
            var segment = Segment;

            switch (segment)
            {
                case null:
                    data = default;
                    next = default;
                    return false;

                case IMemoryList<byte> bufferSegment:
                    var startIndex = Index;
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
                                ThrowEndNotSeen();
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
                    data = ownedMemory.Memory.Slice(Index, end.Index - Index);

                    if (segment != end.Segment)
                    {
                        ThrowEndNotSeen();
                    }

                    next = default;
                    return true;

                case byte[] array:
                    data = new Memory<byte>(array, Index, end.Index - Index);

                    if (segment != end.Segment)
                    {
                        ThrowEndNotSeen();
                    }
                    next = default;
                    return true;
            }

            PipelinesThrowHelper.ThrowNotSupportedException();
            next = default;
            return false;
        }


        private void ThrowEndNotSeen()
        {
            throw new InvalidOperationException("Segments ended by end was never seen");
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

        internal static bool GreaterOrEqual(IMemoryList<byte> start, int startIndex, IMemoryList<byte> end, int endIndex)
        {
            // other.Segment.RunningLength + other.Index  - other.Segment.Start <= Segment.RunningLength + Index- Segment.Start
            // fliped to avoid overflows

            return end.VirtualIndex - startIndex <= start.VirtualIndex - endIndex;
        }

        internal T Get<T>()
        {
            switch (Segment)
            {
                case null:
                    return default;
                case T segment:
                    return segment;
            }

            ThrowWrongType();
            return default;
        }
    }
}
