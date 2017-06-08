// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Runtime.CompilerServices;

namespace System.Text
{
    static class EncodingHelper
    {
        #region Constants

        private const uint FirstNotSupportedCodePoint = 0x110000; // 17 * 2^16
        private const uint BasicMultilingualPlaneEndMarker = 0x10000;

        // TODO: Make this immutable and let them be strong typed
        // http://unicode.org/cldr/utility/list-unicodeset.jsp?a=\p{whitespace}&g=&i=
        private static readonly uint[] SortedWhitespaceCodePoints = new uint[25]
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

        public const char HighSurrogateStart = '\ud800';
        public const char HighSurrogateEnd = '\udbff';
        public const char LowSurrogateStart = '\udc00';
        public const char LowSurrogateEnd = '\udfff';

        #endregion Constants

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsWhitespace(uint codePoint)
        {
            return Array.BinarySearch<uint>(SortedWhitespaceCodePoints, codePoint) >= 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsSupportedCodePoint(uint codePoint)
        {
            return codePoint < FirstNotSupportedCodePoint;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsBmp(uint codePoint)
        {
            return codePoint < BasicMultilingualPlaneEndMarker;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static int PtrDiff(char* a, char* b)
        {
            return (int)(((uint)((byte*)a - (byte*)b)) >> 1);
        }

        // byte* flavor just for parity
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static int PtrDiff(byte* a, byte* b)
        {
            return (int)(a - b);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool InRange(int ch, int start, int end)
        {
            return (uint)(ch - start) <= (uint)(end - start);
        }
    }
}
