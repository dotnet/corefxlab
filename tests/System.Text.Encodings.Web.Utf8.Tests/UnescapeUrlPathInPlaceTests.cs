using Xunit;

namespace System.Text.Encodings.Web.Utf8.Tests
{
    /// <summary>
    /// See <typeparamref name="UrlEncoderTests"/> for test scenarios
    /// </summary>
    public class UnescapeUrlPathInPlaceTests : UrlEncoderTests
    {
        protected override void TestCore(string raw, string expected)
        {
            var input = GetBytes(raw);

            var len = UrlEncoder.DecodeInPlace(input);
            Assert.True(len <= input.Length);

            var outputDecoded = Encoding.UTF8.GetString(input.Slice(0, len).ToArray());
            Assert.Equal(expected, outputDecoded);
        }
    }
}
