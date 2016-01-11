// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Globalization;
using Xunit;

namespace System.Time.Tests
{
    public class TimeOfDayFormattingTests
    {
        public TimeOfDayFormattingTests()
        {
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
            CultureInfo.CurrentUICulture = CultureInfo.InvariantCulture;
        }

        [Fact]
        public void ToLongTimeString()
        {
            var time = new TimeOfDay(10, 49, 12, Meridiem.PM);
            var s = time.ToLongTimeString();
            Assert.Equal("22:49:12", s);
        }

        [Fact]
        public void ToLongTimeStringInvariant()
        {
            var time = new TimeOfDay(10, 49, 12, Meridiem.PM);
            var s = time.ToLongTimeStringInvariant();
            Assert.Equal("22:49:12", s);
        }

        [Fact]
        public void ToShortTimeString()
        {
            var time = new TimeOfDay(22, 49);
            var s = time.ToShortTimeString();
            Assert.Equal("22:49", s);
        }

        [Fact]
        public void ToShortTimeStringInvariant()
        {
            var time = new TimeOfDay(22, 49);
            var s = time.ToShortTimeStringInvariant();
            Assert.Equal("22:49", s);
        }

        [Fact]
        public void ToStringWithStandardTimeFormat()
        {
            var time = new TimeOfDay(23, 59, 59);
            var s = time.ToString("t");
            Assert.Equal("23:59", s);
        }

        [Fact]
        public void ToStringWithNullTimeFormat()
        {
            var time = new TimeOfDay(23, 59, 59);
            var s = time.ToString((string) null);
            Assert.Equal("23:59:59", s);
        }

        [Fact]
        public void ToStringWithEmptyTimeFormat()
        {
            var time = new TimeOfDay(23, 59, 59);
            var s = time.ToString("");
            Assert.Equal("23:59:59", s);
        }

        [Fact]
        public void ToStringWithCustomTimeFormat()
        {
            var time = new TimeOfDay(23, 59, 59);
            var s = time.ToString("HH:mm:ss");
            Assert.Equal("23:59:59", s);
        }

        [Fact]
        public void ToStringWithISOTimeFormat_O()
        {
            var time = new TimeOfDay(23, 59, 59);
            var s = time.ToString("O");
            Assert.Equal("23:59:59.0000000", s);
        }

        [Fact]
        public void ToStringWithISOTimeFormat_o()
        {
            var time = new TimeOfDay(23, 59, 59);
            var s = time.ToString("o");
            Assert.Equal("23:59:59.0000000", s);
        }

        [Fact]
        public void ToStringWithISOTimeFormat_s()
        {
            var time = new TimeOfDay(23, 59, 59);
            var s = time.ToString("s");
            Assert.Equal("23:59:59", s);
        }

        [Fact]
        public void ToStringWithCustomDateTimeFormat()
        {
            Assert.Throws<FormatException>(() => new TimeOfDay(23, 59, 59).ToString("yyyy-MM-dd HH:mm:ss"));
        }

        [Fact]
        public void ToStringWithStandardDateFormat()
        {
            Assert.Throws<FormatException>(() => new TimeOfDay(23, 59, 59).ToString("d"));
        }

        [Fact]
        public void ToStringWithCustomDateFormat()
        {
            Assert.Throws<FormatException>(() => new TimeOfDay(23, 59, 59).ToString("dd MMM yyyy"));
        }
    }
}
