// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace System.Numerics
{
    public static class ArrayTensorExtensions
    {
        public static DenseTensor<T> ToTensor<T>(this T[] array, bool reverseStride = false)
        {
            return new DenseTensor<T>(array, reverseStride);
        }

        public static DenseTensor<T> ToTensor<T>(this T[,] array, bool reverseStride = false)
        {
            return new DenseTensor<T>(array, reverseStride);
        }

        public static DenseTensor<T> ToTensor<T>(this T[,,] array, bool reverseStride = false)
        {
            return new DenseTensor<T>(array, reverseStride);
        }

        public static DenseTensor<T> ToTensor<T>(this Array array, bool reverseStride = false)
        {
            return new DenseTensor<T>(array, reverseStride);
        }

        public static SparseTensor<T> ToSparseTensor<T>(this T[] array, bool reverseStride = false)
        {
            return new SparseTensor<T>(array, reverseStride);
        }

        public static SparseTensor<T> ToSparseTensor<T>(this T[,] array, bool reverseStride = false)
        {
            return new SparseTensor<T>(array, reverseStride);
        }

        public static SparseTensor<T> ToSparseTensor<T>(this T[,,] array, bool reverseStride = false)
        {
            return new SparseTensor<T>(array, reverseStride);
        }

        public static SparseTensor<T> ToSparseTensor<T>(this Array array, bool reverseStride = false)
        {
            return new SparseTensor<T>(array, reverseStride);
        }

        public static CompressedSparseTensor<T> ToCompressedSparseTensor<T>(this T[] array, bool reverseStride = false)
        {
            return new CompressedSparseTensor<T>(array, reverseStride);
        }

        public static CompressedSparseTensor<T> ToCompressedSparseTensor<T>(this T[,] array, bool reverseStride = false)
        {
            return new CompressedSparseTensor<T>(array, reverseStride);
        }

        public static CompressedSparseTensor<T> ToCompressedSparseTensor<T>(this T[,,] array, bool reverseStride = false)
        {
            return new CompressedSparseTensor<T>(array, reverseStride);
        }

        public static CompressedSparseTensor<T> ToCompressedSparseTensor<T>(this Array array, bool reverseStride = false)
        {
            return new CompressedSparseTensor<T>(array, reverseStride);
        }

    }
}
