// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Xunit;

namespace System._Time.Tests
{
    public class TimeCastTests
    {
        [Fact]
        public void CanCastTimeToTimeSpanExplicitly()
        {
            Time t = Time.MaxValue;

            TimeSpan ts = (TimeSpan)t;

            Assert.Equal(0, ts.Days);
            Assert.Equal(t.Hour, ts.Hours);
            Assert.Equal(t.Minute, ts.Minutes);
            Assert.Equal(t.Second, ts.Seconds);
            Assert.Equal(t.Millisecond, ts.Milliseconds);
            Assert.Equal(t.Ticks % TimeSpan.TicksPerMillisecond, ts.Ticks % TimeSpan.TicksPerMillisecond);
        }

        [Fact]
        public void CanCastTimeToTimeSpanImplicitly()
        {
            Time t = Time.MaxValue;

            TimeSpan ts = t;

            Assert.Equal(0, ts.Days);
            Assert.Equal(t.Hour, ts.Hours);
            Assert.Equal(t.Minute, ts.Minutes);
            Assert.Equal(t.Second, ts.Seconds);
            Assert.Equal(t.Millisecond, ts.Milliseconds);
            Assert.Equal(t.Ticks % TimeSpan.TicksPerMillisecond, ts.Ticks % TimeSpan.TicksPerMillisecond);
        }

        [Fact]
        public void CanCastTimeSpanToTimeExplicitly()
        {
            TimeSpan ts = DateTime.MaxValue.TimeOfDay;

            Time t = (Time)ts;

            Assert.Equal(ts.Hours, t.Hour);
            Assert.Equal(ts.Minutes, t.Minute);
            Assert.Equal(ts.Seconds, t.Second);
            Assert.Equal(ts.Milliseconds, t.Millisecond);
            Assert.Equal(ts.Ticks % TimeSpan.TicksPerMillisecond, t.Ticks % TimeSpan.TicksPerMillisecond);
        }

        [Fact]
        public void CannotCastTimeSpanToTimeWhenNegative()
        {
            TimeSpan ts = TimeSpan.FromTicks(-1);

            Assert.Throws<InvalidCastException>(() =>
            {
                Time t = (Time)ts;
            });
        }

        [Fact]
        public void CannotCastTimeSpanToTimeWhenTooLarge()
        {
            TimeSpan ts = TimeSpan.FromHours(24);

            Assert.Throws<InvalidCastException>(() =>
            {
                Time t = (Time)ts;
            });
        }
    }
}
