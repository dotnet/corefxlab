// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

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
            var sb = new StringFormatter(pool);

            sb.Append(time, 'O');
            Assert.Equal(time.ToString("O", CultureInfo.InvariantCulture), sb.ToString());
            sb.Clear();
        }

        [Fact]
        public void FormatDateTimeOffsetR()
        {
            var time = DateTimeOffset.UtcNow;
            var sb = new StringFormatter(pool);

            sb.Append(time, 'R');
            Assert.Equal(time.ToString("R"), sb.ToString());
            sb.Clear();
        }

        [Fact]
        public void FormatDateTimeOffsetG()
        {
            var time = DateTimeOffset.UtcNow;
            var sb = new StringFormatter(pool);

            sb.Append(time, 'G');
            Assert.Equal(time.ToString("G"), sb.ToString());
            sb.Clear();
        }

        [Fact]
        public void FormatDateTimeO()
        {
            var time = DateTime.UtcNow;
            var sb = new StringFormatter(pool);

            sb.Append(time, 'O');
            Assert.Equal(time.ToString("O", CultureInfo.InvariantCulture), sb.ToString());
            sb.Clear();
        }

        [Fact]
        public void FormatDateTimeR()
        {
            var time = DateTime.UtcNow;
            var sb = new StringFormatter(pool);

            sb.Append(time, 'R');
            Assert.Equal(time.ToString("R"), sb.ToString());
            sb.Clear();
        }

        [Fact]
        public void FormatDateTimeG()
        {
            var time = DateTime.UtcNow;
            var sb = new StringFormatter(pool);

            sb.Append(time, 'G');
            Assert.Equal(time.ToString("G"), sb.ToString());
            sb.Clear();
        }

        [Fact]
        public void FormatTimeSpan()
        {
            var time = new TimeSpan(1000, 23, 40, 30, 12345);
            var sb = new StringFormatter(pool);

            sb.Append(time);
            Assert.Equal(time.ToString("", CultureInfo.InvariantCulture), sb.ToString());
            sb.Clear();
        }
    }
}
