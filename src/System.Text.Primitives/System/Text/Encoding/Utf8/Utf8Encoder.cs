// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Runtime.CompilerServices;
using System.Diagnostics;

namespace System.Text.Utf8
{
    internal static class Utf8Encoder
    {
        #region Constants

        internal const int Utf8MaxCodeUnitsPerCodePoint = 4;

        private const byte Utf8NonFirstByteInCodePointValue = 0x80;
        private const byte Utf8NonFirstByteInCodePointMask = 0xC0;

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
        public static unsafe bool TryDecode(ReadOnlySpan<byte> utf8, Span<char> utf16, out int bytesConsumed, out int charactersWritten)
        {
            fixed (byte* pUtf8 = &utf8.DangerousGetPinnableReference())
            fixed (char* pUtf16 = &utf16.DangerousGetPinnableReference())
            {
                byte* pSrc = pUtf8;
                byte* pSrcEnd = pSrc + utf8.Length;
                char* pDst = pUtf16;
                char* pDstEnd = pDst + utf16.Length;

                int ch = 0;
                while (pSrc < pSrcEnd && pDst < pDstEnd)
                {
                    // we may need as many as 1 character per byte, so reduce the byte count if necessary.
                    // If availableChars is too small, pStop will be before pTarget and we won't do fast loop.
                    int availableChars = PtrDiff(pDstEnd, pDst);
                    int availableBytes = PtrDiff(pSrcEnd, pSrc);

                    if (availableChars < availableBytes)
                        availableBytes = availableChars;

                    // don't fall into the fast decoding loop if we don't have enough bytes
                    if (availableBytes <= 13)
                    {
                        // try to get over the remainder of the ascii characters fast though
                        byte* pLocalEnd = pSrc + availableBytes;
                        while (pSrc < pLocalEnd)
                        {
                            ch = *pSrc;
                            pSrc++;

                            if (ch > 0x7F)
                                goto LongCodeSlow;

                            *pDst = (char)ch;
                            pDst++;
                        }

                        // we are done
                        break;
                    }

                    // To compute the upper bound, assume that all characters are ASCII characters at this point,
                    //  the boundary will be decreased for every non-ASCII character we encounter
                    // Also, we need 7 chars reserve for the unrolled ansi decoding loop and for decoding of multibyte sequences
                    char* pStop = pDst + availableBytes - 7;

                    // Fast loop
                    while (pDst < pStop)
                    {
                        ch = *pSrc;
                        pSrc++;

                        if (ch > 0x7F)
                            goto LongCode;

                        *pDst = (char)ch;
                        pDst++;

                        // 2-byte align
                        if ((unchecked((int)pSrc) & 0x1) != 0)
                        {
                            ch = *pSrc;
                            pSrc++;

                            if (ch > 0x7F)
                                goto LongCode;

                            *pDst = (char)ch;
                            pDst++;
                        }

                        // 4-byte align
                        if ((unchecked((int)pSrc) & 0x2) != 0)
                        {
                            ch = *(ushort*)pSrc;
                            if ((ch & 0x8080) != 0)
                                goto LongCodeWithMask16;

                            // Unfortunately, endianness sensitive
#if BIGENDIAN
                            *pDst = (char)((ch >> 8) & 0x7F);
                            pSrc += 2;
                            *(pDst + 1) = (char)(ch & 0x7F);
                            pDst += 2;
#else // BIGENDIAN
                            *pDst = (char)(ch & 0x7F);
                            pSrc += 2;
                            *(pDst + 1) = (char)((ch >> 8) & 0x7F);
                            pDst += 2;
#endif // BIGENDIAN
                        }

                        // Run 8 characters at a time!
                        while (pDst < pStop)
                        {
                            ch = *(int*)pSrc;
                            int chb = *(int*)(pSrc + 4);
                            if (((ch | chb) & unchecked((int)0x80808080)) != 0)
                                goto LongCodeWithMask32;

                            // Unfortunately, endianness sensitive
#if BIGENDIAN
                            *pDst = (char)((ch >> 24) & 0x7F);
                            *(pDst+1) = (char)((ch >> 16) & 0x7F);
                            *(pDst+2) = (char)((ch >> 8) & 0x7F);
                            *(pDst+3) = (char)(ch & 0x7F);
                            pSrc += 8;
                            *(pDst+4) = (char)((chb >> 24) & 0x7F);
                            *(pDst+5) = (char)((chb >> 16) & 0x7F);
                            *(pDst+6) = (char)((chb >> 8) & 0x7F);
                            *(pDst+7) = (char)(chb & 0x7F);
                            pDst += 8;
#else // BIGENDIAN
                            *pDst = (char)(ch & 0x7F);
                            *(pDst + 1) = (char)((ch >> 8) & 0x7F);
                            *(pDst + 2) = (char)((ch >> 16) & 0x7F);
                            *(pDst + 3) = (char)((ch >> 24) & 0x7F);
                            pSrc += 8;
                            *(pDst + 4) = (char)(chb & 0x7F);
                            *(pDst + 5) = (char)((chb >> 8) & 0x7F);
                            *(pDst + 6) = (char)((chb >> 16) & 0x7F);
                            *(pDst + 7) = (char)((chb >> 24) & 0x7F);
                            pDst += 8;
#endif // BIGENDIAN
                        }

                        break;

#if BIGENDIAN
                    LongCodeWithMask32:
                        // be careful about the sign extension
                        ch = (int)(((uint)ch) >> 16);
                    LongCodeWithMask16:
                        ch = (int)(((uint)ch) >> 8);
#else // BIGENDIAN
                    LongCodeWithMask32:
                    LongCodeWithMask16:
                        ch &= 0xFF;
#endif // BIGENDIAN
                        pSrc++;
                        if (ch <= 0x7F)
                        {
                            *pDst = (char)ch;
                            pDst++;
                            continue;
                        }

                    LongCode:
                        int chc = *pSrc;
                        pSrc++;

                        // Bit 6 should be 0, and trailing byte should be 10vvvvvv
                        if ((ch & 0x40) == 0 || (chc & unchecked((sbyte)0xC0)) != 0x80)
                            goto ErrorExit;

                        chc &= 0x3F;

                        if ((ch & 0x20) != 0)
                        {
                            // Handle 3 or 4 byte encoding.

                            // Fold the first 2 bytes together
                            chc |= (ch & 0x0F) << 6;

                            if ((ch & 0x10) != 0)
                            {
                                // 4 byte - surrogate pair
                                ch = *pSrc;

                                // Bit 4 should be zero + the surrogate should be in the range 0x000000 - 0x10FFFF
                                // and the trailing byte should be 10vvvvvv
                                if (!InRange(chc >> 4, 0x01, 0x10) || (ch & unchecked((sbyte)0xC0)) != 0x80)
                                    goto ErrorExit;

                                // Merge 3rd byte then read the last byte
                                chc = (chc << 6) | (ch & 0x3F);
                                ch = *(pSrc + 1);

                                // The last trailing byte still holds the form 10vvvvvv
                                if ((ch & unchecked((sbyte)0xC0)) != 0x80)
                                    goto ErrorExit;

                                pSrc += 2;
                                ch = (chc << 6) | (ch & 0x3F);

                                *pDst = (char)(((ch >> 10) & 0x7FF) + unchecked((short)(HIGH_SURROGATE_START - (0x10000 >> 10))));
                                pDst++;

                                ch = (ch & 0x3FF) + unchecked((short)(LOW_SURROGATE_START));
                            }
                            else
                            {
                                // 3 byte encoding
                                ch = *pSrc;

                                // Check for non-shortest form of 3 byte sequence
                                // No surrogates
                                // Trailing byte must be in the form 10vvvvvv
                                if ((chc & (0x1F << 5)) == 0 ||
                                    (chc & (0xF800 >> 6)) == (0xD800 >> 6) ||
                                    (ch & unchecked((sbyte)0xC0)) != 0x80)
                                    goto ErrorExit;

                                pSrc++;
                                ch = (chc << 6) | (ch & 0x3F);
                            }

                            // extra byte, we're already planning 2 chars for 2 of these bytes,
                            // but the big loop is testing the target against pStop, so we need
                            // to subtract 2 more or we risk overrunning the input.  Subtract
                            // one here and one below.
                            pStop--;
                        }
                        else
                        {
                            // 2 byte encoding
                            ch &= 0x1F;

                            // Check for non-shortest form
                            if (ch <= 1)
                                goto ErrorExit;

                            ch = (ch << 6) | chc;
                        }

                        *pDst = (char)ch;
                        pDst++;

                        // extra byte, we're only expecting 1 char for each of these 2 bytes,
                        // but the loop is testing the target (not source) against pStop.
                        // subtract an extra count from pStop so that we don't overrun the input.
                        pStop--;
                    }

                    continue;

                LongCodeSlow:
                    if (pSrc >= pSrcEnd)
                    {
                        // This is a special case where hit the end of the buffer but are in the middle
                        // of decoding a long code. The error exit thinks we have read 2 extra bytes already,
                        // so we add +1 to pSrc to get the count correct for the bytes consumed value.
                        pSrc++;
                        goto ErrorExit;
                    }

                    int chd = *pSrc;
                    pSrc++;

                    // Bit 6 should be 0, and trailing byte should be 10vvvvvv
                    if ((ch & 0x40) == 0 || (chd & unchecked((sbyte)0xC0)) != 0x80)
                        goto ErrorExit;

                    chd &= 0x3F;

                    if ((ch & 0x20) != 0)
                    {
                        // Handle 3 or 4 byte encoding.

                        // Fold the first 2 bytes together
                        chd |= (ch & 0x0F) << 6;

                        if ((ch & 0x10) != 0)
                        {
                            // 4 byte - surrogate pair
                            // We need 2 more bytes
                            if (pSrc >= pSrcEnd - 1)
                                goto ErrorExit;

                            ch = *pSrc;

                            // Bit 4 should be zero + the surrogate should be in the range 0x000000 - 0x10FFFF
                            // and the trailing byte should be 10vvvvvv
                            if (!InRange(chd >> 4, 0x01, 0x10) || (ch & unchecked((sbyte)0xC0)) != 0x80)
                                goto ErrorExit;

                            // Merge 3rd byte then read the last byte
                            chd = (chd << 6) | (ch & 0x3F);
                            ch = *(pSrc + 1);

                            // The last trailing byte still holds the form 10vvvvvv
                            // We only know for sure we have room for one more char, but we need an extra now.
                            if ((ch & unchecked((sbyte)0xC0)) != 0x80 || PtrDiff(pDstEnd, pDst) < 2)
                                goto ErrorExit;

                            pSrc += 2;
                            ch = (chd << 6) | (ch & 0x3F);

                            *pDst = (char)(((ch >> 10) & 0x7FF) + unchecked((short)(HIGH_SURROGATE_START - (0x10000 >> 10))));
                            pDst++;

                            ch = (ch & 0x3FF) + unchecked((short)(LOW_SURROGATE_START));
                        }
                        else
                        {
                            // 3 byte encoding
                            if (pSrc >= pSrcEnd)
                                goto ErrorExit;

                            ch = *pSrc;

                            // Check for non-shortest form of 3 byte sequence
                            // No surrogates
                            // Trailing byte must be in the form 10vvvvvv
                            if ((chd & (0x1F << 5)) == 0 ||
                                (chd & (0xF800 >> 6)) == (0xD800 >> 6) ||
                                (ch & unchecked((sbyte)0xC0)) != 0x80)
                                goto ErrorExit;

                            pSrc++;
                            ch = (chd << 6) | (ch & 0x3F);
                        }
                    }
                    else
                    {
                        // 2 byte encoding
                        ch &= 0x1F;

                        // Check for non-shortest form
                        if (ch <= 1)
                            goto ErrorExit;

                        ch = (ch << 6) | chd;
                    }

                    *pDst = (char)ch;
                    pDst++;
                }

                bytesConsumed = PtrDiff(pSrc, pUtf8);
                charactersWritten = PtrDiff(pDst, pUtf16);
                return PtrDiff(pSrcEnd, pSrc) == 0;

            ErrorExit:
                bytesConsumed = PtrDiff(pSrc - 2, pUtf8);
                charactersWritten = PtrDiff(pDst, pUtf16);
                return false;
            }
        }

        /// <summary>
        /// Decodes a span of UTF-8 characters into UTF-32.
        ///
        /// This method will consume as many of the input characters as possible.
        ///
        /// On successful exit, the entire input was consumed and encoded successfully. In this case, <paramref name="bytesConsumed"/> will be
        /// equal to the length of the <paramref name="utf8"/> and <paramref name="codeUnitsWritten"/> will equal the total number of code units written to
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
        /// <param name="codeUnitsWritten">An output parameter to store the number of code units written to <paramref name="utf32"/></param>
        /// <returns>True if the input buffer was fully encoded into the output buffer, otherwise false.</returns>
        public static bool TryDecode(ReadOnlySpan<byte> utf8, Span<uint> utf32, out int bytesConsumed, out int codeUnitsWritten)
        {
            bytesConsumed = 0;
            codeUnitsWritten = 0;

            while (bytesConsumed < utf8.Length)
            {
                uint codePoint;
                int consumed;

                if (!TryDecodeCodePoint(utf8, bytesConsumed, out codePoint, out consumed))
                    return false;

                utf32[codeUnitsWritten++] = codePoint;
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
                codePoint = default;
                bytesConsumed = 0;
                return false;
            }

            var first = utf8[index];

            bytesConsumed = GetEncodedBytes(first);
            if (bytesConsumed == 0 || utf8.Length - index < bytesConsumed)
            {
                bytesConsumed = 0;
                codePoint = default;
                return false;
            }

            switch (bytesConsumed)
            {
                case 1:
                    codePoint = first;
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
                    codePoint = default;
                    bytesConsumed = 0;
                    return false;
            }

            for (var i = 1; i < bytesConsumed; i++)
            {
                uint current = utf8[index + i];
                if ((current & b1100_0000U) != b1000_0000U)
                {
                    bytesConsumed = 0;
                    codePoint = default;
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
            // try? because Convert.ConvertToUtf32 can throw
            // if the high/low surrogates aren't valid; no point
            // running all the tests twice per code-point
            try
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
            catch(ArgumentOutOfRangeException)
            {
                bytesNeeded = 0;
                return false;
            }
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

        private const char HIGH_SURROGATE_START = '\ud800';
        private const char HIGH_SURROGATE_END = '\udbff';
        private const char LOW_SURROGATE_START = '\udc00';
        private const char LOW_SURROGATE_END = '\udfff';

        unsafe private static int PtrDiff(char* a, char* b)
        {
            return (int)(((uint)((byte*)a - (byte*)b)) >> 1);
        }

        // byte* flavor just for parity
        unsafe private static int PtrDiff(byte* a, byte* b)
        {
            return (int)(a - b);
        }

        private static bool InRange(int ch, int start, int end)
        {
            return (uint)(ch - start) <= (uint)(end - start);
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
        public static unsafe bool TryEncode(ReadOnlySpan<char> utf16, Span<byte> utf8, out int charactersConsumed, out int bytesWritten)
        {
            //
            //
            // KEEP THIS IMPLEMENTATION IN SYNC WITH https://github.com/dotnet/corert/blob/master/src/System.Private.CoreLib/src/System/Text/UTF8Encoding.cs
            //
            //
            fixed (char* chars = &utf16.DangerousGetPinnableReference())
            fixed (byte* bytes = &utf8.DangerousGetPinnableReference())
            {
                char* pSrc = chars;
                byte* pTarget = bytes;

                char* pEnd = pSrc + utf16.Length;
                byte* pAllocatedBufferEnd = pTarget + utf8.Length;

                // assume that JIT will enregister pSrc, pTarget and ch

                // Entering the fast encoding loop incurs some overhead that does not get amortized for small
                // number of characters, and the slow encoding loop typically ends up running for the last few
                // characters anyway since the fast encoding loop needs 5 characters on input at least.
                // Thus don't use the fast decoding loop at all if we don't have enough characters. The threashold
                // was choosen based on performance testing.
                // Note that if we don't have enough bytes, pStop will prevent us from entering the fast loop.
                while (pEnd - pSrc > 13)
                {
                    // we need at least 1 byte per character, but Convert might allow us to convert
                    // only part of the input, so try as much as we can.  Reduce charCount if necessary
                    int available = Math.Min(PtrDiff(pEnd, pSrc), PtrDiff(pAllocatedBufferEnd, pTarget));

                    // FASTLOOP:
                    // - optimistic range checks
                    // - fallbacks to the slow loop for all special cases, exception throwing, etc.

                    // To compute the upper bound, assume that all characters are ASCII characters at this point,
                    //  the boundary will be decreased for every non-ASCII character we encounter
                    // Also, we need 5 chars reserve for the unrolled ansi decoding loop and for decoding of surrogates
                    // If there aren't enough bytes for the output, then pStop will be <= pSrc and will bypass the loop.
                    char* pStop = pSrc + available - 5;
                    if (pSrc >= pStop)
                        break;

                    do
                    {
                        int ch = *pSrc;
                        pSrc++;

                        if (ch > 0x7F)
                        {
                            goto LongCode;
                        }
                        *pTarget = (byte)ch;
                        pTarget++;

                        // get pSrc aligned
                        if ((unchecked((int)pSrc) & 0x2) != 0)
                        {
                            ch = *pSrc;
                            pSrc++;
                            if (ch > 0x7F)
                            {
                                goto LongCode;
                            }
                            *pTarget = (byte)ch;
                            pTarget++;
                        }

                        // Run 4 characters at a time!
                        while (pSrc < pStop)
                        {
                            ch = *(int*)pSrc;
                            int chc = *(int*)(pSrc + 2);
                            if (((ch | chc) & unchecked((int)0xFF80FF80)) != 0)
                            {
                                goto LongCodeWithMask;
                            }

                            // Unfortunately, this is endianess sensitive
#if BIGENDIAN
                            *pTarget = (byte)(ch >> 16);
                            *(pTarget + 1) = (byte)ch;
                            pSrc += 4;
                            *(pTarget + 2) = (byte)(chc >> 16);
                            *(pTarget + 3) = (byte)chc;
                            pTarget += 4;
#else // BIGENDIAN
                            *pTarget = (byte)ch;
                            *(pTarget + 1) = (byte)(ch >> 16);
                            pSrc += 4;
                            *(pTarget + 2) = (byte)chc;
                            *(pTarget + 3) = (byte)(chc >> 16);
                            pTarget += 4;
#endif // BIGENDIAN
                        }
                        continue;

                        LongCodeWithMask:
#if BIGENDIAN
                        // be careful about the sign extension
                        ch = (int)(((uint)ch) >> 16);
#else // BIGENDIAN
                        ch = (char)ch;
#endif // BIGENDIAN
                        pSrc++;

                        if (ch > 0x7F)
                        {
                            goto LongCode;
                        }
                        *pTarget = (byte)ch;
                        pTarget++;
                        continue;

                        LongCode:
                        // use separate helper variables for slow and fast loop so that the jit optimizations
                        // won't get confused about the variable lifetimes
                        int chd;
                        if (ch <= 0x7FF)
                        {
                            // 2 byte encoding
                            chd = unchecked((sbyte)0xC0) | (ch >> 6);
                        }
                        else
                        {
                            // if (!IsLowSurrogate(ch) && !IsHighSurrogate(ch))
                            if (!InRange(ch, HIGH_SURROGATE_START, LOW_SURROGATE_END))
                            {
                                // 3 byte encoding
                                chd = unchecked((sbyte)0xE0) | (ch >> 12);
                            }
                            else
                            {
                                // 4 byte encoding - high surrogate + low surrogate
                                // if (!IsHighSurrogate(ch))
                                if (ch > HIGH_SURROGATE_END)
                                {
                                    // low without high -> bad
                                    goto NeedMore;
                                }

                                chd = *pSrc;

                                // if (!IsLowSurrogate(chd)) {
                                if (!InRange(chd, LOW_SURROGATE_START, LOW_SURROGATE_END))
                                {
                                    // high not followed by low -> bad
                                    goto NeedMore;
                                }

                                pSrc++;

                                ch = chd + (ch << 10) +
                                    (0x10000
                                    - LOW_SURROGATE_START
                                    - (HIGH_SURROGATE_START << 10));

                                *pTarget = (byte)(unchecked((sbyte)0xF0) | (ch >> 18));
                                // pStop - this byte is compensated by the second surrogate character
                                // 2 input chars require 4 output bytes.  2 have been anticipated already
                                // and 2 more will be accounted for by the 2 pStop-- calls below.
                                pTarget++;

                                chd = unchecked((sbyte)0x80) | (ch >> 12) & 0x3F;
                            }
                            *pTarget = (byte)chd;
                            pStop--;                    // 3 byte sequence for 1 char, so need pStop-- and the one below too.
                            pTarget++;

                            chd = unchecked((sbyte)0x80) | (ch >> 6) & 0x3F;
                        }
                        *pTarget = (byte)chd;
                        pStop--;                        // 2 byte sequence for 1 char so need pStop--.

                        *(pTarget + 1) = (byte)(unchecked((sbyte)0x80) | ch & 0x3F);
                        // pStop - this byte is already included

                        pTarget += 2;
                    }
                    while (pSrc < pStop);

                    Debug.Assert(pTarget <= pAllocatedBufferEnd, "[UTF8Encoding.GetBytes]pTarget <= pAllocatedBufferEnd");
                }

                while (pSrc < pEnd)
                {
                    // SLOWLOOP: does all range checks, handles all special cases, but it is slow

                    // read next char. The JIT optimization seems to be getting confused when
                    // compiling "ch = *pSrc++;", so rather use "ch = *pSrc; pSrc++;" instead
                    int ch = *pSrc;
                    pSrc++;

                    if (ch <= 0x7F)
                    {
                        if (pAllocatedBufferEnd - pTarget <= 0)
                            goto NeedMore;

                        *pTarget = (byte)ch;
                        pTarget++;
                        continue;
                    }

                    int chd;
                    if (ch <= 0x7FF)
                    {
                        if (pAllocatedBufferEnd - pTarget <= 1)
                            goto NeedMore;

                        // 2 byte encoding
                        chd = unchecked((sbyte)0xC0) | (ch >> 6);
                    }
                    else
                    {
                        // if (!IsLowSurrogate(ch) && !IsHighSurrogate(ch))
                        if (!InRange(ch, HIGH_SURROGATE_START, LOW_SURROGATE_END))
                        {
                            if (pAllocatedBufferEnd - pTarget <= 2)
                                goto NeedMore;

                            // 3 byte encoding
                            chd = unchecked((sbyte)0xE0) | (ch >> 12);
                        }
                        else
                        {
                            if (pAllocatedBufferEnd - pTarget <= 3)
                                goto NeedMore;

                            // 4 byte encoding - high surrogate + low surrogate
                            // if (!IsHighSurrogate(ch))
                            if (ch > HIGH_SURROGATE_END)
                            {
                                // low without high -> bad
                                goto NeedMore;
                            }

                            chd = *pSrc;

                            // if (!IsLowSurrogate(chd)) {
                            if (!InRange(chd, LOW_SURROGATE_START, LOW_SURROGATE_END))
                            {
                                // high not followed by low -> bad
                                goto NeedMore;
                            }

                            pSrc++;

                            ch = chd + (ch << 10) +
                                (0x10000
                                - LOW_SURROGATE_START
                                - (HIGH_SURROGATE_START << 10));

                            *pTarget = (byte)(unchecked((sbyte)0xF0) | (ch >> 18));
                            pTarget++;

                            chd = unchecked((sbyte)0x80) | (ch >> 12) & 0x3F;
                        }
                        *pTarget = (byte)chd;
                        pTarget++;

                        chd = unchecked((sbyte)0x80) | (ch >> 6) & 0x3F;
                    }

                    *pTarget = (byte)chd;
                    *(pTarget + 1) = (byte)(unchecked((sbyte)0x80) | ch & 0x3F);

                    pTarget += 2;
                }

                charactersConsumed = (int)(pSrc - chars);
                bytesWritten = (int)(pTarget - bytes);
                return true;

            NeedMore:
                charactersConsumed = (int)((pSrc - 1) - chars);
                bytesWritten = (int)(pTarget - bytes);
                return false;
            }
        }

        /// <summary>
        /// Encodes a span of UTF-32 characters into UTF-8.
        ///
        /// This method will consume as many of the input characters as possible.
        ///
        /// On successful exit, the entire input was consumed and encoded successfully. In this case, <paramref name="codeUnitsConsumed"/> will be
        /// equal to the length of the <paramref name="utf32"/> and <paramref name="bytesWritten"/> will equal the total number of bytes written to
        /// the <paramref name="utf8"/>.
        ///
        /// On unsuccessful exit, the following conditions can exist.
        ///  1) If the output buffer has been filled and no more input code units can be encoded, another call to this method with the input sliced to
        ///     exclude the already encoded code units (using <paramref name="codeUnitsConsumed"/>) and a new output buffer will continue the encoding.
        ///  2) Encoding may have also stopped because the input buffer contains an invalid sequence.
        /// </summary>
        /// <param name="utf32">A span containing a sequence of UTF-32 characters.</param>
        /// <param name="utf8">A span to write the UTF-8 encoded data into.</param>
        /// <param name="codeUnitsConsumed">On exit, contains the number of code points that were consumed from the UTF-32 character span.</param>
        /// <param name="bytesWritten">An output parameter to store the number of bytes written to <paramref name="utf8"/></param>
        /// <returns>True if the input buffer was fully encoded into the output buffer, otherwise false.</returns>
        public static bool TryEncode(ReadOnlySpan<uint> utf32, Span<byte> utf8, out int codeUnitsConsumed, out int bytesWritten)
        {
            codeUnitsConsumed = 0;
            bytesWritten = 0;

            for (int i = 0; i < utf32.Length; i++)
            {
                int written;
                if (!TryEncodeCodePoint(utf32[i], utf8, bytesWritten, out written))
                    return false;

                codeUnitsConsumed++;
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

            if (codePoint <= 0x10FFFF)
                return 4;

            return 0;
        }

        #endregion Encoding implementation
    }
}
