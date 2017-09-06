// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Buffers;
using System.Runtime.CompilerServices;

namespace System.Binary.Base64
{
    public static partial class Base64
    {
        // Pre-computing this table using a custom string(s_characters) and GenerateEncodingMapAndVerify (found in tests)
        static readonly byte[] s_encodingMap = {
            65, 66, 67, 68, 69, 70, 71, 72,         //A..H
            73, 74, 75, 76, 77, 78, 79, 80,         //I..P
            81, 82, 83, 84, 85, 86, 87, 88,         //Q..X
            89, 90, 97, 98, 99, 100, 101, 102,      //Y..Z, a..f
            103, 104, 105, 106, 107, 108, 109, 110, //g..n
            111, 112, 113, 114, 115, 116, 117, 118, //o..v
            119, 120, 121, 122, 48, 49, 50, 51,     //w..z, 0..3
            52, 53, 54, 55, 56, 57, 43, 47          //4..9, +, /
        };

        const byte s_encodingPad = (byte)'=';              // '=', for padding

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ComputeEncodedLength(int sourceLength)
        {
            Diagnostics.Debug.Assert(sourceLength >= 0);
            return ((sourceLength + 2) / 3) << 2;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void Encode(byte b0, byte b1, byte b2, out byte r0, out byte r1, out byte r2, out byte r3)
        {
            int i0 = b0 >> 2;
            r0 = s_encodingMap[i0];

            int i1 = (b0 & 0x3) << 4 | (b1 >> 4);
            r1 = s_encodingMap[i1];

            int i2 = (b1 & 0xF) << 2 | (b2 >> 6);
            r2 = s_encodingMap[i2];

            int i3 = b2 & 0x3F;
            r3 = s_encodingMap[i3];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int Encode(ref byte srcBytes)
        {
            byte b0 = srcBytes;
            byte b1 = Unsafe.Add(ref srcBytes, 1);
            byte b2 = Unsafe.Add(ref srcBytes, 2);

            Encode(b0, b1, b2, out byte r0, out byte r1, out byte r2, out byte r3);

            int result = r3 << 24 | r2 << 16 | r1 << 8 | r0;
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void Encode(byte b0, byte b1, byte b2, out byte r0, out byte r1, out byte r2)
        {
            int i0 = b0 >> 2;
            r0 = s_encodingMap[i0];

            int i1 = (b0 & 0x3) << 4 | (b1 >> 4);
            r1 = s_encodingMap[i1];

            int i2 = (b1 & 0xF) << 2 | (b2 >> 6);
            r2 = s_encodingMap[i2];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int EncodePadByOne(ref byte srcBytes)
        {
            Encode(srcBytes, Unsafe.Add(ref srcBytes, 1), 0, out byte r0, out byte r1, out byte r2);
            int result = s_encodingPad << 24 | r2 << 16 | r1 << 8 | r0;
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void Encode(byte b0, byte b1, out byte r0, out byte r1)
        {
            int i0 = b0 >> 2;
            r0 = s_encodingMap[i0];

            int i1 = (b0 & 0x3) << 4 | (b1 >> 4);
            r1 = s_encodingMap[i1];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int EncodePadByTwo(ref byte srcBytes)
        {
            Encode(srcBytes, 0, out byte r0, out byte r1);
            int result = s_encodingPad << 24 | s_encodingPad << 16 | r1 << 8 | r0;
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        /// <returns>Number of bytes written to the destination.</returns>
        public static OperationStatus Encode(ReadOnlySpan<byte> source, Span<byte> destination, out int bytesConsumed, out int bytesWritten)
        {
            ref byte srcBytes = ref source.DangerousGetPinnableReference();
            ref byte destBytes = ref destination.DangerousGetPinnableReference();

            int srcLength = source.Length;
            int destLength = destination.Length;

            int sourceIndex = 0;
            int destIndex = 0;
            int result = 0;

            while (sourceIndex < srcLength - 2)
            {
                result = Encode(ref Unsafe.Add(ref srcBytes, sourceIndex));
                if (destIndex > destLength - 4) goto DestinationSmallExit;
                Unsafe.WriteUnaligned(ref Unsafe.Add(ref destBytes, destIndex), result);
                destIndex += 4;
                sourceIndex += 3;
            }

            if (sourceIndex == srcLength - 1)
            {
                result = EncodePadByTwo(ref Unsafe.Add(ref srcBytes, sourceIndex));
                if (destIndex > destLength - 4) goto DestinationSmallExit;
                Unsafe.WriteUnaligned(ref Unsafe.Add(ref destBytes, destIndex), result);
                destIndex += 4;
                sourceIndex += 1;
            }
            else if (sourceIndex == srcLength - 2)
            {
                result = EncodePadByOne(ref Unsafe.Add(ref srcBytes, sourceIndex));
                if (destIndex > destLength - 4) goto DestinationSmallExit;
                Unsafe.WriteUnaligned(ref Unsafe.Add(ref destBytes, destIndex), result);
                destIndex += 4;
                sourceIndex += 2;
            }

            bytesConsumed = sourceIndex;
            bytesWritten = destIndex;
            return OperationStatus.Done;

            DestinationSmallExit:
            bytesConsumed = sourceIndex;
            bytesWritten = destIndex;
            return OperationStatus.DestinationTooSmall;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffer">Buffer containing source bytes and empty space for the encoded bytes</param>
        /// <param name="sourceLength">Number of bytes to encode.</param>
        /// <returns>Number of bytes written to the buffer.</returns>
        public static bool EncodeInPlace(Span<byte> buffer, int sourceLength, out int bytesWritten)
        {
            var encodedLength = ComputeEncodedLength(sourceLength);
            if (buffer.Length < encodedLength) goto FalseExit;

            var leftover = sourceLength - sourceLength / 3 * 3; // how many bytes after packs of 3

            var destinationIndex = encodedLength - 4;
            var sourceIndex = sourceLength - leftover;

            // encode last pack to avoid conditional in the main loop
            if (leftover != 0)
            {
                var sourceSlice = buffer.Slice(sourceIndex, leftover);
                var desitnationSlice = buffer.Slice(destinationIndex, 4);
                destinationIndex -= 4;
                Encode(sourceSlice, desitnationSlice, out int consumed, out int written);
            }

            for (int index = sourceIndex - 3; index >= 0; index -= 3)
            {
                var sourceSlice = buffer.Slice(index, 3);
                var desitnationSlice = buffer.Slice(destinationIndex, 4);
                destinationIndex -= 4;
                Encode(sourceSlice, desitnationSlice, out int consumed, out int written);
            }
            
            bytesWritten = encodedLength;
            return true;

            FalseExit:
            bytesWritten = 0;
            return false;
        }

        sealed class ToBase64 : Transformation
        {
            public override OperationStatus Transform(ReadOnlySpan<byte> source, Span<byte> destination, out int bytesConsumed, out int bytesWritten)
                => Encode(source, destination, out bytesConsumed, out bytesWritten);
        }
    }
}
