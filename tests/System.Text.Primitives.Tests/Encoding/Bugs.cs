// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Xunit;

namespace System.Text.Utf8.Tests
{
    public class BugTests
    {
        [Fact]
        public void Bug869DoesNotRepro()
        {
            var bytes = new byte[] { 0xF0, 0xA4, 0xAD, 0xA2 };
            var utf8String = new Utf8String(bytes);
            var str = "𤭢";
            var strFromUtf8 = utf8String.ToString();

            Assert.Equal(str, strFromUtf8);
        }
    }
}
