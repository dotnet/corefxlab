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

            // If value <  0x10000, returns (-1) + 2 = 1
            // If value >= 0x10000, returns   0  + 2 = 2
            return (((int)value - 0x10000) >> 31) + 2;
        }

        /// <summary>
        /// Given a Unicode scalar value, gets the number of UTF-16 code units required to represent this value.
        /// </summary>
        public static int GetUtf8SequenceLength(uint value)
        {
            Debug.Assert(IsValidUnicodeScalar(value));

            // The logic below special-cases ASCII since it should be far and away
            // the most common case. All other cases are handled together in a
            // single branch. This tradeoff gives good performance on 1-byte, 3-byte,
            // and 4-byte sequences, while sacrificing some performance in the 2-byte
            // case. The alternative implementation commented out below gives good
            // performance on multibyte sequences but is unacceptably slow for the
            // common case of ASCII values.
            //
            // const ulong mask = 0b_001_001_001_001_001_001_001_001_010_010_010_010_011_011_011_011_011_100_100_100_100_100UL;
            // return (int)(mask >> (int)(lzcnt(value) * 3 - 33)) & 7;

            if (IsAsciiCodePoint(value))
            {
                return 1;
            }

            uint a = value - 0x10000U; // if input < 0x10000, high byte = 0xFF; else high byte = 0x00
            a += 0x4000000U;           // if input < 0x10000, high byte = 0x03; else high byte = 0x04
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
        public static bool IsValidUnicodeScalar(uint value) => IsInRangeInclusive(value & 0xD800U, 0x800U, 0x10FFFFU);
    }
}
