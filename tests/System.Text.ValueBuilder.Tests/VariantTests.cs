// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Xunit;

namespace System.Text.ValueBuilder.Tests
{
    public class VariantTests
    {
        [Theory,
            InlineData(0),
            InlineData(int.MaxValue),
            InlineData(int.MinValue),
            InlineData(42)]
        public void IntStorage(int value)
        {
            Variant v = new Variant(value);
            Assert.True(v.TryGetValue(out int result));
            Assert.Equal(value, result);
            Assert.False(v.TryGetValue(out object _));
            Assert.Equal(value, (int)v);
            var span = v.ToSpan();
            Assert.Equal(1, span.Length);
            Assert.True(span[0].TryGetValue(out result));
            Assert.Equal(value, result);
        }

        [Fact]
        public void TwoVariant_HardCast()
        {
            var variant = Variant.Create(19, 42);
            var span = variant.ToSpan();
            Assert.Equal(2, span.Length);
            Assert.True(span[0].TryGetValue(out int result));
            Assert.Equal(19, result);
            Assert.True(span[1].TryGetValue(out result));
            Assert.Equal(42, result);
        }

        [Fact]
        public void ThreeVariant_HardCast()
        {
            var variant = Variant.Create(1, 2, 3);
            var span = variant.ToSpan();
            Assert.Equal(3, span.Length);
            Assert.True(span[0].TryGetValue(out int result));
            Assert.Equal(1, result);
            Assert.True(span[1].TryGetValue(out result));
            Assert.Equal(2, result);
            Assert.True(span[2].TryGetValue(out result));
            Assert.Equal(3, result);
        }

        [Fact]
        public void TwoVariant_Variant2()
        {
            var variant = Variant.Create(19, 42);
            var span = variant.ToSpan();
            Assert.Equal(2, span.Length);
            Assert.True(span[0].TryGetValue(out int result));
            Assert.Equal(19, result);
            Assert.True(span[1].TryGetValue(out result));
            Assert.Equal(42, result);
        }

        [Theory,
            InlineData(0),
            InlineData(float.MaxValue),
            InlineData(float.MinValue),
            InlineData(4.2)]
        public void FloatStorage(float value)
        {
            var variant = Variant.Create(value);
            Assert.Equal(VariantType.Single, variant.Type);
            Assert.True(variant.TryGetValue(out float result));
            Assert.Equal(value, result);
            Assert.False(variant.TryGetValue(out object _));
        }

        [Fact]
        public void GuidStorage()
        {
            Guid guid = Guid.NewGuid();
            var variant = Variant.Create(guid);
            Assert.Equal(VariantType.Guid, variant.Type);
            Assert.True(variant.TryGetValue(out Guid result));
            Assert.Equal(guid, result);
            Assert.False(variant.TryGetValue(out object _));
        }

        [Theory,
            InlineData(true),
            InlineData(false)]
        public void BooleanStorage(bool value)
        {
            Variant v = new Variant(value);
            Assert.True(v.TryGetValue(out bool result));
            Assert.Equal(value, result);
            Assert.False(v.TryGetValue(out object _));
        }

        public static TheoryData<decimal> DecimalData => new TheoryData<decimal>
        {
            0,
            decimal.MaxValue,
            decimal.MinValue,
            decimal.MinusOne
        };

        [Theory,
            MemberData(nameof(DecimalData))]
        public void DecimalStorage(decimal value)
        {
            var variant = Variant.Create(value);
            Assert.Equal(VariantType.Decimal, variant.Type);
            Assert.True(variant.TryGetValue(out decimal result));
            Assert.Equal(value, result);
            Assert.False(variant.TryGetValue(out object _));
        }
    }
}
