// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Xunit;
using System.Buffers;
using System.Buffers.Text;
using System.Runtime.InteropServices;

namespace System.Text.Primitives.Tests
{
    public class Utf8Utf16ConversionTests
    {
        public static object[][] InvalidUtf8ToUtf16SequenceData = {
            // new object[] {
            //     consumed,
            //     new byte[] { 0x00, 0x00, ... }, // Input
            //     new char[] { 0x00, 0x00, ... }, // Expected output
            //     expected result
            // },
            new object[] {  // short; slow loop only; starts with invalid first byte
                0,
                new byte[] { 0x80, 0x41, 0x42 },
                new char[] { },
                OperationStatus.InvalidData,
            },
            new object[] { // short; slow loop only; starts with invalid first byte
                0,
                new byte[] { 0xA0, 0x41, 0x42 },
                new char[] { },
                OperationStatus.InvalidData,
            },
            new object[] {  // short; slow loop only; invalid long code after first byte
                0,
                new byte[] { 0xC0, 0x00 },
                new char[] {},
                OperationStatus.InvalidData,
            },
            new object[] {  // short; slow loop only; invalid long code started after consuming a byte
                1,
                new byte[] { 0x41, 0xC0, 0x00 },
                new char[] { 'A' },
                OperationStatus.InvalidData,
            },
            new object[] { // short; slow loop only; incomplete 2-byte long code at end of buffer
                2,
                new byte[] { 0x41, 0x42, 0xC0 },
                new char[] { 'A', 'B' },
                OperationStatus.NeedMoreData,
            },
            new object[] { // short; slow loop only; incomplete 3-byte long code at end of buffer
                2,
                new byte[] { 0x41, 0x42, 0xE0, 0x83 },
                new char[] { 'A', 'B' },
                OperationStatus.NeedMoreData,
            },
            new object[] { // short; slow loop only; incomplete 4-byte long code at end of buffer
                2,
                new byte[] { 0x41, 0x42, 0xF0, 0x83, 0x84 },
                new char[] { 'A', 'B' },
                OperationStatus.NeedMoreData,
            },
            new object[] {  // long; fast loop only; starts with invalid first byte
                0,
                new byte[] { 0x80, 0x41, 0x42, 0x43, 0x44, 0x45, 0x46, 0x47, 0x48, 0x49, 0x50, 0x51, 0x52, 0x53, 0x54 },
                new char[] { },
                OperationStatus.InvalidData,
            },
            new object[] { // long; fast loop only; starts with invalid first byte
                0,
                new byte[] { 0xA0, 0x41, 0x42, 0x43, 0x44, 0x45, 0x46, 0x47, 0x48, 0x49, 0x50, 0x51, 0x52, 0x53, 0x54 },
                new char[] { },
                OperationStatus.InvalidData,
            },
            new object[] {  // long; fast loop only; invalid long code after first byte
                0,
                new byte[] { 0xC0, 0x00, 0x43, 0x44, 0x45, 0x46, 0x47, 0x48, 0x49, 0x50, 0x51, 0x52, 0x53, 0x54 },
                new char[] {},
                OperationStatus.InvalidData,
            },
            new object[] {  // long; fast loop only; invalid long code started after consuming a byte
                1,
                new byte[] { 0x41, 0xC0, 0x00, 0x43, 0x44, 0x45, 0x46, 0x47, 0x48, 0x49, 0x50, 0x51, 0x52, 0x53, 0x54 },
                new char[] { 'A' },
                OperationStatus.InvalidData,
            },
            new object[] { // long; incomplete 2-byte long code at end of buffer
                15,
                new byte[] { 0x41, 0x42, 0x43, 0x44, 0x45, 0x46, 0x47, 0x48, 0x49, 0x4A, 0x4B, 0x4C, 0x4D, 0x4E, 0x4F, 0xC0 },
                new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O' },
                OperationStatus.NeedMoreData,
            },
            new object[] { // long; incomplete 3-byte long code at end of buffer
                15,
                new byte[] { 0x41, 0x42, 0x43, 0x44, 0x45, 0x46, 0x47, 0x48, 0x49, 0x4A, 0x4B, 0x4C, 0x4D, 0x4E, 0x4F, 0xE0, 0x83 },
                new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O' },
                OperationStatus.NeedMoreData,
            },
            new object[] { // long; incomplete 4-byte long code at end of buffer
                15,
                new byte[] { 0x41, 0x42, 0x43, 0x44, 0x45, 0x46, 0x47, 0x48, 0x49, 0x4A, 0x4B, 0x4C, 0x4D, 0x4E, 0x4F, 0xF0, 0x83, 0x84 },
                new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O' },
                OperationStatus.NeedMoreData,
            },
            new object[] { // long; fast loop only; incomplete 2-byte long code inside buffer
                3,
                new byte[] { 0x41, 0x42, 0x43, 0xC0, 0x44, 0x45, 0x46, 0x47, 0x48, 0x49, 0x4A, 0x4B, 0x4C, 0x4D, 0x4E, 0x4F },
                new char[] { 'A', 'B', 'C' },
                OperationStatus.InvalidData,
            },
            new object[] { // long; fast loop only; incomplete 2-byte long code inside buffer
                3,
                new byte[] { 0x41, 0x42, 0x43, 0xE0, 0x83, 0x44, 0x45, 0x46, 0x47, 0x48, 0x49, 0x4A, 0x4B, 0x4C, 0x4D, 0x4E, 0x4F },
                new char[] { 'A', 'B', 'C' },
                OperationStatus.InvalidData,
            },
            new object[] { // long; fast loop only; incomplete 2-byte long code inside buffer
                3,
                new byte[] { 0x41, 0x42, 0x43, 0xF0, 0x83, 0x84, 0x44, 0x45, 0x46, 0x47, 0x48, 0x49, 0x4A, 0x4B, 0x4C, 0x4D, 0x4E, 0x4F },
                new char[] { 'A', 'B', 'C' },
                OperationStatus.InvalidData,
            },
            new object[] { // long; fast loop only; incomplete long code inside unrolled loop
                9,
                new byte[] { 0x41, 0x42, 0x43, 0x44, 0x45, 0x46, 0x47, 0x48, 0x49, 0xF0, 0x83, 0x84, 0x44, 0x45, 0x46, 0x47, 0x48, 0x49, 0x4A, 0x4B, 0x4C, 0x4D, 0x4E, 0x4F },
                new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I' },
                OperationStatus.InvalidData,
            },
            new object[] { // long; fast loop only; bad long code starting byte inside unrolled loop
                9,
                new byte[] { 0x41, 0x42, 0x43, 0x44, 0x45, 0x46, 0x47, 0x48, 0x49, 0x85, 0x84, 0x44, 0x45, 0x46, 0x47, 0x48, 0x49, 0x4A, 0x4B, 0x4C, 0x4D, 0x4E, 0x4F },
                new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I' },
                OperationStatus.InvalidData,
            },
        };

        [Theory]
        [MemberData("InvalidUtf8ToUtf16SequenceData")]
        public void InvalidUtf8ToUtf16SequenceTests(int expectedConsumed, byte[] input, char[] expectedOutput, OperationStatus expectedResult)
        {
            // Allocate a buffer large enough to hold expected output plus a bit of room to see what happens.
            int bytesNeeded = (expectedOutput.Length + 10) * sizeof(char);
            Span<byte> actualOutput = new byte[bytesNeeded];

            var result = TextEncodings.Utf8.ToUtf16(input, actualOutput, out int consumed, out int written);
            Assert.Equal(expectedResult, result);

            Assert.Equal(expectedConsumed, consumed);
            Assert.Equal(expectedOutput.Length * sizeof(char), written);

            actualOutput = actualOutput.Slice(0, written);
            Assert.True(MemoryMarshal.Cast<byte, char>(actualOutput).SequenceEqual(expectedOutput));
        }

        [Theory]
        [InlineData(0, 0x7f)]
        [InlineData(0x80, 0x7ff)]
        [InlineData(0x800, 0x7fff)]
        [InlineData(0, 0xffff)]
        [InlineData(0x10000, 0x10ffff)]
        [InlineData(0, 0x10ffff)]
        public void RandomUtf8ToUtf16ComputeBytesTests(int minCodePoint, int maxCodePoint)
        {
            const int RandomSampleIterations = 100;

            for (var i = 0; i < RandomSampleIterations; i++)
            {
                int charCount = Rnd.Next(50, 1000);
                VerifyUtf8ToUtf16Bytes(charCount, minCodePoint, maxCodePoint);
            }
        }

        static void VerifyUtf8ToUtf16Bytes(int count, int minCodePoint, int maxCodePoint)
        {
            byte[] data = GenerateUtf8String(count, minCodePoint, maxCodePoint);

            // Encoders.Utf16 version
            Span<byte> encodedData = data;
            Assert.Equal(OperationStatus.Done, TextEncodings.Utf8.ToUtf16Length(encodedData, out int neededBytes));

            // System version
            int expectedBytes = Text.Encoding.UTF8.GetCharCount(data) * sizeof(char);

            // Compare
            Assert.Equal(expectedBytes, neededBytes);
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
            var result = TextEncodings.Utf8.ToUtf16Length(encodedData, out int needed);
            Assert.Equal(OperationStatus.Done, result);

            // Encoders.Utf16 version
            Span<byte> actual = new byte[needed];
            result = TextEncodings.Utf8.ToUtf16(encodedData, actual, out int consumed, out int written);
            Assert.Equal(OperationStatus.Done, result);

            // System version
            int neededChars = Text.Encoding.UTF8.GetCharCount(data);
            char[] expected = new char[neededChars];
            Text.Encoding.UTF8.GetChars(data, 0, data.Length, expected, 0);

            // Compare
            Assert.True(MemoryMarshal.Cast<byte, char>(actual).SequenceEqual(expected));
        }

        static byte[] GenerateUtf32String(int length, int minCodePoint, int maxCodePoint)
        {
            int[] codePoints = new int[length];
            for (var idx = 0; idx < length; idx++)
                codePoints[idx] = Rnd.Next(minCodePoint, maxCodePoint + 1);

            return MemoryMarshal.AsBytes(codePoints.AsSpan()).ToArray();
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
