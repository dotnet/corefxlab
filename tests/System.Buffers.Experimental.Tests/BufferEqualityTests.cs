// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace System.Buffers.Tests
{
    public class BufferEqualityTests
    {
        [Fact]
        public void EmptyBuffersAreSequentiallyAndStructurallyEqual()
        {
            var emptyBuffer = new Memory<int>(Array.Empty<int>());
            var otherEmptyBuffer = new Memory<int>(Array.Empty<int>());

            Assert.True(emptyBuffer.SequenceEqual(otherEmptyBuffer));
            Assert.True(otherEmptyBuffer.SequenceEqual(emptyBuffer));

            var bufferFromNonEmptyArrayButWithZeroLength = new Memory<int>(new int[1] { 123 }).Slice(0, 0);

            Assert.True(emptyBuffer.SequenceEqual(bufferFromNonEmptyArrayButWithZeroLength));
            Assert.True(bufferFromNonEmptyArrayButWithZeroLength.SequenceEqual(emptyBuffer));
        }

        [Theory]
        [MemberData(nameof(ValidArraySegments))]
        public void BuffersOfEqualValuesInSameOrderAreSequentiallyAndStructurallyEqual(byte[] bytes, int start, int length)
        {
            var bytesCopy = bytes.ToArray();

            var buffer = new Memory<byte>(bytes, start, length);
            var ofSameValues = new Memory<byte>(bytesCopy, start, length);

            Assert.True(buffer.SequenceEqual(ofSameValues));
            Assert.True(ofSameValues.SequenceEqual(buffer));
            Assert.True(buffer.SequenceEqual(ofSameValues));
            Assert.True(ofSameValues.SequenceEqual(buffer));

            Assert.False(buffer.Equals(ofSameValues));
            Assert.False(ofSameValues.Equals(buffer));
        }

        [Theory]
        [MemberData(nameof(ValidArraySegments))]
        public void ReadOnlyBuffersOfEqualValuesInSameOrderAreSequentiallyAndStructurallyEqual(byte[] bytes, int start, int length)
        {
            var bytesCopy = bytes.ToArray();

            var buffer = new ReadOnlyMemory<byte>(bytes, start, length);
            var ofSameValues = new ReadOnlyMemory<byte>(bytesCopy, start, length);

            Assert.True(buffer.SequenceEqual(ofSameValues));
            Assert.True(ofSameValues.SequenceEqual(buffer));
            Assert.True(buffer.SequenceEqual(ofSameValues));
            Assert.True(ofSameValues.SequenceEqual(buffer));

            Assert.False(buffer.Equals(ofSameValues));
            Assert.False(ofSameValues.Equals(buffer));
        }

        [Fact]
        public void SequentialEqualityOfBuffersOfFloatingPointTypesDoesNotUseBitwiseComparison()
        {
            SequentialEqualityOfBuffersOfFloatingPointTypesDoesNotUseBitwiseComparison(
                double.NegativeInfinity, double.PositiveInfinity, -0.0, +0.0, double.NaN);

            SequentialEqualityOfBuffersOfFloatingPointTypesDoesNotUseBitwiseComparison(
                float.NegativeInfinity, float.PositiveInfinity, -0.0, +0.0, float.NaN);
        }

        private static void SequentialEqualityOfBuffersOfFloatingPointTypesDoesNotUseBitwiseComparison<T>(
            T negativeInfinity, T positiveInfinity, //.Equals => false
            T negativeZero, T positiveZero, //.Equals => TRUE
            T NaN) //.Equals => True
            where T : struct, IEquatable<T>
        {
            const int elementsCountThatNormallyWouldInvolveMemCmp = 1000;
            var negativeInfinities = new Memory<T>(Enumerable.Repeat(negativeInfinity, elementsCountThatNormallyWouldInvolveMemCmp).ToArray());
            var positiveInfinities = new Memory<T>(Enumerable.Repeat(positiveInfinity, elementsCountThatNormallyWouldInvolveMemCmp).ToArray());

            Assert.Equal(positiveInfinity.Equals(negativeInfinity), positiveInfinities.SequenceEqual(negativeInfinities));
            Assert.False(negativeInfinities.SequenceEqual(positiveInfinities));

            var negativeZeroes = new Memory<T>(Enumerable.Repeat(negativeZero, elementsCountThatNormallyWouldInvolveMemCmp).ToArray());
            var positiveZeroes = new Memory<T>(Enumerable.Repeat(positiveZero, elementsCountThatNormallyWouldInvolveMemCmp).ToArray());

            Assert.Equal(negativeZero.Equals(positiveZero), negativeZeroes.SequenceEqual(positiveZeroes));
            Assert.False(positiveInfinities.SequenceEqual(negativeInfinities));

            var nans = new Memory<T>(Enumerable.Repeat(NaN, elementsCountThatNormallyWouldInvolveMemCmp).ToArray());

            Assert.Equal(NaN.Equals(NaN), nans.SequenceEqual(nans));
            Assert.True(nans.SequenceEqual(nans));
        }

        [Theory]
        [InlineData(1)]
        [InlineData(7)]
        [InlineData(537)] // odd bytes count
        [InlineData(1024)]
        [InlineData(20 * 100)]
        public void AllBytesAreTakenUnderConsideration(int bytesCount)
        {
            var buffer = new Memory<byte>(Enumerable.Range(0, bytesCount).Select(index => (byte)index).ToArray());
            var slice = buffer.Span;
            var copy = new Memory<byte>(slice.ToArray()).Span;

            for (int i = 0; i < bytesCount; i++)
            {
                Assert.True(slice.SequenceEqual(copy)); // it is equal

                var valueCopy = slice[i];
                slice[i] = (byte)(valueCopy + 1); // lets just change single value

                Assert.False(slice.SequenceEqual(copy)); // it is not equal anymore

                slice[i] = valueCopy;
            }
        }

        public static IEnumerable<object[]> ValidArraySegments
        {
            get
            {
                return new List<object[]>
                {
                    new object[] { new byte[1] { 0 }, 0, 1},
                    new object[] { new byte[2] { 0, 0 }, 0, 2},
                    new object[] { new byte[2] { 0, 0 }, 0, 1},
                    new object[] { new byte[2] { 0, 0 }, 1, 1},
                    new object[] { new byte[3] { 0, 0, 0 }, 0, 3},
                    new object[] { new byte[3] { 0, 0, 0 }, 0, 2},
                    new object[] { new byte[3] { 0, 0, 0 }, 1, 2},
                    new object[] { new byte[3] { 0, 0, 0 }, 1, 1},
                    new object[] { Enumerable.Range(0, 100000).Select(i => (byte)i).ToArray(), 0, 100000 }
                };
            }
        }
    }
}
