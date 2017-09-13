// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace System.Binary.Base64
{
    public static partial class Base64
    {
        public static readonly BufferEncoder BytesToUtf8Encoder = new ToBase64Utf8();

        public static OperationStatus BytesToUtf8(ReadOnlySpan<byte> bytes, Span<byte> utf8, out int consumed, out int written)
        {
            ref byte srcBytes = ref bytes.DangerousGetPinnableReference();
            ref byte destBytes = ref utf8.DangerousGetPinnableReference();

            int srcLength = bytes.Length;
            int destLength = utf8.Length;

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
                result = EncodeAndPadTwo(ref Unsafe.Add(ref srcBytes, sourceIndex));
                if (destIndex > destLength - 4) goto DestinationSmallExit;
                Unsafe.WriteUnaligned(ref Unsafe.Add(ref destBytes, destIndex), result);
                destIndex += 4;
                sourceIndex += 1;
            }
            else if (sourceIndex == srcLength - 2)
            {
                result = EncodeAndPadOne(ref Unsafe.Add(ref srcBytes, sourceIndex));
                if (destIndex > destLength - 4) goto DestinationSmallExit;
                Unsafe.WriteUnaligned(ref Unsafe.Add(ref destBytes, destIndex), result);
                destIndex += 4;
                sourceIndex += 2;
            }

            consumed = sourceIndex;
            written = destIndex;
            return OperationStatus.Done;

            DestinationSmallExit:
            consumed = sourceIndex;
            written = destIndex;
            return OperationStatus.DestinationTooSmall;
        }

        public static int BytesToUtf8Length(int bytesLength)
        {
            Debug.Assert(bytesLength >= 0);
            return ((bytesLength + 2) / 3) << 2;
        }

        public static OperationStatus BytesToUtf8InPlace(Span<byte> buffer, int bytesLength, out int written)
        {
            var encodedLength = BytesToUtf8Length(bytesLength);
            if (buffer.Length < encodedLength) goto FalseExit;

            var leftover = bytesLength - bytesLength / 3 * 3; // how many bytes after packs of 3

            var destinationIndex = encodedLength - 4;
            var sourceIndex = bytesLength - leftover;

            // encode last pack to avoid conditional in the main loop
            if (leftover != 0)
            {
                var sourceSlice = buffer.Slice(sourceIndex, leftover);
                var desitnationSlice = buffer.Slice(destinationIndex, 4);
                destinationIndex -= 4;
                var result = BytesToUtf8(sourceSlice, desitnationSlice, out int consumed, out int tempWritten);
                Debug.Assert(result == OperationStatus.Done);
            }

            for (int index = sourceIndex - 3; index >= 0; index -= 3)
            {
                var sourceSlice = buffer.Slice(index, 3);
                var desitnationSlice = buffer.Slice(destinationIndex, 4);
                destinationIndex -= 4;
                var result = BytesToUtf8(sourceSlice, desitnationSlice, out int consumed, out int tempWritten);
                Debug.Assert(result == OperationStatus.Done);
            }

            written = encodedLength;
            return OperationStatus.Done;

            FalseExit:
            written = 0;
            return OperationStatus.DestinationTooSmall;
        }

        sealed class ToBase64Utf8 : BufferEncoder
        {
            public override OperationStatus Encode(ReadOnlySpan<byte> source, Span<byte> destination, out int bytesConsumed, out int bytesWritten)
                => Base64.Encode(source, destination, out bytesConsumed, out bytesWritten);

            public override OperationStatus EncodeInPlace(Span<byte> buffer, int inputLength, out int written)
                => Base64.EncodeInPlace(buffer, inputLength, out written) ? OperationStatus.Done : OperationStatus.DestinationTooSmall;

            public override bool IsEncodeInPlaceSupported => true;
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
        private static void Encode(byte b0, byte b1, out byte r0, out byte r1)
        {
            int i0 = b0 >> 2;
            r0 = s_encodingMap[i0];

            int i1 = (b0 & 0x3) << 4 | (b1 >> 4);
            r1 = s_encodingMap[i1];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int Encode(ref byte threeBytes)
        {
            byte b0 = threeBytes;
            byte b1 = Unsafe.Add(ref threeBytes, 1);
            byte b2 = Unsafe.Add(ref threeBytes, 2);

            Encode(b0, b1, b2, out byte r0, out byte r1, out byte r2, out byte r3);

            int result = r3 << 24 | r2 << 16 | r1 << 8 | r0;
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int EncodeAndPadOne(ref byte twoBytes)
        {
            Encode(twoBytes, Unsafe.Add(ref twoBytes, 1), 0, out byte r0, out byte r1, out byte r2);
            int result = s_encodingPad << 24 | r2 << 16 | r1 << 8 | r0;
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int EncodeAndPadTwo(ref byte oneByte)
        {
            Encode(oneByte, 0, out byte r0, out byte r1);
            int result = s_encodingPad << 24 | s_encodingPad << 16 | r1 << 8 | r0;
            return result;
        }

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

        const byte s_encodingPad = (byte)'='; // '=', for padding

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [Obsolete("Use Base64.BytesToUtf8Length")]
        public static int ComputeEncodedUtf8Length(int sourceLength) => BytesToUtf8Length(sourceLength);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        /// <returns>Number of bytes written to the destination.</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("Use Base64.BytesToUtf8InPlace")]
        public static OperationStatus Encode(ReadOnlySpan<byte> source, Span<byte> destination, out int bytesConsumed, out int bytesWritten)
            => BytesToUtf8(source, destination, out bytesConsumed, out bytesWritten);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffer">Memory containing source bytes and empty space for the encoded bytes</param>
        /// <param name="sourceLength">Number of bytes to encode.</param>
        /// <returns>Number of bytes written to the buffer.</returns>
        [Obsolete("Use Base64.BytesToUtf8InPlace")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static bool EncodeInPlace(Span<byte> buffer, int sourceLength, out int bytesWritten)
            => BytesToUtf8InPlace(buffer, sourceLength, out bytesWritten) == OperationStatus.Done;
    }
}
