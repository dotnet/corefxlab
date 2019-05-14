// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers.Text;
using Xunit;

namespace System.Text.Encoders.Tests
{
    public class PrimitiveEncoderTestsAscii
    {
        [Theory]
        [InlineData("")]
        [InlineData("Hello World")]
        public void AsciiToUtf16StringBasics(string original)
        {
            var encoded = (Span<byte>)Encoding.ASCII.GetBytes(original);
            var decoded = TextEncodings.Ascii.ToUtf16String(encoded);
            Assert.Equal(original, decoded);
        }

        [Fact]
        public void AsciiToUtf16StringWorksOnAllAsciiChars()
        {
            for (int index = 0; index < 100; index++) {
                var encoded = (Span<byte>)new byte[100];
                for (int encodedByte = 0; encodedByte < 128; encodedByte++) {
                    encoded[index] = (byte)encodedByte;
                    var result = TextEncodings.Ascii.ToUtf16String(encoded);
                }
            }
        }

        [Fact]
        public void AsciiToUtf16StringFailsOnNonAscii()
        {
            for (int index = 0; index < 100; index++) {
                var encoded = (Span<byte>)new byte[100];
                for (int encodedByte = 128; encodedByte < 256; encodedByte++) {
                    encoded[0] = (byte)encodedByte;
                    bool exception = false;
                    try {
                        var result = TextEncodings.Ascii.ToUtf16String(encoded);
                    }
                    catch(ArgumentException) {
                        exception = true;
                    }
                    Assert.True(exception);
                }
            }
        }
    }
}
