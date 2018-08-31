// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Globalization;
using Xunit;

namespace System.Polyfill.Tests
{
    public class Int32Tests
    {
        [Fact]
        public void Int32TryParse()
        {
            ReadOnlySpan<char> span = int.MaxValue.ToString().AsSpan();
            Assert.True(Int32Polyfill.TryParse(span, out int value));
            Assert.Equal(int.MaxValue, value);
        }

        [Fact]
        public void Int32TryParseCulture()
        {
            var culture = CultureInfo.CreateSpecificCulture("pl-PL");
            var formatted = (" " + int.MaxValue.ToString("N", culture));
            Assert.True(Int32Polyfill.TryParse(formatted.AsSpan(), NumberStyles.AllowLeadingWhite | NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands, culture, out int value));
            Assert.Equal(int.MaxValue, value);
        }

        [Fact]
        public void Int32ParseCulture()
        {
            var culture = CultureInfo.CreateSpecificCulture("pl-PL");
            var formatted = (" " + int.MaxValue.ToString("N", culture));
            var value = Int32Polyfill.Parse(formatted.AsSpan(), NumberStyles.AllowLeadingWhite | NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands, culture);
            Assert.Equal(int.MaxValue, value);
        }

        [Fact]
        public void Int32TryFormat()
        {
            var culture = CultureInfo.CreateSpecificCulture("pl-PL");
            var buffer = new char[100];
            Assert.True(int.MinValue.TryFormat(buffer.AsSpan(), out int written, "N".AsSpan(), culture));
            var result = buffer.AsSpan(0, written).ToString();
            Assert.Equal(int.MinValue.ToString("N", culture), result);
        }
    }
}
