// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Xunit;

namespace System.Text.Encodings.Web.Utf8.Tests
{
    /// <summary>
    /// The scenarios in the base test cases will be executed in its derived test class. 
    /// 
    /// The derived test classes need to override <paramref name="TestCore"/> method to 
    /// alter the core test logic on the same set of test scenarios.
    /// </summary>

    public abstract class UrlDecoderTests : UrlCoderTests
    {
        [Theory]
        [InlineData("/foo/bar", "/foo/bar")]
        [InlineData("/foo/BAR", "/foo/BAR")]
        [InlineData("/foo/", "/foo/")]
        [InlineData("/", "/")]
        [InlineData("", "")]
        [InlineData("   ", "   ")]
        public void NormalCases(string raw, string expect) => TestCore(raw, expect);

        [Theory]
        [InlineData("%2F", "%2F")]
        [InlineData("/foo%2Fbar", "/foo%2Fbar")]
        [InlineData("/foo%2F%20bar", "/foo%2F bar")]
        public void SkipForwardSlash(string raw, string expect) => TestCore(raw, expect);

        [Theory]
        [InlineData("%D0%A4", "Ф")]
        [InlineData("%d0%a4", "Ф")]
        [InlineData("%E0%A4%AD", "भ")]
        [InlineData("%e0%A4%Ad", "भ")]
        [InlineData("%F0%A4%AD%A2", "𤭢")]
        [InlineData("%F0%a4%Ad%a2", "𤭢")]
        [InlineData("%48%65%6C%6C%6F%20%57%6F%72%6C%64", "Hello World")]
        [InlineData("%48%65%6C%6C%6F%2D%C2%B5%40%C3%9F%C3%B6%C3%A4%C3%BC%C3%A0%C3%A1", "Hello-µ@ßöäüàá")]
        // Test the borderline cases of overlong UTF8.
        [InlineData("%C2%80", "\u0080")]
        [InlineData("%E0%A0%80", "\u0800")]
        [InlineData("%F0%90%80%80", "\U00010000")]
        [InlineData("%63", "c")]
        [InlineData("%32", "2")]
        [InlineData("%20", " ")]
        public void ValidUTF8(string raw, string expect) => TestCore(raw, expect);

        [Theory]
        [InlineData("%C3%84ra%20Benetton", "Ära Benetton")]
        [InlineData("%E6%88%91%E8%87%AA%E6%A8%AA%E5%88%80%E5%90%91%E5%A4%A9%E7%AC%91%E5%8E%BB%E7%95%99%E8%82%9D%E8%83%86%E4%B8%A4%E6%98%86%E4%BB%91", "我自横刀向天笑去留肝胆两昆仑")]
        public void Internationalized(string raw, string expect) => TestCore(raw, expect);

        [Theory]
        // Overlong ASCII
        [InlineData("%C0%A4", "%C0%A4")]
        [InlineData("%C1%BF", "%C1%BF")]
        [InlineData("%E0%80%AF", "%E0%80%AF")]
        [InlineData("%E0%9F%BF", "%E0%9F%BF")]
        [InlineData("%F0%80%80%AF", "%F0%80%80%AF")]
        [InlineData("%F0%8F%8F%BF", "%F0%8F%8F%BF")]
        // Incomplete
        [InlineData("%", "%")]
        [InlineData("%%", "%%")]
        [InlineData("%A", "%A")]
        [InlineData("%AY", "%AY")]
        [InlineData("%Y", "%Y")]
        // Mixed
        [InlineData("%%32", "%2")]
        [InlineData("%%20", "% ")]
        [InlineData("%C0%A4%32", "%C0%A42")]
        [InlineData("%32%C0%A4%32", "2%C0%A42")]
        [InlineData("%C0%32%A4", "%C02%A4")]
        public void InvalidUTF8(string raw, string expect) => TestCore(raw, expect);

        [Theory]
        [InlineData("/foo%2Fbar", "/foo%2Fbar")]
        [InlineData("/foo%2Fba", "/foo%2Fba")]
        [InlineData("/foo%2Fb", "/foo%2Fb")]
        [InlineData("%D0%A4", "Ф")]
        [InlineData("%D0%A", "%D0%A")]
        [InlineData("%D0%", "%D0%")]
        [InlineData("%D0", "%D0")]
        [InlineData("%D", "%D")]
        [InlineData("%", "%")]
        [InlineData("%C2%B5%40%C3%9F%C3%B6%C3%A4%C3%BC%C3%A0%C3%A1", "µ@ßöäüàá")]
        [InlineData("%C2%B5%40%C3%9F%C3%B6%C3%A4%C3%BC%C3%A0%C3%A", "µ@ßöäüà%C3%A")]
        public void DecodeWithBoundary(string raw, string expect) => TestCore(raw, expect);
    }

    public abstract class UrlCoderTests
    {
        protected abstract void TestCore(string encoded, string decoded);

        protected ReadOnlySpan<byte> GetBytes(string str) => Encoding.UTF8.GetBytes(str);
    }
}
