using System.Runtime.CompilerServices;

namespace System.Text.Utf8
{
    internal static class Utf8CodeUnit
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsFirstCodeUnitInEncodedCodePoint(byte codeUnit)
        {
            return (codeUnit & UnicodeConstants.Utf8NonFirstByteInCodePointMask) != UnicodeConstants.Utf8NonFirstByteInCodePointValue;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsAscii(byte codeUnit)
        {
            return codeUnit <= UnicodeConstants.AsciiMaxValue;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool IsAscii(char value)
        {
            return value <= UnicodeConstants.AsciiMaxValue;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryCreateFrom(char value, out byte codeUnit)
        {
            if (IsAscii(value)) {
                codeUnit = (byte)value;
                return true;
            }

            codeUnit = default(byte);
            return false;
        }
    }
}
