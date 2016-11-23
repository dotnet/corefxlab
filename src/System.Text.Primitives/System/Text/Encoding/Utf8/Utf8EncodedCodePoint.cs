// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Runtime.InteropServices;

namespace System.Text.Utf8
{
    // TODO: Remove this class, make people use char.IsSurrogate and char.ToUtf32 to convert to code points and use Utf8Encoder to write bytes
    [StructLayout(LayoutKind.Explicit)]
    public struct Utf8EncodedCodePoint
    {
        // TODO: Validate constructors if we decide to keep this class
        public Utf8EncodedCodePoint(char character) : this()
        {
            if (char.IsSurrogate(character))
            {
                throw new ArgumentOutOfRangeException("character", "Surrogate characters are not allowed");
            }

            UnicodeCodePoint codePoint = (UnicodeCodePoint)(uint)character;

            unsafe
            {
                fixed (byte* encodedData = &_byte0)
                {
                    var buffer = new Span<byte>(encodedData, 4);
                    if (!Utf8Encoder.TryEncodeCodePoint(codePoint, buffer, out _length))
                    {
                        // TODO: Change exception type
                        throw new Exception("Internal error: this should never happen as codePoint is within acceptable range and is not surrogate");
                    }
                }
            }
        }

        public Utf8EncodedCodePoint(char highSurrogate, char lowSurrogate) : this()
        {
            UnicodeCodePoint codePoint = (UnicodeCodePoint)(uint)char.ConvertToUtf32(highSurrogate, lowSurrogate);

            unsafe
            {
                fixed (byte* encodedData = &_byte0)
                {
                    var buffer = new Span<byte>(encodedData, 4);
                    if (!Utf8Encoder.TryEncodeCodePoint(codePoint, buffer, out _length))
                    {
                        // TODO: Change exception type
                        throw new Exception("Internal error: this should never happen as codePoint should be within acceptable range");
                    }
                }
            }
        }

        public static unsafe explicit operator Utf8EncodedCodePoint(char character) { return new Utf8EncodedCodePoint(character); }


        // TODO: Make it a property, read the length from the first byte
        [FieldOffset(0)]
        private int _length;

        [FieldOffset(4)]
        private byte _byte0;
        [FieldOffset(5)]
        private byte _byte1;
        [FieldOffset(6)]
        private byte _byte2;
        [FieldOffset(7)]
        private byte _byte3;

        public int Length { get { return _length; } set { _length = value; } }

        public byte Byte0 { get { return _byte0; } set { _byte0 = value; } }
        public byte Byte1 { get { return _byte1; } set { _byte1 = value; } }
        public byte Byte2 { get { return _byte2; } set { _byte2 = value; } }
        public byte Byte3 { get { return _byte3; } set { _byte3 = value; } }
    }
}
