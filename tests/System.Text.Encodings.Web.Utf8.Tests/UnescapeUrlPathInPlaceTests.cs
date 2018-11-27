// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers;
using Xunit;

namespace System.Text.Encodings.Web.Utf8.Tests
{
    /// <summary>
    /// See <typeparamref name="UrlEncoderTests"/> for test scenarios
    /// </summary>
    public class UnescapeUrlPathInPlaceTests : UrlDecoderTests
    {
        protected override void TestCore(string raw, string expected)
        {
            var input = GetBytes(raw).ToArray();

            Assert.Equal(OperationStatus.Done, UrlDecoder.Utf8.DecodeInPlace(input, input.Length, out int written));
            Assert.True(written <= input.Length);

            var outputDecoded = Encoding.UTF8.GetString(input, 0, written);
            Assert.Equal(expected, outputDecoded);
        }
    }

    public class UrlDecoderUtf8Decode : UrlCoderTests
    {
        protected override void TestCore(string raw, string expected)
        {
            var input = GetBytes(raw);
            var output = new byte[input.Length];
            Assert.Equal(OperationStatus.Done, UrlDecoder.Utf8.Decode(input, output, out int consumed, out int written));
            Assert.True(written <= input.Length);

            var outputDecoded = Encoding.UTF8.GetString(output, 0, written);
            Assert.Equal(expected, outputDecoded);
        }
    }

    public class UrlDecoderUtf8DecodeInPlace : UrlDecoderTests
    {
        protected override void TestCore(string encoded, string decoded)
        {
            var input = GetBytes(encoded).ToArray();
            Assert.Equal(OperationStatus.Done, UrlDecoder.Utf8.DecodeInPlace(input, input.Length, out int written));
            Assert.True(written <= input.Length);

            var decodedString = Encoding.UTF8.GetString(input, 0, written);
            Assert.Equal(decoded, decodedString);
        }
    }

    public class UrlEncoderUtf8Encode : UrlCoderTests
    {
        protected override void TestCore(string encoded, string decoded)
        {
            var input = GetBytes(decoded);
            var output = new byte[encoded.Length * 4];
            Assert.Equal(OperationStatus.Done, UrlEncoder.Utf8.Encode(input, output, out int consumed, out int written));
            Assert.True(written >= input.Length);

            var decodedString = Encoding.UTF8.GetString(output, 0, written);
            Assert.Equal(encoded, decodedString);
        }
    }
}
