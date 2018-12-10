// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Xunit;
using System.Linq;
using System.Buffers.Experimental;

namespace System.Buffers.Tests
{
    public class RangeTests
    {
        [Fact]
        public void Basics()
        {
            for (int first = -10; first < 10; first++)
            {
                for (uint length = 0; length < 20; length++)
                {
                    var range = new Range(first, length);

                    Assert.Equal(first, range.First);
                    Assert.Equal(length, range.Length);
                    Assert.Equal(length + first, range.End);

                    long sum = 0;
                    uint numberOfItems = 0;
                    foreach (int value in range)
                    {
                        numberOfItems++;
                        sum += value;
                    }

                    Assert.Equal(numberOfItems, range.Length);
                    Assert.Equal((range.First + (long)range.End - 1) * range.Length / 2, sum);

                    (int f, int l) = range;
                    Assert.Equal(range.First, f);
                    Assert.Equal(range.End, l);

                    var constructed = Range.Construct(first, range.End);
                    Assert.Equal(range.First, constructed.First);
                    Assert.Equal(range.End, constructed.End);
                    Assert.Equal(range.Length, constructed.Length);
                }
            }
        }

        [Fact]
        public void FirstIsInclusiveEndIsExclusive()
        {
            var range = Range.Construct(10, 15);
            Assert.Equal(10, range.First());
            Assert.Equal(14, range.Last());
        }

        [Fact]
        public void UnboundedEnd()
        {
            var unboundedEnd = Range.Construct(10, Range.UnboundedEnd);
            Assert.False(unboundedEnd.IsBound); 
            Assert.Equal(10, unboundedEnd.First);
            Assert.Equal(Range.UnboundedEnd, unboundedEnd.End);          
        }

        [Fact]
        public void UnboundedFirst()
        {
            var unboundedFirst = Range.Construct(Range.UnboundedFirst, 10);
            Assert.False(unboundedFirst.IsBound);
            Assert.Equal(Range.UnboundedFirst, unboundedFirst.First);
            Assert.Equal(10, unboundedFirst.End);           
        }

        [Fact]
        public void Unbounded()
        {
            var unbounded = Range.Construct(Range.UnboundedFirst, Range.UnboundedEnd);
            Assert.False(unbounded.IsBound);
            Assert.Equal(Range.UnboundedFirst, unbounded.First);
            Assert.Equal(Range.UnboundedEnd, unbounded.End);
        }

        [Theory]
        [InlineData(0, 0, 0, true)]
        [InlineData(0, 1, 1, true)]
        [InlineData(0, 1, 0, false)] // non empty range not valid for empty array
        [InlineData(-1, 0, 0, false)] // lower bound negative
        public void IsValidArrayRange(int first, int end, int length, bool result)
        {
            var range = Range.Construct(first, end);
            Assert.Equal(result, range.IsValid(length));
        }

        [Theory]
        [InlineData(Range.UnboundedFirst, 0, 0, 0, 0)]
        [InlineData(Range.UnboundedFirst, 1, 10, 0, 1)]
        [InlineData(0, Range.UnboundedEnd, 1, 0, 1)]
        public void Bind(int first, int end, int length, int resultFirst, int resultEnd)
        {
            var range = Range.Construct(first, end);
            var result = range.Bind(length);
            Assert.Equal(resultFirst, result.First);
            Assert.Equal(resultEnd, result.End);
        }

        [Theory]
        [InlineData(1, 0, Range.UnboundedEnd, 1)]
        [InlineData(1, 1, Range.UnboundedEnd, 0)]
        [InlineData(1, 3, Range.UnboundedEnd, 0)]
        public void BindToValid(int arrayLength, int first, int end, uint boundRangeLength)
        {
            var range = Range.Construct(first, end);
            var result = range.BindToValid(arrayLength);
            Assert.Equal(boundRangeLength, result.Length);
            Assert.Equal(boundRangeLength == 0 ? arrayLength : first, result.First);
        }

        [Theory]
        [InlineData(int.MaxValue - 1, 0, int.MaxValue - 1)]
        [InlineData(int.MinValue + 1, 0, int.MinValue + 1)]
        [InlineData(int.MaxValue - 2, 1, int.MaxValue - 1)]
        [InlineData(int.MinValue + 1, 1, int.MinValue + 2)]
        // full range
        [InlineData(int.MinValue + 1, uint.MaxValue - 2, int.MaxValue -1)]
        public void BoundaryConditions(int first, uint length, int end)
        {
            var empty = new Range(first, length);
            Assert.Equal(length, empty.Length);
            Assert.Equal(first, empty.First);
            Assert.Equal(end, empty.End);
        }

        [Fact]
        public void Errors()
        {
            var max = new Range(int.MaxValue - 1, 1);
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                // MinValue is used as a sentinel for First, so cannot be used with Length
                var tooLong = new Range(int.MinValue, 0);
            });
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                // MaxValue is used as a sentinel for End, so End cannot end up being it.
                var tooLong = new Range(int.MaxValue, 1);
            });
            Assert.Throws<InvalidOperationException>(() =>
            {
                // Cannot enumerate unbound
                var unbound = Range.Construct(Range.UnboundedFirst, 1);
                unbound.GetEnumerator();
            });
            Assert.Throws<InvalidOperationException>(() =>
            {
                // Cannot enumerate unbound
                var unbound = Range.Construct(1, Range.UnboundedEnd);
                unbound.GetEnumerator();
            });
            Assert.Throws<InvalidOperationException>(() =>
            {
                // Cannot get length on unbound
                var unbound = Range.Construct(Range.UnboundedFirst, 1);
                var length = unbound.Length;
            });
            Assert.Throws<InvalidOperationException>(() =>
            {
                // Cannot get length on unbound
                var unbound = Range.Construct(1, Range.UnboundedEnd);
                var length = unbound.Length;
            });
        }

        [Fact]
        public void UnboundedRangeCannotHaveLengthSpecified()
        {
            for (uint length = 0; length < 10; length++)
            {
                Assert.Throws<ArgumentOutOfRangeException>(() =>
                {
                    // Unbound Range cannot have length
                    var invalid = new Range(Range.UnboundedFirst, length);
                });
            }
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                // Unbound Range cannot have length
                var invalid = new Range(Range.UnboundedFirst, uint.MaxValue);
            });
        }
    }
}
