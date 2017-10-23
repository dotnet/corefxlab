// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System.Collections.Generic;
using System.Linq;
using System.Buffers;
using Xunit;

namespace System.Binary.Base64.Tests
{
    public static class Base64TestHelper
    {
        public static string s_characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/";

        // Pre-computing this table using a custom string(s_characters) and GenerateEncodingMapAndVerify (found in tests)
        public static readonly byte[] s_encodingMap = {
            65, 66, 67, 68, 69, 70, 71, 72,         //A..H
            73, 74, 75, 76, 77, 78, 79, 80,         //I..P
            81, 82, 83, 84, 85, 86, 87, 88,         //Q..X
            89, 90, 97, 98, 99, 100, 101, 102,      //Y..Z, a..f
            103, 104, 105, 106, 107, 108, 109, 110, //g..n
            111, 112, 113, 114, 115, 116, 117, 118, //o..v
            119, 120, 121, 122, 48, 49, 50, 51,     //w..z, 0..3
            52, 53, 54, 55, 56, 57, 43, 47          //4..9, +, /
        };

        // Pre-computing this table using a custom string(s_characters) and GenerateDecodingMapAndVerify (found in tests)
        public static readonly sbyte[] s_decodingMap = {
            -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
            -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
            -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, 62, -1, -1, -1, 63,         //62 is placed at index 43 (for +), 63 at index 47 (for /)
            52, 53, 54, 55, 56, 57, 58, 59, 60, 61, -1, -1, -1, -1, -1, -1,         //52-61 are placed at index 48-57 (for 0-9), 64 at index 61 (for =)
            -1, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14,
            15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, -1, -1, -1, -1, -1,         //0-25 are placed at index 65-90 (for A-Z)
            -1, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40,
            41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, -1, -1, -1, -1, -1,         //26-51 are placed at index 97-122 (for a-z)
            -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,         // Bytes over 122 ('z') are invalid and cannot be decoded
            -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,         // Hence, padding the map with 255, which indicates invalid input
            -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
            -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
            -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
            -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
            -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
            -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
        };

        public static readonly byte s_encodingPad = (byte)'=';              // '=', for padding

        public static readonly sbyte s_invalidByte = -1;                    // Designating -1 for invalid bytes in the decoding map

        public static void InitalizeBytes(Span<byte> bytes, int seed = 100)
        {
            var rnd = new Random(seed);
            for (int i = 0; i < bytes.Length; i++)
            {
                bytes[i] = (byte)rnd.Next(0, byte.MaxValue + 1);
            }
        }

        public static void InitalizeDecodableBytes(Span<byte> bytes, int seed = 100)
        {
            var rnd = new Random(seed);
            for (int i = 0; i < bytes.Length; i++)
            {
                int index = (byte)rnd.Next(0, s_encodingMap.Length - 1);    // Do not pick '='
                bytes[i] = s_encodingMap[index];
            }
        }
        
        [Fact]
        public static void GenerateEncodingMapAndVerify()
        {
            var data = new byte[64]; // Base64
            for (int i = 0; i < s_characters.Length; i++)
            {
                data[i] = (byte)s_characters[i];
            }
            Assert.True(s_encodingMap.AsSpan().SequenceEqual(data));
        }

        [Fact]
        public static void GenerateDecodingMapAndVerify()
        {
            var data = new sbyte[256]; // 0 to byte.MaxValue (255)
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = s_invalidByte;
            }
            for (int i = 0; i < s_characters.Length; i++)
            {
                data[s_characters[i]] = (sbyte)i;
            }
            Assert.True(s_decodingMap.AsSpan().SequenceEqual(data));
        }

        public static void SplitSourceIntoSpans(Span<byte> source, bool misaligned, out ReadOnlySpan<byte> source1, out ReadOnlySpan<byte> source2)
        {
            int inputBufferSize = source.Length;
            int boundary = inputBufferSize / 5 * 2 + (misaligned ? 2 : 0);  // if inputBufferSize = 1000 -> boundary = 402 or 400
            source1 = source.Slice(0, boundary);
            source2 = source.Slice(boundary, inputBufferSize - boundary);
        }

        public static void DecodeNoNeedToStich(ReadOnlySpan<byte> source1, ReadOnlySpan<byte> source2, Span<byte> destination, out int bytesConsumed, out int bytesWritten)
        {
            bytesConsumed = 0;
            bytesWritten = 0;
            if (Base64.DecodeFromUtf8(source1, destination, out int consumed1, out int written1) == OperationStatus.Done)
            {
                Base64.DecodeFromUtf8(source2, destination.Slice(written1), out int consumed2, out int written2);
                bytesConsumed = consumed2;
                bytesWritten = written2;
            }
            bytesConsumed += consumed1;
            bytesWritten += written1;
        }

        public static void DecodeStichUsingStack(ReadOnlySpan<byte> source1, ReadOnlySpan<byte> source2, Span<byte> destination, Span<byte> stackSpan, out int bytesConsumed, out int bytesWritten)
        {
            bytesConsumed = 0;
            bytesWritten = 0;
            int afterMergeSlice = 0;
            if (Base64.DecodeFromUtf8(source1, destination, out int consumed1, out int written1) != OperationStatus.Done)
            {
                int leftOverBytes = source1.Length - consumed1;
                if (leftOverBytes < 4)
                {
                    source1.Slice(consumed1, leftOverBytes).CopyTo(stackSpan);
                    int amountOfData = leftOverBytes;
                    int amountToCopy = Math.Min(source2.Length, stackSpan.Length - leftOverBytes);

                    source2.Slice(0, amountToCopy).CopyTo(stackSpan.Slice(leftOverBytes));
                    amountOfData += amountToCopy;

                    Base64.DecodeFromUtf8(stackSpan.Slice(0, amountOfData), destination.Slice(written1), out int consumed2, out int written2);
                    bytesConsumed = consumed2;
                    bytesWritten = written2;

                    afterMergeSlice = consumed2 - leftOverBytes;
                }
            }
            bytesConsumed += consumed1;
            bytesWritten += written1;
            if (afterMergeSlice < 0 || afterMergeSlice >= source2.Length)
            {
                return;
            }
            Base64.DecodeFromUtf8(source2.Slice(afterMergeSlice), destination.Slice(bytesWritten), out int consumed3, out int written3);
            bytesConsumed += consumed3;
            bytesWritten += written3;
        }

        public static int[] FindAllIndexOf<T>(this IEnumerable<T> values, T valueToFind)
        {
            return values.Select((element, index) => Equals(element, valueToFind) ? index : -1).Where(index => index != -1).ToArray();
        }
    }
}
