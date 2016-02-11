// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Xunit;

namespace System.Time.Tests
{
    public class DateConstructionTests
    {
        [Fact]
        public void CanConstructDefaultDate()
        {
            Date date = new Date();
            Assert.Equal(0, date.DayNumber);
        }

        [Fact]
        public void CanConstructDateFromDayNumber()
        {
            Date date = new Date(3652058);
            Assert.Equal(3652058, date.DayNumber);
        }

        [Fact]
        public void CannotConstructDateFromDayNumberTooLarge()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new Date(3652059));
        }

        [Fact]
        public void CannotConstructDateFromDayNumberTooSmall()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new Date(-1));
        }

        [Fact]
        public void CanConstructDateFromParts_Min()
        {
            Date date = new Date(1, 1, 1);
            Assert.Equal(0, date.DayNumber);
        }

        [Fact]
        public void CanConstructDateFromParts_Max()
        {
            Date date = new Date(9999, 12, 31);
            Assert.Equal(3652058, date.DayNumber);
        }

        [Fact]
        public void CanConstructDateFromParts_MaxDay_31()
        {
            Date date = new Date(2000, 1, 31);
            Assert.Equal(730149, date.DayNumber);
        }

        [Fact]
        public void CanConstructDateFromParts_MaxDay_30()
        {
            Date date = new Date(2000, 4, 30);
            Assert.Equal(730239, date.DayNumber);
        }

        [Fact]
        public void CanConstructDateFromParts_MaxDay_29()
        {
            Date date = new Date(2000, 2, 29);
            Assert.Equal(730178, date.DayNumber);
        }

        [Fact]
        public void CanConstructDateFromParts_MaxDay_28()
        {
            Date date = new Date(2001, 2, 28);
            Assert.Equal(730543, date.DayNumber);
        }

        [Fact]
        public void CannotConstructDateFromParts_YearTooSmall()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new Date(-1, 1, 1));
        }

        [Fact]
        public void CannotConstructDateFromParts_YearTooLarge()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new Date(10000, 1, 1));
        }

        [Fact]
        public void CannotConstructDateFromParts_MonthTooSmall()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new Date(2000, -1, 1));
        }

        [Fact]
        public void CannotConstructDateFromParts_MonthTooLarge()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new Date(2000, 13, 1));
        }

        [Fact]
        public void CannotConstructDateFromParts_DayTooSmall()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new Date(2000, 1, -1));
        }

        [Fact]
        public void CannotConstructDateFromParts_DayTooLarge_31()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new Date(2000, 1, 32));
        }

        [Fact]
        public void CannotConstructDateFromParts_DayTooLarge_30()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new Date(2000, 4, 31));
        }

        [Fact]
        public void CannotConstructDateFromParts_DayTooLarge_29()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new Date(2000, 2, 30));
        }

        [Fact]
        public void CannotConstructDateFromParts_DayTooLarge_28()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new Date(2001, 2, 29));
        }

        [Fact]
        public void CanConstructDateFromYearAndDayOfYear_NonLeap()
        {
            Date date = new Date(2001, 365);
            Date expected = new Date(2001, 12, 31);
            Assert.Equal(expected, date);
        }

        [Fact]
        public void CanConstructDateFromYearAndDayOfYear_Leap()
        {
            Date date = new Date(2000, 366);
            Date expected = new Date(2000, 12, 31);
            Assert.Equal(expected, date);
        }

        [Fact]
        public void CannotConstructDateFromYearAndDayOfYear_NonLeap_TooLarge()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new Date(2001, 366));
        }

        [Fact]
        public void CannotConstructDateFromYearAndDayOfYear_Leap_TooLarge()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new Date(2000, 367));
        }

        [Fact]
        public void CannotConstructDateFromYearAndDayOfYear_TooSmall()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new Date(2000, 0));
        }
    }
}
