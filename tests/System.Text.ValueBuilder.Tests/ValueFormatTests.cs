// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Xunit;

namespace System.Text.ValueBuilder.Tests
{
    public class ValueFormatTests
    {
        [Theory,
            InlineData(true, "The answer is {0}"),
            InlineData(false, "{0,10} is the answer"),
            InlineData(true, "{0} is the answer")]
        public void FormatSingleBool(bool value, string format)
        {
            Variant variant = new Variant(value);
            ValueStringBuilder vsb = new ValueStringBuilder();
            vsb.Append(format, variant.ToSpan());
            Assert.Equal(string.Format(format, value), vsb.ToString());
            vsb.Dispose();
        }

        [Theory,
            InlineData(42, "6 x 7", "The answer is {0}, the question is {1}")]
        public void FormatIntString(int value1, string value2, string format)
        {
            ValueStringBuilder vsb = new ValueStringBuilder();
            vsb.Append(format, Variant.Create(value1, value2).ToSpan());
            Assert.Equal(string.Format(format, value1, value2), vsb.ToString());
            vsb.Dispose();
        }
    }
}
