// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Xunit;

namespace System._Time.Tests
{
    public class DatePropertyTests
    {
        [Fact]
        public void CanGetYearFromDate()
        {
            Date date = new Date(2015, 12, 31);
            Assert.Equal(2015, date.Year);
        }

        [Fact]
        public void CanGetMonthFromDate()
        {
            Date date = new Date(2015, 12, 31);
            Assert.Equal(12, date.Month);
        }

        [Fact]
        public void CanGetDayFromDate()
        {
            Date date = new Date(2015, 12, 31);
            Assert.Equal(31, date.Day);
        }

        [Fact]
        public void CanGetDayFromDate_DayAfterLeapDay1()
        {
            Date date = new Date(2000, 3, 1);
            Assert.Equal(1, date.Day);
        }

        [Fact]
        public void CanGetDayFromDate_DayAfterLeapDay2()
        {
            Date date = new Date(2004, 3, 1);
            Assert.Equal(1, date.Day);
        }

        [Fact]
        public void CanGetDayFromDate_DayAfterNonLeapDay1()
        {
            Date date = new Date(2001, 3, 1);
            Assert.Equal(1, date.Day);
        }

        [Fact]
        public void CanGetDayFromDate_DayAfterNonLeapDay2()
        {
            Date date = new Date(1900, 3, 1);
            Assert.Equal(1, date.Day);
        }

        [Fact]
        public void CanGetDayOfWeekFromDate()
        {
            Date date = new Date(2015, 12, 31);
            Assert.Equal(DayOfWeek.Thursday, date.DayOfWeek);
        }

        [Fact]
        public void CanGetDayOfYearFromDate_StandardYear()
        {
            Date date = new Date(2015, 12, 31);
            Assert.Equal(365, date.DayOfYear);
        }

        [Fact]
        public void CanGetDayOfYearFromDate_LeapYear()
        {
            Date date = new Date(2000, 12, 31);
            Assert.Equal(366, date.DayOfYear);
        }

        [Fact]
        public void CanGetDateTimeFromDateAtTime()
        {
            Date date = new Date(2000, 12, 31);
            Time time = new Time(23, 59, 59);
            DateTime dt = date.At(time);

            DateTime expected = new DateTime(2000, 12, 31, 23, 59, 59);
            Assert.Equal(expected, dt);
        }
    }
}
