// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text.Utf8;

namespace System.Text.Utf16
{
    class Utf16TextEncoderLE : TextEncoder
    {
        #region Built-in Invariant Symbol Table

        private static readonly byte[][] Utf16DigitsAndSymbols = new byte[][]
        {
            new byte[] { 48, 0, }, // digit 0
            new byte[] { 49, 0, },
            new byte[] { 50, 0, },
            new byte[] { 51, 0, },
            new byte[] { 52, 0, },
            new byte[] { 53, 0, },
            new byte[] { 54, 0, },
            new byte[] { 55, 0, },
            new byte[] { 56, 0, },
            new byte[] { 57, 0, }, // digit 9
            new byte[] { 46, 0, }, // decimal separator
            new byte[] { 44, 0, }, // group separator
            new byte[] { 73, 0, 110, 0, 102, 0, 105, 0, 110, 0, 105, 0, 116, 0, 121, 0, }, // Infinity
            new byte[] { 45, 0, }, // minus sign 
            new byte[] { 43, 0, }, // plus sign 
            new byte[] { 78, 0, 97, 0, 78, 0, }, // NaN
            new byte[] { 69, 0, }, // E
            new byte[] { 101, 0, }, // e
        };

        #endregion Built-in Invariant Symbol Table

        #region Constructors

        public Utf16TextEncoderLE()
            : this(Utf16DigitsAndSymbols)
        {
        }

        public Utf16TextEncoderLE(byte[][] symbols)
            : base(symbols, EncodingName.Utf16)
        {
        }

        #endregion Constructors

        #region Decoding implementation

        public override bool TryDecode(ReadOnlySpan<byte> encodedBytes, out string text, out int bytesConsumed)
        {
            var utf16 = encodedBytes.NonPortableCast<byte, char>();
            var chars = utf16.ToArray();

            text = new string(chars);
            bytesConsumed = encodedBytes.Length;
            return true;
        }

        public override bool TryDecode(ReadOnlySpan<byte> encodedBytes, Span<byte> utf8, out int bytesConsumed, out int bytesWritten)
        {
            var utf16 = encodedBytes.NonPortableCast<byte, char>();
            return Utf8Encoder.TryEncode(utf16, utf8, out bytesConsumed, out bytesWritten);
        }

        public override bool TryDecode(ReadOnlySpan<byte> encodedBytes, Span<char> utf16, out int bytesConsumed, out int charactersWritten)
        {
            // TODO: Other methods validate that the input stream contains valid sequences as they are consumed.
            //       Currently, this is a copy operation with no validation. What is the right thing here?

            var charInput = encodedBytes.NonPortableCast<byte, char>();

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

        public override bool TryDecode(ReadOnlySpan<byte> encodedBytes, Span<uint> utf32, out int bytesConsumed, out int codeUnitsWritten)
        {
            int consumed;
            var utf16 = encodedBytes.NonPortableCast<byte, char>();
            var result = Utf16LittleEndianEncoder.TryDecode(utf16, utf32, out consumed, out codeUnitsWritten);

            bytesConsumed = consumed * sizeof(char);
            return result;
        }

        #endregion Decoding implementation

        #region Encoding implementation

        public override bool TryEncode(ReadOnlySpan<byte> utf8, Span<byte> encodedBytes, out int bytesConsumed, out int bytesWritten)
        {
            int charactersWritten;
            var utf16 = encodedBytes.NonPortableCast<byte, char>();
            var result = Utf8Encoder.TryDecode(utf8, utf16, out bytesConsumed, out charactersWritten);

            bytesWritten = charactersWritten * sizeof(char);
            return result;
        }

        public override bool TryEncode(ReadOnlySpan<char> utf16, Span<byte> encodedBytes, out int charactersConsumed, out int bytesWritten)
        {
            // TODO: Other methods validate that the input stream contains valid sequences as they are consumed.
            //       Currently, this is a copy operation with no validation. What is the right thing here?

            var charBuffer = encodedBytes.NonPortableCast<byte, char>();

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

        public override bool TryEncode(ReadOnlySpan<uint> utf32, Span<byte> encodedBytes, out int codeUnitsConsumed, out int bytesWritten)
        {
            int written;
            var utf16 = encodedBytes.NonPortableCast<byte, char>();
            var result = Utf16LittleEndianEncoder.TryEncode(utf32, utf16, out codeUnitsConsumed, out written);

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

            text.AsSpan().AsBytes().CopyTo(encodedBytes);
            return true;
        }

        public override bool TryComputeEncodedBytes(ReadOnlySpan<byte> utf8, out int bytesNeeded)
        {
            var result = Utf8Encoder.TryComputeStringLength(utf8, out int characters);
            bytesNeeded = characters * sizeof(char);
            return result;
        }

        public override bool TryComputeEncodedBytes(ReadOnlySpan<char> utf16, out int bytesNeeded)
        {
            bytesNeeded = utf16.Length * sizeof(char);
            return true;
        }

        public override bool TryComputeEncodedBytes(string text, out int bytesNeeded)
        {
            bytesNeeded = text.Length * sizeof(char);
            return true;
        }

        public override bool TryComputeEncodedBytes(ReadOnlySpan<uint> utf32, out int bytesNeeded)
            => Utf16LittleEndianEncoder.TryComputeEncodedBytes(utf32, out bytesNeeded);

        #endregion Encoding implementation
    }
}
