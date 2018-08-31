// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Xunit;

namespace System.Text.Formatting.Globalization.Tests
{
    public class NonInvariantCultureTests
    {
        [Fact]
        public void CustomCulture()
        {
            var sb = new StringFormatter();
            sb.SymbolTable = EncodingProvider.CreateEncoding("pl-PL");

            sb.Append(-10000, 'N');
            Assert.Equal("-10\u00A0000,00", sb.ToString()); // \u00A0 is a space group separator
        }
    }
}
