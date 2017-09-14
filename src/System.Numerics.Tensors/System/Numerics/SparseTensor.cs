using System.Collections.Generic;

namespace System.Numerics
{
    public class SparseTensor<T> : Tensor<T>
    {
        private readonly Dictionary<int, T> values;

        public SparseTensor(int[] dimensions, bool reverseStride= false) : base(dimensions, reverseStride)
        {
            values = new Dictionary<int, T>();
        }

        internal SparseTensor(Dictionary<int, T> values, int[] dimensions, bool reverseStride = false) : base(dimensions, reverseStride)
        {
            this.values = values;
        }

        public SparseTensor(Array fromArray, bool reverseStride = false) : base(GetDimensionsFromArray(fromArray), reverseStride)
        {
            values = new Dictionary<int, T>();

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

        public override T this[Span<int> indices]
        {
            get
            {
                var index = ArrayUtilities.GetIndex(strides, indices);

                T value;

                if (!values.TryGetValue(index, out value))
                {
                    value = arithmetic.Zero;
                }

                return value;
            }

            set
            {
                var index = ArrayUtilities.GetIndex(strides, indices);

                if (value.Equals(arithmetic.Zero))
                {
                    values.Remove(index);
                }
                else
                {
                    values[index] = value;
                }
            }
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
            var valueCopy = new Dictionary<int, T>(values);
            return new SparseTensor<T>(valueCopy, dimensions, IsReversedStride);
        }

        public override Tensor<TResult> CloneEmpty<TResult>(int[] dimensions)
        {
            return new SparseTensor<TResult>(dimensions, IsReversedStride);
        }

        public override Tensor<T> Reshape(params int[] dimensions)
        {
            return new SparseTensor<T>(values, dimensions, IsReversedStride);
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
    }
}
