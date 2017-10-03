// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace System.Numerics
{
    public class DenseTensor<T> : Tensor<T>
    {
        private readonly Memory<T> memory;

        internal DenseTensor(Array fromArray, bool reverseStride = false) : base(GetDimensionsFromArray(fromArray), reverseStride)
        {
            // copy initial array
            var backingArray = new T[fromArray.Length];

            int index = 0;
            if (reverseStride)
            {
                // Array is always row-major
                var sourceStrides = ArrayUtilities.GetStrides(dimensions);

                foreach (var item in fromArray)
                {
                    var destIndex = ArrayUtilities.TransformIndexByStrides(index++, sourceStrides, false, strides);
                    backingArray[destIndex] = (T)item;
                }
            }
            else
            {
                foreach (var item in fromArray)
                {
                    backingArray[index++] = (T)item;
                }
            }
            memory = backingArray;
        }

        /// <summary>
        /// Initializes a rank-1 Tensor using the specified <paramref name="size"/>.
        /// </summary>
        /// <param name="size">Size of the tensor</param>
        public DenseTensor(int size, bool reverseStride = false) : base(new [] { size }, reverseStride)
        {
            memory = new T[size];
        }

        /// <summary>
        /// Initializes a rank-n Tensor using the dimensions specified in <paramref name="dimensions"/>.
        /// </summary>
        /// <param name="dimensions"></param>
        public DenseTensor(ReadOnlySpan<int> dimensions, bool reverseStride = false) : base(dimensions, reverseStride)
        {
            memory = new T[Length];
        }

        public DenseTensor(Memory<T> memory, ReadOnlySpan<int> dimensions, bool reverseStride = false) : base(dimensions, reverseStride)
        {
            // keep a reference to the backing array
            this.memory = memory;

            if (Length != memory.Length)
            {
                throw new ArgumentException($"Length of {nameof(memory)} ({memory.Length}) must match product of {nameof(dimensions)} ({Length}).");
            }
        }

        /// <summary>
        /// Returns a single dimensional view of this Tensor, in C-style ordering
        /// </summary>
        public Memory<T> Buffer => memory;

        public override T GetValue(int index)
        {
            return Buffer.Span[index];
        }

        public override void SetValue(int index, T value)
        {
            Buffer.Span[index] = value;
        }

        public override Tensor<T> Clone()
        {
            return new DenseTensor<T>(Buffer.ToArray(), dimensions, IsReversedStride);
        }

        public override Tensor<TResult> CloneEmpty<TResult>(ReadOnlySpan<int> dimensions)
        {
            return new DenseTensor<TResult>(dimensions, IsReversedStride);
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

        public override Tensor<T> Reshape(ReadOnlySpan<int> dimensions)
        {
            if (dimensions.Length == 0)
            {
                throw new ArgumentException("Dimensions must contain elements.", nameof(dimensions));
            }

            var newSize = ArrayUtilities.GetProduct(dimensions);

            if (newSize != Length)
            {
                throw new ArgumentException($"Cannot reshape array due to mismatch in lengths, currently {Length} would become {newSize}.", nameof(dimensions));
            }

            return new DenseTensor<T>(Buffer, dimensions, IsReversedStride);
        }
    }
}
