// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers;
using System.Globalization;
using System.Text.Utf8;
using Xunit;

namespace System.Text.Formatting.Tests
{
    public class CompositeFormattingTests
    {
        public CompositeFormattingTests()
        {
            var culture = (CultureInfo)CultureInfo.InvariantCulture.Clone();

            culture.DateTimeFormat.LongTimePattern = "h:mm:ss tt";
            culture.DateTimeFormat.ShortTimePattern = "h:mm tt";
            culture.DateTimeFormat.LongDatePattern = "dddd, d MMMM yyyy";
            culture.DateTimeFormat.ShortDatePattern = "M/d/yyyy";

            CultureInfo.CurrentCulture = culture;
            CultureInfo.CurrentUICulture = culture;
        }

        [Fact(Skip = "System.TypeLoadException : The generic type 'System.Nullable`1' was used with an invalid instantiation in assembly 'System.Private.CoreLib")]
        public void CompositeFormattingBasics()
        {
            var time = new DateTime(2016, 2, 9, 4, 1, 59, DateTimeKind.Utc);
            using (var formatter = new StringFormatter()) {
                formatter.Format("{2:G} - Error {0}. File {1} not found.", 404, "index.html", time);
                Assert.Equal("2/9/2016 4:01:59 AM - Error 404. File index.html not found.", formatter.ToString());
            }
        }

        [Fact(Skip = "System.BadImageFormatException : An attempt was made to load a program with an incorrect format.")]
        public void CompositeFormattingUtf8String()
        {
            var value = new Utf8String("hello world!");
            using (var formatter = new StringFormatter()) {
                formatter.Format("{0}#{1}", value, value);
                Assert.Equal("hello world!#hello world!", formatter.ToString());
            }
        }

        [Fact(Skip = "System.TypeLoadException : The generic type 'System.Nullable`1' was used with an invalid instantiation in assembly 'System.Private.CoreLib")]
        public void CompositeFormattingDateTimeOffset()
        {
            var value = new DateTimeOffset(2016, 9, 23, 10, 53, 1, 1, TimeSpan.FromHours(1));
            using (var formatter = new StringFormatter()) {
                formatter.Format("{0}#{1}", value, value);
                Assert.Equal("9/23/2016 10:53:01 AM#9/23/2016 10:53:01 AM", formatter.ToString());
            }
        }

        [Fact(Skip = "System.TypeLoadException : The generic type 'System.Nullable`1' was used with an invalid instantiation in assembly 'System.Private.CoreLib")]
        public void CompositeFormattingGuid()
        {
            var value = Guid.NewGuid();
            using (var formatter = new StringFormatter()) {
                formatter.Format("{0}#{1}", value, value);
                Assert.Equal(value.ToString() +"#"+value.ToString(), formatter.ToString());
            }
        }

        [Fact(Skip = "System.TypeLoadException : The generic type 'System.Nullable`1' was used with an invalid instantiation in assembly 'System.Private.CoreLib")]
        public void CompositeFormattingFormatStrings()
        {
            var formatter = new StringFormatter();
            formatter.Format("Hello{0:x}{1:X}{2:G}", 10, 255, 3);

            Assert.Equal("HelloaFF3", formatter.ToString());
        }

        [Fact(Skip = "System.TypeLoadException : The generic type 'System.Nullable`1' was used with an invalid instantiation in assembly 'System.Private.CoreLib")]
        public void CompositeFormattingEscaping()
        {
            var format = "}}a {0} b {0} c {{{0}}} d {{e}} {{";
            var formatter = new StringFormatter();
            formatter.Format(format, 1);

            Assert.Equal(string.Format(CultureInfo.InvariantCulture, format, 1), formatter.ToString());
        }

        [Fact]
        public void CompositeFormattingEscapingMissingEndBracket()
        {
            var formatter = new StringFormatter();
            Assert.Throws<Exception>(() => formatter.Format("{{0}", 1));
        }

        [Fact(Skip = "System.TypeLoadException : The generic type 'System.Nullable`1' was used with an invalid instantiation in assembly 'System.Private.CoreLib")]
        public void CompositeFormattingEscapingMissingStartBracket()
        {
            var formatter = new StringFormatter();
            Assert.Throws<Exception>(() => formatter.Format("{0}}", 1));
        }
    }
}