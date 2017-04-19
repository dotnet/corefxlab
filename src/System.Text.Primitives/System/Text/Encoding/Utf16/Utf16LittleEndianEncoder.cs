// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Runtime.CompilerServices;
using System.Text.Utf8;

namespace System.Text.Utf16
{
    internal static class Utf16LittleEndianEncoder
    {
        #region Encoding Constants

        // TODO: Some of these members are needed only in Utf16LittleEndianEncoder.
        //       Should we add the usage of them to UnicodeCodePoint class and merge this class with it?
        private const uint Utf16HighSurrogateFirstCodePoint = 0xD800;
        private const uint Utf16HighSurrogateLastCodePoint = 0xDBFF;
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

        /// <summary>
        /// Decodes a span of UTF-16 characters into UTF-32.
        /// 
        /// This method will consume as many of the input characters as possible.
        ///
        /// On successful exit, the entire input was consumed and encoded successfully. In this case, <paramref name="charactersConsumed"/> will be
        /// equal to the length of the <paramref name="utf16"/> and <paramref name="bytesWritten"/> will equal the total number of bytes written to
        /// the <paramref name="utf32"/>.
        /// 
        /// On unsuccessful exit, the following conditions can exist.
        ///  1) If the output buffer has been filled and no more input characters can be encoded, another call to this method with the input sliced to
        ///     exclude the already encoded characters (using <paramref name="charactersConsumed"/>) and a new output buffer will continue the encoding.
        ///  2) Encoding may have also stopped because the input buffer contains an invalid sequence.
        /// </summary>
        /// <param name="utf16">A span containing a sequence of UTF-16 characters.</param>
        /// <param name="utf32">A span to write the UTF-32 data into.</param>
        /// <param name="charactersConsumed">On exit, contains the number of code points that were consumed from the UTF-16 character span.</param>
        /// <param name="bytesWritten">An output parameter to store the number of bytes written to <paramref name="utf32"/></param>
        /// <returns>True if the input buffer was fully encoded into the output buffer, otherwise false.</returns>
        public static bool TryDecode(ReadOnlySpan<char> utf16, Span<uint> utf32, out int charactersConsumed, out int charactersWritten)
        {
            charactersConsumed = 0;
            charactersWritten = 0;

            for (int i = 0; i < utf16.Length; i++)
            {
                if (charactersWritten >= utf32.Length)
                    return false;

                uint codePoint = utf16[i];

                if (IsSurrogate(codePoint))
                {
                    if (!IsHighSurrogate(codePoint) || (i + 1 >= utf16.Length))
                        return false;

                    uint lowSurrogate = utf16[++i];
                    if (!IsLowSurrogate(lowSurrogate))
                        return false;

                    unchecked
                    {
                        codePoint -= Utf16HighSurrogateFirstCodePoint;
                        lowSurrogate -= Utf16LowSurrogateFirstCodePoint;
                    }

                    // High surrogate contains 10 high bits of the code point
                    codePoint = ((codePoint << 10) | lowSurrogate) + 0x010000u;
                    charactersConsumed++;
                }

                utf32[charactersWritten++] = codePoint;
                charactersConsumed++;
            }

            return true;
        }

        #endregion Decoding implementation

        #region Encoding implementation

        /// <summary>
        /// Encodes a span of UTF-32 characters into UTF-16.
        /// 
        /// This method will consume as many of the input characters as possible.
        ///
        /// On successful exit, the entire input was consumed and encoded successfully. In this case, <paramref name="charactersConsumed"/> will be
        /// equal to the length of the <paramref name="utf32"/> and <paramref name="charactersWritten"/> will equal the total number of bytes written to
        /// the <paramref name="utf16"/>.
        /// 
        /// On unsuccessful exit, the following conditions can exist.
        ///  1) If the output buffer has been filled and no more input characters can be encoded, another call to this method with the input sliced to
        ///     exclude the already encoded characters (using <paramref name="charactersConsumed"/>) and a new output buffer will continue the encoding.
        ///  2) Encoding may have also stopped because the input buffer contains an invalid sequence.
        /// </summary>
        /// <param name="utf32">A span containing a sequence of UTF-32 characters.</param>
        /// <param name="utf16">A span to write the UTF-16 encoded data into.</param>
        /// <param name="charactersConsumed">On exit, contains the number of code points that were consumed from the UTF-32 character span.</param>
        /// <param name="charactersWritten">An output parameter to store the number of bytes written to <paramref name="utf8"/></param>
        /// <returns>True if the input buffer was fully encoded into the output buffer, otherwise false.</returns>
        public static bool TryEncode(ReadOnlySpan<uint> utf32, Span<char> utf16, out int charactersConsumed, out int charactersWritten)
        {
            charactersConsumed = 0;
            charactersWritten = 0;

            for (int i = 0; i < utf32.Length; i++)
            {
                int written;
                if (!TryEncode(utf32[i], utf16, charactersWritten, out written))
                    return false;

                charactersConsumed++;
                charactersWritten += written;
            }

            return true;
        }

        /// <summary>
        /// Encodes a single UTF-32 character into UTF-16.
        /// </summary>
        /// <param name="codePoint">A UTF-32 character to encode.</param>
        /// <param name="utf16">A span to write the UTF-16 encoded data into.</param>
        /// <param name="index">An index into the <paramref name="utf16"/> span to to the write operation.</param>
        /// <param name="charactersWritten">An output parameter to store the number of bytes written to <paramref name="utf16"/></param>
        /// <returns>True if the input buffer was fully encoded into the output buffer, otherwise false.</returns>
        public static bool TryEncode(uint codePoint, Span<char> utf16, int index, out int charactersWritten)
        {
            if (!UnicodeHelpers.IsSupportedCodePoint(codePoint))
            {
                charactersWritten = 0;
                return false;
            }

            charactersWritten = UnicodeHelpers.IsBmp(codePoint) ? 1 : 2;

            if (utf16.Length - index < charactersWritten)
            {
                charactersWritten = 0;
                return false;
            }

            unchecked
            {
                if (charactersWritten == 1)
                    utf16[index] = (char)codePoint;
                else
                {
                    utf16[index] = (char)(((codePoint - 0x010000u) >> 10) + Utf16HighSurrogateFirstCodePoint);
                    utf16[index + 1] = (char)((codePoint & 0x3FF) + Utf16LowSurrogateFirstCodePoint);
                }
            }

            return true;
        }

        /// <summary>
        /// Computes the number of bytes necessary to encode a given UTF-32 character sequence.
        /// </summary>
        /// <param name="utf32">A span containing a sequence of UTF-32 characters to encode.</param>
        /// <param name="bytesNeeded">An output parameter to hold the number of bytes needed for encoding.</param>
        /// <returns>Returns true is the span is capable of being fully encoded to UTF-16, else false.</returns>
        public static bool TryComputeEncodedBytes(ReadOnlySpan<uint> utf32, out int bytesNeeded)
        {
            int charactersWritten = 0;
            for (int i = 0; i < utf32.Length; i++)
            {
                var codePoint = utf32[i];
                if (!UnicodeHelpers.IsSupportedCodePoint(codePoint))
                {
                    bytesNeeded = 0;
                    return false;
                }
                charactersWritten += UnicodeHelpers.IsBmp(codePoint) ? 1 : 2;
            }
            bytesNeeded = charactersWritten * sizeof(char);
            return true;
        }

        #endregion Encoding implementation

        #region Surrogate helpers

        private static bool IsSurrogate(uint codePoint)
        {
            return codePoint >= Utf16SurrogateRangeStart && codePoint <= Utf16SurrogateRangeEnd;
        }

        private static bool IsLowSurrogate(uint codePoint)
        {
            return codePoint >= Utf16LowSurrogateFirstCodePoint && codePoint <= Utf16LowSurrogateLastCodePoint;
        }

        private static bool IsHighSurrogate(uint codePoint)
        {
            return codePoint >= Utf16HighSurrogateFirstCodePoint && codePoint <= Utf16HighSurrogateLastCodePoint;
        }

        #endregion Surrogate helpers

        #region Old stuff

        // TODO: Should we rewrite this to not use char.ConvertToUtf32 or is it fast enough?
        public static bool TryDecodeCodePointFromString(string s, int index, out uint codePoint, out int encodedChars)
        {
            if (index < 0 || index >= s.Length)
            {
                codePoint = default(uint);
                encodedChars = 0;
                return false;
            }

            if (index == s.Length - 1 && char.IsSurrogate(s[index]))
            {
                codePoint = default(uint);
                encodedChars = 0;
                return false;
            }

            encodedChars = char.IsHighSurrogate(s[index]) ? 2 : 1;
            codePoint = unchecked((uint)char.ConvertToUtf32(s, index));

            return true;
        }

        #endregion Old stuff
    }
}
