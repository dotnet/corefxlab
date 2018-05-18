// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace System.Text
{
    /// <summary>
    /// Similar to <see cref="UnicodeReader"/>, but makes no attempt to fix-up invalid sequences.
    /// </summary>
    internal static class UnicodeEnumerator
    {
        /// <summary>
        /// Returns the next scalar from a UTF-16 input sequence, or a negative value if the input sequence is invalid.
        /// </summary>
        public static (int scalar, int charsConsumed) GetNextScalarUtf16(ReadOnlySpan<char> buffer)
        {
            // TODO: Perf testing - is this performant enough without falling back to unsafe code?

            if (buffer.Length == 0)
            {
                goto Invalid;
            }

            uint retVal = buffer[0];
            if (!UnicodeHelpers.IsSurrogateCodePoint(retVal))
            {
                return (scalar: (int)retVal, charsConsumed: 1);
            }

            if (retVal >= UnicodeHelpers.FirstLowSurrogateCodePoint || buffer.Length <= 1)
            {
                goto Invalid; // began with a low surrogate or input length was 1
            }

            uint lowSurrogateCodePoint = buffer[1];
            if (!UnicodeHelpers.IsLowSurrogateCodePoint(lowSurrogateCodePoint))
            {
                goto Invalid; // second code point was not a low surrogate code point
            }

            return (scalar: (int)UnicodeHelpers.GetScalarFromUtf16SurrogateCodePoints(retVal, lowSurrogateCodePoint), charsConsumed: 2);

        Invalid:
            // return a different return value than GetNextScalarUtf8 so that comparisons are never equal
            return (scalar: -1, charsConsumed: 0);
        }
    }
}
