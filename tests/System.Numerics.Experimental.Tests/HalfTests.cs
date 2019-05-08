// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Xunit;

namespace System.Numerics.Experimental.Tests
{
    public partial class HalfTests
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static unsafe ushort HalfToUInt16Bits(Half value)
        {
            return *((ushort*)&value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static unsafe Half UInt16BitsToHalf(ushort value)
        {
            return *((Half*)&value);
        }

        [Fact]
        public static void Epsilon()
        {
            Assert.Equal(0x0001u, HalfToUInt16Bits(Half.Epsilon));
        }

        [Fact]
        public static void PositiveInfinity()
        {
            Assert.Equal(0x7C00u, HalfToUInt16Bits(Half.PositiveInfinity));
        }

        [Fact]
        public static void NegativeInfinity()
        {
            Assert.Equal(0xFC00u, HalfToUInt16Bits(Half.NegativeInfinity));
        }

        [Fact]
        public static void NaN()
        {
            Assert.Equal(0xFE00u, HalfToUInt16Bits(Half.NaN));
        }

        [Fact]
        public static void MinValue()
        {
            Assert.Equal(0xFBFFu, HalfToUInt16Bits(Half.MinValue));
        }

        [Fact]
        public static void MaxValue()
        {
            Assert.Equal(0x7BFFu, HalfToUInt16Bits(Half.MaxValue));
        }

        [Fact]
        public static void Ctor_Empty()
        {
            var value = new Half();
            Assert.Equal(0x0000, HalfToUInt16Bits(value));
        }

        public static IEnumerable<object[]> IsFinite_TestData()
        {
            yield return new object[] { Half.NegativeInfinity, false };     // Negative Infinity
            yield return new object[] { Half.MinValue, true };              // Min Negative Normal
            yield return new object[] { UInt16BitsToHalf(0x8400), true };   // Max Negative Normal
            yield return new object[] { UInt16BitsToHalf(0x83FF), true };   // Min Negative Subnormal
            yield return new object[] { UInt16BitsToHalf(0x8001), true };   // Max Negative Subnormal
            yield return new object[] { UInt16BitsToHalf(0x8000), true };   // Negative Zero
            yield return new object[] { Half.NaN, false };                  // NaN
            yield return new object[] { UInt16BitsToHalf(0x0000), true };   // Positive Zero
            yield return new object[] { Half.Epsilon, true };               // Min Positive Subnormal
            yield return new object[] { UInt16BitsToHalf(0x03FF), true };   // Max Positive Subnormal
            yield return new object[] { UInt16BitsToHalf(0x0400), true };   // Min Positive Normal
            yield return new object[] { Half.MaxValue, true };              // Max Positive Normal
            yield return new object[] { Half.PositiveInfinity, false };     // Positive Infinity
        }

        [Theory]
        [MemberData(nameof(IsFinite_TestData))]
        public static void IsFinite(Half value, bool expected)
        {
            Assert.Equal(expected, Half.IsFinite(value));
        }

        public static IEnumerable<object[]> IsInfinity_TestData()
        {
            yield return new object[] { Half.NegativeInfinity, true };      // Negative Infinity
            yield return new object[] { Half.MinValue, false };             // Min Negative Normal
            yield return new object[] { UInt16BitsToHalf(0x8400), false };  // Max Negative Normal
            yield return new object[] { UInt16BitsToHalf(0x83FF), false };  // Min Negative Subnormal
            yield return new object[] { UInt16BitsToHalf(0x8001), false };  // Max Negative Subnormal (Negative Epsilon)
            yield return new object[] { UInt16BitsToHalf(0x8000), false };  // Negative Zero
            yield return new object[] { Half.NaN, false };                  // NaN
            yield return new object[] { UInt16BitsToHalf(0x0000), false };  // Positive Zero
            yield return new object[] { Half.Epsilon, false };              // Min Positive Subnormal (Positive Epsilon)
            yield return new object[] { UInt16BitsToHalf(0x03FF), false };  // Max Positive Subnormal
            yield return new object[] { UInt16BitsToHalf(0x0400), false };  // Min Positive Normal
            yield return new object[] { Half.MaxValue, false };             // Max Positive Normal
            yield return new object[] { Half.PositiveInfinity, true };      // Positive Infinity
        }

        [Theory]
        [MemberData(nameof(IsInfinity_TestData))]
        public static void IsInfinity(Half value, bool expected)
        {
            Assert.Equal(expected, Half.IsInfinity(value));
        }

        public static IEnumerable<object[]> IsNaN_TestData()
        {
            yield return new object[] { Half.NegativeInfinity, false };     // Negative Infinity
            yield return new object[] { Half.MinValue, false };             // Min Negative Normal
            yield return new object[] { UInt16BitsToHalf(0x8400), false };  // Max Negative Normal
            yield return new object[] { UInt16BitsToHalf(0x83FF), false };  // Min Negative Subnormal
            yield return new object[] { UInt16BitsToHalf(0x8001), false };  // Max Negative Subnormal (Negative Epsilon)
            yield return new object[] { UInt16BitsToHalf(0x8000), false };  // Negative Zero
            yield return new object[] { Half.NaN, true };                   // NaN
            yield return new object[] { UInt16BitsToHalf(0x0000), false };  // Positive Zero
            yield return new object[] { Half.Epsilon, false };              // Min Positive Subnormal (Positive Epsilon)
            yield return new object[] { UInt16BitsToHalf(0x03FF), false };  // Max Positive Subnormal
            yield return new object[] { UInt16BitsToHalf(0x0400), false };  // Min Positive Normal
            yield return new object[] { Half.MaxValue, false };             // Max Positive Normal
            yield return new object[] { Half.PositiveInfinity, false };     // Positive Infinity
        }

        [Theory]
        [MemberData(nameof(IsNaN_TestData))]
        public static void IsNaN(Half value, bool expected)
        {
            Assert.Equal(expected, Half.IsNaN(value));
        }

        public static IEnumerable<object[]> IsNegative_TestData()
        {
            yield return new object[] { Half.NegativeInfinity, true };      // Negative Infinity
            yield return new object[] { Half.MinValue, true };              // Min Negative Normal
            yield return new object[] { UInt16BitsToHalf(0x8400), true };   // Max Negative Normal
            yield return new object[] { UInt16BitsToHalf(0x83FF), true };   // Min Negative Subnormal
            yield return new object[] { UInt16BitsToHalf(0x8001), true };   // Max Negative Subnormal
            yield return new object[] { UInt16BitsToHalf(0x8000), true };   // Negative Zero
            yield return new object[] { Half.NaN, true };                   // NaN
            yield return new object[] { UInt16BitsToHalf(0x0000), false };  // Positive Zero
            yield return new object[] { Half.Epsilon, false };              // Min Positive Subnormal
            yield return new object[] { UInt16BitsToHalf(0x03FF), false };  // Max Positive Subnormal
            yield return new object[] { UInt16BitsToHalf(0x0400), false };  // Min Positive Normal
            yield return new object[] { Half.MaxValue, false };             // Max Positive Normal
            yield return new object[] { Half.PositiveInfinity, false };     // Positive Infinity
        }

        [Theory]
        [MemberData(nameof(IsNegative_TestData))]
        public static void IsNegative(Half value, bool expected)
        {
            Assert.Equal(expected, Half.IsNegative(value));
        }

        public static IEnumerable<object[]> IsNegativeInfinity_TestData()
        {
            yield return new object[] { Half.NegativeInfinity, true };      // Negative Infinity
            yield return new object[] { Half.MinValue, false };             // Min Negative Normal
            yield return new object[] { UInt16BitsToHalf(0x8400), false };  // Max Negative Normal
            yield return new object[] { UInt16BitsToHalf(0x83FF), false };  // Min Negative Subnormal
            yield return new object[] { UInt16BitsToHalf(0x8001), false };  // Max Negative Subnormal (Negative Epsilon)
            yield return new object[] { UInt16BitsToHalf(0x8000), false };  // Negative Zero
            yield return new object[] { Half.NaN, false };                  // NaN
            yield return new object[] { UInt16BitsToHalf(0x0000), false };  // Positive Zero
            yield return new object[] { Half.Epsilon, false };              // Min Positive Subnormal (Positive Epsilon)
            yield return new object[] { UInt16BitsToHalf(0x03FF), false };  // Max Positive Subnormal
            yield return new object[] { UInt16BitsToHalf(0x0400), false };  // Min Positive Normal
            yield return new object[] { Half.MaxValue, false };             // Max Positive Normal
            yield return new object[] { Half.PositiveInfinity, false };     // Positive Infinity
        }

        [Theory]
        [MemberData(nameof(IsNegativeInfinity_TestData))]
        public static void IsNegativeInfinity(Half value, bool expected)
        {
            Assert.Equal(expected, Half.IsNegativeInfinity(value));
        }

        public static IEnumerable<object[]> IsNormal_TestData()
        {
            yield return new object[] { Half.NegativeInfinity, false };     // Negative Infinity
            yield return new object[] { Half.MinValue, true };              // Min Negative Normal
            yield return new object[] { UInt16BitsToHalf(0x8400), true };   // Max Negative Normal
            yield return new object[] { UInt16BitsToHalf(0x83FF), false };  // Min Negative Subnormal
            yield return new object[] { UInt16BitsToHalf(0x8001), false };  // Max Negative Subnormal
            yield return new object[] { UInt16BitsToHalf(0x8000), false };  // Negative Zero
            yield return new object[] { Half.NaN, false };                  // NaN
            yield return new object[] { UInt16BitsToHalf(0x0000), false };  // Positive Zero
            yield return new object[] { Half.Epsilon, false };              // Min Positive Subnormal
            yield return new object[] { UInt16BitsToHalf(0x03FF), false };  // Max Positive Subnormal
            yield return new object[] { UInt16BitsToHalf(0x0400), true };   // Min Positive Normal
            yield return new object[] { Half.MaxValue, true };              // Max Positive Normal
            yield return new object[] { Half.PositiveInfinity, false };     // Positive Infinity
        }

        [Theory]
        [MemberData(nameof(IsNormal_TestData))]
        public static void IsNormal(Half value, bool expected)
        {
            Assert.Equal(expected, Half.IsNormal(value));
        }

        public static IEnumerable<object[]> IsPositiveInfinity_TestData()
        {
            yield return new object[] { Half.NegativeInfinity, false };     // Negative Infinity
            yield return new object[] { Half.MinValue, false };             // Min Negative Normal
            yield return new object[] { UInt16BitsToHalf(0x8400), false };  // Max Negative Normal
            yield return new object[] { UInt16BitsToHalf(0x83FF), false };  // Min Negative Subnormal
            yield return new object[] { UInt16BitsToHalf(0x8001), false };  // Max Negative Subnormal (Negative Epsilon)
            yield return new object[] { UInt16BitsToHalf(0x8000), false };  // Negative Zero
            yield return new object[] { Half.NaN, false };                  // NaN
            yield return new object[] { UInt16BitsToHalf(0x0000), false };  // Positive Zero
            yield return new object[] { Half.Epsilon, false };              // Min Positive Subnormal (Positive Epsilon)
            yield return new object[] { UInt16BitsToHalf(0x03FF), false };  // Max Positive Subnormal
            yield return new object[] { UInt16BitsToHalf(0x0400), false };  // Min Positive Normal
            yield return new object[] { Half.MaxValue, false };             // Max Positive Normal
            yield return new object[] { Half.PositiveInfinity, true };      // Positive Infinity
        }

        [Theory]
        [MemberData(nameof(IsPositiveInfinity_TestData))]
        public static void IsPositiveInfinity(Half value, bool expected)
        {
            Assert.Equal(expected, Half.IsPositiveInfinity(value));
        }

        public static IEnumerable<object[]> IsSubnormal_TestData()
        {
            yield return new object[] { Half.NegativeInfinity, false };     // Negative Infinity
            yield return new object[] { Half.MinValue, false };             // Min Negative Normal
            yield return new object[] { UInt16BitsToHalf(0x8400), false };  // Max Negative Normal
            yield return new object[] { UInt16BitsToHalf(0x83FF), true };   // Min Negative Subnormal
            yield return new object[] { UInt16BitsToHalf(0x8001), true };   // Max Negative Subnormal
            yield return new object[] { UInt16BitsToHalf(0x8000), false };  // Negative Zero
            yield return new object[] { Half.NaN, false };                  // NaN
            yield return new object[] { UInt16BitsToHalf(0x0000), false };  // Positive Zero
            yield return new object[] { Half.Epsilon, true };               // Min Positive Subnormal
            yield return new object[] { UInt16BitsToHalf(0x03FF), true };   // Max Positive Subnormal
            yield return new object[] { UInt16BitsToHalf(0x0400), false };  // Min Positive Normal
            yield return new object[] { Half.MaxValue, false };             // Max Positive Normal
            yield return new object[] { Half.PositiveInfinity, false };     // Positive Infinity
        }

        [Theory]
        [MemberData(nameof(IsSubnormal_TestData))]
        public static void IsSubnormal(Half value, bool expected)
        {
            Assert.Equal(expected, Half.IsSubnormal(value));
        }

        public static IEnumerable<object[]> CompareTo_ThrowsArgumentException_TestData()
        {
            yield return new object[] { "a" };
            yield return new object[] { 234.0 };
        }

        [Theory]
        [MemberData(nameof(CompareTo_ThrowsArgumentException_TestData))]
        public static void CompareTo_ThrowsArgumentException(object obj)
        {
            Assert.Throws(typeof(ArgumentException), () => Half.MaxValue.CompareTo(obj));
        }

        public static IEnumerable<object[]> CompareTo_TestData()
        {
            yield return new object[] { Half.MaxValue, Half.MaxValue, 0 };
            yield return new object[] { Half.MaxValue, Half.MinValue, 1 };
            yield return new object[] { Half.Epsilon, UInt16BitsToHalf(0x8001), 1 };
            yield return new object[] { Half.MaxValue, UInt16BitsToHalf(0x0000), 1 };
            yield return new object[] { Half.MaxValue, Half.Epsilon, 1 };
            yield return new object[] { Half.MaxValue, Half.PositiveInfinity, -1 };
            yield return new object[] { Half.MinValue, Half.MaxValue, -1 };
            yield return new object[] { Half.MaxValue, Half.NaN, 1 };
            yield return new object[] { Half.NaN, Half.NaN, 0 };
            yield return new object[] { Half.NaN, UInt16BitsToHalf(0x0000), -1 };
            yield return new object[] { Half.MaxValue, null, 1 };
        }

        [Theory]
        [MemberData(nameof(CompareTo_TestData))]
        public static void CompareTo(Half value, object obj, int expected)
        {
            if (obj is Half other)
            {
                Assert.Equal(expected, Math.Sign(value.CompareTo(other)));

                if (Half.IsNaN(value) || Half.IsNaN(other))
                {
                    Assert.False(value >= other);
                    Assert.False(value > other);
                    Assert.False(value <= other);
                    Assert.False(value < other);
                }
                else
                {
                    if (expected >= 0)
                    {
                        Assert.True(value >= other);
                        Assert.False(value < other);
                    }
                    if (expected > 0)
                    {
                        Assert.True(value > other);
                        Assert.False(value <= other);
                    }
                    if (expected <= 0)
                    {
                        Assert.True(value <= other);
                        Assert.False(value > other);
                    }
                    if (expected < 0)
                    {
                        Assert.True(value < other);
                        Assert.False(value >= other);
                    }
                }
            }

            Assert.Equal(expected, Math.Sign(value.CompareTo(obj)));
        }

        public static IEnumerable<object[]> Equals_TestData()
        {
            yield return new object[] { Half.MaxValue, Half.MaxValue, true };
            yield return new object[] { Half.MaxValue, Half.MinValue, false };
            yield return new object[] { Half.MaxValue, UInt16BitsToHalf(0x0000), false };
            yield return new object[] { Half.NaN, Half.NaN, true };
            yield return new object[] { Half.MaxValue, 789.0f, false };
            yield return new object[] { Half.MaxValue, "789", false };
        }

        [Theory]
        [MemberData(nameof(Equals_TestData))]
        public static void Equals(Half value, object obj, bool expected)
        {
            if (obj is Half other)
            {
                Assert.Equal(expected, value.Equals(other));

                if (Half.IsNaN(value) && Half.IsNaN(other))
                {
                    Assert.Equal(!expected, value == other);
                    Assert.Equal(expected, value != other);
                }
                else
                {
                    Assert.Equal(expected, value == other);
                    Assert.Equal(!expected, value != other);
                }
                Assert.Equal(expected, value.GetHashCode().Equals(other.GetHashCode()));
            }
            Assert.Equal(expected, value.Equals(obj));
        }

        public static IEnumerable<object[]> ImplicitConversion_FromInt64_TestData()
        {
            yield return new object[] { 65504L, (ushort)0b0_11110_1111111111U }; // Half.MaxValue
            yield return new object[] { -65504L, (ushort)0b1_11110_1111111111U }; // Half.MinValue
            yield return new object[] { 1L, (ushort)0b0_10000_0000000000U }; // 1
            yield return new object[] { -1L, (ushort)0b1_10000_0000000000U }; // -1
            yield return new object[] { 0L, (ushort)0b0_00000_0000000000U }; // 0
            yield return new object[] { long.MaxValue, (ushort)0b0_11111_0000000000 }; // Overflow
            yield return new object[] { long.MinValue, (ushort)0b1_11111_0000000000 }; // OverFlow
            yield return new object[] { 65504L + 16, (ushort) 0b0_11111_0000000000 }; // Overflow
            yield return new object[] { -(65504L + 16), (ushort)0b1_11111_0000000000 }; // Overflow
            yield return new object[] { 65504L + 16 - 1, (ushort)0b0_11110_1111111111 }; // MaxValue
            yield return new object[] { -(65504L + 16 -1), (ushort)0b1_11110_1111111111 }; // MinValue
            yield return new object[] { 65504L + 16 + 1, (ushort) 0b0_11111_0000000000 }; // MaxValue + half the increment unit + 1, should overflow
            yield return new object[] { -(65504L + 16 + 1), (ushort)0b1_11111_0000000000 }; // MinValue - half the increment unit - 1, should overflow
        }

        [MemberData(nameof(ImplicitConversion_FromInt64_TestData))]
        [Theory]
        public static void ImplicitConversion_FromInt64(long l, ushort expected)
        {
            Half h = l;
            Assert.Equal(expected, HalfToUInt16Bits(h));
        }

        public static IEnumerable<object[]> ImplicitConversion_FromUInt64_TestData()
        {
            unchecked
            {
                yield return new object[] { 65504UL, (ushort)0b0_11110_1111111111 }; // Half.MaxValue
                yield return new object[] { (ulong)-65504L, (ushort)0b0_11111_0000000000 }; // Overflow
                yield return new object[] { 1UL, (ushort)0b0_10000_0000000000U }; // 1
                yield return new object[] { (ulong)-1L, (ushort)0b0_11111_0000000000 }; // Overflow
                yield return new object[] { 0UL, (ushort)0b0_00000_0000000000 }; // 0
                yield return new object[] { (ulong)long.MaxValue, (ushort)0b0_11111_0000000000 }; // Overflow
                yield return new object[] { (ulong)long.MinValue, (ushort)0b0_11111_0000000000 }; // OverFlow
                yield return new object[] { 65504UL + 16, (ushort)0b0_11111_0000000000 }; // Overflow
                yield return new object[] { (ulong)-(65504L + 16), (ushort)0b0_11111_0000000000 }; // Overflow
                yield return new object[] { 65504UL + 16 - 1, (ushort)0b0_11110_1111111111 }; // MaxValue
                yield return new object[] { (ulong)-(65504L + 16 - 1), (ushort)0b0_11111_0000000000 }; // OverFlow
                yield return new object[] { 65504UL + 16 + 1, (ushort)0b0_11111_0000000000 }; // MaxValue + half the increment unit + 1, should overflow
                yield return new object[] { (ulong)-(65504L + 16 + 1), (ushort)0b0_11111_0000000000 }; // Overflow
            }
        }

        [MemberData(nameof(ImplicitConversion_FromInt64_TestData))]
        [Theory]
        public static void ImplicitConversion_FromUInt64(ulong l, ushort expected)
        {
            Half h = l;
            Assert.Equal(expected, HalfToUInt16Bits(h));
        }

        public static IEnumerable<object[]> ImplicitConversion_FromInt32_TestData()
        {
            yield return new object[] { 65504, (ushort)0b0_11110_1111111111U }; // Half.MaxValue
            yield return new object[] { -65504, (ushort)0b1_11110_1111111111U }; // Half.MinValue
            yield return new object[] { 1, (ushort)0b0_10000_0000000000U }; // 1
            yield return new object[] { -1, (ushort)0b1_10000_0000000000U }; // -1
            yield return new object[] { 0, (ushort)0b0_00000_0000000000U }; // 0
            yield return new object[] { int.MaxValue, (ushort)0b0_11111_0000000000 }; // Overflow
            yield return new object[] { int.MinValue, (ushort)0b1_11111_0000000000 }; // OverFlow
            yield return new object[] { 65504 + 16, (ushort)0b0_11111_0000000000 }; // Overflow
            yield return new object[] { -(65504 + 16), (ushort)0b1_11111_0000000000 }; // Overflow
            yield return new object[] { 65504 + 16 - 1, (ushort)0b0_11110_1111111111 }; // MaxValue
            yield return new object[] { -(65504 + 16 - 1), (ushort)0b1_11110_1111111111 }; // MinValue
            yield return new object[] { 65504 + 16 + 1, (ushort)0b0_11111_0000000000 }; // MaxValue + half the increment unit + 1, should overflow
            yield return new object[] { -(65504 + 16 + 1), (ushort)0b1_11111_0000000000 }; // MinValue - half the increment unit - 1, should overflow
        }

        [MemberData(nameof(ImplicitConversion_FromInt32_TestData))]
        [Theory]
        public static void ImplicitConversion_FromInt32(int i, ushort expected)
        {
            Half h = i;
            Assert.Equal(expected, HalfToUInt16Bits(h));
        }

        public static IEnumerable<object[]> ImplicitConversion_FromUInt32_TestData()
        {
            unchecked
            {
                yield return new object[] { 65504U, (ushort)0b0_11110_1111111111 }; // Half.MaxValue
                yield return new object[] { (uint)-65504, (ushort)0b0_11111_1111111111 }; // Half.MinValue
                yield return new object[] { 1U, (ushort)0b0_10000_0000000000 }; // 1
                yield return new object[] { (uint)-1, (ushort)0b0_11111_0000000000 }; // Overflow 
                yield return new object[] { 0U, (ushort)0b0_00000_0000000000 }; // 0
                yield return new object[] { (uint)int.MaxValue, (ushort)0b0_11111_0000000000 }; // Overflow
                yield return new object[] { (uint)int.MinValue, (ushort)0b0_11111_0000000000 }; // OverFlow
                yield return new object[] { uint.MaxValue, (ushort)0b0_11111_0000000000 };
                yield return new object[] { 65504U + 16, (ushort)0b0_11111_0000000000 }; // Overflow
                yield return new object[] { (uint)-(65504 + 16), (ushort)0b0_11111_0000000000 }; // Overflow
                yield return new object[] { 65504U + 16 - 1, (ushort)0b0_11110_1111111111 }; // MaxValue
                yield return new object[] { (uint)-(65504 + 16 - 1), (ushort)0b0_11110_1111111111 }; // MinValue
                yield return new object[] { 65504U + 16 + 1, (ushort)0b0_11111_0000000000 }; // MaxValue + half the increment unit + 1, should overflow
                yield return new object[] { (uint)-(65504 + 16 + 1), (ushort)0b0_11111_0000000000 }; // MinValue - half the increment unit - 1, should overflow
            }
        }

        [MemberData(nameof(ImplicitConversion_FromUInt32_TestData))]
        [Theory]
        public static void ImplicitConversion_FromUInt32(uint i, ushort expected)
        {
            Half h = i;
            Assert.Equal(expected, HalfToUInt16Bits(h));
        }

        public static IEnumerable<object[]> ImplicitConversion_FromInt16_TestData()
        {
                yield return new object[] { (short)1U, (ushort)0b0_10000_0000000000 }; // 1
                yield return new object[] { (short)-1, (ushort)0b0_11111_0000000000 }; // Overflow 
                yield return new object[] { (short)0U, (ushort)0b0_00000_0000000000 }; // 0
                yield return new object[] { short.MaxValue, (ushort)0b0_11110_0000000000 }; // rounds to 32768
                yield return new object[] { short.MinValue, (ushort)0b1_11111_0000000000 }; // -32768
        }

        [MemberData(nameof(ImplicitConversion_FromInt16_TestData))]
        [Theory]
        public static void ImplicitConversion_FromInt16(short s, ushort expected)
        {
            Half h = s;
            Assert.Equal(expected, HalfToUInt16Bits(h));
        }

        public static IEnumerable<object[]> ImplicitConversion_FromUInt16_TestData()
        {
            unchecked
            {
                yield return new object[] { (ushort)1U, (ushort)0b0_10000_0000000000 }; // 1
                yield return new object[] { (ushort)-1, (ushort)0b0_11111_0000000000 }; // Overflow
                yield return new object[] { (ushort)0U, (ushort)0b0_00000_0000000000 }; // 0
                yield return new object[] { (ushort)short.MaxValue, (ushort)0b0_11110_0000000000 }; // rounds to 32768
                yield return new object[] { (ushort)short.MinValue, (ushort)0b0_11110_0000000000 }; // 32768
                yield return new object[] { ushort.MaxValue, (ushort)0b0_11111_0000000000 }; // Overflow
            }
        }

        [MemberData(nameof(ImplicitConversion_FromUInt16_TestData))]
        [Theory]
        public static void ImplicitConversion_FromUInt16(ushort s, ushort expected)
        {
            Half h = s;
            Assert.Equal(expected, HalfToUInt16Bits(h));
        }

        public static IEnumerable<object[]> ImplicitConversion_FromByte_TestData()
        {
            unchecked
            {
                yield return new object[] { (byte)1U, (ushort)0b0_10000_0000000000 }; // 1
                yield return new object[] { (byte)-1, (ushort)0b0_10110_1111110111 }; // 255
                yield return new object[] { (byte)0U, (ushort)0b0_00000_0000000000 }; // 0
                yield return new object[] { (byte)sbyte.MaxValue, (ushort)0b0_10101_1111110000 }; // 127
                yield return new object[] { (byte)sbyte.MinValue, (ushort)0b0_10110_0010000000 }; // 128
                yield return new object[] { byte.MaxValue, (ushort)0b0_10110_1111110111 }; // 255
            }
        }

        [MemberData(nameof(ImplicitConversion_FromByte_TestData))]
        [Theory]
        public static void ImplicitConversion_FromByte(byte b, ushort expected)
        {
            Half h = b;
            Assert.Equal(expected, HalfToUInt16Bits(h));
        }

        public static IEnumerable<object[]> ImplicitConversion_FromSByte_TestData()
        {
            yield return new object[] { (sbyte)1U, (ushort)0b0_10000_0000000000 }; // 1
            yield return new object[] { (sbyte)-1, (ushort)0b1_10000_0000000000 }; // -1
            yield return new object[] { (sbyte)0U, (ushort)0b0_00000_0000000000 }; // 0
            yield return new object[] { sbyte.MaxValue, (ushort)0b0_10101_1111110000 }; // 127
            yield return new object[] { sbyte.MinValue, (ushort)0b1_10110_0010000000 }; // -128
        }

        [MemberData(nameof(ImplicitConversion_FromByte_TestData))]
        [Theory]
        public static void ImplicitConversion_FromSByte(sbyte b, ushort expected)
        {
            Half h = b;
            Assert.Equal(expected, HalfToUInt16Bits(h));
        }

        public static void ExplicitConversion_FromSingle(float f, ushort expected)
        {
            Half h = (Half)f;
            Assert.Equal(expected, HalfToUInt16Bits(h));
        }

        public static void ExplicitConversion_FromDouble(double d, ushort expected)
        {
            Half h = (Half)d;
            Assert.Equal(expected, HalfToUInt16Bits(h));
        }

        public static void ExplicitConversion_ToInt64(ushort value, long expected)
        {
            long l = (long)UInt16BitsToHalf(value);
            Assert.Equal(expected, l);
        }

        public static void ExplicitConversion_ToUInt64(ushort value, ulong expected)
        {
            ulong l = (ulong)UInt16BitsToHalf(value);
            Assert.Equal(expected, l);
        }

        public static void ExplicitConversion_ToInt32(ushort value, int expected)
        {
            int i = (int)UInt16BitsToHalf(value);
            Assert.Equal(expected, i);
        }

        public static void ExplicitConversion_ToUInt16(ushort value, ushort expected)
        {
            uint i = (uint)UInt16BitsToHalf(value);
            Assert.Equal(expected, i);
        }

        public static void ExplicitConversion_ToInt16(ushort value, short expected)
        {
            short i = (short)UInt16BitsToHalf(value);
            Assert.Equal(expected, i);
        }

        public static void ExplicitConversion_ToByte(ushort value, byte expected)
        {
            byte i = (byte)UInt16BitsToHalf(value);
            Assert.Equal(expected, i);
        }

        public static void ExplicitConversion_ToSByte(ushort value, sbyte expected)
        {
            sbyte i = (sbyte)UInt16BitsToHalf(value);
            Assert.Equal(expected, i);
        }

        public static void ImplicitConversion_ToSingle(ushort value, float expected)
        {
            float f = UInt16BitsToHalf(value);
            Assert.Equal(expected, f);
        }

        public static void ImplicitConversion_ToDouble(ushort value, double expected)
        {
            double d = UInt16BitsToHalf(value);
            Assert.Equal(expected, d);
        }

    }
}
