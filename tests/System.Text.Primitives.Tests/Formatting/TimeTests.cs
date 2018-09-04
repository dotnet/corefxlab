// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers;
using System.Buffers.Text;
using System.Globalization;
using Xunit;

namespace System.Text.Primitives.Tests
{
    public class TimeTests
    {
        const int NumberOfRandomSamples = 1000;

        static readonly SymbolTable[] SymbolTables = new SymbolTable[]
        {
            SymbolTable.InvariantUtf8,
            SymbolTable.InvariantUtf16,
        };

        [Theory]
        [InlineData('G')]
        [InlineData('O')]
        [InlineData('R')]
        [InlineData('l')]
        public void SpecificDateTimeTests(char format)
        {
            foreach (var symbolTable in SymbolTables)
            {
                TestDateTimeFormat(DateTime.MinValue, format, symbolTable);
                TestDateTimeFormat(DateTime.MaxValue, format, symbolTable);
                TestDateTimeFormat(new DateTime(1, 1, 1), format, symbolTable);
                TestDateTimeFormat(new DateTime(9999, 12, 31), format, symbolTable);
                TestDateTimeFormat(new DateTime(2004, 2, 29), format, symbolTable);
            }
        }

        [Theory]
        [InlineData('G')]
        [InlineData('O')]
        [InlineData('R')]
        [InlineData('l')]
        public void RandomDateTimeTests(char format)
        {
            for (var i = 0; i < NumberOfRandomSamples; i++)
            {
                var dt = CreateRandomDate();
                foreach (var symbolTable in SymbolTables)
                {
                    TestDateTimeFormat(dt, format, symbolTable);
                }
            }
        }

        static void TestDateTimeFormat(DateTime dt, char format, SymbolTable symbolTable)
        {
            string expected;
            if (format == 'l')
                expected = dt.ToString("R", CultureInfo.InvariantCulture).ToLowerInvariant();
            else
                expected = dt.ToString(format.ToString(), CultureInfo.InvariantCulture);           

            var span = new Span<byte>(new byte[256]);
            Assert.True(CustomFormatter.TryFormat(dt, span, out int written, format, symbolTable));

            var actual = TestHelper.SpanToString(span.Slice(0, written), symbolTable);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(0)]
        [InlineData('G')]
        [InlineData('O')]
        [InlineData('R')]
        [InlineData('l')]
        public void RandomDateTimeOffsetTests(char format)
        {
            for (var i = 0; i < NumberOfRandomSamples; i++)
            {
                var dto = CreateRandomDateOffset();
                foreach (var symbolTable in SymbolTables)
                {
                    TestDateTimeOffsetFormat(dto, format, symbolTable);
                }
            }
        }

        static void TestDateTimeOffsetFormat(DateTimeOffset dto, char formatChar, SymbolTable symbolTable)
        {
            StandardFormat format = (formatChar == 0) ? default(StandardFormat) : formatChar;

            string expected;
            if (formatChar == 'l') {
                expected = dto.ToString("R", CultureInfo.InvariantCulture).ToLowerInvariant();
            }
            else {
                expected = (format.IsDefault) ? dto.ToString(CultureInfo.InvariantCulture) : dto.ToString(formatChar.ToString(), CultureInfo.InvariantCulture);
            }

            var span = new Span<byte>(new byte[256]);
            Assert.True(CustomFormatter.TryFormat(dto, span, out int written, format, symbolTable));

            var actual = TestHelper.SpanToString(span.Slice(0, written), symbolTable);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData('c')]
        [InlineData('t')]
        [InlineData('g')]
        [InlineData('G')]
        public void SpecificTimeSpanTests(char format)
        {
            foreach (var symbolTable in SymbolTables)
            {
                TestTimeSpanFormat(TimeSpan.MinValue, format, symbolTable);
                TestTimeSpanFormat(TimeSpan.MaxValue, format, symbolTable);
                TestTimeSpanFormat(new TimeSpan(0), format, symbolTable);
                TestTimeSpanFormat(new TimeSpan(-1), format, symbolTable);
                TestTimeSpanFormat(new TimeSpan(1), format, symbolTable);
                TestTimeSpanFormat(TimeSpan.FromDays(-1), format, symbolTable);
                TestTimeSpanFormat(TimeSpan.FromDays(1), format, symbolTable);
                TestTimeSpanFormat(TimeSpan.FromHours(-1), format, symbolTable);
                TestTimeSpanFormat(TimeSpan.FromHours(1), format, symbolTable);
                TestTimeSpanFormat(TimeSpan.FromMinutes(-1), format, symbolTable);
                TestTimeSpanFormat(TimeSpan.FromMinutes(1), format, symbolTable);
                TestTimeSpanFormat(TimeSpan.FromSeconds(-1), format, symbolTable);
                TestTimeSpanFormat(TimeSpan.FromSeconds(1), format, symbolTable);
            }
        }

        [Theory]
        [InlineData('c')]
        [InlineData('t')]
        [InlineData('g')]
        [InlineData('G')]
        public void RandomTimeSpanTests(char format)
        {
            for (var i = 0; i < NumberOfRandomSamples; i++)
            {
                var ts = CreateRandomTimeSpan();
                foreach (var symbolTable in SymbolTables)
                {
                    TestTimeSpanFormat(ts, format, symbolTable);
                }
            }
        }

        static void TestTimeSpanFormat(TimeSpan ts, char format, SymbolTable symbolTable)
        {
            var expected = ts.ToString(format.ToString(), CultureInfo.InvariantCulture);

            var span = new Span<byte>(new byte[256]);
            Assert.True(CustomFormatter.TryFormat(ts, span, out int written, format, symbolTable));

            var actual = TestHelper.SpanToString(span.Slice(0, written), symbolTable);
            Assert.Equal(expected, actual);
        }

        static readonly Random Rnd = new Random(32098);

        static DateTime CreateRandomDate()
        {
            return new DateTime(
                Rnd.Next(1, 10000),
                Rnd.Next(1, 13),
                Rnd.Next(1, 29),
                Rnd.Next(0, 24),
                Rnd.Next(0, 60),
                Rnd.Next(0, 60),
                Rnd.Next(0, 1000),
                (DateTimeKind)Rnd.Next(0, 3));
        }

        static DateTimeOffset CreateRandomDateOffset()
        {
            return new DateTimeOffset(
                Rnd.Next(2, 9999),
                Rnd.Next(1, 13),
                Rnd.Next(1, 29),
                Rnd.Next(0, 24),
                Rnd.Next(0, 60),
                Rnd.Next(0, 60),
                Rnd.Next(0, 1000),
                TimeSpan.FromHours(Rnd.Next(1, 5)));
        }

        static TimeSpan CreateRandomTimeSpan()
        {
            return new TimeSpan(
                Rnd.Next(-1000, 1001),
                Rnd.Next(0, 24),
                Rnd.Next(0, 60),
                Rnd.Next(0, 60),
                Rnd.Next(0, 1000));
        }
    }
}
