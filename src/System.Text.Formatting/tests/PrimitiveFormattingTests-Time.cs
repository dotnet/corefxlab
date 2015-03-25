// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Globalization;
using System.IO;
using System.Text.Formatting;
using Xunit;

namespace System.Text.Formatting.Tests
{
    public partial class SystemTextFormattingTests
    {
        [Fact]
        public void FormatDateTimeO()
        {
            var time = DateTime.UtcNow;
            var sb = new StringFormatter();

            sb.Append(time, Format.Symbol.O);
            Assert.Equal(time.ToString("O", CultureInfo.InvariantCulture), sb.ToString());
            sb.Clear();
        }

        [Fact]
        public void FormatDateTimeR()
        {
            var time = DateTime.UtcNow;
            var sb = new StringFormatter();

            sb.Append(time, Format.Symbol.R);
            Assert.Equal(time.ToString("R"), sb.ToString());
            sb.Clear();
        }

        [Fact]
        public void FormatDateTimeG()
        {
            var time = DateTime.UtcNow;
            var sb = new StringFormatter();

            sb.Append(time, Format.Symbol.G);
            Assert.Equal(time.ToString("G"), sb.ToString());
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
