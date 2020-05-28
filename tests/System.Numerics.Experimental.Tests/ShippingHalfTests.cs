// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using Xunit;

namespace System.Numerics.Experimental.Tests
{
    public partial class ShippingHalfTests
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static unsafe ushort HalfToUInt16Bits(ShippingHalf value)
        {
            return *((ushort*)&value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static unsafe ShippingHalf UInt16BitsToShippingHalf(ushort value)
        {
            return *((ShippingHalf*)&value);
        }

        [Fact]
        public static void Epsilon()
        {
            Assert.Equal(0x0001u, HalfToUInt16Bits(ShippingHalf.Epsilon));
        }

        [Fact]
        public static void PositiveInfinity()
        {
            Assert.Equal(0x7C00u, HalfToUInt16Bits(ShippingHalf.PositiveInfinity));
        }

        [Fact]
        public static void NegativeInfinity()
        {
            Assert.Equal(0xFC00u, HalfToUInt16Bits(ShippingHalf.NegativeInfinity));
        }

        [Fact]
        public static void NaN()
        {
            Assert.Equal(0xFE00u, HalfToUInt16Bits(ShippingHalf.NaN));
        }

        [Fact]
        public static void MinValue()
        {
            Assert.Equal(0xFBFFu, HalfToUInt16Bits(ShippingHalf.MinValue));
        }

        [Fact]
        public static void MaxValue()
        {
            Assert.Equal(0x7BFFu, HalfToUInt16Bits(ShippingHalf.MaxValue));
        }

        [Fact]
        public static void Ctor_Empty()
        {
            var value = new ShippingHalf();
            Assert.Equal(0x0000, HalfToUInt16Bits(value));
        }

        public static IEnumerable<object[]> IsFinite_TestData()
        {
            yield return new object[] { ShippingHalf.NegativeInfinity, false };     // Negative Infinity
            yield return new object[] { ShippingHalf.MinValue, true };              // Min Negative Normal
            yield return new object[] { UInt16BitsToShippingHalf(0x8400), true };   // Max Negative Normal
            yield return new object[] { UInt16BitsToShippingHalf(0x83FF), true };   // Min Negative Subnormal
            yield return new object[] { UInt16BitsToShippingHalf(0x8001), true };   // Max Negative Subnormal
            yield return new object[] { UInt16BitsToShippingHalf(0x8000), true };   // Negative Zero
            yield return new object[] { ShippingHalf.NaN, false };                  // NaN
            yield return new object[] { UInt16BitsToShippingHalf(0x0000), true };   // Positive Zero
            yield return new object[] { ShippingHalf.Epsilon, true };               // Min Positive Subnormal
            yield return new object[] { UInt16BitsToShippingHalf(0x03FF), true };   // Max Positive Subnormal
            yield return new object[] { UInt16BitsToShippingHalf(0x0400), true };   // Min Positive Normal
            yield return new object[] { ShippingHalf.MaxValue, true };              // Max Positive Normal
            yield return new object[] { ShippingHalf.PositiveInfinity, false };     // Positive Infinity
        }

        [Theory]
        [MemberData(nameof(IsFinite_TestData))]
        public static void IsFinite(ShippingHalf value, bool expected)
        {
            Assert.Equal(expected, ShippingHalf.IsFinite(value));
        }

        public static IEnumerable<object[]> IsInfinity_TestData()
        {
            yield return new object[] { ShippingHalf.NegativeInfinity, true };      // Negative Infinity
            yield return new object[] { ShippingHalf.MinValue, false };             // Min Negative Normal
            yield return new object[] { UInt16BitsToShippingHalf(0x8400), false };  // Max Negative Normal
            yield return new object[] { UInt16BitsToShippingHalf(0x83FF), false };  // Min Negative Subnormal
            yield return new object[] { UInt16BitsToShippingHalf(0x8001), false };  // Max Negative Subnormal (Negative Epsilon)
            yield return new object[] { UInt16BitsToShippingHalf(0x8000), false };  // Negative Zero
            yield return new object[] { ShippingHalf.NaN, false };                  // NaN
            yield return new object[] { UInt16BitsToShippingHalf(0x0000), false };  // Positive Zero
            yield return new object[] { ShippingHalf.Epsilon, false };              // Min Positive Subnormal (Positive Epsilon)
            yield return new object[] { UInt16BitsToShippingHalf(0x03FF), false };  // Max Positive Subnormal
            yield return new object[] { UInt16BitsToShippingHalf(0x0400), false };  // Min Positive Normal
            yield return new object[] { ShippingHalf.MaxValue, false };             // Max Positive Normal
            yield return new object[] { ShippingHalf.PositiveInfinity, true };      // Positive Infinity
        }

        [Theory]
        [MemberData(nameof(IsInfinity_TestData))]
        public static void IsInfinity(ShippingHalf value, bool expected)
        {
            Assert.Equal(expected, ShippingHalf.IsInfinity(value));
        }

        public static IEnumerable<object[]> IsNaN_TestData()
        {
            yield return new object[] { ShippingHalf.NegativeInfinity, false };     // Negative Infinity
            yield return new object[] { ShippingHalf.MinValue, false };             // Min Negative Normal
            yield return new object[] { UInt16BitsToShippingHalf(0x8400), false };  // Max Negative Normal
            yield return new object[] { UInt16BitsToShippingHalf(0x83FF), false };  // Min Negative Subnormal
            yield return new object[] { UInt16BitsToShippingHalf(0x8001), false };  // Max Negative Subnormal (Negative Epsilon)
            yield return new object[] { UInt16BitsToShippingHalf(0x8000), false };  // Negative Zero
            yield return new object[] { ShippingHalf.NaN, true };                   // NaN
            yield return new object[] { UInt16BitsToShippingHalf(0x0000), false };  // Positive Zero
            yield return new object[] { ShippingHalf.Epsilon, false };              // Min Positive Subnormal (Positive Epsilon)
            yield return new object[] { UInt16BitsToShippingHalf(0x03FF), false };  // Max Positive Subnormal
            yield return new object[] { UInt16BitsToShippingHalf(0x0400), false };  // Min Positive Normal
            yield return new object[] { ShippingHalf.MaxValue, false };             // Max Positive Normal
            yield return new object[] { ShippingHalf.PositiveInfinity, false };     // Positive Infinity
        }

        [Theory]
        [MemberData(nameof(IsNaN_TestData))]
        public static void IsNaN(ShippingHalf value, bool expected)
        {
            Assert.Equal(expected, ShippingHalf.IsNaN(value));
        }

        public static IEnumerable<object[]> IsNegative_TestData()
        {
            yield return new object[] { ShippingHalf.NegativeInfinity, true };      // Negative Infinity
            yield return new object[] { ShippingHalf.MinValue, true };              // Min Negative Normal
            yield return new object[] { UInt16BitsToShippingHalf(0x8400), true };   // Max Negative Normal
            yield return new object[] { UInt16BitsToShippingHalf(0x83FF), true };   // Min Negative Subnormal
            yield return new object[] { UInt16BitsToShippingHalf(0x8001), true };   // Max Negative Subnormal
            yield return new object[] { UInt16BitsToShippingHalf(0x8000), true };   // Negative Zero
            yield return new object[] { ShippingHalf.NaN, true };                   // NaN
            yield return new object[] { UInt16BitsToShippingHalf(0x0000), false };  // Positive Zero
            yield return new object[] { ShippingHalf.Epsilon, false };              // Min Positive Subnormal
            yield return new object[] { UInt16BitsToShippingHalf(0x03FF), false };  // Max Positive Subnormal
            yield return new object[] { UInt16BitsToShippingHalf(0x0400), false };  // Min Positive Normal
            yield return new object[] { ShippingHalf.MaxValue, false };             // Max Positive Normal
            yield return new object[] { ShippingHalf.PositiveInfinity, false };     // Positive Infinity
        }

        [Theory]
        [MemberData(nameof(IsNegative_TestData))]
        public static void IsNegative(ShippingHalf value, bool expected)
        {
            Assert.Equal(expected, ShippingHalf.IsNegative(value));
        }

        public static IEnumerable<object[]> IsNegativeInfinity_TestData()
        {
            yield return new object[] { ShippingHalf.NegativeInfinity, true };      // Negative Infinity
            yield return new object[] { ShippingHalf.MinValue, false };             // Min Negative Normal
            yield return new object[] { UInt16BitsToShippingHalf(0x8400), false };  // Max Negative Normal
            yield return new object[] { UInt16BitsToShippingHalf(0x83FF), false };  // Min Negative Subnormal
            yield return new object[] { UInt16BitsToShippingHalf(0x8001), false };  // Max Negative Subnormal (Negative Epsilon)
            yield return new object[] { UInt16BitsToShippingHalf(0x8000), false };  // Negative Zero
            yield return new object[] { ShippingHalf.NaN, false };                  // NaN
            yield return new object[] { UInt16BitsToShippingHalf(0x0000), false };  // Positive Zero
            yield return new object[] { ShippingHalf.Epsilon, false };              // Min Positive Subnormal (Positive Epsilon)
            yield return new object[] { UInt16BitsToShippingHalf(0x03FF), false };  // Max Positive Subnormal
            yield return new object[] { UInt16BitsToShippingHalf(0x0400), false };  // Min Positive Normal
            yield return new object[] { ShippingHalf.MaxValue, false };             // Max Positive Normal
            yield return new object[] { ShippingHalf.PositiveInfinity, false };     // Positive Infinity
        }

        [Theory]
        [MemberData(nameof(IsNegativeInfinity_TestData))]
        public static void IsNegativeInfinity(ShippingHalf value, bool expected)
        {
            Assert.Equal(expected, ShippingHalf.IsNegativeInfinity(value));
        }

        public static IEnumerable<object[]> IsNormal_TestData()
        {
            yield return new object[] { ShippingHalf.NegativeInfinity, false };     // Negative Infinity
            yield return new object[] { ShippingHalf.MinValue, true };              // Min Negative Normal
            yield return new object[] { UInt16BitsToShippingHalf(0x8400), true };   // Max Negative Normal
            yield return new object[] { UInt16BitsToShippingHalf(0x83FF), false };  // Min Negative Subnormal
            yield return new object[] { UInt16BitsToShippingHalf(0x8001), false };  // Max Negative Subnormal
            yield return new object[] { UInt16BitsToShippingHalf(0x8000), false };  // Negative Zero
            yield return new object[] { ShippingHalf.NaN, false };                  // NaN
            yield return new object[] { UInt16BitsToShippingHalf(0x0000), false };  // Positive Zero
            yield return new object[] { ShippingHalf.Epsilon, false };              // Min Positive Subnormal
            yield return new object[] { UInt16BitsToShippingHalf(0x03FF), false };  // Max Positive Subnormal
            yield return new object[] { UInt16BitsToShippingHalf(0x0400), true };   // Min Positive Normal
            yield return new object[] { ShippingHalf.MaxValue, true };              // Max Positive Normal
            yield return new object[] { ShippingHalf.PositiveInfinity, false };     // Positive Infinity
        }

        [Theory]
        [MemberData(nameof(IsNormal_TestData))]
        public static void IsNormal(ShippingHalf value, bool expected)
        {
            Assert.Equal(expected, ShippingHalf.IsNormal(value));
        }

        public static IEnumerable<object[]> IsPositiveInfinity_TestData()
        {
            yield return new object[] { ShippingHalf.NegativeInfinity, false };     // Negative Infinity
            yield return new object[] { ShippingHalf.MinValue, false };             // Min Negative Normal
            yield return new object[] { UInt16BitsToShippingHalf(0x8400), false };  // Max Negative Normal
            yield return new object[] { UInt16BitsToShippingHalf(0x83FF), false };  // Min Negative Subnormal
            yield return new object[] { UInt16BitsToShippingHalf(0x8001), false };  // Max Negative Subnormal (Negative Epsilon)
            yield return new object[] { UInt16BitsToShippingHalf(0x8000), false };  // Negative Zero
            yield return new object[] { ShippingHalf.NaN, false };                  // NaN
            yield return new object[] { UInt16BitsToShippingHalf(0x0000), false };  // Positive Zero
            yield return new object[] { ShippingHalf.Epsilon, false };              // Min Positive Subnormal (Positive Epsilon)
            yield return new object[] { UInt16BitsToShippingHalf(0x03FF), false };  // Max Positive Subnormal
            yield return new object[] { UInt16BitsToShippingHalf(0x0400), false };  // Min Positive Normal
            yield return new object[] { ShippingHalf.MaxValue, false };             // Max Positive Normal
            yield return new object[] { ShippingHalf.PositiveInfinity, true };      // Positive Infinity
        }

        [Theory]
        [MemberData(nameof(IsPositiveInfinity_TestData))]
        public static void IsPositiveInfinity(ShippingHalf value, bool expected)
        {
            Assert.Equal(expected, ShippingHalf.IsPositiveInfinity(value));
        }

        public static IEnumerable<object[]> IsSubnormal_TestData()
        {
            yield return new object[] { ShippingHalf.NegativeInfinity, false };     // Negative Infinity
            yield return new object[] { ShippingHalf.MinValue, false };             // Min Negative Normal
            yield return new object[] { UInt16BitsToShippingHalf(0x8400), false };  // Max Negative Normal
            yield return new object[] { UInt16BitsToShippingHalf(0x83FF), true };   // Min Negative Subnormal
            yield return new object[] { UInt16BitsToShippingHalf(0x8001), true };   // Max Negative Subnormal
            yield return new object[] { UInt16BitsToShippingHalf(0x8000), false };  // Negative Zero
            yield return new object[] { ShippingHalf.NaN, false };                  // NaN
            yield return new object[] { UInt16BitsToShippingHalf(0x0000), false };  // Positive Zero
            yield return new object[] { ShippingHalf.Epsilon, true };               // Min Positive Subnormal
            yield return new object[] { UInt16BitsToShippingHalf(0x03FF), true };   // Max Positive Subnormal
            yield return new object[] { UInt16BitsToShippingHalf(0x0400), false };  // Min Positive Normal
            yield return new object[] { ShippingHalf.MaxValue, false };             // Max Positive Normal
            yield return new object[] { ShippingHalf.PositiveInfinity, false };     // Positive Infinity
        }

        [Theory]
        [MemberData(nameof(IsSubnormal_TestData))]
        public static void IsSubnormal(ShippingHalf value, bool expected)
        {
            Assert.Equal(expected, ShippingHalf.IsSubnormal(value));
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
            Assert.Throws(typeof(ArgumentException), () => ShippingHalf.MaxValue.CompareTo(obj));
        }

        public static IEnumerable<object[]> CompareTo_TestData()
        {
            yield return new object[] { ShippingHalf.MaxValue, ShippingHalf.MaxValue, 0 };
            yield return new object[] { ShippingHalf.MaxValue, ShippingHalf.MinValue, 1 };
            yield return new object[] { ShippingHalf.Epsilon, UInt16BitsToShippingHalf(0x8001), 1 };
            yield return new object[] { ShippingHalf.MaxValue, UInt16BitsToShippingHalf(0x0000), 1 };
            yield return new object[] { ShippingHalf.MaxValue, ShippingHalf.Epsilon, 1 };
            yield return new object[] { ShippingHalf.MaxValue, ShippingHalf.PositiveInfinity, -1 };
            yield return new object[] { ShippingHalf.MinValue, ShippingHalf.MaxValue, -1 };
            yield return new object[] { ShippingHalf.MaxValue, ShippingHalf.NaN, 1 };
            yield return new object[] { ShippingHalf.NaN, ShippingHalf.NaN, 0 };
            yield return new object[] { ShippingHalf.NaN, UInt16BitsToShippingHalf(0x0000), -1 };
            yield return new object[] { ShippingHalf.MaxValue, null, 1 };
        }

        [Theory]
        [MemberData(nameof(CompareTo_TestData))]
        public static void CompareTo(ShippingHalf value, object obj, int expected)
        {
            Assert.Equal(expected, Math.Sign(value.CompareTo(obj)));
        }

        public static IEnumerable<object[]> Equals_TestData()
        {
            yield return new object[] { ShippingHalf.MaxValue, ShippingHalf.MaxValue, true };
            yield return new object[] { ShippingHalf.MaxValue, ShippingHalf.MinValue, false };
            yield return new object[] { ShippingHalf.MaxValue, UInt16BitsToShippingHalf(0x0000), false };
            yield return new object[] { ShippingHalf.NaN, ShippingHalf.NaN, false };
            yield return new object[] { ShippingHalf.MaxValue, 789.0f, false };
            yield return new object[] { ShippingHalf.MaxValue, "789", false };
        }

        [Theory]
        [MemberData(nameof(Equals_TestData))]
        public static void Equals(ShippingHalf value, object obj, bool expected)
        {
            Assert.Equal(expected, value.Equals(obj));
        }

        public static IEnumerable<object[]> ToSingle_TestData()
        {
            // Magnitude smaller than 2^-24 maps to 0
            yield return new object[] { new ShippingHalf(5.2e-20f), 0 };
            yield return new object[] { new ShippingHalf(-5.2e-20f), 0 };
            // Magnitude smaller than 2^-14 map to subnormals
            yield return new object[] { new ShippingHalf(1.52e-5f), 1.52e-5f };
            yield return new object[] { new ShippingHalf(-1.52e-5f), -1.52e-5f };
            // Normal numbers
            yield return new object[] { new ShippingHalf(55.77f), 55.75f };
            yield return new object[] { new ShippingHalf(-55.77f), -55.75f };
            // Magnitude smaller than 2^128 map to infinity
            yield return new object[] { new ShippingHalf(1.7e38f), float.PositiveInfinity };
            yield return new object[] { new ShippingHalf(-1.7e38f), float.NegativeInfinity };
            // Infinity and NaN map to infinity and Nan
            yield return new object[] { ShippingHalf.PositiveInfinity, float.PositiveInfinity };
            yield return new object[] { ShippingHalf.NegativeInfinity, float.NegativeInfinity };
            yield return new object[] { ShippingHalf.NaN, float.NaN };
        }

        [Theory]
        [MemberData(nameof(ToSingle_TestData))]
        public static void ToSingle(ShippingHalf ShippingHalf, float verify)
        {
            float f = ShippingHalf.Float;
            Assert.Equal(f, verify, precision: 1);
        }

        public static IEnumerable<object[]> FromSingle_TestData()
        {
            // Magnitude smaller than 2^-24 maps to 0
            yield return new object[] { new ShippingHalf(5.2e-20f), 0, 0, false };
            yield return new object[] { new ShippingHalf(-5.2e-20f), 0, 0, true };
            // Magnitude smaller than 2^-14 map to subnormals
            yield return new object[] { new ShippingHalf(1.52e-5f), 0, 255, false };
            yield return new object[] { new ShippingHalf(1.52e-5f), 0, 255, true };
            // Normal numbers
            yield return new object[] { new ShippingHalf(55.77f), 20, 760, false };
            yield return new object[] { new ShippingHalf(-55.77f), 20, 760, true };
            // Magnitude smaller than 2^128 map to infinity
            yield return new object[] { new ShippingHalf(1.7e38f), 31, 0, false };
            yield return new object[] { new ShippingHalf(-1.7e38f), 31, 0, true };
            // Infinity and NaN map to infinity and Nan
            yield return new object[] { ShippingHalf.PositiveInfinity, 31, 0, false };
            yield return new object[] { ShippingHalf.NegativeInfinity, 31, 0, true };
            yield return new object[] { ShippingHalf.NaN, 31, 512, true };
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
            ShippingHalf actual = ShippingHalf.Parse(value, formatProvider: provider);
            Assert.Equal(expected, actual.Float, precision: 1);
        }

        [Fact]
        public unsafe void TestAllHalfValues()
        {
            for (ushort i = ushort.MinValue; i < ushort.MaxValue; i++)
            {
                ShippingHalf half1 = UInt16BitsToShippingHalf(i);
                ShippingHalf half2 = new ShippingHalf(half1.Float);

                bool half1IsNaN = ShippingHalf.IsNaN(half1);
                bool half2IsNaN = ShippingHalf.IsNaN(half2);
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
