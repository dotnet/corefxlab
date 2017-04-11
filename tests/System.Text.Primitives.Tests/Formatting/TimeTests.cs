// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Globalization;
using Xunit;

namespace System.Text.Primitives.Tests
{
    public class TimeTests
    {
        const int NumberOfRandomSamples = 1000;

        static readonly TextEncoder[] Encoders = new TextEncoder[]
        {
            TextEncoder.Utf8,
            TextEncoder.Utf16,
        };

        [Theory]
        [InlineData('G')]
        [InlineData('O')]
        [InlineData('R')]
        public void SpecificDateTimeTests(char format)
        {
            foreach (var encoder in Encoders)
            {
                TestDateTimeFormat(DateTime.MinValue, format, encoder);
                TestDateTimeFormat(DateTime.MaxValue, format, encoder);
                TestDateTimeFormat(new DateTime(1, 1, 1), format, encoder);
                TestDateTimeFormat(new DateTime(9999, 12, 31), format, encoder);
                TestDateTimeFormat(new DateTime(2004, 2, 29), format, encoder);
            }
        }

        [Theory]
        [InlineData('G')]
        [InlineData('O')]
        [InlineData('R')]
        public void RandomDateTimeTests(char format)
        {
            for (var i = 0; i < NumberOfRandomSamples; i++)
            {
                var dt = CreateRandomDate();
                foreach (var encoder in Encoders)
                {
                    TestDateTimeFormat(dt, format, encoder);
                }
            }
        }

        static void TestDateTimeFormat(DateTime dt, char format, TextEncoder encoder)
        {
            var expected = dt.ToString(format.ToString(), CultureInfo.InvariantCulture);

            var span = new Span<byte>(new byte[256]);
            Assert.True(PrimitiveFormatter.TryFormat(dt, span, out int written, format, encoder));

            var actual = TestHelper.SpanToString(span.Slice(0, written), encoder);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(0)]
        [InlineData('G')]
        [InlineData('O')]
        [InlineData('R')]
        public void RandomDateTimeOffsetTests(char format)
        {
            for (var i = 0; i < NumberOfRandomSamples; i++)
            {
                var dto = CreateRandomDateOffset();
                foreach (var encoder in Encoders)
                {
                    TestDateTimeOffsetFormat(dto, format, encoder);
                }
            }
        }

        static void TestDateTimeOffsetFormat(DateTimeOffset dto, char formatChar, TextEncoder encoder)
        {
            TextFormat format = (formatChar == 0) ? default(TextFormat) : formatChar;
            string expected = (format.IsDefault)
                ? dto.ToString(CultureInfo.InvariantCulture)
                : dto.ToString(formatChar.ToString(), CultureInfo.InvariantCulture);

            var span = new Span<byte>(new byte[256]);
            Assert.True(PrimitiveFormatter.TryFormat(dto, span, out int written, format, encoder));

            var actual = TestHelper.SpanToString(span.Slice(0, written), encoder);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData('c')]
        [InlineData('t')]
        [InlineData('g')]
        [InlineData('G')]
        public void SpecificTimeSpanTests(char format)
        {
            foreach (var encoder in Encoders)
            {
                TestTimeSpanFormat(TimeSpan.MinValue, format, encoder);
                TestTimeSpanFormat(TimeSpan.MaxValue, format, encoder);
                TestTimeSpanFormat(new TimeSpan(0), format, encoder);
                TestTimeSpanFormat(new TimeSpan(-1), format, encoder);
                TestTimeSpanFormat(new TimeSpan(1), format, encoder);
                TestTimeSpanFormat(TimeSpan.FromDays(-1), format, encoder);
                TestTimeSpanFormat(TimeSpan.FromDays(1), format, encoder);
                TestTimeSpanFormat(TimeSpan.FromHours(-1), format, encoder);
                TestTimeSpanFormat(TimeSpan.FromHours(1), format, encoder);
                TestTimeSpanFormat(TimeSpan.FromMinutes(-1), format, encoder);
                TestTimeSpanFormat(TimeSpan.FromMinutes(1), format, encoder);
                TestTimeSpanFormat(TimeSpan.FromSeconds(-1), format, encoder);
                TestTimeSpanFormat(TimeSpan.FromSeconds(1), format, encoder);
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
                foreach (var encoder in Encoders)
                {
                    TestTimeSpanFormat(ts, format, encoder);
                }
            }
        }

        static void TestTimeSpanFormat(TimeSpan ts, char format, TextEncoder encoder)
        {
            var expected = ts.ToString(format.ToString(), CultureInfo.InvariantCulture);

            var span = new Span<byte>(new byte[256]);
            Assert.True(PrimitiveFormatter.TryFormat(ts, span, out int written, format, encoder));

            var actual = TestHelper.SpanToString(span.Slice(0, written), encoder);
            Assert.Equal(expected, actual);
        }

        static readonly Random Rnd = new Random();

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
