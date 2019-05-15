// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers;
using System.Buffers.Text;
using System.Runtime.InteropServices;
using Xunit;

namespace System.Text.Primitives.Tests.Encoding
{
    public class Utf8EncoderTests
    {
        public static object[][] TryEncodeFromUTF16ToUTF8TestData = {
            // empty
            new object[] { new byte[] { }, new char[]{ (char)0x0050 }, OperationStatus.DestinationTooSmall },
            // multiple bytes
            new object[] { new byte[] { 0x50, 0xCF, 0xA8,  0xEA, 0xBF, 0x88, 0xF0, 0xA4, 0xA7, 0xB0 },
                new char[]{ (char)0x0050, (char)0x03E8, (char)0xAFC8, (char)0xD852, (char)0xDDF0 }, OperationStatus.Done },
        };

        [Theory, MemberData("TryEncodeFromUTF16ToUTF8TestData")]
        public void UTF16ToUTF8EncodingTestForReadOnlySpanOfChar(byte[] expectedBytes, char[] chars, OperationStatus expectedReturnVal)
        {
            ReadOnlySpan<byte> utf16 = MemoryMarshal.Cast<char, byte>(new ReadOnlySpan<char>(chars));
            Span<byte> buffer = new byte[expectedBytes.Length];

            Assert.Equal(expectedReturnVal, TextEncodings.Utf16.ToUtf8(utf16, buffer, out int consumed, out int written));
            Assert.Equal(expectedBytes.Length, written);

            if (expectedBytes.Length > 0)
            {
                Assert.Equal(utf16.Length, consumed);
                Assert.True(buffer.Slice(0, written).SequenceEqual(expectedBytes));
            }
        }

        public static object[][] TryEncodeFromUnicodeMultipleCodePointsTestData = {
             // empty
            new object[] { true, new byte[] { }, new uint[] { 0x50 }, OperationStatus.DestinationTooSmall },
            new object[] { false, new byte[] { }, new uint[] { 0x50 }, OperationStatus.DestinationTooSmall },
            // multiple bytes
            new object[] { true, new byte[] { 0x50, 0xCF, 0xA8, 0xEA, 0xBF, 0x88, 0xF0, 0xA4, 0xA7, 0xB0 },
                new uint[] { 0x50, 0x3E8, 0xAFC8, 0x249F0 } , OperationStatus.Done },
            new object[] { false, new byte[] { 0x50, 0x00, 0xE8, 0x03, 0xC8, 0xAF, 0x52, 0xD8, 0xF0, 0xDD },
                new uint[] { 0x50, 0x3E8, 0xAFC8, 0x249F0 } , OperationStatus.Done },
            // multiple bytes - buffer too small
            new object[] { true, new byte[] { 0x50 },
                new uint[] { 0x50, 0x3E8, 0xAFC8, 0x249F0 } , OperationStatus.DestinationTooSmall },
            new object[] { false, new byte[] { 0x50, 0x00 },
                new uint[] { 0x50, 0x3E8, 0xAFC8, 0x249F0 } , OperationStatus.DestinationTooSmall },
        };

        [Theory, MemberData("TryEncodeFromUnicodeMultipleCodePointsTestData")]
        public void TryEncodeFromUnicodeMultipleCodePoints(bool useUtf8Encoder, byte[] expectedBytes, uint[] codePointsArray, Buffers.OperationStatus expectedReturnVal)
        {
            ReadOnlySpan<byte> codePoints = MemoryMarshal.AsBytes(codePointsArray.AsSpan());
            Span<byte> buffer = new byte[expectedBytes.Length];
            int written;
            int consumed;
            Buffers.OperationStatus result;

            if (useUtf8Encoder)
                result = TextEncodings.Utf32.ToUtf8(codePoints, buffer, out consumed, out written);
            else
                result = TextEncodings.Utf32.ToUtf16(codePoints, buffer, out consumed, out written);

            Assert.Equal(expectedReturnVal, result);
            Assert.Equal(expectedBytes.Length, written);

            if (result == Buffers.OperationStatus.Done)
                Assert.Equal(codePoints.Length, consumed);

            Assert.True(buffer.Slice(0, written).SequenceEqual(expectedBytes), "Bad output sequence");
        }

        public static object[][] TryDecodeToUnicodeMultipleCodePointsTestData = {
            //empty
            new object[] { true, new uint[] {}, new byte[] {}, OperationStatus.Done },
            new object[] { false, new uint[] {}, new byte[] {}, OperationStatus.Done },
            // multiple bytes
            new object[] { true,
                new uint[] { 0x50, 0x3E8, 0xAFC8, 0x249F0 }, new byte[] { 0x50, 0xCF, 0xA8,  0xEA, 0xBF, 0x88, 0xF0, 0xA4, 0xA7, 0xB0 },  OperationStatus.Done },
            new object[] { false,
                new uint[] { 0x50, 0x3E8, 0xAFC8, 0x249F0 }, new byte[] {  0x50, 0x00, 0xE8,  0x03, 0xC8, 0xAF, 0x52, 0xD8, 0xF0, 0xDD },  OperationStatus.Done },
        };

        [Theory, MemberData("TryDecodeToUnicodeMultipleCodePointsTestData")]
        public void TryDecodeToUnicodeMultipleCodePoints(bool useUtf8Encoder, uint[] expectedCodePointsArray, byte[] inputBytesArray, OperationStatus expectedReturnVal)
        {
            ReadOnlySpan<byte> expectedBytes = MemoryMarshal.AsBytes<uint>(expectedCodePointsArray);
            ReadOnlySpan<byte> inputBytes = inputBytesArray;
            Span<byte> codePoints = new byte[expectedBytes.Length];
            int written;
            int consumed;
            OperationStatus result;

            if (useUtf8Encoder)
                result = TextEncodings.Utf8.ToUtf32(inputBytes, codePoints, out consumed, out written);
            else
                result = TextEncodings.Utf16.ToUtf32(inputBytes, codePoints, out consumed, out written);

            Assert.Equal(expectedReturnVal, result);
            Assert.Equal(inputBytes.Length, consumed);
            Assert.Equal(codePoints.Length, written);
            Assert.True(expectedBytes.SequenceEqual(codePoints), "Bad output sequence");
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void BruteTestingRoundtripEncodeDecodeAllUnicodeCodePoints(bool useUtf8Encoder)
        {
            const int maximumValidCodePoint = 0x10FFFF;
            uint[] expectedCodePoints = new uint[maximumValidCodePoint + 1];
            for (uint i = 0; i <= maximumValidCodePoint; i++)
            {
                if (!EncodingHelper.IsValidScalarValue(i))
                {
                    expectedCodePoints[i] = 0; // skip unsupported code points.
                }
                else
                {
                    expectedCodePoints[i] = i;
                }
            }

            ReadOnlySpan<uint> allCodePoints = expectedCodePoints;
            Span<byte> buffer = new byte[4 * (maximumValidCodePoint + 1)];
            int consumed;
            int written;

            if (useUtf8Encoder)
                Assert.Equal(OperationStatus.Done, TextEncodings.Utf32.ToUtf8(MemoryMarshal.AsBytes(allCodePoints), buffer, out consumed, out written));
            else
                Assert.Equal(OperationStatus.Done, TextEncodings.Utf32.ToUtf16(MemoryMarshal.AsBytes(allCodePoints), buffer, out consumed, out written));

            Assert.Equal(MemoryMarshal.AsBytes(allCodePoints).Length, consumed);
            buffer = buffer.Slice(0, written);

            Span<uint> utf32 = new uint[maximumValidCodePoint + 1];

            if (useUtf8Encoder)
                Assert.Equal(OperationStatus.Done, TextEncodings.Utf8.ToUtf32(buffer, MemoryMarshal.AsBytes(utf32), out consumed, out written));
            else
                Assert.Equal(OperationStatus.Done, TextEncodings.Utf16.ToUtf32(buffer, MemoryMarshal.AsBytes(utf32), out consumed, out written));

            Assert.Equal(buffer.Length, consumed);
            Assert.Equal((maximumValidCodePoint + 1) * sizeof(uint), written);
            Assert.True(allCodePoints.SequenceEqual(utf32), "Bad output from round-trip");
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void EncodeAllUnicodeCodePoints(bool useUtf8Encoder)
        {
            Text.Encoding systemEncoder = useUtf8Encoder ? Text.Encoding.UTF8 : Text.Encoding.Unicode;
            const uint maximumValidCodePoint = 0x10FFFF;
            uint[] codePoints = new uint[maximumValidCodePoint + 1];

            var plainText = new StringBuilder();
            for (int i = 0; i <= maximumValidCodePoint; i++)
            {
                if (!EncodingHelper.IsValidScalarValue((uint)i))
                {
                    codePoints[i] = 0; // skip unsupported characters
                    plainText.Append((char)0);
                }
                else
                {
                    codePoints[i] = (uint)i;

                    if (i > 0xFFFF)
                    {
                        plainText.Append(char.ConvertFromUtf32(i));
                    }
                    else
                    {
                        plainText.Append((char)i);
                    }
                }
            }

            ReadOnlySpan<uint> allCodePoints = codePoints;
            Span<byte> buffer = new byte[4 * (maximumValidCodePoint + 1)];
            int written;
            int consumed;

            if (useUtf8Encoder)
                Assert.Equal(Buffers.OperationStatus.Done, TextEncodings.Utf32.ToUtf8(MemoryMarshal.AsBytes(allCodePoints), buffer, out consumed, out written));
            else
                Assert.Equal(Buffers.OperationStatus.Done, TextEncodings.Utf32.ToUtf16(MemoryMarshal.AsBytes(allCodePoints), buffer, out consumed, out written));

            buffer = buffer.Slice(0, written);

            string unicodeString = plainText.ToString();
            ReadOnlySpan<char> characters = unicodeString.AsSpan();
            int byteCount = systemEncoder.GetByteCount(unicodeString);
            byte[] expectedBytes = new byte[byteCount];

            systemEncoder.GetBytes(characters.ToArray(), 0, characters.Length, expectedBytes, 0);

            Assert.Equal(expectedBytes.Length, buffer.Length);
            Assert.True(buffer.SequenceEqual(expectedBytes), "Bad output from system encoding comparison");
        }

        public static object[] PartialEncodeUtf8ToUtf16TestCases = new object[]
        {
            //new object[]
            //{
            //    /* output buffer size */, /* consumed on first pass */,
            //    new byte[] { /* UTF-8 encoded input data */ },
            //    new byte[] { /* expected output first pass */ },
            //    new byte[] { /* expected output second pass */ },
            //},
            new object[]
            {
                4, 2,
                new byte[] { 0x48, 0x65, 0x6C, 0x6C, 0x6F },
                new byte[] { 0x48, 0x00, 0x65, 0x00 },
                new byte[] { 0x6C, 0x00, 0x6C, 0x00, 0x6F, 0x00 },
            },
            new object[]
            {
                5, 6,
                new byte[] { 0xE6, 0xA8, 0x99, 0xE6, 0xBA, 0x96, 0xE8, 0x90, 0xAC, 0xE5, 0x9C, 0x8B, 0xE7, 0xA2, 0xBC },
                new byte[] { 0x19, 0x6A, 0x96, 0x6E },
                new byte[] { 0x2C, 0x84, 0x0B, 0x57, 0xBC, 0x78 },
            },
        };

        [Theory, MemberData("PartialEncodeUtf8ToUtf16TestCases")]
        public void TryPartialUtf8ToUtf16DecodingTest(int outputSize, int expectedConsumed, byte[] inputBytes, byte[] expected1, byte[] expected2)
        {

            ReadOnlySpan<byte> input = inputBytes;
            Span<byte> output = new byte[outputSize];

            Assert.Equal(Buffers.OperationStatus.DestinationTooSmall, TextEncodings.Utf8.ToUtf16(input, output, out int consumed, out int written));
            Assert.Equal(expected1.Length, written);
            Assert.Equal(expectedConsumed, consumed);
            Assert.True(output.Slice(0, written).SequenceEqual(expected1), "Bad first segment of partial sequence");

            input = input.Slice(consumed);
            output = new byte[expected2.Length];
            Assert.Equal(Buffers.OperationStatus.Done, TextEncodings.Utf8.ToUtf16(input, output, out consumed, out written));
            Assert.Equal(expected2.Length, written);
            Assert.Equal(input.Length, consumed);
            Assert.True(output.Slice(0, written).SequenceEqual(expected2), "Bad second segment of partial sequence");
        }

        public static object[] PartialEncodeUtf16ToUtf8TestCases = new object[]
        {
            //new object[]
            //{
            //    /* output buffer size */, /* consumed on first pass */,
            //    new char[] { /* UTF-16 encoded input data */ },
            //    new byte[] { /* expected output first pass */ },
            //    new byte[] { /* expected output second pass */ },
            //},
            new object[]
            {
                2, 4,
                new char[] { '\u0048', '\u0065', '\u006C', '\u006C', '\u006F' },
                new byte[] { 0x48, 0x65 },
                new byte[] { 0x6C, 0x6C, 0x6F },
            },
            new object[]
            {
                7, 4,
                new char[] { '\u6A19', '\u6E96', '\u842C', '\u570B', '\u78BC' },
                new byte[] { 0xE6, 0xA8, 0x99, 0xE6, 0xBA, 0x96 },
                new byte[] { 0xE8, 0x90, 0xAC, 0xE5, 0x9C, 0x8B, 0xE7, 0xA2, 0xBC },
            },
        };

        [Theory, MemberData("PartialEncodeUtf16ToUtf8TestCases")]
        public void TryPartialUtf16ToUtf8EncodingTest(int outputSize, int expectedConsumed, char[] inputBytes, byte[] expected1, byte[] expected2)
        {

            ReadOnlySpan<byte> input = MemoryMarshal.AsBytes(inputBytes.AsSpan());
            Span<byte> output = new byte[outputSize];

            Assert.Equal(Buffers.OperationStatus.DestinationTooSmall, TextEncodings.Utf16.ToUtf8(input, output, out int consumed, out int written));
            Assert.Equal(expected1.Length, written);
            Assert.Equal(expectedConsumed, consumed);
            Assert.True(output.Slice(0, written).SequenceEqual(expected1), "Bad first segment of partial sequence");

            input = input.Slice(consumed);
            output = new byte[expected2.Length];
            Assert.Equal(Buffers.OperationStatus.Done, TextEncodings.Utf16.ToUtf8(input, output, out consumed, out written));
            Assert.Equal(expected2.Length, written);
            Assert.Equal(input.Length, consumed);
            Assert.True(output.Slice(0, written).SequenceEqual(expected2), "Bad second segment of partial sequence");
        }
    }
}
