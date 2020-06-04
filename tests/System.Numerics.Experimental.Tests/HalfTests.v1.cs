// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Globalization;
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
            yield return new object[] { Half.NaN, Half.NaN, false };
            yield return new object[] { Half.MaxValue, 789.0f, false };
            yield return new object[] { Half.MaxValue, "789", false };
        }

        [Theory]
        [MemberData(nameof(Equals_TestData))]
        public static void Equals(Half value, object obj, bool expected)
        {
            Assert.Equal(expected, value.Equals(obj));
        }

        public static IEnumerable<object[]> ExplicitConversion_ToSingle_TestData()
        {
            (Half Original, float Expected)[] data = // Fraction is shifted left by 42, Exponent is -15 then +127 = +112
            {
                (UInt16BitsToHalf(0b0_01111_0000000000), 1f), // 1
                (UInt16BitsToHalf(0b1_01111_0000000000), -1f), // -1
                (Half.MaxValue, 65504f), // 65504
                (Half.MinValue, -65504f), // -65504
                (UInt16BitsToHalf(0b0_01011_1001100110), 0.0999755859375f), // 0.1ish
                (UInt16BitsToHalf(0b1_01011_1001100110), -0.0999755859375f), // -0.1ish
                (UInt16BitsToHalf(0b0_10100_0101000000), 42f), // 42
                (UInt16BitsToHalf(0b1_10100_0101000000), -42f), // -42
                (Half.PositiveInfinity, float.PositiveInfinity), // PosInfinity
                (Half.NegativeInfinity, float.NegativeInfinity), // NegInfinity
                (UInt16BitsToHalf(0b0_11111_1000000000), BitConverter.Int32BitsToSingle(0x7FC00000)), // Positive Quiet NaN
                (Half.NaN, float.NaN), // Negative Quiet NaN
                (UInt16BitsToHalf(0b0_11111_1010101010), BitConverter.Int32BitsToSingle(0x7FD54000)), // Positive Signalling NaN - Should preserve payload
                (UInt16BitsToHalf(0b1_11111_1010101010), BitConverter.Int32BitsToSingle(unchecked((int)0xFFD54000))), // Negative Signalling NaN - Should preserve payload
                (Half.Epsilon, 1/16777216f), // PosEpsilon = 0.000000059605...
                (-Half.Epsilon, -1/16777216f), // NegEpsilon = 0.000000059605...
                (UInt16BitsToHalf(0), 0), // 0
                (UInt16BitsToHalf(0b1_00000_0000000000), -0f), // -0 
                (UInt16BitsToHalf(0b0_10000_1001001000), 3.140625f), // 3.140625
                (UInt16BitsToHalf(0b1_10000_1001001000), -3.140625f), // -3.140625
                (UInt16BitsToHalf(0b0_10000_0101110000), 2.71875f), // 2.71875
                (UInt16BitsToHalf(0b1_10000_0101110000), -2.71875f), // -2.71875
                (UInt16BitsToHalf(0b0_01111_1000000000), 1.5f), // 1.5
                (UInt16BitsToHalf(0b1_01111_1000000000), -1.5f), // -1.5
                (UInt16BitsToHalf(0b0_01111_1000000001), 1.5009765625f), // 1.5009765625
                (UInt16BitsToHalf(0b1_01111_1000000001), -1.5009765625f), // -1.5009765625
            };

            foreach ((Half original, float expected) in data)
            {
                yield return new object[] { original, expected };
            }
        }

        [MemberData(nameof(ExplicitConversion_ToSingle_TestData))]
        [Theory]
        public static void ExplicitConversion_ToSingle(Half value, float expected) // Check the underlying bits for verifying NaNs
        {
            float f = (float)value;
            Assert.Equal(BitConverter.SingleToInt32Bits(expected), BitConverter.SingleToInt32Bits(f));
        }

        public static IEnumerable<object[]> ExplicitConversion_ToDouble_TestData()
        {
            (Half Original, double Expected)[] data = // Fraction is shifted left by 42, Exponent is -15 then +127 = +112
            {
                (UInt16BitsToHalf(0b0_01111_0000000000), 1d), // 1
                (UInt16BitsToHalf(0b1_01111_0000000000), -1d), // -1
                (Half.MaxValue, 65504d), // 65504
                (Half.MinValue, -65504d), // -65504
                (UInt16BitsToHalf(0b0_01011_1001100110), 0.0999755859375d), // 0.1ish
                (UInt16BitsToHalf(0b1_01011_1001100110), -0.0999755859375d), // -0.1ish
                (UInt16BitsToHalf(0b0_10100_0101000000), 42d), // 42
                (UInt16BitsToHalf(0b1_10100_0101000000), -42d), // -42
                (Half.PositiveInfinity, double.PositiveInfinity), // PosInfinity
                (Half.NegativeInfinity, double.NegativeInfinity), // NegInfinity
                (UInt16BitsToHalf(0b0_11111_1000000000), BitConverter.Int64BitsToDouble(0x7FF80000_00000000)), // Positive Quiet NaN
                (Half.NaN, double.NaN), // Negative Quiet NaN
                (UInt16BitsToHalf(0b0_11111_1010101010), BitConverter.Int64BitsToDouble(0x7FFAA800_00000000)), // Positive Signalling NaN - Should preserve payload
                (UInt16BitsToHalf(0b1_11111_1010101010), BitConverter.Int64BitsToDouble(unchecked((long)0xFFFAA800_00000000))), // Negative Signalling NaN - Should preserve payload
                (Half.Epsilon, 1/16777216d), // PosEpsilon = 0.000000059605...
                (-Half.Epsilon, -1/16777216d), // NegEpsilon = 0.000000059605...
                (UInt16BitsToHalf(0), 0d), // 0
                (UInt16BitsToHalf(0b1_00000_0000000000), -0d), // -0 
                (UInt16BitsToHalf(0b0_10000_1001001000), 3.140625d), // 3.140625
                (UInt16BitsToHalf(0b1_10000_1001001000), -3.140625d), // -3.140625
                (UInt16BitsToHalf(0b0_10000_0101110000), 2.71875d), // 2.71875
                (UInt16BitsToHalf(0b1_10000_0101110000), -2.71875d), // -2.71875
                (UInt16BitsToHalf(0b0_01111_1000000000), 1.5d), // 1.5
                (UInt16BitsToHalf(0b1_01111_1000000000), -1.5d), // -1.5
                (UInt16BitsToHalf(0b0_01111_1000000001), 1.5009765625d), // 1.5009765625
                (UInt16BitsToHalf(0b1_01111_1000000001), -1.5009765625d) // -1.5009765625
            };

            foreach ((Half original, double expected) in data)
            {
                yield return new object[] { original, expected };
            }
        }

        [MemberData(nameof(ExplicitConversion_ToDouble_TestData))]
        [Theory]
        public static void ExplicitConversion_ToDouble(Half value, double expected) // Check the underlying bits for verifying NaNs
        {
            double d = (double)value;
            Assert.Equal(BitConverter.DoubleToInt64Bits(expected), BitConverter.DoubleToInt64Bits(d));
        }

        // ---------- Start of To-half conversion tests ----------
        public static IEnumerable<object[]> ExplicitConversion_FromSingle_TestData()
        {
            (float, Half)[] data = 
            {
                (MathF.PI, UInt16BitsToHalf(0b0_10000_1001001000)), // 3.140625
                (MathF.E, UInt16BitsToHalf(0b0_10000_0101110000)), // 2.71875
                (-MathF.PI, UInt16BitsToHalf(0b1_10000_1001001000)), // -3.140625
                (-MathF.E, UInt16BitsToHalf(0b1_10000_0101110000)), // -2.71875
                (float.MaxValue, Half.PositiveInfinity), // Overflow
                (float.MinValue, Half.NegativeInfinity), // Overflow
                (float.NaN, Half.NaN), // Quiet Negative NaN
                (BitConverter.Int32BitsToSingle(0x7FC00000), UInt16BitsToHalf(0b0_11111_1000000000)), // Quiet Positive NaN
                (BitConverter.Int32BitsToSingle(unchecked((int)0xFFD55555)),
                    UInt16BitsToHalf(0b1_11111_1010101010)), // Signalling Negative NaN
                (BitConverter.Int32BitsToSingle(0x7FD55555), UInt16BitsToHalf(0b0_11111_1010101010)), // Signalling Positive NaN
                (float.Epsilon, UInt16BitsToHalf(0)), // Underflow
                (-float.Epsilon, UInt16BitsToHalf(0b1_00000_0000000000)), // Underflow
                (1f, UInt16BitsToHalf(0b0_01111_0000000000)), // 1
                (-1f, UInt16BitsToHalf(0b1_01111_0000000000)), // -1
                (0f, UInt16BitsToHalf(0)), // 0
                (-0f, UInt16BitsToHalf(0b1_00000_0000000000)), // -0
                (42f, UInt16BitsToHalf(0b0_10100_0101000000)), // 42
                (-42f, UInt16BitsToHalf(0b1_10100_0101000000)), // -42
                (0.1f, UInt16BitsToHalf(0b0_01011_1001100110)), // 0.0999755859375
                (-0.1f, UInt16BitsToHalf(0b1_01011_1001100110)), // -0.0999755859375
                (1.5f, UInt16BitsToHalf(0b0_01111_1000000000)), // 1.5
                (-1.5f, UInt16BitsToHalf(0b1_01111_1000000000)), // -1.5
                (1.5009765625f, UInt16BitsToHalf(0b0_01111_1000000001)), // 1.5009765625
                (-1.5009765625f, UInt16BitsToHalf(0b1_01111_1000000001)), // -1.5009765625
            };

            foreach ((float original, Half expected) in data)
            {
                yield return new object[] { original, expected };
            }
        }

        [MemberData(nameof(ExplicitConversion_FromSingle_TestData))]
        [Theory]
        public static void ExplicitConversion_FromSingle(float f, Half expected) // Check the underlying bits for verifying NaNs
        {
            Half h = (Half)f;
            Assert.Equal(HalfToUInt16Bits(expected), HalfToUInt16Bits(h));
        }

        public static IEnumerable<object[]> ExplicitConversion_FromDouble_TestData()
        {
            (double, Half)[] data =
            {
                (Math.PI, UInt16BitsToHalf(0b0_10000_1001001000)), // 3.140625
                (Math.E, UInt16BitsToHalf(0b0_10000_0101110000)), // 2.71875
                (-Math.PI, UInt16BitsToHalf(0b1_10000_1001001000)), // -3.140625
                (-Math.E, UInt16BitsToHalf(0b1_10000_0101110000)), // -2.71875
                (double.MaxValue, Half.PositiveInfinity), // Overflow
                (double.MinValue, Half.NegativeInfinity), // Overflow
                (double.NaN, Half.NaN), // Quiet Negative NaN
                (BitConverter.Int64BitsToDouble(0x7FF80000_00000000),
                    UInt16BitsToHalf(0b0_11111_1000000000)), // Quiet Positive NaN
                (BitConverter.Int64BitsToDouble(unchecked((long)0xFFFAAAAA_AAAAAAAA)),
                    UInt16BitsToHalf(0b1_11111_1010101010)), // Signalling Negative NaN
                (BitConverter.Int64BitsToDouble(0x7FFAAAAA_AAAAAAAA),
                    UInt16BitsToHalf(0b0_11111_1010101010)), // Signalling Positive NaN
                (double.Epsilon, UInt16BitsToHalf(0)), // Underflow
                (-double.Epsilon, UInt16BitsToHalf(0b1_00000_0000000000)), // Underflow
                (1d, UInt16BitsToHalf(0b0_01111_0000000000)), // 1
                (-1d, UInt16BitsToHalf(0b1_01111_0000000000)), // -1
                (0d, UInt16BitsToHalf(0)), // 0
                (-0d, UInt16BitsToHalf(0b1_00000_0000000000)), // -0
                (42d, UInt16BitsToHalf(0b0_10100_0101000000)), // 42
                (-42d, UInt16BitsToHalf(0b1_10100_0101000000)), // -42
                (0.1d, UInt16BitsToHalf(0b0_01011_1001100110)), // 0.0999755859375
                (-0.1d, UInt16BitsToHalf(0b1_01011_1001100110)), // -0.0999755859375
                (1.5d, UInt16BitsToHalf(0b0_01111_1000000000)), // 1.5
                (-1.5d, UInt16BitsToHalf(0b1_01111_1000000000)), // -1.5
                (1.5009765625d, UInt16BitsToHalf(0b0_01111_1000000001)), // 1.5009765625
                (-1.5009765625d, UInt16BitsToHalf(0b1_01111_1000000001)), // -1.5009765625
            };

            foreach ((double original, Half expected) in data)
            {
                yield return new object[] { original, expected };
            }
        }

        [MemberData(nameof(ExplicitConversion_FromDouble_TestData))]
        [Theory]
        public static void ExplicitConversion_FromDouble(double d, Half expected) // Check the underlying bits for verifying NaNs
        {
            Half h = (Half)d;
            Assert.Equal(HalfToUInt16Bits(expected), HalfToUInt16Bits(h));
        }

        public static IEnumerable<object[]> Parse_TestData()
        {
            yield return new object[] { "123.456e-2", CultureInfo.CurrentCulture, 1.23456 };
            yield return new object[] { "-123.456e-2", CultureInfo.CurrentCulture, -1.23456 };
        }

        [Theory]
        [MemberData(nameof(Parse_TestData))]
        public static void ParseTests(string value, IFormatProvider provider, float expected)
        {
            // The Parse method just relies on float.Parse, so the test is really just testing the constructor again
            Half actual = Half.Parse(value, formatProvider: provider);
            Assert.Equal(expected, (float)actual, precision: 1);
        }

        public static IEnumerable<object[]> RoundTripFloat_CornerCases()
        {
            // Magnitude smaller than 2^-24 maps to 0
            yield return new object[] { (TableBasedHalf)(5.2e-20f), 0 };
            yield return new object[] { (TableBasedHalf)(-5.2e-20f), 0 };
            // Magnitude smaller than 2^(map to subnormals
            yield return new object[] { (TableBasedHalf)(1.52e-5f), 1.52e-5f };
            yield return new object[] { (TableBasedHalf)(-1.52e-5f), -1.52e-5f };
            // Normal numbers
            yield return new object[] { (TableBasedHalf)(55.77f), 55.75f };
            yield return new object[] { (TableBasedHalf)(-55.77f), -55.75f };
            // Magnitude smaller than 2^(map to infinity
            yield return new object[] { (TableBasedHalf)(1.7e38f), float.PositiveInfinity };
            yield return new object[] { (TableBasedHalf)(-1.7e38f), float.NegativeInfinity };
            // Infinity and NaN map to infinity and Nan
            yield return new object[] { TableBasedHalf.PositiveInfinity, float.PositiveInfinity };
            yield return new object[] { TableBasedHalf.NegativeInfinity, float.NegativeInfinity };
            yield return new object[] { TableBasedHalf.NaN, float.NaN };
        }

        [Theory]
        [MemberData(nameof(RoundTripFloat_CornerCases))]
        public static void ToSingle(TableBasedHalf half, float verify)
        {
            float f = (float)half;
            Assert.Equal(f, verify, precision: 1);
        }
    }
}
