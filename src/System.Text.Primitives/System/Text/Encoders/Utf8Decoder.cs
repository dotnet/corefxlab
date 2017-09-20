// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace System.Text.Primitives.System.Text.Encoders
{
    /// <summary>
    /// Contains facilities for validating and extracting Unicode scalar values from UTF8 byte sequences.
    /// </summary>
    internal static class Utf8Decoder
    {
        // Assumption: bytesAvailable > 0
        /// <summary>
        /// Reads a Unicode scalar value from the provided UTF8 sequence.
        /// </summary>
        /// <param name="data">A reference to the first byte of the UTF8 sequence from which to read the scalar value.</param>
        /// <param name="bytesAvailable">The number of elements available in the buffer referenced by <paramref name="data"/>.</param>
        /// <param name="bytesConsumed">When this method returns, contains the number of bytes
        /// from <paramref name="data"/> which were consumed while reading the scalar value.</param>
        /// <returns>On success, a non-negative integer which represents a Unicode scalar value.
        /// On failure, a negative integer which represents an <see cref="ErrorStatus"/> failure code.</returns>
        /// <remarks>
        /// The caller must use extreme caution to avoid passing a value for <paramref name="bytesAvailable"/> that is
        /// larger than the actual buffer referenced by <paramref name="data"/>, else a buffer overrun could occur.
        /// See http://www.unicode.org/glossary/#unicode_scalar_value for the definition of Unicode scalar value.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int DangerousReadUnicodeScalarValue(ref byte data, int bytesAvailable, out int bytesConsumed)
        {
            if (bytesAvailable == 0)
            {
                // Quick check: no data to consume
                bytesConsumed = 0;
                return ErrorStatus.InsufficientData;
            }
            else
            {
                return DangerousReadUnicodeScalarValueWithoutNullCheck(ref data, bytesAvailable, out bytesConsumed);
            }
        }

        // Assumption: bytesAvailable > 0
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int DangerousReadUnicodeScalarValueWithoutNullCheck(ref byte data, int bytesAvailable, out int bytesConsumed)
        {
            Debug.Assert(bytesAvailable > 0);

            if ((data & 0x80) == 0)
            {
                // Fast case: ASCII character => the code point is the byte itself
                bytesConsumed = 1;
                return data;
            }
            else
            {
                // Slow case: multi-byte sequence
                bytesConsumed = 0;
                return ReadUnicodeScalarValueFromNonAscii(ref data, bytesAvailable, ref bytesConsumed);
            }
        }

        /// <summary>
        /// Retrieves the index of the first invalid byte in the data, i.e., the index of the
        /// first invalid UTF8 subsequence within the data.
        /// </summary>
        /// <param name="data">The data to check.</param>
        /// <returns>The index into <paramref name="data"/> where the first invalid byte appears, or -1
        /// if <paramref name="data"/> represents a well-formed UTF8 string.</returns>
        /// <remarks>An invalid subsequence could occur because of the presence of an illegal byte
        /// (such as 0xFF) or because <paramref name="data"/> begins or ends in the middle of a subsequence.</remarks>
        public static int GetIndexOfFirstInvalidUtf8Byte(ReadOnlySpan<byte> data)
        {
            // TODO: Can this be optimized, perhaps by performing a clever "is ASCII?" bitwise check
            // and consuming entire ASCII sequences one natural word length at a time?

            int numValidBytesSoFar = 0;

            while (numValidBytesSoFar < data.Length)
            {
                if (DangerousReadUnicodeScalarValueWithoutNullCheck(ref Unsafe.Add(ref data.DangerousGetPinnableReference(), numValidBytesSoFar), data.Length - numValidBytesSoFar, out int thisIterBytesConsumed) < 0)
                {
                    return numValidBytesSoFar; // error (not enough data or malformed sequence seen)
                }
                else
                {
                    numValidBytesSoFar += thisIterBytesConsumed; // success (read a valid code point from the sequence)
                }
            }

            return -1; // no invalid bytes
        }

        /// <summary>
        /// Given the first byte of a UTF8 multi-byte sequence, returns the number of expected trailing
        /// bytes and the minimum allowed code point value which is encoded by this sequence. Returns
        /// 0 on invalid leading byte.
        /// </summary>
        private static int GetNumberOfTrailingBytes(byte firstByte, out int minAllowedCodePointValue)
        {
            // Logic below is from http://www.unicode.org/versions/Unicode10.0.0/ch03.pdf, Tables 3.6 and 3.7.

            if ((firstByte & 0b1110_0000) == 0b1100_0000)
            {
                // [ 110yyyyy 10xxxxxx ]
                minAllowedCodePointValue = 0x80;
                return 1;
            }
            else if ((firstByte & 0b1111_0000) == 0b1110_0000)
            {
                // [ 1110zzzz 10yyyyyy 10xxxxxx ]
                minAllowedCodePointValue = 0x800;
                return 2;
            }
            else if ((firstByte & 0b1111_1000) == 0b1111_0000)
            {
                // [ 11110uuu 10uuzzzz 10yyyyyy 10xxxxxx ]
                minAllowedCodePointValue = 0x10000;
                return 3;
            }
            else
            {
                minAllowedCodePointValue = 0;
                return 0;
            }
        }

        /// <summary>
        /// Returns true iff the specified code point is a UTF16 surrogate.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool IsSurrogate(int codePoint)
        {
            Debug.Assert(0 <= codePoint && codePoint <= 0x10FFFF);

            // See http://www.unicode.org/versions/Unicode10.0.0/ch03.pdf, Sec. 3.8.
            // Surrogates are in the range U+D800..U+DFFF, so we can perform a simple
            // bit mask check to see if the requested code point is within this range.
            return ((codePoint & 0b1111_1111_1111_1000_0000_0000) == 0b1101_1000_0000_0000);
        }

        /// <summary>
        /// Returns true iff the specified byte is a valid UTF8 trailing byte.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool IsValidTrailingByte(byte value)
        {
            // See http://www.unicode.org/versions/Unicode10.0.0/ch03.pdf, Table 3-6.
            return ((value & 0b1100_0000) == 0b1000_0000);
        }

        /// <summary>
        /// Determines whether the provided data represents a well-formed UTF8 string.
        /// </summary>
        /// <param name="data">The data to check for well-formedness.</param>
        /// <returns>True if the data is a well-formed UTF8 string; false otherwise.</returns>
        public static bool IsWellFormedUtf8String(ReadOnlySpan<byte> data)
        {
            return (GetIndexOfFirstInvalidUtf8Byte(data) < 0);
        }

        /// <summary>
        /// Reads a Unicode scalar value from the provided UTF8 sequence.
        /// </summary>
        /// <param name="data">The UTF8 sequence from which to read the scalar value.</param>
        /// <param name="bytesConsumed">When this method returns, contains the number of bytes
        /// from <paramref name="data"/> which were consumed while reading the scalar value.</param>
        /// <returns>On success, a non-negative integer which represents a Unicode scalar value.
        /// On failure, a negative integer which represents an <see cref="ErrorStatus"/> failure code.</returns>
        /// <remarks>See http://www.unicode.org/glossary/#unicode_scalar_value for the definition of Unicode scalar value.</remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ReadUnicodeScalarValue(ReadOnlySpan<byte> data, out int bytesConsumed)
        {
            return DangerousReadUnicodeScalarValue(ref data.DangerousGetPinnableReference(), data.Length, out bytesConsumed);
        }

        // Assumption: bytesAvailable > 0
        // Assumption: bytesConsumed = 0
        private static int ReadUnicodeScalarValueFromNonAscii(ref byte data, int bytesAvailable, ref int bytesConsumed)
        {
            Debug.Assert(bytesAvailable > 0);
            Debug.Assert(bytesConsumed == 0);

            // n.b. For most of this method bytesConsumed has a value of *one fewer* than the actual number of
            // bytes consumed so far. This is required for error handling, where if we need to report back to
            // our caller that there's a problem we don't want to mark the byte that caused the error as consumed.
            // See http://www.unicode.org/versions/Unicode10.0.0/ch03.pdf, "Constraints on Conversion Process",
            // and http://www.unicode.org/reports/tr36/, Sec. 3.6.1, for more info.

            // First, determine how many bytes are required to read the entire code point.

            int minAllowedCodePointValue;
            int numTrailingBytes = GetNumberOfTrailingBytes(data, out minAllowedCodePointValue);

            if (numTrailingBytes == 0)
            {
                return ErrorStatus.BadCharacter; // not a valid leading byte
            }
            else if (numTrailingBytes >= bytesAvailable)
            {
                return ErrorStatus.InsufficientData; // not enough trailing bytes (n.b. bytesAvailable includes leading byte)
            }

            // Next, fold the leading byte and all trailing bytes into the code point.

            int constructedCodePoint = data & (0x3F >> numTrailingBytes);

            Debug.Assert(numTrailingBytes > 0);
            do
            {
                byte nextTrailingByte = Unsafe.Add(ref data, ++bytesConsumed);
                if (IsValidTrailingByte(nextTrailingByte))
                {
                    constructedCodePoint = (constructedCodePoint << 6) | (nextTrailingByte & 0b0011_1111);
                }
                else
                {
                    // We don't want to mark this byte as consumed, as it may begin a new valid sequence,
                    // and the Unicode Standard (Sec. 3.9, "Constraints on Conversion Processes") forbids
                    // us from consuming potentially such bytes while processing multibyte sequences.
                    // It's possible that the new byte is itself part of an invalid sequence, but if this
                    // is the case we'll just report that to the caller on the next iteration. The Standard
                    // gives us considerable leeway to report multiple errors as part of a single invalid
                    // sequence.
                    return ErrorStatus.BadCharacter;
                }
            } while (--numTrailingBytes != 0);

            // We've completed reconstruction of the code point. Now all that's left to check is that the code point
            // is in shortest form and is not a surrogate. If the sequence is valid, we can return the value as-is.
            // If the sequence is invalid, we can return a single error because we checked earlier in the method
            // that no individual byte within this sequence (apart from the first byte) could possibly be the start
            // of a new valid UTF8 subsequence. In either case we consumed all bytes so must perform one final fixup
            // to the bytesConsumed variable.

            bytesConsumed++;
            Debug.Assert(bytesConsumed <= bytesAvailable);

            return ((constructedCodePoint < minAllowedCodePointValue) || IsSurrogate(constructedCodePoint))
                ? ErrorStatus.BadCharacter : constructedCodePoint;
        }

        /// <summary>
        /// Contains error codes that may be returned when attempting to read Unicode scalar values.
        /// </summary>
        /// <remarks>
        /// All error codes are negative integers.
        /// </remarks>
        public static class ErrorStatus
        {
            /// <summary>
            /// The subsequence was malformed or contained an illegal byte.
            /// </summary>
            public const int BadCharacter = -1;

            /// <summary>
            /// The end of the buffer was reached while trying to decode a subsequence.
            /// </summary>
            public const int InsufficientData = -2;
        }
    }
}
