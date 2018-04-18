// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System.Text
{
    /// <summary>
    /// Provides low-level methods for reading data directly from Unicode strings.
    /// </summary>
    public static class UnicodeReader
    {
        /// <summary>
        /// Given a UTF-8 input string, returns the first scalar value in the string.
        /// </summary>
        /// <param name="utf8Data">The UTF-8 input string to process.</param>
        /// <returns>
        /// If <paramref name="utf8Data"/> is empty, returns <see cref="SequenceValidity.Empty"/>, and the caller should
        /// not attempt to use the returned <see cref="UnicodeScalar"/> value.
        /// If <paramref name="utf8Data"/> begins with a valid UTF-8 representation of a scalar value, returns
        /// <see cref="SequenceValidity.ValidSequence"/>, the <see cref="UnicodeScalar"/> which appears at the
        /// beginning of the string, and the number of UTF-8 code units required to encode the scalar.
        /// If <paramref name="utf8Data"/> begins with an invalid or incomplete UTF-8 representation of a scalar
        /// value, returns <see cref="SequenceValidity.InvalidSequence"/>, <see cref="UnicodeScalar.ReplacementChar"/>,
        /// and the number of UTF-8 code units that the caller should skip before attempting to read the next scalar.
        /// </returns>
        public static (SequenceValidity status, UnicodeScalar scalar, int charsConsumed) PeekFirstScalar(ReadOnlySpan<Utf8Char> utf8Data)
            => PeekFirstScalarUtf8(ref Unsafe.As<Utf8Char, byte>(ref MemoryMarshal.GetReference(utf8Data)), (uint)utf8Data.Length);

        private static (SequenceValidity status, UnicodeScalar scalar, int charsConsumed) PeekFirstScalarUtf8(ref byte buffer, uint bufferLength)
        {
            // This method is implemented to match the behavior of System.Text.Encoding.UTF8 in terms of
            // how many bytes it consumes when reporting invalid sequences. The behavior is as follows:
            //
            // - Some bytes are *always* invalid (ranges [ C0..C1 ] and [ F5..FF ]), and when these
            //   are encountered it's an invalid sequence of length 1.
            //
            // - Standalone continuation bytes (when the beginning of a sequence is expected) are treated
            //   as an invalid sequence of length 1.
            //
            // - Multi-byte sequences which are overlong are reported as an invalid sequence of length 2,
            //   since per the Unicode Standard Table 3-7 it's always possible to tell these by the second byte.
            //   Exception: Sequences which begin with [ C0..C1 ] are covered by the above case, thus length 1.
            //
            // - Multi-byte sequences which are improperly terminated (no continuation byte when one is
            //   expected) are reported as invalid sequences up to and including the last seen continuation byte.

            uint dataAsDword;

            if (bufferLength >= sizeof(uint))
            {
                dataAsDword = Unsafe.ReadUnaligned<uint>(ref buffer);

                // Quick check for ASCII data before moving on to multi-byte sequence logic

                if (BitConverter.IsLittleEndian && ((int)dataAsDword >= 0))
                {
                    return (SequenceValidity.ValidSequence, UnicodeScalar.DangerousCreateWithoutValidation(dataAsDword >> 24), charsConsumed: 1);
                }
                else if (!BitConverter.IsLittleEndian && ((dataAsDword & 0x80U) == 0))
                {
                    return (SequenceValidity.ValidSequence, UnicodeScalar.DangerousCreateWithoutValidation((byte)dataAsDword), charsConsumed: 1);
                }
            }
            else if (bufferLength > 0)
            {
                uint byte0 = buffer;

                // Quick check for ASCII data before consuming multi-byte sequence

                if ((byte0 & 0x80U) == 0)
                {
                    return (SequenceValidity.ValidSequence, UnicodeScalar.DangerousCreateWithoutValidation(byte0), charsConsumed: 1);
                }

                uint byte1 = (uint)Unsafe.Add(ref buffer, (int)(bufferLength >> 1)) << 8; // or byte0 if length < 2
                uint byte2 = (uint)Unsafe.Add(ref buffer, (int)((bufferLength - 1) & 2)) << 16; // or byte0 if length < 3

                // It's ok for us to reuse the leading byte if we run out of data since the leading byte can never be
                // a valid continuation byte, and we'll eventually catch the error and handle it correctly.

                if (BitConverter.IsLittleEndian)
                {
                    dataAsDword = (byte2 << 16) | (byte1 << 8) | byte0;
                }
                else
                {
                    dataAsDword = (byte0 << 24) | (byte1 << 16) | (byte2 << 8);
                }
            }
            else
            {
                return (SequenceValidity.Empty, default, default);
            }

            // Already handled the ASCII case above, so handle multi-byte sequence case now.
            // We optimize for success cases and move all the error handling to the end of the method.

            // Check for 2-byte sequence.

            if ((BitConverter.IsLittleEndian && UnicodeHelpers.IsInRangeInclusive(dataAsDword & 0xC0FFU, 0x80C2U, 0x80DFU))
                || (!BitConverter.IsLittleEndian && UnicodeHelpers.IsInRangeInclusive(UnicodeHelpers.ROL32(dataAsDword & 0xFFC00000U, 8), 0x800000C2U, 0x800000DFU)))
            {
                // Well-formed 2-byte sequence.
                uint scalarValue;
                if (BitConverter.IsLittleEndian)
                {
                    scalarValue = ((dataAsDword & 0x1FU) << 6) | ((dataAsDword & 0x3F00U) >> 8);
                }
                else
                {
                    scalarValue = ((dataAsDword & 0x1F000000U) >> 18) | ((dataAsDword & 0x3F0000U) >> 16);
                }
                return (SequenceValidity.ValidSequence, UnicodeScalar.DangerousCreateWithoutValidation(scalarValue), charsConsumed: 2);
            }

            // Check for 3-byte sequence.

            if ((BitConverter.IsLittleEndian && ((dataAsDword & 0xC0C0F0U) == 0x8080E0U))
                || (!BitConverter.IsLittleEndian && ((dataAsDword & 0xF0C0C000U) == 0xE0808000U)))
            {
                // Build up the scalar value without validation.

                uint scalarValue;
                if (BitConverter.IsLittleEndian)
                {
                    scalarValue = ((dataAsDword & 0xFU) << 12) | ((dataAsDword & 0x3F00U) >> 2) | ((dataAsDword & 0x3F0000U) >> 16);
                }
                else
                {
                    scalarValue = ((dataAsDword & 0xF000000U) >> 12) | ((dataAsDword & 0x3F0000U) >> 10) | ((dataAsDword & 0x3F00U) >> 8);
                }

                // Now validate the scalar value.
                // It should be between [ U+0800..U+D7FF ], inclusive; or [ U+E000..U+FFFF ], inclusive.
                // Otherwise it's overlong or represents a scalar.
                //
                // n.b. The check below is not correct in the general case because a scalar value of U+10000
                //      would pass, but we're in the 3-byte sequence case so we know our input value (before
                //      validation) is in [ U+0000..U+FFFF ].

                if (UnicodeHelpers.IsInRangeInclusive((scalarValue - 0x800U) ^ 0xD000U, 0x800U, 0xFFFFU))
                {
                    // Well-formed 3-byte sequence.
                    return (SequenceValidity.ValidSequence, UnicodeScalar.DangerousCreateWithoutValidation(scalarValue), charsConsumed: 3);
                }
            }

            // Check for 4-byte sequence.

            if ((BitConverter.IsLittleEndian && ((dataAsDword & 0xC0C0C0F8U) == 0x808080F0U))
                || (!BitConverter.IsLittleEndian && ((dataAsDword & 0xF8C0C0C0U) == 0xF0808080U)))
            {
                // Build up the scalar value without validation.

                uint scalarValue;
                if (BitConverter.IsLittleEndian)
                {
                    scalarValue = ((dataAsDword & 0xFU) << 18) | ((dataAsDword & 0x3F00U) << 4) | ((dataAsDword & 0x3F0000U) >> 10) | ((dataAsDword & 0x3F000000U) >> 24);
                }
                else
                {
                    scalarValue = ((dataAsDword & 0xF000000U) >> 6) | ((dataAsDword & 0x3F0000U) >> 4) | ((dataAsDword & 0x3F00U) >> 2) | (dataAsDword & 0x3FU);
                }

                // Now validate the scalar value.
                // It should be between [ U+10000..U+10FFFF ], inclusive; otherwise it's overlong.

                if (UnicodeHelpers.IsInRangeInclusive(scalarValue, 0x10000U, 0x10FFFFU))
                {
                    // Well-formed 4-byte sequence.
                    return (SequenceValidity.ValidSequence, UnicodeScalar.DangerousCreateWithoutValidation(scalarValue), charsConsumed: 4);
                }
            }

            /*
             * ERROR HANDLING
             */

            // At this point, we know the buffer doesn't represent a well-formed sequence.
            // It's ok for this logic to be somewhat unoptimized since ill-formed buffers should be rare.

            // First, see if the first byte isn't a valid leading byte.
            // If so, we have an invalid sequence of length 1.

            if ((BitConverter.IsLittleEndian && !UnicodeHelpers.IsInRangeInclusive((byte)dataAsDword, 0xC2U, 0xF4U))
                || (!BitConverter.IsLittleEndian && !UnicodeHelpers.IsInRangeInclusive(dataAsDword, 0xC2000000U, 0xF4FFFFFFU)))
            {
                return (SequenceValidity.InvalidSequence, UnicodeScalar.ReplacementChar, charsConsumed: 1);
            }

            // First byte is fine, are we simply lacking further data?
            // If so, we have an incomplete sequence of length 1.

            if (bufferLength < 2)
            {
                return (SequenceValidity.Incomplete, UnicodeScalar.ReplacementChar, charsConsumed: 1);
            }

            // Is the second byte a continuation byte?
            // If not, we have an invalid sequence of length 1.
            // (For this purpose ignore overlong / out-of-range / surrogate sequences.)

            if ((BitConverter.IsLittleEndian && ((dataAsDword & 0x8000U) == 0))
                || (!BitConverter.IsLittleEndian && ((dataAsDword & 0x800000U) == 0)))
            {
                return (SequenceValidity.InvalidSequence, UnicodeScalar.ReplacementChar, charsConsumed: 1);
            }

            // Did the second byte result in an overlong, surrogate, or out-of-range sequence?
            // If so, we have an invalid sequence of length 2.

            uint firstTwoBytesAsBigEndian;
            if (BitConverter.IsLittleEndian)
            {
                firstTwoBytesAsBigEndian = ((uint)(byte)dataAsDword << 8) + (uint)(byte)(dataAsDword >> 8);
            }
            else
            {
                firstTwoBytesAsBigEndian = dataAsDword >> 16;
            }

            if (!UnicodeHelpers.IsInRangeInclusive(firstTwoBytesAsBigEndian, 0xE0A0U, 0xED9FU)
                && !UnicodeHelpers.IsInRangeInclusive(firstTwoBytesAsBigEndian, 0xEE80U, 0xEFBFU)
                && !UnicodeHelpers.IsInRangeInclusive(firstTwoBytesAsBigEndian, 0xF090U, 0xF48FU))
            {
                return (SequenceValidity.InvalidSequence, UnicodeScalar.ReplacementChar, charsConsumed: 2);
            }

            // First two bytes are fine, are we simply lacking further data?
            // If so, we have an incomplete sequence of length 2.

            if (bufferLength < 3)
            {
                return (SequenceValidity.Incomplete, UnicodeScalar.ReplacementChar, charsConsumed: 2);
            }

            // Is the third byte a continuation byte?
            // If not, we have an invalid sequence of length 2.

            if ((BitConverter.IsLittleEndian && ((dataAsDword & 0x800000U) == 0))
              || (!BitConverter.IsLittleEndian && ((dataAsDword & 0x8000U) == 0)))
            {
                return (SequenceValidity.InvalidSequence, UnicodeScalar.ReplacementChar, charsConsumed: 2);
            }

            // First three bytes are fine, are we simply lacking further data?
            // If so, we have an incomplete sequence of length 3.

            if (bufferLength < 4)
            {
                return (SequenceValidity.Incomplete, UnicodeScalar.ReplacementChar, charsConsumed: 3);
            }

            // Only possible remaining option is that the last byte isn't a continuation byte as expected.
            // And so we have an invalid sequence of length 3.

            return (SequenceValidity.InvalidSequence, UnicodeScalar.ReplacementChar, charsConsumed: 3);
        }

        /// <summary>
        /// Given a UTF-16 input string, returns the first scalar value in the string.
        /// </summary>
        /// <param name="utf16Data">The UTF-16 input string to process.</param>
        /// <returns>
        /// If <paramref name="utf16Data"/> is empty, returns <see cref="SequenceValidity.Empty"/>, and the caller should
        /// not attempt to use the returned <see cref="UnicodeScalar"/> value.
        /// If <paramref name="utf16Data"/> begins with a valid UTF-16 representation of a scalar value, returns
        /// <see cref="SequenceValidity.ValidSequence"/>, the <see cref="UnicodeScalar"/> which appears at the
        /// beginning of the string, and the number of UTF-16 code units required to encode the scalar.
        /// If <paramref name="utf16Data"/> begins with an invalid or incomplete UTF-16 representation of a scalar
        /// value, returns <see cref="SequenceValidity.InvalidSequence"/>, <see cref="UnicodeScalar.ReplacementChar"/>,
        /// and the number of UTF-16 code units that the caller should skip before attempting to read the next scalar.
        /// </returns>
        public static (SequenceValidity status, UnicodeScalar scalar, int charsConsumed) PeekFirstScalar(ReadOnlySpan<char> utf16Data)
        {
            if (utf16Data.Length == 0)
            {
                return (SequenceValidity.Empty, default, default);
            }

            uint thisCodePoint = utf16Data[0];
            if (!UnicodeHelpers.IsSurrogateCodePoint(thisCodePoint))
            {
                return (SequenceValidity.ValidSequence, UnicodeScalar.DangerousCreateWithoutValidation(thisCodePoint), charsConsumed: 1);
            }

            // At this point, we know the value is a surrogate, but we haven't determined if it's a high or a low surrogate.

            uint nextCodePoint = (utf16Data.Length >= 2) ? (uint)utf16Data[1] : 0;
            if (!UnicodeHelpers.IsHighSurrogateCodePoint(thisCodePoint) || !UnicodeHelpers.IsLowSurrogateCodePoint(nextCodePoint))
            {
                // Not a (high surrogate, low surrogate) pair.
                return (SequenceValidity.InvalidSequence, UnicodeScalar.ReplacementChar, charsConsumed: 1);
            }

            // At this point, we have a valid surrogate pair.

            return (
                SequenceValidity.ValidSequence,
                UnicodeScalar.DangerousCreateWithoutValidation(UnicodeHelpers.GetScalarFromUtf16SurrogateCodePoints(thisCodePoint, nextCodePoint)),
                charsConsumed: 2);
        }
    }
}
