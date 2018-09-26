// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers.Text;
using System.Globalization;
using Xunit;

namespace System.Text.Formatting.Tests
{
    public partial class SystemTextFormattingTests
    {
        [Fact]
        public void FormatDateTimeOffsetO()
        {
            var time = DateTimeOffset.UtcNow;
            var sb = new StringFormatter();

            sb.Append(time, 'O');
            Assert.Equal(time.ToString("O", CultureInfo.InvariantCulture), sb.ToString());
            sb.Clear();
        }

        [Fact]
        public void FormatDateTimeOffsetR()
        {
            var time = DateTimeOffset.UtcNow;
            var sb = new StringFormatter();

            sb.Append(time, 'R');
            Assert.Equal(time.ToString("R"), sb.ToString());
            sb.Clear();
        }

        [Fact]
        public void FormatDateTimeOffsetG()
        {
            var time = DateTimeOffset.UtcNow;
            var sb = new StringFormatter();

            sb.Append(time, 'G');
            Assert.Equal(time.ToString("G", CultureInfo.InvariantCulture), sb.ToString());
            sb.Clear();
        }

        [Fact]
        public void FormatDateTimeO()
        {
            var time = DateTime.UtcNow;
            var sb = new StringFormatter();

            sb.Append(time, 'O');
            Assert.Equal(time.ToString("O", CultureInfo.InvariantCulture), sb.ToString());
            sb.Clear();
        }

        [Fact]
        public void FormatDateTimeR()
        {
            var time = DateTime.UtcNow;
            var sb = new StringFormatter();

            sb.Append(time, 'R');
            Assert.Equal(time.ToString("R"), sb.ToString());
            sb.Clear();
        }

        [Fact]
        public void FormatDateTimeRUtf8()
        {
            var time = DateTime.UtcNow;

            var expected = time.ToString("R");

            var sb = new ArrayFormatter(100, SymbolTable.InvariantUtf8);
            sb.Append(time, 'R');
            var result = sb.Formatted.AsSpan().ToArray();
            var resultString = Encoding.UTF8.GetString(result);

            Assert.Equal(expected, resultString);

            sb.Clear();
        }

        [Fact]
        public void FormatDateTimeG()
        {
            var time = DateTime.UtcNow;
            var sb = new StringFormatter();

            sb.Append(time, 'G');
            Assert.Equal(time.ToString("G", CultureInfo.InvariantCulture), sb.ToString());
            sb.Clear();
        }

        [Fact]
        public void FormatTimeSpan()
        {
            var time = new TimeSpan(1000, 23, 40, 30, 12345);
            var sb = new StringFormatter();

            sb.Append(time);
            Assert.Equal(time.ToString("", CultureInfo.InvariantCulture), sb.ToString());
            sb.Clear();
        }
    }
}
