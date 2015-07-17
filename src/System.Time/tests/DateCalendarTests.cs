// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Globalization;
using Xunit;

namespace System.Time.Tests
{
    public class DateCalendarTests
    {
        [Fact]
        public void CanCreateDateWithCalendar()
        {
            var actual = new Date(1436, 3, 10, new UmAlQuraCalendar());
            var expected = new Date(2015, 1, 1);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CanUseDateInCalendar()
        {
            var dt = new Date(2015, 1, 1);

            var calendar = new UmAlQuraCalendar();
            var year = calendar.GetYear(dt);
            var month = calendar.GetMonth(dt);
            var day = calendar.GetDayOfMonth(dt);
            
            Assert.Equal(1436, year);
            Assert.Equal(3, month);
            Assert.Equal(10, day);
        }
    }
}
