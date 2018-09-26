// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Xunit;

namespace System.Text.Formatting.Tests
{
    public partial class SystemTextFormattingTests
    {
        [Fact]
        public void FormatGuid()
        {
            var guid = Guid.NewGuid();
            var sb = new StringFormatter();

            sb.Append(guid);
            Assert.Equal(guid.ToString(), sb.ToString());
            sb.Clear();

            sb.Append(guid, 'D');
            Assert.Equal(guid.ToString("D"), sb.ToString());
            sb.Clear();

            sb.Append(guid, 'N');
            Assert.Equal(guid.ToString("N"), sb.ToString());
            sb.Clear();

            sb.Append(guid, 'B');
            Assert.Equal(guid.ToString("B"), sb.ToString());
            sb.Clear();

            sb.Append(guid, 'P');
            Assert.Equal(guid.ToString("P"), sb.ToString());
            sb.Clear();
        }
    }
}
