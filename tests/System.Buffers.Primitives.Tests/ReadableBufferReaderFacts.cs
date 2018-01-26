// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers;
using System.Collections.Sequences;
using System.IO.Pipelines.Testing;
using Xunit;

namespace System.IO.Pipelines.Tests
{
    public abstract class ReadableBufferReaderFacts
    {
        public class Array: SingleSegment
        {
            public Array() : base(ReadOnlyBufferFactory.Array) { }
            internal Array(ReadOnlyBufferFactory factory) : base(factory) { }
        }

        public class OwnedMemory: SingleSegment
        {
            public OwnedMemory() : base(ReadOnlyBufferFactory.OwnedMemory) { }
        }

        public class SingleSegment: SegmentPerByte
        {
            public SingleSegment() : base(ReadOnlyBufferFactory.SingleSegment) { }
            internal SingleSegment(ReadOnlyBufferFactory factory) : base(factory) { }

            [Fact]
            public void AdvanceSingleBufferSkipsBytes()
            {
                var reader = BufferReader.Create(BufferUtilities.CreateBuffer(new byte[] { 1, 2, 3, 4, 5 }));
                reader.Advance(2);
                Assert.Equal(2, reader.CurrentSegmentIndex);
                Assert.Equal(3, reader.CurrentSegment[reader.CurrentSegmentIndex]);
                Assert.Equal(3, reader.Peek());
                reader.Advance(2);
                Assert.Equal(5, reader.Peek());
                Assert.Equal(4, reader.CurrentSegmentIndex);
                Assert.Equal(5, reader.CurrentSegment[reader.CurrentSegmentIndex]);
            }

            [Fact]
            public void TakeReturnsByteAndMoves()
            {
                var reader = BufferReader.Create(Factory.CreateWithContent(new byte[] { 1, 2 }));
                Assert.Equal(0, reader.CurrentSegmentIndex);
                Assert.Equal(1, reader.CurrentSegment[reader.CurrentSegmentIndex]);
                Assert.Equal(1, reader.Read());
                Assert.Equal(1, reader.CurrentSegmentIndex);
                Assert.Equal(2, reader.CurrentSegment[reader.CurrentSegmentIndex]);
                Assert.Equal(2, reader.Read());
                Assert.Equal(-1, reader.Read());
            }
        }

        public class SegmentPerByte: ReadableBufferReaderFacts
        {
            public SegmentPerByte() : base(ReadOnlyBufferFactory.SegmentPerByte) { }
            internal SegmentPerByte(ReadOnlyBufferFactory factory) : base(factory) { }
        }

        internal ReadOnlyBufferFactory Factory { get; }

        internal ReadableBufferReaderFacts(ReadOnlyBufferFactory factory)
        {
            Factory = factory;
        }

        [Fact]
        public void PeekReturnsByteWithoutMoving()
        {
            var reader = BufferReader.Create(Factory.CreateWithContent(new byte[] { 1, 2 }));
            Assert.Equal(1, reader.Peek());
            Assert.Equal(1, reader.Peek());
        }

        [Fact]
        public void CursorIsCorrectAtEnd()
        {
            var reader = BufferReader.Create(Factory.CreateWithContent(new byte[] { 1, 2 }));
            reader.Read();
            reader.Read();
            Assert.True(reader.End);
        }

        [Fact]
        public void CursorIsCorrectWithEmptyLastBlock()
        {
            var last = new BufferSegment();
            last.SetMemory(new OwnedArray<byte>(new byte[4]), 0, 4);

            var first = new BufferSegment();
            first.SetMemory(new OwnedArray<byte>(new byte[] { 1, 2 }), 0, 2);
            first.SetNext(last);

            var reader = BufferReader.Create(new ReadOnlyBuffer<byte>(first, first.Start, last, last.Start));
            reader.Read();
            reader.Read();
            reader.Read();
            Assert.Same(last, reader.SequenceIndex.Segment);
            Assert.Equal(0, reader.SequenceIndex.Index);
            Assert.True(reader.End);
        }

        [Fact]
        public void PeekReturnsMinuOneByteInTheEnd()
        {
            var reader = BufferReader.Create(Factory.CreateWithContent(new byte[] { 1, 2 }));
            Assert.Equal(1, reader.Read());
            Assert.Equal(2, reader.Read());
            Assert.Equal(-1, reader.Peek());
        }

        [Fact]
        public void AdvanceToEndThenPeekReturnsMinusOne()
        {
            var reader = BufferReader.Create(Factory.CreateWithContent(new byte[] { 1, 2, 3, 4, 5 }));
            reader.Advance(5);
            Assert.True(reader.End);
            Assert.Equal(-1, reader.Peek());
        }

        [Fact]
        public void AdvancingPastLengthThrows()
        {
            var reader = BufferReader.Create(Factory.CreateWithContent(new byte[] { 1, 2, 3, 4, 5 }));
            try
            {
                reader.Advance(6);
                Assert.True(false);
            }
            catch (Exception ex)
            {
                Assert.True(ex is ArgumentOutOfRangeException);
            }
        }

        [Fact]
        public void CtorFindsFirstNonEmptySegment()
        {
            var buffer = Factory.CreateWithContent(new byte[] { 1 });
            var reader = BufferReader.Create(buffer);

            Assert.Equal(1, reader.Peek());
        }

        [Fact]
        public void EmptySegmentsAreSkippedOnMoveNext()
        {
            var buffer = Factory.CreateWithContent(new byte[] { 1, 2 });
            var reader = BufferReader.Create(buffer);

            Assert.Equal(1, reader.Peek());
            reader.Advance(1);
            Assert.Equal(2, reader.Peek());
        }

        [Fact]
        public void PeekGoesToEndIfAllEmptySegments()
        {
            var buffer = BufferUtilities.CreateBuffer(new[] { new byte[] { }, new byte[] { }, new byte[] { }, new byte[] { } });
            var reader = BufferReader.Create(buffer);

            Assert.Equal(-1, reader.Peek());
            Assert.True(reader.End);
        }

        [Fact]
        public void AdvanceTraversesSegments()
        {
            var buffer = Factory.CreateWithContent(new byte[] { 1, 2, 3 });
            var reader = BufferReader.Create(buffer);

            reader.Advance(2);
            Assert.Equal(3, reader.CurrentSegment[reader.CurrentSegmentIndex]);
            Assert.Equal(3, reader.Read());
        }

        [Fact]
        public void AdvanceThrowsPastLengthMultipleSegments()
        {
            var buffer = Factory.CreateWithContent(new byte[] { 1, 2, 3 });
            var reader = BufferReader.Create(buffer);

            try
            {
                reader.Advance(4);
                Assert.True(false);
            }
            catch (Exception ex)
            {
                Assert.True(ex is ArgumentOutOfRangeException);
            }
        }

        [Fact]
        public void TakeTraversesSegments()
        {
            var buffer = Factory.CreateWithContent(new byte[] { 1, 2, 3 });
            var reader = BufferReader.Create(buffer);

            Assert.Equal(1, reader.Read());
            Assert.Equal(2, reader.Read());
            Assert.Equal(3, reader.Read());
            Assert.Equal(-1, reader.Read());
        }

        [Fact]
        public void PeekTraversesSegments()
        {
            var buffer = Factory.CreateWithContent(new byte[] { 1, 2 });
            var reader = BufferReader.Create(buffer);

            Assert.Equal(1, reader.CurrentSegment[reader.CurrentSegmentIndex]);
            Assert.Equal(1, reader.Read());

            Assert.Equal(2, reader.CurrentSegment[reader.CurrentSegmentIndex]);
            Assert.Equal(2, reader.Peek());
            Assert.Equal(2, reader.Read());
            Assert.Equal(-1, reader.Peek());
            Assert.Equal(-1, reader.Read());
        }

        [Fact]
        public void PeekWorkesWithEmptySegments()
        {
            var buffer = Factory.CreateWithContent(new byte[] { 1 });
            var reader = BufferReader.Create(buffer);

            Assert.Equal(0, reader.CurrentSegmentIndex);
            Assert.Equal(1, reader.CurrentSegment.Length);
            Assert.Equal(1, reader.Peek());
            Assert.Equal(1, reader.Read());
            Assert.Equal(-1, reader.Peek());
            Assert.Equal(-1, reader.Read());
        }

        [Fact]
        public void WorkesWithEmptyBuffer()
        {
            var reader = BufferReader.Create(Factory.CreateWithContent(new byte[] { }));

            Assert.Equal(0, reader.CurrentSegmentIndex);
            Assert.Equal(0, reader.CurrentSegment.Length);
            Assert.Equal(-1, reader.Peek());
            Assert.Equal(-1, reader.Read());
        }

        [Theory]
        [InlineData(0, false)]
        [InlineData(5, false)]
        [InlineData(10, false)]
        [InlineData(11, true)]
        [InlineData(12, true)]
        [InlineData(15, true)]
        public void ReturnsCorrectCursor(int takes, bool end)
        {
            var readableBuffer = Factory.CreateWithContent(new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 });
            var reader = BufferReader.Create(readableBuffer);
            for (int i = 0; i < takes; i++)
            {
                reader.Read();
            }

            var expected = end ?  new byte[] {} : readableBuffer.Slice((long)takes).ToArray();
            Assert.Equal(expected, readableBuffer.Slice(reader.SequenceIndex).ToArray());
        }

        [Fact]
        public void SlicingBufferReturnsCorrectCursor()
        {
            var buffer = Factory.CreateWithContent(new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 });
            var sliced = buffer.Slice(2L);

            var reader = BufferReader.Create(sliced);
            Assert.Equal(sliced.ToArray(), buffer.Slice(reader.SequenceIndex).ToArray());
            Assert.Equal(2, reader.Peek());
            Assert.Equal(0, reader.CurrentSegmentIndex);
        }

        [Fact]
        public void ReaderIndexIsCorrect()
        {
            var buffer = Factory.CreateWithContent(new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 });
            var reader = BufferReader.Create(buffer);

            var counter = 1;
            while (!reader.End)
            {
                var span = reader.CurrentSegment;
                for (int i = reader.CurrentSegmentIndex; i < span.Length; i++)
                {
                    Assert.Equal(counter++, reader.CurrentSegment[i]);
                }
                reader.Advance(span.Length);
            }
            Assert.Equal(buffer.Length, reader.ConsumedBytes);
        }

        [Fact]
        public void CopyToLargerBufferWorks()
        {
            var content = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

            Span<byte> buffer = new byte[content.Length + 1];
            var reader = BufferReader.Create(Factory.CreateWithContent(content));

            // this loop skips more and more items in the reader
            for (int i = 0; i < content.Length; i++) {

                int copied = BufferReader.Peek(reader, buffer);
                Assert.Equal(content.Length - i, copied);
                Assert.True(buffer.Slice(0, copied).SequenceEqual(content.AsSpan().Slice(i)));

                // make sure that nothing more got written, i.e. tail is empty
                for (int r = copied; r < buffer.Length; r++)
                {
                    Assert.Equal(0, buffer[r]);
                }

                reader.Advance(1);
                buffer.Clear();
            }
        }

        [Fact]
        public void CopyToSmallerBufferWorks()
        {
            var content = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

            Span<byte> buffer = new byte[content.Length];
            var reader = BufferReader.Create(Factory.CreateWithContent(content));

            // this loop skips more and more items in the reader
            for (int i = 0; i < content.Length; i++)
            {
                // this loop makes the destination buffer smaller and smaller
                for (int j = 0; j < buffer.Length - i; j++)
                {
                    var bufferSlice = buffer.Slice(0, j);
                    bufferSlice.Clear();
                    int copied = BufferReader.Peek(reader, bufferSlice);
                    Assert.Equal(Math.Min(bufferSlice.Length, content.Length - i), copied);

                    Assert.True(bufferSlice.Slice(0, copied).SequenceEqual(content.AsSpan().Slice(i, j)));
                }

                reader.Advance(1);
            }
        }
    }

}
