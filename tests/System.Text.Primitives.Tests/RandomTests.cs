// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace System.Text.Primitives.Tests
{
    public class Utf8StringTests
    {
        public static IEnumerable<object[]> TryComputeEncodedBytesShouldMatchEncoding_Strings()
        {
            string[] data =
            {
                "",
                "abc",
                "def",
                "\uABCD",
                "\uABC0bc",
                "a\uABC1c",
                "ab\uABC2",
                "\uABC0\uABC1\uABC2",
                Text.Encoding.UTF8.GetString(new byte[] { 0xF0, 0x9F, 0x92, 0xA9})
            };
            return data.Select(s => new object[] { s });
        }
        [Theory]
        [MemberData("TryComputeEncodedBytesShouldMatchEncoding_Strings")]
        public void TryComputeEncodedBytesShouldMatchEncoding_Utf8(string value)
            => TryTryComputeEncodedBytesShouldMatchEncoding(value, TextEncoder.Utf8, Text.Encoding.UTF8);

        [Theory]
        [MemberData("TryComputeEncodedBytesShouldMatchEncoding_Strings")]
        public void TryComputeEncodedBytesShouldMatchEncoding_Utf16(string value)
            => TryTryComputeEncodedBytesShouldMatchEncoding(value, TextEncoder.Utf16, Text.Encoding.Unicode);

        static unsafe void TryTryComputeEncodedBytesShouldMatchEncoding(string value, TextEncoder encoder, Text.Encoding encoding)
        {
            int expectedBytes = encoding.GetByteCount(value);

            // test via string input
            int actual;
            Assert.True(encoder.TryComputeEncodedBytes(value, out actual));
            Assert.Equal(expectedBytes, actual);

            // test via utf8 input
            var bytes = Text.Encoding.UTF8.GetBytes(value);
            fixed (byte* ptr = bytes)
            {
                var utf8 = new Span<byte>(ptr, bytes.Length);
                Assert.True(encoder.TryComputeEncodedBytes(utf8, out actual));
                Assert.Equal(expectedBytes, actual);
            }

            // test via utf16 input
            bytes = Text.Encoding.Unicode.GetBytes(value);
            fixed (byte* ptr = bytes)
            {
                var utf16 = new Span<char>(ptr, bytes.Length / 2);
                Assert.True(encoder.TryComputeEncodedBytes(utf16, out actual));
                Assert.Equal(expectedBytes, actual);
            }

            // test via utf32 input
            bytes = Text.Encoding.UTF32.GetBytes(value);
            fixed (byte* ptr = bytes)
            {
                var utf32 = new Span<uint>(ptr, bytes.Length / 4);
                Assert.True(encoder.TryComputeEncodedBytes(utf32, out actual));
                Assert.Equal(expectedBytes, actual);
            }
        }

        [Fact]
        public void TryComputeEncodedBytesIllegal_Utf8()
        {
            string text = Encoding.TextEncoderTestHelper.GenerateOnlyInvalidString(20);

            int bytes;
            Assert.False(TextEncoder.Utf8.TryComputeEncodedBytes(text, out bytes));
        }
    }
}
