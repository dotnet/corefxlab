// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Diagnostics;

namespace System.Numerics
{
    public class SparseTensor<T> : Tensor<T>
    {
        private readonly Dictionary<int, T> values;

        public SparseTensor(ReadOnlySpan<int> dimensions, bool reverseStride = false, int capacity = 0) : base(dimensions, reverseStride)
        {
            values = new Dictionary<int, T>(capacity);
        }

        internal SparseTensor(Dictionary<int, T> values, ReadOnlySpan<int> dimensions, bool reverseStride = false) : base(dimensions, reverseStride)
        {
            this.values = values;
        }

        internal SparseTensor(Array fromArray, bool reverseStride = false) : base(GetDimensionsFromArray(fromArray), reverseStride)
        {
            values = new Dictionary<int, T>(fromArray.Length);

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
                        values[destIndex] = item;
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
                        values[index] = item;
                    }

                    index++;
                }
            }
        }

        public override T GetValue(int index)
        {
            T value;

            if (!values.TryGetValue(index, out value))
            {
                value = arithmetic.Zero;
            }

            return value;
        }

        public override void SetValue(int index, T value)
        {
            if (value.Equals(arithmetic.Zero))
            {
                values.Remove(index);
            }
            else
            {
                values[index] = value;
            }
        }

        public int NonZeroCount => values.Count;

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
            var valueCopy = new Dictionary<int, T>(values);
            return new SparseTensor<T>(valueCopy, dimensions, IsReversedStride);
        }

        public override Tensor<TResult> CloneEmpty<TResult>(ReadOnlySpan<int> dimensions)
        {
            return new SparseTensor<TResult>(dimensions, IsReversedStride);
        }

        public override Tensor<T> Reshape(ReadOnlySpan<int> dimensions)
        {
            return new SparseTensor<T>(values, dimensions, IsReversedStride);
        }

        public override DenseTensor<T> ToDenseTensor()
        {
            var denseTensor = new DenseTensor<T>(Dimensions, reverseStride: IsReversedStride);
            
            // only set non-zero values
            foreach (var pair in values)
            {
                denseTensor.SetValue(pair.Key, pair.Value);
            }

            return denseTensor;
        }

        public override SparseTensor<T> ToSparseTensor()
        {
            var valueCopy = new Dictionary<int, T>(values);
            return new SparseTensor<T>(valueCopy, dimensions, IsReversedStride);
        }

        public override CompressedSparseTensor<T> ToCompressedSparseTensor()
        {
            var compressedSparseTensor = new CompressedSparseTensor<T>(dimensions, capacity: NonZeroCount, reverseStride: IsReversedStride);

            Span<int> indices = new Span<int>(new int[Rank]);
            foreach (var pair in values)
            {
                compressedSparseTensor.SetValue(pair.Key, pair.Value);
            }
            return compressedSparseTensor;
        }
    }
}
