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
            => m_value = (ushort)((sign ? 1 : 0 << SignShift) + (exp << ExponentShift) + sig);

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
            return ToString(format: null, formatProvider: null);
        }

        public string ToString(string format = null, IFormatProvider formatProvider = null)
        {
            throw new NotImplementedException();
        }

        public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider formatProvider)
        {
            throw new NotImplementedException();
        }

        // -----------------------Start of to-half conversions-------------------------

        public static implicit operator Half(int i)
        {
            bool sign = i < 0;
            Half h = (uint)(sign ? -i : i); // Math.Abs but doesn't throw exception, because we cast it to uint anyway
            return sign ? new Half((ushort)(h.m_value | SignMask)) : h;
        }

        public static implicit operator Half(uint i)
        {
            if (i == 0)
                return default;

            int shiftDist = BitOperations.LeadingZeroCount(i) - 21;
            if (shiftDist >= 0)
                return new Half(false, (ushort)(0x18 - shiftDist), (ushort) (i << shiftDist));

            shiftDist += 4;
            uint sig = shiftDist < 0 ? ShiftRightJam(i, -shiftDist) : i << shiftDist;
            return RoundPackToHalf(false, (short) (0x1C - shiftDist), (ushort) sig);
        }

        public static implicit operator Half(long l)
        {
            bool sign = l < 0;
            Half h = (ulong)(sign ? -l : l); // Math.Abs but doesn't throw exception, because we cast it to ulong anyway
            return sign ? new Half((ushort)(h.m_value | SignMask)) : h;
        }

        public static implicit operator Half(ulong l)
        {
            if (l == 0)
                return default;

            int shiftDist = BitOperations.LeadingZeroCount(l) - 53;

            if (shiftDist >= 0)
                return new Half(false, (ushort)(0x18 - shiftDist), (ushort)(l << shiftDist));

            shiftDist += 4;
            ushort sig = (ushort)(shiftDist < 0 ? ShiftRightJam(l, -shiftDist) : l << shiftDist);
            return new Half(RoundPackToHalf(false, (short)(0x1C - shiftDist), sig));
        }

        public static implicit operator Half(short s)
        {
            return (int) s;
        }

        public static implicit operator Half(ushort s)
        {
            return (uint) s;
        }

        public static implicit operator Half(byte b)
        {
            return (uint) b;
        }

        public static implicit operator Half(sbyte b)
        {
            return (int) b;
        }

        private const uint SingleSignMask = 0x80000000;
        private const int SingleSignShiftBit = 31;
        private const int SingleExpMask = 0x7F800000;
        private const int SingleExpShiftBit = 23;
        private const uint SingleSigMask = 0x7FFFFF;
        private const int SingleSigShiftBit = 0;

        public static explicit operator Half(float f)
        {
            const int singleMaxExponent = 0xFF;

            uint floatInt = (uint)BitConverter.SingleToInt32Bits(f);
            bool sign = (floatInt & SingleSignMask) >> SingleSignShiftBit != 0;
            int exp = ((int)floatInt & SingleExpMask) >> SingleExpShiftBit;
            uint sig = floatInt & SingleSigMask;

            if (exp == singleMaxExponent)
            {
                if (sig != 0) // NaN
                    return CreateHalfNaN(sign, (ulong)sig << 41); // 41: bits required to shift the significand bits to the left end
                return sign ? NegativeInfinity : PositiveInfinity;
            }

            uint sigHalf = sig >> 9 | ((sig & 0x1FFU) != 0 ? 1U : 0U); // RightShiftJam

            if ((exp | (int)sigHalf) == 0) // TODO: is f == 0 faster?
                return new Half(sign, 0, 0);
            return RoundPackToHalf(sign, (short)(exp - 0x71), (ushort)(sigHalf | 0x4000));
        }

        private const ulong DoubleSignMask = 0x80000000_00000000;
        private const int DoubleSignShiftBit = 63;
        private const long DoubleExpMask = 0x7FF80000_00000000;
        private const int DoubleExpShiftBit = 52;
        private const ulong DoubleSigMask = 0x000FFFFF_FFFFFFFF;
        private const int DoubleSigShiftBit = 0;

        public static explicit operator Half(double d)
        {
            ulong doubleInt = (ulong)BitConverter.DoubleToInt64Bits(d);
            bool sign = (doubleInt & DoubleSignMask) >> DoubleSignShiftBit != 0;
            int exp = (int)(((long)doubleInt & DoubleExpMask) >> DoubleExpShiftBit);
            ulong sig = doubleInt & DoubleSigMask;

            if (exp == 0x7FF)
            {
                if (sig != 0) // NaN
                    return CreateHalfNaN(sign, sig << 12); // 12: bits required to shift the significand bits to the left end
                return sign ? NegativeInfinity : PositiveInfinity;
            }

            uint sigHalf = (uint)ShiftRightJam(sig, 38);
            if ((exp | (int)sigHalf) == 0) // TODO: Is d == 0 faster?
                return new Half(sign, 0, 0);
            return RoundPackToHalf(sign, (short)(exp - 0x3F1), (ushort)(sigHalf | 0x4000));
        }

        // -----------------------Start of from-half conversions-------------------------

        public static explicit operator int(Half h)
        {
            bool sign = IsNegative(h);
            int exp = h.Exponent;
            uint sig = h.Significand;

            int shiftDist = exp - 0x0F;
            if (shiftDist < 0)
                return 0;

            if (exp == MaxExponent)
                return int.MinValue; // Architecture-dependent; x86's behaviour

            int alignedSig = (int) (sig | 0x4000) << shiftDist;
            alignedSig >>= 10;
            return sign ? -alignedSig : alignedSig;
        }

        public static explicit operator uint(Half h) // 0 for every case
        {
            bool sign = IsNegative(h);
            if (sign)
                return (uint)(int)h; // Matching the behaviour of neg. float/double -> ulong
            // TODO: Confirm this behaviour when h is negative
            int exp = h.Exponent;
            uint sig = h.Significand;

            int shiftDist = exp - 0x0F;
            if (shiftDist < 0)
                return 0;

            if (exp == MaxExponent)
                return uint.MaxValue; // Architecture-dependent; x86's behaviour
            // TODO: I think it just returns 0 in C#

            uint alignedSig = (sig | 0x0400) << shiftDist;
            return alignedSig >> 10;
        }

        public static explicit operator long(Half h)
        {
            bool sign = IsNegative(h);
            int exp = h.Exponent;
            uint sig = h.Significand;

            int shiftDist = exp - 0x0F;
            if (shiftDist < 0)
                return 0;

            if (exp == MaxExponent)
                return long.MinValue; // Architecture-dependent; x86's behaviour

            int alignedSig = (int) (sig | 0x0400) << shiftDist;
            alignedSig >>= 10;
            return sign ? -alignedSig : alignedSig;
        }

        public static explicit operator ulong(Half h) // 0 for PosInfinity/NaN, long.MinValue for NegInfinity
        {
            bool sign = IsNegative(h);
            if (sign)
                return (ulong)(long)h; // Matching the behaviour of neg. float/double -> ulong
            // TODO: Confirm this behaviour when h is negative
            int exp = h.Exponent;
            uint sig = h.Significand;

            int shiftDist = exp - 0x0F;
            if (shiftDist < 0)
                return 0;

            if (exp == MaxExponent)
                return ulong.MaxValue; // Architecture-dependent; x86's behaviour
            // TODO: Need to check

            uint alignedSig = (sig | 0x0400) << shiftDist;
            return alignedSig >> 10;
        }

        // TODO: confirm behaviours
        public static explicit operator short(Half h)
        {
            return (short) (int) h;
        }

        public static explicit operator ushort(Half h)
        {
            return (ushort)(short)(int)h;
        }

        public static explicit operator byte(Half h)
        {
            return (byte)(sbyte)(int)h;
        }

        public static explicit operator sbyte(Half h)
        {
            return (sbyte)(int)h;
        } // TODO: not sure what should happen here

        public static implicit operator float(Half h)
        {
            bool sign = IsNegative(h);
            int exp = h.Exponent;
            uint sig = h.Significand;

            if (exp == MaxExponent)
            {
                if (sig != 0)
                    return CreateSingleNaN(sign, (ulong) sig << 54);
                return sign ? float.NegativeInfinity : float.PositiveInfinity;
            }

            if (exp == 0)
            {
                if (sig == 0)
                    return BitConverter.Int32BitsToSingle(unchecked((int)(sign ? SingleSignMask : 0))); // Positive / Negative zero
                (exp, sig) = NormSubnormalF16Sig(sig);
                exp -= 1;
            }

            return CreateSingle(sign, (byte)(exp + 0x70), sig << 13);
        }

        public static implicit operator double(Half h)
        {
            bool sign = IsNegative(h);
            int exp = h.Exponent;
            uint sig = h.Significand;

            if (exp == MaxExponent)
            {
                if (sig != 0)
                    return CreateDoubleNaN(sign, (ulong) sig << 54);
                return sign ? double.NegativeInfinity : double.PositiveInfinity;
            }

            if (exp == 0)
            {
                if (sig == 0)
                    return BitConverter.Int64BitsToDouble(unchecked((long) (sign ? DoubleSignMask : 0))); // Positive / Negative zero
                (exp, sig) = NormSubnormalF16Sig(sig);
                exp -= 1;
            }

            return CreateDouble(sign, (ushort)(exp + 0x3F0), sig << 42);
        }

        #region Utilities

        // TODO: Worth bringing the `ShortShiftRightJam`? looks like some perf difference only
        // If any bits are lost by shifting, "jam" them into the LSB.
        // if dist > bit count, Will be 1 or 0  depending on i 
        // (unlike bitwise operators that masks the lower 5 bits)
        private static uint ShiftRightJam(uint i, int dist)
            => dist < 31 ? i >> dist | (i << (-dist & 31) != 0 ? 1U : 0U) : (i != 0 ? 1U : 0U);

        private static ulong ShiftRightJam(ulong l, int dist)
            => dist < 63 ? l >> dist | (l << (-dist & 63) != 0 ? 1UL : 0UL) : (l != 0 ? 1UL : 0UL);

        private static ushort RoundPackToHalf(bool sign, short exp, ushort sig)
        {
            const int roundIncrement = 0x8; // Depends on rounding mode but it's always towards closest / ties to even
            int roundBits = sig & 0xF;

            if ((uint) exp >= 0x1D)
            {
                if (exp < 0)
                {
                    sig = (ushort)ShiftRightJam(sig, -exp);
                    exp = 0;
                }
                else if (exp > 0x1D || sig + roundIncrement >= 0x8000) // Overflow
                    return sign ? NegativeInfinityBits : PositiveInfinityBits;
            }

            sig = (ushort)((sig + roundIncrement) >> 4);
            sig &= (ushort)~(((roundBits ^ 8) != 0 ? 1 : 0) & 1);

            if (sig == 0)
                exp = 0;

            return new Half(sign, (ushort)exp, sig).m_value;
        }

        // Significand bits should be shifted towards to the left end before calling these methods
        // Creates Quiet NaN if significand == 0
        private static Half CreateHalfNaN(bool sign, ulong significand)
        {
            uint signInt = (sign ? 1U : 0U) << SignShift;
            const uint expInt = PositiveQNaNBits;
            uint sigInt = (uint)(significand >> 54); // 54: bits to shift to place bits at significand bits

            return new Half((ushort)(signInt | expInt | sigInt));
        }

        private static float CreateSingleNaN(bool sign, ulong significand)
        {
            uint signInt = (sign ? 1U : 0U) << SingleSignShiftBit;
            const uint expInt = 0x7FC00000;
            uint sigInt = (uint)(significand >> 41); // 41: bits to shift to place bits at significand bits

            return BitConverter.Int32BitsToSingle((int)(signInt | expInt | sigInt));
        }

        private static double CreateDoubleNaN(bool sign, ulong significand)
        {
            ulong signInt = (sign ? 1UL : 0UL) << DoubleSignShiftBit;
            const ulong expInt = 0x7FF80000_00000000;
            ulong sigInt = significand >> 12; // 12: bits to shift to place bits at significand bits

            return BitConverter.Int64BitsToDouble((long) (signInt | expInt | sigInt));
        }

        private static int RoundToInt32(bool sign, ulong sig)
        {
            const int roundIncrement = 0x800;
            uint roundBits = (uint)(sig & 0xFFF);
            sig += roundIncrement;

            if ((sig & 0xFFFFF000_00000000) != 0)
                return int.MinValue; //goto Invalid = what does it actually mean? Value too large?
            uint sig32 = (uint)(sig >> 12);

            if (roundBits == 0x800)
                sig32 &= ~1U;
            int z = (int) (sign ? (uint)-sig32 : sig32);
            if (z != 0 && (z < 0) ^ sign)
                return int.MinValue; //goto Invalid

            return z;
        }

        private static uint RoundToUInt32(bool sign, ulong sig)
        {
            const int roundIncrement = 0x800;
            uint roundBits = (uint)(sig & 0xFFF);
            sig += roundIncrement;
            if ((sig & 0xFFFFF000_00000000) != 0)
                return uint.MaxValue; //goto Invalid
            uint z = (uint) (sig >> 12);
            if (roundBits == 0x800)
                z &= ~1U;
            if (sign && z != 0)
                return uint.MaxValue; //goto invalid

            return z;
        }

        private static float CreateSingle(bool sign, byte exp, uint frac)
            => BitConverter.Int32BitsToSingle((int)(((sign ? 1U : 0U) << SingleSignShiftBit) | ((uint) exp << SingleExpShiftBit) | frac));

        private static double CreateDouble(bool sign, ushort exp, ulong frac)
            => BitConverter.Int64BitsToDouble((long)(((sign ? 1UL : 0UL) << DoubleSignShiftBit) | ((ulong)exp << DoubleExpShiftBit) | frac));

        private static (int Exp, uint Sig) NormSubnormalF16Sig(uint sig) // TODO: better names? I have no idea what this does
        {
            int shiftDist = BitOperations.LeadingZeroCount(sig) - 16 - 5; // No LZCNT for 16-bit
            return (1 - shiftDist, sig << shiftDist);
        }

        #endregion
    }
}
