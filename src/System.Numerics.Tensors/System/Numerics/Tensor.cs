// Copyright (c) Microsoft. All rights reserved.
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
                throw new ArgumentException("Operands must have matching dimensions");
            }

            if (left.Rank == 0)
            {
                throw new ArgumentException($"Cannot operate on Tensor with {nameof(Tensor<T>.Rank)} of 0.");
            }

            for (int i = 0; i < left.Rank; i++)
            {
                if (left.Dimensions[i] != right.Dimensions[i])
                {
                    throw new ArgumentException("Operands must have matching dimensions");
                }
            }
        }

        internal static void ValidateBinaryArgs<T>(Tensor<T> left, Tensor<T> right, Tensor<T> result)
        {
            if (left.Rank != result.Rank || right.Rank != result.Rank || left.Length != result.Length || right.Length != result.Length)
            {
                throw new ArgumentException("Operands and result must have matching dimensions");
            }

            if (left.Rank == 0)
            {
                throw new ArgumentException($"Cannot operate on Tensor with {nameof(Tensor<T>.Rank)} of 0.");
            }

            for (int i = 0; i < result.Rank; i++)
            {
                if (left.Dimensions[i] != result.Dimensions[i] || right.Dimensions[i] != result.Dimensions[i])
                {
                    throw new ArgumentException("Operands and result must have matching dimensions");
                }
            }
        }

        internal static void ValidateBinaryArgs<T>(Tensor<T> left, Tensor<T> right, Tensor<bool> result)
        {
            if (left.Rank != result.Rank || right.Rank != result.Rank || left.Length != result.Length || right.Length != result.Length)
            {
                throw new ArgumentException("Operands and result must have matching dimensions");
            }

            if (left.Rank == 0)
            {
                throw new ArgumentException($"Cannot operate on Tensor with {nameof(Tensor<T>.Rank)} of 0.");
            }

            for (int i = 0; i < result.Rank; i++)
            {
                if (left.Dimensions[i] != result.Dimensions[i] || right.Dimensions[i] != result.Dimensions[i])
                {
                    throw new ArgumentException("Operands and result must have matching dimensions");
                }
            }
        }

        internal static void ValidateArgs<T>(Tensor<T> tensor)
        {
            if (tensor.Rank == 0)
            {
                throw new ArgumentException($"Cannot operate on Tensor with {nameof(Tensor<T>.Rank)} of 0.");
            }
        }

        internal static void ValidateArgs<T>(Tensor<T> tensor, Tensor<T> result)
        {
            if (tensor.Rank != result.Rank || tensor.Length != result.Length)
            {
                throw new ArgumentException("Operands and result must have matching dimensions");
            }

            if (tensor.Rank == 0)
            {
                throw new ArgumentException($"Cannot operate on Tensor with {nameof(Tensor<T>.Rank)} of 0.");
            }

            for (int i = 0; i < result.Rank; i++)
            {
                if (tensor.Dimensions[i] != result.Dimensions[i])
                {
                    throw new ArgumentException("Operands and result must have matching dimensions");
                }
            }
        }
    }

    // When we cross-compile for frameworks that expose ICloneable this must implement ICloneable as well.
    public struct Tensor<T> : IList, ICollection, IEnumerable, IStructuralComparable, IStructuralEquatable
    {
        private readonly T[] backingArray;
        private readonly int[] dimensions;
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

            long size = GetProduct(dimensions);

            // could throw, let the runtime decide what that limit is
            backingArray = new T[size];

            // make a private copy of mutable dimensions array
            this.dimensions = new int[dimensions.Length];
            dimensions.CopyTo(this.dimensions, 0);
            readOnlyDimensions = null;
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

            long size = GetProduct(dimensions);

            if (size != fromBackingArray.Length)
            {
                throw new ArgumentException($"Length of {nameof(fromBackingArray)} ({fromBackingArray.Length}) must match product of {nameof(dimensions)} ({size}).");
            }

            // keep a reference to the backing array
            backingArray = fromBackingArray;

            // make a private copy of mutable dimensions array
            this.dimensions = new int[dimensions.Length];
            dimensions.CopyTo(this.dimensions, 0);
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
            return new Tensor<T>((T[])Buffer.Clone(), dimensions ?? ArrayUtilities.Empty<int>());
        }

        /// <summary>
        /// Creates a new Tensor with the same size as this tensor with elements initialized to their default value.
        /// </summary>
        /// <returns></returns>
        public Tensor<T> CloneEmpty()
        {
            return new Tensor<T>(dimensions ?? ArrayUtilities.Empty<int>());
        }


        /// <summary>
        /// Creates a new Tensor of a different type with the same size as this tensor with elements initialized to their default value.
        /// </summary>
        /// <returns></returns>
        public Tensor<TResult> CloneEmpty<TResult>()
        {
            return new Tensor<TResult>(dimensions ?? ArrayUtilities.Empty<int>());
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

            var remainingDimensions = new int[dimensions.Length - 2];
            
            for(int i = 1; i < diagonal.dimensions.Length; i++)
            {
                dimensions[i + 1] = diagonal.dimensions[i];
                remainingDimensions[i - 1] = diagonal.dimensions[i];
            }

            var result = new Tensor<T>(dimensions);

            var sizePerDiagonal = diagonal.Length / diagonalLength;
            var destIndices = new int[result.Rank];
            var diagnonalIndices = new int[diagonal.Rank];
            var diagProjectionIndices = new int[remainingDimensions.Length];

            for (int diagIndex = 0; diagIndex < diagonalLength; diagIndex++)
            {
                var destIndex0 = offset < 0 ? diagIndex - offset : diagIndex;
                var destIndex1 = offset > 0 ? diagIndex + offset : diagIndex;

                destIndices[0] = destIndex0;
                destIndices[1] = destIndex1;
                diagnonalIndices[0] = diagIndex;

                for (int diagProjectionOffset = 0; diagProjectionOffset < sizePerDiagonal; diagProjectionOffset++)
                {
                    if (remainingDimensions.Length > 0)
                    {
                        GetIndicesFromOffset(diagProjectionOffset, sizePerDiagonal, remainingDimensions, diagProjectionIndices);

                        Array.Copy(diagProjectionIndices, 0, destIndices, 2, diagProjectionIndices.Length);
                        Array.Copy(diagProjectionIndices, 0, diagnonalIndices, 1, diagProjectionIndices.Length);
                    }

                    result[destIndices] = diagonal[diagnonalIndices];
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

            var remainingDimensions = new int[dimensions.Length - 2];

            for(int i = 2; i < dimensions.Length; i++)
            {
                newTensorDimensions[i - 1] = dimensions[i];
                remainingDimensions[i - 2] = dimensions[i];
            }

            var diagonalTensor = new Tensor<T>(dimensions:newTensorDimensions);
            var sizePerDiagonal = diagonalTensor.Length / diagonalLength;

            Debug.Assert(sizePerDiagonal == GetProduct(remainingDimensions) || remainingDimensions.Length == 0);

            // TODO: avoid translating to indices and back by directly accessing backing array
            var sourceIndices = new int[Rank];
            var diagnonalIndices = new int[diagonalTensor.Rank];
            var diagProjectionIndices = new int[remainingDimensions.Length];

            for (int diagIndex = 0; diagIndex < diagonalLength; diagIndex++)
            {
                var sourceIndex0 = offset < 0 ? diagIndex - offset : diagIndex;
                var sourceIndex1 = offset > 0 ? diagIndex + offset : diagIndex;
                
                sourceIndices[0] = sourceIndex0;
                sourceIndices[1] = sourceIndex1;
                diagnonalIndices[0] = diagIndex;

                for(int diagProjectionOffset = 0; diagProjectionOffset < sizePerDiagonal; diagProjectionOffset++)
                {
                    if (remainingDimensions.Length > 0)
                    {
                        GetIndicesFromOffset(diagProjectionOffset, sizePerDiagonal, remainingDimensions, diagProjectionIndices);

                        Array.Copy(diagProjectionIndices, 0, sourceIndices, 2, diagProjectionIndices.Length);
                        Array.Copy(diagProjectionIndices, 0, diagnonalIndices, 1, diagProjectionIndices.Length);
                    }

                    diagonalTensor[diagnonalIndices] = this[sourceIndices];
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

            var projectionSize = result.Length / (axisLength0 * axisLength1);

            var remainingDimensions = new int[dimensions.Length - 2];

            for (int i = 2; i < dimensions.Length; i++)
            {
                remainingDimensions[i - 2] = dimensions[i];
            }

            // TODO: avoid translating to indices and back by directly accessing backing array
            var indices = new int[Rank];

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
                    indices[0] = triIndex0;
                    indices[1] = triIndex1;

                    for (int projectionOffset = 0; projectionOffset < projectionSize; projectionOffset++)
                    {
                        // copy a given tri element, projected across remaining dimensions
                        if (indices.Length > 2)
                        {
                            GetIndicesFromOffset(projectionOffset, projectionSize, dimensions, indices, startIndex: 2 /* skip first two dimensions*/);
                        }
                        result[indices] = this[indices];
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

        public Tensor<T> Reshape(params int[] dimensions)
        {
            if (dimensions == null)
            {
                throw new ArgumentNullException(nameof(dimensions));
            }

            var newSize = GetProduct(dimensions);

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

            var newSize = (int)GetProduct(dimensions);

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

                return Buffer[index];
            }
            set
            {
                if (Rank != 1)
                {
                    throw new ArgumentOutOfRangeException($"Cannot use single dimension indexer on {nameof(Tensor<T>)} with {Rank} dimensions.");
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

        private int GetOffset(int indexRow, int indexColumn)
        {
            if (Rank != 2)
            {
                throw new ArgumentOutOfRangeException($"Cannot use 2 dimension indexer on {nameof(Tensor<T>)} with {Rank} dimensions.");
            }

            if (indexRow < 0 || indexRow >= dimensions[0])
            {
                throw new ArgumentOutOfRangeException(nameof(indexRow));
            }

            if (indexColumn < 0 || indexColumn >= dimensions[1])
            {
                throw new ArgumentOutOfRangeException(nameof(indexColumn));
            }

            return indexRow * dimensions[1] + indexColumn;
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

            return GetOffsetFromIndicies(indices, dimensions);
        }

        // Inverse of GetOffsetFromIndices
        private void GetIndicesFromOffset(int offset, int[] indices)
        {
            GetIndicesFromOffset(offset, Length, dimensions, indices);
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

        private static int GetOffsetFromIndicies(int[] indices, int[] dimensions)
        {
            Debug.Assert(indices.Length == dimensions.Length);

            int index = 0, dimension = 0;
            for (int i = 0; i < indices.Length; i++)
            {
                dimension = dimensions[i];
                if (i != 0)
                {
                    index *= dimension;
                }

                var currentDimensionIndex = indices[i];

                if (currentDimensionIndex < 0 || currentDimensionIndex >= dimension)
                {
                    throw new ArgumentOutOfRangeException($"{nameof(indices)}[{i}]");
                }

                index += currentDimensionIndex;
            }

            return index;
        }

        private static void GetIndicesFromOffset(int offset, int totalLength, int[] dimensions, int[] indices, int startIndex = 0)
        {
            Debug.Assert(indices.Length == dimensions.Length);
            Debug.Assert(startIndex < dimensions.Length);
            Debug.Assert(totalLength == GetProduct(dimensions, startIndex));

            var divisor = totalLength;

            for (int i = startIndex; i < indices.Length; i++)
            {
                divisor /= dimensions[i];

                var current = offset / divisor;

                indices[i] = current;
                offset %= divisor;
            }
        }

        private static long GetProduct(int[] dimensions, int startIndex = 0)
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
                GetIndicesFromOffset(i, indices);
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
                GetIndicesFromOffset(i, indices);
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
