using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace System.Numerics.Tensors.System.Numerics
{
    public struct TensorShape
    {
        private readonly int[] dimensions;
        private readonly int[] strides;
        private readonly int offset;
        private readonly bool isReversedStride;
        private readonly long length;
        private readonly long memoryLength;
        
        public ReadOnlySpan<int> Dimensions => dimensions;
        public ReadOnlySpan<int> Strides => strides;
        public int Offset => offset;
        public int Rank => dimensions.Length;
        public bool IsReversedStride => isReversedStride;
        public long Length => length;
        public long MemoryLength => memoryLength;

        public TensorShape(ReadOnlySpan<int> dimensions, bool isReversedStride = false)
        {
            this.dimensions = new int[dimensions.Length];
            strides = new int[dimensions.Length];
            this.isReversedStride = isReversedStride;
            offset = 0;
            length = 1;
            memoryLength = 1;

            for (int i = 0; i < dimensions.Length; i++)
            {
                var index = isReversedStride ? i : dimensions.Length - 1 - i;
                if (dimensions[index] < 1)
                {
                    throw new ArgumentOutOfRangeException(nameof(dimensions), "Dimensions must be positive and non-zero");
                }
                this.dimensions[index] = dimensions[index];
                strides[index] = (int)length;
                memoryLength = length *= dimensions[index];
            }
        }

        public TensorShape(ReadOnlySpan<int> dimensions, ReadOnlySpan<int> strides, int offset = 0)
        {
            this.dimensions = new int[dimensions.Length];
            this.strides = new int[dimensions.Length];
            this.offset = offset;
            length = 1;
            memoryLength = offset;
            bool? stridesAscending = null;

            for (int i = 0; i < dimensions.Length; i++)
            {
                if (dimensions[i] < 1)
                {
                    throw new ArgumentOutOfRangeException(nameof(dimensions), "Dimensions must be positive and non-zero");
                }

                if (strides[i] < 1)
                {
                    throw new ArgumentOutOfRangeException(nameof(strides), "Strides must be positive and non-zero");
                }

                if (i > 0 && strides[i] != strides[i - 1])
                {
                    if (stridesAscending.HasValue && stridesAscending != (strides[i] > strides[i - 1]))
                    {
                        throw new ArgumentException(nameof(strides), "Strides must be monotonic");
                    }

                    stridesAscending = (strides[i] > strides[i - 1]);
                }

                this.dimensions[i] = dimensions[i];
                this.strides[i] = strides[i];
                length *= dimensions[i];
                memoryLength += dimensions[i] * strides[i];
            }

            isReversedStride = stridesAscending ?? false;
        }

        public int GetLinearIndex(ReadOnlySpan<int> indices)
        {
            Debug.Assert(strides.Length == indices.Length);

            int index = 0;
            for (int i = 0; i < indices.Length; i++)
            {
                index += strides[i] * indices[i];
            }

            return index;
        }
        public int GetLinearMemoryIndex(ReadOnlySpan<int> indices)
        {
            Debug.Assert(strides.Length == indices.Length);

            int index = offset;
            for (int i = 0; i < indices.Length; i++)
            {
                index += strides[i] * indices[i];
            }

            return index;
        }

        public void GetIndices(int linearIndex, Span<int> indices)
        {
            Debug.Assert(strides.Length == indices.Length);

            int remainder = linearIndex;
            for (int i = 0; i < strides.Length; i++)
            {
                // reverse the index for reverseStride so that we divide by largest stride first
                var index = isReversedStride ? strides.Length - 1 - i : i;

                var stride = strides[index];
                indices[index] = remainder / stride;
                remainder %= stride;
            }
        }

        public void GetIndicesFromMemoryIndex(int linearMemoryIndex, Span<int> indices)
        {
            Debug.Assert(strides.Length == indices.Length);
            Debug.Assert(linearMemoryIndex >= offset);

            int remainder = linearMemoryIndex - offset;
            for (int i = 0; i < strides.Length; i++)
            {
                // reverse the index for reverseStride so that we divide by largest stride first
                var index = isReversedStride ? strides.Length - 1 - i : i;

                var stride = strides[index];
                indices[index] = remainder / stride;
                remainder %= stride;
            }
        }

        public TensorShape Slice(params Range[] ranges)
        {
            return Slice(new ReadOnlySpan<Range>(ranges));
        }

        public TensorShape Slice(ReadOnlySpan<Range> ranges)
        {
            if (ranges.Length != Rank)
            {
                throw new ArgumentException($"{nameof(ranges.Length)} of {nameof(ranges)} ({ranges.Length}) must match the {nameof(Rank)} of this {nameof(TensorShape)} ({Rank}).");
            }

            Span<int> newDimensions = Rank < ArrayUtilities.StackallocMax ? stackalloc int[Rank] : new Span<int>(new int[Rank]);
            Span<int> newStrides = Rank < ArrayUtilities.StackallocMax ? stackalloc int[Rank] : new Span<int>(new int[Rank]);
            int offset = 0;

            // construct new dimensions and strides, compute offset as the linearization of the lower bounds
            for (int i = 0; i < ranges.Length; i++)
            {
                var range = ranges[i];
                newDimensions[i] = range.Length;
                offset += range.LowerBound * strides[i];
                newStrides[i] = strides[i];  // strides cannot change since the backing memory must stay the same.
            }

            return new TensorShape(newDimensions, newStrides, offset);
        }
    }
}
