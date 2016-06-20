// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Buffers;
using Xunit;
using System.Text;

namespace System.Text.Formatting.Globalization.Tests
{
    public class NonInvariantCultureTests
    {
        [Fact]
        public void CustomCulture()
        {
            var sb = new StringFormatter(ArrayPool<byte>.Shared);
            sb.FormattingData = FormattingDataProvider.CreateFormattingData("pl-PL");

            sb.Append(-10000, Format.Parse('N'));
            Assert.Equal("-10\u00A0000,00", sb.ToString()); // \u00A0 is a space group separator
        }
    }
}
