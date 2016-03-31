using System.Buffers;
using System.Collections.Generic;
using Xunit;

namespace System.Buffers.Tests
{
    public class MultispanTests
    {
        [Fact]
        public void BasicsWork()
        {
            var ints = new Multispan<int>();
            Assert.Equal(0, ints.Count);

            int index1 = ints.AppendNewSegment(10);
            Assert.Equal(1, ints.Count);
            Assert.Equal(0, index1);
            Span<int> segment1 = ints[index1];
            Assert.True(segment1.Length >= 10);

            int index2 = ints.AppendNewSegment(1000);
            Assert.Equal(2, ints.Count);
            Assert.Equal(1, index2);
            Span<int> segment2 = ints[index2];
            Assert.True(segment2.Length >= 1000);

            var totalSize = segment1.Length + segment2.Length;
            var computedSize = ints.TotalItemCount();
            Assert.Equal(totalSize, computedSize);

            ints.ResizeSegment(0, 20);
            ints.ResizeSegment(1, 100);
            segment1 = ints[0];
            segment2 = ints[1];
            Assert.True(segment1.Length >= 20);
            Assert.True(segment2.Length >= 100);

            ints.Dispose();
            Assert.Equal(0, ints.Count);
        }

        [InlineData(0, 60)]
        [InlineData(5, 55)]
        [InlineData(10, 50)]
        [InlineData(15, 45)]
        [InlineData(30, 30)]
        [InlineData(35, 25)]
        [InlineData(60, 0)]
        [Theory]
        public void Slicing(int sliceIndex, int expectedTotalItems)
        {
            var ms = new Multispan<byte>();
            Initialize(ref ms);

            var slice = ms.Slice(sliceIndex);
            Assert.Equal(expectedTotalItems, slice.TotalItemCount());

            ms.Dispose();
        }

        private static void Initialize(ref Multispan<byte> ms)
        {
            Assert.Equal(0, ms.TotalItemCount());

            ms.AppendNewSegment(10);
            ms.ResizeSegment(0, 10);
            ms.AppendNewSegment(20);
            ms.ResizeSegment(1, 20);
            ms.AppendNewSegment(30);
            ms.ResizeSegment(2, 30);
            Assert.Equal(60, ms.TotalItemCount());
        }
    }
}
