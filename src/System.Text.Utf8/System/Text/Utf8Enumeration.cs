// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Runtime.CompilerServices;

namespace System.Text
{
    // These APIs are very similar to UnicodeReader.PeekFirstScalar, but they're optimized for enumeration
    // over mostly ASCII sequences. They also don't explicitly report invalid subsequences, instead normalizing
    // invalid subsequences to the U+FFFD replacement character.

    internal static class Utf8Enumeration
    {
        /// <summary>
        /// Returns the first Unicode scalar value from a UTF-8 string. If the input is malformed,
        /// returns U+FFFD and the number of invalid code units to skip. If the input is empty, returns U+0000.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint ReadFirstScalar(ReadOnlySpan<byte> input, out int bytesConsumed)
        {
            bytesConsumed = 0;
            uint retVal = 0;

            if (input.Length > 0)
            {
                // Optimistically assume ASCII data.
                bytesConsumed = 1;
                retVal = input[0];
                if (retVal > 0x7FU)
                {
                    // Turns out the assumption was wrong.
                    retVal = ReadFirstScalarSlow(input, retVal, out bytesConsumed);
                }
            }

            return retVal;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static uint ReadFirstScalarSlow(ReadOnlySpan<byte> input, uint firstByte, out int bytesConsumed)
        {
            if (input.Length < 2)
            {
                goto BadData; // not enough data for multi-byte sequence, which means first byte invaild
            }

            uint secondByte = input[1];
            if (!UnicodeHelpers.IsUtf8ContinuationByte(secondByte))
            {
                goto BadData; // expected a continuation byte
            }

            uint scalar = (firstByte << 6) + secondByte; // n.b. still has UTF-8 header bits at this point
            if (UnicodeHelpers.IsInRangeInclusive(scalar, 0x80U + 0x3080U, 0x7FFU + 0x3080U))
            {
                // Valid 2-byte sequence, not overlong.
                bytesConsumed = 2;
                return scalar - 0x3080U; // remove UTF-8 header bits
            }

            // At this point, not a valid 2-byte sequence. Try 3-byte.

            if (input.Length < 3)
            {
                goto BadData;
            }

            uint thirdByte = input[2];
            if (!UnicodeHelpers.IsUtf8ContinuationByte(thirdByte))
            {
                goto BadData; // expected a continuation byte
            }

            scalar = (scalar << 6) + thirdByte; // still has UTF-8 header bits at this point
            if (UnicodeHelpers.IsInRangeInclusive(scalar, 0x800U + 0xE2080U, 0xFFFFU + 0xE2080U)
                && !UnicodeHelpers.IsSurrogateCodePoint(scalar - 0xE2080U))
            {
                // Valid 3-byte sequence, not overlong or surrogate
                bytesConsumed = 3;
                return scalar - 0xE2080U;
            }

            // At this point, not a valid 3-byte sequence. Try 4-byte.

            if (input.Length < 4)
            {
                goto BadData;
            }

            uint fourthByte = input[3];
            if (!UnicodeHelpers.IsUtf8ContinuationByte(fourthByte))
            {
                goto BadData; // expected a continuation byte
            }

            scalar = (scalar << 6) + fourthByte; // still has UTF-8 header bits at this point
            if (UnicodeHelpers.IsInRangeInclusive(scalar, 0x10000U + 0x3C82080U, 0x10FFFFU + 0x3C82080U))
            {
                // Value 4-byte sequence, not overlong or out-of-range
                bytesConsumed = 4;
                return scalar - 0x3C82080U;
            }

        BadData:

            // If we reached this point, we hit a bad / incomplete sequence in the input.
            // Need to figure out what the exact problem was so we know how many bytes to skip.

            bytesConsumed = GetNumberOfBytesToSkipForInvalidSequence(input);
            return UnicodeHelpers.ReplacementChar;
        }

        /// <summary>
        /// Returns the last Unicode scalar value from a UTF-8 string. If the input is malformed,
        /// returns U+FFFD and the number of invalid code units to skip. If the input is empty, returns U+0000.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint ReadLastScalar(ReadOnlySpan<byte> input, out int bytesConsumed)
        {
            bytesConsumed = 0;
            uint retVal = 0;

            if (input.Length > 0)
            {
                // Optimistically assume ASCII data.
                bytesConsumed = 1;
                retVal = input[input.Length - 1];
                if (retVal > 0x7FU)
                {
                    // Turns out the assumption was wrong.
                    retVal = ReadLastScalarSlow(input, retVal, out bytesConsumed);
                }
            }

            return retVal;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static uint ReadLastScalarSlow(ReadOnlySpan<byte> input, uint lastByte, out int bytesConsumed)
        {
            if (input.Length < 2)
            {
                goto BadData; // not enough data for multi-byte sequence, which means last byte invaild
            }

            if (!UnicodeHelpers.IsUtf8ContinuationByte(lastByte))
            {
                goto BadData; // expected a continuation byte
            }

            uint scalar = ((uint)input[input.Length - 2] << 6) + lastByte; // n.b. still has UTF-8 header bits at this point
            if (UnicodeHelpers.IsInRangeInclusive(scalar, 0x80U + 0x3080U, 0x7FFU + 0x3080U))
            {
                // Valid 2-byte sequence, not overlong.
                bytesConsumed = 2;
                return scalar - 0x3080U; // remove UTF-8 header bits
            }

            // At this point, not a valid standalone 2-byte sequence. Make sure they're both continuation bytes.
            // Then try a 3-byte sequence.

            if (!UnicodeHelpers.IsInRangeInclusive(scalar, 0U + 0x2080U, 0xFFFU + 0x2080U) || input.Length < 3)
            {
                goto BadData; // observed a non-continuation byte or ran out of data to backtrack
            }

            scalar += (uint)input[input.Length - 3] << 6; // still has UTF-8 header bits at this point
            if (UnicodeHelpers.IsInRangeInclusive(scalar, 0x800U + 0xE2080U, 0xFFFFU + 0xE2080U)
                && !UnicodeHelpers.IsSurrogateCodePoint(scalar - 0xE2080U))
            {
                // Valid 3-byte sequence, not overlong or surrogate
                bytesConsumed = 3;
                return scalar - 0xE2080U;
            }

            // At this point, not a valid standalone 3-byte sequence. Make sure they're all continuation bytes.
            // Then try a 4-byte sequence.

            if (!UnicodeHelpers.IsInRangeInclusive(scalar, 0U + 0x82080U, 0x3FFFFU + 0x82080U) || input.Length < 4)
            {
                goto BadData; // observed a non-continuation byte or ran out of data to backtrack
            }

            scalar += (uint)input[input.Length - 4] << 6; // still has UTF-8 header bits at this point
            if (UnicodeHelpers.IsInRangeInclusive(scalar, 0x10000U + 0x3C82080U, 0x10FFFFU + 0x3C82080U))
            {
                // Value 4-byte sequence, not overlong or out-of-range
                bytesConsumed = 4;
                return scalar - 0x3C82080U;
            }

        BadData:

            // If we reached this point, there's not a valid scalar sequence at the end of the input.
            // In order to keep forward and reverse enumeration consistent with respect to each other,
            // we need to (from the end) find the longest possible "invalid scalar" sequence that isn't
            // itself a subsequence of a longer "invalid scalar" sequence. Since invalid sequences can
            // be at most length 3 bytes, we start reading *forward* near the end of the input buffer
            // to see what the last forward-iterated sequence would have been.

            if (input.Length > 3)
            {
                input = input.Slice(input.Length - 3);
            }

            while (input.Length > 1)
            {
                ReadFirstScalar(input, out int innerBytesConsumed); // ignore whatever scalar we got back
                if (innerBytesConsumed == input.Length)
                {
                    break; // This is the longest possible sequence.
                }

                input = input.Slice(innerBytesConsumed);
            }

            bytesConsumed = input.Length;
            return UnicodeHelpers.ReplacementChar;
        }

        private static int GetNumberOfBytesToSkipForInvalidSequence(ReadOnlySpan<byte> input)
        {
            /*
             * ERROR HANDLING
             */

            // At this point, we know the buffer doesn't represent a well-formed sequence.
            // It's ok for this logic to be somewhat unoptimized since ill-formed buffers should be rare.

            if (input.Length < 2)
            {
                return input.Length; // empty or single non-ASCII byte
            }

            // Is the second byte a continuation byte?
            // If not, we have an invalid sequence of length 1.
            // (For this purpose ignore overlong / out-of-range / surrogate sequences.)

            if (!UnicodeHelpers.IsUtf8ContinuationByte(input[1]))
            {
                return 1;
            }

            // If only two bytes available, treat the entire subsequence as invalid.

            if (input.Length < 3)
            {
                return 2;
            }

            // Before checking whether the third byte is a proper continuation byte,
            // check for overlong, surrogate, or out-of-range sequence.
            // If so, we have an invalid sequence of length 2.

            uint firstTwoBytes = ((uint)input[0]) << 8 | input[1];

            if (!UnicodeHelpers.IsInRangeInclusive(firstTwoBytes, 0xE0A0U, 0xED9FU)
                && !UnicodeHelpers.IsInRangeInclusive(firstTwoBytes, 0xEE80U, 0xEFBFU)
                && !UnicodeHelpers.IsInRangeInclusive(firstTwoBytes, 0xF090U, 0xF48FU))
            {
                return 2;
            }

            // At this point, we know the first two bytes are ok and that there are at
            // least three bytes total in the subsequence under inspection. Any continuation
            // byte is valid for the third and fourth byte of a UTF-8 sequence, so the only
            // remaining cases are that the third byte was not a continuation byte (and so
            // the invalid subsequence has length 2), or the third byte is a continuation byte
            // but the fourth byte is not (and so the invalid subsequence has length 3).

            return UnicodeHelpers.IsUtf8ContinuationByte(input[2]) ? 3 : 2;
        }
    }
}
