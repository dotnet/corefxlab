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

            Assert.True(buffer == bytes);
            Assert.False(buffer != bytes);
            Assert.True(readOnlyBuffer == bytes);
            Assert.False(readOnlyBuffer != bytes);
            Assert.True(implicitReadOnlyBuffer == bytes);
            Assert.False(implicitReadOnlyBuffer != bytes);
            Assert.True(implicitBufferArray == bytes);
            Assert.False(implicitBufferArray != bytes);
            Assert.True(implicitReadOnlyBufferArray == bytes);
            Assert.False(implicitReadOnlyBufferArray != bytes);

            Assert.True(bytes == buffer);
            Assert.False(bytes != buffer);
            Assert.True(readOnlyBuffer == buffer);
            Assert.False(readOnlyBuffer != buffer);
            Assert.True(implicitReadOnlyBuffer == buffer);
            Assert.False(implicitReadOnlyBuffer != buffer);
            Assert.True(implicitBufferArray == buffer);
            Assert.False(implicitBufferArray != buffer);
            Assert.True(implicitReadOnlyBufferArray == buffer);
            Assert.False(implicitReadOnlyBufferArray != buffer);

            Assert.True(buffer == readOnlyBuffer);
            Assert.False(buffer != readOnlyBuffer);
            Assert.True(bytes == readOnlyBuffer);
            Assert.False(bytes != readOnlyBuffer);
            Assert.True(implicitReadOnlyBuffer == readOnlyBuffer);
            Assert.False(implicitReadOnlyBuffer != readOnlyBuffer);
            Assert.True(implicitBufferArray == readOnlyBuffer);
            Assert.False(implicitBufferArray != readOnlyBuffer);
            Assert.True(implicitReadOnlyBufferArray == readOnlyBuffer);
            Assert.False(implicitReadOnlyBufferArray != readOnlyBuffer);

            Assert.True(buffer == implicitBufferArray);
            Assert.False(buffer != implicitBufferArray);
            Assert.True(readOnlyBuffer == implicitBufferArray);
            Assert.False(readOnlyBuffer != implicitBufferArray);
            Assert.True(bytes == implicitBufferArray);
            Assert.False(bytes != implicitBufferArray);
            Assert.True(implicitReadOnlyBuffer == implicitBufferArray);
            Assert.False(implicitReadOnlyBuffer != implicitBufferArray);
            Assert.True(implicitReadOnlyBufferArray == implicitBufferArray);
            Assert.False(implicitReadOnlyBufferArray != implicitBufferArray);

            Assert.True(buffer == implicitReadOnlyBuffer);
            Assert.False(buffer != implicitReadOnlyBuffer);
            Assert.True(readOnlyBuffer == implicitReadOnlyBuffer);
            Assert.False(readOnlyBuffer != implicitReadOnlyBuffer);
            Assert.True(bytes == implicitReadOnlyBuffer);
            Assert.False(bytes != implicitReadOnlyBuffer);
            Assert.True(implicitBufferArray == implicitReadOnlyBuffer);
            Assert.False(implicitBufferArray != implicitReadOnlyBuffer);
            Assert.True(implicitReadOnlyBufferArray == implicitReadOnlyBuffer);
            Assert.False(implicitReadOnlyBufferArray != implicitReadOnlyBuffer);

            Assert.True(buffer == implicitReadOnlyBufferArray);
            Assert.False(buffer != implicitReadOnlyBufferArray);
            Assert.True(readOnlyBuffer == implicitReadOnlyBufferArray);
            Assert.False(readOnlyBuffer != implicitReadOnlyBufferArray);
            Assert.True(implicitReadOnlyBuffer == implicitReadOnlyBufferArray);
            Assert.False(implicitReadOnlyBuffer != implicitReadOnlyBufferArray);
            Assert.True(implicitBufferArray == implicitReadOnlyBufferArray);
            Assert.False(implicitBufferArray != implicitReadOnlyBufferArray);
            Assert.True(bytes == implicitReadOnlyBufferArray);
            Assert.False(bytes != implicitReadOnlyBufferArray);
        }

        [Theory]
        [MemberData(nameof(ValidArraySegments))]
        public void RangedBufferEquivalenceAndImplicitCastsAreEqual(byte[] bytes, int start, int length)
        {
            var buffer = new Buffer<byte>(bytes, start, length);
            var readOnlyBuffer = new ReadOnlyBuffer<byte>(bytes, start, length);
            ReadOnlyBuffer<byte> implicitReadOnlyBuffer = buffer;

            Assert.True(readOnlyBuffer == buffer);
            Assert.False(readOnlyBuffer != buffer);
            Assert.True(implicitReadOnlyBuffer == buffer);
            Assert.False(implicitReadOnlyBuffer != buffer);

            Assert.True(buffer == readOnlyBuffer);
            Assert.False(buffer != readOnlyBuffer);
            Assert.True(implicitReadOnlyBuffer == readOnlyBuffer);
            Assert.False(implicitReadOnlyBuffer != readOnlyBuffer);

            Assert.True(buffer == implicitReadOnlyBuffer);
            Assert.False(buffer != implicitReadOnlyBuffer);
            Assert.True(readOnlyBuffer == implicitReadOnlyBuffer);
            Assert.False(readOnlyBuffer != implicitReadOnlyBuffer);
        }

        private static void BuffersReferencingSameMemoryAreEqualInEveryAspect(Buffer<byte> buffer, Buffer<byte> pointingToSameMemory)
        {
            Assert.True(buffer == pointingToSameMemory);
            Assert.True(pointingToSameMemory == buffer);
            Assert.False(buffer != pointingToSameMemory);
            Assert.False(pointingToSameMemory != buffer);

            Assert.True(buffer.SequenceEqual(pointingToSameMemory));
            Assert.True(pointingToSameMemory.SequenceEqual(buffer));
            Assert.True(buffer.SequenceEqual(pointingToSameMemory));
            Assert.True(pointingToSameMemory.SequenceEqual(buffer));
        }

        private static void BuffersReferencingSameMemoryAreEqualInEveryAspect(ref ReadOnlyBuffer<byte> buffer, ref ReadOnlyBuffer<byte> pointingToSameMemory)
        {
            Assert.True(buffer == pointingToSameMemory);
            Assert.True(pointingToSameMemory == buffer);
            Assert.False(buffer != pointingToSameMemory);
            Assert.False(pointingToSameMemory != buffer);

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

            Assert.False(buffer == ofSameValues);
            Assert.True(buffer != ofSameValues);
            Assert.False(ofSameValues == buffer);
            Assert.True(ofSameValues != buffer);
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

            Assert.False(buffer == ofSameValues);
            Assert.True(buffer != ofSameValues);
            Assert.False(ofSameValues == buffer);
            Assert.True(ofSameValues != buffer);

            Assert.False(buffer == ofSameValues);
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

            Assert.False(buffer == ofDifferentValues);
            Assert.True(buffer != ofDifferentValues);
            Assert.False(ofDifferentValues == buffer);
            Assert.True(ofDifferentValues != buffer);

            Assert.False(buffer == ofDifferentValues);
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

            Assert.False(buffer == ofDifferentValues);
            Assert.True(buffer != ofDifferentValues);
            Assert.False(ofDifferentValues == buffer);
            Assert.True(ofDifferentValues != buffer);

            Assert.False(buffer == ofDifferentValues);
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
            T negativeInfinity, T positiveInfinity, // .Equals => false
            T negativeZero, T positiveZero, // .Equals => TRUE
            T NaN) // .Equals => True
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
