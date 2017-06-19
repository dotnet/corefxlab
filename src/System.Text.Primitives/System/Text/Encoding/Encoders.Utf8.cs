// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Buffers;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace System.Text
{
    public static partial class Encoders
    {
        public static class Utf8
        {
            #region Tranforms

            public static readonly Transformation FromUtf16 = new Utf16ToUtf8Transform();
            public static readonly Transformation FromUtf32 = new Utf32ToUtf8Transform();

            private sealed class Utf16ToUtf8Transform : Transformation
            {
                public override TransformationStatus Transform(ReadOnlySpan<byte> source, Span<byte> destination, out int bytesConsumed, out int bytesWritten)
                    => ConvertFromUtf16(source, destination, out bytesConsumed, out bytesWritten);
            }

            private sealed class Utf32ToUtf8Transform : Transformation
            {
                public override TransformationStatus Transform(ReadOnlySpan<byte> source, Span<byte> destination, out int bytesConsumed, out int bytesWritten)
                    => ConvertFromUtf32(source, destination, out bytesConsumed, out bytesWritten);
            }

            #endregion Transforms

            #region Utf-16 to Utf-8 conversion

            public static TransformationStatus ComputeEncodedBytesFromUtf16(ReadOnlySpan<byte> source, out int bytesNeeded)
            {
                bytesNeeded = 0;

                // try? because Convert.ConvertToUtf32 can throw
                // if the high/low surrogates aren't valid; no point
                // running all the tests twice per code-point
                try
                {
                    ref char utf16 = ref Unsafe.As<byte, char>(ref source.DangerousGetPinnableReference());
                    int utf16Length = source.Length >> 1; // byte => char count

                    for (int i = 0; i < utf16Length; i++)
                    {
                        var ch = Unsafe.Add(ref utf16, i);

                        if ((ushort)ch <= 0x7f) // Fast path for ASCII
                            bytesNeeded++;
                        else if (!char.IsSurrogate(ch))
                            bytesNeeded += EncodingHelper.GetUtf8EncodedBytes((uint)ch);
                        else
                        {
                            if (++i >= utf16Length)
                                return TransformationStatus.NeedMoreSourceData;

                            uint codePoint = (uint)char.ConvertToUtf32(ch, Unsafe.Add(ref utf16, i));
                            bytesNeeded += EncodingHelper.GetUtf8EncodedBytes(codePoint);
                        }
                    }

                    if ((utf16Length << 1) != source.Length)
                        return TransformationStatus.NeedMoreSourceData;

                    return TransformationStatus.Done;
                }
                catch (ArgumentOutOfRangeException)
                {
                    return TransformationStatus.InvalidData;
                }
            }

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
            /// <returns>A <see cref="TransformationStatus"/> value representing the state of the conversion.</returns>
            public unsafe static TransformationStatus ConvertFromUtf16(ReadOnlySpan<byte> source, Span<byte> destination, out int bytesConsumed, out int bytesWritten)
            {
                //
                //
                // KEEP THIS IMPLEMENTATION IN SYNC WITH https://github.com/dotnet/corert/blob/master/src/System.Private.CoreLib/src/System/Text/UTF8Encoding.cs
                //
                //
                fixed (byte* chars = &source.DangerousGetPinnableReference())
                fixed (byte* bytes = &destination.DangerousGetPinnableReference())
                {
                    char* pSrc = (char*)chars;
                    byte* pTarget = bytes;

                    char* pEnd = (char*)(chars + source.Length);
                    byte* pAllocatedBufferEnd = pTarget + destination.Length;

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
                        int available = Math.Min(EncodingHelper.PtrDiff(pEnd, pSrc), EncodingHelper.PtrDiff(pAllocatedBufferEnd, pTarget));

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
                                if (!EncodingHelper.InRange(ch, EncodingHelper.HighSurrogateStart, EncodingHelper.LowSurrogateEnd))
                                {
                                    // 3 byte encoding
                                    chd = unchecked((sbyte)0xE0) | (ch >> 12);
                                }
                                else
                                {
                                    // 4 byte encoding - high surrogate + low surrogate
                                    // if (!IsHighSurrogate(ch))
                                    if (ch > EncodingHelper.HighSurrogateEnd)
                                    {
                                        // low without high -> bad
                                        goto InvalidData;
                                    }

                                    chd = *pSrc;

                                    // if (!IsLowSurrogate(chd)) {
                                    if (!EncodingHelper.InRange(chd, EncodingHelper.LowSurrogateStart, EncodingHelper.LowSurrogateEnd))
                                    {
                                        // high not followed by low -> bad
                                        goto InvalidData;
                                    }

                                    pSrc++;

                                    ch = chd + (ch << 10) +
                                        (0x10000
                                        - EncodingHelper.LowSurrogateStart
                                        - (EncodingHelper.HighSurrogateStart << 10));

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
                                goto DestinationFull;

                            *pTarget = (byte)ch;
                            pTarget++;
                            continue;
                        }

                        int chd;
                        if (ch <= 0x7FF)
                        {
                            if (pAllocatedBufferEnd - pTarget <= 1)
                                goto DestinationFull;

                            // 2 byte encoding
                            chd = unchecked((sbyte)0xC0) | (ch >> 6);
                        }
                        else
                        {
                            // if (!IsLowSurrogate(ch) && !IsHighSurrogate(ch))
                            if (!EncodingHelper.InRange(ch, EncodingHelper.HighSurrogateStart, EncodingHelper.LowSurrogateEnd))
                            {
                                if (pAllocatedBufferEnd - pTarget <= 2)
                                    goto DestinationFull;

                                // 3 byte encoding
                                chd = unchecked((sbyte)0xE0) | (ch >> 12);
                            }
                            else
                            {
                                if (pAllocatedBufferEnd - pTarget <= 3)
                                    goto DestinationFull;

                                // 4 byte encoding - high surrogate + low surrogate
                                // if (!IsHighSurrogate(ch))
                                if (ch > EncodingHelper.HighSurrogateEnd)
                                {
                                    // low without high -> bad
                                    goto InvalidData;
                                }

                                if (pSrc >= pEnd)
                                    goto NeedMoreData;

                                chd = *pSrc;

                                // if (!IsLowSurrogate(chd)) {
                                if (!EncodingHelper.InRange(chd, EncodingHelper.LowSurrogateStart, EncodingHelper.LowSurrogateEnd))
                                {
                                    // high not followed by low -> bad
                                    goto InvalidData;
                                }

                                pSrc++;

                                ch = chd + (ch << 10) +
                                    (0x10000
                                    - EncodingHelper.LowSurrogateStart
                                    - (EncodingHelper.HighSurrogateStart<< 10));

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

                    bytesConsumed = (int)((byte*)pSrc - chars);
                    bytesWritten = (int)(pTarget - bytes);
                    return TransformationStatus.Done;

                InvalidData:
                    bytesConsumed = (int)((byte*)(pSrc - 1) - chars);
                    bytesWritten = (int)(pTarget - bytes);
                    return TransformationStatus.InvalidData;

                DestinationFull:
                    bytesConsumed = (int)((byte*)(pSrc - 1) - chars);
                    bytesWritten = (int)(pTarget - bytes);
                    return TransformationStatus.DestinationTooSmall;

                NeedMoreData:
                    bytesConsumed = (int)((byte*)(pSrc - 1) - chars);
                    bytesWritten = (int)(pTarget - bytes);
                    return TransformationStatus.NeedMoreSourceData;
                }
            }

            #endregion Utf-16 to Utf-8 conversion

            #region Utf-32 to Utf-8 conversion

            public static TransformationStatus ComputeEncodedBytesFromUtf32(ReadOnlySpan<byte> source, out int bytesNeeded)
            {
                bytesNeeded = 0;

                ref uint utf32 = ref Unsafe.As<byte, uint>(ref source.DangerousGetPinnableReference());
                int utf32Length = source.Length >> 2; // byte => uint count

                for (int i = 0; i < utf32Length; i++)
                {
                    uint codePoint = Unsafe.Add(ref utf32, i);
                    if (!EncodingHelper.IsSupportedCodePoint(codePoint))
                        return TransformationStatus.InvalidData;

                    bytesNeeded += EncodingHelper.GetUtf8EncodedBytes(codePoint);
                }

                if (utf32Length << 2 != source.Length)
                    return TransformationStatus.NeedMoreSourceData;

                return TransformationStatus.Done;
            }

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
            /// <returns>A <see cref="TransformationStatus"/> value representing the state of the conversion.</returns>
            public static TransformationStatus ConvertFromUtf32(ReadOnlySpan<byte> source, Span<byte> destination, out int bytesConsumed, out int bytesWritten)
            {
                bytesConsumed = 0;
                bytesWritten = 0;

                ref byte src = ref source.DangerousGetPinnableReference();
                int srcLength = source.Length;

                ref byte dst = ref destination.DangerousGetPinnableReference();
                int dstLength = destination.Length;

                while (srcLength - bytesConsumed >= sizeof(uint))
                {
                    uint codePoint = Unsafe.As<byte, uint>(ref Unsafe.Add(ref src, bytesConsumed));
                    if (!EncodingHelper.IsSupportedCodePoint(codePoint))
                        return TransformationStatus.InvalidData;

                    int bytesNeeded = EncodingHelper.GetUtf8EncodedBytes(codePoint);
                    if (dstLength - bytesWritten < bytesNeeded)
                        return TransformationStatus.DestinationTooSmall;

                    switch (bytesNeeded)
                    {
                        case 1:
                            Unsafe.Add(ref dst, bytesWritten) = (byte)(EncodingHelper.b0111_1111U & codePoint);
                            break;

                        case 2:
                            Unsafe.Add(ref dst, bytesWritten) = (byte)(((codePoint >> 6) & EncodingHelper.b0001_1111U) | EncodingHelper.b1100_0000U);
                            Unsafe.Add(ref dst, bytesWritten + 1) = (byte)((codePoint & EncodingHelper.b0011_1111U) | EncodingHelper.b1000_0000U);
                            break;

                        case 3:
                            Unsafe.Add(ref dst, bytesWritten) = (byte)(((codePoint >> 12) & EncodingHelper.b0000_1111U) | EncodingHelper.b1110_0000U);
                            Unsafe.Add(ref dst, bytesWritten + 1) = (byte)(((codePoint >> 6) & EncodingHelper.b0011_1111U) | EncodingHelper.b1000_0000U);
                            Unsafe.Add(ref dst, bytesWritten + 2) = (byte)((codePoint & EncodingHelper.b0011_1111U) | EncodingHelper.b1000_0000U);
                            break;

                        case 4:
                            Unsafe.Add(ref dst, bytesWritten) = (byte)(((codePoint >> 18) & EncodingHelper.b0000_0111U) | EncodingHelper.b1111_0000U);
                            Unsafe.Add(ref dst, bytesWritten + 1) = (byte)(((codePoint >> 12) & EncodingHelper.b0011_1111U) | EncodingHelper.b1000_0000U);
                            Unsafe.Add(ref dst, bytesWritten + 2) = (byte)(((codePoint >> 6) & EncodingHelper.b0011_1111U) | EncodingHelper.b1000_0000U);
                            Unsafe.Add(ref dst, bytesWritten + 3) = (byte)((codePoint & EncodingHelper.b0011_1111U) | EncodingHelper.b1000_0000U);
                            break;

                        default:
                            return TransformationStatus.InvalidData;
                    }

                    bytesConsumed += 4;
                    bytesWritten += bytesNeeded;
                }

                return bytesConsumed < srcLength ? TransformationStatus.NeedMoreSourceData : TransformationStatus.Done;
            }

            #endregion Utf-32 to Utf-8 conversion
        }
    }
}
