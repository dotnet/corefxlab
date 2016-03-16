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
    }
}
