using System.Collections.Sequences;
using Xunit;

namespace System.Buffers.Tests
{
    public class SequenceExtensionsTests
    {
        [Fact]
        public void SequenceIndexOfSingleSegment()
        {
            var array = new byte[] { 1, 2, 3, 4, 5 };
            var bytes = new ReadOnlyBytes(array);
            Assert.Equal(array.Length, bytes.Length);

            // Static method call to avoid calling ReadOnlyBytes.IndexOf
            Assert.Equal(-1, SequenceExtensions.IndexOf(bytes, 0));

            for (int i = 0; i < array.Length; i++)
            {
                Assert.Equal(i, SequenceExtensions.IndexOf(bytes, (byte)(i+1)));
            }
        }

        [Fact]
        public void SequenceIndexOfMultiSegment()
        {
            ReadOnlyBytes bytes = ListHelper.CreateRob(
                new byte[] { 1, 2},
                new byte[] { 3, 4 }
            );

            Assert.Equal(4, bytes.Length);

            // Static method call to avoid calling ReadOnlyBytes.IndexOf
            Assert.Equal(-1, SequenceExtensions.IndexOf(bytes, 0));

            for (int i = 0; i < bytes.Length; i++)
            {
                Assert.Equal(i, SequenceExtensions.IndexOf(bytes, (byte)(i + 1)));
            }
        }

        [Fact]
        public void SequenceIndexOfMultiSegmentSliced()
        {
            ReadOnlyBytes bytes = ListHelper.CreateRob(
                new byte[] { 1, 2 },
                new byte[] { 3, 4 }
            );
            bytes = bytes.Slice(1);

            Assert.Equal(3, bytes.Length);

            // Static method call to avoid calling ReadOnlyBytes.IndexOf
            Assert.Equal(-1, SequenceExtensions.IndexOf(bytes, 0));

            for (int i = 0; i < bytes.Length; i++)
            {
                Assert.Equal(i, SequenceExtensions.IndexOf(bytes, (byte)(i + 2)));
            }
        }

        [Fact]
        public void SequencePositionOfMultiSegment()
        {
            var (list, length) = MemoryList.Create(
                new byte[] { 1, 2 },
                new byte[] { 3, 4 }
            );
            var bytes = new ReadOnlyBytes(list, length);

            Assert.Equal(4, length);
            Assert.Equal(4, bytes.Length);

            // Static method call to avoid calling instance methods
            Assert.Equal(Position.End, SequenceExtensions.PositionOf(list, 0));

            for (int i = 0; i < bytes.Length; i++)
            {
                var value = (byte)(i + 1);
                var position = SequenceExtensions.PositionOf(list, value);
                var (node, index) = position.Get<IMemoryList<byte>>();
                Assert.Equal(value, node.Memory.Span[index]);

                var robPosition = bytes.PositionOf(value);
                Assert.Equal(position, robPosition);

                var robSlice = bytes.Slice(1);
                robPosition = robSlice.PositionOf(value);
                if (i > 0)
                {
                    Assert.Equal(position, robPosition);
                }
                else
                {
                    Assert.Equal(Position.End, robPosition);
                }

                if (position != Position.End)
                {
                    robSlice = bytes.Slice(position);
                    Assert.Equal(value, robSlice.First.Span[0]);
                }
            }
        }
    }
}
