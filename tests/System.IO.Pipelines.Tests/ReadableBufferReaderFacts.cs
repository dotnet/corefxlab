// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace System.IO.Pipelines.Tests
{
    public class ReadableBufferReaderFacts
    {
        [Fact]
        public void PeekReturnsByteWithoutMoving()
        {
            var reader = new ReadableBufferReader(ReadableBuffer.Create(new byte[] { 1, 2 }, 0, 2));
            Assert.Equal(1, reader.Peek());
            Assert.Equal(1, reader.Peek());
        }

        [Fact]
        public void CursorIsCorrectAtEnd()
        {
            var reader = new ReadableBufferReader(ReadableBuffer.Create(new byte[] { 1, 2 }, 0, 2));
            reader.Take();
            reader.Take();
            Assert.True(reader.End);
            Assert.True(reader.Cursor.IsEnd);
        }

        [Fact]
        public void TakeReturnsByteAndMoves()
        {
            var reader = new ReadableBufferReader(ReadableBuffer.Create(new byte[] { 1, 2 }, 0, 2));
            Assert.Equal(1, reader.Take());
            Assert.Equal(2, reader.Take());
            Assert.Equal(-1, reader.Take());
        }

        [Fact]
        public void PeekReturnsMinuOneByteInTheEnd()
        {
            var reader = new ReadableBufferReader(ReadableBuffer.Create(new byte[] { 1, 2 }, 0, 2));
            Assert.Equal(1, reader.Take());
            Assert.Equal(2, reader.Take());
            Assert.Equal(-1, reader.Peek());
        }

        [Fact]
        public void SkipToEndThenPeekReturnsMinusOne()
        {
            var reader = new ReadableBufferReader(ReadableBuffer.Create(new byte[] { 1, 2, 3, 4, 5 }, 0, 5));
            reader.Skip(5);
            Assert.True(reader.End);
            Assert.Equal(-1, reader.Peek());
            Assert.True(reader.Cursor.IsEnd);
        }

        [Fact]
        public void SkipSingleBufferSkipsBytes()
        {
            var reader = new ReadableBufferReader(ReadableBuffer.Create(new byte[] { 1, 2, 3, 4, 5 }, 0, 5));
            reader.Skip(2);
            Assert.Equal(3, reader.Peek());
            reader.Skip(2);
            Assert.Equal(5, reader.Peek());
        }

        [Fact]
        public void SkippingPastLengthThrows()
        {
            var reader = new ReadableBufferReader(ReadableBuffer.Create(new byte[] { 1, 2, 3, 4, 5 }, 0, 5));
            Assert.Throws<ArgumentOutOfRangeException>(() => reader.Skip(6));
        }

        [Fact]
        public void CtorFindsFirstNonEmptySegment()
        {
            var buffer = BufferUtilities.CreateBuffer(new[] { new byte[] { }, new byte[] { 1 } });
            var reader = new ReadableBufferReader(buffer);

            Assert.Equal(1, reader.Peek());
        }

        [Fact]
        public void EmptySegmentsAreSkippedOnMoveNext()
        {
            var buffer = BufferUtilities.CreateBuffer(new[] { new byte[] { 1 }, new byte[] { }, new byte[] { }, new byte[] { 2 } });
            var reader = new ReadableBufferReader(buffer);

            Assert.Equal(1, reader.Peek());
            reader.Skip(1);
            Assert.Equal(2, reader.Peek());
        }

        [Fact]
        public void PeekGoesToEndIfAllEmptySegments()
        {
            var buffer = BufferUtilities.CreateBuffer(new[] { new byte[] { }, new byte[] { }, new byte[] { }, new byte[] { } });
            var reader = new ReadableBufferReader(buffer);

            Assert.Equal(-1, reader.Peek());
            Assert.True(reader.End);
            Assert.True(reader.Cursor.IsEnd);
        }

        [Fact]
        public void SkipTraversesSegments()
        {
            var buffer = BufferUtilities.CreateBuffer(new[] { new byte[] { 1 }, new byte[] { 2 }, new byte[] { 3 } });
            var reader = new ReadableBufferReader(buffer);

            reader.Skip(2);
            Assert.Equal(3, reader.Take());
        }

        [Fact]
        public void SkipThrowsPastLengthMultipleSegments()
        {
            var buffer = BufferUtilities.CreateBuffer(new[] { new byte[] { 1 }, new byte[] { 2 }, new byte[] { 3 } });
            var reader = new ReadableBufferReader(buffer);

            Assert.Throws<ArgumentOutOfRangeException>(() => reader.Skip(4));
        }

        [Fact]
        public void TakeTraversesSegments()
        {
            var buffer = BufferUtilities.CreateBuffer(new[] { new byte[] { 1 }, new byte[] { 2 }, new byte[] { 3 } });
            var reader = new ReadableBufferReader(buffer);

            Assert.Equal(1, reader.Take());
            Assert.Equal(2, reader.Take());
            Assert.Equal(3, reader.Take());
            Assert.Equal(-1, reader.Take());
        }

        [Fact]
        public void PeekTraversesSegments()
        {
            var buffer = BufferUtilities.CreateBuffer(new[] { new byte[] { 1 }, new byte[] { 2 } });
            var reader = new ReadableBufferReader(buffer);

            Assert.Equal(1, reader.Take());
            Assert.Equal(2, reader.Peek());
            Assert.Equal(2, reader.Take());
            Assert.Equal(-1, reader.Peek());
            Assert.Equal(-1, reader.Take());
        }

        [Fact]
        public void PeekWorkesWithEmptySegments()
        {
            var buffer = BufferUtilities.CreateBuffer(new[] { new byte[] {  }, new byte[] { 1 } });
            var reader = new ReadableBufferReader(buffer);

            Assert.Equal(1, reader.Peek());
            Assert.Equal(1, reader.Take());
            Assert.Equal(-1, reader.Peek());
            Assert.Equal(-1, reader.Take());
        }

        [Fact]
        public void WorkesWithEmptyBuffer()
        {
            var reader = new ReadableBufferReader(ReadableBuffer.Create(new byte[] { 0 }, 0, 0));

            Assert.Equal(-1, reader.Peek());
            Assert.Equal(-1, reader.Take());
        }

        [Theory]
        [InlineData(0, 0)]
        [InlineData(5, 5)]
        [InlineData(10, 10)]
        [InlineData(11, int.MaxValue)]
        [InlineData(12, int.MaxValue)]
        [InlineData(15, int.MaxValue)]
        public void ReturnsCorrectCursor(int takes, int slice)
        {
            var readableBuffer = ReadableBuffer.Create(new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 }, 0, 10);
            var reader = new ReadableBufferReader(readableBuffer);
            for (int i = 0; i < takes; i++)
            {
                reader.Take();
            }

            var expected = slice == int.MaxValue ? readableBuffer.End : readableBuffer.Slice(slice).Start;
            Assert.Equal(expected, reader.Cursor);
        }
    }

}
