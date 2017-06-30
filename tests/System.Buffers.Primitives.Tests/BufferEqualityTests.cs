// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace System.Buffers.Tests
{
    public class BufferEqualityTests
    {
        [Fact]
        public void BuffersCanBeBoxed()
        {
            var buffer = Buffer<byte>.Empty;
            object bufferAsObject = buffer;
            Assert.True(buffer.Equals(bufferAsObject));

            var readOnlyBuffer = ReadOnlyBuffer<byte>.Empty;
            object readOnlyBufferAsObject = readOnlyBuffer;
            Assert.True(readOnlyBuffer.Equals(readOnlyBufferAsObject));

            Assert.False(buffer.Equals(new object()));
            Assert.False(readOnlyBuffer.Equals(new object()));

            Assert.False(buffer.Equals((object)(new Buffer<byte>(new byte[] { 1, 2 }))));
            Assert.False(readOnlyBuffer.Equals((object)(new ReadOnlyBuffer<byte>(new byte[] { 1, 2 }))));

            Assert.True(buffer.Equals(readOnlyBufferAsObject));
            Assert.True(readOnlyBuffer.Equals(bufferAsObject));
        }

        [Theory]
        [MemberData(nameof(ValidArraySegments))]
        public void BuffersReferencingSameMemoryAreEqualInEveryAspect(byte[] bytes, int start, int length)
        {
            var buffer = new Buffer<byte>(bytes, start, length);
            var pointingToSameMemory = new Buffer<byte>(bytes, start, length);
            Buffer<byte> structCopy = buffer;

            BuffersReferencingSameMemoryAreEqualInEveryAspect(buffer, pointingToSameMemory);
            BuffersReferencingSameMemoryAreEqualInEveryAspect(buffer, structCopy);
        }

        [Theory]
        [MemberData(nameof(ValidArraySegments))]
        public void ReadOnlyBuffersReferencingSameMemoryAreEqualInEveryAspect(byte[] bytes, int start, int length)
        {
            var buffer = new ReadOnlyBuffer<byte>(bytes, start, length);
            var pointingToSameMemory = new ReadOnlyBuffer<byte>(bytes, start, length);
            var structCopy = buffer;

            BuffersReferencingSameMemoryAreEqualInEveryAspect(ref buffer, ref pointingToSameMemory);
            BuffersReferencingSameMemoryAreEqualInEveryAspect(ref buffer, ref structCopy);
        }

        [Theory]
        [MemberData(nameof(FullArraySegments))]
        public void BufferArrayEquivalenceAndImplicitCastsAreEqual(byte[] bytes)
        {
            var buffer = new Buffer<byte>(bytes);
            var readOnlyBuffer = new ReadOnlyBuffer<byte>(bytes);
            ReadOnlyBuffer<byte> implicitReadOnlyBuffer = buffer;
            Buffer<byte> implicitBufferArray = bytes;
            ReadOnlyBuffer<byte> implicitReadOnlyBufferArray = bytes;

            Assert.True(buffer.Equals(bytes));
            Assert.True(readOnlyBuffer.Equals(bytes));
            Assert.True(implicitReadOnlyBuffer.Equals(bytes));
            Assert.True(implicitBufferArray.Equals(bytes));
            Assert.True(implicitReadOnlyBufferArray.Equals(bytes));
            
            Assert.True(readOnlyBuffer.Equals(buffer));
            Assert.True(implicitReadOnlyBuffer.Equals(buffer));
            Assert.True(implicitBufferArray.Equals(buffer));
            Assert.True(implicitReadOnlyBufferArray.Equals(buffer));

            Assert.True(buffer.Equals(readOnlyBuffer));
            Assert.True(implicitReadOnlyBuffer.Equals(readOnlyBuffer));
            Assert.True(implicitBufferArray.Equals(readOnlyBuffer));
            Assert.True(implicitReadOnlyBufferArray.Equals(readOnlyBuffer));

            Assert.True(buffer.Equals(implicitBufferArray));
            Assert.True(readOnlyBuffer.Equals(implicitBufferArray));
            Assert.True(implicitReadOnlyBuffer.Equals(implicitBufferArray));
            Assert.True(implicitReadOnlyBufferArray.Equals(implicitBufferArray));

            Assert.True(buffer.Equals(implicitReadOnlyBuffer));
            Assert.True(readOnlyBuffer.Equals(implicitReadOnlyBuffer));
            Assert.True(implicitBufferArray.Equals(implicitReadOnlyBuffer));
            Assert.True(implicitReadOnlyBufferArray.Equals(implicitReadOnlyBuffer));

            Assert.True(buffer.Equals(implicitReadOnlyBufferArray));
            Assert.True(readOnlyBuffer.Equals(implicitReadOnlyBufferArray));
            Assert.True(implicitReadOnlyBuffer.Equals(implicitReadOnlyBufferArray));
            Assert.True(implicitBufferArray.Equals(implicitReadOnlyBufferArray));
        }

        [Theory]
        [MemberData(nameof(ValidArraySegments))]
        public void RangedBufferEquivalenceAndImplicitCastsAreEqual(byte[] bytes, int start, int length)
        {
            var buffer = new Buffer<byte>(bytes, start, length);
            var readOnlyBuffer = new ReadOnlyBuffer<byte>(bytes, start, length);
            ReadOnlyBuffer<byte> implicitReadOnlyBuffer = buffer;

            Assert.True(readOnlyBuffer.Equals(buffer));
            Assert.True(implicitReadOnlyBuffer.Equals(buffer));

            Assert.True(buffer.Equals(readOnlyBuffer));
            Assert.True(implicitReadOnlyBuffer.Equals(readOnlyBuffer));

            Assert.True(buffer.Equals(implicitReadOnlyBuffer));
            Assert.True(readOnlyBuffer.Equals(implicitReadOnlyBuffer));
        }

        private static void BuffersReferencingSameMemoryAreEqualInEveryAspect(Buffer<byte> buffer, Buffer<byte> pointingToSameMemory)
        {
            Assert.True(buffer.Equals(pointingToSameMemory));
            Assert.True(pointingToSameMemory.Equals(buffer));

            Assert.True(buffer.SequenceEqual(pointingToSameMemory));
            Assert.True(pointingToSameMemory.SequenceEqual(buffer));
            Assert.True(buffer.SequenceEqual(pointingToSameMemory));
            Assert.True(pointingToSameMemory.SequenceEqual(buffer));
        }

        private static void BuffersReferencingSameMemoryAreEqualInEveryAspect(ref ReadOnlyBuffer<byte> buffer, ref ReadOnlyBuffer<byte> pointingToSameMemory)
        {
            Assert.True(buffer.Equals(pointingToSameMemory));
            Assert.True(pointingToSameMemory.Equals(buffer));

            Assert.True(buffer.SequenceEqual(pointingToSameMemory));
            Assert.True(pointingToSameMemory.SequenceEqual(buffer));
            Assert.True(buffer.SequenceEqual(pointingToSameMemory));
            Assert.True(pointingToSameMemory.SequenceEqual(buffer));
        }

        [Fact]
        public void EmptyBuffersAreSequentiallyAndStructurallyEqual()
        {
            var emptyBuffer = new Buffer<int>(Array.Empty<int>());
            var otherEmptyBuffer = new Buffer<int>(Array.Empty<int>());

            Assert.True(emptyBuffer.SequenceEqual(otherEmptyBuffer));
            Assert.True(otherEmptyBuffer.SequenceEqual(emptyBuffer));

            var bufferFromNonEmptyArrayButWithZeroLength = new Buffer<int>(new int[1] { 123 }).Slice(0, 0);

            Assert.True(emptyBuffer.SequenceEqual(bufferFromNonEmptyArrayButWithZeroLength));
            Assert.True(bufferFromNonEmptyArrayButWithZeroLength.SequenceEqual(emptyBuffer));
        }

        [Theory]
        [MemberData(nameof(ValidArraySegments))]
        public void BuffersOfEqualValuesInSameOrderAreSequentiallyAndStructurallyEqual(byte[] bytes, int start, int length)
        {
            var bytesCopy = bytes.ToArray();

            var buffer = new Buffer<byte>(bytes, start, length);
            var ofSameValues = new Buffer<byte>(bytesCopy, start, length);

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

            var buffer = new ReadOnlyBuffer<byte>(bytes, start, length);
            var ofSameValues = new ReadOnlyBuffer<byte>(bytesCopy, start, length);

            Assert.True(buffer.SequenceEqual(ofSameValues));
            Assert.True(ofSameValues.SequenceEqual(buffer));
            Assert.True(buffer.SequenceEqual(ofSameValues));
            Assert.True(ofSameValues.SequenceEqual(buffer));

            Assert.False(buffer.Equals(ofSameValues));
            Assert.False(ofSameValues.Equals(buffer));
        }

        [Theory]
        [MemberData(nameof(ValidArraySegments))]
        public void BuffersOfDifferentValuesAreNotEqual(byte[] bytes, int start, int length)
        {
            var differentBytes = bytes.Select(value => ++value).ToArray();

            var buffer = new Buffer<byte>(bytes, start, length);
            var ofDifferentValues = new Buffer<byte>(differentBytes, start, length);

            Assert.False(buffer.SequenceEqual(ofDifferentValues));
            Assert.False(ofDifferentValues.SequenceEqual(buffer));
            Assert.False(buffer.SequenceEqual(ofDifferentValues));
            Assert.False(ofDifferentValues.SequenceEqual(buffer));

            Assert.False(buffer.Equals(ofDifferentValues));
            Assert.False(ofDifferentValues.Equals(buffer));
        }

        [Theory]
        [MemberData(nameof(ValidArraySegments))]
        public void ReadOnlyBuffersOfDifferentValuesAreNotEqual(byte[] bytes, int start, int length)
        {
            var differentBytes = bytes.Select(value => ++value).ToArray();

            var buffer = new ReadOnlyBuffer<byte>(bytes, start, length);
            var ofDifferentValues = new ReadOnlyBuffer<byte>(differentBytes, start, length);

            Assert.False(buffer.SequenceEqual(ofDifferentValues));
            Assert.False(ofDifferentValues.SequenceEqual(buffer));
            Assert.False(buffer.SequenceEqual(ofDifferentValues));
            Assert.False(ofDifferentValues.SequenceEqual(buffer));

            Assert.False(buffer.Equals(ofDifferentValues));
            Assert.False(ofDifferentValues.Equals(buffer));
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
            var negativeInfinities = new Buffer<T>(Enumerable.Repeat(negativeInfinity, elementsCountThatNormallyWouldInvolveMemCmp).ToArray());
            var positiveInfinities = new Buffer<T>(Enumerable.Repeat(positiveInfinity, elementsCountThatNormallyWouldInvolveMemCmp).ToArray());

            Assert.Equal(positiveInfinity.Equals(negativeInfinity), positiveInfinities.SequenceEqual(negativeInfinities));
            Assert.False(negativeInfinities.SequenceEqual(positiveInfinities));

            var negativeZeroes = new Buffer<T>(Enumerable.Repeat(negativeZero, elementsCountThatNormallyWouldInvolveMemCmp).ToArray());
            var positiveZeroes = new Buffer<T>(Enumerable.Repeat(positiveZero, elementsCountThatNormallyWouldInvolveMemCmp).ToArray());

            Assert.Equal(negativeZero.Equals(positiveZero), negativeZeroes.SequenceEqual(positiveZeroes));
            Assert.False(positiveInfinities.SequenceEqual(negativeInfinities));

            var nans = new Buffer<T>(Enumerable.Repeat(NaN, elementsCountThatNormallyWouldInvolveMemCmp).ToArray());

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
            var buffer = new Buffer<byte>(Enumerable.Range(0, bytesCount).Select(index => (byte)index).ToArray());
            var slice = buffer.Span;
            var copy = new Buffer<byte>(slice.ToArray()).Span;

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

        public static IEnumerable<object[]> FullArraySegments
        {
            get
            {
                return new List<object[]>
                {
                    new object[] { new byte[1] { 0 } },
                    new object[] { new byte[2] { 0, 0 } },
                    new object[] { new byte[3] { 0, 0, 0 } },
                    new object[] { Enumerable.Range(0, 100000).Select(i => (byte)i).ToArray() }
                };
            }
        }
    }
}
