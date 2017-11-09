using System.Runtime.CompilerServices;

namespace System.Buffers.Text
{
    public static class Unicode
    {
        // TODO: Make this immutable and let them be strong typed
        // http://unicode.org/cldr/utility/list-unicodeset.jsp?a=\p{whitespace}&g=&i=
        private static readonly uint[] SortedWhitespaceCodePoints = new uint[]
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsWhitespace(uint codePoint)
        {
            return Array.BinarySearch<uint>(SortedWhitespaceCodePoints, codePoint) >= 0;
        }
    }
}
