using System;

namespace System.Numerics.Experimental
{
    internal static class Ieee754Helpers
    {
        public const ulong DoubleSignMask = 0x80000000_00000000;
        public const int DoubleSignShift = 63;
        public const long DoubleExponentMask = 0x7FF00000_00000000;
        public const int DoubleExponentShift = 52;
        public const ulong DoubleSignificandMask = 0x000FFFFF_FFFFFFFF;
        public const int DoubleSignificandShift = 0;

        public const uint SingleSignMask = 0x80000000;
        public const int SingleSignShift = 31;
        public const int SingleExponentMask = 0x7F800000;
        public const int SingleExponentShift = 23;
        public const uint SingleSignificandMask = 0x7FFFFF;
        public const int SingleSignificandShift = 0;

        public const ushort HalfSignMask = 0x8000;
        public const int HalfSignShift = 15;
        public const ushort HalfExponentMask = 0x7C00;
        public const int HalfExponentShift = 10;
        public const ushort HalfSignificandMask = 0x03FF;
        public const int HalfSignificandShift = 0;

        public static double CreateDouble(ulong value)
            => BitConverter.Int64BitsToDouble((long)value);

        public static float CreateSingle(uint value)
            => BitConverter.Int32BitsToSingle((int)value);

        public static unsafe Half CreateHalf(ushort value)
            => *(Half*)&value;

        public static Half CreateHalf(bool sign, ushort exp, ushort sig)
            => CreateHalf((ushort)(((sign ? 1U : 0U) << HalfSignShift) | ((uint)exp << SingleExponentShift) | sig));

        public static float CreateSingle(bool sign, byte exp, uint sig)
            => BitConverter.Int32BitsToSingle((int)(((sign ? 1U : 0U) << SingleSignShift) | ((uint)exp << SingleExponentShift) | sig));

        public static double CreateDouble(bool sign, ushort exp, ulong sig)
            => BitConverter.Int64BitsToDouble((long)(((sign ? 1UL : 0UL) << DoubleSignShift) | ((ulong)exp << DoubleExponentShift) | sig));

        public static unsafe ushort ToUInt16(Half value)
            => *(ushort*)&value;

        public static uint ToUInt32(float value)
            => (uint)BitConverter.SingleToInt32Bits(value);

        public static ulong ToUInt64(double value)
            => (ulong)BitConverter.DoubleToInt64Bits(value);

        // Significand bits should be shifted towards to the left end before calling these methods
        // Creates Quiet NaN if significand == 0
        public static Half CreateHalfNaN(bool sign, ulong significand)
        {
            const uint NaNBits = HalfExponentMask | 0x200; // Most significant significand bit

            uint signInt = (sign ? 1U : 0U) << HalfSignShift;
            uint sigInt = (uint)(significand >> 54);

            return CreateHalf((ushort)(signInt | NaNBits | sigInt));
        }

        public static float CreateSingleNaN(bool sign, ulong significand)
        {
            const uint NaNBits = SingleExponentMask | 0x400000; // Most significant significand bit

            uint signInt = (sign ? 1U : 0U) << SingleSignShift;
            uint sigInt = (uint)(significand >> 41);

            return CreateSingle(signInt | NaNBits | sigInt);
        }

        public static double CreateDoubleNaN(bool sign, ulong significand)
        {
            const ulong NaNBits = DoubleExponentMask | 0x80000_00000000; // Most significant significand bit

            ulong signInt = (sign ? 1UL : 0UL) << DoubleSignShift;
            ulong sigInt = significand >> 12;

            return CreateDouble(signInt | NaNBits | sigInt);
        }

        // TODO: Worth bringing the `ShortShiftRightJam`? looks like some perf difference only.
        // Functional difference is that dist must be [0..32), maybe as part of micro-optimisations

        // If any bits are lost by shifting, "jam" them into the LSB.
        // if dist > bit count, Will be 1 or 0 depending on i
        // (unlike bitwise operators that masks the lower 5 bits)
        public static uint ShiftRightJam(uint i, int dist)
            => dist < 31 ? (i >> dist) | (i << (-dist & 31) != 0 ? 1U : 0U) : (i != 0 ? 1U : 0U);

        public static ulong ShiftRightJam(ulong l, int dist)
            => dist < 63 ? (l >> dist) | (l << (-dist & 63) != 0 ? 1UL : 0UL) : (l != 0 ? 1UL : 0UL);
    }
}
