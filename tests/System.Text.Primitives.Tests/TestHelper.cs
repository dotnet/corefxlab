// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Text;
using System.Text.Utf8;
using Xunit;

namespace System.Text.Primitives.Tests
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

        public static string SpanToString(Span<byte> span, TextEncoder encoder = null)
        {
            // Assume no encoder means the buffer is UTF-8
            encoder = (encoder == null) ? TextEncoder.Utf8 : encoder;
            Assert.True(encoder.TryDecode(span, out string text, out int consumed));
            return text;
        }
    }
}
