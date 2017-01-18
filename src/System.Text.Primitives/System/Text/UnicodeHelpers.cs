// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace System.Text
{
    internal static class UnicodeHelpers
    {
        #region Constants

        private const uint FirstNotSupportedCodePoint = 0x110000; // 17 * 2^16

        // TODO: Make this immutable and let them be strong typed
        // http://unicode.org/cldr/utility/list-unicodeset.jsp?a=\p{whitespace}&g=&i=
        public static readonly uint[] SortedWhitespaceCodePoints = new uint[25]
        {
            0x0009, 0x000A, 0x000B, 0x000C, 0x000D,
            0x0020,
            0x0085,
            0x00A0,
            0x1680,
            0x2000, 0x2001, 0x2002, 0x2003, 0x2004, 0x2005, 0x2006,
            0x2007,
            0x2008, 0x2009, 0x200A,
            0x2028, 0x2029,
            0x202F,
            0x205F,
            0x3000
        };

        #endregion Constants

        public static bool IsSupportedCodePoint(uint codePoint)
        {
            return codePoint < FirstNotSupportedCodePoint;
        }

        public static bool IsBmp(uint codePoint)
        {
            return codePoint < 0x10000;
        }
    }
}
