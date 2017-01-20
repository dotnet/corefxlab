// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text.Utf8;
using System.Text.Utf16;

namespace System.Text {

    public abstract class TextEncoder
    {
        public EncodingName Encoding { get; private set; }

        public enum EncodingName : byte
        {
            Utf16 = 0,
            Utf8 = 1,
            //Ascii = 2,
            //Utf16BE = 3,
            //ISO8859_1 = 4,
        }

        public readonly static TextEncoder Utf8 = new Utf8TextEncoding() { Encoding = EncodingName.Utf8 };
        public readonly static TextEncoder Utf16 = new Utf16TextEncodingLE() { Encoding = EncodingName.Utf16 };

        #region Decode Methods

        /// <summary>
        /// Decodes a span containing a sequence of bytes into a UTF-16 string value.
        /// </summary>
        /// <param name="encodedBytes">The data span to consume for decoding. The encoded data should be relative to the concrete instance of the class used.</param>
        /// <param name="text">An output parameter to capture the decoded string.</param>
        /// <param name="bytesConsumed">An output parameter to capture the number of bytes successfully consumed during decoding from <paramref name="encodedBytes"/></param>
        /// <returns>
        /// True if successfully able to consume the entire data span in the decoding process.
        /// False if decoding failed. <paramref name="text"/> will contain as much of the string as possible to decode with
        /// <paramref name="bytesConsumed"> set to the number of bytes used.
        /// 
        /// It is possible to continue decoding from the stop point by slicing off the processed part of <paramref name="encodedBytes"/>, fixing
        /// errors (reading more data, patching encoded bytes, etc.) and calling this method again.
        /// </returns>
        public abstract bool TryDecode(ReadOnlySpan<byte> encodedBytes, out string text, out int bytesConsumed);

        /// <summary>
        /// Decodes a span containing a sequence of bytes into UTF-8 bytes.
        /// </summary>
        /// <param name="encodedBytes">The data span to consume for decoding. The encoded data should be relative to the concrete instance of the class used.</param>
        /// <param name="utf8">A span buffer to capture the decoded UTF-8 bytes.</param>
        /// <param name="bytesConsumed">An output parameter to capture the number of bytes successfully consumed during decoding from <paramref name="encodedBytes"/></param>
        /// <param name="bytesWritten">A output parameter to capture the number of bytes written to <paramref name="utf8"/></param>
        /// <returns>
        /// True if successfully able to consume the entire data span in the decoding process.
        /// False if decoding failed. <paramref name="utf8"/> will contain as much of the data as possible to decode with
        /// <paramref name="bytesConsumed"> set to the number of bytes used.
        /// 
        /// It is possible to continue decoding from the stop point by slicing off the processed part of <paramref name="encodedBytes"/>, fixing
        /// errors (reading more data, patching encoded bytes, etc.) and calling this method again.
        /// </returns>
        public abstract bool TryDecode(ReadOnlySpan<byte> encodedBytes, Span<byte> utf8, out int bytesConsumed, out int bytesWritten);

        /// <summary>
        /// Decodes a span containing a sequence of bytes into UTF-16 characters.
        /// </summary>
        /// <param name="encodedBytes">The data span to consume for decoding. The encoded data should be relative to the concrete instance of the class used.</param>
        /// <param name="utf16">A span buffer to capture the decoded UTF-16 characters.</param>
        /// <param name="bytesConsumed">An output parameter to capture the number of bytes successfully consumed during decoding from <paramref name="encodedBytes"/></param>
        /// <param name="charactersWritten">A output parameter to capture the number of bytes written to <paramref name="utf16"/></param>
        /// <returns>
        /// True if successfully able to consume the entire data span in the decoding process.
        /// False if decoding failed. <paramref name="utf16"/> will contain as much of the data as possible to decode with
        /// <paramref name="bytesConsumed"> set to the number of bytes used.
        /// 
        /// It is possible to continue decoding from the stop point by slicing off the processed part of <paramref name="encodedBytes"/>, fixing
        /// errors (reading more data, patching encoded bytes, etc.) and calling this method again.
        /// </returns>
        public abstract bool TryDecode(ReadOnlySpan<byte> encodedBytes, Span<char> utf16, out int bytesConsumed, out int charactersWritten);

        /// <summary>
        /// Decodes a span containing a sequence of bytes into UTF-32 characters.
        /// </summary>
        /// <param name="encodedBytes">The data span to consume for decoding. The encoded data should be relative to the concrete instance of the class used.</param>
        /// <param name="utf32">A span buffer to capture the decoded UTF-32 characters.</param>
        /// <param name="bytesConsumed">An output parameter to capture the number of bytes successfully consumed during decoding from <paramref name="encodedBytes"/></param>
        /// <param name="charactersWritten">A output parameter to capture the number of bytes written to <paramref name="utf32"/></param>
        /// <returns>
        /// True if successfully able to consume the entire data span in the decoding process.
        /// False if decoding failed. <paramref name="utf32"/> will contain as much of the data as possible to decode with
        /// <paramref name="bytesConsumed"> set to the number of bytes used.
        /// 
        /// It is possible to continue decoding from the stop point by slicing off the processed part of <paramref name="encodedBytes"/>, fixing
        /// errors (reading more data, patching encoded bytes, etc.) and calling this method again.
        /// </returns>
        public abstract bool TryDecode(ReadOnlySpan<byte> encodedBytes, Span<uint> utf32, out int bytesConsumed, out int charactersWritten);

        #endregion Decode Methods

        #region Encode Methods

        /// <summary>
        /// Encodes a span containing a sequence of UTF-8 bytes. The target encoding is relative to the concrete class being executed.
        /// </summary>
        /// <param name="utf8">The data span to consume for encoding.</param>
        /// <param name="encodedBytes">A span buffer to write the encoded bytes.</param>
        /// <param name="bytesConsumed">An output parameter to capture the number of bytes successfully consumed during encoding from <paramref name="utf8"/></param>
        /// <param name="bytesWritten">A output parameter to capture the number of bytes written to <paramref name="encodedBytes"/></param>
        /// <returns>
        /// True if successfully able to consume the entire data span in the encoding process.
        /// False if encoding failed. <paramref name="encodedBytes"/> will contain as much of the data as possible to encode with
        /// <paramref name="bytesConsumed"> set to the number of bytes used.
        /// 
        /// It is possible to continue encoding from the stop point by slicing off the processed part of <paramref name="utf8"/>, fixing
        /// errors (reading more data, new output buffer, etc.) and calling this method again.
        /// </returns>
        public abstract bool TryEncode(ReadOnlySpan<byte> utf8, Span<byte> encodedBytes, out int bytesConsumed, out int bytesWritten);

        /// <summary>
        /// Encodes a span containing a sequence of UTF-16 characters. The target encoding is relative to the concrete class being executed.
        /// </summary>
        /// <param name="utf16">The data span to consume for encoding.</param>
        /// <param name="encodedBytes">A span buffer to write the encoded bytes.</param>
        /// <param name="charactersConsumed">An output parameter to capture the number of characters successfully consumed during encoding from <paramref name="utf8"/></param>
        /// <param name="bytesWritten">A output parameter to capture the number of bytes written to <paramref name="encodedBytes"/></param>
        /// <returns>
        /// True if successfully able to consume the entire data span in the encoding process.
        /// False if encoding failed. <paramref name="encodedBytes"/> will contain as much of the data as possible to encode with
        /// <paramref name="charactersConsumed"> set to the number of characters used.
        /// 
        /// It is possible to continue encoding from the stop point by slicing off the processed part of <paramref name="utf16"/>, fixing
        /// errors (reading more data, new output buffer, etc.) and calling this method again.
        /// </returns>
        public abstract bool TryEncode(ReadOnlySpan<char> utf16, Span<byte> encodedBytes, out int charactersConsumed, out int bytesWritten);

        /// <summary>
        /// Encodes a span containing a sequence of UTF-32 characters. The target encoding is relative to the concrete class being executed.
        /// </summary>
        /// <param name="utf32">The data span to consume for encoding.</param>
        /// <param name="encodedBytes">A span buffer to write the encoded bytes.</param>
        /// <param name="charactersConsumed">An output parameter to capture the number of characters successfully consumed during encoding from <paramref name="utf8"/></param>
        /// <param name="bytesWritten">A output parameter to capture the number of bytes written to <paramref name="encodedBytes"/></param>
        /// <returns>
        /// True if successfully able to consume the entire data span in the encoding process.
        /// False if encoding failed. <paramref name="encodedBytes"/> will contain as much of the data as possible to encode with
        /// <paramref name="charactersConsumed"> set to the number of characters used.
        /// 
        /// It is possible to continue encoding from the stop point by slicing off the processed part of <paramref name="utf32"/>, fixing
        /// errors (reading more data, new output buffer, etc.) and calling this method again.
        /// </returns>
        public abstract bool TryEncode(ReadOnlySpan<uint> utf32, Span<byte> encodedBytes, out int charactersConsumed, out int bytesWritten);

        /// <summary>
        /// Encodes a UTF-16 string. The target encoding is relative to the concrete class being executed.
        /// </summary>
        /// <param name="text">A string containing the characters to encode.</param>
        /// <param name="encodedBytes">A span buffer to write the encoded bytes.</param>
        /// <param name="bytesWritten">A output parameter to capture the number of bytes written to <paramref name="encodedBytes"/></param>
        /// <returns>
        /// True if successfully able to encode the entire string of characters, else false.
        ///
        /// A failure likely means the output buffer is too small.
        /// </returns>
        public abstract bool TryEncode(string text, Span<byte> encodedBytes, out int bytesWritten);

        #endregion Encode Methods
    }
}