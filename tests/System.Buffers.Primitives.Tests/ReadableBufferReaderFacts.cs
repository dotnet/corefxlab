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
            public void SkipSingleBufferSkipsBytes()
            {
                var reader = BufferReader.Create(BufferUtilities.CreateBuffer(new byte[] { 1, 2, 3, 4, 5 }));
                reader.Skip(2);
                Assert.Equal(2, reader.Index);
                Assert.Equal(3, reader.Span[reader.Index]);
                Assert.Equal(3, reader.Peek());
                reader.Skip(2);
                Assert.Equal(5, reader.Peek());
                Assert.Equal(4, reader.Index);
                Assert.Equal(5, reader.Span[reader.Index]);
            }

            [Fact]
            public void TakeReturnsByteAndMoves()
            {
                var reader = BufferReader.Create(Factory.CreateWithContent(new byte[] { 1, 2 }));
                Assert.Equal(0, reader.Index);
                Assert.Equal(1, reader.Span[reader.Index]);
                Assert.Equal(1, reader.Take());
                Assert.Equal(1, reader.Index);
                Assert.Equal(2, reader.Span[reader.Index]);
                Assert.Equal(2, reader.Take());
                Assert.Equal(-1, reader.Take());
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
            reader.Take();
            reader.Take();
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

            var reader = BufferReader.Create(new ReadOnlyBuffer(first, first.Start, last, last.Start));
            reader.Take();
            reader.Take();
            reader.Take();
            Assert.Same(last, reader.Position.Segment);
            Assert.Equal(0, reader.Position.Index);
            Assert.True(reader.End);
        }

        [Fact]
        public void PeekReturnsMinuOneByteInTheEnd()
        {
            var reader = BufferReader.Create(Factory.CreateWithContent(new byte[] { 1, 2 }));
            Assert.Equal(1, reader.Take());
            Assert.Equal(2, reader.Take());
            Assert.Equal(-1, reader.Peek());
        }

        [Fact]
        public void SkipToEndThenPeekReturnsMinusOne()
        {
            var reader = BufferReader.Create(Factory.CreateWithContent(new byte[] { 1, 2, 3, 4, 5 }));
            reader.Skip(5);
            Assert.True(reader.End);
            Assert.Equal(-1, reader.Peek());
        }

        [Fact]
        public void SkippingPastLengthThrows()
        {
            var reader = BufferReader.Create(Factory.CreateWithContent(new byte[] { 1, 2, 3, 4, 5 }));
            try
            {
                reader.Skip(6);
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
            reader.Skip(1);
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
        public void SkipTraversesSegments()
        {
            var buffer = Factory.CreateWithContent(new byte[] { 1, 2, 3 });
            var reader = BufferReader.Create(buffer);

            reader.Skip(2);
            Assert.Equal(3, reader.Span[reader.Index]);
            Assert.Equal(3, reader.Take());
        }

        [Fact]
        public void SkipThrowsPastLengthMultipleSegments()
        {
            var buffer = Factory.CreateWithContent(new byte[] { 1, 2, 3 });
            var reader = BufferReader.Create(buffer);

            try
            {
                reader.Skip(4);
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

            Assert.Equal(1, reader.Take());
            Assert.Equal(2, reader.Take());
            Assert.Equal(3, reader.Take());
            Assert.Equal(-1, reader.Take());
        }

        [Fact]
        public void PeekTraversesSegments()
        {
            var buffer = Factory.CreateWithContent(new byte[] { 1, 2 });
            var reader = BufferReader.Create(buffer);

            Assert.Equal(1, reader.Span[reader.Index]);
            Assert.Equal(1, reader.Take());

            Assert.Equal(2, reader.Span[reader.Index]);
            Assert.Equal(2, reader.Peek());
            Assert.Equal(2, reader.Take());
            Assert.Equal(-1, reader.Peek());
            Assert.Equal(-1, reader.Take());
        }

        [Fact]
        public void PeekWorkesWithEmptySegments()
        {
            var buffer = Factory.CreateWithContent(new byte[] { 1 });
            var reader = BufferReader.Create(buffer);

            Assert.Equal(0, reader.Index);
            Assert.Equal(1, reader.Span.Length);
            Assert.Equal(1, reader.Peek());
            Assert.Equal(1, reader.Take());
            Assert.Equal(-1, reader.Peek());
            Assert.Equal(-1, reader.Take());
        }

        [Fact]
        public void WorkesWithEmptyBuffer()
        {
            var reader = BufferReader.Create(Factory.CreateWithContent(new byte[] { }));

            Assert.Equal(0, reader.Index);
            Assert.Equal(0, reader.Span.Length);
            Assert.Equal(-1, reader.Peek());
            Assert.Equal(-1, reader.Take());
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
                reader.Take();
            }

            var expected = end ?  new byte[] {} : readableBuffer.Slice((long)takes).ToArray();
            Assert.Equal(expected, readableBuffer.Slice(reader.Position).ToArray());
        }

        [Fact]
        public void SlicingBufferReturnsCorrectCursor()
        {
            var buffer = Factory.CreateWithContent(new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 });
            var sliced = buffer.Slice(2L);

            var reader = BufferReader.Create(sliced);
            Assert.Equal(sliced.ToArray(), buffer.Slice(reader.Position).ToArray());
            Assert.Equal(2, reader.Peek());
            Assert.Equal(0, reader.Index);
        }

        [Fact]
        public void ReaderIndexIsCorrect()
        {
            var buffer = Factory.CreateWithContent(new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 });
            var reader = BufferReader.Create(buffer);

            var counter = 1;
            while (!reader.End)
            {
                var span = reader.Span;
                for (int i = reader.Index; i < span.Length; i++)
                {
                    Assert.Equal(counter++, reader.Span[i]);
                }
                reader.Skip(span.Length);
            }
            Assert.Equal(buffer.Length, reader.ConsumedBytes);
        }
    }

}
