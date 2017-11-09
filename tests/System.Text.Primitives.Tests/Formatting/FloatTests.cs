// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Buffers.Text;
using System.Collections.Generic;
using System.Globalization;
using Xunit;

namespace System.Text.Primitives.Tests
{
    public partial class FormattingTests
    {
        public static IEnumerable<object[]> GetDecimals()
        {
            yield return new object[] { 0M };
            yield return new object[] { Decimal.MaxValue };
            yield return new object[] { Decimal.MinValue };
            yield return new object[] { 0.1M };
            yield return new object[] { 1.1M };
            yield return new object[] { 1.0M };
        }

        [Theory]
        [MemberData(nameof(GetDecimals))]
        public void DecimalFormattingUtf8(decimal number)
        {
            var expected = number.ToString("G", CultureInfo.InvariantCulture);
            var buffer = new byte[expected.Length * 2];
            Assert.True(Utf8Formatter.TryFormat(number, buffer, out int bytesWritten, 'G'));
            var actual = Text.Encoding.UTF8.GetString(buffer, 0, bytesWritten);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [MemberData(nameof(GetDecimals))]
        public void DecimalFormattingUtf16(decimal number)
        {
            var expected = number.ToString("G", CultureInfo.InvariantCulture);
            var buffer = new byte[expected.Length * 2];
            Assert.True(Utf16Formatter.TryFormat(number, buffer, out int bytesWritten, 'G'));
            var actual = Text.Encoding.Unicode.GetString(buffer, 0, bytesWritten);
            Assert.Equal(expected, actual);
        }
    }
}
