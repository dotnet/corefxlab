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

        [Theory]
        [MemberData("FullArraySegments")]
        public void SpanArrayEquivalenceAndImplicitCastsAreEqual(byte[] bytes)
        {
            var span = new Span<byte>(bytes);
            var readOnlySpan = new ReadOnlySpan<byte>(bytes);
            ReadOnlySpan<byte> impReadOnlySpan = span;
            Span<byte> impSpanArray = bytes;
            ReadOnlySpan<byte> impReadOnlySpanArray = bytes;

            Assert.True(span == bytes);
            Assert.False(span != bytes);
            Assert.True(readOnlySpan == bytes);
            Assert.False(readOnlySpan != bytes);
            Assert.True(impReadOnlySpan == bytes);
            Assert.False(impReadOnlySpan != bytes);
            Assert.True(impSpanArray == bytes);
            Assert.False(impSpanArray != bytes);
            Assert.True(impReadOnlySpanArray == bytes);
            Assert.False(impReadOnlySpanArray != bytes);

            Assert.True(bytes == span);
            Assert.False(bytes != span);
            Assert.True(readOnlySpan == span);
            Assert.False(readOnlySpan != span);
            Assert.True(impReadOnlySpan == span);
            Assert.False(impReadOnlySpan != span);
            Assert.True(impSpanArray == span);
            Assert.False(impSpanArray != span);
            Assert.True(impReadOnlySpanArray == span);
            Assert.False(impReadOnlySpanArray != span);

            Assert.True(span == readOnlySpan);
            Assert.False(span != readOnlySpan);
            Assert.True(bytes == readOnlySpan);
            Assert.False(bytes != readOnlySpan);
            Assert.True(impReadOnlySpan == readOnlySpan);
            Assert.False(impReadOnlySpan != readOnlySpan);
            Assert.True(impSpanArray == readOnlySpan);
            Assert.False(impSpanArray != readOnlySpan);
            Assert.True(impReadOnlySpanArray == readOnlySpan);
            Assert.False(impReadOnlySpanArray != readOnlySpan);

            Assert.True(span == impSpanArray);
            Assert.False(span != impSpanArray);
            Assert.True(readOnlySpan == impSpanArray);
            Assert.False(readOnlySpan != impSpanArray);
            Assert.True(bytes == impSpanArray);
            Assert.False(bytes != impSpanArray);
            Assert.True(impReadOnlySpan == impSpanArray);
            Assert.False(impReadOnlySpan != impSpanArray);
            Assert.True(impReadOnlySpanArray == impSpanArray);
            Assert.False(impReadOnlySpanArray != impSpanArray);

            Assert.True(span == impReadOnlySpan);
            Assert.False(span != impReadOnlySpan);
            Assert.True(readOnlySpan == impReadOnlySpan);
            Assert.False(readOnlySpan != impReadOnlySpan);
            Assert.True(bytes == impReadOnlySpan);
            Assert.False(bytes != impReadOnlySpan);
            Assert.True(impSpanArray == impReadOnlySpan);
            Assert.False(impSpanArray != impReadOnlySpan);
            Assert.True(impReadOnlySpanArray == impReadOnlySpan);
            Assert.False(impReadOnlySpanArray != impReadOnlySpan);

            Assert.True(span == impReadOnlySpanArray);
            Assert.False(span != impReadOnlySpanArray);
            Assert.True(readOnlySpan == impReadOnlySpanArray);
            Assert.False(readOnlySpan != impReadOnlySpanArray);
            Assert.True(impReadOnlySpan == impReadOnlySpanArray);
            Assert.False(impReadOnlySpan != impReadOnlySpanArray);
            Assert.True(impSpanArray == impReadOnlySpanArray);
            Assert.False(impSpanArray != impReadOnlySpanArray);
            Assert.True(bytes == impReadOnlySpanArray);
            Assert.False(bytes != impReadOnlySpanArray);

            Assert.True(span.Equals(bytes));
            Assert.True(span.Equals(readOnlySpan));
            Assert.True(readOnlySpan.Equals(bytes));
            Assert.True(readOnlySpan.Equals(span));

            Assert.False(span.Equals((object)bytes));
            Assert.False(span.Equals((object)readOnlySpan));
            Assert.False(readOnlySpan.Equals((object)bytes));
            Assert.False(readOnlySpan.Equals((object)span));
        }

        [Theory]
        [MemberData("ValidArraySegments")]
        public void RangedSpanEquivalenceAndImplicitCastsAreEqual(byte[] bytes, int start, int length)
        {
            var span = new Span<byte>(bytes, start, length);
            var readOnlySpan = new ReadOnlySpan<byte>(bytes, start, length);
            ReadOnlySpan<byte> impReadOnlySpan = span;

            Assert.True(readOnlySpan == span);
            Assert.False(readOnlySpan != span);
            Assert.True(impReadOnlySpan == span);
            Assert.False(impReadOnlySpan != span);

            Assert.True(span == readOnlySpan);
            Assert.False(span != readOnlySpan);
            Assert.True(impReadOnlySpan == readOnlySpan);
            Assert.False(impReadOnlySpan != readOnlySpan);

            Assert.True(span == impReadOnlySpan);
            Assert.False(span != impReadOnlySpan);
            Assert.True(readOnlySpan == impReadOnlySpan);
            Assert.False(readOnlySpan != impReadOnlySpan);

            Assert.True(span.Equals(readOnlySpan));
            Assert.True(readOnlySpan.Equals(span));
            Assert.True(span.Equals(impReadOnlySpan));
            Assert.True(readOnlySpan.Equals(impReadOnlySpan));
            Assert.True(impReadOnlySpan.Equals(readOnlySpan));
            Assert.True(impReadOnlySpan.Equals(span));

            Assert.False(span.Equals((object)readOnlySpan));
            Assert.False(readOnlySpan.Equals((object)span));
            Assert.False(span.Equals((object)impReadOnlySpan));
            Assert.False(impReadOnlySpan.Equals((object)span));

            Assert.True(readOnlySpan.Equals((object)impReadOnlySpan));
            Assert.True(impReadOnlySpan.Equals((object)readOnlySpan));
        }

        // ref is used just for the structCopy scenario, otherwise it would always be copy

        private static void SpansReferencingSameMemoryAreEqualInEveryAspect(ref Span<byte> span, ref Span<byte> pointingToSameMemory)
        {
            Assert.True(span.ReferenceEquals(pointingToSameMemory));
            Assert.True(span == pointingToSameMemory);
            Assert.True(pointingToSameMemory == span);
            Assert.False(span != pointingToSameMemory);
            Assert.False(pointingToSameMemory != span);
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
            Assert.True(span == pointingToSameMemory);
            Assert.True(pointingToSameMemory == span);
            Assert.False(span != pointingToSameMemory);
            Assert.False(pointingToSameMemory != span);
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

            Assert.False(span == ofSameValues);
            Assert.True(span != ofSameValues);
            Assert.False(ofSameValues == span);
            Assert.True(ofSameValues != span);

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

            Assert.False(span == ofSameValues);
            Assert.True(span != ofSameValues);
            Assert.False(ofSameValues == span);
            Assert.True(ofSameValues != span);

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

            Assert.False(span == ofDifferentValues);
            Assert.True(span != ofDifferentValues);
            Assert.False(ofDifferentValues == span);
            Assert.True(ofDifferentValues != span);

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

            Assert.False(span == ofDifferentValues);
            Assert.True(span != ofDifferentValues);
            Assert.False(ofDifferentValues == span);
            Assert.True(ofDifferentValues != span);

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

                Assert.False(stackSlice == unmanagedHeapSlice);
                Assert.False(stackSlice == managedHeapSlice);
                Assert.False(unmanagedHeapSlice == managedHeapSlice);
                Assert.False(unmanagedHeapSlice == stackSlice);
                Assert.False(managedHeapSlice == stackSlice);
                Assert.False(managedHeapSlice == unmanagedHeapSlice);

                Assert.True(stackSlice != unmanagedHeapSlice);
                Assert.True(stackSlice != managedHeapSlice);
                Assert.True(unmanagedHeapSlice != managedHeapSlice);
                Assert.True(unmanagedHeapSlice != stackSlice);
                Assert.True(managedHeapSlice != stackSlice);
                Assert.True(managedHeapSlice != unmanagedHeapSlice);

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

                Assert.False(stackSlice == unmanagedHeapSlice);
                Assert.False(stackSlice == managedHeapSlice);
                Assert.False(unmanagedHeapSlice == managedHeapSlice);
                Assert.False(unmanagedHeapSlice == stackSlice);
                Assert.False(managedHeapSlice == stackSlice);
                Assert.False(managedHeapSlice == unmanagedHeapSlice);

                Assert.True(stackSlice != unmanagedHeapSlice);
                Assert.True(stackSlice != managedHeapSlice);
                Assert.True(unmanagedHeapSlice != managedHeapSlice);
                Assert.True(unmanagedHeapSlice != stackSlice);
                Assert.True(managedHeapSlice != stackSlice);
                Assert.True(managedHeapSlice != unmanagedHeapSlice);

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