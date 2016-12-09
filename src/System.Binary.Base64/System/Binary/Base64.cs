// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text.Utf8;
using System.Runtime.CompilerServices;

namespace System.Binary
{
    public static class Base64
    {
        static string s_characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/=";
        static byte[] s_encodingMap = new Utf8String(s_characters).CopyBytes();
        static byte[] s_decodingMap = new byte[123];

        static Base64()
        {
            for(int i=0; i< s_characters.Length; i++) {
                var nextChar = s_characters[i];
                var indexOfChar = s_characters.IndexOf(nextChar);
                s_decodingMap[nextChar] = (byte)indexOfChar;
            }
        }
        
        public static int ComputeEncodedLength(int inputLength)
        {
            var third = inputLength / 3;
            var thirdTimes3 = third * 3;
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
        private static int Encode(ReadOnlySpan<byte> threeBytes)
        {
            Diagnostics.Debug.Assert(threeBytes.Length > 2);

            byte b0 = threeBytes[0];
            byte b1 = threeBytes[1];
            byte b2 = threeBytes[2];

            int i3 = b2 & 0x3F;
            int result = s_encodingMap[i3];
            result <<= 8;

            int i2 = (b1 & 0xF) << 2 | (b2 >> 6);
            result |=  s_encodingMap[i2];
            result <<= 8;

            int i1 = (b0 & 0x3) << 4 | (b1 >> 4);
            result |= s_encodingMap[i1];
            result <<= 8;

            int i0 = b0 >> 2;
            result |= s_encodingMap[i0];
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
            int di = 0;
            int si = 0;
            byte b0, b1, b2, b3;
            for (; si<source.Length - 2;) {
                var result = Encode(source.Slice(si));
                si += 3;
                destination.Slice(di).Write(result);
                di += 4;
            }

            if (si == source.Length - 1) {
                Encode(source[si], 0, 0, out b0, out b1, out b2, out b3);
                destination[di++] = b0;
                destination[di++] = b1;
                destination[di++] = s_encodingMap[64];
                destination[di++] = s_encodingMap[64];
            }
            else if(si == source.Length - 2) {
                Encode(source[si++], source[si], 0, out b0, out b1, out b2, out b3);
                destination[di++] = b0;
                destination[di++] = b1;
                destination[di++] = b2;
                destination[di++] = s_encodingMap[64];
            }

            return di; 
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