// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

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
