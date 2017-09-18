using System.Diagnostics;

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
        private T[] values;
        private int[] compressedCounts;
        private int[] indices;

        private int nonZeroCount;

        private readonly int[] nonCompressedStrides;
        private readonly int compressedDimension;

        private const int defaultCapacity = 64;


        public CompressedSparseTensor(int[] dimensions, bool reverseStride = false) : this(dimensions, defaultCapacity, reverseStride)
        { }

        public CompressedSparseTensor(int[] dimensions, int capacity, bool reverseStride = false) : base(dimensions, reverseStride)
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

        public CompressedSparseTensor(T[] values, int[] compressedCounts, int[] indices, int nonZeroCount, int[] dimensions, bool reverseStride = false) : base(dimensions, reverseStride)
        {
            compressedDimension = reverseStride ? Rank - 1 : 0;
            nonCompressedStrides = (int[])strides.Clone();
            nonCompressedStrides[compressedDimension] = 0;
            this.values = values;
            this.compressedCounts = compressedCounts;
            this.indices = indices;
            this.nonZeroCount = nonZeroCount;
        }

        internal CompressedSparseTensor(Array fromArray, bool reverseStride = false) : base(GetDimensionsFromArray(fromArray), reverseStride)
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

        public override T this[Span<int> indices]
        {
            get
            {
                var compressedIndex = indices[compressedDimension];
                var nonCompressedIndex = ArrayUtilities.GetIndex(nonCompressedStrides, indices);

                var valueIndex = 0;

                if (TryFindIndex(compressedIndex, nonCompressedIndex, out valueIndex))
                {
                    return values[valueIndex];
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

        public int Capacity => values.Length;
        public int NonZeroCount => nonZeroCount;

        // unsafe accessors
        public T[] Values => values;
        public int[] CompressedCounts => compressedCounts;
        public int[] Indices => indices;

        private void EnsureCapacity(int min, int allocateIndex = -1)
        {
            if (values == null || values.Length < min)
            {
                var newCapacity = (values == null || values.Length == 0) ? defaultCapacity : values.Length * 2;

                if (newCapacity > Length)
                {
                    newCapacity = (int)Length;
                }

                if (newCapacity < min)
                {
                    newCapacity = min;
                }

                T[] newValues = new T[newCapacity];
                int[] newIndices = new int[newCapacity];

                if (nonZeroCount > 0)
                {
                    if (allocateIndex == -1)
                    {
                        Array.Copy(values, newValues, nonZeroCount);
                        Array.Copy(indices, newIndices, nonZeroCount);
                    }
                    else
                    {
                        Debug.Assert(allocateIndex <= nonZeroCount);
                        // leave a gap at allocateIndex

                        // copy range before allocateIndex
                        if (allocateIndex > 0)
                        {
                            Array.Copy(values, newValues, allocateIndex);
                            Array.Copy(indices, newIndices, allocateIndex);
                        }

                        if (allocateIndex < nonZeroCount)
                        {
                            Array.Copy(values, allocateIndex, newValues, allocateIndex + 1, nonZeroCount - allocateIndex);
                            Array.Copy(indices, allocateIndex, newIndices, allocateIndex + 1, nonZeroCount - allocateIndex);
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

            if (values == null || values.Length <= valueIndex)
            {
                // allocate a new array, leaving a gap
                EnsureCapacity(valueIndex + 1, valueIndex);

                // insert into the gap
                values[valueIndex] = value;
                indices[valueIndex] = nonCompressedIndex;
            }
            else if (nonZeroCount != valueIndex)
            {
                // shift values to make a gap
                Array.Copy(values, valueIndex, values, valueIndex + 1, nonZeroCount - valueIndex);
                Array.Copy(indices, valueIndex, indices, valueIndex + 1, nonZeroCount - valueIndex);
            }

            values[valueIndex] = value;
            indices[valueIndex] = nonCompressedIndex;

            for (int i = compressedIndex + 1; i < compressedCounts.Length; i++)
            {
                compressedCounts[i]++;
            }
            nonZeroCount++;
        }

        private void RemoveAt(int valueIndex, int compressedIndex)
        {
            Debug.Assert(valueIndex < nonZeroCount);
            Debug.Assert(compressedIndex < compressedCounts.Length - 1);

            // shift values to close the gap
            Array.Copy(values, valueIndex + 1, values, valueIndex, nonZeroCount - valueIndex - 1);
            Array.Copy(indices, valueIndex + 1, indices, valueIndex, nonZeroCount - valueIndex - 1);

            for (int i = compressedIndex + 1; i < compressedCounts.Length; i++)
            {
                compressedCounts[i]--;
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
                    values[valueIndex] = value;
                    indices[valueIndex] = nonCompressedIndex;
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

            var lowerValueIndex = compressedCounts[compressedIndex];
            var upperValueIndex = compressedCounts[compressedIndex + 1];

            // could be a faster search

            for (valueIndex = lowerValueIndex; valueIndex < upperValueIndex; valueIndex++)
            {
                if (indices[valueIndex] == nonCompressedIndex)
                {
                    return true;
                }
            }

            return false;
        }

        private static int[] GetDimensionsFromArray(Array fromArray)
        {
            if (fromArray == null)
            {
                throw new ArgumentNullException(nameof(fromArray));
            }

            var dimensions = new int[fromArray.Rank];
            for (int i = 0; i < dimensions.Length; i++)
            {
                dimensions[i] = fromArray.GetLength(i);
            }

            return dimensions;
        }

        public override Tensor<T> Clone()
        {
            return new CompressedSparseTensor<T>((T[])values.Clone(), (int[])compressedCounts.Clone(), (int[])indices.Clone(), nonZeroCount, dimensions, IsReversedStride);
        }

        public override Tensor<TResult> CloneEmpty<TResult>(int[] dimensions)
        {
            return new CompressedSparseTensor<TResult>(dimensions, IsReversedStride);
        }

        public override Tensor<T> Reshape(params int[] dimensions)
        {
            // reshape currently has shallow semantics which are not compatible with the backing storage for CompressedSparseTensor
            // which bakes in information about dimensions (compressedCounts and indices)

            var newCompressedDimension = IsReversedStride ? dimensions.Length - 1 : 0;
            var newCompressedDimensionLength = dimensions[newCompressedDimension];
            var newCompressedDimensionStride = (int)(Length / newCompressedDimensionLength);
            

            var newValues = (T[])values.Clone();
            var newCompressedCounts = new int[newCompressedDimensionLength + 1];
            var newIndices = new int[indices.Length];

            var compressedIndex = 0;

            for (int valueIndex = 0; valueIndex < nonZeroCount; valueIndex++)
            {
                while (valueIndex >= compressedCounts[compressedIndex + 1])
                {
                    compressedIndex++;
                    Debug.Assert(compressedIndex < compressedCounts.Length);
                }

                var currentIndex = indices[valueIndex] + compressedIndex * strides[compressedDimension];

                newIndices[valueIndex] = currentIndex % newCompressedDimensionStride;

                var newCompressedIndex = currentIndex / newCompressedDimensionStride;
                newCompressedCounts[newCompressedIndex + 1] = valueIndex + 1;
            }

            return new CompressedSparseTensor<T>(newValues, newCompressedCounts, newIndices, nonZeroCount, dimensions, IsReversedStride);
        }

        public override DenseTensor<T> ToDenseTensor()
        {
            var denseTensor = new DenseTensor<T>(dimensions, reverseStride: IsReversedStride);

            var compressedIndex = 0;

            for (int valueIndex = 0; valueIndex < nonZeroCount; valueIndex++)
            {
                while (valueIndex >= compressedCounts[compressedIndex + 1])
                {
                    compressedIndex++;
                    Debug.Assert(compressedIndex < compressedCounts.Length);
                }

                var index = indices[valueIndex] + compressedIndex * strides[compressedDimension];

                denseTensor.Buffer[index] = values[valueIndex];
            }

            return denseTensor;
        }

        public override CompressedSparseTensor<T> ToCompressedSparseTensor()
        {
            var newValues = new T[NonZeroCount];
            Array.Copy(values, newValues, NonZeroCount);

            var newIndicies = new int[NonZeroCount];
            Array.Copy(indices, newIndicies, NonZeroCount);

            return new CompressedSparseTensor<T>(newValues, (int[])compressedCounts.Clone(), newIndicies, nonZeroCount, dimensions, IsReversedStride);
        }

        public override SparseTensor<T> ToSparseTensor()
        {
            var sparseTensor = new SparseTensor<T>(dimensions, capacity: NonZeroCount, reverseStride: IsReversedStride);

            var compressedIndex = 0;

            for (int valueIndex = 0; valueIndex < nonZeroCount; valueIndex++)
            {
                while (valueIndex >= compressedCounts[compressedIndex + 1])
                {
                    compressedIndex++;
                    Debug.Assert(compressedIndex < compressedCounts.Length);
                }

                var index = indices[valueIndex] + compressedIndex * strides[compressedDimension];

                // interals access :( remove if we expose layout-based indexing
                sparseTensor.values[index] = values[valueIndex];
            }

            return sparseTensor;
        }
    }
}
