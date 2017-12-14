// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Xunit;

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
                    Assert.Equal(length + first - 1, range.Last);

                    long sum = 0;
                    foreach (int value in range)
                    {
                        sum += value;
                    }

                    Assert.Equal((range.First + range.Last) * range.Length / 2, sum);

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
        public void Unbounded()
        {
            var unboundedLast = Range.Construct(10, Range.UnboundedLast);
            Assert.Equal(uint.MaxValue, unboundedLast.Length); // is that what we want?
            Assert.Equal(10, unboundedLast.First);
            Assert.Equal(Range.UnboundedLast, unboundedLast.Last);

            var unboundedFirst = Range.Construct(Range.UnboundedFirst, 10);
            Assert.Equal(uint.MaxValue, unboundedFirst.Length); // is that what we want?
            Assert.Equal(Range.UnboundedFirst, unboundedFirst.First);
            Assert.Equal(Range.UnboundedLast, unboundedFirst.Last); // is that what we want?

            var unbounded = Range.Construct(Range.UnboundedFirst, Range.UnboundedLast);
            Assert.Equal(uint.MaxValue, unbounded.Length); // is that what we want?
            Assert.Equal(Range.UnboundedFirst, unbounded.First);
            Assert.Equal(Range.UnboundedLast, unbounded.Last);
        }

        [Fact]
        public void Errors()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                var tooLong = new Range(int.MaxValue - 1, 2);
            });
        }
    }
}
