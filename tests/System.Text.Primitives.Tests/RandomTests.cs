// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Xunit;

namespace System.Text.Primitives.Tests
{
    public class RandomEncoderTests
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
        {
            int expectedBytes = Text.Encoding.UTF8.GetByteCount(value);

            // test via string input
            Assert.Equal(OperationStatus.Done, TextEncodings.Utf16.ToUtf8Length(MemoryMarshal.AsBytes(value.AsSpan()), out int actual));
            Assert.Equal(expectedBytes, actual);

            // test via utf16 input
            ReadOnlySpan<byte> bytes = Text.Encoding.Unicode.GetBytes(value);
            Assert.Equal(OperationStatus.Done, TextEncodings.Utf16.ToUtf8Length(bytes, out actual));
            Assert.Equal(expectedBytes, actual);

            // test via utf32 input
            bytes = Text.Encoding.UTF32.GetBytes(value);
            Assert.Equal(OperationStatus.Done, TextEncodings.Utf32.ToUtf8Length(bytes, out actual));
            Assert.Equal(expectedBytes, actual);
        }

        [Theory]
        [MemberData("TryComputeEncodedBytesShouldMatchEncoding_Strings")]
        public void TryComputeEncodedBytesShouldMatchEncoding_Utf16(string value)
        {
            int expectedBytes = Text.Encoding.Unicode.GetByteCount(value);

            // test via utf8 input
            ReadOnlySpan<byte> bytes = Text.Encoding.UTF8.GetBytes(value);
            Assert.Equal(Buffers.OperationStatus.Done, TextEncodings.Utf8.ToUtf16Length(bytes, out int actual));
            Assert.Equal(expectedBytes, actual);

            // test via utf32 input
            bytes = Text.Encoding.UTF32.GetBytes(value);
            Assert.Equal(Buffers.OperationStatus.Done, TextEncodings.Utf32.ToUtf16Length(bytes, out actual));
            Assert.Equal(expectedBytes, actual);
        }

        [Fact]
        public void TryComputeEncodedBytesIllegal_Utf8()
        {
            string text = Encoding.TextEncoderTestHelper.GenerateOnlyInvalidString(20);
            Assert.Equal(Buffers.OperationStatus.InvalidData, TextEncodings.Utf16.ToUtf8Length(MemoryMarshal.AsBytes(text.AsSpan()), out int needed));
        }
    }
}
