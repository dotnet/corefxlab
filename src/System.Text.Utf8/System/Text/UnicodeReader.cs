// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers.Binary;
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
        public static (SequenceValidity status, UnicodeScalar scalar, int charsConsumed) PeekFirstScalar(ReadOnlySpan<Utf8Char> utf8Data) => PeekFirstScalarUtf8(MemoryMarshal.Cast<Utf8Char, byte>(utf8Data));

        private static (SequenceValidity status, UnicodeScalar scalar, int charsConsumed) PeekFirstScalarUtf8(ReadOnlySpan<byte> buffer)
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

            if (BinaryPrimitives.TryReadUInt32LittleEndian(buffer, out uint fourBytes))
            {
                return PeekFirstScalarUtf8Fast(fourBytes);
            }
            else
            {
                return PeekFirstScalarUtf8Slow(buffer);
            }
        }

        private static (SequenceValidity status, UnicodeScalar scalar, int charsConsumed) PeekFirstScalarUtf8Fast(uint fourBytes)
        {
            // Quick check for ASCII data.

            if ((fourBytes & 0x80U) == 0)
            {
                uint scalarValue = fourBytes & 0xFFU;
                return (SequenceValidity.ValidSequence, UnicodeScalar.DangerousCreateWithoutValidation(scalarValue), charsConsumed: 1);
            }

            // Check for two-byte sequence.

            uint maskedLeadingByteAndContinuationBytes = fourBytes & 0xC0FFU;
            if (UnicodeHelpers.IsInRangeInclusive(maskedLeadingByteAndContinuationBytes, 0x80C2U, 0x80DFU)) // overlong + continuation byte check all in one
            {
                fourBytes >>= 8; // strip off leading byte
                uint scalarValue = (maskedLeadingByteAndContinuationBytes << 6) + (uint)(byte)fourBytes - 0x80U /* continuation byte marker */ - (0x80C0U << 6) /* mask marker, shifted */;
                return (SequenceValidity.ValidSequence, UnicodeScalar.DangerousCreateWithoutValidation(scalarValue), charsConsumed: 2);
            }

            // Check for three-byte sequence.

            maskedLeadingByteAndContinuationBytes = fourBytes & 0xC0C0FFU;
            if (UnicodeHelpers.IsInRangeInclusive(maskedLeadingByteAndContinuationBytes, 0x8080E0U, 0x8080EFU)) // continuation byte check, not overlong or surrogate check
            {
                fourBytes >>= 8; // strip off leading byte
                uint scalarValue = (maskedLeadingByteAndContinuationBytes << 12) + ((uint)(byte)fourBytes << 6) + (uint)(byte)(fourBytes >> 8) - 0x80U /* continuation byte marker */ - (0x80U << 6) /* continuation byte marker, shifted */ - (0x8080E0U << 12) /* mask marker, shifted */;
                if (UnicodeHelpers.IsInRangeInclusive((scalarValue - 0x800U) ^ 0xD000U, 0x800U, 0xFFFFU)) // overlong and surrogate check
                {
                    return (SequenceValidity.ValidSequence, UnicodeScalar.DangerousCreateWithoutValidation(scalarValue), charsConsumed: 3);
                }
                else
                {
                    goto OverlongSurrogateOrOutOfRange;
                }
            }

            // Check for four-byte sequence.

            maskedLeadingByteAndContinuationBytes = fourBytes & 0xC0C0C0FFU;
            if (UnicodeHelpers.IsInRangeInclusive(maskedLeadingByteAndContinuationBytes, 0x808080F0U, 0x808080F4U))
            {
                // n.b. presence of >>= operators in line below; order of operations matters
                uint scalarValue = (maskedLeadingByteAndContinuationBytes << 18) + ((uint)(byte)(fourBytes >>= 8) << 12) + ((uint)(byte)(fourBytes >>= 8) << 6) + (uint)(byte)(fourBytes >> 8) - 0x80U /* continuation byte marker */- (0x80U << 6) /* continuation byte marker, shifted */- (0x80U << 12) /* continuation byte marker, shifted */- (0x808080F0U << 18) /* mask marker, shifted */;
                if (UnicodeHelpers.IsInRangeInclusive(scalarValue, 0x10000U, 0x10FFFFU)) // overlong and out-of-range check
                {
                    return (SequenceValidity.ValidSequence, UnicodeScalar.DangerousCreateWithoutValidation(scalarValue), charsConsumed: 4);
                }
                else
                {
                    goto OverlongSurrogateOrOutOfRange;
                }
            }

            /*
             * ERROR HANDLING
             */

            // Need to figure out why this value is invalid so that we can return the appropriate number of code units to skip.
            // Overlong, out-of-range, and surrogates are handled at the end of the method.
            // It's impossible to have incomplete sequences when the buffer has at least four bytes in it (as is the case here).

            if (!UnicodeHelpers.IsInRangeInclusive(fourBytes & 0xC0FFU, 0x80C2U, 0x80F4U))
            {
                // Leading byte is a standalone continuation byte, or
                // Leading byte is never valid in UTF-8, or
                // Leading byte is a valid multi-byte sequence start, but it's not followed by a continuation byte.
                return (SequenceValidity.InvalidSequence, UnicodeScalar.ReplacementChar, charsConsumed: 1);
            }

            if ((fourBytes & 0xC00000U) != 0x800000U)
            {
                // Saw a valid two-byte prefix of a longer sequence, but the third byte isn't a continuation byte.
                return (SequenceValidity.InvalidSequence, UnicodeScalar.ReplacementChar, charsConsumed: 2);
            }

            // The only possible error condition left is that we saw a valid three-byte prefix of a longer sequence, but the fourth byte isn't a continuation byte.
            // It's not possible to have an invalid sequence of length 4 per the rules we set for ourselves.
            return (SequenceValidity.InvalidSequence, UnicodeScalar.ReplacementChar, charsConsumed: 3);

            OverlongSurrogateOrOutOfRange:

            // Overlong (e.g., [ E0 80 ]), surrogate (e.g., [ ED A0 ]), or out-of-range (e.g., [ F4 90 ]) sequence.
            return (SequenceValidity.InvalidSequence, UnicodeScalar.ReplacementChar, charsConsumed: 2);
        }

        private static (SequenceValidity status, UnicodeScalar scalar, int charsConsumed) PeekFirstScalarUtf8Slow(ReadOnlySpan<byte> buffer)
        {
            if (buffer.Length == 0)
            {
                return (SequenceValidity.Empty, default, default);
            }

            // First, check for ASCII.

            uint currentScalar = buffer[0];

            if (UnicodeHelpers.IsAsciiCodePoint(currentScalar))
            {
                return (SequenceValidity.ValidSequence, UnicodeScalar.DangerousCreateWithoutValidation(currentScalar), charsConsumed: 1);
            }

            // Not ASCII, go down multi-byte sequence path.
            // This is optimized for the success case; failure cases are handled at the bottom of the method.

            // Check for 2-byte sequence.

            if (buffer.Length < 2)
            {
                goto Error; // out of data
            }

            uint nextByte = buffer[1]; // optimistically assume for now it's a valid continuation byte
            currentScalar = (currentScalar << 6) + nextByte - 0x80U /* remove continuation byte marker */ - 0x3000U /* remove first byte header */ ;

            if (UnicodeHelpers.IsInRangeInclusive(currentScalar, 0x80U, 0x7FFU) && UnicodeHelpers.IsUtf8ContinuationByte(nextByte))
            {
                // Valid 2-byte sequence.
                return (SequenceValidity.ValidSequence, UnicodeScalar.DangerousCreateWithoutValidation(currentScalar), charsConsumed: 2);
            }

            // Check for 3-byte sequence.

            if (buffer.Length < 3)
            {
                goto Error; // out of data
            }

            uint continuationByteAccumulator = nextByte - 0x80U; // bits 6 and 7 should never be set
            nextByte = (uint)buffer[2] - 0x80U; // optimistically assume for now it's a valid continuation byte
            continuationByteAccumulator |= nextByte;
            currentScalar = (currentScalar << 6) + nextByte + 0xC0000U - 0xE0000U /* fix first byte header */;

            if (UnicodeHelpers.IsInRangeInclusive(currentScalar, 0x800U, 0xFFFFU) && !UnicodeHelpers.IsSurrogateCodePoint(currentScalar) && ((continuationByteAccumulator & 0xC0U) == 0))
            {
                // Valid 3-byte sequence.
                return (SequenceValidity.ValidSequence, UnicodeScalar.DangerousCreateWithoutValidation(currentScalar), charsConsumed: 3);
            }

            // Check for 4-byte sequence.

            if (buffer.Length < 4)
            {
                goto Error; // out of data
            }

            nextByte = (uint)buffer[3] - 0x80U; // optimistically assume for now it's a valid continuation byte
            continuationByteAccumulator |= nextByte;
            currentScalar = (currentScalar << 6) + nextByte + 0x3800000U - 0x3C00000U /* fix first byte header */;

            if (UnicodeHelpers.IsInRangeInclusive(currentScalar, 0x10000U, 0x10FFFFU) && ((continuationByteAccumulator & 0xC0U) == 0))
            {
                // Valid 4-byte sequence.
                return (SequenceValidity.ValidSequence, UnicodeScalar.DangerousCreateWithoutValidation(currentScalar), charsConsumed: 4);
            }

            Error:

            /*
             * ERROR HANDLING
             */

            // At this point, we know the buffer doesn't represent a well-formed sequence.
            // It's ok for this logic to be somewhat unoptimized since ill-formed buffers should be rare.

            // First, see if the first byte isn't a valid sequence start byte.
            // If so, we have an invalid sequence of length 1.

            if (!UnicodeHelpers.IsInRangeInclusive(buffer[0], 0xC2U, 0xF4U))
            {
                return (SequenceValidity.InvalidSequence, UnicodeScalar.ReplacementChar, charsConsumed: 1);
            }

            // First byte is fine, are we simply lacking further data?
            // If so, we have an incomplete sequence of length 1.

            if (buffer.Length < 2)
            {
                return (SequenceValidity.Incomplete, UnicodeScalar.ReplacementChar, charsConsumed: 1);
            }

            // Is the second byte a continuation byte?
            // If not, we have an invalid sequence of length 1.
            // (For this purpose ignore overlong / out-of-range / surrogate sequences.)

            if ((buffer[1] & 0xC0U) != 0x80U)
            {
                return (SequenceValidity.InvalidSequence, UnicodeScalar.ReplacementChar, charsConsumed: 1);
            }

            // Did the second byte result in an overlong, surrogate, or out-of-range sequence?
            // If so, we have an invalid sequence of length 2.

            uint firstTwoBytes = ((uint)buffer[0]) << 8 | buffer[1];

            if (!UnicodeHelpers.IsInRangeInclusive(firstTwoBytes, 0xE0A0U, 0xED9FU)
                && !UnicodeHelpers.IsInRangeInclusive(firstTwoBytes, 0xEE80U, 0xEFBFU)
                && !UnicodeHelpers.IsInRangeInclusive(firstTwoBytes, 0xF090U, 0xF48FU))
            {
                return (SequenceValidity.InvalidSequence, UnicodeScalar.ReplacementChar, charsConsumed: 2);
            }

            // First two bytes are fine, are we simply lacking further data?
            // If so, we have an incomplete sequence of length 2.

            if (buffer.Length < 3)
            {
                return (SequenceValidity.Incomplete, UnicodeScalar.ReplacementChar, charsConsumed: 2);
            }

            // Is the third byte a continuation byte?
            // If not, we have an invalid sequence of length 2.

            if ((buffer[2] & 0xC0U) != 0x80U)
            {
                return (SequenceValidity.InvalidSequence, UnicodeScalar.ReplacementChar, charsConsumed: 2);
            }

            // First three bytes are fine, are we simply lacking further data?
            // If so, we have an incomplete sequence of length 3.

            if (buffer.Length < 4)
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
