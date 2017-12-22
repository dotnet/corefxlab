// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers;
using System.Collections.Sequences;
using System.IO.Pipelines.Testing;
using System.Linq;
using Xunit;

namespace System.IO.Pipelines.Tests
{
    public abstract class ReadableBufferFacts
    {
        public class Array: ReadableBufferFacts
        {
            public Array() : base(TestBufferFactory.Array) { }
        }

        public class OwnedMemory: ReadableBufferFacts
        {
            public OwnedMemory() : base(TestBufferFactory.OwnedMemory) { }
        }

        public class SingleSegment: ReadableBufferFacts
        {
            public SingleSegment() : base(TestBufferFactory.SingleSegment) { }
        }

        public class SegmentPerByte: ReadableBufferFacts
        {
            public SegmentPerByte() : base(TestBufferFactory.SegmentPerByte) { }

            [Fact]
            // This test verifies that optimization for known cursors works and
            // avoids additional walk but it's only valid for multi segmented buffers
            public void ReadCursorSeekDoesNotCheckEndIfTrustingEnd()
            {
                var buffer = Factory.CreateOfSize(3);
                var buffer2 = Factory.CreateOfSize(3);
                Seek(buffer.Start, buffer2.End, 2, false);
            }
        }

        internal TestBufferFactory Factory { get; }

        internal ReadableBufferFacts(TestBufferFactory factory)
        {
            Factory = factory;
        }

        [Fact]
        public void EmptyIsCorrect()
        {
            var buffer = Factory.CreateOfSize(0);
            Assert.Equal(0, buffer.Length);
            Assert.True(buffer.IsEmpty);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(8)]
        public void LengthIsCorrect(int length)
        {
            var buffer = Factory.CreateOfSize(length);
            Assert.Equal(length, buffer.Length);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(8)]
        public void ToArrayIsCorrect(int length)
        {
            var data = Enumerable.Range(0, length).Select(i => (byte)i).ToArray();
            var buffer = Factory.CreateWithContent(data);
            Assert.Equal(length, buffer.Length);
            Assert.Equal(data, buffer.ToArray());
        }

        [Theory]
        [MemberData(nameof(OutOfRangeSliceCases))]
        public void ReadableBufferDoesNotAllowSlicingOutOfRange(Action<ReadOnlyBuffer> fail)
        {
            var buffer = Factory.CreateOfSize(100);
            var ex = Assert.Throws<InvalidOperationException>(() => fail(buffer));
        }

        [Fact]
        public void ReadableBufferMove_MovesReadCursor()
        {
            var buffer = Factory.CreateOfSize(100);
            var cursor = buffer.Move(buffer.Start, 65);
            Assert.Equal(buffer.Slice(65).Start, cursor);
        }

        [Fact]
        public void ReadableBufferMove_ChecksBounds()
        {
            var buffer = Factory.CreateOfSize(100);
            Assert.Throws<InvalidOperationException>(() => buffer.Move(buffer.Start, 101));
        }

        [Fact]
        public void ReadableBufferMove_DoesNotAlowNegative()
        {
            var buffer = Factory.CreateOfSize(20);
            Assert.Throws<ArgumentOutOfRangeException>(() => buffer.Move(buffer.Start, -1));
        }

        [Fact]
        public void ReadCursorSeekChecksEndIfNotTrustingEnd()
        {
            var buffer = Factory.CreateOfSize(3);
            var buffer2 = Factory.CreateOfSize(3);
            Assert.Throws<InvalidOperationException>(() => Seek(buffer.Start, buffer2.End, 2, true));
        }

        [Fact]
        public void SegmentStartIsConsideredInBoundsCheck()
        {
            // 0               50           100    0             50             100
            // [                ##############] -> [##############                ]
            //                         ^c1            ^c2
            var bufferSegment1 = new BufferSegment();
            bufferSegment1.SetMemory(new OwnedArray<byte>(new byte[100]), 50, 99);

            var bufferSegment2 = new BufferSegment();
            bufferSegment2.SetMemory(new OwnedArray<byte>(new byte[100]), 0, 50);
            bufferSegment1.SetNext(bufferSegment2);

            var readableBuffer = new ReadOnlyBuffer(new Position(bufferSegment1, 0), new Position(bufferSegment2, 50));

            var c1 = readableBuffer.Move(readableBuffer.Start, 25); // segment 1 index 75
            var c2 = readableBuffer.Move(readableBuffer.Start, 55); // segment 2 index 5

            var sliced = readableBuffer.Slice(c1, c2);

            Assert.Equal(30, sliced.Length);
        }

        [Fact]
        public void MovePrefersNextSegment()
        {
            var bufferSegment1 = new BufferSegment();
            bufferSegment1.SetMemory(new OwnedArray<byte>(new byte[100]), 49, 99);

            var bufferSegment2 = new BufferSegment();
            bufferSegment2.SetMemory(new OwnedArray<byte>(new byte[100]), 0, 0);
            bufferSegment1.SetNext(bufferSegment2);

            var readableBuffer = new ReadOnlyBuffer(new Position(bufferSegment1, 0), new Position(bufferSegment2, 0));

            var c1 = readableBuffer.Move(readableBuffer.Start, 50);

            Assert.Equal(0, c1.Index);
            Assert.Equal(bufferSegment2, c1.Segment);
        }

        [Fact]
        public void Create_WorksWithArray()
        {
            var readableBuffer = new ReadOnlyBuffer(new byte[] {1, 2, 3, 4, 5}, 2, 3);
            Assert.Equal(readableBuffer.ToArray(), new byte[] {3, 4, 5});
        }

        [Fact]
        public void Create_WorksWithOwnedMemory()
        {
            var memory = new OwnedArray<byte>(new byte[] {1, 2, 3, 4, 5});
            var readableBuffer = new ReadOnlyBuffer(memory, 2, 3);
            Assert.Equal(new byte[] {3, 4, 5}, readableBuffer.ToArray());
        }

        public static TheoryData<Action<ReadOnlyBuffer>> OutOfRangeSliceCases => new TheoryData<Action<ReadOnlyBuffer>>
        {
            b => b.Slice(101),
            b => b.Slice(0, 101),
            b => b.Slice(b.Start, 101),
            b => b.Slice(0, 70).Slice(b.End, b.End),
            b => b.Slice(0, 70).Slice(b.Start, b.End),
            b => b.Slice(0, 70).Slice(0, b.End),
            b => b.Slice(70, b.Start)
        };

        static bool TryGetBuffer(Position begin, Position end, out ReadOnlyMemory<byte> data, out Position next)
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

        static Position Seek(Position begin, Position end, long bytes, bool checkEndReachable = true)
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

        static Position SeekMultiSegment(Position begin, Position end, long bytes, bool checkEndReachable)
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
    }
}
