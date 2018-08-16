// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers.Reader;
using System.Buffers.Testing;
using System.IO.Pipelines.Tests;
using Xunit;

namespace System.Buffers.Tests
{
    public class ArrayByte : SingleSegment<byte>
    {
        public ArrayByte() : base(ReadOnlyBufferFactory<byte>.Array, s_byteInputData) { }
    }

    public class ArrayChar : SingleSegment<char>
    {
        public ArrayChar() : base(ReadOnlyBufferFactory<char>.Array, s_charInputData) { }
    }

    public class OwnedMemoryByte : SingleSegment<byte>
    {
        public OwnedMemoryByte() : base(ReadOnlyBufferFactory<byte>.OwnedMemory, s_byteInputData) { }
    }

    public class OwnedMemoryChar : SingleSegment<char>
    {
        public OwnedMemoryChar() : base(ReadOnlyBufferFactory<char>.OwnedMemory, s_charInputData) { }
    }

    public class SingleSegmentByte : SingleSegment<byte>
    {
        public SingleSegmentByte() : base(s_byteInputData) { }
    }

    public class SingleSegmentChar : SingleSegment<char>
    {
        public SingleSegmentChar() : base(s_charInputData) { }
    }

    public abstract class SingleSegment<T> : ReaderBasicTests<T> where T : unmanaged, IEquatable<T>
    {
        public SingleSegment(T[] inputData) : base(ReadOnlyBufferFactory<T>.SingleSegment, inputData) { }
        internal SingleSegment(ReadOnlyBufferFactory<T> factory, T[] inputData) : base(factory, inputData) { }

        [Fact]
        public void AdvanceSingleBufferSkipsBytes()
        {
            var reader = new BufferReader<T>(BufferUtilities.CreateBuffer(GetInputData(5)));
            reader.Advance(2);
            Assert.Equal(2, reader.CurrentSpanIndex);
            Assert.Equal(InputData[2], reader.CurrentSpan[reader.CurrentSpanIndex]);
            Assert.Equal(InputData[2], reader.Peek());
            reader.Advance(2);
            Assert.Equal(InputData[4], reader.Peek());
            Assert.Equal(4, reader.CurrentSpanIndex);
            Assert.Equal(InputData[4], reader.CurrentSpan[reader.CurrentSpanIndex]);
        }

        [Fact]
        public void TakeReturnsByteAndMoves()
        {
            var reader = new BufferReader<T>(Factory.CreateWithContent(GetInputData(2)));
            Assert.Equal(0, reader.CurrentSpanIndex);
            Assert.Equal(InputData[0], reader.CurrentSpan[reader.CurrentSpanIndex]);
            Assert.Equal(InputData[0], reader.Read());
            Assert.Equal(1, reader.CurrentSpanIndex);
            Assert.Equal(InputData[1], reader.CurrentSpan[reader.CurrentSpanIndex]);
            Assert.Equal(InputData[1], reader.Read());
            Assert.Equal(default, reader.Read());
            Assert.True(reader.End);
        }
    }

    public class SegmentPerByte : ReaderBasicTests<byte>
    {
        public SegmentPerByte() : base(ReadOnlyBufferFactory<byte>.SegmentPerPosition, s_byteInputData) { }
    }

    public class SegmentPerChar : ReaderBasicTests<char>
    {
        public SegmentPerChar() : base(ReadOnlyBufferFactory<char>.SegmentPerPosition, s_charInputData) { }
    }

    public abstract class ReaderBasicTests<T> where T : unmanaged, IEquatable<T>
    {
        internal static byte[] s_byteInputData = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
        internal static char[] s_charInputData = new char[] { '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A' };
        private T[] _inputData;

        internal ReadOnlyBufferFactory<T> Factory { get; }
        protected ReadOnlySpan<T> InputData { get => _inputData; }

        public T[] GetInputData(int count) => InputData.Slice(0, count).ToArray();

        internal ReaderBasicTests(ReadOnlyBufferFactory<T> factory, T[] inputData)
        {
            Factory = factory;
            _inputData = inputData;
        }

        [Fact]
        public void PeekReturnsWithoutMoving()
        {
            var reader = new BufferReader<T>(Factory.CreateWithContent(GetInputData(2)));
            Assert.Equal(InputData[0], reader.Peek());
            Assert.Equal(InputData[0], reader.Peek());
        }

        [Fact]
        public void CursorIsCorrectAtEnd()
        {
            var reader = new BufferReader<T>(Factory.CreateWithContent(GetInputData(2)));
            reader.Read();
            reader.Read();
            Assert.True(reader.End);
        }

        [Fact]
        public void CursorIsCorrectWithEmptyLastBlock()
        {
            var last = new BufferSegment<T>();
            last.SetMemory(new OwnedArray<T>(new T[4]), 0, 4);

            var first = new BufferSegment<T>();
            first.SetMemory(new OwnedArray<T>(GetInputData(2)), 0, 2);
            first.SetNext(last);

            var reader = new BufferReader<T>(new ReadOnlySequence<T>(first, first.Start, last, last.Start));
            reader.Read();
            reader.Read();
            reader.Read();
            Assert.Same(last, reader.Position.GetObject());
            Assert.Equal(0, reader.Position.GetInteger());
            Assert.True(reader.End);
        }

        [Fact]
        public void PeekReturnsDefaultInTheEnd()
        {
            var reader = new BufferReader<T>(Factory.CreateWithContent(GetInputData(2)));
            Assert.Equal(InputData[0], reader.Read());
            Assert.Equal(InputData[1], reader.Read());
            Assert.Equal(default, reader.Peek());
        }

        [Fact]
        public void AdvanceToEndThenPeekReturnsDefault()
        {
            var reader = new BufferReader<T>(Factory.CreateWithContent(GetInputData(5)));
            reader.Advance(5);
            Assert.True(reader.End);
            Assert.Equal(default, reader.Peek());
        }

        [Fact]
        public void AdvancingPastLengthThrows()
        {
            var reader = new BufferReader<T>(Factory.CreateWithContent(GetInputData(5)));
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
            var buffer = Factory.CreateWithContent(GetInputData(1));
            var reader = new BufferReader<T>(buffer);

            Assert.Equal(InputData[0], reader.Peek());
        }

        [Fact]
        public void EmptySegmentsAreSkippedOnMoveNext()
        {
            var buffer = Factory.CreateWithContent(GetInputData(2));
            var reader = new BufferReader<T>(buffer);

            Assert.Equal(InputData[0], reader.Peek());
            reader.Advance(1);
            Assert.Equal(InputData[1], reader.Peek());
        }

        [Fact]
        public void PeekGoesToEndIfAllEmptySegments()
        {
            var buffer = BufferUtilities.CreateBuffer(new[] { new T[] { }, new T[] { }, new T[] { }, new T[] { } });
            var reader = new BufferReader<T>(buffer);

            Assert.Equal(default, reader.Peek());
            Assert.True(reader.End);
        }

        [Fact]
        public void AdvanceTraversesSegments()
        {
            var buffer = Factory.CreateWithContent(GetInputData(3));
            var reader = new BufferReader<T>(buffer);

            reader.Advance(2);
            Assert.Equal(InputData[2], reader.CurrentSpan[reader.CurrentSpanIndex]);
            Assert.Equal(InputData[2], reader.Read());
        }

        [Fact]
        public void AdvanceThrowsPastLengthMultipleSegments()
        {
            var buffer = Factory.CreateWithContent(GetInputData(3));
            var reader = new BufferReader<T>(buffer);

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
            var buffer = Factory.CreateWithContent(GetInputData(3));
            var reader = new BufferReader<T>(buffer);

            Assert.Equal(InputData[0], reader.Read());
            Assert.Equal(InputData[1], reader.Read());
            Assert.Equal(InputData[2], reader.Read());
            Assert.Equal(default, reader.Read());
            Assert.True(reader.End);
        }

        [Fact]
        public void PeekTraversesSegments()
        {
            var buffer = Factory.CreateWithContent(GetInputData(2));
            var reader = new BufferReader<T>(buffer);

            Assert.Equal(InputData[0], reader.CurrentSpan[reader.CurrentSpanIndex]);
            Assert.Equal(InputData[0], reader.Read());

            Assert.Equal(InputData[1], reader.CurrentSpan[reader.CurrentSpanIndex]);
            Assert.Equal(InputData[1], reader.Peek());
            Assert.Equal(InputData[1], reader.Read());
            Assert.Equal(default, reader.Peek());
            Assert.Equal(default, reader.Read());
        }

        [Fact]
        public void PeekWorkesWithEmptySegments()
        {
            var buffer = Factory.CreateWithContent(GetInputData(1));
            var reader = new BufferReader<T>(buffer);

            Assert.Equal(0, reader.CurrentSpanIndex);
            Assert.Equal(1, reader.CurrentSpan.Length);
            Assert.Equal(InputData[0], reader.Peek());
            Assert.Equal(InputData[0], reader.Read());
            Assert.Equal(default, reader.Peek());
            Assert.Equal(default, reader.Read());
        }

        [Fact]
        public void WorkesWithEmptyBuffer()
        {
            var reader = new BufferReader<T>(Factory.CreateWithContent(new T[] { }));

            Assert.Equal(0, reader.CurrentSpanIndex);
            Assert.Equal(0, reader.CurrentSpan.Length);
            Assert.Equal(default, reader.Peek());
            Assert.Equal(default, reader.Read());
            Assert.True(reader.End);
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
            var readableBuffer = Factory.CreateWithContent(GetInputData(10));
            var reader = new BufferReader<T>(readableBuffer);
            for (int i = 0; i < takes; i++)
            {
                reader.Read();
            }

            var expected = end ? new T[] { } : readableBuffer.Slice(takes).ToArray();
            Assert.Equal(expected, readableBuffer.Slice(reader.Position).ToArray());
        }

        [Fact]
        public void SlicingBufferReturnsCorrectCursor()
        {
            var buffer = Factory.CreateWithContent(GetInputData(10));
            var sliced = buffer.Slice(2L);

            var reader = new BufferReader<T>(sliced);
            Assert.Equal(sliced.ToArray(), buffer.Slice(reader.Position).ToArray());
            Assert.Equal(InputData[2], reader.Peek());
            Assert.Equal(0, reader.CurrentSpanIndex);
        }

        [Fact]
        public void ReaderIndexIsCorrect()
        {
            var buffer = Factory.CreateWithContent(GetInputData(10));
            var reader = new BufferReader<T>(buffer);

            int counter = 0;
            while (!reader.End)
            {
                var span = reader.CurrentSpan;
                for (int i = reader.CurrentSpanIndex; i < span.Length; i++)
                {
                    Assert.Equal(InputData[counter++], reader.CurrentSpan[i]);
                }
                reader.Advance(span.Length);
            }
            Assert.Equal(buffer.Length, reader.ConsumedValues);
        }

        [Fact]
        public void CopyToLargerBufferWorks()
        {
            var content = (T[])_inputData.Clone();

            Span<T> buffer = new T[content.Length + 1];
            var reader = new BufferReader<T>(Factory.CreateWithContent(content));

            // this loop skips more and more items in the reader
            for (int i = 0; i < content.Length; i++)
            {

                int copied = reader.Peek(buffer).Length;
                Assert.Equal(content.Length - i, copied);
                Assert.True(buffer.Slice(0, copied).SequenceEqual(content.AsSpan(i)));

                // make sure that nothing more got written, i.e. tail is empty
                for (int r = copied; r < buffer.Length; r++)
                {
                    Assert.Equal(default, buffer[r]);
                }

                reader.Advance(1);
                buffer.Clear();
            }
        }

        [Fact]
        public void CopyToSmallerBufferWorks()
        {
            var content = (T[])_inputData.Clone();

            Span<T> buffer = new T[content.Length];
            var reader = new BufferReader<T>(Factory.CreateWithContent(content));

            // this loop skips more and more items in the reader
            for (int i = 0; i < content.Length; i++)
            {
                // this loop makes the destination buffer smaller and smaller
                for (int j = 0; j < buffer.Length - i; j++)
                {
                    Span<T> bufferSlice = buffer.Slice(0, j);
                    bufferSlice.Clear();
                    ReadOnlySpan<T> peeked = reader.Peek(bufferSlice);
                    Assert.Equal(Math.Min(bufferSlice.Length, content.Length - i), peeked.Length);

                    Assert.True(peeked.SequenceEqual(content.AsSpan(i, j)));
                }

                reader.Advance(1);
            }
        }
    }

}
