// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Xunit;

namespace System._Time.Tests
{
    public class TimeMathTests
    {
        [Fact]
        public void CanCalculateDuration_Normal()
        {
            Time startTime = new Time(10, 0);
            Time endTime = new Time(12, 0);

            TimeSpan duration = endTime - startTime;
            Assert.Equal(TimeSpan.FromHours(2), duration);
        }

        [Fact]
        public void CanCalculateDuration_OverMidnight()
        {
            Time startTime = new Time(23, 0);
            Time endTime = new Time(1, 0);

            TimeSpan duration = endTime - startTime;
            Assert.Equal(TimeSpan.FromHours(2), duration);
        }

        [Fact]
        public void CanAddPositiveTime()
        {
            Time startTime = new Time(12, 0);
            Time actual = startTime.AddHours(13);
            Time expected = new Time(1, 0);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CanAddNegativeTime()
        {
            Time startTime = new Time(12, 0);
            Time actual = startTime.AddHours(-13);
            Time expected = new Time(23, 0);

            Assert.Equal(expected, actual);
        }
    }
}
