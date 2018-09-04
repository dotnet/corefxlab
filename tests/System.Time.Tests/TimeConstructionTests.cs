// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Xunit;

namespace System._Time.Tests
{
    public class TimeConstructionTests
    {
        [Fact]
        public void CanConstructDefaultTime()
        {
            Time time = new Time();
            Assert.Equal(0, time.Ticks);
        }

        [Fact]
        public void CanConstructTimeFromTicks()
        {
            Time time = new Time(863999999999L);
            Assert.Equal(863999999999L, time.Ticks);
        }

        [Fact]
        public void CanConstructTimeFrom24HoursAndMinutes()
        {
            Time time = new Time(23, 59);
            const long expected = 23 * TimeSpan.TicksPerHour +
                                  59 * TimeSpan.TicksPerMinute;
            Assert.Equal(expected, time.Ticks);
        }

        [Fact]
        public void CanConstructTimeFrom12HoursAndMinutes()
        {
            Time time = new Time(11, 59, Meridiem.PM);
            const long expected = 23 * TimeSpan.TicksPerHour +
                                  59 * TimeSpan.TicksPerMinute;
            Assert.Equal(expected, time.Ticks);
        }

        [Fact]
        public void CanConstructTimeFrom24HoursAndMinutesAndSeconds()
        {
            Time time = new Time(23, 59, 59);
            const long expected = 23 * TimeSpan.TicksPerHour +
                                  59 * TimeSpan.TicksPerMinute +
                                  59 * TimeSpan.TicksPerSecond;
            Assert.Equal(expected, time.Ticks);
        }

        [Fact]
        public void CanConstructTimeFrom12HoursAndMinutesAndSeconds()
        {
            Time time = new Time(11, 59, 59, Meridiem.PM);
            const long expected = 23 * TimeSpan.TicksPerHour +
                                  59 * TimeSpan.TicksPerMinute +
                                  59 * TimeSpan.TicksPerSecond;
            Assert.Equal(expected, time.Ticks);
        }

        [Fact]
        public void CanConstructTimeFrom24HoursAndMinutesAndSecondsAndMilliseconds()
        {
            Time time = new Time(23, 59, 59, 59);
            const long expected = 23 * TimeSpan.TicksPerHour +
                                  59 * TimeSpan.TicksPerMinute +
                                  59 * TimeSpan.TicksPerSecond +
                                  59 * TimeSpan.TicksPerMillisecond;
            Assert.Equal(expected, time.Ticks);
        }

        [Fact]
        public void CanConstructTimeFrom12HoursAndMinutesAndSecondsAndMilliseconds()
        {
            Time time = new Time(11, 59, 59, 59, Meridiem.PM);
            const long expected = 23 * TimeSpan.TicksPerHour +
                                  59 * TimeSpan.TicksPerMinute +
                                  59 * TimeSpan.TicksPerSecond +
                                  59 * TimeSpan.TicksPerMillisecond;
            Assert.Equal(expected, time.Ticks);
        }
    }
}
