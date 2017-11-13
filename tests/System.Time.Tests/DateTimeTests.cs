// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Xunit;

namespace System._Time.Tests
{
    public class DateTimeTests
    {
        [Fact]
        public void CanGetDateFromDateTime()
        {
            DateTime dt = DateTime.MaxValue;

            Date d = dt.GetDate();

            Assert.Equal(dt.Year, d.Year);
            Assert.Equal(dt.Month, d.Month);
            Assert.Equal(dt.Day, d.Day);
        }

        [Fact]
        public void CanGetTimeFromDateTime()
        {
            DateTime dt = DateTime.MaxValue;

            Time t = dt.GetTime();

            Assert.Equal(dt.Hour, t.Hour);
            Assert.Equal(dt.Minute, t.Minute);
            Assert.Equal(dt.Second, t.Second);
            Assert.Equal(dt.Millisecond, t.Millisecond);
            Assert.Equal(dt.Ticks % TimeSpan.TicksPerMillisecond, t.Ticks % TimeSpan.TicksPerMillisecond);
        }
    }
}
