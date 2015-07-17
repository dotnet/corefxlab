// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Xunit;

namespace System.Time.Tests
{
    public class DateTimeTests
    {
        [Fact]
        public void CanAssignDateTimeTodayToDate()
        {
            Date dt = DateTime.Today;
        }

        [Fact]
        public void CanAssignDateTimeTimeOfDayToTimeOfDay()
        {
            TimeOfDay time = DateTime.Now.TimeOfDay;
        }

        [Fact]
        public void CanAddYearsAcrossDstTransition()
        {
            var tz = TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time");
            var dt = new DateTime(2014, 3, 9, 0, 0, 0);
            var result = dt.AddYears(1, tz);

            var expected = new DateTimeOffset(2015, 3, 9, 0, 0, 0, TimeSpan.FromHours(-7));
            Assert.Equal(expected, result);
            Assert.Equal(expected.Offset, result.Offset);
        }

        [Fact]
        public void CanAddMonthsAcrossDstTransition()
        {
            var tz = TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time");
            var dt = new DateTime(2015, 2, 9, 0, 0, 0);
            var result = dt.AddMonths(1, tz);

            var expected = new DateTimeOffset(2015, 3, 9, 0, 0, 0, TimeSpan.FromHours(-7));
            Assert.Equal(expected, result);
            Assert.Equal(expected.Offset, result.Offset);
        }

        [Fact]
        public void CanAddDaysAcrossDstTransition()
        {
            var tz = TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time");
            var dt = new DateTime(2015, 3, 8, 0, 0, 0);
            var result = dt.AddDays(1, tz);

            var expected = new DateTimeOffset(2015, 3, 9, 0, 0, 0, TimeSpan.FromHours(-7));
            Assert.Equal(expected, result);
            Assert.Equal(expected.Offset, result.Offset);
        }

        [Fact]
        public void CanAddDaysAcrossDstTransition_LandInGap()
        {
            var tz = TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time");
            var dt = new DateTime(2015, 3, 7, 2, 30, 0);
            var result = dt.AddDays(1, tz);

            var expected = new DateTimeOffset(2015, 3, 8, 3, 30, 0, TimeSpan.FromHours(-7));
            Assert.Equal(expected, result);
            Assert.Equal(expected.Offset, result.Offset);
        }

        [Fact]
        public void CanAddDaysAcrossDstTransition_LandInOverlap()
        {
            var tz = TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time");
            var dt = new DateTime(2015, 10, 31, 1, 30, 0);
            var result = dt.AddDays(1, tz);

            var expected = new DateTimeOffset(2015, 11, 1, 1, 30, 0, TimeSpan.FromHours(-7));
            Assert.Equal(expected, result);
            Assert.Equal(expected.Offset, result.Offset);
        }

        [Fact]
        public void CanAddDaysAcrossDstTransition_StartWithMismatchedKind()
        {
            var tz = TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time");
            var dt = new DateTime(2015, 3, 8, 8, 0, 0, DateTimeKind.Utc);
            var result = dt.AddDays(1, tz);

            var expected = new DateTimeOffset(2015, 3, 9, 0, 0, 0, TimeSpan.FromHours(-7));
            Assert.Equal(expected, result);
            Assert.Equal(expected.Offset, result.Offset);
        }


        [Fact]
        public void CanAddHoursAcrossDstTransition()
        {
            var tz = TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time");
            var dt = new DateTime(2015, 3, 8, 1, 0, 0);
            var result = dt.AddHours(1, tz);

            var expected = new DateTimeOffset(2015, 3, 8, 3, 0, 0, TimeSpan.FromHours(-7));
            Assert.Equal(expected, result);
            Assert.Equal(expected.Offset, result.Offset);
        }

        [Fact]
        public void CanAddHoursAcrossDstTransition_StartWithMismatchedKind()
        {
            var tz = TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time");
            var dt = new DateTime(2015, 3, 8, 9, 0, 0, DateTimeKind.Utc);
            var result = dt.AddHours(1, tz);

            var expected = new DateTimeOffset(2015, 3, 8, 3, 0, 0, TimeSpan.FromHours(-7));
            Assert.Equal(expected, result);
            Assert.Equal(expected.Offset, result.Offset);
        }

        [Fact]
        public void CanAddMinutesAcrossDstTransition()
        {
            var tz = TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time");
            var dt = new DateTime(2015, 3, 8, 1, 59, 0);
            var result = dt.AddMinutes(1, tz);

            var expected = new DateTimeOffset(2015, 3, 8, 3, 0, 0, TimeSpan.FromHours(-7));
            Assert.Equal(expected, result);
            Assert.Equal(expected.Offset, result.Offset);
        }

        [Fact]
        public void CanAddSecondsAcrossDstTransition()
        {
            var tz = TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time");
            var dt = new DateTime(2015, 3, 8, 1, 59, 59);
            var result = dt.AddSeconds(1, tz);

            var expected = new DateTimeOffset(2015, 3, 8, 3, 0, 0, TimeSpan.FromHours(-7));
            Assert.Equal(expected, result);
            Assert.Equal(expected.Offset, result.Offset);
        }

        [Fact]
        public void CanAddMillisecondsAcrossDstTransition()
        {
            var tz = TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time");
            var dt = new DateTime(2015, 3, 8, 1, 59, 59, 999);
            var result = dt.AddMilliseconds(1, tz);

            var expected = new DateTimeOffset(2015, 3, 8, 3, 0, 0, TimeSpan.FromHours(-7));
            Assert.Equal(expected, result);
            Assert.Equal(expected.Offset, result.Offset);
        }

        [Fact]
        public void CanAddTicksAcrossDstTransition()
        {
            var tz = TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time");
            var dt = new DateTime(2015, 3, 8, 1, 59, 59, 999).AddTicks(9999);
            var result = dt.AddTicks(1, tz);

            var expected = new DateTimeOffset(2015, 3, 8, 3, 0, 0, TimeSpan.FromHours(-7));
            Assert.Equal(expected, result);
            Assert.Equal(expected.Offset, result.Offset);
        }
    }
}
