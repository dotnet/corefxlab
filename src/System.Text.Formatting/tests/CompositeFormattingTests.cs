using System;
using System.Text.Formatting;
using System.IO;
using Xunit;

namespace System.Text.Formatting.Tests
{
    public class CompositeFormattingTests
    {
        [Fact]
        public void CompositeFormattingBasics()
        {
            var time = DateTime.UtcNow;

            var formatter = new StringFormatter();
            formatter.Format("{2} - Error {0}. File {1} not found.", 404, "index.html", time);

            Assert.Equal(time.ToString("G") + " - Error 404. File index.html not found.", formatter.ToString());
        }
    }
}
