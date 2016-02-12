// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Linq;
using Xunit;

namespace System.Time.Tests
{
    public class DateMiscTests
    {
        [Fact]
        public void CanDetermineLeapYear_2000()
        {
            Assert.True(Date.IsLeapYear(2000));
        }

        [Fact]
        public void CanDetermineLeapYear_2001()
        {
            Assert.False(Date.IsLeapYear(2001));
        }

        [Fact]
        public void CanDetermineLeapYear_2004()
        {
            Assert.True(Date.IsLeapYear(2004));
        }

        [Fact]
        public void CanDetermineLeapYear_2100()
        {
            Assert.False(Date.IsLeapYear(2100));
        }

        [Fact]
        public void CannotDetermineLeapYear_TooSmall()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => Date.IsLeapYear(0));
        }

        [Fact]
        public void CannotDetermineLeapYear_TooLarge()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => Date.IsLeapYear(10000));
        }

        [Fact]
        public void CanDetermineDaysInMonth_LeapYear()
        {
            var expected = new[] {31, 29, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31};
            
            var actual = Enumerable.Range(1, 12).Select(m => Date.DaysInMonth(2000, m)).ToArray();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CanDetermineDaysInMonth_NonLeapYear()
        {
            var expected = new[] { 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };

            var actual = Enumerable.Range(1, 12).Select(m => Date.DaysInMonth(2001, m)).ToArray();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CannoDetermineDaysInMonth_YearTooLarge()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => Date.DaysInMonth(10000, 1));
        }

        [Fact]
        public void CannoDetermineDaysInMonth_YearTooSmall()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => Date.DaysInMonth(0, 1));
        }

        [Fact]
        public void CannoDetermineDaysInMonth_MonthTooLarge()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => Date.DaysInMonth(2000, 13));
        }

        [Fact]
        public void CannoDetermineDaysInMonth_MonthTooSmall()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => Date.DaysInMonth(2000, 0));
        }

    }
}
