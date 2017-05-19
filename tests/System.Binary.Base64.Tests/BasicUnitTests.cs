// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Xunit;
using System.Collections.Generic;

namespace System.Binary.Base64.Tests
{
    public class Base64Tests
    {
        static string s_characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/=";

        static readonly byte[] s_encodingMap = {
            65, 66, 67, 68, 69, 70, 71, 72,
            73, 74, 75, 76, 77, 78, 79, 80,
            81, 82, 83, 84, 85, 86, 87, 88,
            89, 90, 97, 98, 99, 100, 101, 102,
            103, 104, 105, 106, 107, 108, 109, 110,
            111, 112, 113, 114, 115, 116, 117, 118,
            119, 120, 121, 122, 48, 49, 50, 51,
            52, 53, 54, 55, 56, 57, 43, 47,
            61
        };

        static readonly byte[] s_decodingMap = {
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 62, 0, 0, 0, 63,
            52, 53, 54, 55, 56, 57, 58, 59, 60, 61, 0, 0, 0, 64, 0, 0,
            0, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14,
            15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 0, 0, 0, 0, 0,
            0, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40,
            41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51
        };

        static readonly byte s_encodingPad = 61;    // s_encodingMap[64] is '=', for padding

        [Fact]
        public void BasicEncodingDecoding()
        {
            var list = new List<byte>();
            for (int value = 0; value < 256; value++)
            {
                list.Add((byte)value);
            }
            var testBytes = list.ToArray();

            for (int value = 0; value < 256; value++)
            {

                var sourceBytes = testBytes.AsSpan().Slice(0, value + 1);
                var encodedBytes = new byte[Base64.ComputeEncodedLength(sourceBytes.Length)].AsSpan();
                var encodedBytesCount = Base64.Encode(sourceBytes, encodedBytes);
                Assert.Equal(encodedBytes.Length, encodedBytesCount);

                var encodedText = Text.Encoding.ASCII.GetString(encodedBytes.ToArray());
                var expectedText = Convert.ToBase64String(testBytes, 0, value + 1);
                Assert.Equal(expectedText, encodedText);

                var decodedBytes = new byte[sourceBytes.Length];
                var decodedByteCount = Base64.Decode(encodedBytes, decodedBytes.AsSpan());
                Assert.Equal(sourceBytes.Length, decodedByteCount);

                for (int i = 0; i < decodedBytes.Length; i++)
                {
                    Assert.Equal(sourceBytes[i], decodedBytes[i]);
                }
            }
        }

        [Fact]
        public void ComputeEncodedLength()
        {
            // (int.MaxValue - 4)/(4/3) => 1610612733, otherwise integer overflow
            int[] input = { int.MinValue, -50, -1, 0, 1, 2, 3, 4, 5, 6, 1610612728, 1610612729, 1610612730, 1610612731, 1610612732, 1610612733 };
            int[] expected = { 0, 0, 0, 0, 4, 4, 4, 8, 8, 8, 2147483640, 2147483640, 2147483640, 2147483644, 2147483644, 2147483644 };
            for (int i = 0; i < input.Length; i++)
            {
                Assert.Equal(expected[i], Base64.ComputeEncodedLength(input[i]));
            }

            Assert.True(Base64.ComputeEncodedLength(1610612734) < 0);   // integer overflow
        }

        [Fact]
        public void ComputeDecodedLength()
        {
            Span<byte> sourceEmpty = Span<byte>.Empty;
            Assert.Equal(0, Base64.ComputeDecodedLength(sourceEmpty));

            // int.MaxValue - (int.MaxValue % 4) => 2147483644, largest multiple of 4 less than int.MaxValue
            // CLR default limit of 2 gigabytes (GB).
            int[] input = { 4, 8, 12, 16, 20, 2000000000 };
            int[] expected = { 3, 6, 9, 12, 15, 1500000000 };
            
            for (int i = 0; i < input.Length; i++)
            {
                int sourceLength = input[i];
                Span<byte> source = new byte[sourceLength];
                Assert.Equal(expected[i], Base64.ComputeDecodedLength(source));
                source[sourceLength - 1] = s_encodingPad;                          // single character padding
                Assert.Equal(expected[i] - 1, Base64.ComputeDecodedLength(source));
                source[sourceLength - 2] = s_encodingPad;                          // two characters padding
                Assert.Equal(expected[i] - 2, Base64.ComputeDecodedLength(source));
            }

            // Lengths that are not a multiple of 4.
            int[] invalidInput = { 1, 2, 3, 5, 6, 7, 9, 10, 11, 13, 14, 15, 1001, 1002, 1003};

            for (int i = 0; i < invalidInput.Length; i++)
            {
                int sourceLength = invalidInput[i];
                Span<byte> source = new byte[sourceLength];
                Assert.Equal(0, Base64.ComputeDecodedLength(source));
                source[sourceLength - 1] = s_encodingPad;
                Assert.Equal(0, Base64.ComputeDecodedLength(source));
                if (sourceLength > 1)
                {
                    source[sourceLength - 2] = s_encodingPad;
                    Assert.Equal(0, Base64.ComputeDecodedLength(source));
                }
            }
        }

        [Fact]
        public void DecodeInPlace()
        {
            var list = new List<byte>();
            for (int value = 0; value < 256; value++)
            {
                list.Add((byte)value);
            }
            var testBytes = list.ToArray();

            for (int value = 0; value < 256; value++)
            {
                var sourceBytes = testBytes.AsSpan().Slice(0, value + 1);
                var buffer = new byte[Base64.ComputeEncodedLength(sourceBytes.Length)];
                var bufferSlice = buffer.AsSpan();

                Base64.Encode(sourceBytes, bufferSlice);

                var encodedText = Text.Encoding.ASCII.GetString(bufferSlice.ToArray());
                var expectedText = Convert.ToBase64String(testBytes, 0, value + 1);
                Assert.Equal(expectedText, encodedText);

                var decodedByteCount = Base64.DecodeInPlace(bufferSlice);
                Assert.Equal(sourceBytes.Length, decodedByteCount);

                for (int i = 0; i < decodedByteCount; i++)
                {
                    Assert.Equal(sourceBytes[i], buffer[i]);
                }
            }
        }

        [Fact]
        public void EncodeInPlace()
        {
            const int numberOfBytes = 15;

            var list = new List<byte>();
            for (int value = 0; value < numberOfBytes; value++)
            {
                list.Add((byte)value);
            }
            // padding
            for (int i = 0; i < (numberOfBytes / 3) + 2; i++)
            {
                list.Add(0);
            }

            var testBytes = list.ToArray();

            for (int numberOfBytesToTest = 1; numberOfBytesToTest <= numberOfBytes; numberOfBytesToTest++)
            {
                var copy = testBytes.Clone();
                var expectedText = Convert.ToBase64String(testBytes, 0, numberOfBytesToTest);

                var encoded = Base64.EncodeInPlace(testBytes, numberOfBytesToTest);
                var encodedText = Text.Encoding.ASCII.GetString(testBytes, 0, encoded);

                Assert.Equal(expectedText, encodedText);
            }
        }

        [Fact]
        public void GenerateEncodingMapAndVerify()
        {
            var data = new byte[65]; // Base64 + pad character
            for (int i = 0; i < s_characters.Length; i++)
            {
                data[i] = (byte)s_characters[i];
            }
            Assert.True(s_encodingMap.AsSpan().SequenceEqual(data));
            Assert.Equal(s_encodingPad, s_encodingMap[64]);
        }

        [Fact]
        public void GenerateDecodingMapAndVerify()
        {
            var data = new byte[123]; // 'z' = 123
            for (int i = 0; i < s_characters.Length; i++)
            {
                data[s_characters[i]] = (byte)i;
            }
            Assert.True(s_decodingMap.AsSpan().SequenceEqual(data));
        }
    }
}
