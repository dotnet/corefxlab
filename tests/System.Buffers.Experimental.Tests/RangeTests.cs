// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Xunit;
using System.Linq;

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
                    Assert.Equal(length + first, range.Last);

                    long sum = 0;
                    uint numberOfItems = 0;
                    foreach (int value in range)
                    {
                        numberOfItems++;
                        sum += value;
                    }

                    Assert.Equal(numberOfItems, range.Length);
                    Assert.Equal((range.First + (long)range.Last - 1) * range.Length / 2, sum);

                    (int f, int l) = range;
                    Assert.Equal(range.First, f);
                    Assert.Equal(range.Last, l);

                    var constructed = Range.Construct(first, range.Last);
                    Assert.Equal(range.First, constructed.First);
                    Assert.Equal(range.Last, constructed.Last);
                    Assert.Equal(range.Length, constructed.Length);
                }
            }
        }

        [Fact]
        public void FirstIsInclusiveLastIsExclusive()
        {
            var range = Range.Construct(10, 15);
            Assert.Equal(10, range.First());
            Assert.Equal(14, range.Last());
        }

        [Fact]
        public void UnboundedLast()
        {
            var unboundedLast = Range.Construct(10, Range.UnboundedLast);
            Assert.False(unboundedLast.IsBound); 
            Assert.Equal(10, unboundedLast.First);
            Assert.Equal(Range.UnboundedLast, unboundedLast.Last);          
        }

        [Fact]
        public void UnboundedFirst()
        {
            var unboundedFirst = Range.Construct(Range.UnboundedFirst, 10);
            Assert.False(unboundedFirst.IsBound);
            Assert.Equal(Range.UnboundedFirst, unboundedFirst.First);
            Assert.Equal(10, unboundedFirst.Last);           
        }

        [Fact]
        public void Unbounded()
        {
            var unbounded = Range.Construct(Range.UnboundedFirst, Range.UnboundedLast);
            Assert.False(unbounded.IsBound);
            Assert.Equal(Range.UnboundedFirst, unbounded.First);
            Assert.Equal(Range.UnboundedLast, unbounded.Last);
        }

        [Theory]
        [InlineData(int.MaxValue - 1, 0, int.MaxValue - 1)]
        [InlineData(int.MinValue + 1, 0, int.MinValue + 1)]
        [InlineData(int.MaxValue - 2, 1, int.MaxValue - 1)]
        [InlineData(int.MinValue + 1, 1, int.MinValue + 2)]
        // full range
        [InlineData(int.MinValue + 1, uint.MaxValue - 2, int.MaxValue -1)]
        public void BoundaryConditions(int first, uint length, int last)
        {
            var empty = new Range(first, length);
            Assert.Equal(length, empty.Length);
            Assert.Equal(first, empty.First);
            Assert.Equal(last, empty.Last);
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
                // MaxValue is used as a sentinel for Last, so Last cannot endup being it.
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
                var unbound = Range.Construct(1, Range.UnboundedLast);
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
                var unbound = Range.Construct(1, Range.UnboundedLast);
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
