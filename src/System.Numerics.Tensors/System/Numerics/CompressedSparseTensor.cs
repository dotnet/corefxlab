// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Diagnostics;
using System.Linq;

namespace System.Numerics
{
    /// <summary>
    /// Represents a tensor using compressed sparse format
    /// For a two dimensional tensor this is referred to as compressed sparse row (CSR), compressed sparse column (CSR)
    /// 
    /// In this format, data that is in the same value for the compressed dimension has locality
    /// 
    /// In standard layout of a dense tensor, data with the same value for first dimensions has locality.
    /// As such we'll use reverseStride = false (default) to mean that the first dimension is compressed (CSR)
    /// and reverseStride = true to mean that the last dimension is compressed (CSC)
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CompressedSparseTensor<T> : Tensor<T>
    {
        private Memory<T> values;
        private Memory<int> compressedCounts;
        private Memory<int> indices;

        private int nonZeroCount;

        private readonly int[] nonCompressedStrides;
        private readonly int compressedDimension;

        private const int defaultCapacity = 64;


        public CompressedSparseTensor(ReadOnlySpan<int> dimensions, bool reverseStride = false) : this(dimensions, defaultCapacity, reverseStride)
        { }

        public CompressedSparseTensor(ReadOnlySpan<int> dimensions, int capacity, bool reverseStride = false) : base(dimensions, reverseStride)
        {
            nonZeroCount = 0;
            compressedDimension = reverseStride ? Rank - 1 : 0;
            nonCompressedStrides = (int[])strides.Clone();
            nonCompressedStrides[compressedDimension] = 0;
            var compressedDimensionLength = dimensions[compressedDimension];
            compressedCounts = new int[compressedDimensionLength + 1];
            values = new T[capacity];
            indices = new int[capacity];
        }

        public CompressedSparseTensor(Memory<T> values, Memory<int> compressedCounts, Memory<int> indices, int nonZeroCount, ReadOnlySpan<int> dimensions, bool reverseStride = false) : base(dimensions, reverseStride)
        {
            compressedDimension = reverseStride ? Rank - 1 : 0;
            nonCompressedStrides = (int[])strides.Clone();
            nonCompressedStrides[compressedDimension] = 0;
            this.values = values;
            this.compressedCounts = compressedCounts;
            this.indices = indices;
            this.nonZeroCount = nonZeroCount;
        }

        internal CompressedSparseTensor(Array fromArray, bool reverseStride = false) : base(fromArray, reverseStride)
        {
            nonZeroCount = 0;
            compressedDimension = reverseStride ? Rank - 1 : 0;
            nonCompressedStrides = (int[])strides.Clone();
            nonCompressedStrides[compressedDimension] = 0;
            var compressedDimensionLength = dimensions[compressedDimension];
            compressedCounts = new int[compressedDimensionLength + 1];

            int index = 0;
            if (reverseStride)
            {
                // Array is always row-major
                var sourceStrides = ArrayUtilities.GetStrides(dimensions);

                foreach (T item in fromArray)
                {
                    if (!item.Equals(arithmetic.Zero))
                    {
                        var destIndex = ArrayUtilities.TransformIndexByStrides(index, sourceStrides, false, strides);
                        var compressedIndex = destIndex / strides[compressedDimension];
                        var nonCompressedIndex = destIndex % strides[compressedDimension];

                        SetAt(item, compressedIndex, nonCompressedIndex);
                    }
                    
                    index++;
                }
            }
            else
            {
                foreach (T item in fromArray)
                {
                    if (!item.Equals(arithmetic.Zero))
                    {
                        var compressedIndex = index / strides[compressedDimension];
                        var nonCompressedIndex = index % strides[compressedDimension];

                        SetAt(item, compressedIndex, nonCompressedIndex);
                    }

                    index++;
                }
            }
        }

        public override T this[ReadOnlySpan<int> indices]
        {
            get
            {
                var compressedIndex = indices[compressedDimension];
                var nonCompressedIndex = ArrayUtilities.GetIndex(nonCompressedStrides, indices);

                var valueIndex = 0;

                if (TryFindIndex(compressedIndex, nonCompressedIndex, out valueIndex))
                {
                    return values.Span[valueIndex];
                }

                return arithmetic.Zero;
            }

            set
            {
                var compressedIndex = indices[compressedDimension];
                var nonCompressedIndex = ArrayUtilities.GetIndex(nonCompressedStrides, indices);

                SetAt(value, compressedIndex, nonCompressedIndex);
            }
        }

        public override T GetValue(int index)
        {
            var compressedDimensionStride = strides[compressedDimension];
            Debug.Assert(compressedDimensionStride == strides.Max());

            var compressedIndex = index / compressedDimensionStride;
            var nonCompressedIndex = index % compressedDimensionStride;

            var valueIndex = 0;

            if (TryFindIndex(compressedIndex, nonCompressedIndex, out valueIndex))
            {
                return values.Span[valueIndex];
            }

            return arithmetic.Zero;
        }

        public override void SetValue(int index, T value)
        {
            var compressedDimensionStride = strides[compressedDimension];
            Debug.Assert(compressedDimensionStride == strides.Max());

            var compressedIndex = index / compressedDimensionStride;
            var nonCompressedIndex = index % compressedDimensionStride;

            SetAt(value, compressedIndex, nonCompressedIndex);

        }

        public int Capacity => values.Length;
        public int NonZeroCount => nonZeroCount;

        // unsafe accessors
        public Memory<T> Values => values;
        public Memory<int> CompressedCounts => compressedCounts;
        public Memory<int> Indices => indices;

        private void EnsureCapacity(int min, int allocateIndex = -1)
        {
            if (values.Length < min)
            {
                var newCapacity = values.Length == 0 ? defaultCapacity : values.Length * 2;

                if (newCapacity > Length)
                {
                    newCapacity = (int)Length;
                }

                if (newCapacity < min)
                {
                    newCapacity = min;
                }

                Memory<T> newValues = new T[newCapacity];
                Memory<int> newIndices = new int[newCapacity];

                if (nonZeroCount > 0)
                {
                    if (allocateIndex == -1)
                    {
                        var valuesSpan = values.Span.Slice(0, nonZeroCount);
                        var indicesSpan = indices.Span.Slice(0, nonZeroCount);

                        valuesSpan.CopyTo(newValues.Span);
                        indicesSpan.CopyTo(newIndices.Span);
                    }
                    else
                    {
                        Debug.Assert(allocateIndex <= nonZeroCount);
                        // leave a gap at allocateIndex

                        // copy range before allocateIndex
                        if (allocateIndex > 0)
                        {
                            var valuesSpan = values.Span.Slice(0, allocateIndex);
                            var indicesSpan = indices.Span.Slice(0, allocateIndex);

                            valuesSpan.CopyTo(newValues.Span);
                            indicesSpan.CopyTo(newIndices.Span);
                        }

                        if (allocateIndex < nonZeroCount)
                        {
                            var valuesSpan = values.Span.Slice(allocateIndex, nonZeroCount - allocateIndex);
                            var indicesSpan = indices.Span.Slice(allocateIndex, nonZeroCount - allocateIndex);

                            var newValuesSpan = newValues.Span.Slice(allocateIndex + 1, nonZeroCount - allocateIndex);
                            var newIndicesSpan = newIndices.Span.Slice(allocateIndex + 1, nonZeroCount - allocateIndex);

                            valuesSpan.CopyTo(newValuesSpan);
                            indicesSpan.CopyTo(newIndicesSpan);
                        }
                    }
                }

                values = newValues;
                indices = newIndices;
            }
        }

        private void InsertAt(int valueIndex, T value, int compressedIndex, int nonCompressedIndex)
        {
            Debug.Assert(valueIndex <= nonZeroCount);
            Debug.Assert(compressedIndex < compressedCounts.Length - 1);

            if (values.Length <= valueIndex)
            {
                // allocate a new array, leaving a gap
                EnsureCapacity(valueIndex + 1, valueIndex);
            }
            else if (nonZeroCount != valueIndex)
            {
                // shift values to make a gap
                values.Span.Slice(valueIndex, nonZeroCount - valueIndex).CopyTo(values.Span.Slice(valueIndex + 1));
                indices.Span.Slice(valueIndex, nonZeroCount - valueIndex).CopyTo(indices.Span.Slice(valueIndex + 1));
            }

            values.Span[valueIndex] = value;
            indices.Span[valueIndex] = nonCompressedIndex;

            var compressedCountsSpan = compressedCounts.Span.Slice(compressedIndex + 1);
            for (int i = 0; i < compressedCountsSpan.Length; i++)
            {
                compressedCountsSpan[i]++;
            }
            nonZeroCount++;
        }

        private void RemoveAt(int valueIndex, int compressedIndex)
        {
            Debug.Assert(valueIndex < nonZeroCount);
            Debug.Assert(compressedIndex < compressedCounts.Length - 1);

            // shift values to close the gap
            values.Span.Slice(valueIndex + 1, nonZeroCount - valueIndex - 1).CopyTo(values.Span.Slice(valueIndex));
            indices.Span.Slice(valueIndex + 1, nonZeroCount - valueIndex - 1).CopyTo(indices.Span.Slice(valueIndex));

            var compressedCountsSpan = compressedCounts.Span.Slice(compressedIndex + 1);
            for (int i = 0; i < compressedCountsSpan.Length; i++)
            {
                compressedCountsSpan[i]--;
            }
            nonZeroCount--;
        }

        private void SetAt(T value, int compressedIndex, int nonCompressedIndex)
        {
            var valueIndex = 0;
            bool isZero = value.Equals(arithmetic.Zero);

            if (TryFindIndex(compressedIndex, nonCompressedIndex, out valueIndex))
            {
                if (isZero)
                {
                    RemoveAt(valueIndex, compressedIndex);
                }
                else
                {
                    values.Span[valueIndex] = value;
                    indices.Span[valueIndex] = nonCompressedIndex;
                }
            }
            else if (!isZero)
            {
                InsertAt(valueIndex, value, compressedIndex, nonCompressedIndex);
            }
        }

        /// <summary>
        /// Trys to find the place to store a value
        /// </summary>
        /// <param name="compressedIndex"></param>
        /// <param name="nonCompressedIndex"></param>
        /// <param name="valueIndex"></param>
        /// <returns>True if element is found at specific index, false if no specific index is found and insertion point is returned</returns>
        private bool TryFindIndex(int compressedIndex, int nonCompressedIndex, out int valueIndex)
        {
            if (nonZeroCount == 0)
            {
                valueIndex = 0;
                return false;
            }

            Debug.Assert(compressedIndex < compressedCounts.Length - 1);

            var compressedCountsSpan = compressedCounts.Span;
            var lowerValueIndex = compressedCountsSpan[compressedIndex];
            var upperValueIndex = compressedCountsSpan[compressedIndex + 1];
            var indicesSpan = indices.Span;

            // could be a faster search
            for (valueIndex = lowerValueIndex; valueIndex < upperValueIndex; valueIndex++)
            {
                if (indicesSpan[valueIndex] == nonCompressedIndex)
                {
                    return true;
                }
            }

            return false;
        }


        public override Tensor<T> Clone()
        {
            return new CompressedSparseTensor<T>(values.ToArray(), compressedCounts.ToArray(), indices.ToArray(), nonZeroCount, dimensions, IsReversedStride);
        }

        public override Tensor<TResult> CloneEmpty<TResult>(ReadOnlySpan<int> dimensions)
        {
            return new CompressedSparseTensor<TResult>(dimensions, IsReversedStride);
        }

        public override Tensor<T> Reshape(ReadOnlySpan<int> dimensions)
        {
            // reshape currently has shallow semantics which are not compatible with the backing storage for CompressedSparseTensor
            // which bakes in information about dimensions (compressedCounts and indices)

            var newCompressedDimension = IsReversedStride ? dimensions.Length - 1 : 0;
            var newCompressedDimensionLength = dimensions[newCompressedDimension];
            var newCompressedDimensionStride = (int)(Length / newCompressedDimensionLength);
            
            var newValues = (T[])values.ToArray();
            var newCompressedCounts = new int[newCompressedDimensionLength + 1];
            var newIndices = new int[indices.Length];

            var compressedIndex = 0;

            var compressedCountsSpan = compressedCounts.Span;
            var indicesSpan = indices.Span.Slice(0, nonZeroCount);
            for (int valueIndex = 0; valueIndex < indicesSpan.Length; valueIndex++)
            {
                while (valueIndex >= compressedCountsSpan[compressedIndex + 1])
                {
                    compressedIndex++;
                    Debug.Assert(compressedIndex < compressedCounts.Length);
                }

                var currentIndex = indicesSpan[valueIndex] + compressedIndex * strides[compressedDimension];

                newIndices[valueIndex] = currentIndex % newCompressedDimensionStride;

                var newCompressedIndex = currentIndex / newCompressedDimensionStride;
                newCompressedCounts[newCompressedIndex + 1] = valueIndex + 1;
            }

            return new CompressedSparseTensor<T>(newValues, newCompressedCounts, newIndices, nonZeroCount, dimensions, IsReversedStride);
        }

        public override DenseTensor<T> ToDenseTensor()
        {
            var denseTensor = new DenseTensor<T>(Dimensions, reverseStride: IsReversedStride);

            var compressedIndex = 0;

            var compressedCountsSpan = compressedCounts.Span;
            var indicesSpan = indices.Span.Slice(0, nonZeroCount);
            var valuesSpan = values.Span.Slice(0, nonZeroCount);
            for (int valueIndex = 0; valueIndex < valuesSpan.Length; valueIndex++)
            {
                while (valueIndex >= compressedCountsSpan[compressedIndex + 1])
                {
                    compressedIndex++;
                    Debug.Assert(compressedIndex < compressedCounts.Length);
                }

                var index = indicesSpan[valueIndex] + compressedIndex * strides[compressedDimension];

                denseTensor.SetValue(index, valuesSpan[valueIndex]);
            }

            return denseTensor;
        }

        public override CompressedSparseTensor<T> ToCompressedSparseTensor()
        {
            // Create a copy of the backing storage, eliminating any unused space.
            var newValues = values.Slice(0, nonZeroCount).ToArray();
            var newIndicies = indices.Slice(0, nonZeroCount).ToArray();

            return new CompressedSparseTensor<T>(newValues, compressedCounts.ToArray(), newIndicies, nonZeroCount, dimensions, IsReversedStride);
        }

        public override SparseTensor<T> ToSparseTensor()
        {
            var sparseTensor = new SparseTensor<T>(dimensions, capacity: NonZeroCount, reverseStride: IsReversedStride);

            var compressedIndex = 0;

            var compressedCountsSpan = compressedCounts.Span;
            var indicesSpan = indices.Span.Slice(0, nonZeroCount);
            var valuesSpan = values.Span.Slice(0, nonZeroCount);
            for (int valueIndex = 0; valueIndex < valuesSpan.Length; valueIndex++)
            {
                while (valueIndex >= compressedCountsSpan[compressedIndex + 1])
                {
                    compressedIndex++;
                    Debug.Assert(compressedIndex < compressedCounts.Length);
                }

                var index = indicesSpan[valueIndex] + compressedIndex * strides[compressedDimension];
                
                sparseTensor.SetValue(index, valuesSpan[valueIndex]);
            }

            return sparseTensor;
        }
    }
}
