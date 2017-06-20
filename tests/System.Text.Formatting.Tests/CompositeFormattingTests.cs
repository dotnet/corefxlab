// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Globalization;
using Xunit;

namespace System.Text.Formatting.Tests
{
    public class CompositeFormattingTests
    {
        [Fact]
        public void CompositeFormattingBasics()
        {
            var format = "{2:G} - Error {0}. File {1} not found.";
            var time = new DateTime(2016, 2, 9, 4, 1, 59, DateTimeKind.Utc);
            var code = 404;
            var fileName = "index.html";

            using (var formatter = new StringFormatter())
            {
                var expected = string.Format(CultureInfo.InvariantCulture, format, code, fileName, time);

                formatter.Format(format, code, fileName, time);
                Assert.Equal(expected, formatter.ToString());
            }
        }

        [Fact]
        public void CompositeFormattingDateTimeOffset()
        {
            var format = "{0}#{1}";
            var value = new DateTimeOffset(2016, 9, 23, 10, 53, 1, 1, TimeSpan.FromHours(1));

            using (var formatter = new StringFormatter())
            {
                var expected = string.Format(CultureInfo.InvariantCulture, format, value, value);

                formatter.Format(format, value, value);
                Assert.Equal(expected, formatter.ToString());
            }
        }

        [Fact]
        public void CompositeFormattingGuid()
        {
            var format = "{0}#{1}";
            var value = Guid.NewGuid();

            using (var formatter = new StringFormatter())
            {
                var expected = string.Format(CultureInfo.InvariantCulture, format, value, value);

                formatter.Format(format, value, value);
                Assert.Equal(expected, formatter.ToString());
            }
        }

        [Fact]
        public void CompositeFormattingFormatStrings()
        {
            var format = "Hello{0:x}{1:X}{2:G}";

            using (var formatter = new StringFormatter())
            {
                var expected = string.Format(CultureInfo.InvariantCulture, format, 10, 255, 3);

                formatter.Format(format, 10, 255, 3);
                Assert.Equal(expected, formatter.ToString());
            }
        }

        [Fact]
        public void CompositeFormattingEscaping()
        {
            var format = "}}a {0} b {0} c {{{0}}} d {{e}} {{";

            using (var formatter = new StringFormatter())
            {
                var expected = string.Format(CultureInfo.InvariantCulture, format, 1);

                formatter.Format(format, 1);
                Assert.Equal(expected, formatter.ToString());
            }
        }

        [Fact]
        public void CompositeFormattingEscapingMissingEndBracket()
        {
            using (var formatter = new StringFormatter())
            {
                Assert.Throws<Exception>(() => formatter.Format("{{0}", 1));
            }
        }

        [Fact]
        public void CompositeFormattingEscapingMissingStartBracket()
        {
            using (var formatter = new StringFormatter())
            {
                Assert.Throws<Exception>(() => formatter.Format("{0}}", 1));
            }
        }
    }
}
