// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Xunit;

namespace System._Time.Tests
{
    public class TimePropertyTests
    {
        [Fact]
        public void CanGetHourFromTime()
        {
            Time time = new Time(23, 0);
            Assert.Equal(23, time.Hour);
        }

        [Fact]
        public void CanGetHourOf12HourClockFromTime_12AM()
        {
            Time time = new Time(0, 0);
            Assert.Equal(12, time.HourOf12HourClock);
            Assert.Equal(Meridiem.AM, time.Meridiem);
        }

        [Fact]
        public void CanGetHourOf12HourClockFromTime_01AM()
        {
            Time time = new Time(1, 0);
            Assert.Equal(1, time.HourOf12HourClock);
            Assert.Equal(Meridiem.AM, time.Meridiem);
        }

        [Fact]
        public void CanGetHourOf12HourClockFromTime_11AM()
        {
            Time time = new Time(11, 0);
            Assert.Equal(11, time.HourOf12HourClock);
            Assert.Equal(Meridiem.AM, time.Meridiem);
        }

        [Fact]
        public void CanGetHourOf12HourClockFromTime_12PM()
        {
            Time time = new Time(12, 0);
            Assert.Equal(12, time.HourOf12HourClock);
            Assert.Equal(Meridiem.PM, time.Meridiem);
        }

        [Fact]
        public void CanGetHourOf12HourClockFromTime_01PM()
        {
            Time time = new Time(13, 0);
            Assert.Equal(1, time.HourOf12HourClock);
            Assert.Equal(Meridiem.PM, time.Meridiem);
        }

        [Fact]
        public void CanGetHourOf12HourClockFromTime_11PM()
        {
            Time time = new Time(23, 0);
            Assert.Equal(11, time.HourOf12HourClock);
            Assert.Equal(Meridiem.PM, time.Meridiem);
        }

        [Fact]
        public void CanGetDateTimeFromTimeOnDate()
        {
            Date date = new Date(2000, 12, 31);
            Time time = new Time(23, 59, 59);
            DateTime dt = time.On(date);

            DateTime expected = new DateTime(2000, 12, 31, 23, 59, 59);
            Assert.Equal(expected, dt);
        }
    }
}
