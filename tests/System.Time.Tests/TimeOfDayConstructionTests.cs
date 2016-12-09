// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Xunit;

namespace System.Time.Tests
{
    public class TimeOfDayConstructionTests
    {
        [Fact]
        public void CanConstructDefaultTimeOfDay()
        {
            TimeOfDay time = new TimeOfDay();
            Assert.Equal(0, time.Ticks);
        }

        [Fact]
        public void CanConstructTimeOfDayFromTicks()
        {
            TimeOfDay time = new TimeOfDay(863999999999L);
            Assert.Equal(863999999999L, time.Ticks);
        }

        [Fact]
        public void CanConstructTimeOfDayFrom24HoursAndMinutes()
        {
            TimeOfDay time = new TimeOfDay(23, 59);
            const long expected = 23 * TimeSpan.TicksPerHour +
                                  59 * TimeSpan.TicksPerMinute;
            Assert.Equal(expected, time.Ticks);
        }

        [Fact]
        public void CanConstructTimeOfDayFrom12HoursAndMinutes()
        {
            TimeOfDay time = new TimeOfDay(11, 59, Meridiem.PM);
            const long expected = 23 * TimeSpan.TicksPerHour +
                                  59 * TimeSpan.TicksPerMinute;
            Assert.Equal(expected, time.Ticks);
        }

        [Fact]
        public void CanConstructTimeOfDayFrom24HoursAndMinutesAndSeconds()
        {
            TimeOfDay time = new TimeOfDay(23, 59, 59);
            const long expected = 23 * TimeSpan.TicksPerHour +
                                  59 * TimeSpan.TicksPerMinute +
                                  59 * TimeSpan.TicksPerSecond;
            Assert.Equal(expected, time.Ticks);
        }

        [Fact]
        public void CanConstructTimeOfDayFrom12HoursAndMinutesAndSeconds()
        {
            TimeOfDay time = new TimeOfDay(11, 59, 59, Meridiem.PM);
            const long expected = 23 * TimeSpan.TicksPerHour +
                                  59 * TimeSpan.TicksPerMinute +
                                  59 * TimeSpan.TicksPerSecond;
            Assert.Equal(expected, time.Ticks);
        }

        [Fact]
        public void CanConstructTimeOfDayFrom24HoursAndMinutesAndSecondsAndMilliseconds()
        {
            TimeOfDay time = new TimeOfDay(23, 59, 59, 59);
            const long expected = 23 * TimeSpan.TicksPerHour +
                                  59 * TimeSpan.TicksPerMinute +
                                  59 * TimeSpan.TicksPerSecond +
                                  59 * TimeSpan.TicksPerMillisecond;
            Assert.Equal(expected, time.Ticks);
        }

        [Fact]
        public void CanConstructTimeOfDayFrom12HoursAndMinutesAndSecondsAndMilliseconds()
        {
            TimeOfDay time = new TimeOfDay(11, 59, 59, 59, Meridiem.PM);
            const long expected = 23 * TimeSpan.TicksPerHour +
                                  59 * TimeSpan.TicksPerMinute +
                                  59 * TimeSpan.TicksPerSecond +
                                  59 * TimeSpan.TicksPerMillisecond;
            Assert.Equal(expected, time.Ticks);
        }
    }
}
