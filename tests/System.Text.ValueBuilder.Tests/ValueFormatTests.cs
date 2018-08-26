// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Xunit;

namespace System.Text.ValueBuilder.Tests
{
    public class ValueFormatTests
    {
        [Theory,
            InlineData(true, "The answer is {0}", "The answer is True"),
            InlineData(true, "{0} is the answer", "False is the answer")]
        public void FormatSingleBool(bool value, string format, string expected)
        {
            Variant variant = new Variant(value);
            ValueStringBuilder vsb = new ValueStringBuilder(expected.Length);
            vsb.Append(format, variant.ToSpan());
            string result = vsb.ToString();
        }
    }
}
