// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Runtime.CompilerServices;

namespace System.Text
{
    internal static class AsciiInvariantHelpers
    {
        /// <summary>
        /// Given 4 ASCII bytes packed into a DWORD, returns the equivalent packed
        /// byte representation where all uppercase ASCII bytes have been converted
        /// to lowercase. The input must be ASCII-only.
        /// </summary>
        public static uint ConvertPackedBytesToLowercase(uint packedBytes)
        {
            UnicodeDebug.AssertContainsOnlyAsciiBytes(packedBytes);

            // See comment in PackedBytesContainsLowercaseAsciiChar for how this works.
            // n.b. 0x41 is 'A', 0x5A is 'Z'

            uint p = packedBytes + 0x80808080U - 0x41414141U;
            uint q = packedBytes + 0x80808080U - 0x5B5B5B5BU;
            uint mask = (p ^ q) & 0x80808080U;

            // Each high bit of mask represents a byte that needs to have its 0x20 bit flipped.
            // This will convert lowercase <-> uppercase.

            return packedBytes ^ (mask >> 2);
        }

        /// <summary>
        /// Given 4 ASCII bytes packed into a DWORD, returns the equivalent packed
        /// byte representation where all lowercase ASCII bytes have been converted
        /// to uppercase. The input must be ASCII-only.
        /// </summary>
        public static uint ConvertPackedBytesToUppercase(uint packedBytes)
        {
            UnicodeDebug.AssertContainsOnlyAsciiBytes(packedBytes);

            // See comment in PackedBytesContainsLowercaseAsciiChar for how this works.
            // n.b. 0x61 is 'a', 0x7A is 'z'

            uint p = packedBytes + 0x80808080U - 0x61616161U;
            uint q = packedBytes + 0x80808080U - 0x7B7B7B7BU;
            uint mask = (p ^ q) & 0x80808080U;

            // Each high bit of mask represents a byte that needs to have its 0x20 bit flipped.
            // This will convert lowercase <-> uppercase.

            return packedBytes ^ (mask >> 2);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        /// <summary>
        /// Returns <see langword="true"/> iff any byte of the input value represents a
        /// lowercase ASCII character. The input must be ASCII-only.
        /// </summary>
        public static bool PackedBytesContainsLowercaseAsciiChar(uint packedBytes)
        {
            UnicodeDebug.AssertContainsOnlyAsciiBytes(packedBytes);

            // The input value is of the following form:
            // 0wwwwwww 0xxxxxxx 0yyyyyyy 0zzzzzzz
            //
            // We can set the high bit of each byte of the input, which allows
            // us to treat the high bit as a carry bit to determine if each
            // individual byte was greater than or equal to the search value.

            // 0x61 is 'a', so the high bit of each byte of p will be set
            // iff the corresponding byte was >= 0x61.

            uint p = packedBytes + 0x80808080U - 0x61616161U;

            // 0x7A is 'z', so the high bit of each byte of q will be set
            // iff the corresponding byte was >= 0x7B.

            uint q = packedBytes + 0x80808080U - 0x7B7B7B7BU;

            // We now compare the high bit each byte of p against the high
            // bit of each byte of q. This has the following result matrix.
            //
            // p_high = 0, q_high = 0 ===> byte is < 0x61, not a lowercase ASCII char
            //          0           1 ===> (cannot happen)
            //          1           0 ===> 0x61h <= byte < 0x7B, lowercase ASCII char
            //          1           1 ===> byte is >= 0x7B, not a lowercase ASCII char

            return (((p ^ q) & 0x80808080U) != 0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        /// <summary>
        /// Returns <see langword="true"/> iff any byte of the input value represents a
        /// uppercase ASCII character. The input must be ASCII-only.
        /// </summary>
        public static bool PackedBytesContainsUppercaseAsciiChar(uint packedBytes)
        {
            UnicodeDebug.AssertContainsOnlyAsciiBytes(packedBytes);

            // See comment in PackedBytesContainsLowercaseAsciiChar for how this works.
            // n.b. 0x41 is 'A', 0x5A is 'Z'

            uint p = packedBytes + 0x80808080U - 0x41414141U;
            uint q = packedBytes + 0x80808080U - 0x5B5B5B5BU;
            return (((p ^ q) & 0x80808080U) != 0);
        }
    }
}
