using System.Runtime.CompilerServices;

namespace System.Text.Utf8
{
    // TODO: Check if name is understandable for users
    //       I personally think it is better to keep the spec name than calling it with some random new name or byte
    //       In the end it is less confusing than trying to figure out what spec means and what .NET means
    public partial struct Utf8CodeUnit : IEquatable<Utf8CodeUnit>
    {
        private byte _value;

        public Utf8CodeUnit(byte value)
        {
            _value = value;
        }

        public byte Value { get { return _value; } }

        public bool Equals(Utf8CodeUnit other)
        {
            return Value == other.Value;
        }

        // TODO: Validate if it should work with char. Added for consistency with Utf8String for now.
        public override bool Equals(object obj)
        {
            if (obj is Utf8CodeUnit)
            {
                return Equals((Utf8CodeUnit)obj);
            }

            if (obj is char)
            {
                return Equals((char)obj);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public static explicit operator Utf8CodeUnit(byte value)
        {
            return new Utf8CodeUnit(value);
        }

        public static explicit operator byte(Utf8CodeUnit codeUnit)
        {
            return codeUnit.Value;
        }

        public static bool operator ==(Utf8CodeUnit left, Utf8CodeUnit right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Utf8CodeUnit left, Utf8CodeUnit right)
        {
            return !left.Equals(right);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsFirstCodeUnitInEncodedCodePoint(Utf8CodeUnit codeUnit)
        {
            return (codeUnit.Value & UnicodeConstants.Utf8NonFirstByteInCodePointMask) != UnicodeConstants.Utf8NonFirstByteInCodePointValue;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsAscii(Utf8CodeUnit codeUnit)
        {
            // TODO: Is there any faster way to check if first bit is 0?
            return codeUnit.Value <= UnicodeConstants.AsciiMaxValue;
        }
    }
}
