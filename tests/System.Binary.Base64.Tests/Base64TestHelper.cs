// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System.Buffers;
using System.Buffers.Text;

namespace System.Binary.Base64Experimental.Tests
{
    public static class Base64TestHelper
    {
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
    }
}
