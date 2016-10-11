using System.Text.Utf8;
using Xunit;
using System.Text;
using System.Collections.Generic;
using System.Collections.Sequences;

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

        [Fact]
        public void SequenceWorks()
        {
            var ms = new Multispan<byte>();
            Initialize(ref ms);
            var values = new byte[] { 1, 2, 7 };
            ms[0].Set(values);
            ms[1].Set(values);
            ms[2].Set(values);

            var sum = ms.Do((all) => {
                int acumulator = 0;
                foreach (var i in all) acumulator += i;
                return acumulator;
            });

            Assert.Equal(30, sum);

            int capturedSum = 0;
            ms.Do((all) => {
                foreach (var i in all) capturedSum += i;
            });
            Assert.Equal(30, capturedSum);
        }

        [Fact]
        public void SingleSpanMultispanBasics()
        {
            var ints = new Multispan<int>();

            Assert.Equal(0, ints.Count);

            int index1 = ints.AppendNewSegment(10);
            Assert.Equal(1, ints.Count);
            Assert.Equal(0, index1);
            Span<int> segment1 = ints[index1];
            Assert.True(segment1.Length >= 10);
            var sliced = ints.Slice(1);

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

        [Fact]
        public void SpanEnumeration()
        {
            Span<byte> span;
            {
                Multispan<byte> collection = ToMultispan("A");
                Assert.Equal(1, collection.Count);
                Position position = Position.First;

                Assert.True(collection.TryGet(ref position, out span, advance: true));
                Assert.True(position.Equals(Position.AfterLast));
                Assert.Equal(span[0], (byte)'A');
                collection.Dispose();
            }
            {
                Multispan<byte> collection = ToMultispan("A", "B");
                Assert.Equal(2, collection.Count);
                Position position = Position.First;
                Assert.True(collection.TryGet(ref position, out span, advance: true));
                Assert.Equal(span[0], (byte)'A');
                Assert.True(collection.TryGet(ref position, out span, advance: true));
                Assert.Equal(span[0], (byte)'B');
                Assert.True(position.Equals(Position.AfterLast));
                collection.Dispose();
            }
        }

        [Fact]
        public void ReadOnlySpanEnumeration()
        {
            ReadOnlySpan<byte> span;
            {
                Multispan<byte> collection = ToMultispan("A");
                Assert.Equal(1, collection.Count);
                Position position = Position.First;

                Assert.True(collection.TryGet(ref position, out span, advance: true));
                Assert.True(position.Equals(Position.AfterLast));
                Assert.Equal(span[0], (byte)'A');
                collection.Dispose();
            }
            {
                Multispan<byte> collection = ToMultispan("A", "B");
                Assert.Equal(2, collection.Count);
                Position position = Position.First;
                Assert.True(collection.TryGet(ref position, out span, advance: true));
                Assert.Equal(span[0], (byte)'A');
                Assert.True(collection.TryGet(ref position, out span, advance: true));
                Assert.Equal(span[0], (byte)'B');
                Assert.True(position.Equals(Position.AfterLast));
                collection.Dispose();
            }
        }

        [Fact]
        public void MultispanParsing()
        {
            uint value;
            Multispan<byte> collection;

            collection = ToMultispan("12");
            value = collection.ParseUInt32();
            Assert.Equal(12u, value);
            collection.Dispose();

            collection = ToMultispan("12_", "34");
            value = collection.ParseUInt32();
            Assert.Equal(12u, value);
            collection.Dispose();

            collection = ToMultispan("12", "34");
            value = collection.ParseUInt32();
            Assert.Equal(1234u, value);
            collection.Dispose();

            collection = ToMultispan("12", "34_", "56");
            value = collection.ParseUInt32();
            Assert.Equal(1234u, value);
            collection.Dispose();

            // TODO: this is NYI
            //collection = ToMultispan("12", "34", "56");
            //value = collection.ParseUInt32();
            //Assert.Equal(123456u, value);
            //collection.Dispose();
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

        public static Multispan<byte> ToMultispan(params string[] sections)
        {
            var result = new Multispan<byte>();
            for (int i = 0; i < sections.Length; i++) {
                var bytes = Encoding.UTF8.GetBytes(sections[i]);
                result.AppendNewSegment(bytes.Length);
                result.Last.Set(bytes);
                result.ResizeSegment(i, bytes.Length);
            }
            return result;
        }
    }

    static class TestingExtensions
    {
        public static uint ParseUInt32(this Multispan<byte> bytes)
        {
            int consumed;
            uint value;
            if (!bytes.TryParseUInt32(EncodingData.InvariantUtf8, out value, out consumed)) {
                throw new ArgumentException();
            }
            return value;
        }
    }
}
