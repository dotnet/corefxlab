// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Xunit;

namespace System.Slices.Tests
{
    public class EqualityTests
    {
        [Theory]
        [MemberData("ValidArraySegments")]
        public void SpansReferencingSameMemoryAreEqualInEveryAspect(byte[] bytes, int start, int length)
        {
            var span = new Span<byte>(bytes, start, length);
            var pointingToSameMemory = new Span<byte>(bytes, start, length);
            var structCopy = span;

            SpansReferencingSameMemoryAreEqualInEveryAspect(ref span, ref pointingToSameMemory);
            SpansReferencingSameMemoryAreEqualInEveryAspect(ref span, ref structCopy);
        }

        [Theory]
        [MemberData("ValidArraySegments")]
        public void ReadOnlySpansReferencingSameMemoryAreEqualInEveryAspect(byte[] bytes, int start, int length)
        {
            var span = new ReadOnlySpan<byte>(bytes, start, length);
            var pointingToSameMemory = new ReadOnlySpan<byte>(bytes, start, length);
            var structCopy = span;

            SpansReferencingSameMemoryAreEqualInEveryAspect(ref span, ref pointingToSameMemory);
            SpansReferencingSameMemoryAreEqualInEveryAspect(ref span, ref structCopy);
        }

        // ref is used just for the structCopy scenario, otherwise it would always be copy

        private static void SpansReferencingSameMemoryAreEqualInEveryAspect(ref Span<byte> span, ref Span<byte> pointingToSameMemory)
        {
            Assert.True(span.ReferenceEquals(pointingToSameMemory));
            Assert.True(span.Equals(pointingToSameMemory));
            Assert.True(span.Equals((Object)pointingToSameMemory));
            Assert.Equal(span.GetHashCode(), pointingToSameMemory.GetHashCode());

            Assert.True(span.SequenceEqual(pointingToSameMemory));
            Assert.True(pointingToSameMemory.SequenceEqual(span));
            Assert.True(span.BlockEquals(pointingToSameMemory));
            Assert.True(pointingToSameMemory.BlockEquals(span));
        }

        private static void SpansReferencingSameMemoryAreEqualInEveryAspect(ref ReadOnlySpan<byte> span, ref ReadOnlySpan<byte> pointingToSameMemory)
        {
            Assert.True(span.ReferenceEquals(pointingToSameMemory));
            Assert.True(span.Equals(pointingToSameMemory));
            Assert.True(span.Equals((Object)pointingToSameMemory));
            Assert.Equal(span.GetHashCode(), pointingToSameMemory.GetHashCode());

            Assert.True(span.SequenceEqual(pointingToSameMemory));
            Assert.True(pointingToSameMemory.SequenceEqual(span));
            Assert.True(span.BlockEquals(pointingToSameMemory));
            Assert.True(pointingToSameMemory.BlockEquals(span));
        }

        [Fact]
        public void EmptySpansAreSequentiallyAndStructurallyEqual()
        {
            var emptySpan = Array.Empty<int>().Slice();
            var otherEmptySpan = Array.Empty<int>().Slice();

            Assert.True(emptySpan.SequenceEqual(otherEmptySpan));
            Assert.True(otherEmptySpan.SequenceEqual(emptySpan));

            var emptySpanOfOtherType = Array.Empty<double>().Slice();

            Assert.True(emptySpan.BlockEquals(emptySpanOfOtherType));
            Assert.True(emptySpanOfOtherType.BlockEquals(emptySpan));

            var spanFromNonEmptyArrayButWithZeroLength = new int[1] { 123 }.Slice(0, 0);

            Assert.True(emptySpan.SequenceEqual(spanFromNonEmptyArrayButWithZeroLength));
            Assert.True(spanFromNonEmptyArrayButWithZeroLength.SequenceEqual(emptySpan));
        }

        [Theory]
        [MemberData("ValidArraySegments")]
        public void SpansOfEqualValuesInSameOrderAreSequentiallyAndStructurallyEqual(byte[] bytes, int start, int length)
        {
            var bytesCopy = bytes.ToArray();

            var span = new Span<byte>(bytes, start, length);
            var ofSameValues = new Span<byte>(bytesCopy, start, length);

            Assert.True(span.SequenceEqual(ofSameValues));
            Assert.True(ofSameValues.SequenceEqual(span));
            Assert.True(span.BlockEquals(ofSameValues));
            Assert.True(ofSameValues.BlockEquals(span));

            Assert.False(span.ReferenceEquals(ofSameValues));
            Assert.False(span.Equals(ofSameValues));
            Assert.False(span.Equals((Object)ofSameValues));
            Assert.NotEqual(span.GetHashCode(), ofSameValues.GetHashCode());
        }

        [Theory]
        [MemberData("ValidArraySegments")]
        public void ReadOnlySpansOfEqualValuesInSameOrderAreSequentiallyAndStructurallyEqual(byte[] bytes, int start, int length)
        {
            var bytesCopy = bytes.ToArray();

            var span = new ReadOnlySpan<byte>(bytes, start, length);
            var ofSameValues = new ReadOnlySpan<byte>(bytesCopy, start, length);

            Assert.True(span.SequenceEqual(ofSameValues));
            Assert.True(ofSameValues.SequenceEqual(span));
            Assert.True(span.BlockEquals(ofSameValues));
            Assert.True(ofSameValues.BlockEquals(span));

            Assert.False(span.ReferenceEquals(ofSameValues));
            Assert.False(span.Equals(ofSameValues));
            Assert.False(span.Equals((Object)ofSameValues));
            Assert.NotEqual(span.GetHashCode(), ofSameValues.GetHashCode());
        }

        [Theory]
        [MemberData("ValidArraySegments")]
        public void SpansOfDifferentValuesAreNotEqual(byte[] bytes, int start, int length)
        {
            var differentBytes = bytes.Select(value => ++value).ToArray();

            var span = new Span<byte>(bytes, start, length);
            var ofDifferentValues = new Span<byte>(differentBytes, start, length);

            Assert.False(span.SequenceEqual(ofDifferentValues));
            Assert.False(ofDifferentValues.SequenceEqual(span));
            Assert.False(span.BlockEquals(ofDifferentValues));
            Assert.False(ofDifferentValues.BlockEquals(span));

            Assert.False(span.ReferenceEquals(ofDifferentValues));
            Assert.False(span.Equals(ofDifferentValues));
            Assert.False(span.Equals((Object)ofDifferentValues));
            Assert.False(span.GetHashCode() == ofDifferentValues.GetHashCode());
        }

        [Theory]
        [MemberData("ValidArraySegments")]
        public void ReadOnlySpanOfDifferentValuesAreNotEqual(byte[] bytes, int start, int length)
        {
            var differentBytes = bytes.Select(value => ++value).ToArray();

            var span = new ReadOnlySpan<byte>(bytes, start, length);
            var ofDifferentValues = new ReadOnlySpan<byte>(differentBytes, start, length);

            Assert.False(span.SequenceEqual(ofDifferentValues));
            Assert.False(ofDifferentValues.SequenceEqual(span));
            Assert.False(span.BlockEquals(ofDifferentValues));
            Assert.False(ofDifferentValues.BlockEquals(span));

            Assert.False(span.ReferenceEquals(ofDifferentValues));
            Assert.False(span.Equals(ofDifferentValues));
            Assert.False(span.Equals((Object)ofDifferentValues));
            Assert.False(span.GetHashCode() == ofDifferentValues.GetHashCode());
        }

        [Theory]
        [MemberData("ValidArraySegments")]
        public void SpansOfEqualValuesOfDifferentTypesInSameOrderAreStructurallyEqual(byte[] bytes, int start, int length)
        {
            var differentTypeButSameValues = bytes.Select(value => new SingleByteStruct(value)).ToArray();

            var span = new Span<byte>(bytes, start, length);
            var ofSameValuesOfDifferentType = new Span<SingleByteStruct>(differentTypeButSameValues, start, length);

            Assert.True(span.BlockEquals(ofSameValuesOfDifferentType));
            Assert.True(ofSameValuesOfDifferentType.BlockEquals(span));

            Assert.False(span.Equals((Object)ofSameValuesOfDifferentType));
            Assert.NotEqual(span.GetHashCode(), ofSameValuesOfDifferentType.GetHashCode());
        }

        [Theory]
        [MemberData("ValidArraySegments")]
        public void ReadOnlySpansOfEqualValuesOfDifferentTypesInSameOrderAreStructurallyEqual(byte[] bytes, int start, int length)
        {
            var differentTypeButSameValues = bytes.Select(value => new SingleByteStruct(value)).ToArray();

            var span = new ReadOnlySpan<byte>(bytes, start, length);
            var ofSameValuesOfDifferentType = new ReadOnlySpan<SingleByteStruct>(differentTypeButSameValues, start, length);

            Assert.True(span.BlockEquals(ofSameValuesOfDifferentType));
            Assert.True(ofSameValuesOfDifferentType.BlockEquals(span));

            Assert.False(span.Equals((Object)ofSameValuesOfDifferentType));
            Assert.NotEqual(span.GetHashCode(), ofSameValuesOfDifferentType.GetHashCode());
        }

        [Fact]
        public void SpansOfEqualValuesOfDifferentTypesAndDifferentSizesInSameOrderAreStructurallyEqual()
        {
            var guids = Enumerable.Range(0, 10).Select(_ => Guid.NewGuid()).ToArray();
            var guidsBytes = guids.SelectMany(guid => guid.ToByteArray()).ToArray();

            var spanOfGuids = new Span<Guid>(guids);
            var spanOfGuidsByteRepresentation = new Span<byte>(guidsBytes);

            Assert.True(spanOfGuids.BlockEquals(spanOfGuidsByteRepresentation));
            Assert.True(spanOfGuidsByteRepresentation.BlockEquals(spanOfGuids));

            guidsBytes[guidsBytes.Length - 1] = --guidsBytes[guidsBytes.Length - 1]; // make sure that comparison covers whole span

            Assert.False(spanOfGuids.BlockEquals(spanOfGuidsByteRepresentation));
            Assert.False(spanOfGuidsByteRepresentation.BlockEquals(spanOfGuids));
        }

        [Fact]
        public void ReadOnlySpanOfEqualValuesOfDifferentTypesAndDifferentSizesInSameOrderAreStructurallyEqual()
        {
            var guids = Enumerable.Range(0, 10).Select(_ => Guid.NewGuid()).ToArray();
            var guidsBytes = guids.SelectMany(guid => guid.ToByteArray()).ToArray();

            var spanOfGuids = new ReadOnlySpan<Guid>(guids);
            var spanOfGuidsByteRepresentation = new ReadOnlySpan<byte>(guidsBytes);

            Assert.True(spanOfGuids.BlockEquals(spanOfGuidsByteRepresentation));
            Assert.True(spanOfGuidsByteRepresentation.BlockEquals(spanOfGuids));

            guidsBytes[guidsBytes.Length - 1] = --guidsBytes[guidsBytes.Length - 1]; // make sure that comparison covers whole span

            Assert.False(spanOfGuids.BlockEquals(spanOfGuidsByteRepresentation));
            Assert.False(spanOfGuidsByteRepresentation.BlockEquals(spanOfGuids));
        }

        [Fact]
        public unsafe void SlicesReferencingBothHeapsAndStackCanBeComparedForEquality()
        {
            const int arraySize = 100;
            int[] arrayAllocatedOnManagedHeap = new int[arraySize];
            int* arrayAllocatedOnStack = stackalloc int[arraySize];
            int* arrayAllocatedOnUnmanagedHeap = (int*)Marshal.AllocHGlobal(arraySize * sizeof(int)).ToPointer();

            try
            {
                for (int i = 0; i < arraySize; i++)
                {
                    arrayAllocatedOnManagedHeap[i] = i;
                    arrayAllocatedOnStack[i] = i;
                    arrayAllocatedOnUnmanagedHeap[i] = i;
                }

                var stackSlice = new Span<int>(arrayAllocatedOnStack, arraySize);
                var unmanagedHeapSlice = new Span<int>(arrayAllocatedOnUnmanagedHeap, arraySize);
                var managedHeapSlice = new Span<int>(arrayAllocatedOnManagedHeap, 0, arraySize);

                Assert.False(stackSlice.ReferenceEquals(unmanagedHeapSlice));
                Assert.False(stackSlice.ReferenceEquals(managedHeapSlice));
                Assert.False(unmanagedHeapSlice.ReferenceEquals(managedHeapSlice));

                Assert.True(stackSlice.SequenceEqual(unmanagedHeapSlice));
                Assert.True(stackSlice.SequenceEqual(managedHeapSlice));
                Assert.True(unmanagedHeapSlice.SequenceEqual(stackSlice));
                Assert.True(unmanagedHeapSlice.SequenceEqual(managedHeapSlice));
                Assert.True(managedHeapSlice.SequenceEqual(unmanagedHeapSlice));
                Assert.True(managedHeapSlice.SequenceEqual(stackSlice));

                Assert.True(stackSlice.BlockEquals(unmanagedHeapSlice));
                Assert.True(stackSlice.BlockEquals(managedHeapSlice));
                Assert.True(unmanagedHeapSlice.BlockEquals(stackSlice));
                Assert.True(unmanagedHeapSlice.BlockEquals(managedHeapSlice));
                Assert.True(managedHeapSlice.BlockEquals(unmanagedHeapSlice));
                Assert.True(managedHeapSlice.BlockEquals(stackSlice));
            }
            finally
            {
                Marshal.FreeHGlobal(new IntPtr(arrayAllocatedOnUnmanagedHeap));
            }
        }

        [Fact]
        public unsafe void ReadOnlySlicesReferencingBothHeapsAndStackCanBeComparedForEquality()
        {
            const int arraySize = 100;
            int[] arrayAllocatedOnManagedHeap = new int[arraySize];
            int* arrayAllocatedOnStack = stackalloc int[arraySize];
            int* arrayAllocatedOnUnmanagedHeap = (int*)Marshal.AllocHGlobal(arraySize * sizeof(int)).ToPointer();

            try
            {
                for (int i = 0; i < arraySize; i++)
                {
                    arrayAllocatedOnManagedHeap[i] = i;
                    arrayAllocatedOnStack[i] = i;
                    arrayAllocatedOnUnmanagedHeap[i] = i;
                }

                var stackSlice = new ReadOnlySpan<int>(arrayAllocatedOnStack, arraySize);
                var unmanagedHeapSlice = new ReadOnlySpan<int>(arrayAllocatedOnUnmanagedHeap, arraySize);
                var managedHeapSlice = new ReadOnlySpan<int>(arrayAllocatedOnManagedHeap, 0, arraySize);

                Assert.False(stackSlice.ReferenceEquals(unmanagedHeapSlice));
                Assert.False(stackSlice.ReferenceEquals(managedHeapSlice));
                Assert.False(unmanagedHeapSlice.ReferenceEquals(managedHeapSlice));

                Assert.True(stackSlice.SequenceEqual(unmanagedHeapSlice));
                Assert.True(stackSlice.SequenceEqual(managedHeapSlice));
                Assert.True(unmanagedHeapSlice.SequenceEqual(stackSlice));
                Assert.True(unmanagedHeapSlice.SequenceEqual(managedHeapSlice));
                Assert.True(managedHeapSlice.SequenceEqual(unmanagedHeapSlice));
                Assert.True(managedHeapSlice.SequenceEqual(stackSlice));

                Assert.True(stackSlice.BlockEquals(unmanagedHeapSlice));
                Assert.True(stackSlice.BlockEquals(managedHeapSlice));
                Assert.True(unmanagedHeapSlice.BlockEquals(stackSlice));
                Assert.True(unmanagedHeapSlice.BlockEquals(managedHeapSlice));
                Assert.True(managedHeapSlice.BlockEquals(unmanagedHeapSlice));
                Assert.True(managedHeapSlice.BlockEquals(stackSlice));
            }
            finally
            {
                Marshal.FreeHGlobal(new IntPtr(arrayAllocatedOnUnmanagedHeap));
            }
        }

        [Fact]
        public void NonByteWiseEqualsMethodsAreBeingRespectedForSequentialEqualityButIgnoredForStructuralEquality()
        {
            // we just don't call memcmp for these structures
            var sliceOfStructuresWithCustomEquals =
                Enumerable.Range(0, 200).Select(index => new CustomStructWithNonTrivialEquals(index)).ToArray().Slice();
            var sliceOfSameBytes = sliceOfStructuresWithCustomEquals.ToArray().Slice();

            Assert.False(sliceOfStructuresWithCustomEquals.SequenceEqual(sliceOfSameBytes));
            Assert.False(sliceOfSameBytes.SequenceEqual(sliceOfStructuresWithCustomEquals));

            Assert.True(sliceOfStructuresWithCustomEquals.BlockEquals(sliceOfSameBytes));
            Assert.True(sliceOfSameBytes.BlockEquals(sliceOfStructuresWithCustomEquals));
        }

        [Fact]
        public void SequentialEqualityOfSpansOfFloatingPointTypesDoesNotUseBitwiseComparison()
        {
            SequentialEqualityOfSpansOfFloatingPointTypesDoesNotUseBitwiseComparison(
                double.NegativeInfinity, double.PositiveInfinity, -0.0, +0.0, double.NaN);

            SequentialEqualityOfSpansOfFloatingPointTypesDoesNotUseBitwiseComparison(
                float.NegativeInfinity, float.PositiveInfinity, -0.0, +0.0, float.NaN);
        }

        private static void SequentialEqualityOfSpansOfFloatingPointTypesDoesNotUseBitwiseComparison<T>(
            T negativeInfinity, T positiveInfinity, // .Equals => false
            T negativeZero, T positiveZero, // .Equals => TRUE
            T NaN) // .Equals => True
            where T : struct, IEquatable<T>
        {
            const int elementsCountThatNormallyWouldInvolveMemCmp = 1000;
            var negativeInfinities = Enumerable.Repeat(negativeInfinity, elementsCountThatNormallyWouldInvolveMemCmp).ToArray().Slice();
            var positiveInfinities = Enumerable.Repeat(positiveInfinity, elementsCountThatNormallyWouldInvolveMemCmp).ToArray().Slice();

            Assert.Equal(positiveInfinity.Equals(negativeInfinity), positiveInfinities.SequenceEqual(negativeInfinities));
            Assert.False(negativeInfinities.BlockEquals(positiveInfinities));

            var negativeZeroes = Enumerable.Repeat(negativeZero, elementsCountThatNormallyWouldInvolveMemCmp).ToArray().Slice();
            var positiveZeroes = Enumerable.Repeat(positiveZero, elementsCountThatNormallyWouldInvolveMemCmp).ToArray().Slice();

            Assert.Equal(negativeZero.Equals(positiveZero), negativeZeroes.SequenceEqual(positiveZeroes));
            Assert.False(positiveInfinities.BlockEquals(negativeInfinities));

            var nans = Enumerable.Repeat(NaN, elementsCountThatNormallyWouldInvolveMemCmp).ToArray().Slice();

            Assert.Equal(NaN.Equals(NaN), nans.SequenceEqual(nans));
            Assert.True(nans.BlockEquals(nans));
        }

        [Theory]
        [InlineData(1)]
        [InlineData(7)]
        [InlineData(537)] // odd bytes count
        [InlineData(1024)]
        [InlineData(20 * 100)]
        public void AllBytesAreTakenUnderConsideration(int bytesCount)
        {
            var slice = Enumerable.Range(0, bytesCount).Select(index => (byte)index).ToArray().Slice();
            var copy = slice.ToArray().Slice();

            for (int i = 0; i < bytesCount; i++)
            {
                Assert.True(slice.BlockEquals(copy)); // it is equal

                var valueCopy = slice[i];
                slice[i] = (byte)(valueCopy + 1); // lets just change single value

                Assert.False(slice.BlockEquals(copy)); // it is not equal anymore

                slice[i] = valueCopy;
            }
        }

        struct CustomStructWithNonTrivialEquals : IEquatable<CustomStructWithNonTrivialEquals>
        {
            int Field;

            public CustomStructWithNonTrivialEquals(int field) { Field = field; }

            public bool Equals(CustomStructWithNonTrivialEquals other)
            {
                return Field == other.Field + 1245; // just make sure it is not byte-wise equal
            }
        }

        struct SingleByteStruct
        {
            byte Field;

            public SingleByteStruct(byte field) { Field = field; }
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