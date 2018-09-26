// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

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
