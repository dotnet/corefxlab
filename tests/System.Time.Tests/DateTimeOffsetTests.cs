// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Xunit;

namespace System.Time.Tests
{
    public class DateTimeOffsetTests
    {
        [Fact]
        public void CanAddYearsAcrossDstTransition()
        {
            var tz = TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time");
            var dto = new DateTimeOffset(2014, 3, 9, 0, 0, 0, TimeSpan.FromHours(-8));
            var result = dto.AddYears(1, tz);

            var expected = new DateTimeOffset(2015, 3, 9, 0, 0, 0, TimeSpan.FromHours(-7));
            Assert.Equal(expected, result);
            Assert.Equal(expected.Offset, result.Offset);
        }

        [Fact]
        public void CanAddMonthsAcrossDstTransition()
        {
            var tz = TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time");
            var dto = new DateTimeOffset(2015, 2, 9, 0, 0, 0, TimeSpan.FromHours(-8));
            var result = dto.AddMonths(1, tz);

            var expected = new DateTimeOffset(2015, 3, 9, 0, 0, 0, TimeSpan.FromHours(-7));
            Assert.Equal(expected, result);
            Assert.Equal(expected.Offset, result.Offset);
        }

        [Fact]
        public void CanAddDaysAcrossDstTransition()
        {
            var tz = TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time");
            var dto = new DateTimeOffset(2015, 3, 8, 0, 0, 0, TimeSpan.FromHours(-8));
            var result = dto.AddDays(1, tz);

            var expected = new DateTimeOffset(2015, 3, 9, 0, 0, 0, TimeSpan.FromHours(-7));
            Assert.Equal(expected, result);
            Assert.Equal(expected.Offset, result.Offset);
        }

        [Fact]
        public void CanAddDaysAcrossDstTransition_LandInGap()
        {
            var tz = TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time");
            var dto = new DateTimeOffset(2015, 3, 7, 2, 30, 0, TimeSpan.FromHours(-8));
            var result = dto.AddDays(1, tz);

            var expected = new DateTimeOffset(2015, 3, 8, 3, 30, 0, TimeSpan.FromHours(-7));
            Assert.Equal(expected, result);
            Assert.Equal(expected.Offset, result.Offset);
        }

        [Fact]
        public void CanAddDaysAcrossDstTransition_LandInOverlap()
        {
            var tz = TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time");
            var dto = new DateTimeOffset(2015, 10, 31, 1, 30, 0, TimeSpan.FromHours(-7));
            var result = dto.AddDays(1, tz);

            var expected = new DateTimeOffset(2015, 11, 1, 1, 30, 0, TimeSpan.FromHours(-7));
            Assert.Equal(expected, result);
            Assert.Equal(expected.Offset, result.Offset);
        }

        [Fact]
        public void CanAddDaysAcrossDstTransition_StartWithMismatchedOffset()
        {
            var tz = TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time");
            var dto = new DateTimeOffset(2015, 3, 8, 4, 0, 0, TimeSpan.FromHours(-4));
            var result = dto.AddDays(1, tz);

            var expected = new DateTimeOffset(2015, 3, 9, 0, 0, 0, TimeSpan.FromHours(-7));
            Assert.Equal(expected, result);
            Assert.Equal(expected.Offset, result.Offset);
        }

        [Fact]
        public void CanAddHoursAcrossDstTransition()
        {
            var tz = TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time");
            var dto = new DateTimeOffset(2015, 3, 8, 1, 0, 0, TimeSpan.FromHours(-8));
            var result = dto.AddHours(1, tz);

            var expected = new DateTimeOffset(2015, 3, 8, 3, 0, 0, TimeSpan.FromHours(-7));
            Assert.Equal(expected, result);
            Assert.Equal(expected.Offset, result.Offset);
        }

        [Fact]
        public void CanAddMinutesAcrossDstTransition()
        {
            var tz = TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time");
            var dto = new DateTimeOffset(2015, 3, 8, 1, 59, 0, TimeSpan.FromHours(-8));
            var result = dto.AddMinutes(1, tz);

            var expected = new DateTimeOffset(2015, 3, 8, 3, 0, 0, TimeSpan.FromHours(-7));
            Assert.Equal(expected, result);
            Assert.Equal(expected.Offset, result.Offset);
        }

        [Fact]
        public void CanAddSecondsAcrossDstTransition()
        {
            var tz = TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time");
            var dto = new DateTimeOffset(2015, 3, 8, 1, 59, 59, TimeSpan.FromHours(-8));
            var result = dto.AddSeconds(1, tz);

            var expected = new DateTimeOffset(2015, 3, 8, 3, 0, 0, TimeSpan.FromHours(-7));
            Assert.Equal(expected, result);
            Assert.Equal(expected.Offset, result.Offset);
        }

        [Fact]
        public void CanAddMillisecondsAcrossDstTransition()
        {
            var tz = TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time");
            var dto = new DateTimeOffset(2015, 3, 8, 1, 59, 59, 999, TimeSpan.FromHours(-8));
            var result = dto.AddMilliseconds(1, tz);

            var expected = new DateTimeOffset(2015, 3, 8, 3, 0, 0, TimeSpan.FromHours(-7));
            Assert.Equal(expected, result);
            Assert.Equal(expected.Offset, result.Offset);
        }

        [Fact]
        public void CanAddTicksAcrossDstTransition()
        {
            var tz = TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time");
            var dto = new DateTimeOffset(2015, 3, 8, 1, 59, 59, 999, TimeSpan.FromHours(-8)).AddTicks(9999);
            var result = dto.AddTicks(1, tz);

            var expected = new DateTimeOffset(2015, 3, 8, 3, 0, 0, TimeSpan.FromHours(-7));
            Assert.Equal(expected, result);
            Assert.Equal(expected.Offset, result.Offset);
        }
    }
}
