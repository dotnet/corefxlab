using System.Runtime.CompilerServices;

namespace System.Text.Utf8
{
    // TODO: Should we directly use byte or leave it. If we leave it should we leave the name?
    public struct Utf8CodeUnit : IEquatable<Utf8CodeUnit>
    {
        public Utf8CodeUnit(byte value)
        {
            Value = value;
        }

        public byte Value { get; private set; }

        public bool Equals(Utf8CodeUnit other)
        {
            return Value == other.Value;
        }

        public override bool Equals(object obj)
        {
            if (obj is Utf8CodeUnit)
            {
                return Equals((Utf8CodeUnit)obj);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public static explicit operator Utf8CodeUnit(byte value) { return new Utf8CodeUnit(value); }
        public static explicit operator byte(Utf8CodeUnit codeUnit) { return codeUnit.Value; }

        public static bool operator ==(Utf8CodeUnit left, Utf8CodeUnit right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Utf8CodeUnit left, Utf8CodeUnit right)
        {
            return !left.Equals(right);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsSurrogate(Utf8CodeUnit codeUnit)
        {
            return (codeUnit.Value & UnicodeConstants.Utf8SurrogateMask) == UnicodeConstants.Utf8SurrogateValue;
        }
    }
}
