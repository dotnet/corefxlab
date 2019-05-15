// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System.Buffers.Text
{
    public static partial class TextEncodings
    {
        public static class Utf8
        {
            #region UTF-16 Conversions

            /// <summary>
            /// Calculates the byte count needed to encode the UTF-8 bytes from the specified UTF-16 sequence.
            ///
            /// This method will consume as many of the input bytes as possible.
            /// </summary>
            /// <param name="source">A span containing a sequence of UTF-16 bytes.</param>
            /// <param name="bytesNeeded">On exit, contains the number of bytes required for encoding from the <paramref name="source"/>.</param>
            /// <returns>A <see cref="OperationStatus"/> value representing the expected state of the conversion.</returns>
            public static OperationStatus FromUtf16Length(ReadOnlySpan<byte> source, out int bytesNeeded)
                => Utf16.ToUtf8Length(source, out bytesNeeded);

            /// <summary>
            /// Converts a span containing a sequence of UTF-16 bytes into UTF-8 bytes.
            ///
            /// This method will consume as many of the input bytes as possible.
            ///
            /// On successful exit, the entire input was consumed and encoded successfully. In this case, <paramref name="bytesConsumed"/> will be
            /// equal to the length of the <paramref name="source"/> and <paramref name="bytesWritten"/> will equal the total number of bytes written to
            /// the <paramref name="destination"/>.
            /// </summary>
            /// <param name="source">A span containing a sequence of UTF-16 bytes.</param>
            /// <param name="destination">A span to write the UTF-8 bytes into.</param>
            /// <param name="bytesConsumed">On exit, contains the number of bytes that were consumed from the <paramref name="source"/>.</param>
            /// <param name="bytesWritten">On exit, contains the number of bytes written to <paramref name="destination"/></param>
            /// <returns>A <see cref="OperationStatus"/> value representing the state of the conversion.</returns>
            public static OperationStatus FromUtf16(ReadOnlySpan<byte> source, Span<byte> destination, out int bytesConsumed, out int bytesWritten)
                => Utf16.ToUtf8(source, destination, out bytesConsumed, out bytesWritten);

            /// <summary>
            /// Calculates the byte count needed to encode the UTF-16 bytes from the specified UTF-8 sequence.
            ///
            /// This method will consume as many of the input bytes as possible.
            /// </summary>
            /// <param name="source">A span containing a sequence of UTF-8 bytes.</param>
            /// <param name="bytesNeeded">On exit, contains the number of bytes required for encoding from the <paramref name="source"/>.</param>
            /// <returns>A <see cref="OperationStatus"/> value representing the expected state of the conversion.</returns>
            public static OperationStatus ToUtf16Length(ReadOnlySpan<byte> source, out int bytesNeeded)
            {
                if (Utf8Util.GetIndexOfFirstInvalidUtf8Sequence(source, out int scalarCount, out int surrogatePairCount) < 0)
                {
                    // Well-formed UTF-8 string.

                    // 'scalarCount + surrogatePairCount' is guaranteed not to overflow because
                    // the UTF-16 representation of a string will never have a greater number of
                    // of code units than its UTF-8 representation.
                    int numCodeUnits = scalarCount + surrogatePairCount;

                    // UTF-8 code units are 2 bytes.
                    bytesNeeded = checked(numCodeUnits * 2);
                    return OperationStatus.Done;
                }
                else
                {
                    // Not a well-formed UTF-8 string. Perhaps incomplete, perhaps invalid.
                    // Regardless, we can't calculate an answer to return to the caller.
                    // Since this API doesn't have a "bytes consumed" out parameter, the
                    // input argument has an implicit "isFinalChunk = true", so we treat
                    // incomplete strings identically to invalid strings.
                    bytesNeeded = 0;
                    return OperationStatus.InvalidData;
                }
            }

            /// <summary>
            /// Converts a span containing a sequence of UTF-8 bytes into UTF-16 bytes.
            ///
            /// This method will consume as many of the input bytes as possible.
            ///
            /// On successful exit, the entire input was consumed and encoded successfully. In this case, <paramref name="bytesConsumed"/> will be
            /// equal to the length of the <paramref name="source"/> and <paramref name="bytesWritten"/> will equal the total number of bytes written to
            /// the <paramref name="destination"/>.
            /// </summary>
            /// <param name="source">A span containing a sequence of UTF-8 bytes.</param>
            /// <param name="destination">A span to write the UTF-16 bytes into.</param>
            /// <param name="bytesConsumed">On exit, contains the number of bytes that were consumed from the <paramref name="source"/>.</param>
            /// <param name="bytesWritten">On exit, contains the number of bytes written to <paramref name="destination"/></param>
            /// <returns>A <see cref="OperationStatus"/> value representing the state of the conversion.</returns>
            public unsafe static OperationStatus ToUtf16(ReadOnlySpan<byte> source, Span<byte> destination, out int bytesConsumed, out int bytesWritten)
            {
                fixed (byte* pUtf8 = &MemoryMarshal.GetReference(source))
                fixed (byte* pUtf16 = &MemoryMarshal.GetReference(destination))
                {
                    byte* pSrc = pUtf8;
                    byte* pSrcEnd = pSrc + source.Length;
                    char* pDst = (char*)pUtf16;
                    char* pDstEnd = pDst + (destination.Length >> 1);   // Conversion from bytes to chars - div by sizeof(char)

                    int ch = 0;
                    while (pSrc < pSrcEnd && pDst < pDstEnd)
                    {
                        // we may need as many as 1 character per byte, so reduce the byte count if necessary.
                        // If availableChars is too small, pStop will be before pTarget and we won't do fast loop.
                        int availableChars = EncodingHelper.PtrDiff(pDstEnd, pDst);
                        int availableBytes = EncodingHelper.PtrDiff(pSrcEnd, pSrc);

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
                                goto InvalidData;

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
                                    if (!EncodingHelper.InRange(chc >> 4, 0x01, 0x10) || (ch & unchecked((sbyte)0xC0)) != 0x80)
                                        goto InvalidData;

                                    // Merge 3rd byte then read the last byte
                                    chc = (chc << 6) | (ch & 0x3F);
                                    ch = *(pSrc + 1);

                                    // The last trailing byte still holds the form 10vvvvvv
                                    if ((ch & unchecked((sbyte)0xC0)) != 0x80)
                                        goto InvalidData;

                                    pSrc += 2;
                                    ch = (chc << 6) | (ch & 0x3F);

                                    *pDst = (char)(((ch >> 10) & 0x7FF) + unchecked((short)(EncodingHelper.HighSurrogateStart - (0x10000 >> 10))));
                                    pDst++;

                                    ch = (ch & 0x3FF) + unchecked((short)(EncodingHelper.LowSurrogateStart));
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
                                        goto InvalidData;

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
                                    goto InvalidData;

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
                            goto NeedMoreData;
                        }

                        int chd = *pSrc;
                        pSrc++;

                        // Bit 6 should be 0, and trailing byte should be 10vvvvvv
                        if ((ch & 0x40) == 0 || (chd & unchecked((sbyte)0xC0)) != 0x80)
                            goto InvalidData;

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
                                    goto NeedMoreData;

                                ch = *pSrc;

                                // Bit 4 should be zero + the surrogate should be in the range 0x000000 - 0x10FFFF
                                // and the trailing byte should be 10vvvvvv
                                if (!EncodingHelper.InRange(chd >> 4, 0x01, 0x10) || (ch & unchecked((sbyte)0xC0)) != 0x80)
                                    goto InvalidData;

                                // Merge 3rd byte then read the last byte
                                chd = (chd << 6) | (ch & 0x3F);
                                ch = *(pSrc + 1);

                                // The last trailing byte still holds the form 10vvvvvv
                                // We only know for sure we have room for one more char, but we need an extra now.
                                if ((ch & unchecked((sbyte)0xC0)) != 0x80)
                                    goto InvalidData;

                                if (EncodingHelper.PtrDiff(pDstEnd, pDst) < 2)
                                    goto DestinationFull;

                                pSrc += 2;
                                ch = (chd << 6) | (ch & 0x3F);

                                *pDst = (char)(((ch >> 10) & 0x7FF) + unchecked((short)(EncodingHelper.HighSurrogateStart - (0x10000 >> 10))));
                                pDst++;

                                ch = (ch & 0x3FF) + unchecked((short)(EncodingHelper.LowSurrogateStart));
                            }
                            else
                            {
                                // 3 byte encoding
                                if (pSrc >= pSrcEnd)
                                    goto NeedMoreData;

                                ch = *pSrc;

                                // Check for non-shortest form of 3 byte sequence
                                // No surrogates
                                // Trailing byte must be in the form 10vvvvvv
                                if ((chd & (0x1F << 5)) == 0 ||
                                    (chd & (0xF800 >> 6)) == (0xD800 >> 6) ||
                                    (ch & unchecked((sbyte)0xC0)) != 0x80)
                                    goto InvalidData;

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
                                goto InvalidData;

                            ch = (ch << 6) | chd;
                        }

                        *pDst = (char)ch;
                        pDst++;
                    }

                    DestinationFull:
                    bytesConsumed = EncodingHelper.PtrDiff(pSrc, pUtf8);
                    bytesWritten = EncodingHelper.PtrDiff((byte*)pDst, pUtf16);
                    return EncodingHelper.PtrDiff(pSrcEnd, pSrc) == 0 ? OperationStatus.Done : OperationStatus.DestinationTooSmall;

                    NeedMoreData:
                    bytesConsumed = EncodingHelper.PtrDiff(pSrc - 2, pUtf8);
                    bytesWritten = EncodingHelper.PtrDiff((byte*)pDst, pUtf16);
                    return OperationStatus.NeedMoreData;

                    InvalidData:
                    bytesConsumed = EncodingHelper.PtrDiff(pSrc - 2, pUtf8);
                    bytesWritten = EncodingHelper.PtrDiff((byte*)pDst, pUtf16);
                    return OperationStatus.InvalidData;
                }
            }

            public static string ToString(ReadOnlySpan<byte> utf8Bytes)
            {
                // TODO: why do we return status here?
                var status = ToUtf16Length(utf8Bytes, out int bytesNeeded);
                var result = new String(' ', bytesNeeded >> 1);
                unsafe
                {
                    fixed (char* pResult = result)
                    {
                        var resultBytes = new Span<byte>((void*)pResult, bytesNeeded);
                        if (ToUtf16(utf8Bytes, resultBytes, out int consumed, out int written) == OperationStatus.Done)
                        {
                            Debug.Assert(written == resultBytes.Length);
                            return result;
                        }
                    }
                }
                return String.Empty; // TODO: is this what we want to do if Bytes are invalid UTF8? Can Bytes be invalid UTF8?
            }

            #endregion UTF-16 Conversions

            #region UTF-32 Conversions

            /// <summary>
            /// Calculates the byte count needed to encode the UTF-8 bytes from the specified UTF-32 sequence.
            ///
            /// This method will consume as many of the input bytes as possible.
            /// </summary>
            /// <param name="source">A span containing a sequence of UTF-32 bytes.</param>
            /// <param name="bytesNeeded">On exit, contains the number of bytes required for encoding from the <paramref name="source"/>.</param>
            /// <returns>A <see cref="OperationStatus"/> value representing the expected state of the conversion.</returns>
            public static OperationStatus FromUtf32Length(ReadOnlySpan<byte> source, out int bytesNeeded)
                => Utf32.ToUtf8Length(source, out bytesNeeded);

            /// <summary>
            /// Converts a span containing a sequence of UTF-32 bytes into UTF-8 bytes.
            ///
            /// This method will consume as many of the input bytes as possible.
            ///
            /// On successful exit, the entire input was consumed and encoded successfully. In this case, <paramref name="bytesConsumed"/> will be
            /// equal to the length of the <paramref name="source"/> and <paramref name="bytesWritten"/> will equal the total number of bytes written to
            /// the <paramref name="destination"/>.
            /// </summary>
            /// <param name="source">A span containing a sequence of UTF-32 bytes.</param>
            /// <param name="destination">A span to write the UTF-8 bytes into.</param>
            /// <param name="bytesConsumed">On exit, contains the number of bytes that were consumed from the <paramref name="source"/>.</param>
            /// <param name="bytesWritten">On exit, contains the number of bytes written to <paramref name="destination"/></param>
            /// <returns>A <see cref="OperationStatus"/> value representing the state of the conversion.</returns>
            public static OperationStatus FromUtf32(ReadOnlySpan<byte> source, Span<byte> destination, out int bytesConsumed, out int bytesWritten)
                => Utf32.ToUtf8(source, destination, out bytesConsumed, out bytesWritten);

            /// <summary>
            /// Calculates the byte count needed to encode the UTF-32 bytes from the specified UTF-8 sequence.
            ///
            /// This method will consume as many of the input bytes as possible.
            /// </summary>
            /// <param name="source">A span containing a sequence of UTF-8 bytes.</param>
            /// <param name="bytesNeeded">On exit, contains the number of bytes required for encoding from the <paramref name="source"/>.</param>
            /// <returns>A <see cref="OperationStatus"/> value representing the expected state of the conversion.</returns>
            public static OperationStatus ToUtf32Length(ReadOnlySpan<byte> source, out int bytesNeeded)
            {
                if (Utf8Util.GetIndexOfFirstInvalidUtf8Sequence(source, out int scalarCount, out _) < 0)
                {
                    // Well-formed UTF-8 string.
                    // UTF-32 code units are 4 bytes per scalar.
                    bytesNeeded = checked(scalarCount * 4);
                    return OperationStatus.Done;
                }
                else
                {
                    // Not a well-formed UTF-8 string. Perhaps incomplete, perhaps invalid.
                    // Regardless, we can't calculate an answer to return to the caller.
                    // Since this API doesn't have a "bytes consumed" out parameter, the
                    // input argument has an implicit "isFinalChunk = true", so we treat
                    // incomplete strings identically to invalid strings.
                    bytesNeeded = 0;
                    return OperationStatus.InvalidData;
                }
            }

            /// <summary>
            /// Converts a span containing a sequence of UTF-8 bytes into UTF-32 bytes.
            ///
            /// This method will consume as many of the input bytes as possible.
            ///
            /// On successful exit, the entire input was consumed and encoded successfully. In this case, <paramref name="bytesConsumed"/> will be
            /// equal to the length of the <paramref name="source"/> and <paramref name="bytesWritten"/> will equal the total number of bytes written to
            /// the <paramref name="destination"/>.
            /// </summary>
            /// <param name="source">A span containing a sequence of UTF-8 bytes.</param>
            /// <param name="destination">A span to write the UTF-32 bytes into.</param>
            /// <param name="bytesConsumed">On exit, contains the number of bytes that were consumed from the <paramref name="source"/>.</param>
            /// <param name="bytesWritten">On exit, contains the number of bytes written to <paramref name="destination"/></param>
            /// <returns>A <see cref="OperationStatus"/> value representing the state of the conversion.</returns>
            public static OperationStatus ToUtf32(ReadOnlySpan<byte> source, Span<byte> destination, out int bytesConsumed, out int bytesWritten)
            {
                bytesConsumed = 0;
                bytesWritten = 0;

                int srcLength = source.Length;
                int dstLength = destination.Length;
                ref byte src = ref MemoryMarshal.GetReference(source);
                ref byte dst = ref MemoryMarshal.GetReference(destination);

                while (bytesConsumed < srcLength && bytesWritten < dstLength)
                {
                    uint codePoint = Unsafe.Add(ref src, bytesConsumed);

                    int byteCount = EncodingHelper.GetUtf8DecodedBytes((byte)codePoint);
                    if (byteCount == 0)
                        goto InvalidData;
                    if (srcLength - bytesConsumed < byteCount)
                        goto NeedMoreData;

                    if (byteCount > 1)
                        codePoint &= (byte)(0x7F >> byteCount);

                    for (var i = 1; i < byteCount; i++)
                    {
                        ref byte next = ref Unsafe.Add(ref src, bytesConsumed + i);
                        if ((next & EncodingHelper.b1100_0000U) != EncodingHelper.b1000_0000U)
                            goto InvalidData;

                        codePoint = (codePoint << 6) | (uint)(EncodingHelper.b0011_1111U & next);
                    }

                    Unsafe.As<byte, uint>(ref Unsafe.Add(ref dst, bytesWritten)) = codePoint;
                    bytesWritten += 4;
                    bytesConsumed += byteCount;
                }

                return bytesConsumed < srcLength ? OperationStatus.DestinationTooSmall : OperationStatus.Done;

                InvalidData:
                return OperationStatus.InvalidData;

                NeedMoreData:
                return OperationStatus.NeedMoreData;
            }

            #endregion UTF-32 Conversions
        }
    }
}
