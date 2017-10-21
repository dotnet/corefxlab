// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Diagnostics;

namespace System.Numerics
{
    /// <summary>
    /// Represents a multi-dimensional collection of objects of type T that can be accessed by indices.  Unlike other Tensor<T> implementations SparseTensor<T> does not expose its backing storage.  It is meant as an intermediate to be used to build other Tensors, such as CompressedSparseTensor.  Unlike CompressedSparseTensor where insertions are O(n), insertions to SparseTensor<T> are nominally O(1).
    /// </summary>
    /// <typeparam name="T">type contained within the Tensor.  Typically a value type such as int, double, float, etc.</typeparam>
    public class SparseTensor<T> : Tensor<T>
    {
        private readonly Dictionary<int, T> values;
        /// <summary>
        /// Constructs a new CompressedSparseTensor of the specifed dimensions, initial capacity, and stride ordering.
        /// </summary>
        /// <param name="dimensions"></param>
        /// <param name="reverseStride"></param>
        /// <param name="capacity"></param>
        public SparseTensor(ReadOnlySpan<int> dimensions, bool reverseStride = false, int capacity = 0) : base(dimensions, reverseStride)
        {
            values = new Dictionary<int, T>(capacity);
        }

        internal SparseTensor(Dictionary<int, T> values, ReadOnlySpan<int> dimensions, bool reverseStride = false) : base(dimensions, reverseStride)
        {
            this.values = values;
        }

        internal SparseTensor(Array fromArray, bool reverseStride = false) : base(fromArray, reverseStride)
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

        /// <summary>
        /// Gets the value at the specied index, where index is a linearized version of n-dimension indices using strides.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public override T GetValue(int index)
        {
            T value;

            if (!values.TryGetValue(index, out value))
            {
                value = arithmetic.Zero;
            }

            return value;
        }

        /// <summary>
        /// Sets the value at the specied index, where index is a linearized version of n-dimension indices using strides.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
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

        /// <summary>
        /// Get's the number on non-zero values currently being stored in this tensor.
        /// </summary>
        public int NonZeroCount => values.Count;

        /// <summary>
        /// Creates a copy of this tensor, with new backing storage.
        /// </summary>
        /// <returns></returns>
        public override Tensor<T> Clone()
        {
            var valueCopy = new Dictionary<int, T>(values);
            return new SparseTensor<T>(valueCopy, dimensions, IsReversedStride);
        }

        /// <summary>
        /// Creates a new Tensor of a different type with the specified dimensions and the same layout as this tensor with elements initialized to their default value.
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="dimensions"></param>
        /// <returns></returns>
        public override Tensor<TResult> CloneEmpty<TResult>(ReadOnlySpan<int> dimensions)
        {
            return new SparseTensor<TResult>(dimensions, IsReversedStride);
        }

        /// <summary>
        /// Reshapes the current tensor to new dimensions, using the same backing storage.
        /// </summary>
        /// <param name="dimensions"></param>
        /// <returns></returns>
        public override Tensor<T> Reshape(ReadOnlySpan<int> dimensions)
        {
            return new SparseTensor<T>(values, dimensions, IsReversedStride);
        }

        /// <summary>
        /// Creates a copy of this tensor as a DenseTensor<T>.  
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Creates a copy of this tensor as a new SparseTensor<T> eliminating any unused space in the backing storage.
        /// </summary>
        /// <returns></returns>
        public override SparseTensor<T> ToSparseTensor()
        {
            var valueCopy = new Dictionary<int, T>(values);
            return new SparseTensor<T>(valueCopy, dimensions, IsReversedStride);
        }

        /// <summary>
        /// Creates a copy of this tensor as a CompressedSparseTensor<T>.
        /// </summary>
        /// <returns></returns>
        public override CompressedSparseTensor<T> ToCompressedSparseTensor()
        {
            var compressedSparseTensor = new CompressedSparseTensor<T>(dimensions, capacity: NonZeroCount, reverseStride: IsReversedStride);

            foreach (var pair in values)
            {
                compressedSparseTensor.SetValue(pair.Key, pair.Value);
            }
            return compressedSparseTensor;
        }
    }
}
