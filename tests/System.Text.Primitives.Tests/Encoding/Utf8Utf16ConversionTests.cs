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

        public static object[][] InvalidUtf8ToUtf16SequenceData = {
            // new object[] {
            //     consumed,
            //     new byte[] { 0x00, 0x00, ... }, // Input
            //     new char[] { 0x00, 0x00, ... }, // Expected output
            // },
            new object[] {  // short; slow loop only; starts with invalid first byte
                0,
                new byte[] { 0x80, 0x41, 0x42 },
                new char[] { },
            },
            new object[] { // short; slow loop only; starts with invalid first byte
                0,
                new byte[] { 0xA0, 0x41, 0x42 },
                new char[] { },
            },
            new object[] {  // short; slow loop only; invalid long code after first byte
                0,
                new byte[] { 0xC0, 0x00 },
                new char[] {},
            },
            new object[] {  // short; slow loop only; invalid long code started after consuming a byte
                1,
                new byte[] { 0x41, 0xC0, 0x00 },
                new char[] { 'A' },
            },
            new object[] { // short; slow loop only; incomplete 2-byte long code at end of buffer
                2,
                new byte[] { 0x41, 0x42, 0xC0 },
                new char[] { 'A', 'B' },
            },
            new object[] { // short; slow loop only; incomplete 3-byte long code at end of buffer
                2,
                new byte[] { 0x41, 0x42, 0xE0, 0x83 },
                new char[] { 'A', 'B' },
            },
            new object[] { // short; slow loop only; incomplete 4-byte long code at end of buffer
                2,
                new byte[] { 0x41, 0x42, 0xF0, 0x83, 0x84 },
                new char[] { 'A', 'B' },
            },
            new object[] {  // long; fast loop only; starts with invalid first byte
                0,
                new byte[] { 0x80, 0x41, 0x42, 0x43, 0x44, 0x45, 0x46, 0x47, 0x48, 0x49, 0x50, 0x51, 0x52, 0x53, 0x54 },
                new char[] { },
            },
            new object[] { // long; fast loop only; starts with invalid first byte
                0,
                new byte[] { 0xA0, 0x41, 0x42, 0x43, 0x44, 0x45, 0x46, 0x47, 0x48, 0x49, 0x50, 0x51, 0x52, 0x53, 0x54 },
                new char[] { },
            },
            new object[] {  // long; fast loop only; invalid long code after first byte
                0,
                new byte[] { 0xC0, 0x00, 0x43, 0x44, 0x45, 0x46, 0x47, 0x48, 0x49, 0x50, 0x51, 0x52, 0x53, 0x54 },
                new char[] {},
            },
            new object[] {  // long; fast loop only; invalid long code started after consuming a byte
                1,
                new byte[] { 0x41, 0xC0, 0x00, 0x43, 0x44, 0x45, 0x46, 0x47, 0x48, 0x49, 0x50, 0x51, 0x52, 0x53, 0x54 },
                new char[] { 'A' },
            },
            new object[] { // long; incomplete 2-byte long code at end of buffer
                15,
                new byte[] { 0x41, 0x42, 0x43, 0x44, 0x45, 0x46, 0x47, 0x48, 0x49, 0x4A, 0x4B, 0x4C, 0x4D, 0x4E, 0x4F, 0xC0 },
                new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O' },
            },
            new object[] { // long; incomplete 3-byte long code at end of buffer
                15,
                new byte[] { 0x41, 0x42, 0x43, 0x44, 0x45, 0x46, 0x47, 0x48, 0x49, 0x4A, 0x4B, 0x4C, 0x4D, 0x4E, 0x4F, 0xE0, 0x83 },
                new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O' },
            },
            new object[] { // long; incomplete 4-byte long code at end of buffer
                15,
                new byte[] { 0x41, 0x42, 0x43, 0x44, 0x45, 0x46, 0x47, 0x48, 0x49, 0x4A, 0x4B, 0x4C, 0x4D, 0x4E, 0x4F, 0xF0, 0x83, 0x84 },
                new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O' },
            },
            new object[] { // long; fast loop only; incomplete 2-byte long code inside buffer
                3,
                new byte[] { 0x41, 0x42, 0x43, 0xC0, 0x44, 0x45, 0x46, 0x47, 0x48, 0x49, 0x4A, 0x4B, 0x4C, 0x4D, 0x4E, 0x4F },
                new char[] { 'A', 'B', 'C' },
            },
            new object[] { // long; fast loop only; incomplete 2-byte long code inside buffer
                3,
                new byte[] { 0x41, 0x42, 0x43, 0xE0, 0x83, 0x44, 0x45, 0x46, 0x47, 0x48, 0x49, 0x4A, 0x4B, 0x4C, 0x4D, 0x4E, 0x4F },
                new char[] { 'A', 'B', 'C' },
            },
            new object[] { // long; fast loop only; incomplete 2-byte long code inside buffer
                3,
                new byte[] { 0x41, 0x42, 0x43, 0xF0, 0x83, 0x84, 0x44, 0x45, 0x46, 0x47, 0x48, 0x49, 0x4A, 0x4B, 0x4C, 0x4D, 0x4E, 0x4F },
                new char[] { 'A', 'B', 'C' },
            },
            new object[] { // long; fast loop only; incomplete long code inside unrolled loop
                9,
                new byte[] { 0x41, 0x42, 0x43, 0x44, 0x45, 0x46, 0x47, 0x48, 0x49, 0xF0, 0x83, 0x84, 0x44, 0x45, 0x46, 0x47, 0x48, 0x49, 0x4A, 0x4B, 0x4C, 0x4D, 0x4E, 0x4F },
                new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I' },
            },
            new object[] { // long; fast loop only; bad long code starting byte inside unrolled loop
                9,
                new byte[] { 0x41, 0x42, 0x43, 0x44, 0x45, 0x46, 0x47, 0x48, 0x49, 0x85, 0x84, 0x44, 0x45, 0x46, 0x47, 0x48, 0x49, 0x4A, 0x4B, 0x4C, 0x4D, 0x4E, 0x4F },
                new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I' },
            },
        };

        [Theory]
        [MemberData("InvalidUtf8ToUtf16SequenceData")]
        public void InvalidUtf8ToUtf16SequenceTests(int expectedConsumed, byte[] input, char[] expectedOutput)
        {
            // Allocate a buffer large enough to hold expected output plus a bit of room to see what happens.
            Span<char> actualOutput = new Span<char>(new char[expectedOutput.Length + 10]);
            Assert.False(TextEncoder.Utf8.TryDecode(input, actualOutput, out int consumed, out int written));
            Assert.Equal(expectedConsumed, consumed);
            Assert.Equal(expectedOutput.Length, written);

            actualOutput = actualOutput.Slice(0, written);
            Assert.True(actualOutput.SequenceEqual(expectedOutput));
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
            int neededChars = Text.Encoding.UTF8.GetCharCount(data);
            char[] expected = new char[neededChars];
            Text.Encoding.UTF8.GetChars(data, 0, data.Length, expected, 0);

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
            return Text.Encoding.UTF32.GetChars(utf32);
        }

        static byte[] GenerateUtf8String(int length, int minCodePoint, int maxCodePoint)
        {
            char[] strChars = GenerateUtf16String(length, minCodePoint, maxCodePoint);
            return Text.Encoding.UTF8.GetBytes(strChars);
        }

        static readonly Random Rnd = new Random(23098423);
    }
}
