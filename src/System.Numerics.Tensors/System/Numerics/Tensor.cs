﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace System.Numerics
{

    public static partial class Tensor
    {
        internal static void ValidateBinaryArgs<T>(Tensor<T> left, Tensor<T> right)
        {
            if (left.Rank != right.Rank || left.Length != right.Length)
            {
                throw new ArgumentException("Operands must have matching dimensions", nameof(right));
            }

            if (left.Rank == 0)
            {
                throw new ArgumentException($"Cannot operate on Tensor with {nameof(Tensor<T>.Rank)} of 0.", nameof(left));
            }

            for (int i = 0; i < left.Rank; i++)
            {
                if (left.dimensions[i] != right.dimensions[i])
                {
                    throw new ArgumentException("Operands must have matching dimensions", nameof(right));
                }
            }
        }

        internal static void ValidateBinaryArgs<T>(Tensor<T> left, Tensor<T> right, Tensor<T> result)
        {
            if (left.Rank != right.Rank || left.Length != right.Length)
            {
                throw new ArgumentException("Operands must have matching dimensions", nameof(right));
            }

            if (left.Rank != result.Rank || left.Length != result.Length)
            {
                throw new ArgumentException("Operands must have matching dimensions", nameof(result));
            }

            if (left.Rank == 0)
            {
                throw new ArgumentException($"Cannot operate on Tensor with {nameof(Tensor<T>.Rank)} of 0.", nameof(left));
            }

            for (int i = 0; i < result.Rank; i++)
            {
                if (left.dimensions[i] != right.dimensions[i])
                {
                    throw new ArgumentException("Operands must have matching dimensions", nameof(right));
                }

                if (left.dimensions[i] != result.dimensions[i])
                {
                    throw new ArgumentException("Operands and result must have matching dimensions", nameof(result));
                }
            }
        }

        internal static void ValidateBinaryArgs<T>(Tensor<T> left, Tensor<T> right, Tensor<bool> result)
        {
            if (left.Rank != right.Rank || left.Length != right.Length)
            {
                throw new ArgumentException("Operands must have matching dimensions", nameof(right));
            }

            if (left.Rank != result.Rank || left.Length != result.Length)
            {
                throw new ArgumentException("Operands must have matching dimensions", nameof(result));
            }

            if (left.Rank == 0)
            {
                throw new ArgumentException($"Cannot operate on Tensor with {nameof(Tensor<T>.Rank)} of 0.", nameof(left));
            }

            for (int i = 0; i < result.Rank; i++)
            {
                if (left.dimensions[i] != right.dimensions[i])
                {
                    throw new ArgumentException("Operands must have matching dimensions", nameof(right));
                }

                if (left.dimensions[i] != result.dimensions[i])
                {
                    throw new ArgumentException("Operands and result must have matching dimensions", nameof(result));
                }
            }
        }

        internal static void ValidateArgs<T>(Tensor<T> tensor)
        {
            if (tensor.Rank == 0)
            {
                throw new ArgumentException($"Cannot operate on Tensor with {nameof(Tensor<T>.Rank)} of 0.", nameof(tensor));
            }
        }

        internal static void ValidateArgs<T>(Tensor<T> tensor, Tensor<T> result)
        {
            if (tensor.Rank != result.Rank || tensor.Length != result.Length)
            {
                throw new ArgumentException("Operands and result must have matching dimensions", nameof(result));
            }

            if (tensor.Rank == 0)
            {
                throw new ArgumentException($"Cannot operate on Tensor with {nameof(Tensor<T>.Rank)} of 0.", nameof(tensor));
            }

            for (int i = 0; i < result.Rank; i++)
            {
                if (tensor.dimensions[i] != result.dimensions[i])
                {
                    throw new ArgumentException("Operands and result must have matching dimensions", nameof(result));
                }
            }
        }

        internal static int[] ValidateContractArgs<T>(Tensor<T> left, Tensor<T> right, int[] leftAxes, int[] rightAxes)
        {
            if (leftAxes == null)
            {
                throw new ArgumentNullException(nameof(left));
            }

            if (rightAxes == null)
            {
                throw new ArgumentNullException(nameof(left));
            }

            if (leftAxes.Length != rightAxes.Length)
            {
                throw new ArgumentException($"{nameof(leftAxes)} and {nameof(rightAxes)} must have the same length, but were {leftAxes.Length} and {rightAxes.Length}, respectively.");
            }

            for(int i = 0; i < leftAxes.Length; i++)
            {
                var leftAxis = leftAxes[i];

                if (leftAxis >= left.Rank)
                {
                    throw new ArgumentOutOfRangeException($"{nameof(leftAxes)}[{i}] was set to axis index {leftAxis} which exceeds the Rank of {left}.");
                }

                var leftDimension = left.dimensions[leftAxis];

                var rightAxis = rightAxes[i];

                if (rightAxis >= right.Rank)
                {
                    throw new ArgumentOutOfRangeException($"{nameof(rightAxes)}[{i}] was set to axis index {rightAxis} which exceeds the Rank of {right}.");
                }

                var rightDimension = right.dimensions[rightAxis];

                if (leftDimension != rightDimension)
                {
                    throw new ArgumentOutOfRangeException($"Tensors may only be contracted on axes of the same length, but {nameof(leftAxes)} index {i} was length {leftDimension} and {nameof(rightAxes)} index {i} was length {rightDimension}.");
                }
            }

            var leftNonSummingDimensions = left.Rank - leftAxes.Length;
            var rightNonSummingDimensions = right.Rank - rightAxes.Length;
            var resultDimensions = new int[leftNonSummingDimensions + rightNonSummingDimensions];
            int dimensionsIndex = 0;

            Action<Tensor<T>, int[]> fillDimensions = (tensor, axes) =>
            {
                for (int i = 0; i < tensor.Rank; i++)
                {
                    var skip = false;
                    foreach (var contractionIndex in axes)
                    {
                        if (contractionIndex == i)
                        {
                            skip = true;
                            break;
                        }
                    }

                    if (!skip)
                    {
                        resultDimensions[dimensionsIndex++] = tensor.dimensions[i];
                    }
                }
            };

            fillDimensions(left, leftAxes);
            fillDimensions(right, rightAxes);

            return resultDimensions;
        }

        internal static int[] ValidateContractArgs<T>(Tensor<T> left, Tensor<T> right, int[] leftAxes, int[] rightAxes, Tensor<T> result)
        {
            var expectedDimensions = ValidateContractArgs(left, right, leftAxes, rightAxes);

            if (result.Rank != expectedDimensions.Length)
            {
                throw new ArgumentException($"{nameof(result)} should have {expectedDimensions.Length} dimensions but had {result.Rank}.");
            }

            for(int i = 0; i < expectedDimensions.Length; i++)
            {
                if (result.dimensions[i] != expectedDimensions[i])
                {
                    throw new ArgumentException($"{nameof(result)} dimension {i} should be {expectedDimensions[i]} but was {result.dimensions[i]}.");
                }
            }

            return expectedDimensions;
        }
    }

    // When we cross-compile for frameworks that expose ICloneable this must implement ICloneable as well.
    public struct Tensor<T> : IList, ICollection, IEnumerable, IStructuralComparable, IStructuralEquatable
    {
        private readonly T[] backingArray;
        internal readonly int[] dimensions;
        private readonly int[] strides;
        private IReadOnlyList<int> readOnlyDimensions;

        private static ITensorArithmetic<T> arithmetic => TensorArithmetic.GetArithmetic<T>();

        public Tensor(Array fromArray)
        {
            if (fromArray == null)
            {
                throw new ArgumentNullException(nameof(fromArray));
            }

            // copy initial array
            dimensions = new int[fromArray.Rank];
            for (int i = 0; i < dimensions.Length; i++)
            {
                dimensions[i] = fromArray.GetLength(i);
            }

            strides = ArrayUtilities.GetStrides(dimensions);

            backingArray = new T[fromArray.Length];

            int index = 0;
            foreach (var item in fromArray)
            {
                backingArray[index++] = (T)item;
            }

            readOnlyDimensions = null;
        }

        /// <summary>
        /// Initializes a rank-1 Tensor using the specified <paramref name="size"/>.
        /// </summary>
        /// <param name="size">Size of the tensor</param>
        public Tensor(int size)
        {
            if (size < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(size));
            }

            backingArray = new T[size];
            dimensions = new[] { size };
            strides = ArrayUtilities.GetStrides(dimensions);
            readOnlyDimensions = null;
        }

        /// <summary>
        /// Initializes a rank-n Tensor using the dimensions specified in <paramref name="dimensions"/>.
        /// </summary>
        /// <param name="dimensions"></param>
        public Tensor(params int[] dimensions)
        {
            if (dimensions == null)
            {
                throw new ArgumentNullException(nameof(dimensions));
            }

            if (dimensions.Length == 0)
            {
                throw new ArgumentException("Dimensions must contain elements.", nameof(dimensions));
            }

            // make a private copy of mutable dimensions array
            this.dimensions = new int[dimensions.Length];
            long size = 1;
            for(int i = 0; i < dimensions.Length; i++)
            {
                if (dimensions[i] < 1)
                {
                    throw new ArgumentOutOfRangeException(nameof(dimensions), "Dimensions must be positive and non-zero");
                }
                this.dimensions[i] = dimensions[i];
                size *= dimensions[i];
            }
            strides = ArrayUtilities.GetStrides(this.dimensions);
            readOnlyDimensions = null;

            // could throw, let the runtime decide what that limit is
            backingArray = new T[size];
        }

        public Tensor(T[] fromBackingArray, params int[] dimensions)
        {
            if (fromBackingArray == null)
            {
                throw new ArgumentNullException(nameof(fromBackingArray));
            }

            if (dimensions == null)
            {
                throw new ArgumentNullException(nameof(dimensions));
            }

            if (dimensions.Length == 0)
            {
                throw new ArgumentException("Dimensions must contain elements.", nameof(dimensions));
            }

            // keep a reference to the backing array
            backingArray = fromBackingArray;

            // make a private copy of mutable dimensions array
            this.dimensions = new int[dimensions.Length];
            long size = 1;
            for (int i = 0; i < dimensions.Length; i++)
            {
                if (dimensions[i] < 1)
                {
                    throw new ArgumentOutOfRangeException(nameof(dimensions), "Dimensions must be positive and non-zero");
                }
                this.dimensions[i] = dimensions[i];
                size *= dimensions[i];
            }

            if (size != fromBackingArray.Length)
            {
                throw new ArgumentException($"Length of {nameof(fromBackingArray)} ({fromBackingArray.Length}) must match product of {nameof(dimensions)} ({size}).");
            }

            strides = ArrayUtilities.GetStrides(this.dimensions);
            readOnlyDimensions = null;
        }

        /// <summary>
        /// Total length of the Tensor.
        /// </summary>
        public int Length => backingArray?.Length ?? 0;

        /// <summary>
        /// Rank of the tensor: number of dimensions.
        /// </summary>
        public int Rank => dimensions?.Length ?? 0;

        /// <summary>
        /// Returns a copy of the dimensions array.
        /// </summary>
        public IReadOnlyList<int> Dimensions
        {
            get
            {
                if (dimensions == null)
                {
                    // make sure we don't mutate a default(Tensor<T>) object by accessing this property
                    return ArrayUtilities.Empty<int>();
                }

                return readOnlyDimensions ?? (readOnlyDimensions = new ReadOnlyCollection<int>(dimensions));
            }
        }

        /// <summary>
        /// Returns a single dimensional view of this Tensor, in C-style ordering
        /// </summary>
        public T[] Buffer => backingArray ?? ArrayUtilities.Empty<T>();

        /// <summary>
        /// Sets all elements in Tensor to <paramref name="value"/>.
        /// </summary>
        /// <param name="value">Value to fill</param>
        public void Fill(T value)
        {
            if (backingArray != null)
            {
                // JIT look here, lend us a hand
                for (int i = 0; i < backingArray.Length; i++)
                {
                    // is it possible to fast-path when initialValue == default(T)?
                    backingArray[i] = value;
                }
            }
        }

        public Tensor<T> Clone()
        {
            if (dimensions == null)
            {
                return default(Tensor<T>);
            }
            return new Tensor<T>((T[])Buffer.Clone(), dimensions);
        }

        /// <summary>
        /// Creates a new Tensor with the same size as this tensor with elements initialized to their default value.
        /// </summary>
        /// <returns></returns>
        public Tensor<T> CloneEmpty()
        {
            if (dimensions == null)
            {
                return default(Tensor<T>);
            }
            return new Tensor<T>(dimensions);
        }


        /// <summary>
        /// Creates a new Tensor of a different type with the same size as this tensor with elements initialized to their default value.
        /// </summary>
        /// <returns></returns>
        public Tensor<TResult> CloneEmpty<TResult>()
        {
            if (dimensions == null)
            {
                return default(Tensor<TResult>);
            }
            return new Tensor<TResult>(dimensions);
        }

        /// <summary>
        /// Creates an identity tensor
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        public static Tensor<T> CreateIdentity(int size)
        {
            return CreateIdentity(size, arithmetic.One);
        }

        /// <summary>
        /// Creates an identity tensor
        /// </summary>
        /// <param name="size"></param>
        /// <param name="oneValue"></param>
        /// <returns></returns>
        public static Tensor<T> CreateIdentity(int size, T oneValue)
        {
            var result = new Tensor<T>(size, size);

            for(int i = 0; i < size; i++)
            {
                result.Buffer[i * size + i] = oneValue;
            }

            return result;
        }

        public static Tensor<T> CreateFromDiagonal(Tensor<T> diagonal)
        {
            return CreateFromDiagonal(diagonal, 0);
        }

        public static Tensor<T> CreateFromDiagonal(Tensor<T> diagonal, int offset)
        {
            if (diagonal.Rank < 1)
            {
                throw new ArgumentException($"Tensor {nameof(diagonal)} must have at least one dimension.", nameof(diagonal));
            }

            int diagonalLength = diagonal.dimensions[0];

            // TODO: allow specification of axis1 and axis2?
            var dimensions = new int[diagonal.dimensions.Length + 1];

            // assume square
            var axisLength = diagonalLength + Math.Abs(offset);
            dimensions[0] = dimensions[1] = axisLength;
            
            for(int i = 1; i < diagonal.dimensions.Length; i++)
            {
                dimensions[i + 1] = diagonal.dimensions[i];
            }

            var result = new Tensor<T>(dimensions);

            // each element in the diagonal's 0 dimension is strides[0] appart
            var sizePerDiagonal = diagonal.strides[0];

            for (int diagIndex = 0; diagIndex < diagonalLength; diagIndex++)
            {
                var destIndex0 = offset < 0 ? diagIndex - offset : diagIndex;
                var destIndex1 = offset > 0 ? diagIndex + offset : diagIndex;

                var destBuffIndex = destIndex0 * result.strides[0] + destIndex1 * result.strides[1];
                var diagBuffIndex = diagIndex * diagonal.strides[0];

                for (int diagProjectionOffset = 0; diagProjectionOffset < sizePerDiagonal; diagProjectionOffset++)
                {
                    // since result and diagonal have the same strides for remaining dimensions we can directly sum the offset
                    result.Buffer[destBuffIndex + diagProjectionOffset] = diagonal.Buffer[diagBuffIndex + diagProjectionOffset];
                }
            }

            return result;
        }

        public Tensor<T> GetDiagonal()
        {
            return GetDiagonal(0);
        }

        public Tensor<T> GetDiagonal(int offset)
        {
            // Get diagonal of first two dimensions for all remaining dimensions

            // diagnonal is as follows:
            // { 1, 2, 4 }
            // { 8, 3, 9 }
            // { 0, 7, 5 }
            // The diagonal at offset 0 is { 1, 3, 5 }
            // The diagonal at offset 1 is { 2, 9 }
            // The diagonal at offset -1 is { 8, 7 }

            if (Rank < 2)
            {
                throw new InvalidOperationException($"Cannot compute diagonal of {nameof(Tensor<T>)} with Rank less than 2.");
            }

            // TODO: allow specification of axis1 and axis2?
            var axisLength0 = dimensions[0];
            var axisLength1 = dimensions[1];

            // the diagonal will be the length of the smaller axis
            // if offset it positive, the length will shift along the second axis
            // if the offsett is negative, the length will shift along the first axis
            // In that way the length of the diagonal will be 
            //   Min(offset < 0 ? axisLength0 + offset : axisLength0, offset > 0 ? axisLength1 - offset : axisLength1)
            // To illustrate, consider the following
            // { 1, 2, 4, 3, 7 }
            // { 8, 3, 9, 2, 6 }
            // { 0, 7, 5, 2, 9 }
            // The diagonal at offset 0 is { 1, 3, 5 }, Min(3, 5) = 3
            // The diagonal at offset 1 is { 2, 9, 2 }, Min(3, 5 - 1) = 3
            // The diagonal at offset 3 is { 3, 6 }, Min(3, 5 - 3) = 2
            // The diagonal at offset -1 is { 8, 7 }, Min(3 - 1, 5) = 2
            var offsetAxisLength0 = offset < 0 ? axisLength0 + offset : axisLength0;
            var offsetAxisLength1 = offset > 0 ? axisLength1 - offset : axisLength1;

            var diagonalLength = Math.Min(offsetAxisLength0, offsetAxisLength1);

            if (diagonalLength <= 0)
            {
                throw new ArgumentException($"Cannot compute diagonal with offset {offset}", nameof(offset));
            }

            var newTensorDimensions = new int[dimensions.Length - 1];
            newTensorDimensions[0] = diagonalLength;

            for(int i = 2; i < dimensions.Length; i++)
            {
                newTensorDimensions[i - 1] = dimensions[i];
            }

            var diagonalTensor = new Tensor<T>(dimensions:newTensorDimensions);
            var sizePerDiagonal = diagonalTensor.strides[0];

            for (int diagIndex = 0; diagIndex < diagonalLength; diagIndex++)
            {
                var sourceIndex0 = offset < 0 ? diagIndex - offset : diagIndex;
                var sourceIndex1 = offset > 0 ? diagIndex + offset : diagIndex;

                var sourceBuffIndex = sourceIndex0 * strides[0] + sourceIndex1 * strides[1];
                var diagBuffIndex = diagIndex * diagonalTensor.strides[0];

                for(int diagProjectionOffset = 0; diagProjectionOffset < sizePerDiagonal; diagProjectionOffset++)
                {
                    // since the source and diagonal have the same strides for remaining dimensions we can directly sum the offset
                    diagonalTensor.Buffer[diagBuffIndex + diagProjectionOffset] = Buffer[sourceBuffIndex + diagProjectionOffset];
                }
            }

            return diagonalTensor;
        }


        public Tensor<T> GetTriangle()
        {
            return GetTriangle(0, upper: false);
        }

        public Tensor<T> GetTriangle(int offset)
        {
            return GetTriangle(offset, upper: false);
        }

        public Tensor<T> GetUpperTriangle()
        {
            return GetTriangle(0, upper: true);
        }

        public Tensor<T> GetUpperTriangle(int offset)
        {
            return GetTriangle(offset, upper: true);
        }

        private Tensor<T> GetTriangle(int offset, bool upper)
        {
            if (Rank < 2)
            {
                throw new InvalidOperationException($"Cannot compute triangle of {nameof(Tensor<T>)} with Rank less than 2.");
            }

            // Similar to get diagonal except it gets every element below and including the diagonal.

            // TODO: allow specification of axis1 and axis2?
            var axisLength0 = dimensions[0];
            var axisLength1 = dimensions[1];
            var diagonalLength = Math.Max(axisLength0, axisLength1);

            var result = CloneEmpty();

            var projectionSize = strides[1];

            for (int diagIndex = 0; diagIndex < diagonalLength; diagIndex++)
            {
                // starting point for the tri
                var triIndex0 = offset > 0 ? diagIndex - offset: diagIndex;
                var triIndex1 = offset > 0 ? diagIndex : diagIndex + offset;

                // for lower triangle, iterate index0 keeping same index1
                // for upper triangle, iterate index1 keeping same index0

                if (triIndex0 < 0)
                {
                    if (upper)
                    {
                        // out of bounds, ignore this diagIndex.
                        continue;
                    }
                    else
                    {
                        // set index to 0 so that we can iterate on the remaining index0 values.
                        triIndex0 = 0;
                    }
                }

                if (triIndex1 < 0)
                {
                    if (upper)
                    {
                        // set index to 0 so that we can iterate on the remaining index1 values.
                        triIndex1 = 0;
                    }
                    else
                    {
                        // out of bounds, ignore this diagIndex.
                        continue;
                    }
                }

                while ((triIndex1 < axisLength1) && (triIndex0 < axisLength0))
                {
                    var buffIndex = triIndex0 * strides[0] + triIndex1 * strides[1];

                    for (int projectionOffset = 0; projectionOffset < projectionSize; projectionOffset++)
                    {
                        result.Buffer[buffIndex + projectionOffset] = Buffer[buffIndex + projectionOffset];
                    }

                    if (upper)
                    {
                        triIndex1++;
                    }
                    else
                    {
                        triIndex0++;
                    }
                }
            }

            return result;
        }

        static int[] s_zeroArray = new[] { 0 };
        static int[] s_oneArray = new[] { 1 };
        public Tensor<T> MatrixMultiply(Tensor<T> right)
        {
            if (Rank != 2)
            {
                throw new InvalidOperationException($"{nameof(MatrixMultiply)} is only valid for a {nameof(Tensor<T>)} of {nameof(Rank)} 2.");
            }

            if (right.Rank != 2)
            {
                throw new ArgumentException($"{nameof(Tensor<T>)} {nameof(right)} must have {nameof(Rank)} 2.", nameof(right));
            }

            if (dimensions[1] != right.dimensions[0])
            {
                throw new ArgumentException($"{nameof(Tensor<T>)} {nameof(right)} must have first dimension of {dimensions[1]}.", nameof(right));
            }

            return Tensor.Contract(this, right, s_oneArray, s_zeroArray);
        }

        public Tensor<T> Reshape(params int[] dimensions)
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

            return new Tensor<T>(Buffer, dimensions);
        }

        public Tensor<T> ReshapeCopy(params int[] dimensions)
        {
            if (dimensions == null)
            {
                throw new ArgumentNullException(nameof(dimensions));
            }

            if (dimensions.Length == 0)
            {
                throw new ArgumentException("Dimensions must contain elements.", nameof(dimensions));
            }

            var newSize = (int)ArrayUtilities.GetProduct(dimensions);

            T[] copyBackingArray = new T[newSize];
            var copyLength = Math.Min(newSize, Length);

            if (copyLength != 0)
            {
                Array.Copy(Buffer, copyBackingArray, copyLength);
            }

            return new Tensor<T>(copyBackingArray, dimensions);
        }

        public T this[int index]
        {
            get
            {
                if (Rank != 1)
                {
                    throw new ArgumentOutOfRangeException($"Cannot use single dimension indexer on {nameof(Tensor<T>)} with {Rank} dimensions.");
                }

                if (index < 0 || index > Length)
                {
                    throw new ArgumentOutOfRangeException(nameof(index));
                }

                return Buffer[index];
            }
            set
            {
                if (Rank != 1)
                {
                    throw new ArgumentOutOfRangeException($"Cannot use single dimension indexer on {nameof(Tensor<T>)} with {Rank} dimensions.");
                }

                if (index < 0 || index > Length)
                {
                    throw new ArgumentOutOfRangeException(nameof(index));
                }

                Buffer[index] = value;
            }
        }

        public T this[int indexRow, int indexColumn]
        {
            get
            {
                var index = GetOffset(indexRow, indexColumn);
                return Buffer[index];
            }
            set
            {

                var index = GetOffset(indexRow, indexColumn);
                Buffer[index] = value;
            }
        }
        
        public T this[params int[] indices]
        {
            get
            {
                var index = GetOffsetFromIndices(indices);
                return Buffer[index];
            }

            set
            {
                var index = GetOffsetFromIndices(indices);
                Buffer[index] = value;
            }
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

            Debug.Assert(strides[1] == 1);
            return index0 * strides[0] + index1;
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
        #region statics

        public static int Compare(Tensor<T> left, Tensor<T> right)
        {
            return StructuralComparisons.StructuralComparer.Compare(left, right);
        }

        public static bool Equals(Tensor<T> left, Tensor<T> right)
        {
            return StructuralComparisons.StructuralEqualityComparer.Equals(left, right);
        }
        #endregion

        #region Operators
        public static Tensor<T> operator +(Tensor<T> left, Tensor<T> right)
        {
            return Tensor.Add(left, right);
        }

        public static Tensor<T> operator +(Tensor<T> tensor, T scalar)
        {
            return Tensor.Add(tensor, scalar);
        }

        public static Tensor<T> operator +(Tensor<T> tensor)
        {
            return Tensor.UnaryPlus(tensor);
        }
        public static Tensor<T> operator -(Tensor<T> left, Tensor<T> right)
        {
            return Tensor.Subtract(left, right);
        }

        public static Tensor<T> operator -(Tensor<T> tensor, T scalar)
        {
            return Tensor.Subtract(tensor, scalar);
        }

        public static Tensor<T> operator -(Tensor<T> tensor)
        {
            return Tensor.UnaryMinus(tensor);
        }

        public static Tensor<T> operator ++(Tensor<T> tensor)
        {
            return Tensor.Increment(tensor);
        }

        public static Tensor<T> operator --(Tensor<T> tensor)
        {
            return Tensor.Decrement(tensor);
        }

        public static Tensor<T> operator *(Tensor<T> left, Tensor<T> right)
        {
            return Tensor.Multiply(left, right);
        }

        public static Tensor<T> operator *(Tensor<T> tensor, T scalar)
        {
            return Tensor.Multiply(tensor, scalar);
        }

        public static Tensor<T> operator /(Tensor<T> left, Tensor<T> right)
        {
            return Tensor.Divide(left, right);
        }

        public static Tensor<T> operator /(Tensor<T> tensor, T scalar)
        {
            return Tensor.Divide(tensor, scalar);
        }

        public static Tensor<T> operator %(Tensor<T> left, Tensor<T> right)
        {
            return Tensor.Modulo(left, right);
        }

        public static Tensor<T> operator %(Tensor<T> tensor, T scalar)
        {
            return Tensor.Modulo(tensor, scalar);
        }

        public static Tensor<T> operator &(Tensor<T> left, Tensor<T> right)
        {
            return Tensor.And(left, right);
        }

        public static Tensor<T> operator &(Tensor<T> tensor, T scalar)
        {
            return Tensor.And(tensor, scalar);
        }

        public static Tensor<T> operator |(Tensor<T> left, Tensor<T> right)
        {
            return Tensor.Or(left, right);
        }

        public static Tensor<T> operator |(Tensor<T> tensor, T scalar)
        {
            return Tensor.Or(tensor, scalar);
        }

        public static Tensor<T> operator ^(Tensor<T> left, Tensor<T> right)
        {
            return Tensor.Xor(left, right);
        }

        public static Tensor<T> operator ^(Tensor<T> tensor, T scalar)
        {
            return Tensor.Xor(tensor, scalar);
        }

        public static Tensor<T> operator <<(Tensor<T> tensor, int value)
        {
            return Tensor.LeftShift(tensor, value);
        }

        public static Tensor<T> operator >>(Tensor<T> tensor, int value)
        {
            return Tensor.RightShift(tensor, value);
        }
        #endregion

        #region IEnumerable members
        public IEnumerator GetEnumerator()
        {
            return Buffer.GetEnumerator();
        }
        #endregion

        #region ICollection members
        int ICollection.Count => Buffer.Length;

        bool ICollection.IsSynchronized => false;

        object ICollection.SyncRoot => this; // backingArray.this?

        public void CopyTo(Array array, int index)
        {
            Buffer.CopyTo(array, index);
        }
        #endregion

        #region IList members
        object IList.this[int index]
        {
            get
            {
                return this[index];
            }
            set
            {
                try
                {
                    this[index] = (T)value;
                }
                catch (InvalidCastException)
                {
                    throw new ArgumentException($"The value \"{value}\" is not of type \"{typeof(T)}\" and cannot be used in this generic collection.");
                }
            }
        }

        public bool IsFixedSize => true;

        public bool IsReadOnly => false;

        int IList.Add(object value)
        {
            // will throw
            return ((IList)Buffer).Add(value);
        }

        void IList.Clear()
        {
            ((IList)Buffer).Clear();
        }

        bool IList.Contains(object value)
        {
            return ((IList)Buffer).Contains(value);
        }

        int IList.IndexOf(object value)
        {
            return ((IList)Buffer).IndexOf(value);
        }

        void IList.Insert(int index, object value)
        {
            // will throw
            ((IList)Buffer).Insert(index, value);
        }

        void IList.Remove(object value)
        {
            // will throw
            ((IList)Buffer).Remove(value);
        }

        void IList.RemoveAt(int index)
        {
            // will throw
            ((IList)Buffer).RemoveAt(index);
        }
        #endregion

        #region IStructuralComparable members
        int IStructuralComparable.CompareTo(object other, IComparer comparer)
        {
            if (other == null)
            {
                return 1;
            }

            if (other is Tensor<T>)
            {
                return CompareTo((Tensor<T>)other, comparer);
            }

            var otherArray = other as Array;

            if (otherArray != null)
            {
                return CompareTo(otherArray, comparer);
            }

            throw new ArgumentException($"Cannot compare {nameof(Tensor<T>)} to {other.GetType()}.", nameof(other));
        }

        private int CompareTo(Tensor<T> other, IComparer comparer)
        {
            if (Rank != other.Rank)
            {
                throw new ArgumentException($"Cannot compare {nameof(Tensor<T>)} with Rank {Rank} to {nameof(other)} with Rank {other.Rank}.", nameof(other));
            }

            for (int i = 0; i < dimensions.Length; i++)
            {
                if (dimensions[i] != other.dimensions[i])
                {
                    throw new ArgumentException($"Cannot compare {nameof(Tensor<T>)}s with differning dimension {i}, {dimensions[i]} != {other.dimensions[i]}.", nameof(other));
                }
            }

            int result = 0;
            // for tensors we can just rip through the backing array
            for (int i = 0; i < backingArray.Length; i++)
            {
                result = comparer.Compare(backingArray[i], other.backingArray[i]);

                if (result != 0)
                {
                    break;
                }
            }

            return result;
        }

        private int CompareTo(Array other, IComparer comparer)
        {
            if (Rank != other.Rank)
            {
                throw new ArgumentException($"Cannot compare {nameof(Tensor<T>)} with Rank {Rank} to {nameof(Array)} with rank {other.Rank}.", nameof(other));
            }

            for (int i = 0; i < dimensions.Length; i++)
            {
                var otherDimension = other.GetLength(i);
                if (dimensions[i] != otherDimension)
                {
                    throw new ArgumentException($"Cannot compare {nameof(Tensor<T>)} to {nameof(Array)} with differning dimension {i}, {dimensions[i]} != {otherDimension}.", nameof(other));
                }
            }

            int result = 0;
            var indices = new int[Rank];  // consider stackalloc
            for (int i = 0; i < backingArray.Length; i++)
            {
                var left = backingArray[i];
                ArrayUtilities.GetIndices(strides, i, indices);
                var right = other.GetValue(indices);

                result = comparer.Compare(left, right);

                if (result != 0)
                {
                    break;
                }
            }

            return result;
        }
        #endregion

        #region IStructuralEquatable members
        bool IStructuralEquatable.Equals(object other, IEqualityComparer comparer)
        {
            if (other == null)
            {
                return false;
            }

            if (other is Tensor<T>)
            {
                return Equals((Tensor<T>)other, comparer);
            }

            var otherArray = other as Array;

            if (otherArray != null)
            {
                return Equals(otherArray, comparer);
            }

            throw new ArgumentException($"Cannot compare {nameof(Tensor<T>)} to {other.GetType()}.", nameof(other));
        }

        private bool Equals(Tensor<T> other, IEqualityComparer comparer)
        {
            if (Rank != other.Rank)
            {
                throw new ArgumentException($"Cannot compare {nameof(Tensor<T>)} with Rank {Rank} to {nameof(other)} with Rank {other.Rank}.", nameof(other));
            }

            for (int i = 0; i < dimensions.Length; i++)
            {
                if (dimensions[i] != other.dimensions[i])
                {
                    throw new ArgumentException($"Cannot compare {nameof(Tensor<T>)}s with differning dimension {i}, {dimensions[i]} != {other.dimensions[i]}.", nameof(other));
                }
            }

            // for tensors we can just rip through the backing array
            for (int i = 0; i < backingArray.Length; i++)
            {
                if (!comparer.Equals(backingArray[i], other.backingArray[i]))
                {
                    return false;
                }
            }

            return true;
        }

        private bool Equals(Array other, IEqualityComparer comparer)
        {
            if (Rank != other.Rank)
            {
                throw new ArgumentException($"Cannot compare {nameof(Tensor<T>)} with Rank {Rank} to {nameof(Array)} with rank {other.Rank}.", nameof(other));
            }

            for (int i = 0; i < dimensions.Length; i++)
            {
                var otherDimension = other.GetLength(i);
                if (dimensions[i] != otherDimension)
                {
                    throw new ArgumentException($"Cannot compare {nameof(Tensor<T>)} to {nameof(Array)} with differning dimension {i}, {dimensions[i]} != {otherDimension}.", nameof(other));
                }
            }
            
            var indices = new int[Rank];  // consider stackalloc
            for (int i = 0; i < backingArray.Length; i++)
            {
                var left = backingArray[i];
                ArrayUtilities.GetIndices(strides, i, indices);
                var right = other.GetValue(indices);

                if (!comparer.Equals(left, right))
                {
                    return false;
                }
            }

            return true;
        }
        int IStructuralEquatable.GetHashCode(IEqualityComparer comparer)
        {
            // this ignores shape, which is fine  it just means we'll have hash collisions for things 
            // with the same content and different shape.
            return ((IStructuralEquatable)backingArray).GetHashCode(comparer);
        }
        #endregion

    }
}
