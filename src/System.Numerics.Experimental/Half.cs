// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System.Numerics.Experimental
{
    // ===================================================================================================
    // Portions of the code implemented below are based on the 'Berkeley SoftFloat Release 3e' algorithms.
    // ===================================================================================================

    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public readonly struct Half : IComparable, IFormattable, IComparable<Half>, IEquatable<Half>
    {
        private const NumberStyles DefaultParseStyle = NumberStyles.Float | NumberStyles.AllowThousands;

        //
        // Constants for manipulating the private bit-representation
        //

        private const ushort SignMask = 0x8000;
        private const ushort SignShift = 15;

        private const ushort ExponentMask = 0x7C00;
        private const ushort ExponentShift = 10;

        private const ushort SignificandMask = 0x03FF;
        private const ushort SignificandShift = 0;

        private const ushort MinSign = 0;
        private const ushort MaxSign = 1;

        private const ushort MinExponent = 0x00;
        private const ushort MaxExponent = 0x1F;

        private const ushort MinSignificand = 0x0000;
        private const ushort MaxSignificand = 0x03FF;

        //
        // Constants representing the private bit-representation for various default values
        //

        private const ushort PositiveZeroBits = 0x0000;
        private const ushort NegativeZeroBits = 0x8000;

        private const ushort EpsilonBits = 0x0001;

        private const ushort PositiveInfinityBits = 0x7C00;
        private const ushort NegativeInfinityBits = 0xFC00;

        private const ushort PositiveQNaNBits = 0x7E00;
        private const ushort NegativeQNaNBits = 0xFE00;

        private const ushort MinValueBits = 0xFBFF;
        private const ushort MaxValueBits = 0x7BFF;

        //
        // Constants that should be returned if values that cannot be represented are converted
        //

        private const long IllegalValueToInt64 = long.MinValue;
        private const ulong IllegalValueToUInt64 = ulong.MinValue;
        private const int IllegalValueToInt32 = int.MinValue;
        private const uint IllegalValueToUInt32 = uint.MinValue;

        //
        // Well-defined and commonly used values
        //

        public static readonly Half Epsilon = new Half(EpsilonBits);                        //  5.9605E-08

        public static readonly Half PositiveInfinity = new Half(PositiveInfinityBits);      //  1.0 / 0.0
        public static readonly Half NegativeInfinity = new Half(NegativeInfinityBits);      // -1.0 / 0.0

        public static readonly Half NaN = new Half(NegativeQNaNBits);                       //  0.0 / 0.0

        public static readonly Half MinValue = new Half(MinValueBits);                      // -65504
        public static readonly Half MaxValue = new Half(MaxValueBits);                      //  65504

        // We use these explicit definitions to avoid the confusion between 0.0 and -0.0.
        private static readonly Half PositiveZero = new Half(PositiveZeroBits);            //  0.0
        private static readonly Half NegativeZero = new Half(NegativeZeroBits);            // -0.0

        private readonly ushort m_value; // Do not rename (binary serialization)

        private Half(ushort value)
        {
            m_value = value;
        }

        private Half(bool sign, ushort exp, ushort sig)
            => m_value = (ushort)(((sign ? 1 : 0) << SignShift) + (exp << ExponentShift) + sig);

        private sbyte Exponent
        {
            get
            {
                return (sbyte)((m_value & ExponentMask) >> ExponentShift);
            }
        }

        private ushort Significand
        {
            get
            {
                return (ushort)((m_value & SignificandMask) >> SignificandShift);
            }
        }

        public static bool operator <(Half left, Half right)
        {
            if (IsNaN(left) || IsNaN(right))
            {
                // IEEE defines that NaN is unordered with respect to everything, including itself.
                return false;
            }

            bool leftIsNegative = IsNegative(left);

            if (leftIsNegative != IsNegative(right))
            {
                // When the signs of left and right differ, we know that left is less than right if it is
                // the negative value. The exception to this is if both values are zero, in which case IEEE
                // says they should be equal, even if the signs differ.
                return leftIsNegative && !AreZero(left, right);
            }
            return (short)(left.m_value) < (short)(right.m_value);
        }

        public static bool operator >(Half left, Half right)
        {
            return right < left;
        }

        public static bool operator <=(Half left, Half right)
        {
            if (IsNaN(left) || IsNaN(right))
            {
                // IEEE defines that NaN is unordered with respect to everything, including itself.
                return false;
            }

            bool leftIsNegative = IsNegative(left);

            if (leftIsNegative != IsNegative(right))
            {
                // When the signs of left and right differ, we know that left is less than right if it is
                // the negative value. The exception to this is if both values are zero, in which case IEEE
                // says they should be equal, even if the signs differ.
                return leftIsNegative || AreZero(left, right);
            }
            return (short)(left.m_value) <= (short)(right.m_value);
        }

        public static bool operator >=(Half left, Half right)
        {
            return right <= left;
        }

        public static bool operator ==(Half left, Half right)
        {
            if (IsNaN(left) || IsNaN(right))
            {
                // IEEE defines that NaN is not equal to anything, including itself.
                return false;
            }

            // IEEE defines that positive and negative zero are equivalent.
            return (left.m_value == right.m_value) || AreZero(left, right);
        }

        public static bool operator !=(Half left, Half right)
        {
            return !(left == right);
        }

        // <summary>Determines whether the specified value is finite (zero, subnormal, or normal).</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsFinite(Half value)
        {
            return StripSign(value) < PositiveInfinityBits;
        }

        /// <summary>Determines whether the specified value is infinite.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsInfinity(Half value)
        {
            return StripSign(value) == PositiveInfinityBits;
        }

        /// <summary>Determines whether the specified value is NaN.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsNaN(Half value)
        {
            return StripSign(value) > PositiveInfinityBits;
        }

        /// <summary>Determines whether the specified value is negative.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsNegative(Half value)
        {
            return (short)(value.m_value) < 0;
        }

        /// <summary>Determines whether the specified value is negative infinity.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsNegativeInfinity(Half value)
        {
            return value.m_value == NegativeInfinityBits;
        }

        /// <summary>Determines whether the specified value is normal.</summary>
        // This is probably not worth inlining, it has branches and should be rarely called
        public static bool IsNormal(Half value)
        {
            int absValue = StripSign(value);
            return (absValue < PositiveInfinityBits)    // is finite
                && (absValue != 0)                      // is not zero
                && ((absValue & ExponentMask) != 0);    // is not subnormal (has a non-zero exponent)
        }

        /// <summary>Determines whether the specified value is positive infinity.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsPositiveInfinity(Half value)
        {
            return value.m_value == PositiveInfinityBits;
        }

        /// <summary>Determines whether the specified value is subnormal.</summary>
        // This is probably not worth inlining, it has branches and should be rarely called
        public static bool IsSubnormal(Half value)
        {
            int absValue = StripSign(value);
            return (absValue < PositiveInfinityBits)    // is finite
                && (absValue != 0)                      // is not zero
                && ((absValue & ExponentMask) == 0);    // is subnormal (has a zero exponent)
        }

        public static Half Parse(string s, NumberStyles style = DefaultParseStyle, IFormatProvider formatProvider = null)
        {
            if (s is null)
            {
                throw new ArgumentNullException(nameof(s));
            }
            return Parse(s.AsSpan(), style, formatProvider);
        }

        public static Half Parse(ReadOnlySpan<char> s, NumberStyles style = DefaultParseStyle, IFormatProvider formatProvider = null)
        {
            throw new NotImplementedException();
        }

        public static bool TryParse(string s, out Half result)
        {
            return TryParse(s, DefaultParseStyle, formatProvider: null, out result);
        }

        public static bool TryParse(ReadOnlySpan<char> s, out Half result)
        {
            return TryParse(s, DefaultParseStyle, formatProvider: null, out result);
        }

        public static bool TryParse(string s, NumberStyles style, IFormatProvider formatProvider, out Half result)
        {
            return TryParse(s.AsSpan(), style, formatProvider, out result);
        }

        public static bool TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider formatProvider, out Half result)
        {
            throw new NotImplementedException();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool AreZero(Half left, Half right)
        {
            // IEEE defines that positive and negative zero are equal, this gives us a quick equality check
            // for two values by or'ing the private bits together and stripping the sign. They are both zero,
            // and therefore equivalent, if the resulting value is still zero.
            return (ushort)((left.m_value | right.m_value) & ~SignMask) == 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool IsNaNOrZero(Half value)
        {
            return ((value.m_value - 1) & ~SignMask) >= PositiveInfinityBits;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static ushort StripSign(Half value)
        {
            return (ushort)(value.m_value & ~SignMask);
        }

        public int CompareTo(object obj)
        {
            if (!(obj is Half))
            {
                return (obj is null) ? 1 : throw new ArgumentException(Strings.Arg_MustBeHalf);
            }
            return CompareTo((Half)(obj));
        }

        public int CompareTo(Half other)
        {
            if ((short)(m_value) < (short)(other.m_value))
            {
                return -1;
            }

            if ((short)(m_value) > (short)(other.m_value))
            {
                return 1;
            }

            if (m_value == other.m_value)
            {
                return 0;
            }

            if (IsNaN(this))
            {
                return IsNaN(other) ? 0 : -1;
            }

            Debug.Assert(IsNaN(other));
            return 1;
        }

        public override bool Equals(object obj)
        {
            return (obj is Half) && Equals((Half)(obj));
        }

        public bool Equals(Half other)
        {
            return (this == other) || (IsNaN(this) && IsNaN(other));
        }

        public override int GetHashCode()
        {
            if (IsNaNOrZero(this))
            {
                // All NaNs should have the same hash code, as should both Zeros.
                return m_value & PositiveInfinityBits;
            }
            return m_value;
        }

        public override string ToString()
        {
            return $"0x{m_value:X4}";
            // return ToString(format: null, formatProvider: null);
            // TODO: Implement this
        }

        public string ToString(string format = null, IFormatProvider formatProvider = null)
        {
            return $"0x{m_value:X4}";
            // throw new NotImplementedException();
            // TODO: Implement this
        }

        public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider formatProvider)
        {
            throw new NotImplementedException();
        }

        // -----------------------Start of to-half conversions-------------------------

        public static implicit operator Half(int value)
        {
            bool sign = value < 0;
            Half h = (uint)(sign ? -value : value); // Math.Abs but doesn't throw exception, because we cast it to uint anyway
            return sign ? new Half((ushort)(h.m_value | SignMask)) : h;
        }

        public static implicit operator Half(uint value)
        {
            int shiftDist = BitOperations.LeadingZeroCount(value) - 21;
            if (shiftDist >= 0)
            {
                return value != 0 ?
                    new Half(false, (ushort)(0x18 - shiftDist), (ushort)(value << shiftDist)) :
                    default;
            }

            shiftDist += 4;
            uint sig = shiftDist < 0 ? Ieee754Helpers.ShiftRightJam(value, -shiftDist) : value << shiftDist;
            return new Half(RoundPackToHalf(false, (short)(0x1C - shiftDist), (ushort)sig));
        }

        public static implicit operator Half(long value)
        {
            bool sign = value < 0;
            Half h = (ulong)(sign ? -value : value); // Math.Abs but doesn't throw exception, because we cast it to ulong anyway
            return sign ? new Half((ushort)(h.m_value | SignMask)) : h;
        }

        public static implicit operator Half(ulong value)
        {
            int shiftDist = BitOperations.LeadingZeroCount(value) - 53;

            if (shiftDist >= 0)
            {
                return value != 0 ?
                    new Half(false, (ushort)(0x18 - shiftDist), (ushort)(value << shiftDist)) :
                    default;
            }

            shiftDist += 4;
            ushort sig = (ushort)(shiftDist < 0 ? Ieee754Helpers.ShiftRightJam(value, -shiftDist) : value << shiftDist);
            return new Half(RoundPackToHalf(false, (short)(0x1C - shiftDist), sig));
        }

        public static implicit operator Half(short value)
        {
            return (int)value;
        }

        public static implicit operator Half(ushort value)
        {
            return (uint)value;
        }

        public static implicit operator Half(byte value)
        {
            return (uint)value;
        }

        public static implicit operator Half(sbyte value)
        {
            return (int)value;
        }

        public static explicit operator Half(float value)
        {
            const int singleMaxExponent = 0xFF;

            uint floatInt = Ieee754Helpers.ToUInt32(value);
            bool sign = (floatInt & Ieee754Helpers.SingleSignMask) >> Ieee754Helpers.SingleSignShift != 0;
            int exp = (int)(floatInt & Ieee754Helpers.SingleExponentMask) >> Ieee754Helpers.SingleExponentShift;
            uint sig = floatInt & Ieee754Helpers.SingleSignificandMask;

            if (exp == singleMaxExponent)
            {
                if (sig != 0) // NaN
                {
                    return Ieee754Helpers.CreateHalfNaN(sign, (ulong)sig << 41); // Shift the significand bits to the left end
                }
                return sign ? NegativeInfinity : PositiveInfinity;
            }

            uint sigHalf = sig >> 9 | ((sig & 0x1FFU) != 0 ? 1U : 0U); // RightShiftJam

            if ((exp | (int)sigHalf) == 0)
            {
                return new Half(sign, 0, 0);
            }

            return new Half(RoundPackToHalf(sign, (short)(exp - 0x71), (ushort)(sigHalf | 0x4000)));
        }
        
        public static explicit operator Half(double value)
        {
            const int doubleMaxExponent = 0x7FF;
            
            ulong doubleInt = Ieee754Helpers.ToUInt64(value);
            bool sign = (doubleInt & Ieee754Helpers.DoubleSignMask) >> Ieee754Helpers.DoubleSignShift != 0;
            int exp = (int)((doubleInt & Ieee754Helpers.DoubleExponentMask) >> Ieee754Helpers.DoubleExponentShift);
            ulong sig = doubleInt & Ieee754Helpers.DoubleSignificandMask;

            if (exp == doubleMaxExponent)
            {
                if (sig != 0) // NaN
                {
                    return Ieee754Helpers.CreateHalfNaN(sign, sig << 12); // Shift the significand bits to the left end
                }
                return sign ? NegativeInfinity : PositiveInfinity;
            }

            uint sigHalf = (uint)Ieee754Helpers.ShiftRightJam(sig, 38);
            if ((exp | (int)sigHalf) == 0)
            {
                return new Half(sign, 0, 0);
            }
            return new Half(RoundPackToHalf(sign, (short)(exp - 0x3F1), (ushort)(sigHalf | 0x4000)));
        }

        // -----------------------Start of from-half conversions-------------------------

        public static explicit operator int(Half value)
        {
            bool sign = IsNegative(value);
            int exp = value.Exponent;
            uint sig = value.Significand;

            int shiftDist = exp - 0x0F;
            if (shiftDist < 0) // Value < 1
            {
                return 0;
            }

            if (exp == MaxExponent)
            {
                return IllegalValueToInt32;
            }

            int alignedSig = (int)(sig | 0x0400) << shiftDist;
            alignedSig >>= 10;
            return sign ? -alignedSig : alignedSig;
        }

        public static explicit operator uint(Half value) // 0 for every case
        {
            bool sign = IsNegative(value);
            int exp = value.Exponent;
            uint sig = value.Significand;

            int shiftDist = exp - 0x0F;
            if (shiftDist < 0) // Value < 1
            {
                return 0;
            }

            if (exp == MaxExponent)
            {
                return IllegalValueToUInt32;
            }

            uint alignedSig = (sig | 0x0400) << shiftDist;
            alignedSig >>= 10;
            return (uint)(sign ? -(int)alignedSig : (int)alignedSig);
        }

        public static explicit operator long(Half value)
        {
            bool sign = IsNegative(value);
            int exp = value.Exponent;
            uint sig = value.Significand;

            int shiftDist = exp - 0x0F;
            if (shiftDist < 0) // value < 1
            {
                return 0;
            }

            if (exp == MaxExponent)
            {
                return IllegalValueToInt64;
            }

            int alignedSig = (int) (sig | 0x0400) << shiftDist;
            alignedSig >>= 10;
            return sign ? -alignedSig : alignedSig;
        }

        public static explicit operator ulong(Half value) // 0 for PosInfinity/NaN, long.MinValue for NegInfinity
        {
            bool sign = IsNegative(value);
            int exp = value.Exponent;
            uint sig = value.Significand;

            int shiftDist = exp - 0x0F;
            if (shiftDist < 0) // value < 1
            {
                return 0;
            }

            if (exp == MaxExponent)
            {
                return IllegalValueToUInt64;
            }

            uint alignedSig = (sig | 0x0400) << shiftDist;
            alignedSig >>= 10;
            return (ulong)(sign ? -alignedSig : alignedSig);
        }

        public static explicit operator short(Half value)
        {
            return (short)(int)value;
        }

        public static explicit operator ushort(Half value)
        {
            return (ushort)(short)(int)value;
        }

        public static explicit operator byte(Half value)
        {
            return (byte)(sbyte)(int)value;
        }

        public static explicit operator sbyte(Half value)
        {
            return (sbyte)(int)value;
        }

        public static implicit operator float(Half value)
        {
            bool sign = IsNegative(value);
            int exp = value.Exponent;
            uint sig = value.Significand;

            if (exp == MaxExponent)
            {
                if (sig != 0)
                {
                    return Ieee754Helpers.CreateSingleNaN(sign, (ulong)sig << 54);
                }
                return sign ? float.NegativeInfinity : float.PositiveInfinity;
            }

            if (exp == 0)
            {
                if (sig == 0)
                {
                    return Ieee754Helpers.CreateSingle(sign ? Ieee754Helpers.SingleSignMask : 0); // Positive / Negative zero
                }
                (exp, sig) = NormSubnormalF16Sig(sig);
                exp -= 1;
            }

            return Ieee754Helpers.CreateSingle(sign, (byte)(exp + 0x70), sig << 13);
        }

        public static implicit operator double(Half value)
        {
            bool sign = IsNegative(value);
            int exp = value.Exponent;
            uint sig = value.Significand;

            if (exp == MaxExponent)
            {
                if (sig != 0)
                {
                    return Ieee754Helpers.CreateDoubleNaN(sign, (ulong)sig << 54);
                }
                return sign ? double.NegativeInfinity : double.PositiveInfinity;
            }

            if (exp == 0)
            {
                if (sig == 0)
                {
                    return Ieee754Helpers.CreateDouble(sign ? Ieee754Helpers.DoubleSignMask : 0); // Positive / Negative zero
                }
                (exp, sig) = NormSubnormalF16Sig(sig);
                exp -= 1;
            }

            return Ieee754Helpers.CreateDouble(sign, (ushort)(exp + 0x3F0), (ulong)sig << 42);
        }

        // IEEE 754 specifies NaNs to be propagated
        public static Half operator -(Half value)
        {
            return IsNaN(value) ? value : new Half((ushort)(value.m_value ^ SignMask));
        }

        public static Half operator +(Half value)
        {
            return value;
        }

        #region Utilities

        private static ushort RoundPackToHalf(bool sign, short exp, ushort sig)
        {
            const int roundIncrement = 0x8; // Depends on rounding mode but it's always towards closest / ties to even
            int roundBits = sig & 0xF;

            if ((uint) exp >= 0x1D)
            {
                if (exp < 0)
                {
                    sig = (ushort)Ieee754Helpers.ShiftRightJam(sig, -exp);
                    exp = 0;
                }
                else if (exp > 0x1D || sig + roundIncrement >= 0x8000) // Overflow
                {
                    return sign ? NegativeInfinityBits : PositiveInfinityBits;
                }
            }

            sig = (ushort)((sig + roundIncrement) >> 4);
            sig &= (ushort)~(((roundBits ^ 8) != 0 ? 0 : 1) & 1);

            if (sig == 0)
            {
                exp = 0;
            }

            return new Half(sign, (ushort)exp, sig).m_value;
        }

        private static (int Exp, uint Sig) NormSubnormalF16Sig(uint sig)
        {
            int shiftDist = BitOperations.LeadingZeroCount(sig) - 16 - 5; // No LZCNT for 16-bit
            return (1 - shiftDist, sig << shiftDist);
        }

        #endregion
    }
}
