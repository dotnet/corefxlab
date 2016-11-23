using Xunit;

namespace System.Text.Encodings.Web.Utf8.Tests
{
    /// <summary>
    /// See <typeparamref name="UrlEncoderTests"/> for test scenarios
    /// </summary>
    public class UnescapeUrlPathTests : UrlEncoderTests
    {
        protected override void TestCore(string raw, string expected)
        {
            var input = GetBytes(raw);
            var destination = new Span<byte>(new byte[input.Length]);

            var len = UrlEncoder.Decode(input, destination);
            Assert.True(len <= input.Length);

            var unescaped = destination.Slice(0, len);
            Assert.False(unescaped == input.Slice(0, len));

            var outputDecoded = Encoding.UTF8.GetString(unescaped.ToArray());
            Assert.Equal(expected, outputDecoded);
        }

        [Fact]
        public void ThrowWhenDesinationIsSmaller()
        {
            var input = GetBytes("/test%20test");
            var destination = new Span<byte>(new byte[input.Length - 1]);

            Assert.Throws<ArgumentException>(() =>
            {
                UrlEncoder.Decode(input, destination);
            });
        }
    }
}
