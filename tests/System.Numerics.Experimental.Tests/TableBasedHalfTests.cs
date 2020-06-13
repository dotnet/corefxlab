// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.Runtime.CompilerServices;
using Xunit;

namespace System.Numerics.Experimental.Tests
{
    public partial class TableBasedHalfTests
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static unsafe ushort HalfToUInt16Bits(TableBasedHalf value)
        {
            return *((ushort*)&value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static unsafe TableBasedHalf UInt16BitsToTableBasedHalf(ushort value)
        {
            return *((TableBasedHalf*)&value);
        }

        [Fact]
        public static void Epsilon()
        {
            Assert.Equal(0x0001u, HalfToUInt16Bits(TableBasedHalf.Epsilon));
        }

        [Fact]
        public static void PositiveInfinity()
        {
            Assert.Equal(0x7C00u, HalfToUInt16Bits(TableBasedHalf.PositiveInfinity));
        }

        [Fact]
        public static void NegativeInfinity()
        {
            Assert.Equal(0xFC00u, HalfToUInt16Bits(TableBasedHalf.NegativeInfinity));
        }

        [Fact]
        public static void NaN()
        {
            Assert.Equal(0xFE00u, HalfToUInt16Bits(TableBasedHalf.NaN));
        }

        [Fact]
        public static void MinValue()
        {
            Assert.Equal(0xFBFFu, HalfToUInt16Bits(TableBasedHalf.MinValue));
        }

        [Fact]
        public static void MaxValue()
        {
            Assert.Equal(0x7BFFu, HalfToUInt16Bits(TableBasedHalf.MaxValue));
        }

        [Fact]
        public static void Ctor_Empty()
        {
            var value = new TableBasedHalf();
            Assert.Equal(0x0000, HalfToUInt16Bits(value));
        }

        public static IEnumerable<object[]> IsFinite_TestData()
        {
            yield return new object[] { TableBasedHalf.NegativeInfinity, false };     // Negative Infinity
            yield return new object[] { TableBasedHalf.MinValue, true };              // Min Negative Normal
            yield return new object[] { UInt16BitsToTableBasedHalf(0x8400), true };   // Max Negative Normal
            yield return new object[] { UInt16BitsToTableBasedHalf(0x83FF), true };   // Min Negative Subnormal
            yield return new object[] { UInt16BitsToTableBasedHalf(0x8001), true };   // Max Negative Subnormal
            yield return new object[] { UInt16BitsToTableBasedHalf(0x8000), true };   // Negative Zero
            yield return new object[] { TableBasedHalf.NaN, false };                  // NaN
            yield return new object[] { UInt16BitsToTableBasedHalf(0x0000), true };   // Positive Zero
            yield return new object[] { TableBasedHalf.Epsilon, true };               // Min Positive Subnormal
            yield return new object[] { UInt16BitsToTableBasedHalf(0x03FF), true };   // Max Positive Subnormal
            yield return new object[] { UInt16BitsToTableBasedHalf(0x0400), true };   // Min Positive Normal
            yield return new object[] { TableBasedHalf.MaxValue, true };              // Max Positive Normal
            yield return new object[] { TableBasedHalf.PositiveInfinity, false };     // Positive Infinity
        }

        [Theory]
        [MemberData(nameof(IsFinite_TestData))]
        public static void IsFinite(TableBasedHalf value, bool expected)
        {
            Assert.Equal(expected, TableBasedHalf.IsFinite(value));
        }

        public static IEnumerable<object[]> IsInfinity_TestData()
        {
            yield return new object[] { TableBasedHalf.NegativeInfinity, true };      // Negative Infinity
            yield return new object[] { TableBasedHalf.MinValue, false };             // Min Negative Normal
            yield return new object[] { UInt16BitsToTableBasedHalf(0x8400), false };  // Max Negative Normal
            yield return new object[] { UInt16BitsToTableBasedHalf(0x83FF), false };  // Min Negative Subnormal
            yield return new object[] { UInt16BitsToTableBasedHalf(0x8001), false };  // Max Negative Subnormal (Negative Epsilon)
            yield return new object[] { UInt16BitsToTableBasedHalf(0x8000), false };  // Negative Zero
            yield return new object[] { TableBasedHalf.NaN, false };                  // NaN
            yield return new object[] { UInt16BitsToTableBasedHalf(0x0000), false };  // Positive Zero
            yield return new object[] { TableBasedHalf.Epsilon, false };              // Min Positive Subnormal (Positive Epsilon)
            yield return new object[] { UInt16BitsToTableBasedHalf(0x03FF), false };  // Max Positive Subnormal
            yield return new object[] { UInt16BitsToTableBasedHalf(0x0400), false };  // Min Positive Normal
            yield return new object[] { TableBasedHalf.MaxValue, false };             // Max Positive Normal
            yield return new object[] { TableBasedHalf.PositiveInfinity, true };      // Positive Infinity
        }

        [Theory]
        [MemberData(nameof(IsInfinity_TestData))]
        public static void IsInfinity(TableBasedHalf value, bool expected)
        {
            Assert.Equal(expected, TableBasedHalf.IsInfinity(value));
        }

        public static IEnumerable<object[]> IsNaN_TestData()
        {
            yield return new object[] { TableBasedHalf.NegativeInfinity, false };     // Negative Infinity
            yield return new object[] { TableBasedHalf.MinValue, false };             // Min Negative Normal
            yield return new object[] { UInt16BitsToTableBasedHalf(0x8400), false };  // Max Negative Normal
            yield return new object[] { UInt16BitsToTableBasedHalf(0x83FF), false };  // Min Negative Subnormal
            yield return new object[] { UInt16BitsToTableBasedHalf(0x8001), false };  // Max Negative Subnormal (Negative Epsilon)
            yield return new object[] { UInt16BitsToTableBasedHalf(0x8000), false };  // Negative Zero
            yield return new object[] { TableBasedHalf.NaN, true };                   // NaN
            yield return new object[] { UInt16BitsToTableBasedHalf(0x0000), false };  // Positive Zero
            yield return new object[] { TableBasedHalf.Epsilon, false };              // Min Positive Subnormal (Positive Epsilon)
            yield return new object[] { UInt16BitsToTableBasedHalf(0x03FF), false };  // Max Positive Subnormal
            yield return new object[] { UInt16BitsToTableBasedHalf(0x0400), false };  // Min Positive Normal
            yield return new object[] { TableBasedHalf.MaxValue, false };             // Max Positive Normal
            yield return new object[] { TableBasedHalf.PositiveInfinity, false };     // Positive Infinity
        }

        [Theory]
        [MemberData(nameof(IsNaN_TestData))]
        public static void IsNaN(TableBasedHalf value, bool expected)
        {
            Assert.Equal(expected, TableBasedHalf.IsNaN(value));
        }

        public static IEnumerable<object[]> IsNegative_TestData()
        {
            yield return new object[] { TableBasedHalf.NegativeInfinity, true };      // Negative Infinity
            yield return new object[] { TableBasedHalf.MinValue, true };              // Min Negative Normal
            yield return new object[] { UInt16BitsToTableBasedHalf(0x8400), true };   // Max Negative Normal
            yield return new object[] { UInt16BitsToTableBasedHalf(0x83FF), true };   // Min Negative Subnormal
            yield return new object[] { UInt16BitsToTableBasedHalf(0x8001), true };   // Max Negative Subnormal
            yield return new object[] { UInt16BitsToTableBasedHalf(0x8000), true };   // Negative Zero
            yield return new object[] { TableBasedHalf.NaN, true };                   // NaN
            yield return new object[] { UInt16BitsToTableBasedHalf(0x0000), false };  // Positive Zero
            yield return new object[] { TableBasedHalf.Epsilon, false };              // Min Positive Subnormal
            yield return new object[] { UInt16BitsToTableBasedHalf(0x03FF), false };  // Max Positive Subnormal
            yield return new object[] { UInt16BitsToTableBasedHalf(0x0400), false };  // Min Positive Normal
            yield return new object[] { TableBasedHalf.MaxValue, false };             // Max Positive Normal
            yield return new object[] { TableBasedHalf.PositiveInfinity, false };     // Positive Infinity
        }

        [Theory]
        [MemberData(nameof(IsNegative_TestData))]
        public static void IsNegative(TableBasedHalf value, bool expected)
        {
            Assert.Equal(expected, TableBasedHalf.IsNegative(value));
        }

        public static IEnumerable<object[]> IsNegativeInfinity_TestData()
        {
            yield return new object[] { TableBasedHalf.NegativeInfinity, true };      // Negative Infinity
            yield return new object[] { TableBasedHalf.MinValue, false };             // Min Negative Normal
            yield return new object[] { UInt16BitsToTableBasedHalf(0x8400), false };  // Max Negative Normal
            yield return new object[] { UInt16BitsToTableBasedHalf(0x83FF), false };  // Min Negative Subnormal
            yield return new object[] { UInt16BitsToTableBasedHalf(0x8001), false };  // Max Negative Subnormal (Negative Epsilon)
            yield return new object[] { UInt16BitsToTableBasedHalf(0x8000), false };  // Negative Zero
            yield return new object[] { TableBasedHalf.NaN, false };                  // NaN
            yield return new object[] { UInt16BitsToTableBasedHalf(0x0000), false };  // Positive Zero
            yield return new object[] { TableBasedHalf.Epsilon, false };              // Min Positive Subnormal (Positive Epsilon)
            yield return new object[] { UInt16BitsToTableBasedHalf(0x03FF), false };  // Max Positive Subnormal
            yield return new object[] { UInt16BitsToTableBasedHalf(0x0400), false };  // Min Positive Normal
            yield return new object[] { TableBasedHalf.MaxValue, false };             // Max Positive Normal
            yield return new object[] { TableBasedHalf.PositiveInfinity, false };     // Positive Infinity
        }

        [Theory]
        [MemberData(nameof(IsNegativeInfinity_TestData))]
        public static void IsNegativeInfinity(TableBasedHalf value, bool expected)
        {
            Assert.Equal(expected, TableBasedHalf.IsNegativeInfinity(value));
        }

        public static IEnumerable<object[]> IsNormal_TestData()
        {
            yield return new object[] { TableBasedHalf.NegativeInfinity, false };     // Negative Infinity
            yield return new object[] { TableBasedHalf.MinValue, true };              // Min Negative Normal
            yield return new object[] { UInt16BitsToTableBasedHalf(0x8400), true };   // Max Negative Normal
            yield return new object[] { UInt16BitsToTableBasedHalf(0x83FF), false };  // Min Negative Subnormal
            yield return new object[] { UInt16BitsToTableBasedHalf(0x8001), false };  // Max Negative Subnormal
            yield return new object[] { UInt16BitsToTableBasedHalf(0x8000), false };  // Negative Zero
            yield return new object[] { TableBasedHalf.NaN, false };                  // NaN
            yield return new object[] { UInt16BitsToTableBasedHalf(0x0000), false };  // Positive Zero
            yield return new object[] { TableBasedHalf.Epsilon, false };              // Min Positive Subnormal
            yield return new object[] { UInt16BitsToTableBasedHalf(0x03FF), false };  // Max Positive Subnormal
            yield return new object[] { UInt16BitsToTableBasedHalf(0x0400), true };   // Min Positive Normal
            yield return new object[] { TableBasedHalf.MaxValue, true };              // Max Positive Normal
            yield return new object[] { TableBasedHalf.PositiveInfinity, false };     // Positive Infinity
        }

        [Theory]
        [MemberData(nameof(IsNormal_TestData))]
        public static void IsNormal(TableBasedHalf value, bool expected)
        {
            Assert.Equal(expected, TableBasedHalf.IsNormal(value));
        }

        public static IEnumerable<object[]> IsPositiveInfinity_TestData()
        {
            yield return new object[] { TableBasedHalf.NegativeInfinity, false };     // Negative Infinity
            yield return new object[] { TableBasedHalf.MinValue, false };             // Min Negative Normal
            yield return new object[] { UInt16BitsToTableBasedHalf(0x8400), false };  // Max Negative Normal
            yield return new object[] { UInt16BitsToTableBasedHalf(0x83FF), false };  // Min Negative Subnormal
            yield return new object[] { UInt16BitsToTableBasedHalf(0x8001), false };  // Max Negative Subnormal (Negative Epsilon)
            yield return new object[] { UInt16BitsToTableBasedHalf(0x8000), false };  // Negative Zero
            yield return new object[] { TableBasedHalf.NaN, false };                  // NaN
            yield return new object[] { UInt16BitsToTableBasedHalf(0x0000), false };  // Positive Zero
            yield return new object[] { TableBasedHalf.Epsilon, false };              // Min Positive Subnormal (Positive Epsilon)
            yield return new object[] { UInt16BitsToTableBasedHalf(0x03FF), false };  // Max Positive Subnormal
            yield return new object[] { UInt16BitsToTableBasedHalf(0x0400), false };  // Min Positive Normal
            yield return new object[] { TableBasedHalf.MaxValue, false };             // Max Positive Normal
            yield return new object[] { TableBasedHalf.PositiveInfinity, true };      // Positive Infinity
        }

        [Theory]
        [MemberData(nameof(IsPositiveInfinity_TestData))]
        public static void IsPositiveInfinity(TableBasedHalf value, bool expected)
        {
            Assert.Equal(expected, TableBasedHalf.IsPositiveInfinity(value));
        }

        public static IEnumerable<object[]> IsSubnormal_TestData()
        {
            yield return new object[] { TableBasedHalf.NegativeInfinity, false };     // Negative Infinity
            yield return new object[] { TableBasedHalf.MinValue, false };             // Min Negative Normal
            yield return new object[] { UInt16BitsToTableBasedHalf(0x8400), false };  // Max Negative Normal
            yield return new object[] { UInt16BitsToTableBasedHalf(0x83FF), true };   // Min Negative Subnormal
            yield return new object[] { UInt16BitsToTableBasedHalf(0x8001), true };   // Max Negative Subnormal
            yield return new object[] { UInt16BitsToTableBasedHalf(0x8000), false };  // Negative Zero
            yield return new object[] { TableBasedHalf.NaN, false };                  // NaN
            yield return new object[] { UInt16BitsToTableBasedHalf(0x0000), false };  // Positive Zero
            yield return new object[] { TableBasedHalf.Epsilon, true };               // Min Positive Subnormal
            yield return new object[] { UInt16BitsToTableBasedHalf(0x03FF), true };   // Max Positive Subnormal
            yield return new object[] { UInt16BitsToTableBasedHalf(0x0400), false };  // Min Positive Normal
            yield return new object[] { TableBasedHalf.MaxValue, false };             // Max Positive Normal
            yield return new object[] { TableBasedHalf.PositiveInfinity, false };     // Positive Infinity
        }

        [Theory]
        [MemberData(nameof(IsSubnormal_TestData))]
        public static void IsSubnormal(TableBasedHalf value, bool expected)
        {
            Assert.Equal(expected, TableBasedHalf.IsSubnormal(value));
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
            Assert.Throws(typeof(ArgumentException), () => TableBasedHalf.MaxValue.CompareTo(obj));
        }

        public static IEnumerable<object[]> CompareTo_TestData()
        {
            yield return new object[] { TableBasedHalf.MaxValue, TableBasedHalf.MaxValue, 0 };
            yield return new object[] { TableBasedHalf.MaxValue, TableBasedHalf.MinValue, 1 };
            yield return new object[] { TableBasedHalf.Epsilon, UInt16BitsToTableBasedHalf(0x8001), 1 };
            yield return new object[] { TableBasedHalf.MaxValue, UInt16BitsToTableBasedHalf(0x0000), 1 };
            yield return new object[] { TableBasedHalf.MaxValue, TableBasedHalf.Epsilon, 1 };
            yield return new object[] { TableBasedHalf.MaxValue, TableBasedHalf.PositiveInfinity, -1 };
            yield return new object[] { TableBasedHalf.MinValue, TableBasedHalf.MaxValue, -1 };
            yield return new object[] { TableBasedHalf.MaxValue, TableBasedHalf.NaN, 1 };
            yield return new object[] { TableBasedHalf.NaN, TableBasedHalf.NaN, 0 };
            yield return new object[] { TableBasedHalf.NaN, UInt16BitsToTableBasedHalf(0x0000), -1 };
            yield return new object[] { TableBasedHalf.MaxValue, null, 1 };
        }

        [Theory]
        [MemberData(nameof(CompareTo_TestData))]
        public static void CompareTo(TableBasedHalf value, object obj, int expected)
        {
            Assert.Equal(expected, Math.Sign(value.CompareTo(obj)));
        }

        public static IEnumerable<object[]> Equals_TestData()
        {
            yield return new object[] { TableBasedHalf.MaxValue, TableBasedHalf.MaxValue, true };
            yield return new object[] { TableBasedHalf.MaxValue, TableBasedHalf.MinValue, false };
            yield return new object[] { TableBasedHalf.MaxValue, UInt16BitsToTableBasedHalf(0x0000), false };
            yield return new object[] { TableBasedHalf.NaN, TableBasedHalf.NaN, false };
            yield return new object[] { TableBasedHalf.MaxValue, 789.0f, false };
            yield return new object[] { TableBasedHalf.MaxValue, "789", false };
        }

        [Theory]
        [MemberData(nameof(Equals_TestData))]
        public static void Equals(TableBasedHalf value, object obj, bool expected)
        {
            Assert.Equal(expected, value.Equals(obj));
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

        public static IEnumerable<object[]> FromSingle_TestData()
        {
            // Magnitude smaller than 2^-24 maps to 0
            yield return new object[] { (TableBasedHalf)(5.2e-20f), 0, 0, false };
            yield return new object[] { (TableBasedHalf)(-5.2e-20f), 0, 0, true };
            // Magnitude smaller than 2^(map to subnf)rmals
            yield return new object[] { (TableBasedHalf)(1.52e-5f), 0, 255, false };
            yield return new object[] { (TableBasedHalf)(1.52e-5f), 0, 255, true };
            // Normal numbers
            yield return new object[] { (TableBasedHalf)(55.77f), 20, 760, false };
            yield return new object[] { (TableBasedHalf)(-55.77f), 20, 760, true };
            // Magnitude smaller than 2^(map to infif)ity
            yield return new object[] { (TableBasedHalf)(1.7e38f), 31, 0, false };
            yield return new object[] { (TableBasedHalf)(-1.7e38f), 31, 0, true };
            // Infinity and NaN map to infinity and Nan
            yield return new object[] { TableBasedHalf.PositiveInfinity, 31, 0, false };
            yield return new object[] { TableBasedHalf.NegativeInfinity, 31, 0, true };
            yield return new object[] { TableBasedHalf.NaN, 31, 512, true };
        }

        public static IEnumerable<object[]> ExplicitConversion_FromSingle_TestData()
        {
            yield return new object[] { MathF.PI, UInt16BitsToTableBasedHalf(0b0_10000_1001001000) }; // 3.140625
            yield return new object[] { MathF.E, UInt16BitsToTableBasedHalf(0b0_10000_0101101111) }; // 2.71875
            yield return new object[] { -MathF.PI, UInt16BitsToTableBasedHalf(0b1_10000_1001001000) }; // -3.140625
            yield return new object[] { -MathF.E, UInt16BitsToTableBasedHalf(0b1_10000_0101101111) }; // -2.71875
            yield return new object[] { float.MaxValue, TableBasedHalf.PositiveInfinity }; // Overflow
            yield return new object[] { float.MinValue, TableBasedHalf.NegativeInfinity }; // Overflow
            yield return new object[] { float.NaN, TableBasedHalf.NaN }; // Quiet Negative NaN
            yield return new object[] { BitConverter.Int32BitsToSingle(0x7FC00000), UInt16BitsToTableBasedHalf(0b0_11111_1000000000) }; // Quiet Positive NaN
            yield return new object[] { BitConverter.Int32BitsToSingle(unchecked((int)0xFFD55555)), UInt16BitsToTableBasedHalf(0b1_11111_1010101010) }; // Signalling Negative NaN
            yield return new object[] { BitConverter.Int32BitsToSingle(0x7FD55555), UInt16BitsToTableBasedHalf(0b0_11111_1010101010) }; // Signalling Positive NaN
            yield return new object[] { float.Epsilon, UInt16BitsToTableBasedHalf(0) }; // Underflow
            yield return new object[] { -float.Epsilon, UInt16BitsToTableBasedHalf(0b1_00000_0000000000) }; // Underflow
            yield return new object[] { 1f, UInt16BitsToTableBasedHalf(0b0_01111_0000000000) }; // 1
            yield return new object[] { -1f, UInt16BitsToTableBasedHalf(0b1_01111_0000000000) }; // -1
            yield return new object[] { 0f, UInt16BitsToTableBasedHalf(0) }; // 0
            yield return new object[] { -0f, UInt16BitsToTableBasedHalf(0b1_00000_0000000000) }; // -0
            yield return new object[] { 42f, UInt16BitsToTableBasedHalf(0b0_10100_0101000000) }; // 42
            yield return new object[] { -42f, UInt16BitsToTableBasedHalf(0b1_10100_0101000000) }; // -42
            yield return new object[] { 0.1f, UInt16BitsToTableBasedHalf(0b0_01011_1001100110) }; // 0.0999755859375
            yield return new object[] { -0.1f, UInt16BitsToTableBasedHalf(0b1_01011_1001100110) }; // -0.0999755859375
            yield return new object[] { 1.5f, UInt16BitsToTableBasedHalf(0b0_01111_1000000000) }; // 1.5
            yield return new object[] { -1.5f, UInt16BitsToTableBasedHalf(0b1_01111_1000000000) }; // -1.5
            yield return new object[] { 1.5009765625f, UInt16BitsToTableBasedHalf(0b0_01111_1000000001) }; // 1.5009765625
            yield return new object[] { -1.5009765625f, UInt16BitsToTableBasedHalf(0b1_01111_1000000001) }; // -1.5009765625
        }

        [MemberData(nameof(ExplicitConversion_FromSingle_TestData))]
        [Theory]
        public static void ExplicitConversion_FromSingle(float f, TableBasedHalf expected) // Check the underlying bits for verifying NaNs
        {
            TableBasedHalf h = (TableBasedHalf)f;
            Assert.Equal(HalfToUInt16Bits(expected), HalfToUInt16Bits(h));
        }

        public static IEnumerable<object[]> ExplicitConversion_FromDouble_TestData()
        {
            yield return new object[] { MathF.PI, UInt16BitsToTableBasedHalf(0b0_10000_1001001000) }; // 3.140625
            yield return new object[] { MathF.E, UInt16BitsToTableBasedHalf(0b0_10000_0101101111) }; // 2.71875
            yield return new object[] { -MathF.PI, UInt16BitsToTableBasedHalf(0b1_10000_1001001000) }; // -3.140625
            yield return new object[] { -MathF.E, UInt16BitsToTableBasedHalf(0b1_10000_0101101111) }; // -2.71875
            yield return new object[] { double.MaxValue, TableBasedHalf.PositiveInfinity }; // Overflow
            yield return new object[] { double.MinValue, TableBasedHalf.NegativeInfinity }; // Overflow
            yield return new object[] { double.NaN, TableBasedHalf.NaN }; // Quiet Negative NaN
            yield return new object[] { BitConverter.Int32BitsToSingle(0x7FC00000), UInt16BitsToTableBasedHalf(0b0_11111_1000000000) }; // Quiet Positive NaN
            yield return new object[] { BitConverter.Int32BitsToSingle(unchecked((int)0xFFD55555)), UInt16BitsToTableBasedHalf(0b1_11111_1010101010) }; // Signalling Negative NaN
            yield return new object[] { BitConverter.Int32BitsToSingle(0x7FD55555), UInt16BitsToTableBasedHalf(0b0_11111_1010101010) }; // Signalling Positive NaN
            yield return new object[] { double.Epsilon, UInt16BitsToTableBasedHalf(0) }; // Underflow
            yield return new object[] { -double.Epsilon, UInt16BitsToTableBasedHalf(0b1_00000_0000000000) }; // Underflow
            yield return new object[] { 1d, UInt16BitsToTableBasedHalf(0b0_01111_0000000000) }; // 1
            yield return new object[] { -1d, UInt16BitsToTableBasedHalf(0b1_01111_0000000000) }; // -1
            yield return new object[] { 0d, UInt16BitsToTableBasedHalf(0) }; // 0
            yield return new object[] { -0d, UInt16BitsToTableBasedHalf(0b1_00000_0000000000) }; // -0
            yield return new object[] { 42d, UInt16BitsToTableBasedHalf(0b0_10100_0101000000) }; // 42
            yield return new object[] { -42d, UInt16BitsToTableBasedHalf(0b1_10100_0101000000) }; // -42
            yield return new object[] { 0.1d, UInt16BitsToTableBasedHalf(0b0_01011_1001100110) }; // 0.0999755859375
            yield return new object[] { -0.1d, UInt16BitsToTableBasedHalf(0b1_01011_1001100110) }; // -0.0999755859375
            yield return new object[] { 1.5d, UInt16BitsToTableBasedHalf(0b0_01111_1000000000) }; // 1.5
            yield return new object[] { -1.5d, UInt16BitsToTableBasedHalf(0b1_01111_1000000000) }; // -1.5
            yield return new object[] { 1.5009765625d, UInt16BitsToTableBasedHalf(0b0_01111_1000000001) }; // 1.5009765625
            yield return new object[] { -1.5009765625d, UInt16BitsToTableBasedHalf(0b1_01111_1000000001) }; // -1.5009765625
        }

        [MemberData(nameof(ExplicitConversion_FromDouble_TestData))]
        [Theory]
        public static void ExplicitConversion_FromDouble(double d, TableBasedHalf expected) // Check the underlying bits for verifying NaNs
        {
            TableBasedHalf h = (TableBasedHalf)d;
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
            TableBasedHalf actual = TableBasedHalf.Parse(value, formatProvider: provider);
            Assert.Equal(expected, (float)actual, precision: 1);
        }

        [Fact]
        public unsafe void TestAllHalfValues()
        {
            for (ushort i = ushort.MinValue; i < ushort.MaxValue; i++)
            {
                TableBasedHalf half1 = UInt16BitsToTableBasedHalf(i);
                TableBasedHalf half2 = (TableBasedHalf)((float)half1);

                bool half1IsNaN = TableBasedHalf.IsNaN(half1);
                bool half2IsNaN = TableBasedHalf.IsNaN(half2);
                if (half1IsNaN && half2IsNaN)
                {
                    continue;
                }
                Assert.Equal(half1IsNaN, half2IsNaN);
                Assert.True(half1.Equals(half2), $"{i} is wrong");
            }
        }
    }
}
