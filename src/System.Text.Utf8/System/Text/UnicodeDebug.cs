// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics;
using System.Runtime.InteropServices;

namespace System.Text
{
    internal static class UnicodeDebug
    {
        [Conditional("DEBUG")]
        public static void AssertContainsOnlyAsciiBytes(uint value)
        {
            Debug.Assert((value & 0x80808080U) == 0, "Input value does not consist of only ASCII bytes.");
        }

        [Conditional("DEBUG")]
        public static void AssertDoesNotOverlap(ref byte a, ref byte b, uint length)
        {
            Debug.Assert(length <= Int32.MaxValue);
            Debug.Assert(!MemoryMarshal.CreateSpan(ref a, (int)length).Overlaps(MemoryMarshal.CreateSpan(ref b, (int)length)), "Buffers may not overlap.");
        }

        [Conditional("DEBUG")]
        public static void AssertIsHighSurrogteCodePoint(uint codePoint)
        {
            Debug.Assert(UnicodeHelpers.IsHighSurrogateCodePoint(codePoint), $"The value {ToHexString(codePoint)} is not a UTF-16 high surrogate code point.");
        }

        [Conditional("DEBUG")]
        public static void AssertIsLowSurrogteCodePoint(uint codePoint)
        {
            Debug.Assert(UnicodeHelpers.IsLowSurrogateCodePoint(codePoint), $"The value {ToHexString(codePoint)} is not a UTF-16 low surrogate code point.");
        }

        [Conditional("DEBUG")]
        public static void AssertIsValidCodePoint(uint codePoint)
        {
            Debug.Assert(UnicodeHelpers.IsValidCodePoint(codePoint), $"The value {ToHexString(codePoint)} is not a valid Unicode code point.");
        }

        [Conditional("DEBUG")]
        public static void AssertIsValidScalar(uint scalarValue)
        {
            Debug.Assert(UnicodeHelpers.IsValidUnicodeScalar(scalarValue), $"The value {ToHexString(scalarValue)} is not a valid Unicode scalar value.");
        }

        /// <summary>
        /// Formats a code point as the hex string "U+XXXX".
        /// </summary>
        /// <remarks>
        /// The input value doesn't have to be a real code point in the Unicode codespace. It can be any integer.
        /// </remarks>
        private static string ToHexString(uint codePoint)
        {
            return FormattableString.Invariant($"U+{codePoint:X4}");
        }
    }
}
