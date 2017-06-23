// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Xunit;

namespace System.Text.Utf8.Tests
{
    public static class TestHelper
    {
        public static void Validate(Utf8String str1, Utf8String str2)
        {
            Assert.Equal(str1.Length, str2.Length);
            for (int i = 0; i < str1.Length; i++)
            {
                Assert.Equal(str1[i], str2[i]);
            }
        }
    }
}
