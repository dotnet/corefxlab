// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.IO;
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
    }
}
