// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Xunit;

namespace System.Text.Primitives.Tests
{
    public class PrimitiveEncoderTestsAscii
    {
        [Theory]
        [InlineData("")]
        [InlineData("Hello World")]
        public void DecodeAsciiToStringBasics(string original)
        {
            var encoded = (Span<byte>)Text.Encoding.ASCII.GetBytes(original);
            var decoded = encoded.DecodeAscii();
            Assert.Equal(original, decoded);
        }

        [Fact]
        public void DecodeAsciiWorksOnAllAsciiChars()
        {
            for (int index = 0; index < 100; index++) {
                var encoded = (Span<byte>)new byte[100];
                for (int encodedByte = 0; encodedByte < 128; encodedByte++) {
                    encoded[index] = (byte)encodedByte;
                    var result = encoded.DecodeAscii();
                }
            }
        }

        [Fact]
        public void DecodeAsciiToStringFailsOnNonAscii()
        {
            for (int index = 0; index < 100; index++) {
                var encoded = (Span<byte>)new byte[100];
                for (int encodedByte = 128; encodedByte < 256; encodedByte++) {
                    encoded[0] = (byte)encodedByte;
                    bool exception = false;
                    try {
                        var result = encoded.DecodeAscii();
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
