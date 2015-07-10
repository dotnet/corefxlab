// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Xunit;

namespace System.Time.Tests
{
    public class TimeOfDayPropertyTests
    {
        [Fact]
        public void CanGetHourFromTimeOfDay()
        {
            TimeOfDay time = new TimeOfDay(23, 0);
            Assert.Equal(23, time.Hour);
        }

        [Fact]
        public void CanGetHourOf12HourClockFromTimeOfDay_12AM()
        {
            TimeOfDay time = new TimeOfDay(0, 0);
            Assert.Equal(12, time.HourOf12HourClock);
            Assert.Equal(Meridiem.AM, time.Meridiem);
        }

        [Fact]
        public void CanGetHourOf12HourClockFromTimeOfDay_01AM()
        {
            TimeOfDay time = new TimeOfDay(1, 0);
            Assert.Equal(1, time.HourOf12HourClock);
            Assert.Equal(Meridiem.AM, time.Meridiem);
        }

        [Fact]
        public void CanGetHourOf12HourClockFromTimeOfDay_11AM()
        {
            TimeOfDay time = new TimeOfDay(11, 0);
            Assert.Equal(11, time.HourOf12HourClock);
            Assert.Equal(Meridiem.AM, time.Meridiem);
        }

        [Fact]
        public void CanGetHourOf12HourClockFromTimeOfDay_12PM()
        {
            TimeOfDay time = new TimeOfDay(12, 0);
            Assert.Equal(12, time.HourOf12HourClock);
            Assert.Equal(Meridiem.PM, time.Meridiem);
        }

        [Fact]
        public void CanGetHourOf12HourClockFromTimeOfDay_01PM()
        {
            TimeOfDay time = new TimeOfDay(13, 0);
            Assert.Equal(1, time.HourOf12HourClock);
            Assert.Equal(Meridiem.PM, time.Meridiem);
        }

        [Fact]
        public void CanGetHourOf12HourClockFromTimeOfDay_11PM()
        {
            TimeOfDay time = new TimeOfDay(23, 0);
            Assert.Equal(11, time.HourOf12HourClock);
            Assert.Equal(Meridiem.PM, time.Meridiem);
        }

        [Fact]
        public void CanGetDateTimeFromTimeOnDate()
        {
            Date date = new Date(2000, 12, 31);
            TimeOfDay time = new TimeOfDay(23, 59, 59);
            DateTime dt = time.On(date);

            DateTime expected = new DateTime(2000, 12, 31, 23, 59, 59);
            Assert.Equal(expected, dt);
        }
    }
}
