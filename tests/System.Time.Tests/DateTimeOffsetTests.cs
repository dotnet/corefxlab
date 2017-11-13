// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Xunit;

namespace System._Time.Tests
{
    public class DateTimeOffsetTests
    {
        [Fact]
        public void CanGetDateFromDateTimeOffset()
        {
            DateTimeOffset dto = DateTimeOffset.MaxValue;

            Date d = dto.GetDate();

            Assert.Equal(dto.Year, d.Year);
            Assert.Equal(dto.Month, d.Month);
            Assert.Equal(dto.Day, d.Day);
        }

        [Fact]
        public void CanGetTimeFromDateTimeOffset()
        {
            DateTimeOffset dto = DateTimeOffset.MaxValue;

            Time t = dto.GetTime();

            Assert.Equal(dto.Hour, t.Hour);
            Assert.Equal(dto.Minute, t.Minute);
            Assert.Equal(dto.Second, t.Second);
            Assert.Equal(dto.Millisecond, t.Millisecond);
            Assert.Equal(dto.Ticks % TimeSpan.TicksPerMillisecond, t.Ticks % TimeSpan.TicksPerMillisecond);
        }
    }
}
