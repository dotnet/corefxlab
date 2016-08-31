// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text.Utf8;

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

        public static void Encode(byte b0, byte b1, byte b2, out byte r0, out byte r1, out byte r2, out byte r3)
        {
            int i0 = b0 >> 2;
            r0 = s_encodingMap[i0];

            int i1 = (b0 & 3) << 4 | (b1 >> 4);
            r1 = s_encodingMap[i1];

            int i2 = (b1 & 15) << 2 | (b2 >> 6);
            r2 = s_encodingMap[i2];

            int i3 = b2 & 63;
            r3 = s_encodingMap[i3];
        }

        public static void Encode(ReadOnlySpan<byte> source, Span<byte> destination)
        {
            int di = 0;
            int si = 0;
            byte b0, b1, b2, b3;
            for (; si<source.Length - 2;) {
                Encode(source[si++], source[si++], source[si++], out b0, out b1, out b2, out b3);
                destination[di++] = b0;
                destination[di++] = b1;
                destination[di++] = b2;
                destination[di++] = b3;
            }

            if (si == source.Length) { return; }

            if (si == source.Length - 1) {
                Encode(source[si], 0, 0, out b0, out b1, out b2, out b3);
                destination[di++] = b0;
                destination[di++] = b1;
                destination[di++] = s_encodingMap[64];
                destination[di++] = s_encodingMap[64];
                return;
            }
            if(si == source.Length - 2) {
                Encode(source[si++], source[si], 0, out b0, out b1, out b2, out b3);
                destination[di++] = b0;
                destination[di++] = b1;
                destination[di++] = b2;
                destination[di++] = s_encodingMap[64];
                return;
            }

            throw new NotImplementedException();
        }

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

        public static void Decode(ReadOnlySpan<byte> source, Span<byte> destination)
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
        }
    }
}