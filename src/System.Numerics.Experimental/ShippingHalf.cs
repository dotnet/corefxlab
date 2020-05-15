// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

/*============================================================
**
**
**
** Purpose: An IEEE 768 compliant float16 type
**
**
===========================================================*/
using System.Diagnostics;
using System.Globalization;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System
{
    // ===================================================================================================
    // Portions of the code implemented below are based on the 'Berkeley SoftFloat Release 3e' algorithms.
    // ===================================================================================================

    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public readonly struct ShippingHalf : IComparable, IFormattable, IComparable<ShippingHalf>, IEquatable<ShippingHalf>
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

        private const ushort FirstSignificandBitMask = 0x0200;
        private const ushort SecondSignificandBitMask = 0x0100;
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

        public static readonly ShippingHalf Epsilon = new ShippingHalf(EpsilonBits);                        //  5.9605E-08

        public static readonly ShippingHalf PositiveInfinity = new ShippingHalf(PositiveInfinityBits);      //  1.0 / 0.0
        public static readonly ShippingHalf NegativeInfinity = new ShippingHalf(NegativeInfinityBits);      // -1.0 / 0.0

        public static readonly ShippingHalf NaN = new ShippingHalf(NegativeQNaNBits);                       //  0.0 / 0.0

        public static readonly ShippingHalf MinValue = new ShippingHalf(MinValueBits);                      // -65504
        public static readonly ShippingHalf MaxValue = new ShippingHalf(MaxValueBits);                      //  65504

        // We use these explicit definitions to avoid the confusion between 0.0 and -0.0.
        private static readonly ShippingHalf PositiveZero = new ShippingHalf(PositiveZeroBits);            //  0.0
        private static readonly ShippingHalf NegativeZero = new ShippingHalf(NegativeZeroBits);            // -0.0

        private readonly ushort m_value; // Do not rename (binary serialization)

        private ShippingHalf(ushort value)
        {
            m_value = value;
        }

        private ShippingHalf(bool sign, ushort exp, ushort sig)
            => m_value = (ushort)(((sign ? 1 : 0) << SignShift) + (exp << ExponentShift) + sig);

        public ShippingHalf(float single)
        {
            uint value = (uint)BitConverter.SingleToInt32Bits(single);
            bool sign = value >> 31 == 1;

            uint singleExponent = value >> 23 & 0xFF;
            uint singleSignificand = value & 0x007FFFFF;

            if (singleExponent == 0xFF)
            {
                if (singleSignificand > 0)
                {
                    // Quiet (1) or Signaling (0) NaN. 
                    var signV = (sign ? 1 : 0) << 31;
                    m_value = (ushort)(((sign ? 1 : 0) << 31) + (MaxExponent << ExponentShift) + singleSignificand >> 13);
                }
                else
                {
                    m_value = (ushort)(((sign ? 1 : 0) << SignShift) + (MaxExponent << ExponentShift));
                }
            }
            else
            {
                if (singleExponent == MinExponent)
                {

                }
            }


        }

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

        private bool Sign => (m_value & SignMask) >> SignShift == 1;

        public float Float
        {
            get
            {
                if (Exponent == MaxExponent)
                {
                    ushort significand = Significand;
                    if (significand > 0)
                    {
                        // Quiet (1) or Signaling (0) NaN. 
                        return (Sign ? 1 : 0) << 31 + 0xFF << 23 + significand;
                    }
                    else
                    {
                        // +- Inf
                        return (Sign ? 1 : 0) << 31 + 0xFF << 23;
                    }
                }
                else
                {
                    if (Exponent == MinExponent)
                    {
                        ushort significand = Significand;
                        if (significand > 0)
                        {
                            return (Sign ? 1 : 0) << 31 + significand;
                        }
                        else
                        {
                            return (Sign ? 1 : 0) << 31;
                        }
                    }
                    return (Sign ? 1 : 0) << 31 + Exponent + 0x70 + Significand << 13;
                }
            }
        }

        // <summary>Determines whether the specified value is finite (zero, subnormal, or normal).</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsFinite(ShippingHalf value)
        {
            return StripSign(value) < PositiveInfinityBits;
        }

        /// <summary>Determines whether the specified value is infinite.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsInfinity(ShippingHalf value)
        {
            return StripSign(value) == PositiveInfinityBits;
        }

        /// <summary>Determines whether the specified value is NaN.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsNaN(ShippingHalf value)
        {
            return StripSign(value) > PositiveInfinityBits;
        }

        /// <summary>Determines whether the specified value is negative.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsNegative(ShippingHalf value)
        {
            return (short)(value.m_value) < 0;
        }

        /// <summary>Determines whether the specified value is negative infinity.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsNegativeInfinity(ShippingHalf value)
        {
            return value.m_value == NegativeInfinityBits;
        }

        /// <summary>Determines whether the specified value is normal.</summary>
        // This is probably not worth inlining, it has branches and should be rarely called
        public static bool IsNormal(ShippingHalf value)
        {
            int absValue = StripSign(value);
            return (absValue < PositiveInfinityBits)    // is finite
                && (absValue != 0)                      // is not zero
                && ((absValue & ExponentMask) != 0);    // is not subnormal (has a non-zero exponent)
        }

        /// <summary>Determines whether the specified value is positive infinity.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsPositiveInfinity(ShippingHalf value)
        {
            return value.m_value == PositiveInfinityBits;
        }

        /// <summary>Determines whether the specified value is subnormal.</summary>
        // This is probably not worth inlining, it has branches and should be rarely called
        public static bool IsSubnormal(ShippingHalf value)
        {
            int absValue = StripSign(value);
            return (absValue < PositiveInfinityBits)    // is finite
                && (absValue != 0)                      // is not zero
                && ((absValue & ExponentMask) == 0);    // is subnormal (has a zero exponent)
        }

        public static ShippingHalf Parse(string s, NumberStyles style = DefaultParseStyle, IFormatProvider formatProvider = null)
        {
            if (s is null)
            {
                throw new ArgumentNullException(nameof(s));
            }
            return Parse(s.AsSpan(), style, formatProvider);
        }

        public static ShippingHalf Parse(ReadOnlySpan<char> s, NumberStyles style = DefaultParseStyle, IFormatProvider formatProvider = null)
        {
            throw new NotImplementedException();
        }

        public static bool TryParse(string s, out ShippingHalf result)
        {
            return TryParse(s, DefaultParseStyle, formatProvider: null, out result);
        }

        public static bool TryParse(ReadOnlySpan<char> s, out ShippingHalf result)
        {
            return TryParse(s, DefaultParseStyle, formatProvider: null, out result);
        }

        public static bool TryParse(string s, NumberStyles style, IFormatProvider formatProvider, out ShippingHalf result)
        {
            return TryParse(s.AsSpan(), style, formatProvider, out result);
        }

        public static bool TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider formatProvider, out ShippingHalf result)
        {
            throw new NotImplementedException();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool AreZero(ShippingHalf left, ShippingHalf right)
        {
            // IEEE defines that positive and negative zero are equal, this gives us a quick equality check
            // for two values by or'ing the private bits together and stripping the sign. They are both zero,
            // and therefore equivalent, if the resulting value is still zero.
            return (ushort)((left.m_value | right.m_value) & ~SignMask) == 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool IsNaNOrZero(ShippingHalf value)
        {
            return ((value.m_value - 1) & ~SignMask) >= PositiveInfinityBits;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static ushort StripSign(ShippingHalf value)
        {
            return (ushort)(value.m_value & ~SignMask);
        }

        public int CompareTo(object obj)
        {
            if (!(obj is ShippingHalf))
            {
                return (obj is null) ? 1 : throw new ArgumentException(Strings.Arg_MustBeHalf);
            }
            return CompareTo((ShippingHalf)(obj));
        }

        public int CompareTo(ShippingHalf other)
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
            return (obj is ShippingHalf) && Equals((ShippingHalf)(obj));
        }

        public bool Equals(ShippingHalf other)
        {
            if (IsNaN(this) || IsNaN(other))
            {
                // IEEE defines that NaN is not equal to anything, including itself.
                return false;
            }

            // IEEE defines that positive and negative zero are equivalent.
            return (m_value == other.m_value) || AreZero(this, other);
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

        #region Utilities

        // TODO: Replace long with uint?
        public static long ShiftRightJam(long i, int dist)
    => dist < 31 ? (i >> dist) | (i << (-dist & 31) != 0 ? 1U : 0U) : (i != 0 ? 1U : 0U);

        private static ushort RoundPackToHalf(bool sign, short exp, ushort sig)
        {
            const int roundIncrement = 0x8; // Depends on rounding mode but it's always towards closest / ties to even
            int roundBits = sig & 0xF;

            if ((uint)exp >= 0x1D)
            {
                if (exp < 0)
                {
                    sig = (ushort)ShiftRightJam(sig, -exp);
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

            return new ShippingHalf(sign, (ushort)exp, sig).m_value;
        }

        private static (int Exp, uint Sig) NormSubnormalF16Sig(uint sig)
        {
            int shiftDist = BitOperations.LeadingZeroCount(sig) - 16 - 5; // No LZCNT for 16-bit
            return (1 - shiftDist, sig << shiftDist);
        }

        #endregion
    }

}
