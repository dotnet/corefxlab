using System.Buffers;
using System.Globalization;
using Xunit;

namespace System.Text.Formatting.Tests
{
    public class CompositeFormattingTests
    {
        public CompositeFormattingTests()
        {
            var culture = (CultureInfo)CultureInfo.InvariantCulture.Clone();

            culture.DateTimeFormat.LongTimePattern = "hh:mm:ss tt";
            culture.DateTimeFormat.ShortTimePattern = "hh:mm tt";

            CultureInfo.CurrentCulture = culture;
            CultureInfo.CurrentUICulture = culture;
        }

        [Fact]
        public void CompositeFormattingBasics()
        {
            var time = DateTime.UtcNow;
            var pool = new ManagedBufferPool<byte>(1024);
            var formatter = new StringFormatter(pool);
            formatter.Format("{2} - Error {0}. File {1} not found.", 404, "index.html", time);

            Assert.Equal(time.ToString("G") + " - Error 404. File index.html not found.", formatter.ToString());
        }

        [Fact]
        public void CompositeFormattingFormatStrings()
        {
            var pool = new ManagedBufferPool<byte>(1024);
            var formatter = new StringFormatter(pool);
            formatter.Format("Hello{0:x}{1:X}{2:G}", 10, 255, 3);

            Assert.Equal("HelloaFF3", formatter.ToString());
        }
    }
}
