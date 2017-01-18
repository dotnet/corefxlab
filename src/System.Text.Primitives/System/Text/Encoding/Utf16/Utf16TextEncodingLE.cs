// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text.Utf8;

namespace System.Text.Utf16
{
    class Utf16TextEncodingLE : TextEncoder
    {
        #region Encoding Constants

        // TODO: Some of these members are needed only in Utf16LittleEndianEncoder.
        //       Should we add the usage of them to UnicodeCodePoint class and merge this class with it?
        private const uint Utf16HighSurrogateFirstCodePoint = 0xD800;
        private const uint Utf16HighSurrogateLastCodePoint = 0xDFFF;
        private const uint Utf16LowSurrogateFirstCodePoint = 0xDC00;
        private const uint Utf16LowSurrogateLastCodePoint = 0xDFFF;

        private const uint Utf16SurrogateRangeStart = Utf16HighSurrogateFirstCodePoint;
        private const uint Utf16SurrogateRangeEnd = Utf16LowSurrogateLastCodePoint;

        // To get this to compile with dotnet cli, we need to temporarily un-binary the magic values
        private const byte b0000_0111U = 7;
        private const byte b0000_1111U = 15;
        private const byte b0001_1111U = 31;
        private const byte b0011_1111U = 63;
        private const byte b0111_1111U = 127;
        private const byte b1000_0000U = 128;
        private const byte b1100_0000U = 192;
        private const byte b1110_0000U = 224;
        private const byte b1111_0000U = 240;
        private const byte b1111_1000U = 248;

        #endregion Encoding Constants

        #region Decoding implementation

        public override bool TryDecode(ReadOnlySpan<byte> encodedBytes, out string text, out int bytesConsumed)
        {
            var utf16 = encodedBytes.Cast<byte, char>();
            var chars = utf16.ToArray();

            text = new string(chars);
            bytesConsumed = encodedBytes.Length;
            return true;
        }

        public override bool TryDecode(ReadOnlySpan<byte> encodedBytes, Span<byte> utf8, out int bytesConsumed, out int bytesWritten)
        {
            var utf16 = encodedBytes.Cast<byte, char>();
            return Utf8Encoder.TryEncode(utf16, utf8, out bytesConsumed, out bytesWritten);
        }

        public override bool TryDecode(ReadOnlySpan<byte> encodedBytes, Span<char> utf16, out int bytesConsumed, out int charactersWritten)
        {
            // TODO: Other methods validate that the input stream contains valid sequences as they are consumed.
            //       Currently, this is a copy operation with no validation. What is the right thing here?

            var charInput = encodedBytes.Cast<byte, char>();

            if (charInput.Length >= utf16.Length)
            {
                bytesConsumed = 0;
                charactersWritten = 0;
                return false;
            }

            charInput.CopyTo(utf16);
            bytesConsumed = charInput.Length;
            charactersWritten = charInput.Length;
            return true;
        }

        public override bool TryDecode(ReadOnlySpan<byte> encodedBytes, Span<uint> utf32, out int bytesConsumed, out int charactersWritten)
        {
            int consumed;
            var utf16 = encodedBytes.Cast<byte, char>();
            var result = Utf16LittleEndianEncoder.TryDecode(utf16, utf32, out consumed, out charactersWritten);

            bytesConsumed = consumed * sizeof(char);
            return result;
        }

        #endregion Decoding implementation

        #region Encoding implementation

        public override bool TryEncode(ReadOnlySpan<byte> utf8, Span<byte> encodedBytes, out int bytesConsumed, out int bytesWritten)
        {
            int charactersWritten;
            var utf16 = encodedBytes.Cast<byte, char>();
            var result = Utf8Encoder.TryDecode(utf8, utf16, out bytesConsumed, out charactersWritten);

            bytesWritten = charactersWritten * sizeof(char);
            return result;
        }

        public override bool TryEncode(ReadOnlySpan<char> utf16, Span<byte> encodedBytes, out int charactersConsumed, out int bytesWritten)
        {
            // TODO: Other methods validate that the input stream contains valid sequences as they are consumed.
            //       Currently, this is a copy operation with no validation. What is the right thing here?

            var charBuffer = encodedBytes.Cast<byte, char>();

            if (utf16.Length > charBuffer.Length)
            {
                charactersConsumed = 0;
                bytesWritten = 0;
                return false;
            }

            utf16.CopyTo(charBuffer);
            bytesWritten = utf16.Length * sizeof(char);
            charactersConsumed = utf16.Length;
            return true;
        }

        public override bool TryEncode(ReadOnlySpan<uint> utf32, Span<byte> encodedBytes, out int charactersConsumed, out int bytesWritten)
        {
            int written;
            var utf16 = encodedBytes.Cast<byte, char>();
            var result = Utf16LittleEndianEncoder.TryEncode(utf32, utf16, out charactersConsumed, out written);

            bytesWritten = written * sizeof(char);
            return result;
        }

        public override bool TryEncode(string text, Span<byte> encodedBytes, out int bytesWritten)
        {
            bytesWritten = text.Length << 1;
            if (bytesWritten > encodedBytes.Length)
            {
                bytesWritten = 0;
                return false;
            }

            unsafe
            {
                fixed (char* pChars = text)
                {
                    byte* pBytes = (byte*)pChars;
                    new Span<byte>(pBytes, bytesWritten).CopyTo(encodedBytes);
                }
            }

            return true;
        }

        #endregion Encoding implementation
    }
}
