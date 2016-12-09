// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Xunit;

namespace System.Time.Tests
{
    public class DateMathTests
    {
        [Fact]
        public void CanAddPositiveYears()
        {
            var dt = new Date(2000, 1, 1);
            var actual = dt.AddYears(1);
            var expected = new Date(2001, 1, 1);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CanAddNegativeYears()
        {
            var dt = new Date(2000, 1, 1);
            var actual = dt.AddYears(-1);
            var expected = new Date(1999, 1, 1);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CanAddZeroYears()
        {
            var dt = new Date(2000, 1, 1);
            var actual = dt.AddYears(0);
            var expected = new Date(2000, 1, 1);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CannotAddYearsMoreThanMaxDate()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => Date.MaxValue.AddYears(1));
        }

        [Fact]
        public void CannotAddYearsLessThanMinDate()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => Date.MinValue.AddYears(-1));
        }

        [Fact]
        public void CannotAddYearsMoreThanMaxPossibleYears()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => Date.MinValue.AddYears(10000));
        }

        [Fact]
        public void CannotAddYearsLessThanMinPossibleYears()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => Date.MaxValue.AddYears(-10000));
        }

        [Fact]
        public void CanAddPositiveYears_FromLeapDay_ToNonLeapYear()
        {
            var dt = new Date(2000, 2, 29);
            var actual = dt.AddYears(1);
            var expected = new Date(2001, 2, 28);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CanAddNegativeYears_FromLeapDay_ToNonLeapYear()
        {
            var dt = new Date(2000, 2, 29);
            var actual = dt.AddYears(-1);
            var expected = new Date(1999, 2, 28);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CanAddPositiveYears_FromLeapDay_ToLeapYear()
        {
            var dt = new Date(2000, 2, 29);
            var actual = dt.AddYears(4);
            var expected = new Date(2004, 2, 29);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CanAddNegativeYears_FromLeapDay_ToLeapYear()
        {
            var dt = new Date(2000, 2, 29);
            var actual = dt.AddYears(-4);
            var expected = new Date(1996, 2, 29);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CanAddPositiveMonths()
        {
            var dt = new Date(2000, 1, 1);
            var actual = dt.AddMonths(1);
            var expected = new Date(2000, 2, 1);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CanAddNegativeMonths()
        {
            var dt = new Date(2000, 1, 1);
            var actual = dt.AddMonths(-1);
            var expected = new Date(1999, 12, 1);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CanAddZeroMonths()
        {
            var dt = new Date(2000, 1, 1);
            var actual = dt.AddMonths(0);
            var expected = new Date(2000, 1, 1);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CannotAddMonthsMoreThanMaxDate()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => Date.MaxValue.AddMonths(1));
        }

        [Fact]
        public void CannotAddMonthsLessThanMinDate()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => Date.MinValue.AddMonths(-1));
        }

        [Fact]
        public void CannotAddMonthsMoreThanMaxPossibleMonths()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => Date.MinValue.AddMonths(120000));
        }

        [Fact]
        public void CannotAddMonthsLessThanMinPossibleMonths()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => Date.MaxValue.AddMonths(-120000));
        }

        [Fact]
        public void CanAddPositiveMonths_FromDay31_ToDay30()
        {
            var dt = new Date(2000, 3, 31);
            var actual = dt.AddMonths(1);
            var expected = new Date(2000, 4, 30);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CanAddNegativeMonths_FromDay31_ToDay30()
        {
            var dt = new Date(2000, 5, 31);
            var actual = dt.AddMonths(-1);
            var expected = new Date(2000, 4, 30);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CanAddPositiveMonths_FromDay31_ToDay29()
        {
            var dt = new Date(2000, 1, 31);
            var actual = dt.AddMonths(1);
            var expected = new Date(2000, 2, 29);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CanAddNegativeMonths_FromDay31_ToDay29()
        {
            var dt = new Date(2000, 3, 31);
            var actual = dt.AddMonths(-1);
            var expected = new Date(2000, 2, 29);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CanAddPositiveMonths_FromDay31_ToDay28()
        {
            var dt = new Date(2001, 1, 31);
            var actual = dt.AddMonths(1);
            var expected = new Date(2001, 2, 28);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CanAddNegativeMonths_FromDay31_ToDay28()
        {
            var dt = new Date(2001, 3, 31);
            var actual = dt.AddMonths(-1);
            var expected = new Date(2001, 2, 28);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CanAddPositiveMonths_FromDay30_ToDay29()
        {
            var dt = new Date(1999, 11, 30);
            var actual = dt.AddMonths(3);
            var expected = new Date(2000, 2, 29);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CanAddNegativeMonths_FromDay30_ToDay29()
        {
            var dt = new Date(2000, 4, 30);
            var actual = dt.AddMonths(-2);
            var expected = new Date(2000, 2, 29);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CanAddPositiveMonths_FromDay30_ToDay28()
        {
            var dt = new Date(2000, 11, 30);
            var actual = dt.AddMonths(3);
            var expected = new Date(2001, 2, 28);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CanAddNegativeMonths_FromDay30_ToDay28()
        {
            var dt = new Date(2001, 4, 30);
            var actual = dt.AddMonths(-2);
            var expected = new Date(2001, 2, 28);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CanAddPositiveDays()
        {
            var dt = new Date(2000, 1, 1);
            var actual = dt.AddDays(1);
            var expected = new Date(2000, 1, 2);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CanAddZeroDays()
        {
            var dt = new Date(2000, 1, 1);
            var actual = dt.AddDays(0);
            var expected = new Date(2000, 1, 1);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CanAddNegativeDays()
        {
            var dt = new Date(2000, 1, 1);
            var actual = dt.AddDays(-1);
            var expected = new Date(1999, 12, 31);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CannotAddDaysMoreThanMaxDate()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => Date.MaxValue.AddDays(1));
        }

        [Fact]
        public void CannotAddDaysLessThanMinDate()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => Date.MinValue.AddDays(-1));
        }

        [Fact]
        public void CannotAddDaysMoreThanMaxPossibleDays()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => Date.MinValue.AddDays(3652059));
        }

        [Fact]
        public void CannotAddDaysLessThanMinPossibleDays()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => Date.MaxValue.AddDays(-3652059));
        }

        [Fact]
        public void CanCalculateDaysUntilDate_Positive()
        {
            Date start = new Date(2000, 1, 1);
            Date end = new Date(2000, 1, 2);

            int days = start.DaysUntil(end);

            Assert.Equal(1, days);
        }

        [Fact]
        public void CanCalculateDaysUntilDate_Zero()
        {
            Date start = new Date(2000, 1, 1);
            Date end = new Date(2000, 1, 1);

            int days = start.DaysUntil(end);

            Assert.Equal(0, days);
        }

        [Fact]
        public void CanCalculateDaysUntilDate_Negative()
        {
            Date start = new Date(2000, 1, 2);
            Date end = new Date(2000, 1, 1);

            int days = start.DaysUntil(end);

            Assert.Equal(-1, days);
        }

        [Fact]
        public void CanCalculateDaysUntilDate_MinToMax()
        {
            Date start = Date.MinValue;
            Date end = Date.MaxValue;

            int days = start.DaysUntil(end);

            Assert.Equal(3652058, days);
        }

        [Fact]
        public void CanCalculateDaysSinceDate_Positive()
        {
            Date start = new Date(2000, 1, 1);
            Date end = new Date(2000, 1, 2);

            int days = end.DaysSince(start);

            Assert.Equal(1, days);
        }

        [Fact]
        public void CanCalculateDaysSinceDate_Zero()
        {
            Date start = new Date(2000, 1, 1);
            Date end = new Date(2000, 1, 1);

            int days = end.DaysSince(start);

            Assert.Equal(0, days);
        }

        [Fact]
        public void CanCalculateDaysSinceDate_Negative()
        {
            Date start = new Date(2000, 1, 2);
            Date end = new Date(2000, 1, 1);

            int days = end.DaysSince(start);

            Assert.Equal(-1, days);
        }

        [Fact]
        public void CanCalculateDaysSinceDate_MinToMax()
        {
            Date start = Date.MinValue;
            Date end = Date.MaxValue;

            int days = end.DaysSince(start);

            Assert.Equal(3652058, days);
        }

        [Fact]
        public void CanCalculateMonthsUntilDate_Positive1()
        {
            Date start = new Date(2000, 1, 1);
            Date end = new Date(2000, 2, 1);

            int months = start.MonthsUntil(end);

            Assert.Equal(1, months);
        }

        [Fact]
        public void CanCalculateMonthsUntilDate_Positive2()
        {
            Date start = new Date(2000, 1, 1);
            Date end = new Date(2000, 2, 2);

            int months = start.MonthsUntil(end);

            Assert.Equal(1, months);
        }

        [Fact]
        public void CanCalculateMonthsUntilDate_Zero1()
        {
            Date start = new Date(2000, 1, 1);
            Date end = new Date(2000, 1, 1);

            int months = start.MonthsUntil(end);

            Assert.Equal(0, months);
        }

        [Fact]
        public void CanCalculateMonthsUntilDate_Zero2()
        {
            Date start = new Date(2000, 1, 1);
            Date end = new Date(2000, 1, 2);

            int months = start.MonthsUntil(end);

            Assert.Equal(0, months);
        }

        [Fact]
        public void CanCalculateMonthsUntilDate_Zero3()
        {
            Date start = new Date(2000, 1, 2);
            Date end = new Date(2000, 2, 1);

            int months = start.MonthsUntil(end);

            Assert.Equal(0, months);
        }

        [Fact]
        public void CanCalculateMonthsUntilDate_Negative1()
        {
            Date start = new Date(2000, 2, 1);
            Date end = new Date(2000, 1, 1);

            int months = start.MonthsUntil(end);

            Assert.Equal(-1, months);
        }

        [Fact]
        public void CanCalculateMonthsUntilDate_Negative2()
        {
            Date start = new Date(2000, 2, 2);
            Date end = new Date(2000, 1, 1);

            int months = start.MonthsUntil(end);

            Assert.Equal(-1, months);
        }

        [Fact]
        public void CanCalculateMonthsUntilDate_MinToMax()
        {
            Date start = Date.MinValue;
            Date end = Date.MaxValue;

            int months = start.MonthsUntil(end);

            Assert.Equal(119987, months);
        }

        [Fact]
        public void CanCalculateMonthsSinceDate_Positive1()
        {
            Date start = new Date(2000, 1, 1);
            Date end = new Date(2000, 2, 1);

            int months = end.MonthsSince(start);

            Assert.Equal(1, months);
        }

        [Fact]
        public void CanCalculateMonthsSinceDate_Positive2()
        {
            Date start = new Date(2000, 1, 1);
            Date end = new Date(2000, 2, 2);

            int months = end.MonthsSince(start);

            Assert.Equal(1, months);
        }

        [Fact]
        public void CanCalculateMonthsSinceDate_Zero1()
        {
            Date start = new Date(2000, 1, 1);
            Date end = new Date(2000, 1, 1);

            int months = end.MonthsSince(start);

            Assert.Equal(0, months);
        }

        [Fact]
        public void CanCalculateMonthsSinceDate_Zero2()
        {
            Date start = new Date(2000, 1, 2);
            Date end = new Date(2000, 1, 1);

            int months = end.MonthsSince(start);

            Assert.Equal(0, months);
        }

        [Fact]
        public void CanCalculateMonthsSinceDate_Zero3()
        {
            Date start = new Date(2000, 2, 1);
            Date end = new Date(2000, 1, 2);

            int months = end.MonthsSince(start);

            Assert.Equal(0, months);
        }

        [Fact]
        public void CanCalculateMonthsSinceDate_Negative1()
        {
            Date start = new Date(2000, 2, 1);
            Date end = new Date(2000, 1, 1);

            int months = end.MonthsSince(start);

            Assert.Equal(-1, months);
        }

        [Fact]
        public void CanCalculateMonthsSinceDate_Negative2()
        {
            Date start = new Date(2000, 2, 2);
            Date end = new Date(2000, 1, 1);

            int months = end.MonthsSince(start);

            Assert.Equal(-1, months);
        }

        [Fact]
        public void CanCalculateMonthsSinceDate_MinToMax()
        {
            Date start = Date.MinValue;
            Date end = Date.MaxValue;

            int months = end.MonthsSince(start);

            Assert.Equal(119987, months);
        }

        [Fact]
        public void CanCalculateYearsUntilDate_Positive1()
        {
            Date start = new Date(2000, 1, 1);
            Date end = new Date(2001, 1, 1);

            int years = start.YearsUntil(end);

            Assert.Equal(1, years);
        }

        [Fact]
        public void CanCalculateYearsUntilDate_Positive2()
        {
            Date start = new Date(2000, 1, 1);
            Date end = new Date(2001, 1, 2);

            int years = start.YearsUntil(end);

            Assert.Equal(1, years);
        }

        [Fact]
        public void CanCalculateYearsUntilDate_Zero1()
        {
            Date start = new Date(2000, 1, 1);
            Date end = new Date(2000, 1, 1);

            int years = start.YearsUntil(end);

            Assert.Equal(0, years);
        }

        [Fact]
        public void CanCalculateYearsUntilDate_Zero2()
        {
            Date start = new Date(2000, 1, 2);
            Date end = new Date(2001, 1, 1);

            int years = start.YearsUntil(end);

            Assert.Equal(0, years);
        }

        [Fact]
        public void CanCalculateYearsUntilDate_Zero3()
        {
            Date start = new Date(2001, 1, 1);
            Date end = new Date(2000, 1, 2);

            int years = start.YearsUntil(end);

            Assert.Equal(0, years);
        }

        [Fact]
        public void CanCalculateYearsUntilDate_Negative1()
        {
            Date start = new Date(2001, 1, 1);
            Date end = new Date(2000, 1, 1);

            int years = start.YearsUntil(end);

            Assert.Equal(-1, years);
        }

        [Fact]
        public void CanCalculateYearsUntilDate_Negative2()
        {
            Date start = new Date(2001, 1, 2);
            Date end = new Date(2000, 1, 1);

            int years = start.YearsUntil(end);

            Assert.Equal(-1, years);
        }

        [Fact]
        public void CanCalculateYearsUntilDate_MinToMax()
        {
            Date start = Date.MinValue;
            Date end = Date.MaxValue;

            int years = start.YearsUntil(end);

            Assert.Equal(9998, years);
        }

        [Fact]
        public void CanCalculateYearsSinceDate_Positive1()
        {
            Date start = new Date(2000, 1, 1);
            Date end = new Date(2001, 1, 1);

            int years = end.YearsSince(start);

            Assert.Equal(1, years);
        }

        [Fact]
        public void CanCalculateYearsSinceDate_Positive2()
        {
            Date start = new Date(2000, 1, 1);
            Date end = new Date(2001, 1, 2);

            int years = end.YearsSince(start);

            Assert.Equal(1, years);
        }

        [Fact]
        public void CanCalculateYearsSinceDate_Zero1()
        {
            Date start = new Date(2000, 1, 1);
            Date end = new Date(2000, 1, 1);

            int years = end.YearsSince(start);

            Assert.Equal(0, years);
        }

        [Fact]
        public void CanCalculateYearsSinceDate_Zero2()
        {
            Date start = new Date(2000, 1, 1);
            Date end = new Date(2000, 1, 2);

            int years = end.YearsSince(start);

            Assert.Equal(0, years);
        }

        [Fact]
        public void CanCalculateYearsSinceDate_Zero3()
        {
            Date start = new Date(2000, 1, 2);
            Date end = new Date(2001, 1, 1);

            int years = end.YearsSince(start);

            Assert.Equal(0, years);
        }

        [Fact]
        public void CanCalculateYearsSinceDate_Negative1()
        {
            Date start = new Date(2001, 1, 1);
            Date end = new Date(2000, 1, 1);

            int years = end.YearsSince(start);

            Assert.Equal(-1, years);
        }

        [Fact]
        public void CanCalculateYearsSinceDate_Negative2()
        {
            Date start = new Date(2001, 1, 2);
            Date end = new Date(2000, 1, 1);

            int years = end.YearsSince(start);

            Assert.Equal(-1, years);
        }

        [Fact]
        public void CanCalculateYearsSinceDate_MinToMax()
        {
            Date start = Date.MinValue;
            Date end = Date.MaxValue;

            int years = end.YearsSince(start);

            Assert.Equal(9998, years);
        }
    }
}
