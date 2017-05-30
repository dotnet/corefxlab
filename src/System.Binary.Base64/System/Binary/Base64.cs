// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Runtime.CompilerServices;

namespace System.Binary.Base64
{
    public static class Base64Encoder
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
            52, 53, 54, 55, 56, 57, 43, 47,         //4..9, +, /
            61                                      // =
        };

        // Pre-computing this table using a custom string(s_characters) and GenerateDecodingMapAndVerify (found in tests)
        static readonly byte[] s_decodingMap = {
            255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255,
            255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255,
            255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 62, 255, 255, 255, 63,   //62 is placed at index 43 (for +), 63 at index 47 (for /)
            52, 53, 54, 55, 56, 57, 58, 59, 60, 61, 255, 255, 255, 64, 255, 255,            //52-61 are placed at index 48-57 (for 0-9), 64 at index 61 (for =)
            255, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14,
            15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 255, 255, 255, 255, 255,            //0-25 are placed at index 65-90 (for A-Z)
            255, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40,
            41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 255, 255, 255, 255, 255,            //26-51 are placed at index 97-122 (for a-z)
            255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, // Bytes over 122 ('z') are invalid and cannot be decoded
            255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, // Hence, padding the map with 255, which indicates invalid input
            255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255,
            255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255,
            255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255,
            255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255,
            255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255,
            255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255,
        };

        static readonly byte s_encodingPad = s_encodingMap[64];     // s_encodingMap[64] is '=', for padding

        static readonly byte s_invalidByte = byte.MaxValue;         // Designating 255 for invalid bytes in the decoding map

        #region Encode

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
        public static bool TryEncode(ReadOnlySpan<byte> source, Span<byte> destination, out int bytesConsumed, out int bytesWritten)
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
                if (destIndex > destLength - 4) goto FalseExit;
                Unsafe.WriteUnaligned(ref Unsafe.Add(ref destBytes, destIndex), result);
                destIndex += 4;
                sourceIndex += 3;
            }

            if (sourceIndex == srcLength - 1)
            {
                result = EncodePadByTwo(ref Unsafe.Add(ref srcBytes, sourceIndex));
                if (destIndex > destLength - 4) goto FalseExit;
                Unsafe.WriteUnaligned(ref Unsafe.Add(ref destBytes, destIndex), result);
                destIndex += 4;
                sourceIndex += 1;
            }
            else if (sourceIndex == srcLength - 2)
            {
                result = EncodePadByOne(ref Unsafe.Add(ref srcBytes, sourceIndex));
                if (destIndex > destLength - 4) goto FalseExit;
                Unsafe.WriteUnaligned(ref Unsafe.Add(ref destBytes, destIndex), result);
                destIndex += 4;
                sourceIndex += 2;
            }

            bytesConsumed = sourceIndex;
            bytesWritten = destIndex;
            return true;

            FalseExit:
            bytesConsumed = sourceIndex;
            bytesWritten = destIndex;
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffer">Buffer containing source bytes and empty space for the encoded bytes</param>
        /// <param name="sourceLength">Number of bytes to encode.</param>
        /// <returns>Number of bytes written to the buffer.</returns>
        public static int EncodeInPlace(Span<byte> buffer, int sourceLength)
        {
            var encodedLength = ComputeEncodedLength(sourceLength);
            if (buffer.Length < encodedLength) throw new ArgumentException("buffer too small.");

            var leftover = sourceLength - sourceLength / 3 * 3; // how many bytes after packs of 3

            var destinationIndex = encodedLength - 4;
            var sourceIndex = sourceLength - leftover;

            // encode last pack to avoid conditional in the main loop
            if (leftover != 0) {
                var sourceSlice = buffer.Slice(sourceIndex, leftover);
                var desitnationSlice = buffer.Slice(destinationIndex, 4);
                destinationIndex -= 4;
                TryEncode(sourceSlice, desitnationSlice, out int consumed, out int written);
            }

            for (int index = sourceIndex - 3; index>=0; index -= 3) {
                var sourceSlice = buffer.Slice(index, 3);
                var desitnationSlice = buffer.Slice(destinationIndex, 4);
                destinationIndex -= 4;
                TryEncode(sourceSlice, desitnationSlice, out int consumed, out int written);
            }

            return encodedLength;
        }

        #endregion

        #region Decode

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
        private static bool Decode(byte b0, byte b1, byte b2, byte b3, out byte r0, out byte r1, out byte r2)
        {
            if (b0 == s_encodingPad || b1 == s_encodingPad) goto ErrorExit;

            byte i0 = s_decodingMap[b0];
            byte i1 = s_decodingMap[b1];
            byte i2 = s_decodingMap[b2];
            byte i3 = s_decodingMap[b3];
            
            if (i0 == s_invalidByte || i1 == s_invalidByte || i2 == s_invalidByte || i3 == s_invalidByte) goto ErrorExit;

            r0 = (byte)(i0 << 2 | i1 >> 4);
            r1 = (byte)(i1 << 4 | i2 >> 2);
            r2 = (byte)(i2 << 6 | i3);
            return true;

            ErrorExit:
            r0 = r1 = r2 = 0;
            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool DecodeNoPad(byte b0, byte b1, byte b2, byte b3, out byte r0, out byte r1, out byte r2)
        {
            if (b0 == s_encodingPad || b1 == s_encodingPad || b2 == s_encodingPad || b3 == s_encodingPad) goto ErrorExit;

            byte i0 = s_decodingMap[b0];
            byte i1 = s_decodingMap[b1];
            byte i2 = s_decodingMap[b2];
            byte i3 = s_decodingMap[b3];

            if (i0 == s_invalidByte || i1 == s_invalidByte || i2 == s_invalidByte || i3 == s_invalidByte) goto ErrorExit;

            r0 = (byte)(i0 << 2 | i1 >> 4);
            r1 = (byte)(i1 << 4 | i2 >> 2);
            r2 = (byte)(i2 << 6 | i3);
            return true;

            ErrorExit:
            r0 = r1 = r2 = 0;
            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int Decode(ref byte srcBytes)
        {
            byte b0 = srcBytes;
            byte b1 = Unsafe.Add(ref srcBytes, 1);
            byte b2 = Unsafe.Add(ref srcBytes, 2);
            byte b3 = Unsafe.Add(ref srcBytes, 3);

            if (!DecodeNoPad(b0, b1, b2, b3, out byte r0, out byte r1, out byte r2)) return -1;

            int result = r2 << 16 | r1 << 8 | r0;
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool Decode(byte b0, byte b1, byte b2, out byte r0, out byte r1)
        {
            if (b0 == s_encodingPad || b1 == s_encodingPad) goto ErrorExit;

            byte i0 = s_decodingMap[b0];
            byte i1 = s_decodingMap[b1];
            byte i2 = s_decodingMap[b2];

            if (i0 == s_invalidByte || i1 == s_invalidByte || i2 == s_invalidByte) goto ErrorExit;

            r0 = (byte)(i0 << 2 | i1 >> 4);
            r1 = (byte)(i1 << 4 | i2 >> 2);
            return true;

            ErrorExit:
            r0 = r1 = 0;
            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int DecodePadByOne(ref byte srcBytes)
        {
            byte b0 = srcBytes;
            byte b1 = Unsafe.Add(ref srcBytes, 1);
            byte b2 = Unsafe.Add(ref srcBytes, 2);

            if (!Decode(b0, b1, b2, out byte r0, out byte r1)) return -1;

            int result = r1 << 8 | r0;
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool Decode(byte b0, byte b1, out byte r0)
        {
            if (b0 == s_encodingPad || b1 == s_encodingPad) goto ErrorExit;

            byte i0 = s_decodingMap[b0];
            byte i1 = s_decodingMap[b1];

            if (i0 == s_invalidByte || i1 == s_invalidByte) goto ErrorExit;

            r0 = (byte)(i0 << 2 | i1 >> 4);
            return true;

            ErrorExit:
            r0 = 0;
            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int DecodePadByTwo(ref byte srcBytes)
        {
            byte b0 = srcBytes;
            byte b1 = Unsafe.Add(ref srcBytes, 1);

            if (!Decode(b0, b1, out byte r0)) return -1;

            int result = r0;
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        /// <returns>Number of bytes written to the destination.</returns>
        public static bool TryDecode(ReadOnlySpan<byte> source, Span<byte> destination, out int bytesConsumed, out int bytesWritten)
        {
            ref byte srcBytes = ref source.DangerousGetPinnableReference();
            ref byte destBytes = ref destination.DangerousGetPinnableReference();

            int srcLength = source.Length / 4 * 4;  // only decode input up to the closest multiple of 4.
            int destLength = destination.Length;

            int sourceIndex = 0;
            int destIndex = 0;

            if (source.Length == 0) goto TrueExit;

            int result = 0;

            while (sourceIndex < srcLength - 4)
            {
                result = Decode(ref Unsafe.Add(ref srcBytes, sourceIndex));
                if (result == -1) throw new FormatException();  // invalid bytes
                if (destIndex > destLength - 3) goto FalseExit;
                Unsafe.WriteUnaligned(ref Unsafe.Add(ref destBytes, destIndex), result);
                destIndex += 3;
                sourceIndex += 4;
            }

            if (sourceIndex >= srcLength) goto FalseExit;

            int padding = 0;

            if (Unsafe.Add(ref srcBytes, srcLength - 1) == s_encodingPad)
            {
                padding = (Unsafe.Add(ref srcBytes, srcLength - 2) == s_encodingPad) ? 2 : 1;
            }

            if (padding == 1)
            {
                result = DecodePadByOne(ref Unsafe.Add(ref srcBytes, sourceIndex));
                if (result == -1) throw new FormatException();  // invalid bytes
                if (destIndex > destLength - 2) goto FalseExit;
                Unsafe.WriteUnaligned(ref Unsafe.Add(ref destBytes, destIndex), result);
                destIndex += 2;
                sourceIndex += 4;
            }
            else if (padding == 2)
            {
                result = DecodePadByTwo(ref Unsafe.Add(ref srcBytes, sourceIndex));
                if (result == -1) throw new FormatException();  // invalid bytes
                if (destIndex > destLength - 1) goto FalseExit;
                Unsafe.WriteUnaligned(ref Unsafe.Add(ref destBytes, destIndex), result);
                destIndex += 1;
                sourceIndex += 4;
            }
            else
            {
                result = Decode(ref Unsafe.Add(ref srcBytes, sourceIndex));
                if (result == -1) throw new FormatException();  // invalid bytes
                if (destIndex > destLength - 3) goto FalseExit;
                Unsafe.WriteUnaligned(ref Unsafe.Add(ref destBytes, destIndex), result);
                destIndex += 3;
                sourceIndex += 4;
            }

            if (srcLength != source.Length) goto FalseExit;

            TrueExit:
            bytesConsumed = sourceIndex;
            bytesWritten = destIndex;
            return true;

            FalseExit:
            bytesConsumed = sourceIndex;
            bytesWritten = destIndex;
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns>Number of bytes written to the buffer.</returns>
        public static int DecodeInPlace(Span<byte> buffer)
        {
            int di = 0;
            int si = 0;
            byte r0, r1, r2;
            int padding = 0;

            if (buffer[buffer.Length - 1] == s_encodingPad) {
                padding = 1;
                if (buffer[buffer.Length - 2] == s_encodingPad) padding = 2;
            }

            for (; si < buffer.Length - (padding!=0?4:0);) {
                Decode(buffer[si++], buffer[si++], buffer[si++], buffer[si++], out r0, out r1, out r2);
                buffer[di++] = r0;
                buffer[di++] = r1;
                buffer[di++] = r2;
            }

            if (padding != 0) {
                Decode(buffer[si++], buffer[si++], buffer[si++], buffer[si++], out r0, out r1, out r2);
                buffer[di++] = r0;

                if (padding == 1) {
                    buffer[di++] = r1;
                }
            }

            return di;
        }

        #endregion

        public enum ReturnState
        {
            SUCCESS, OUTPUT_TOO_SMALL, NEED_MORE_INPUT, INPUT_INVALID
        }
    }
}
