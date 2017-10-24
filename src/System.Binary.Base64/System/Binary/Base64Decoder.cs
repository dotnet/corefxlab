// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace System.Binary.Base64
{
    public static partial class Base64
    {
        public static readonly Utf8Decoder Utf8ToBytesDecoder = new Utf8Decoder();

        public static OperationStatus DecodeFromUtf8(ReadOnlySpan<byte> utf8, Span<byte> bytes, out int consumed, out int written, bool isFinalBlock = true)
        {
            ref byte srcBytes = ref utf8.DangerousGetPinnableReference();
            ref byte destBytes = ref bytes.DangerousGetPinnableReference();

            int srcLength = utf8.Length & ~0x3;  // only decode input up to the closest multiple of 4.
            int destLength = bytes.Length;

            int sourceIndex = 0;
            int destIndex = 0;

            if (utf8.Length == 0) goto DoneExit;

            ref sbyte decodingMap = ref s_decodingMap[0];

            int skipLastChunk = isFinalBlock ? 4 : 0;

            while (sourceIndex < srcLength - skipLastChunk)
            {
                int result = Decode(ref Unsafe.Add(ref srcBytes, sourceIndex), ref decodingMap);
                if (result < 0) goto InvalidExit;
                if (destIndex > destLength - 3) goto DestinationSmallExit;
                WriteThreeLowOrderBytes(ref Unsafe.Add(ref destBytes, destIndex), result);
                destIndex += 3;
                sourceIndex += 4;
            }

            if (sourceIndex >= srcLength)
            {
                if (isFinalBlock) goto InvalidExit;
                goto NeedMoreExit;
            }

            int i0 = Unsafe.Add(ref srcBytes, srcLength - 4);
            int i1 = Unsafe.Add(ref srcBytes, srcLength - 3);
            int i2 = Unsafe.Add(ref srcBytes, srcLength - 2);
            int i3 = Unsafe.Add(ref srcBytes, srcLength - 1);

            i0 = Unsafe.Add(ref decodingMap, i0);
            i1 = Unsafe.Add(ref decodingMap, i1);

            i0 <<= 18;
            i1 <<= 12;

            i0 |= i1;

            if (i3 != s_encodingPad)
            {
                i2 = Unsafe.Add(ref decodingMap, i2);
                i3 = Unsafe.Add(ref decodingMap, i3);

                i2 <<= 6;

                i0 |= i3;
                i0 |= i2;

                if (i0 < 0) goto InvalidExit;
                if (destIndex > destLength - 3) goto DestinationSmallExit;
                WriteThreeLowOrderBytes(ref Unsafe.Add(ref destBytes, destIndex), i0);
                destIndex += 3;
            }
            else if (i2 != s_encodingPad)
            {
                i2 = Unsafe.Add(ref decodingMap, i2);

                i2 <<= 6;

                i0 |= i2;

                if (i0 < 0) goto InvalidExit;
                if (destIndex > destLength - 2) goto DestinationSmallExit;
                Unsafe.Add(ref destBytes, destIndex) = (byte)(i0 >> 16);
                Unsafe.Add(ref destBytes, destIndex + 1) = (byte)(i0 >> 8);
                destIndex += 2;
            }
            else
            {
                if (i0 < 0) goto InvalidExit;
                if (destIndex > destLength - 1) goto DestinationSmallExit;
                Unsafe.Add(ref destBytes, destIndex) = (byte)(i0 >> 16);
                destIndex += 1;
            }

            sourceIndex += 4;

            if (srcLength != utf8.Length)
            {
                if (isFinalBlock) goto InvalidExit;
                goto NeedMoreExit;
            }

            DoneExit:
            consumed = sourceIndex;
            written = destIndex;
            return OperationStatus.Done;

            DestinationSmallExit:
            consumed = sourceIndex;
            written = destIndex;
            return OperationStatus.DestinationTooSmall;

            NeedMoreExit:
            consumed = sourceIndex;
            written = destIndex;
            return OperationStatus.NeedMoreData;

            InvalidExit:
            consumed = sourceIndex;
            written = destIndex;
            return OperationStatus.InvalidData;
        }

        public static OperationStatus DecodeFromUtf8InPlace(Span<byte> buffer, int dataLength, out int written)
        {
            buffer = buffer.Slice(0, dataLength);

            int sourceIndex = 0;
            int destIndex = 0;

            // only decode input if it is a multiple of 4
            if (buffer.Length % 4 != 0) goto InvalidExit;
            if (buffer.Length == 0) goto DoneExit;

            ref byte bufferBytes = ref buffer.DangerousGetPinnableReference();

            ref sbyte decodingMap = ref s_decodingMap[0];

            while (sourceIndex < buffer.Length - 4)
            {
                int result = Decode(ref Unsafe.Add(ref bufferBytes, sourceIndex), ref decodingMap);
                if (result < 0) goto InvalidExit;
                WriteThreeLowOrderBytes(ref Unsafe.Add(ref bufferBytes, destIndex), result);
                destIndex += 3;
                sourceIndex += 4;
            }

            int i0 = Unsafe.Add(ref bufferBytes, buffer.Length - 4);
            int i1 = Unsafe.Add(ref bufferBytes, buffer.Length - 3);
            int i2 = Unsafe.Add(ref bufferBytes, buffer.Length - 2);
            int i3 = Unsafe.Add(ref bufferBytes, buffer.Length - 1);

            i0 = Unsafe.Add(ref decodingMap, i0);
            i1 = Unsafe.Add(ref decodingMap, i1);

            i0 <<= 18;
            i1 <<= 12;

            i0 |= i1;

            if (i3 != s_encodingPad)
            {
                i2 = Unsafe.Add(ref decodingMap, i2);
                i3 = Unsafe.Add(ref decodingMap, i3);

                i2 <<= 6;

                i0 |= i3;
                i0 |= i2;

                if (i0 < 0) goto InvalidExit;
                WriteThreeLowOrderBytes(ref Unsafe.Add(ref bufferBytes, destIndex), i0);
                destIndex += 3;
            }
            else if (i2 != s_encodingPad)
            {
                i2 = Unsafe.Add(ref decodingMap, i2);

                i2 <<= 6;

                i0 |= i2;

                if (i0 < 0) goto InvalidExit;
                Unsafe.Add(ref bufferBytes, destIndex) = (byte)(i0 >> 16);
                Unsafe.Add(ref bufferBytes, destIndex + 1) = (byte)(i0 >> 8);
                destIndex += 2;
            }
            else
            {
                if (i0 < 0) goto InvalidExit;
                Unsafe.Add(ref bufferBytes, destIndex) = (byte)(i0 >> 16);
                destIndex += 1;
            }

            DoneExit:
            written = destIndex;
            return OperationStatus.Done;

            InvalidExit:
            written = 0;
            return OperationStatus.InvalidData;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetMaxDecodedFromUtf8Length(int length)
        {
            Debug.Assert(length >= 0);
            return (length >> 2) * 3;
        }

        public sealed class Utf8Decoder : IBufferOperation, IBufferTransformation
        {
            public OperationStatus Decode(ReadOnlySpan<byte> source, Span<byte> destination, out int bytesConsumed, out int bytesWritten/*, bool isFinalBlock*/)
                => DecodeFromUtf8(source, destination, out bytesConsumed, out bytesWritten);

            public OperationStatus DecodeInPlace(Span<byte> buffer, int dataLength, out int written)
                => DecodeFromUtf8InPlace(buffer, dataLength, out written);

            OperationStatus IBufferOperation.Execute(ReadOnlySpan<byte> input, Span<byte> output, out int consumed, out int written/*, bool isFinalBlock*/)
                => Decode(input, output, out consumed, out written);

            OperationStatus IBufferTransformation.Transform(Span<byte> buffer, int dataLength, out int written)
                => DecodeInPlace(buffer, dataLength, out written);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int Decode(ref byte encodedBytes, ref sbyte decodingMap)
        {
            int i0 = encodedBytes;
            int i1 = Unsafe.Add(ref encodedBytes, 1);
            int i2 = Unsafe.Add(ref encodedBytes, 2);
            int i3 = Unsafe.Add(ref encodedBytes, 3);

            i0 = Unsafe.Add(ref decodingMap, i0);
            i1 = Unsafe.Add(ref decodingMap, i1);
            i2 = Unsafe.Add(ref decodingMap, i2);
            i3 = Unsafe.Add(ref decodingMap, i3);

            i0 <<= 18;
            i1 <<= 12;
            i2 <<= 6;

            i0 |= i3;
            i1 |= i2;

            i0 |= i1;
            return i0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void WriteThreeLowOrderBytes(ref byte destination, int value)
        {
            destination = (byte)(value >> 16);
            Unsafe.Add(ref destination, 1) = (byte)(value >> 8);
            Unsafe.Add(ref destination, 2) = (byte)value;
        }

        // Pre-computing this table using a custom string(s_characters) and GenerateDecodingMapAndVerify (found in tests)
        static readonly sbyte[] s_decodingMap = {
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
    }
}
