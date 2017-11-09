// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Xunit;

namespace System._Time.Tests
{
    public class DateCastTests
    {
        [Fact]
        public void CanCastDateToDateTimeExplicitly()
        {
            Date d = Date.MaxValue;

            DateTime dt = (DateTime)d;

            Assert.Equal(d.Year, dt.Year);
            Assert.Equal(d.Month, dt.Month);
            Assert.Equal(d.Day, dt.Day);
            Assert.Equal(TimeSpan.Zero, dt.TimeOfDay);
            Assert.Equal(DateTimeKind.Unspecified, dt.Kind);
        }

        [Fact]
        public void CanCastDateToDateTimeImplicitly()
        {
            Date d = Date.MaxValue;

            DateTime dt = d;

            Assert.Equal(d.Year, dt.Year);
            Assert.Equal(d.Month, dt.Month);
            Assert.Equal(d.Day, dt.Day);
            Assert.Equal(TimeSpan.Zero, dt.TimeOfDay);
            Assert.Equal(DateTimeKind.Unspecified, dt.Kind);
        }

        [Fact]
        public void CanCastDateTimeToDateExplicitly()
        {
            DateTime dt = DateTime.MaxValue.Date;

            Date d = (Date)dt;

            Assert.Equal(dt.Year, d.Year);
            Assert.Equal(dt.Month, d.Month);
            Assert.Equal(dt.Day, d.Day);
        }

        [Fact]
        public void CannotCastDateTimeToDateWhenTimeNotZero()
        {
            DateTime dt = DateTime.MinValue.AddTicks(1);

            Assert.Throws<InvalidCastException>(() =>
            {
                Date d = (Date)dt;
            });
        }
    }
}
