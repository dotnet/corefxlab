using System.IO.Pipelines;
using System.IO.Pipelines.Testing;
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
    }
}
