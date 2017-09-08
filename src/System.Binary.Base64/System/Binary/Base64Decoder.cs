// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Buffers;
using System.Runtime.CompilerServices;

namespace System.Binary.Base64
{
    public static partial class Base64
    {
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ComputeDecodedLength(ReadOnlySpan<byte> source)
        {
            int srcLength = source.Length;

            int baseLength = (srcLength >> 2) * 3;

            if ((srcLength & 0x3) != 0) return baseLength;   // Length of source is not a multiple of 4, assume more bytes will follow

            // Only check for padding if source is multiple of 4 and we know we are at the end of the input.
            if (srcLength > 1 && source[srcLength - 2] == s_encodingPad)
            {
                return baseLength - 2;
            }
            else if (srcLength > 0 && source[srcLength - 1] == s_encodingPad)
            {
                return baseLength - 1;
            }
            else
            {
                return baseLength;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int Decode(ref byte srcBytes, ref sbyte decodingMap)
        {
            int i0 = srcBytes;
            int i1 = Unsafe.Add(ref srcBytes, 1);
            int i2 = Unsafe.Add(ref srcBytes, 2);
            int i3 = Unsafe.Add(ref srcBytes, 3);

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
        private static void WriteThreeBytes(ref byte destBytes, int i0)
        {
            destBytes = (byte)(i0 >> 16);
            Unsafe.Add(ref destBytes, 1) = (byte)(i0 >> 8);
            Unsafe.Add(ref destBytes, 2) = (byte)i0;
        }

        public static OperationStatus Decode(ReadOnlySpan<byte> source, Span<byte> destination, out int bytesConsumed, out int bytesWritten)
        {
            ref byte srcBytes = ref source.DangerousGetPinnableReference();
            ref byte destBytes = ref destination.DangerousGetPinnableReference();

            int srcLength = source.Length & ~0x3;  // only decode input up to the closest multiple of 4.
            int destLength = destination.Length;

            int sourceIndex = 0;
            int destIndex = 0;

            if (source.Length == 0) goto DoneExit;

            ref sbyte decodingMap = ref s_decodingMap[0];

            while (sourceIndex < srcLength - 4)
            {
                int result = Decode(ref Unsafe.Add(ref srcBytes, sourceIndex), ref decodingMap);
                if (result < 0) goto InvalidExit;
                if (destIndex > destLength - 3) goto DestinationSmallExit;
                WriteThreeBytes(ref Unsafe.Add(ref destBytes, destIndex), result);
                destIndex += 3;
                sourceIndex += 4;
            }

            if (sourceIndex >= srcLength) goto NeedMoreExit;

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
                WriteThreeBytes(ref Unsafe.Add(ref destBytes, destIndex), i0);
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

            if (srcLength != source.Length) goto NeedMoreExit;

            DoneExit:
            bytesConsumed = sourceIndex;
            bytesWritten = destIndex;
            return OperationStatus.Done;

            DestinationSmallExit:
            bytesConsumed = sourceIndex;
            bytesWritten = destIndex;
            return OperationStatus.DestinationTooSmall;

            NeedMoreExit:
            bytesConsumed = sourceIndex;
            bytesWritten = destIndex;
            return OperationStatus.NeedMoreSourceData;

            InvalidExit:
            bytesConsumed = sourceIndex;
            bytesWritten = destIndex;
            return OperationStatus.InvalidData;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns>Number of bytes written to the buffer.</returns>
        public static OperationStatus DecodeInPlace(Span<byte> buffer, out int bytesConsumed, out int bytesWritten)
        {
            ref byte bufferBytes = ref buffer.DangerousGetPinnableReference();

            int bufferLength = buffer.Length & ~0x3;  // only decode input up to the closest multiple of 4.

            int sourceIndex = 0;
            int destIndex = 0;

            if (buffer.Length == 0) goto DoneExit;

            ref sbyte decodingMap = ref s_decodingMap[0];

            while (sourceIndex < bufferLength - 4)
            {
                int result = Decode(ref Unsafe.Add(ref bufferBytes, sourceIndex), ref decodingMap);
                if (result < 0) goto InvalidExit;
                WriteThreeBytes(ref Unsafe.Add(ref bufferBytes, destIndex), result);
                destIndex += 3;
                sourceIndex += 4;
            }

            if (sourceIndex >= bufferLength) goto NeedMoreExit;

            int i0 = Unsafe.Add(ref bufferBytes, bufferLength - 4);
            int i1 = Unsafe.Add(ref bufferBytes, bufferLength - 3);
            int i2 = Unsafe.Add(ref bufferBytes, bufferLength - 2);
            int i3 = Unsafe.Add(ref bufferBytes, bufferLength - 1);

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
                WriteThreeBytes(ref Unsafe.Add(ref bufferBytes, destIndex), i0);
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

            sourceIndex += 4;

            if (bufferLength != buffer.Length) goto NeedMoreExit;

            DoneExit:
            bytesConsumed = sourceIndex;
            bytesWritten = destIndex;
            return OperationStatus.Done;

            NeedMoreExit:
            bytesConsumed = sourceIndex;
            bytesWritten = destIndex;
            return OperationStatus.NeedMoreSourceData;

            InvalidExit:
            bytesConsumed = sourceIndex;
            bytesWritten = destIndex;
            return OperationStatus.InvalidData;
        }

        sealed class FromBase64 : BufferDecoder
        {
            public override OperationStatus Decode(ReadOnlySpan<byte> source, Span<byte> destination, out int bytesConsumed, out int bytesWritten)
                => Base64.Decode(source, destination, out bytesConsumed, out bytesWritten);
        }
    }
}
