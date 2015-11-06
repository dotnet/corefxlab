// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace System.Text
{
    internal static class UnicodeConstants
    {
        // TODO: Some of these members are needed only in Utf16LittleEndianEncoder.
        //       Should we add the usage of them to UnicodeCodePoint class and merge this class with it?
        public const uint Utf16HighSurrogateFirstCodePoint = 0xD800;
        public const uint Utf16HighSurrogateLastCodePoint = 0xDFFF;
        public const uint Utf16LowSurrogateFirstCodePoint = 0xDC00;
        public const uint Utf16LowSurrogateLastCodePoint = 0xDFFF;

        public const uint Utf16SurrogateRangeStart = Utf16HighSurrogateFirstCodePoint;
        public const uint Utf16SurrogateRangeEnd = Utf16LowSurrogateLastCodePoint;

        public const byte Utf8NonFirstByteInCodePointValue = 0x80;
        public const byte Utf8NonFirstByteInCodePointMask = 0xC0;
        public const int Utf8MaxCodeUnitsPerCodePoint = 4;
        public const int AsciiMaxValue = 0x7F;

        public const uint FirstNotSupportedCodePoint = 0x110000; // 17 * 2^16

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
    }
}
