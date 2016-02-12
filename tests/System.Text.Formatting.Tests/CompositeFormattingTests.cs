using System.Buffers;
using System.Globalization;
using Xunit;

namespace System.Text.Formatting.Tests
{
    public class CompositeFormattingTests
    {
        static ArrayPool<byte> pool = ArrayPool<byte>.Shared;

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

        [Fact]
        public void CompositeFormattingBasics()
        {
            var time = new DateTime(2016, 2, 9, 4, 1, 59, DateTimeKind.Utc);
            using (var formatter = new StringFormatter(pool))
            {
                formatter.Format("{2:G} - Error {0}. File {1} not found.", 404, "index.html", time);
                Assert.Equal("2/9/2016 4:01:59 AM - Error 404. File index.html not found.", formatter.ToString());
            }
        }

        [Fact]
        public void CompositeFormattingFormatStrings()
        {
            var formatter = new StringFormatter(pool);
            formatter.Format("Hello{0:x}{1:X}{2:G}", 10, 255, 3);

            Assert.Equal("HelloaFF3", formatter.ToString());
        }

        [Fact]
        public void CompositeFormattingEscaping()
        {
            var format = "}}a {0} b {0} c {{{0}}} d {{e}} {{";
            var formatter = new StringFormatter(pool);
            formatter.Format(format, 1);

            Assert.Equal(string.Format(CultureInfo.InvariantCulture, format, 1), formatter.ToString());
        }

        [Fact]
        public void CompositeFormattingEscapingMissingEndBracket()
        {
            var formatter = new StringFormatter(pool);
            Assert.Throws<Exception>(() => formatter.Format("{{0}", 1));
        }

        [Fact]
        public void CompositeFormattingEscapingMissingStartBracket()
        {
            var formatter = new StringFormatter(pool);
            Assert.Throws<Exception>(() => formatter.Format("{0}}", 1));
        }
    }
}
