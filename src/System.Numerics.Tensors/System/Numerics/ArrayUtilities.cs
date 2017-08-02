using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace System.Numerics
{
    internal static class EmptyArray<T>
    {
        public static readonly T[] Value = new T[0];
    }

    internal static class ArrayUtilities
    {
        public static T[] Empty<T>()
        {
            return EmptyArray<T>.Value;
        }

        public static long GetProduct(int[] dimensions, int startIndex = 0)
        {
            if (dimensions.Length == 0)
            {
                return 0;
            }

            long product = 1;
            for (int i = startIndex; i < dimensions?.Length; i++)
            {
                if (dimensions[i] < 0)
                {
                    throw new ArgumentOutOfRangeException($"{nameof(dimensions)}[{i}]");
                }

                // we use a long which should be much larger than is ever used here,
                // but still force checked
                checked
                {
                    product *= dimensions[i];
                }
            }

            return product;
        }

        public static bool IsAscending(int[] values)
        {
            for (int i = 1; i < values.Length; i++)
            {
                if (values[i] < values[i - 1])
                {
                    return false;
                }
            }

            return true;
        }

        public static bool IsDescending(int[] values)
        {
            for (int i = 1; i < values.Length; i++)
            {
                if (values[i] > values[i - 1])
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Gets the set of strides that can be used to calculate the offset of n-dimensions in a 1-dimensional layout
        /// </summary>
        /// <param name="dimensions"></param>
        /// <returns></returns>
        public static int[] GetStrides(IReadOnlyList<int> dimensions)
        {
            int[] strides = new int[dimensions.Count];

            int stride = 1;
            for (int i = strides.Length - 1; i >= 0; i--)
            {
                strides[i] = stride;
                stride *= dimensions[i];
            }

            return strides;
        }

        public static void GetSplitStrides(IReadOnlyList<int> dimensions, int[] splitAxes, int[] strides, int stridesOffset, int[] splitStrides, int splitStridesOffset)
        {
            int stride = 1;

            int strideIndex = 0;
            for (int i = dimensions.Count - 1; i >= 0; i--)
            {
                bool isSplit = false;
                for (int j = 0; j < splitAxes.Length; j++)
                {
                    if (splitAxes[j] == i)
                    {
                        splitStrides[splitStridesOffset + j] = stride;
                        isSplit = true;
                        break;
                    }
                }

                if (!isSplit)
                {
                    strides[stridesOffset + strideIndex++] = stride;
                }

                stride *= dimensions[i];
            }
        }

        /// <summary>
        /// Calculates the 1-d index for n-d indices in layout specified by strides.
        /// </summary>
        /// <param name="strides"></param>
        /// <param name="indices"></param>
        /// <param name="startFromDimension"></param>
        /// <returns></returns>
        public static int GetIndex(int[] strides, int[] indices, int startFromDimension = 0)
        {
            Debug.Assert(strides.Length == indices.Length);

            int index = 0;
            for (int i = startFromDimension; i < indices.Length; i++)
            {
                index += strides[i] * indices[i];
            }

            return index;
        }

        /// <summary>
        /// Calculates the 1-d index for 2-d indices in layout specified by strides.
        /// </summary>
        /// <param name="strides"></param>
        /// <param name="index0"></param>
        /// <param name="index1"></param>
        /// <returns></returns>
        public static int GetIndex(int[] strides, int index0, int index1)
        {
            Debug.Assert(strides.Length == 2);
            return strides[0] * index0 + strides[1] * index1;
        }
        
        /// <summary>
        /// Calculates the n-d indices from the 1-d index in a layoyut specificed by strides
        /// </summary>
        /// <param name="strides"></param>
        /// <param name="index"></param>
        /// <param name="indices"></param>
        /// <param name="startFromDimension"></param>
        public static void GetIndices(int[] strides, int index, int[] indices, int startFromDimension = 0)
        {
            Debug.Assert(IsDescending(strides), "Index decomposition requires ordered strides");
            Debug.Assert(strides.Length == indices.Length);

            int remainder = index;
            for (int i = startFromDimension; i < strides.Length; i++)
            {
                var stride = strides[i];
                indices[i] = remainder / stride;
                remainder %= stride;
            }
        }

        /// <summary>
        /// Takes an 1-d index over n-d sourceStrides and recalculates it assuming same n-d coordinates over a different n-d strides
        /// </summary>
        public static int TransformIndexByStrides(int index, int[] sourceStrides, int[] transformStrides)
        {
            Debug.Assert(index >= 0);
            Debug.Assert(IsDescending(sourceStrides), "Index decomposition requires ordered strides");
            Debug.Assert(sourceStrides.Length == transformStrides.Length);

            int transformIndex = 0;
            int remainder = index;

            for (int i = 0; i < sourceStrides.Length; i++)
            {
                var sourceStride = sourceStrides[i];
                var transformStride = transformStrides[i];

                transformIndex += transformStride * (remainder / sourceStride);
                remainder %= sourceStride;
            }

            return transformIndex;
        }
    }
}
