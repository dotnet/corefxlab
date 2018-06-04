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
            char[] buffer = int.MaxValue.ToString().ToCharArray();
            ReadOnlySpan<char> span = buffer.AsSpan();
            Assert.True(Int32Polyfill.TryParse(span, out int value));
            Assert.Equal(int.MaxValue, value);
        }
    }
}
