// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace System.Buffers.Text
{
    /// <summary>
    /// Contains utility methods for inspecting UTF-8 sequences.
    /// </summary>
    /// <remarks>
    /// The UTF-8 specification can be found in the Unicode Standard, Sec. 3.9.
    /// http://www.unicode.org/versions/Unicode10.0.0/ch03.pdf
    /// </remarks>
    public static class Utf8Utility
    {
        private static readonly byte[] _replacementCharAsUtf8 = new byte[] { 0xEF, 0xBF, 0xBD };

        /// <summary>
        /// The Unicode Replacement Character (U+FFFD) as the three-byte UTF-8 sequence [ EF BF BD ].
        /// </summary>
        public static ReadOnlySpan<byte> ReplacementCharacterByteSequence => _replacementCharAsUtf8;

        /// <summary>
        /// Given the first byte of a sequence, returns the expected number of continuation bytes
        /// which should follow this byte.
        /// </summary>
        /// <remarks>
        /// Returns 0 if <paramref name="firstByte"/> does not have any expected continuation bytes
        /// or if <paramref name="firstByte"/> cannot begin a well-formed UTF-8 sequence.
        /// </remarks>
        public static int GetExpectedNumberOfContinuationBytes(byte firstByte)
        {
            if (firstByte < (byte)0xC2)
            {
                // ASII (one-byte sequence), or
                // continuation byte (invalid as first byte, hence no expected followers), or
                // [ C0..C1 ] (always invalid UTF-8 byte, hence no expected followers)
                return 0;
            }
            else if (firstByte < (byte)0xE0)
            {
                // [ C2..DF ] can start a two-byte sequence
                return 1;
            }
            else if (firstByte < (byte)0xF0)
            {
                // [ E0..EF ] can start a three-byte sequence
                return 2;
            }
            else if (firstByte <= (byte)0xF4)
            {
                // [ F0..F4 ] can start a four-byte sequence
                return 3;
            }
            else
            {
                // [ F5 .. FF ] (always invalid UTF-8 byte, hence no expected followers)
                return 0;
            }
        }

        /// <summary>
        /// Returns the index of the first byte of the first invalid UTF-8 sequence in <paramref name="data"/>,
        /// or -1 if <paramref name="data"/> is a well-formed UTF-8 string.
        /// </summary>
        public static int GetIndexOfFirstInvalidUtf8Sequence(ReadOnlySpan<byte> data) =>
            Utf8Util.GetIndexOfFirstInvalidUtf8Sequence(data, out _, out _);

        /// <summary>
        /// Return <see langword="true"/> iff <paramref name="value"/> is an ASCII value (within the range 0-127, inclusive).
        /// </summary>
        public static bool IsAsciiValue(byte value) => (value < (byte)0x80);

        /// <summary>
        /// Returns <see langword="true"/> iff <paramref name="value"/> is a UTF-8 continuation byte.
        /// A UTF-8 continuation byte is a byte whose value is in the range 0x80-0xBF, inclusive.
        /// </summary>
        public static bool IsUtf8ContinuationByte(byte value) => ((value & (byte)0xC0) == (byte)0x80);

        /// <summary>
        /// Returns <see langword="true"/> iff <paramref name="data"/> represents a well-formed UTF-8 string.
        /// </summary>
        public static bool IsWellFormedUtf8String(ReadOnlySpan<byte> data) => (GetIndexOfFirstInvalidUtf8Sequence(data) < 0);

        /// <summary>
        /// Peeks at the first UTF-8 sequence in the input buffer and returns information about that
        /// sequence. If the sequence is well-formed, returns <see cref="SequenceValidity.WellFormed"/>
        /// and sets the <paramref name="scalarValue"/> output parameter to the scalar value encoded by
        /// the sequence. If the return value is anything other than <see cref="SequenceValidity.WellFormed"/>,
        /// sets the <paramref name="scalarValue"/> output parameter to <see cref="UnicodeScalar.ReplacementChar"/>.
        /// In all cases, the <paramref name="numBytesConsumed"/> output parameter will contain the
        /// number of UTF-8 code units read from the input buffer in order to make the determination.
        /// </summary>
        public static SequenceValidity PeekFirstSequence(ReadOnlySpan<byte> data, out int numBytesConsumed, out UnicodeScalar scalarValue)
        {
            // This method is implemented to match the behavior of System.Text.Encoding.UTF8 in terms of
            // how many bytes it consumes when reporting invalid sequences. The behavior is as follows:
            //
            // - Some bytes are *always* invalid (ranges [ C0..C1 ] and [ F5..FF ]), and when these
            //   are encountered it's an invalid sequence of length 1.
            //
            // - Multi-byte sequences which are overlong are reported as an invalid sequence of length 2,
            //   since per the Unicode Standard Table 3-7 it's always possible to tell these by the second byte.
            //   Exception: Sequences which begin with [ C0..C1 ] are covered by the above case, thus length 1.
            //
            // - Multi-byte sequences which are improperly terminated (no continuation byte when one is
            //   expected) are reported as invalid sequences up to and including the last seen continuation byte.

            scalarValue = UnicodeScalar.ReplacementChar;

            if (data.IsEmpty)
            {
                // No data to peek at
                numBytesConsumed = 0;
                return SequenceValidity.Empty;
            }

            byte firstByte = data[0];

            if (IsAsciiValue(firstByte))
            {
                // ASCII byte = well-formed one-byte sequence.
                scalarValue = UnicodeScalar.CreateWithoutValidation(firstByte);
                numBytesConsumed = 1;
                return SequenceValidity.WellFormed;
            }

            if (!Utf8Util.IsInRangeInclusive(firstByte, (byte)0xC2U, (byte)0xF4U))
            {
                // Standalone continuation byte or "always invalid" byte = ill-formed one-byte sequence.
                goto InvalidOneByteSequence;
            }

            // At this point, we know we're working with a multi-byte sequence,
            // and we know that at least the first byte is potentially valid.

            if (data.Length < 2)
            {
                // One byte of an incomplete multi-byte sequence.
                goto OneByteOfIncompleteMultiByteSequence;
            }

            byte secondByte = data[1];

            if (!IsUtf8ContinuationByte(secondByte))
            {
                // One byte of an improperly terminated multi-byte sequence.
                goto InvalidOneByteSequence;
            }

            if (firstByte < (byte)0xE0U)
            {
                // Well-formed two-byte sequence.
                scalarValue = UnicodeScalar.CreateWithoutValidation((((uint)firstByte & 0x1FU) << 6) | ((uint)secondByte & 0x3FU));
                numBytesConsumed = 2;
                return SequenceValidity.WellFormed;
            }

            if (firstByte < (byte)0xF0U)
            {
                // Start of a three-byte sequence.
                // Need to check for overlong or surrogate sequences.

                uint scalar = (((uint)firstByte & 0x0FU) << 12) | (((uint)secondByte & 0x3FU) << 6);
                if (scalar < 0x800U || Utf8Util.IsLowWordSurrogate(scalar)) { goto OverlongOutOfRangeOrSurrogateSequence; }

                // At this point, we have a valid two-byte start of a three-byte sequence.

                if (data.Length < 3)
                {
                    // Two bytes of an incomplete three-byte sequence.
                    goto TwoBytesOfIncompleteMultiByteSequence;
                }
                else
                {
                    byte thirdByte = data[2];
                    if (IsUtf8ContinuationByte(thirdByte))
                    {
                        // Well-formed three-byte sequence.
                        scalar |= (uint)thirdByte & 0x3FU;
                        scalarValue = UnicodeScalar.CreateWithoutValidation(scalar);
                        numBytesConsumed = 3;
                        return SequenceValidity.WellFormed;
                    }
                    else
                    {
                        // Two bytes of improperly terminated multi-byte sequence.
                        goto InvalidTwoByteSequence;
                    }
                }
            }

            {
                // Start of four-byte sequence.
                // Need to check for overlong or out-of-range sequences.

                uint scalar = (((uint)firstByte & 0x07U) << 18) | (((uint)secondByte & 0x3FU) << 12);
                if (!Utf8Util.IsInRangeInclusive(scalar, 0x10000U, 0x10FFFFU)) { goto OverlongOutOfRangeOrSurrogateSequence; }

                // At this point, we have a valid two-byte start of a four-byte sequence.

                if (data.Length < 3)
                {
                    // Two bytes of an incomplete four-byte sequence.
                    goto TwoBytesOfIncompleteMultiByteSequence;
                }
                else
                {
                    byte thirdByte = data[2];
                    if (IsUtf8ContinuationByte(thirdByte))
                    {
                        // Valid three-byte start of a four-byte sequence.

                        if (data.Length < 4)
                        {
                            // Three bytes of an incomplete four-byte sequence.
                            goto ThreeBytesOfIncompleteMultiByteSequence;
                        }
                        else
                        {
                            byte fourthByte = data[3];
                            if (IsUtf8ContinuationByte(fourthByte))
                            {
                                // Well-formed four-byte sequence.
                                scalar |= (((uint)thirdByte & 0x3FU) << 6) | ((uint)fourthByte & 0x3FU);
                                scalarValue = UnicodeScalar.CreateWithoutValidation(scalar);
                                numBytesConsumed = 4;
                                return SequenceValidity.WellFormed;
                            }
                            else
                            {
                                // Three bytes of an improperly terminated multi-byte sequence.
                                goto InvalidThreeByteSequence;
                            }
                        }
                    }
                    else
                    {
                        // Two bytes of improperly terminated multi-byte sequence.
                        goto InvalidTwoByteSequence;
                    }
                }
            }

            // Everything below here is error handling.

            InvalidOneByteSequence:
            numBytesConsumed = 1;
            return SequenceValidity.Invalid;

            InvalidTwoByteSequence:
            OverlongOutOfRangeOrSurrogateSequence:
            numBytesConsumed = 2;
            return SequenceValidity.Invalid;

            InvalidThreeByteSequence:
            numBytesConsumed = 3;
            return SequenceValidity.Invalid;

            OneByteOfIncompleteMultiByteSequence:
            numBytesConsumed = 1;
            return SequenceValidity.Incomplete;

            TwoBytesOfIncompleteMultiByteSequence:
            numBytesConsumed = 2;
            return SequenceValidity.Incomplete;

            ThreeBytesOfIncompleteMultiByteSequence:
            numBytesConsumed = 3;
            return SequenceValidity.Incomplete;
        }

        /// <summary>
        /// Calculates the number of UTF-16 code units (<see cref="char"/>s) that would result from converting
        /// the provided UTF-8 string to UTF-16 representation.
        /// </summary>
        /// <param name="inputBuffer">The buffer containing UTF-8 text.</param>
        /// <param name="charCount">
        /// If this method returns <see langword="true"/>, contains the equivalent <see cref="char"/> count of the input string.
        /// If this method returns <see langword="false"/>, the value is undefined.
        /// </param>
        /// <returns>
        /// <see langword="true"/> on success, <see langword="false"/> if the input is not a well-formed UTF-8 string.
        /// </returns>
        public static bool TryGetUtf16CharCount(ReadOnlySpan<byte> inputBuffer, out int charCount)
        {
            if (Utf8Util.GetIndexOfFirstInvalidUtf8Sequence(inputBuffer, out int runeCount, out int surrogatePairCount) < 0)
            {
                // can't overflow because UTF-16 code unit count is always <= UTF-8 code unit count for well-formed strings
                charCount = runeCount + surrogatePairCount;
                return true;
            }
            else
            {
                charCount = default;
                return false;
            }
        }

        /// <summary>
        /// Attempts to read the first rune (24-bit scalar value) from the provided UTF-8 sequence.
        /// </summary>
        /// <param name="rune">
        /// If this method returns <see langword="true"/>, contains the scalar value that appears at the beginning of the input buffer.
        /// If this method returns <see langword="false"/>, the value is undefined.
        /// </param>
        /// <param name="bytesConsumed">
        /// If this method returns <see langword="true"/>, contains the number of bytes of the input buffer which were consumed in order to generate this scalar value.
        /// If this method returns <see langword="false"/>, the value is undefined.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if a scalar value could be decoded from the beginning of the input buffer,
        /// <see langword="false"/> if the input buffer did not begin with a valid UTF-8 sequence.
        /// </returns>
        public static bool TryReadFirstRune(ReadOnlySpan<byte> inputBuffer, out int rune, out int bytesConsumed)
        {
            if (PeekFirstSequence(inputBuffer, out bytesConsumed, out var scalar) == SequenceValidity.WellFormed)
            {
                rune = scalar.Value;
                return true;
            }

            // Failure case

            rune = default;
            bytesConsumed = default;
            return false;
        }

        /// <summary>
        /// Attempts to read the first rune (24-bit scalar value) from the provided UTF-8 sequence
        /// and convert that value to its UTF-16 representation.
        /// </summary>
        /// <returns>
        /// <see langword="true"/> if a scalar value could be decoded from the beginning of the input buffer and could be written to the output buffer,
        /// <see langword="false"/> if the input buffer did not begin with a valid UTF-8 sequence or the output buffer was too small to receive the value.
        /// </returns>
        public static bool TryReadFirstRuneAsUtf16(ReadOnlySpan<byte> inputBuffer, Span<char> outputBuffer, out int bytesConsumed, out int charsWritten)
        {
            if (PeekFirstSequence(inputBuffer, out bytesConsumed, out var scalar) == SequenceValidity.WellFormed)
            {
                int requiredLength = scalar.Utf16CodeUnitCount;
                if (outputBuffer.Length >= requiredLength)
                {
                    scalar.CopyUtf16CodeUnitsTo(outputBuffer);
                    charsWritten = requiredLength;
                    return true;
                }
            }

            // Failure case

            bytesConsumed = default;
            charsWritten = default;
            return false;
        }
    }
}
