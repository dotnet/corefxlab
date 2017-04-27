// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Text.Utf8;
using System.Text.Utf16;
using Xunit;

namespace System.Text.Primitives.Tests
{
    public class Utf8Utf16ConversionTests
    {
        [Theory]
        [InlineData("abcdefghijklmnopqrstuvwxyz")]
        [InlineData("ABCDEFGHIJKLMNOPQRSTUVWXYZ")]
        [InlineData("0123456789")]
        [InlineData(" ,.\r\n[]<>()")]
        [InlineData("")]
        [InlineData("1258")]
        [InlineData("1258Hello")]
        [InlineData("\uABCD")]
        [InlineData("a\uABEE")]
        [InlineData("a\uABEEa")]
        [InlineData("a\uABEE\uABCDa")]
        public void Utf16StringToUtf8StringToUtf16StringRoundTrip(string utf16String)
        {
            Utf8String utf8String = new Utf8String(utf16String);
            Assert.Equal(utf16String, utf8String.ToString());
        }

        [Theory]
        [InlineData("abcdefghijklmnopqrstuvwxyz")]
        [InlineData("ABCDEFGHIJKLMNOPQRSTUVWXYZ")]
        [InlineData("0123456789")]
        [InlineData(" ,.\r\n[]<>()")]
        [InlineData("")]
        [InlineData("1258")]
        [InlineData("1258Hello")]
        [InlineData("\uABCD")]
        [InlineData("a\uABEE")]
        [InlineData("a\uABEEa")]
        [InlineData("a\uABEE\uABCDa")]
        public void Utf8StringToUtf16StringToUtf8StringRoundTrip(string str)
        {
            Utf8String utf8String = new Utf8String(str);
            string utf16String = utf8String.ToString();
            TestHelper.Validate(utf8String, new Utf8String(utf16String));
        }

        public static object[][] EnumerateAndEnsureCodePointsOfTheSameUtf8AndUtf16StringsAreTheSameTestCases = {
            new object[] { "", ""},
            new object[] { "1258", "1258"},
            new object[] { "\uABCD", "\uABCD"},
            new object[] { "a\uABEE", "a\uABEE"},
            new object[] { "a\uABEEa", "a\uABEEa"},
            new object[] { "a\uABEE\uABCDa", "a\uABEE\uABCDa"}
        };

        [Theory, MemberData("EnumerateAndEnsureCodePointsOfTheSameUtf8AndUtf16StringsAreTheSameTestCases")]
        public void EnumerateAndEnsureCodePointsOfTheSameUtf8AndUtf16StringsAreTheSame(string utf16String, string str)
        {
            var utf8String = new Utf8String(str);
            var utf16StringCodePoints = new Utf16LittleEndianCodePointEnumerable(utf16String);

            var utf16CodePointEnumerator = utf16StringCodePoints.GetEnumerator();
            var utf8CodePointEnumerator = utf8String.CodePoints.GetEnumerator();

            bool moveNext;
            while (true)
            {
                moveNext = utf16CodePointEnumerator.MoveNext();
                Assert.Equal(moveNext, utf8CodePointEnumerator.MoveNext());
                if (!moveNext)
                {
                    break;
                }
                Assert.Equal(utf16CodePointEnumerator.Current, utf8CodePointEnumerator.Current);
            }
        }

        [Theory]
        [InlineData(0, 0x7f)]
        [InlineData(0x80, 0x7ff)]
        [InlineData(0x800, 0x7fff)]
        [InlineData(0, 0xffff)]
        [InlineData(0x10000, 0x10ffff)]
        [InlineData(0, 0x10ffff)]
        public void RandomUtf8ToUtf16DecodingTests(int minCodePoint, int maxCodePoint)
        {
            const int RandomSampleIterations = 100;

            for (var i = 0; i < RandomSampleIterations; i++)
            {
                int charCount = Rnd.Next(50, 1000);
                VerifyUtf8ToUtf16(charCount, minCodePoint, maxCodePoint);
            }
        }

        static void VerifyUtf8ToUtf16(int count, int minCodePoint, int maxCodePoint)
        {
            byte[] data = GenerateUtf8String(count, minCodePoint, maxCodePoint);

            Span<byte> encodedData = data;
            Assert.True(TextEncoder.Utf16.TryComputeEncodedBytes(encodedData, out int needed));

            // TextEncoder version
            Span<byte> dst = new Span<byte>(new byte[needed]);
            Span<char> actual = dst.NonPortableCast<byte, char>();
            Assert.True(TextEncoder.Utf8.TryDecode(encodedData, actual, out int consumed, out int written));

            // System version
            int neededChars = Encoding.UTF8.GetCharCount(data);
            char[] expected = new char[neededChars];
            Encoding.UTF8.GetChars(data, 0, data.Length, expected, 0);

            // Compare
            Assert.True(actual.SequenceEqual(expected));
        }

        static byte[] GenerateUtf32String(int length, int minCodePoint, int maxCodePoint)
        {
            int[] codePoints = new int[length];
            for (var idx = 0; idx < length; idx++)
                codePoints[idx] = Rnd.Next(minCodePoint, maxCodePoint + 1);

            return codePoints.AsSpan().AsBytes().ToArray();
        }

        static char[] GenerateUtf16String(int length, int minCodePoint, int maxCodePoint)
        {
            byte[] utf32 = GenerateUtf32String(length, minCodePoint, maxCodePoint);
            return Encoding.UTF32.GetChars(utf32);
        }

        static byte[] GenerateUtf8String(int length, int minCodePoint, int maxCodePoint)
        {
            char[] strChars = GenerateUtf16String(length, minCodePoint, maxCodePoint);
            return Encoding.UTF8.GetBytes(strChars);
        }

        static readonly Random Rnd = new Random(23098423);
    }
}
