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

    public readonly partial struct Half : IComparable, IFormattable, IComparable<Half>, IEquatable<Half>
    {
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

        #endregion
    }
}
