// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace System.Text.JsonLab
{
    internal static class JsonEscapeUtilities
    {
        private const char UNICODE_REPLACEMENT_CHAR = '\uFFFD';
        private const int UNICODE_LAST_CODEPOINT = 0x10FFFF;

        /// <summary>
        /// Converts a number 0 - 15 to its associated hex character '0' - 'F'.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static char UInt32LsbToHexDigit(uint value)
        {
            Debug.Assert(value < 16);
            return (value < 10) ? (char)('0' + value) : (char)('A' + (value - 10));
        }

        /// <summary>
        /// Converts a number 0 - 15 to its associated hex character '0' - 'F'.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static char Int32LsbToHexDigit(int value)
        {
            Debug.Assert(value < 16);
            return (char)((value < 10) ? ('0' + value) : ('A' + (value - 10)));
        }

        /// <summary>
        /// Given a UTF-16 character stream, reads the next scalar value from the stream.
        /// Set 'endOfString' to true if 'pChar' points to the last character in the stream.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetScalarValueFromUtf16(char first, char? second, out bool wasSurrogatePair)
        {
            if (!char.IsSurrogate(first))
            {
                wasSurrogatePair = false;
                return first;
            }
            return GetScalarValueFromUtf16Slow(first, second, out wasSurrogatePair);
        }

        private static int GetScalarValueFromUtf16SurrogatePair(char highSurrogate, char lowSurrogate)
        {
            Debug.Assert(char.IsHighSurrogate(highSurrogate));
            Debug.Assert(char.IsLowSurrogate(lowSurrogate));

            // See http://www.unicode.org/versions/Unicode6.2.0/ch03.pdf, Table 3.5 for the
            // details of this conversion. We don't use Char.ConvertToUtf32 because its exception
            // handling shows up on the hot path, and our caller has already sanitized the inputs.
            return (lowSurrogate & 0x3ff) | (((highSurrogate & 0x3ff) + (1 << 6)) << 10);
        }

        private static int GetScalarValueFromUtf16Slow(char first, char? second, out bool wasSurrogatePair)
        {
            if (char.IsHighSurrogate(first))
            {
                if (second != null)
                {
                    if (char.IsLowSurrogate(second.Value))
                    {
                        // valid surrogate pair - extract codepoint
                        wasSurrogatePair = true;
                        return GetScalarValueFromUtf16SurrogatePair(first, second.Value);
                    }
                    else
                    {
                        // unmatched surrogate - substitute
                        wasSurrogatePair = false;
                        return UNICODE_REPLACEMENT_CHAR;
                    }
                }
                else
                {
                    // unmatched surrogate - substitute
                    wasSurrogatePair = false;
                    return UNICODE_REPLACEMENT_CHAR;
                }
            }
            else
            {
                // unmatched surrogate - substitute
                Debug.Assert(char.IsLowSurrogate(first));
                wasSurrogatePair = false;
                return UNICODE_REPLACEMENT_CHAR;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryWriteScalarAsChar(int unicodeScalar, Span<char> destination, out int numberOfCharactersWritten)
        {
            Debug.Assert(unicodeScalar < ushort.MaxValue);
            if (destination.Length < 1)
            {
                numberOfCharactersWritten = 0;
                return false;
            }
            destination[0] = (char)unicodeScalar;
            numberOfCharactersWritten = 1;
            return true;
        }

        /// <summary>
        /// Determines whether the given scalar value is in the supplementary plane and thus
        /// requires 2 characters to be represented in UTF-16 (as a surrogate pair).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsSupplementaryCodePoint(int scalar)
        {
            return (scalar & ~char.MaxValue) != 0;
        }

        public static void GetUtf16SurrogatePairFromAstralScalarValue(int scalar, out char highSurrogate, out char lowSurrogate)
        {
            Debug.Assert(0x10000 <= scalar && scalar <= UNICODE_LAST_CODEPOINT);

            // See http://www.unicode.org/versions/Unicode6.2.0/ch03.pdf, Table 3.5 for the
            // details of this conversion. We don't use Char.ConvertFromUtf32 because its exception
            // handling shows up on the hot path, it allocates temporary strings (which we don't want),
            // and our caller has already sanitized the inputs.

            int x = scalar & 0xFFFF;
            int u = scalar >> 16;
            int w = u - 1;
            highSurrogate = (char)(0xD800 | (w << 6) | (x >> 10));
            lowSurrogate = (char)(0xDC00 | (x & 0x3FF));
        }
    }
}
