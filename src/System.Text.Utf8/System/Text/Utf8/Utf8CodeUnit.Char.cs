using System.Runtime.CompilerServices;

namespace System.Text.Utf8
{
    // TODO: Should we have interaction with char or should we keep them separate?
    partial struct Utf8CodeUnit
    {
        // TODO: Validate if this constructor should exist (and if it should throw)
        public Utf8CodeUnit(char value)
        {
            if (!IsAscii(value))
            {
                throw new ArgumentOutOfRangeException("value", "Character cannot be represented as single utf-8 code unit. It needs to be ASCII. For complex characters use UnicodeCodePoint.");
            }

            _value = unchecked((byte)(value & UnicodeConstants.AsciiMaxValue));
        }

        public bool Equals(char other)
        {
            return IsAscii(other) && unchecked((uint)Value == (uint)other);
        }

        public static explicit operator Utf8CodeUnit(char value)
        {
            return new Utf8CodeUnit(value);
        }

        public static explicit operator char (Utf8CodeUnit codeUnit)
        {
            if (!IsAscii(codeUnit))
            {
                throw new ArgumentOutOfRangeException("codeUnit", "This code unit is invalid without a context. It is a utf-8 surrogate character which means it is part of something bigger.");
            }

            return (char)codeUnit.Value;
        }

        // TODO: Char already has identical method but it's private so let's reimplement it...
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool IsAscii(char value)
        {
            return value <= UnicodeConstants.AsciiMaxValue;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryCreateFrom(char value, out Utf8CodeUnit codeUnit)
        {
            if (IsAscii(value))
            {
                codeUnit = new Utf8CodeUnit(unchecked((byte)value));
                return true;
            }

            codeUnit = default(Utf8CodeUnit);
            return false;
        }
    }
}
