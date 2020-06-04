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
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public readonly partial struct TableBasedHalf : IComparable, IFormattable, IComparable<TableBasedHalf>, IEquatable<TableBasedHalf>
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

        public static readonly TableBasedHalf Epsilon = new TableBasedHalf(EpsilonBits);                        //  5.9605E-08

        public static readonly TableBasedHalf PositiveInfinity = new TableBasedHalf(PositiveInfinityBits);      //  1.0 / 0.0
        public static readonly TableBasedHalf NegativeInfinity = new TableBasedHalf(NegativeInfinityBits);      // -1.0 / 0.0

        public static readonly TableBasedHalf NaN = new TableBasedHalf(NegativeQNaNBits);                       //  0.0 / 0.0

        public static readonly TableBasedHalf MinValue = new TableBasedHalf(MinValueBits);                      // -65504
        public static readonly TableBasedHalf MaxValue = new TableBasedHalf(MaxValueBits);                      //  65504

        // We use these explicit definitions to avoid the confusion between 0.0 and -0.0.
        private static readonly TableBasedHalf PositiveZero = new TableBasedHalf(PositiveZeroBits);            //  0.0
        private static readonly TableBasedHalf NegativeZero = new TableBasedHalf(NegativeZeroBits);            // -0.0

        private readonly ushort m_value; // Do not rename (binary serialization)

        private TableBasedHalf(ushort value)
        {
            m_value = value;
        }

        private TableBasedHalf(bool sign, ushort exp, ushort sig)
            => m_value = (ushort)(((sign ? 1 : 0) << SignShift) + (exp << ExponentShift) + sig);

        private TableBasedHalf(float single)
        {
            uint value = (uint)BitConverter.SingleToInt32Bits(single);
            m_value = (ushort)(s_baseTable[(value >> 23) & 0x1ff] + ((value & 0x007fffff) >> s_shiftTable[value >> 23]));
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

        // <summary>Determines whether the specified value is finite (zero, subnormal, or normal).</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsFinite(TableBasedHalf value)
        {
            return StripSign(value) < PositiveInfinityBits;
        }

        /// <summary>Determines whether the specified value is infinite.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsInfinity(TableBasedHalf value)
        {
            return StripSign(value) == PositiveInfinityBits;
        }

        /// <summary>Determines whether the specified value is NaN.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsNaN(TableBasedHalf value)
        {
            return StripSign(value) > PositiveInfinityBits;
        }

        /// <summary>Determines whether the specified value is negative.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsNegative(TableBasedHalf value)
        {
            return (short)(value.m_value) < 0;
        }

        /// <summary>Determines whether the specified value is negative infinity.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsNegativeInfinity(TableBasedHalf value)
        {
            return value.m_value == NegativeInfinityBits;
        }

        /// <summary>Determines whether the specified value is normal.</summary>
        // This is probably not worth inlining, it has branches and should be rarely called
        public static bool IsNormal(TableBasedHalf value)
        {
            int absValue = StripSign(value);
            return (absValue < PositiveInfinityBits)    // is finite
                && (absValue != 0)                      // is not zero
                && ((absValue & ExponentMask) != 0);    // is not subnormal (has a non-zero exponent)
        }

        /// <summary>Determines whether the specified value is positive infinity.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsPositiveInfinity(TableBasedHalf value)
        {
            return value.m_value == PositiveInfinityBits;
        }

        /// <summary>Determines whether the specified value is subnormal.</summary>
        // This is probably not worth inlining, it has branches and should be rarely called
        public static bool IsSubnormal(TableBasedHalf value)
        {
            int absValue = StripSign(value);
            return (absValue < PositiveInfinityBits)    // is finite
                && (absValue != 0)                      // is not zero
                && ((absValue & ExponentMask) == 0);    // is subnormal (has a zero exponent)
        }

        public static TableBasedHalf Parse(string s, NumberStyles style = DefaultParseStyle, IFormatProvider formatProvider = null)
        {
            if (s is null)
            {
                throw new ArgumentNullException(nameof(s));
            }
            return Parse(s.AsSpan(), style, formatProvider);
        }

        public static TableBasedHalf Parse(ReadOnlySpan<char> s, NumberStyles style = DefaultParseStyle, IFormatProvider formatProvider = null)
        {
            return (TableBasedHalf)(float.Parse(s, style, formatProvider));
        }

        public static bool TryParse(string s, out TableBasedHalf result)
        {
            return TryParse(s, DefaultParseStyle, formatProvider: null, out result);
        }

        public static bool TryParse(ReadOnlySpan<char> s, out TableBasedHalf result)
        {
            return TryParse(s, DefaultParseStyle, formatProvider: null, out result);
        }

        public static bool TryParse(string s, NumberStyles style, IFormatProvider formatProvider, out TableBasedHalf result)
        {
            return TryParse(s.AsSpan(), style, formatProvider, out result);
        }

        public static bool TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider formatProvider, out TableBasedHalf result)
        {
            bool ret = false;
            if (float.TryParse(s, style, formatProvider, out float floatResult))
            {
                result = (TableBasedHalf)floatResult;
                ret = true;
            }
            else
            {
                result = new TableBasedHalf();
            }
            return ret;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool AreZero(TableBasedHalf left, TableBasedHalf right)
        {
            // IEEE defines that positive and negative zero are equal, this gives us a quick equality check
            // for two values by or'ing the private bits together and stripping the sign. They are both zero,
            // and therefore equivalent, if the resulting value is still zero.
            return (ushort)((left.m_value | right.m_value) & ~SignMask) == 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool IsNaNOrZero(TableBasedHalf value)
        {
            return ((value.m_value - 1) & ~SignMask) >= PositiveInfinityBits;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static ushort StripSign(TableBasedHalf value)
        {
            return (ushort)(value.m_value & ~SignMask);
        }

        public int CompareTo(object obj)
        {
            if (!(obj is TableBasedHalf))
            {
                return (obj is null) ? 1 : throw new ArgumentException(Strings.Arg_MustBeHalf);
            }
            return CompareTo((TableBasedHalf)(obj));
        }

        public int CompareTo(TableBasedHalf other)
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
            return (obj is TableBasedHalf) && Equals((TableBasedHalf)(obj));
        }

        public bool Equals(TableBasedHalf other)
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
            return ToString(format: null, formatProvider: null);
        }

        public string ToString(string format = null, IFormatProvider formatProvider = null)
        {
            return ((float)this).ToString(format, formatProvider);
        }

        public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider formatProvider)
        {
            return ((float)this).TryFormat(destination, out charsWritten, format, formatProvider);
        }

        public static explicit operator TableBasedHalf(float value) => new TableBasedHalf(value);

        public static explicit operator TableBasedHalf(double value) => new TableBasedHalf((float)value);

        public static explicit operator float(TableBasedHalf half)
        {
            uint result = s_mantissaTable[offsetTable[half.m_value >> 10] + (half.m_value & 0x3ff)] + s_exponentTable[half.m_value >> 10];
            return BitConverter.ToSingle(BitConverter.GetBytes(result));
        }

        public static explicit operator double(TableBasedHalf half) => (float)half;

        public static bool operator <(TableBasedHalf left, TableBasedHalf right)
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

        public static bool operator >(TableBasedHalf left, TableBasedHalf right)
        {
            return right < left;
        }

        public static bool operator <=(TableBasedHalf left, TableBasedHalf right)
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

        public static bool operator >=(TableBasedHalf left, TableBasedHalf right)
        {
            return right <= left;
        }

        public static bool operator ==(TableBasedHalf left, TableBasedHalf right)
        {
            if (IsNaN(left) || IsNaN(right))
            {
                // IEEE defines that NaN is not equal to anything, including itself.
                return false;
            }

            // IEEE defines that positive and negative zero are equivalent.
            return (left.m_value == right.m_value) || AreZero(left, right);
        }

        public static bool operator !=(TableBasedHalf left, TableBasedHalf right)
        {
            return !(left == right);
        }
    }
}
