// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Runtime.CompilerServices;
using System.Text.Utf16;

namespace System.Text.Utf8
{
    internal static class Utf8Encoder
    {
        #region Constants

        // To get this to compile with dotnet cli, we need to temporarily un-binary the magic values
        private const byte b0000_0111U = 0x07; //7
        private const byte b0000_1111U = 0x0F; //15
        private const byte b0001_1111U = 0x1F; //31
        private const byte b0011_1111U = 0x3F; //63
        private const byte b0111_1111U = 0x7F; //127
        private const byte b1000_0000U = 0x80; //128
        private const byte b1100_0000U = 0xC0; //192
        private const byte b1110_0000U = 0xE0; //224
        private const byte b1111_0000U = 0xF0; //240
        private const byte b1111_1000U = 0xF8; //248

        #endregion Constants

        #region Decoding implementation

        /// <summary>
        /// Decodes a span of UTF-8 characters into UTF-16.
        /// 
        /// This method will consume as many of the input characters as possible.
        ///
        /// On successful exit, the entire input was consumed and encoded successfully. In this case, <paramref name="bytesConsumed"/> will be
        /// equal to the length of the <paramref name="utf8"/> and <paramref name="charactersWritten"/> will equal the total number of bytes written to
        /// the <paramref name="utf16"/>.
        /// 
        /// On unsuccessful exit, the following conditions can exist.
        ///  1) If the output buffer has been filled and no more input characters can be encoded, another call to this method with the input sliced to
        ///     exclude the already encoded characters (using <paramref name="bytesConsumed"/>) and a new output buffer will continue the encoding.
        ///  2) Encoding may have also stopped because the input buffer contains an invalid sequence.
        /// </summary>
        /// <param name="utf8">A span containing a sequence of UTF-8 characters.</param>
        /// <param name="utf16">A span to write the UTF-16 data into.</param>
        /// <param name="bytesConsumed">On exit, contains the number of code points that were consumed from the UTF-16 character span.</param>
        /// <param name="charactersWritten">An output parameter to store the number of characters written to <paramref name="utf16"/></param>
        /// <returns>True if the input buffer was fully encoded into the output buffer, otherwise false.</returns>
        public static bool TryDecode(ReadOnlySpan<byte> utf8, Span<char> utf16, out int bytesConsumed, out int charactersWritten)
        {
            bytesConsumed = 0;
            charactersWritten = 0;

            while (bytesConsumed < utf8.Length)
            {
                uint codePoint;
                int consumed;

                if (!TryDecodeCodePoint(utf8, bytesConsumed, out codePoint, out consumed))
                    return false;

                int written;
                if (!Utf16LittleEndianEncoder.TryEncode(codePoint, utf16, charactersWritten, out written))
                    return false;

                charactersWritten += written;
                bytesConsumed += consumed;
            }

            return true;
        }

        /// <summary>
        /// Decodes a span of UTF-8 characters into UTF-32.
        /// 
        /// This method will consume as many of the input characters as possible.
        ///
        /// On successful exit, the entire input was consumed and encoded successfully. In this case, <paramref name="bytesConsumed"/> will be
        /// equal to the length of the <paramref name="utf8"/> and <paramref name="charactersWritten"/> will equal the total number of bytes written to
        /// the <paramref name="utf32"/>.
        /// 
        /// On unsuccessful exit, the following conditions can exist.
        ///  1) If the output buffer has been filled and no more input characters can be encoded, another call to this method with the input sliced to
        ///     exclude the already encoded characters (using <paramref name="bytesConsumed"/>) and a new output buffer will continue the encoding.
        ///  2) Encoding may have also stopped because the input buffer contains an invalid sequence.
        /// </summary>
        /// <param name="utf8">A span containing a sequence of UTF-8 bytes.</param>
        /// <param name="utf32">A span to write the UTF-32 data into.</param>
        /// <param name="bytesConsumed">On exit, contains the number of code points that were consumed from the UTF-8 byte span.</param>
        /// <param name="charactersWritten">An output parameter to store the number of characters written to <paramref name="utf32"/></param>
        /// <returns>True if the input buffer was fully encoded into the output buffer, otherwise false.</returns>
        public static bool TryDecode(ReadOnlySpan<byte> utf8, Span<uint> utf32, out int bytesConsumed, out int charactersWritten)
        {
            bytesConsumed = 0;
            charactersWritten = 0;

            // TODO: Is this really what we want?
            if (utf8.Length == 0)
                return false;

            while (bytesConsumed < utf8.Length)
            {
                uint codePoint;
                int consumed;

                if (!TryDecodeCodePoint(utf8, bytesConsumed, out codePoint, out consumed))
                    return false;

                utf32[charactersWritten++] = codePoint;
                bytesConsumed += consumed;
            }

            return true;
        }

        /// <summary>
        /// Decode a single unicode character from a UTF-8 stream at the specified index.
        /// </summary>
        /// <param name="utf8">The UTF-8 stream to read the encoded sequence from.</param>
        /// <param name="index">The index into the UTF-8 Span to start processing.</param>
        /// <param name="codePoint">An output parameter to capture the decoded unicode character.</param>
        /// <param name="bytesConsumed">The number of bytes that were consumed from the UTF-8 span.</param>
        /// <returns>True if successfully able to decode a unicode character, else false.</returns>
        internal static bool TryDecodeCodePoint(ReadOnlySpan<byte> utf8, int index, out uint codePoint, out int bytesConsumed)
        {
            if (index >= utf8.Length)
            {
                codePoint = default(uint);
                bytesConsumed = 0;
                return false;
            }

            var first = utf8[index];

            bytesConsumed = GetEncodedBytes(first);
            if (bytesConsumed == 0 || utf8.Length - index < bytesConsumed)
            {
                bytesConsumed = 0;
                codePoint = default(uint);
                return false;
            }

            // ROBERT: The values in the masks below look wrong. Need to validate.
            switch (bytesConsumed)
            {
                case 1:
                    codePoint = (uint)(first & b0111_1111U);
                    break;

                case 2:
                    codePoint = (uint)(first & b0001_1111U);
                    break;

                case 3:
                    codePoint = (uint)(first & b0000_1111U);
                    break;

                case 4:
                    codePoint = (uint)(first & b0000_0111U);
                    break;

                default:
                    codePoint = default(uint);
                    bytesConsumed = 0;
                    return false;
            }

            for (var i = 1; i < bytesConsumed; i++)
            {
                uint current = utf8[index + i];
                if ((current & b1100_0000U) != b1000_0000U)
                {
                    bytesConsumed = 0;
                    codePoint = default(uint);
                    return false;
                }

                codePoint = (codePoint << 6) | (b0011_1111U & current);
            }

            return true;
        }

        /// <summary>
        /// Compute the length of a string in UTF-16 characters for a given UTF-8 byte stream.
        /// </summary>
        /// <param name="utf8">The UTF-8 span to process.</param>
        /// <param name="length">An output parameter to capture the string length in UTF-16 characters.</param>
        /// <returns>True is successful in processing the entire UTF-8 sequence, else false.</returns>
        internal static bool TryComputeStringLength(ReadOnlySpan<byte> utf8, out int length)
        {
            length = 0;

            for (int i = 0; i < utf8.Length; /* Increment is based on consumed below */)
            {
                uint codePoint;
                int consumed;

                if (!TryDecodeCodePoint(utf8, i, out codePoint, out consumed))
                    return false;

                length += UnicodeHelpers.IsBmp(codePoint) ? 1 : 2;

                i += consumed;
            }

            return true;
        }

        private static int GetEncodedBytes(byte b)
        {
            if ((b & b1000_0000U) == 0)
                return 1;

            if ((b & b1110_0000U) == b1100_0000U)
                return 2;

            if ((b & b1111_0000U) == b1110_0000U)
                return 3;

            if ((b & b1111_1000U) == b1111_0000U)
                return 4;

            return 0;
        }

        #endregion Decoding implementation

        #region Encoding implementation

        /// <summary>
        /// Computes the number of bytes necessary to encode a given UTF-16 sequence.
        /// </summary>
        /// <param name="utf16">A span containing a sequence of UTF-16 characters to encode.</param>
        /// <param name="bytesNeeded">An output parameter to hold the number of bytes needed for encoding.</param>
        /// <returns>Returns true is the span is capable of being fully encoded to UTF-8, else false.</returns>
        internal static bool TryComputeEncodedBytes(ReadOnlySpan<char> utf16, out int bytesNeeded)
        {
            bytesNeeded = 0;

            for (int i = 0; i < utf16.Length; i++)
            {
                var ch = utf16[i];

                if ((ushort)ch <= 0x7f) // Fast path for ASCII
                    bytesNeeded++;
                else if (!char.IsSurrogate(ch))
                    bytesNeeded += GetNumberOfEncodedBytes((uint)ch);
                else
                {
                    if (++i >= utf16.Length)
                        return false;

                    uint codePoint = (uint)char.ConvertToUtf32(ch, utf16[i]);
                    bytesNeeded += GetNumberOfEncodedBytes(codePoint);
                }
            }

            return true;
        }

        /// <summary>
        /// Computes the number of bytes necessary to encode a given UTF-32 character sequence.
        /// </summary>
        /// <param name="utf32">A span containing a sequence of UTF-32 characters to encode.</param>
        /// <param name="bytesNeeded">An output parameter to hold the number of bytes needed for encoding.</param>
        /// <returns>Returns true is the span is capable of being fully encoded to UTF-8, else false.</returns>
        internal static bool TryComputeEncodedBytes(ReadOnlySpan<uint> utf32, out int bytesNeeded)
        {
            bytesNeeded = 0;

            for (int i = 0; i < utf32.Length; i++)
                bytesNeeded += GetNumberOfEncodedBytes(utf32[i]);

            return true;
        }

        /// <summary>
        /// Encodes a UTF-16 span of characters into UTF-8.
        /// 
        /// This method will consume as many of the input characters as possible.
        ///
        /// On successful exit, the entire input was consumed and encoded successfully. In this case, <paramref name="charactersConsumed"/> will be
        /// equal to the length of the <paramref name="utf16"/> and <paramref name="bytesWritten"/> will equal the total number of bytes written to
        /// the <paramref name="utf8"/>.
        /// 
        /// On unsuccessful exit, the following conditions can exist.
        ///  1) If the output buffer has been filled and no more input characters can be encoded, another call to this method with the input sliced to
        ///     exclude the already encoded characters (using <paramref name="charactersConsumed"/>) and a new output buffer will continue the encoding.
        ///  2) Encoding may have also stopped because the input buffer contains an invalid sequence.
        /// </summary>
        /// <param name="utf16">A span containing a sequence of UTF-16 characters.</param>
        /// <param name="utf8">A span to write the UTF-8 encoded data into.</param>
        /// <param name="charactersConsumed">On exit, contains the number of characters that were consumed from the UTF-16 span.</param>
        /// <param name="bytesWritten">An output parameter to store the number of bytes written to <paramref name="utf8"/></param>
        /// <returns>True if the input buffer was fully encoded into the output buffer, otherwise false.</returns>
        public static bool TryEncode(ReadOnlySpan<char> utf16, Span<byte> utf8, out int charactersConsumed, out int bytesWritten)
        {
            var availableCharacters = utf16.Length;
            var bufferLength = utf8.Length;

            charactersConsumed = 0;
            bytesWritten = 0;

            for (int i = 0; i < availableCharacters; i++)
            {
                var ch = utf16[i];

                if ((ushort)ch <= 0x7f) // this if block just optimizes for ascii
                {
                    if (bufferLength - bytesWritten < 1)
                        return false;

                    utf8[bytesWritten++] = (byte)ch;
                    charactersConsumed++;
                }
                else
                {
                    uint codePoint;
                    int consumed;

                    if (!char.IsSurrogate(ch))
                    {
                        codePoint = (uint)ch;
                        consumed = 1;
                    }
                    else
                    {
                        if (++i >= availableCharacters)
                            return false;

                        codePoint = (uint)char.ConvertToUtf32(ch, utf16[i]);
                        consumed = 2;
                    }

                    int written;
                    if (!TryEncodeCodePoint(codePoint, utf8, bytesWritten, out written))
                        return false;

                    bytesWritten += written;
                    charactersConsumed += consumed;
                }
            }

            return true;
        }

        /// <summary>
        /// Encodes a span of UTF-32 characters into UTF-8.
        /// 
        /// This method will consume as many of the input characters as possible.
        ///
        /// On successful exit, the entire input was consumed and encoded successfully. In this case, <paramref name="charactersConsumed"/> will be
        /// equal to the length of the <paramref name="utf32"/> and <paramref name="bytesWritten"/> will equal the total number of bytes written to
        /// the <paramref name="utf8"/>.
        /// 
        /// On unsuccessful exit, the following conditions can exist.
        ///  1) If the output buffer has been filled and no more input characters can be encoded, another call to this method with the input sliced to
        ///     exclude the already encoded characters (using <paramref name="charactersConsumed"/>) and a new output buffer will continue the encoding.
        ///  2) Encoding may have also stopped because the input buffer contains an invalid sequence.
        /// </summary>
        /// <param name="utf32">A span containing a sequence of UTF-32 characters.</param>
        /// <param name="utf8">A span to write the UTF-8 encoded data into.</param>
        /// <param name="charactersConsumed">On exit, contains the number of code points that were consumed from the UTF-32 character span.</param>
        /// <param name="bytesWritten">An output parameter to store the number of bytes written to <paramref name="utf8"/></param>
        /// <returns>True if the input buffer was fully encoded into the output buffer, otherwise false.</returns>
        public static bool TryEncode(ReadOnlySpan<uint> utf32, Span<byte> utf8, out int charactersConsumed, out int bytesWritten)
        {
            charactersConsumed = 0;
            bytesWritten = 0;

            for (int i = 0; i < utf32.Length; i++)
            {
                int written;
                if (!TryEncodeCodePoint(utf32[i], utf8, bytesWritten, out written))
                    return false;

                charactersConsumed++;
                bytesWritten += written;
            }

            return true;
        }

        /// <summary>
        /// Encodes a single UTF-32 character into UTF-8.
        /// </summary>
        /// <param name="codePoint">A UTF-32 character to encode.</param>
        /// <param name="utf8">A span to write the UTF-8 encoded data into.</param>
        /// <param name="index">An index into the <paramref name="utf8"/> span to to the write operation.</param>
        /// <param name="bytesWritten">An output parameter to store the number of bytes written to <paramref name="utf8"/></param>
        /// <returns>True if the input buffer was fully encoded into the output buffer, otherwise false.</returns>
        internal static bool TryEncodeCodePoint(uint codePoint, Span<byte> utf8, int index, out int bytesWritten)
        {
            bytesWritten = 0;

            if (!UnicodeHelpers.IsSupportedCodePoint(codePoint))
                return false;

            bytesWritten = GetNumberOfEncodedBytes(codePoint);
            if (utf8.Length - index < bytesWritten)
            {
                bytesWritten = 0;
                return false;
            }

            switch (bytesWritten)
            {
                case 1:
                    utf8[index] = (byte)(b0111_1111U & codePoint);
                    return true;

                case 2:
                    utf8[index] = (byte)(((codePoint >> 6) & b0001_1111U) | b1100_0000U);
                    utf8[index + 1] = (byte)(((codePoint >> 0) & b0011_1111U) | b1000_0000U);
                    return true;

                case 3:
                    utf8[index] = (byte)(((codePoint >> 12) & b0000_1111U) | b1110_0000U);
                    utf8[index + 1] = (byte)(((codePoint >> 6) & b0011_1111U) | b1000_0000U);
                    utf8[index + 2] = (byte)(((codePoint >> 0) & b0011_1111U) | b1000_0000U);
                    return true;
                case 4:
                    utf8[index] = (byte)(((codePoint >> 18) & b0000_0111U) | b1111_0000U);
                    utf8[index + 1] = (byte)(((codePoint >> 12) & b0011_1111U) | b1000_0000U);
                    utf8[index + 2] = (byte)(((codePoint >> 6) & b0011_1111U) | b1000_0000U);
                    utf8[index + 3] = (byte)(((codePoint >> 0) & b0011_1111U) | b1000_0000U);
                    return true;

                default:
                    return false;
            }
        }

        /// <summary>
        /// Calculates the number of encoded bytes need to represent a given unicode code point.
        /// </summary>
        /// <param name="codePoint">The code point to encode.</param>
        /// <returns>A count of bytes needed to store the UTF-8 representation of <paramref name="codePoint"/></returns>
        internal static int GetNumberOfEncodedBytes(uint codePoint)
        {
            if (codePoint <= 0x7F)
                return 1;

            if (codePoint <= 0x7FF)
                return 2;

            if (codePoint <= 0xFFFF)
                return 3;

            if (codePoint <= 0x1FFFFF)
                return 4;

            return 0;
        }

        #endregion Encoding implementation

        #region Old stuff

        #region Decoder

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool TryFindEncodedCodePointBytesCountGoingBackwards(ReadOnlySpan<byte> buffer, out int encodedBytes)
        {
            encodedBytes = 1;
            ReadOnlySpan<byte> it = buffer;
            // TODO: Should we have something like: Span<byte>.(Slice from the back)
            for (; encodedBytes <= UnicodeConstants.Utf8MaxCodeUnitsPerCodePoint; encodedBytes++, it = it.Slice(0, it.Length - 1))
            {
                if (it.Length == 0)
                {
                    encodedBytes = default(int);
                    return false;
                }

                // TODO: Should we have Span<byte>.Last?
                if (Utf8CodeUnit.IsFirstCodeUnitInEncodedCodePoint(it[it.Length - 1]))
                {
                    // output: encodedBytes
                    return true;
                }
            }

            // Invalid unicode character or stream prematurely ended (which is still invalid character in that stream)
            encodedBytes = default(int);
            return false;
        }

        // TODO: Name TBD
        // TODO: optimize?
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static bool TryDecodeCodePointBackwards(ReadOnlySpan<byte> buffer, out UnicodeCodePoint codePoint, out int encodedBytes)
        {
            if (TryFindEncodedCodePointBytesCountGoingBackwards(buffer, out encodedBytes))
            {
                int realEncodedBytes;
                uint cp;
                // TODO: Inline decoding, as the invalid surrogate check can be done faster
                bool ret = TryDecodeCodePoint(buffer, buffer.Length - encodedBytes, out cp, out realEncodedBytes);
                if (ret && encodedBytes != realEncodedBytes)
                {
                    // invalid surrogate character
                    // we know the character length by iterating on surrogate characters from the end
                    // but the first byte of the character has also encoded length
                    // seems like the lengths don't match
                    codePoint = default(UnicodeCodePoint);
                    return false;
                }

                codePoint = new UnicodeCodePoint(cp);
                return true;
            }

            codePoint = default(UnicodeCodePoint);
            encodedBytes = default(int);
            return false;
        }

        #endregion

        #endregion Old stuff
    }
}
