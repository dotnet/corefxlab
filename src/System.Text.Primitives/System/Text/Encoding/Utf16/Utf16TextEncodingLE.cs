// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text.Utf8;

namespace System.Text.Utf16
{
    class Utf16TextEncodingLE : TextEncoder
    {
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

        public override bool TryDecode(ReadOnlySpan<byte> encodedBytes, Span<uint> utf32, out int bytesConsumed, out int charactersWritten)
        {
            int consumed;
            var utf16 = encodedBytes.NonPortableCast<byte, char>();
            var result = Utf16LittleEndianEncoder.TryDecode(utf16, utf32, out consumed, out charactersWritten);

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

        public override bool TryEncode(ReadOnlySpan<uint> utf32, Span<byte> encodedBytes, out int charactersConsumed, out int bytesWritten)
        {
            int written;
            var utf16 = encodedBytes.NonPortableCast<byte, char>();
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

            text.Slice().AsBytes().CopyTo(encodedBytes);
            return true;
        }

        #endregion Encoding implementation
    }
}
