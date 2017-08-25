using System;
using System.Collections.Generic;
using System.Text;

namespace System.Numerics
{
    public class DenseTensor<T> : Tensor<T>
    {
        private readonly T[] backingArray;

        public DenseTensor(Array fromArray, bool reverseStride = false) : base(GetDimensionsFromArray(fromArray), reverseStride)
        {
            // copy initial array
            backingArray = new T[fromArray.Length];

            int index = 0;
            if (reverseStride)
            {
                // Array is always row-major
                var sourceStrides = ArrayUtilities.GetStrides(dimensions);

                foreach (var item in fromArray)
                {
                    var destIndex = ArrayUtilities.TransformIndexByStrides(index++, sourceStrides, strides);
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
        }

        /// <summary>
        /// Initializes a rank-1 Tensor using the specified <paramref name="size"/>.
        /// </summary>
        /// <param name="size">Size of the tensor</param>
        public DenseTensor(int size, bool reverseStride = false) : base(new [] { size }, reverseStride)
        {
            backingArray = new T[size];
        }

        /// <summary>
        /// Initializes a rank-n Tensor using the dimensions specified in <paramref name="dimensions"/>.
        /// </summary>
        /// <param name="dimensions"></param>
        public DenseTensor(int[] dimensions, bool reverseStride = false) : base(dimensions, reverseStride)
        {
            backingArray = new T[Length];
        }

        public DenseTensor(T[] fromBackingArray, int[] dimensions, bool reverseStride = false) : base(dimensions, reverseStride)
        {
            // keep a reference to the backing array
            backingArray = fromBackingArray ?? throw new ArgumentNullException(nameof(fromBackingArray));

            if (Length != fromBackingArray.Length)
            {
                throw new ArgumentException($"Length of {nameof(fromBackingArray)} ({fromBackingArray.Length}) must match product of {nameof(dimensions)} ({Length}).");
            }
        }

        /// <summary>
        /// Returns a single dimensional view of this Tensor, in C-style ordering
        /// </summary>
        public T[] Buffer => backingArray;
        
        public override T this[Span<int> indices]
        {
            get
            {
                return Buffer[ArrayUtilities.GetIndex(strides, indices)];
            }
            set
            {
                Buffer[ArrayUtilities.GetIndex(strides, indices)] = value;
            }
        }

        public override Tensor<T> Clone()
        {
            return new DenseTensor<T>((T[])backingArray.Clone(), dimensions, IsReversedStride);
        }

        public override Tensor<TResult> CloneEmpty<TResult>(int[] dimensions)
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

        private int GetOffset(int index0, int index1)
        {
            if (Rank != 2)
            {
                throw new ArgumentOutOfRangeException($"Cannot use 2 dimension indexer on {nameof(Tensor<T>)} with {Rank} dimensions.");
            }

            if (index0 < 0 || index0 >= dimensions[0])
            {
                throw new ArgumentOutOfRangeException(nameof(index0));
            }

            if (index1 < 0 || index1 >= dimensions[1])
            {
                throw new ArgumentOutOfRangeException(nameof(index1));
            }

            return index0 * strides[0] + index1 * strides[1];
        }

        private int GetOffsetFromIndices(int[] indices)
        {
            if (indices == null)
            {
                throw new ArgumentNullException(nameof(indices));
            }

            if (indices.Length != dimensions?.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(indices));
            }

            return ArrayUtilities.GetIndex(strides, indices);
        }

        public override Tensor<T> Reshape(params int[] dimensions)
        {
            if (dimensions == null)
            {
                throw new ArgumentNullException(nameof(dimensions));
            }

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
