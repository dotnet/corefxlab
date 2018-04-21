using System.Buffers.Text;
using Xunit;

namespace System.Buffers.Tests
{
    public class SequenceExtensionsTests
    {
        [Fact]
        public void SequenceIndexOfSingleSegment()
        {
            var array = new byte[] { 1, 2, 3, 4, 5 };
            var bytes = new ReadOnlySequence<byte>(array);
            Assert.Equal(array.Length, bytes.Length);

            // Static method call to avoid calling ReadOnlyBytes.IndexOf
            Assert.Equal(-1, Sequence.IndexOf(bytes, 0));

            for (int i = 0; i < array.Length; i++)
            {
                Assert.Equal(i, Sequence.IndexOf(bytes, (byte)(i + 1)));
            }
        }

        [Fact]
        public void SequenceIndexOfMultiSegment()
        {
            ReadOnlySequence<byte> bytes = BufferFactory.Create(
                new byte[] { 1, 2 },
                new byte[] { 3, 4 }
            );

            Assert.Equal(4, bytes.Length);

            // Static method call to avoid calling ReadOnlyBytes.IndexOf
            Assert.Equal(-1, Sequence.IndexOf(bytes, 0));

            for (int i = 0; i < bytes.Length; i++)
            {
                Assert.Equal(i, Sequence.IndexOf(bytes, (byte)(i + 1)));
            }
        }

        [Fact]
        public void SequenceIndexOfMultiSegmentSliced()
        {
            ReadOnlySequence<byte> bytes = BufferFactory.Create(
                new byte[] { 1, 2 },
                new byte[] { 3, 4 }
            );
            bytes = bytes.Slice(1);

            Assert.Equal(3, bytes.Length);

            // Static method call to avoid calling ReadOnlyBytes.IndexOf
            Assert.Equal(-1, Sequence.IndexOf(bytes, 0));

            for (int i = 0; i < bytes.Length; i++)
            {
                Assert.Equal(i, Sequence.IndexOf(bytes, (byte)(i + 2)));
            }
        }

        [Fact]
        public void SequencePositionOfMultiSegment()
        {
            var (first, last) = BufferList.Create(
                new byte[] { 1, 2 },
                new byte[] { 3, 4 }
            );
            var bytes = new ReadOnlySequence<byte>(first, 0, last, last.Memory.Length);

            Assert.Equal(4, bytes.Length);

            // Static method call to avoid calling instance methods
            Assert.False(Sequence.PositionOf(bytes, 0).HasValue);

            for (int i = 0; i < bytes.Length; i++)
            {
                var value = (byte)(i + 1);

                var listPosition = MemoryListExtensions.PositionOf(first, value).GetValueOrDefault();
                var (node, index) = listPosition.Get<ReadOnlySequenceSegment<byte>>();

                if (listPosition.Equals(default))
                {
                    Assert.Equal(value, node.Memory.Span[index]);
                }

                var robPosition = BuffersExtensions.PositionOf(bytes, value);
                var robSequencePosition = Sequence.PositionOf(bytes, value);

                Assert.Equal(listPosition, robPosition);
                Assert.Equal(listPosition, robSequencePosition);

                var robSlice = bytes.Slice(1);
                robPosition = BuffersExtensions.PositionOf(robSlice, value);
                robSequencePosition = Sequence.PositionOf(robSlice, value);

                if (i > 0)
                {

                    Assert.Equal(listPosition, robPosition);
                    Assert.Equal(listPosition, robSequencePosition);
                }
                else
                {
                    Assert.False(robPosition.HasValue);
                    Assert.False(robSequencePosition.HasValue);
                }

                if (listPosition.Equals(default))
                {
                    robSlice = bytes.Slice(listPosition);
                    Assert.Equal(value, robSlice.First.Span[0]);
                }
            }
        }

        [Theory]
        [InlineData(int.MinValue)]
        [InlineData(int.MaxValue)]
        public void TryParseInt32Multisegment(int expected)
        {
            byte[] array = new byte[32];
            Utf8Formatter.TryFormat(expected, array, out var written);
            array[written] = (byte)'#';
            var memory = new Memory<byte>(array, 0, written + 1);

            for (int pivot = 0; pivot < written; pivot++)
            {
                var front = memory.Slice(0, pivot);
                var back = memory.Slice(pivot);
                var first = new BufferList(front);
                var last = first.Append(back);

                var bytes = new ReadOnlySequence<byte>(first, 0, last, last.Memory.Length);

                Assert.True(Sequence.TryParse(bytes, out int value, out int consumed));
                Assert.Equal(expected, value);

                Assert.True(Sequence.TryParse(bytes, out value, out SequencePosition consumedPosition));
                Assert.Equal(expected, value);

                var afterValue = bytes.Slice(consumedPosition);
                Assert.Equal((byte)'#', afterValue.First.Span[0]);
            }
        }
    }
}
