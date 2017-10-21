// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace System.Numerics
{
    public static class ArrayTensorExtensions
    {
        /// <summary>
        /// Creates a copy of this single-dimensional array as a DenseTensor<T>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <param name="reverseStride"></param>
        /// <returns></returns>
        public static DenseTensor<T> ToTensor<T>(this T[] array, bool reverseStride = false)
        {
            return new DenseTensor<T>(array, reverseStride);
        }

        /// <summary>
        /// Creates a copy of this two-dimensional array as a DenseTensor<T>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <param name="reverseStride"></param>
        /// <returns></returns>
        public static DenseTensor<T> ToTensor<T>(this T[,] array, bool reverseStride = false)
        {
            return new DenseTensor<T>(array, reverseStride);
        }

        /// <summary>
        /// Creates a copy of this three-dimensional array as a DenseTensor<T>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <param name="reverseStride"></param>
        /// <returns></returns>
        public static DenseTensor<T> ToTensor<T>(this T[,,] array, bool reverseStride = false)
        {
            return new DenseTensor<T>(array, reverseStride);
        }

        /// <summary>
        /// Creates a copy of this n-dimensional array as a DenseTensor<T>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <param name="reverseStride"></param>
        /// <returns></returns>
        public static DenseTensor<T> ToTensor<T>(this Array array, bool reverseStride = false)
        {
            return new DenseTensor<T>(array, reverseStride);
        }

        /// <summary>
        /// Creates a copy of this single-dimensional array as a SparseTensor<T>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <param name="reverseStride"></param>
        /// <returns></returns>
        public static SparseTensor<T> ToSparseTensor<T>(this T[] array, bool reverseStride = false)
        {
            return new SparseTensor<T>(array, reverseStride);
        }

        /// <summary>
        /// Creates a copy of this two-dimensional array as a SparseTensor<T>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <param name="reverseStride"></param>
        /// <returns></returns>
        public static SparseTensor<T> ToSparseTensor<T>(this T[,] array, bool reverseStride = false)
        {
            return new SparseTensor<T>(array, reverseStride);
        }

        /// <summary>
        /// Creates a copy of this three-dimensional array as a SparseTensor<T>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <param name="reverseStride"></param>
        /// <returns></returns>
        public static SparseTensor<T> ToSparseTensor<T>(this T[,,] array, bool reverseStride = false)
        {
            return new SparseTensor<T>(array, reverseStride);
        }

        /// <summary>
        /// Creates a copy of this n-dimensional array as a SparseTensor<T>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <param name="reverseStride"></param>
        /// <returns></returns>
        public static SparseTensor<T> ToSparseTensor<T>(this Array array, bool reverseStride = false)
        {
            return new SparseTensor<T>(array, reverseStride);
        }

        /// <summary>
        /// Creates a copy of this single-dimensional array as a CompressedSparseTensor<T>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <param name="reverseStride"></param>
        /// <returns></returns>
        public static CompressedSparseTensor<T> ToCompressedSparseTensor<T>(this T[] array, bool reverseStride = false)
        {
            return new CompressedSparseTensor<T>(array, reverseStride);
        }

        /// <summary>
        /// Creates a copy of this two-dimensional array as a CompressedSparseTensor<T>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <param name="reverseStride"></param>
        /// <returns></returns>
        public static CompressedSparseTensor<T> ToCompressedSparseTensor<T>(this T[,] array, bool reverseStride = false)
        {
            return new CompressedSparseTensor<T>(array, reverseStride);
        }

        /// <summary>
        /// Creates a copy of this three-dimensional array as a CompressedSparseTensor<T>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <param name="reverseStride"></param>
        /// <returns></returns>
        public static CompressedSparseTensor<T> ToCompressedSparseTensor<T>(this T[,,] array, bool reverseStride = false)
        {
            return new CompressedSparseTensor<T>(array, reverseStride);
        }

        /// <summary>
        /// Creates a copy of this n-dimensional array as a CompressedSparseTensor<T>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <param name="reverseStride"></param>
        /// <returns></returns>
        public static CompressedSparseTensor<T> ToCompressedSparseTensor<T>(this Array array, bool reverseStride = false)
        {
            return new CompressedSparseTensor<T>(array, reverseStride);
        }

    }
}
