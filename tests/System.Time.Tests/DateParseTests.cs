using System.Collections.Generic;
using System.Globalization;
using Xunit;

namespace System._Time.Tests
{
    public class DateParseTests
    {
        public DateParseTests()
        {
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
            CultureInfo.CurrentUICulture = CultureInfo.InvariantCulture;
        }

        [Fact]
        public static void CannotParseInvalidArguments()
        {
            Assert.Throws<ArgumentNullException>(() => Date.Parse(null));

            Assert.Throws<FormatException>(() => Date.Parse(""));
        }

        public static IEnumerable<object[]> Parse_CurrentYearWithoutTime_TestData()
        {
            int utcNowYear = DateTime.UtcNow.Year;

            yield return new object[] {"December 31", utcNowYear, 12, 31};
            yield return new object[] {"December 30", utcNowYear, 12, 30};
            yield return new object[] {"December 29", utcNowYear, 12, 29};
        }

        [Theory]
        [InlineData("01/01/0001", 1, 1, 1)] // Invariant short
        [InlineData("12/31/9999", 9999, 12, 31)]
        [InlineData("12/31/2000", 2000, 12, 31)]
        [InlineData("    12/31/2000", 2000, 12, 31)]
        [InlineData("12/31/2000    ", 2000, 12, 31)]
        [InlineData("12 / 31 / 2000", 2000, 12, 31)]
        [InlineData("12-31-2000", 2000, 12, 31)]
        [InlineData("02/29/2000", 2000, 02, 29)]
        [InlineData("02/29/2004", 2004, 02, 29)]
        [InlineData("02/29/2400", 2400, 02, 29)]

        [InlineData("Sunday, 31 December 2000", 2000, 12, 31)] // Invariant long
        [InlineData("Monday, 31 December 2001", 2001, 12, 31)]
        [InlineData("Sunday 31 December 2000", 2000, 12, 31)]
        [InlineData("Sun, 31 Dec 2000", 2000, 12, 31)]
        [InlineData("Sunday, December 31 2000", 2000, 12, 31)]
        [InlineData("Sunday, December 2000 31", 2000, 12, 31)]
        [InlineData("31 December 2000", 2000, 12, 31)]
        [InlineData("December 31 2000", 2000, 12, 31)]
        [InlineData("December 2000 31", 2000, 12, 31)]
        [InlineData("31 2000 December ", 2000, 12, 31)]
        [InlineData("2000 31 December", 2000, 12, 31)]
        [InlineData("2000 December 31", 2000, 12, 31)]
        [InlineData("December 2000", 2000, 12, 1)]
        [InlineData("2000 December", 2000, 12, 1)]

        [InlineData("31 December", 1931, 12, 1)]
        [InlineData("30 December", 1930, 12, 1)]
        [InlineData("29 December", 2029, 12, 1)]
        [MemberData(nameof(Parse_CurrentYearWithoutTime_TestData))]

        [InlineData("0001-01-01", 1, 1, 1)] // Iso Short
        [InlineData("9999-12-31", 9999, 12, 31)]
        [InlineData("2000-12-31", 2000, 12, 31)]
        [InlineData("    2000-12-31", 2000, 12, 31)]
        [InlineData("2000-12-31    ", 2000, 12, 31)]
        [InlineData("2000 - 12 - 31", 2000, 12, 31)]
        [InlineData("2000/12/31", 2000, 12, 31)]
        [InlineData("2000-02-28", 2000, 2, 28)]
        [InlineData("2004-02-28", 2004, 2, 28)]
        [InlineData("2400-02-28", 2400, 2, 28)]
        public void CanParseDate(string input, int expectedYear, int expectedMonth, int expectedDay)
        {
            var date = Date.Parse(input);

            Assert.Equal(expectedYear, date.Year);
            Assert.Equal(expectedMonth, date.Month);
            Assert.Equal(expectedDay, date.Day);
        }

        [Theory]
        [InlineData("12/31/2000 10:49:12")] // Invariant short - Has time information
        [InlineData("10:49:12 12/31/2000")]
        [InlineData("12/31/2000 22:49:12")]

        [InlineData("12/31/2000 13:00:00Z")] // Invariant short - Time zone - Has time information
        [InlineData("12/31/2000 13:00:00+1100")]
        [InlineData("12/31/2000 01:00:00+1100")]
        [InlineData("12/31/2000 13:00:00-1100")]
        [InlineData("12/31/2000 01:00:00-1100")]

        [InlineData("Sunday, 31 December 2000 10:49:12")] // Invariant long - Has time information
        [InlineData("10:49:12 Sunday, 31 December 2000")]
        [InlineData("Sunday, 31 December 2000 22:49:12")]

        [InlineData("Sunday, 31 December 2000 13:00:00Z")] //  Invariant long - Time zone - Has time information
        [InlineData("Sunday, 31 December 2000 13:00:00+1100")]
        [InlineData("Sunday, 31 December 2000 01:00:00+1100")]
        [InlineData("Sunday, 31 December 2000 13:00:00-1100")]
        [InlineData("Sunday, 31 December 2000 01:00:00-1100")]

        [InlineData("10:49:12 31 December")] // Has time information
        [InlineData("10:49:12 30 December")]
        [InlineData("10:49:12 29 December")]
        [InlineData("31 December 10:49:12")] // Cannot determine year - Has time information
        [InlineData("30 December 10:49:12")]
        [InlineData("29 December 10:49:12")]

        [InlineData("2000-12-31 10:49:12")] // Iso short - Has time information
        [InlineData("10:49:12 2000-12-31")]
        [InlineData("2000-12-31 22:49:12")]

        [InlineData("01-01-0000")] // Smaller than Date.MinValue
        [InlineData("31-12-10000")] // Larger than Date.MaxValue

        [InlineData("31/12/2000")] // Month and day switched
        [InlineData("2000-31-12")]

        [InlineData("12\\31\\2000")] // Incorrect separators
        [InlineData("12:31:2000")]

        [InlineData("02/29/2001")] // Not a leap year
        [InlineData("02/29/2100")]

        [InlineData("December")] // Cannot determine year and day
        [InlineData("December 10:49:12")] // Has time information
        [InlineData("10:49:12 December")]

        [InlineData("Monday, 31 December 2000")] // Incorrect day name
        [InlineData("Sunday, 31 December 2001")]

        [InlineData("10:49:12")] // No date specified
        public void CannotParseDate(string input)
        {
            Assert.Throws<FormatException>(() => Date.Parse(input));
        }
    }
}
