﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Runtime.CompilerServices;

namespace System.Binary.Base64
{
    public static class Base64
    {
        static string s_characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/=";
        static readonly byte[] s_encodingMap = GetEncodingMap(s_characters);
        static readonly byte[] s_decodingMap = GetDecodingMap(s_characters);
        
        private static byte[] GetEncodingMap(string str)
        {
            var data = new byte[str.Length];
            if (!Text.TextEncoder.Utf8.TryEncode(str, data, out int written))
                return null;    // This shouldn't happen...
            return data;
        }

        private static byte[] GetDecodingMap(string str)
        {
            var data = new byte[123]; // 'z' = 123
            for (int i = 0; i < str.Length; i++)
            {
                data[str[i]] = (byte)i;
            }
            return data;
        }

        public static int ComputeEncodedLength(int inputLength)
        {
            int third = inputLength / 3;
            int thirdTimes3 = third * 3;
            if(thirdTimes3 == inputLength) return third * 4;
            return third * 4 + 4;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Encode(byte b0, byte b1, byte b2, out byte r0, out byte r1, out byte r2, out byte r3)
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
            int result = s_encodingMap[64] << 24 | r2 << 16 | r1 << 8 | r0;
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
            int result = s_encodingMap[64] << 24 | s_encodingMap[64] << 16 | r1 << 8 | r0;
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        /// <returns>Number of bytes written to the destination.</returns>
        public static int Encode(ReadOnlySpan<byte> source, Span<byte> destination)
        {
            Diagnostics.Debug.Assert(destination.Length >= ComputeEncodedLength(source.Length));

            ref byte srcBytes = ref source.DangerousGetPinnableReference();
            ref byte destBytes = ref destination.DangerousGetPinnableReference();

            int srcLength = source.Length;

            int sourceIndex = 0;
            int destIndex = 0;
            int result = 0;

            while (sourceIndex < srcLength - 2)
            {
                result = Encode(ref Unsafe.Add(ref srcBytes, sourceIndex));
                Unsafe.WriteUnaligned(ref Unsafe.Add(ref destBytes, destIndex), result);
                sourceIndex += 3;
                destIndex += 4;
            }

            if (sourceIndex == srcLength - 1)
            {
                result = EncodePadByTwo(ref Unsafe.Add(ref srcBytes, sourceIndex));
                Unsafe.WriteUnaligned(ref Unsafe.Add(ref destBytes, destIndex), result);
                destIndex += 4;
            }
            else if (sourceIndex == srcLength - 2)
            {
                result = EncodePadByOne(ref Unsafe.Add(ref srcBytes, sourceIndex));
                Unsafe.WriteUnaligned(ref Unsafe.Add(ref destBytes, destIndex), result);
                destIndex += 4;
            }

            return destIndex;
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
                Encode(sourceSlice, desitnationSlice);
            }

            for (int index = sourceIndex - 3; index>=0; index -= 3) {
                var sourceSlice = buffer.Slice(index, 3);
                var desitnationSlice = buffer.Slice(destinationIndex, 4);
                destinationIndex -= 4;
                Encode(sourceSlice, desitnationSlice);
            }

            return encodedLength;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Decode(byte b0, byte b1, byte b2, byte b3, out byte r0, out byte r1, out byte r2)
        {
            byte i0 = s_decodingMap[b0];
            byte i1 = s_decodingMap[b1];
            byte i2 = s_decodingMap[b2];
            byte i3 = s_decodingMap[b3];

            r0 = (byte)(i0<<2 | i1 >> 4);
            r1 = (byte)(i1<<4 | i2>>2);
            r2 = (byte)(i2<<6 | i3);
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

            if (buffer[buffer.Length - 1] == s_encodingMap[64]) {
                padding = 1;
                if (buffer[buffer.Length - 2] == s_encodingMap[64]) padding = 2;
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        /// <returns>Number of bytes written to the destination.</returns>
        public static int Decode(ReadOnlySpan<byte> source, Span<byte> destination)
        {
            int di = 0;
            int si = 0;
            byte r0, r1, r2;
            int padding = 0;

            if (source[source.Length - 1] == s_encodingMap[64]) {
                padding = 1;
                if (source[source.Length - 2] == s_encodingMap[64]) padding = 2;
            }

            for (; si < source.Length - (padding!=0?4:0);) {
                Decode(source[si++], source[si++], source[si++], source[si++], out r0, out r1, out r2);
                destination[di++] = r0;
                destination[di++] = r1;
                destination[di++] = r2;
            }

            if (padding != 0) {
                Decode(source[si++], source[si++], source[si++], source[si++], out r0, out r1, out r2);
                destination[di++] = r0;

                if (padding == 1) {
                    destination[di++] = r1;
                }
            }

            return di;
        }
    }
}
