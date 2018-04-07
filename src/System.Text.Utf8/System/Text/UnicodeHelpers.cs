// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace System.Text
{
    internal static class UnicodeHelpers
    {
        /// <summary>
        /// The Unicode replacement character U+FFFD.
        /// </summary>
        public const uint ReplacementChar = 0xFFFDU;

        /// <summary>
        /// Given a Unicode scalar value, gets the number of UTF-16 code units required to represent this value.
        /// </summary>
        public static int GetUtf16SequenceLength(uint value)
        {
            Debug.Assert(IsValidUnicodeScalar(value));

            value -= 0x10000;   // if value < 0x10000, high byte = 0xFF; else high byte = 0x00
            value += (2 << 24); // if value < 0x10000, high byte = 0x01; else high byte = 0x02
            value >>= 24;       // shift high byte down
            return (int)value;  // and return it
        }

        /// <summary>
        /// Given a Unicode scalar value, gets the number of UTF-8 code units required to represent this value.
        /// </summary>
        public static int GetUtf8SequenceLength(uint value)
        {
            Debug.Assert(IsValidUnicodeScalar(value));

            // The logic below can handle all valid scalar values branchlessly.
            // It gives good performance across all inputs. If the caller strongly
            // suspects the input to be ASCII, the method GetUtf8SequenceLength_BiasForAscii
            // has approx. 25% performance gain over this routine.

            // The logic below works roughly as follows. Start with a default result of 4.
            // If the scalar value is 1 or 2 UTF-8 code units, subtract 2 from the result.
            // If the scalar value is 1 or 3 UTF-8 code units, subtract 1 from the result.
            // Return the final result.

            // 'a' will be -1 if input is < 0x800; else 'a' will be 0
            // => 'a' will be -1 if input is 1 or 2 UTF-8 code units; else 'a' will be 0

            uint a = (value - 0x0800U) >> 31;

            // The number of UTF-8 code units for a given scalar is as follows:
            // - U+0000..U+007F => 1 code unit
            // - U+0080..U+07FF => 2 code units
            // - U+0800..U+FFFF => 3 code units
            // - U+10000+       => 4 code units
            //
            // If we XOR the incoming scalar with 0xF800, the chart mutates:
            // - U+0000..U+F7FF => 3 code units
            // - U+F800..U+F87F => 1 code unit
            // - U+F880..U+FFFF => 2 code units
            // - U+10000+       => 4 code units
            //
            // Since the 1- and 3-code unit cases are now clustered, they can
            // both be checked together very cheaply.

            value ^= 0xF800U;
            value -= 0xF880U;   // if scalar is 1 or 3 code units, high byte = 0xFF; else high byte = 0x00
            value += (4 << 24); // if scalar is 1 or 3 code units, high byte = 0x03; else high byte = 0x04
            value >>= 24;       // shift high byte down

            // Final return value:
            // - U+0000..U+007F => 3 + (-1) * 2 = 1
            // - U+0080..U+07FF => 4 + (-1) * 2 = 2
            // - U+0800..U+FFFF => 3 + ( 0) * 2 = 3
            // - U+10000+       => 4 + ( 0) * 2 = 4
            return (int)(value + (a * 2));
        }

        /// <summary>
        /// Given a Unicode scalar value, gets the number of UTF-8 code units required to represent this value.
        /// This method is optimized for ASCII input values but can handle any valid scalar input.
        /// </summary>
        public static int GetUtf8SequenceLength_BiasForAscii(uint value)
        {
            Debug.Assert(IsValidUnicodeScalar(value));

            // This method is similar to GetUtf8SequenceLength, but it optimizes for the case
            // of ASCII input, sacrificing performance in the case of multi-byte UTF-8 sequences.

            // If the caller thinks the input is likely to be ASCII, special-case it now.

            if (IsAsciiCodePoint(value))
            {
                return 1;
            }

            // Not ASCII.

            uint a = value - 0x10000U; // if input < 0x10000, high byte = 0xFF; else high byte = 0x00
            a += (4 << 24);            // if input < 0x10000, high byte = 0x03; else high byte = 0x04
            a >>= 24;                  // shift high byte down; final value = 3 or 4

            int b = (int)value - 0x800; // if input < 0x800, high byte = 0xFF; else high byte = 0x00
            b >>= 31;                   // if input < 0x800, final value = -1; else final value = 0

            // If            input < 0x800,   returns 3 + (-1) = 2
            // If   0x800 <= input < 0x10000, returns 3 +   0  = 3
            // If 0x10000 <= input,           returns 4 +   0  = 4
            return (int)a + b;
        }

        /// <summary>
        /// Returns <see langword="true"/> iff <paramref name="value"/> is an ASCII
        /// character ([ U+0000..U+007F ]).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsAsciiCodePoint(uint value) => (value < 0x80U);

        /// <summary>
        /// Returns <see langword="true"/> iff <paramref name="value"/> is in the
        /// Basic Multilingual Plane (BMP).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsBmpCodePoint(uint value) => (value < 0x10000U);


        /// <summary>
        /// Returns <see langword="true"/> iff <paramref name="value"/> is between
        /// <paramref name="lowerBound"/> and <paramref name="upperBound"/>, inclusive.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsInRangeInclusive(uint value, uint lowerBound, uint upperBound) => ((value - lowerBound) <= (upperBound - lowerBound));

        /// <summary>
        /// Returns <see langword="true"/> iff <paramref name="value"/> is a UTF-16 surrogate code point,
        /// i.e., is in [ U+D800..U+DFFF ], inclusive.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsSurrogateCodePoint(uint value) => IsInRangeInclusive(value, 0xD800U, 0xDFFFU);

        /// <summary>
        /// Returns <see langword="true"/> iff <paramref name="value"/> is a valid Unicode scalar
        /// value, i.e., is in [ U+0000..U+D7FF ], inclusive; or [ U+E000..U+10FFFF ], inclusive.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsValidUnicodeScalar(uint value) => IsInRangeInclusive(value ^ 0xD800U, 0x800U, 0x10FFFFU);
    }
}
