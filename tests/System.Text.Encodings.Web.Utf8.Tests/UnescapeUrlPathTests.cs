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
    public class UnescapeUrlPathTests : UrlCoderTests
    {
        protected override void TestCore(string raw, string expected)
        {
            var input = GetBytes(raw);
            var destination = new Span<byte>(new byte[input.Length]);

            Assert.Equal(OperationStatus.Done, UrlDecoder.Utf8.Decode(input, destination, out int consumed, out int written));
            Assert.True(written <= input.Length);

            var unescaped = destination.Slice(0, written);
            Assert.False(unescaped == input.Slice(0, written));

            var outputDecoded = Encoding.UTF8.GetString(unescaped.ToArray());
            Assert.Equal(expected, outputDecoded);
        }

        [Fact]
        public void ThrowWhenDesinationIsSmaller()
        {
            var input = GetBytes("/test%20test");
            var destination = new Span<byte>(new byte[input.Length - 1]);

            try
            {
                UrlDecoder.Utf8.Decode(input, destination, out _, out _);
                Assert.True(false);
            }
            catch (Exception ex)
            {
                Assert.True(ex is ArgumentException);
            }
        }
    }
}
