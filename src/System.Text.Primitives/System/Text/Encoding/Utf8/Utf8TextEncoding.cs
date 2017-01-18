// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace System.Text.Utf8
{
    class Utf8TextEncoding : TextEncoder
    {
        #region Decoding implementation

        public override bool TryDecode(ReadOnlySpan<byte> encodedBytes, out string text, out int bytesConsumed)
        {
            int len;
            if (!Utf8Encoder.TryComputeStringLength(encodedBytes, out len))
            {
                text = string.Empty;
                bytesConsumed = 0;
                return false;
            }

            text = new string(' ', len);

            unsafe
            {
                fixed (char* p = text)
                {
                    var charSpan = new Span<char>(p, len);

                    int written;
                    return Utf8Encoder.TryDecode(encodedBytes, charSpan, out bytesConsumed, out written);
                }
            }
        }

        public override bool TryDecode(ReadOnlySpan<byte> encodedBytes, Span<byte> utf8, out int bytesConsumed, out int bytesWritten)
        {
            // TODO: Other methods validate that the input stream contains valid sequences as they are consumed.
            //       Currently, this is a copy operation with no validation. What is the right thing here?

            if (encodedBytes.Length > utf8.Length)
                encodedBytes = encodedBytes.Slice(0, utf8.Length);

            encodedBytes.CopyTo(utf8);
            bytesWritten = encodedBytes.Length;
            bytesConsumed = encodedBytes.Length;
            return true;
        }

        public override bool TryDecode(ReadOnlySpan<byte> encodedBytes, Span<char> utf16, out int bytesConsumed, out int charactersWritten)
        {
            return Utf8Encoder.TryDecode(encodedBytes, utf16, out bytesConsumed, out charactersWritten);
        }

        public override bool TryDecode(ReadOnlySpan<byte> encodedBytes, Span<uint> utf32, out int bytesConsumed, out int charactersWritten)
        {
            return Utf8Encoder.TryDecode(encodedBytes, utf32, out bytesConsumed, out charactersWritten);
        }

        #endregion Decoding implementation

        #region Encoding implementation

        public override bool TryEncode(ReadOnlySpan<byte> utf8, Span<byte> data, out int bytesConsumed, out int bytesWritten)
        {
            // TODO: Other methods validate that the input stream contains valid sequences as they are consumed.
            //       Currently, this is a copy operation with no validation. What is the right thing here?

            if (utf8.Length > data.Length)
            {
                bytesConsumed = 0;
                bytesWritten = 0;
                return false;
            }

            utf8.CopyTo(data);
            bytesWritten = utf8.Length;
            bytesConsumed = utf8.Length;
            return true;
        }

        public override bool TryEncode(ReadOnlySpan<char> utf16, Span<byte> data, out int charactersConsumed, out int bytesWritten)
        {
            return Utf8Encoder.TryEncode(utf16, data, out charactersConsumed, out bytesWritten);
        }

        public override bool TryEncode(ReadOnlySpan<uint> utf32, Span<byte> data, out int charactersConsumed, out int bytesWritten)
        {
            return Utf8Encoder.TryEncode(utf32, data, out charactersConsumed, out bytesWritten);
        }

        public override bool TryEncode(string text, Span<byte> data, out int bytesWritten)
        {
            int consumed;
            var utf16 = text.Slice();

            return Utf8Encoder.TryEncode(utf16, data, out consumed, out bytesWritten);
        }

        #endregion Encoding implementation
    }
}
