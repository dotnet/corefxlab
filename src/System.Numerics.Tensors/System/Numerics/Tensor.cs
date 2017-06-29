// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections;
using System.Diagnostics;

namespace System.Numerics
{
    public class Tensor<T>: IList, ICollection, IEnumerable, IStructuralComparable, IStructuralEquatable // todo: ICloneable
    {
        private readonly T[] backingArray;
        private readonly int[] dimensions;


        public Tensor(Array fromArray)
        {
            if (fromArray == null)
            {
                throw new ArgumentNullException(nameof(fromArray));
            }

            // copy initial array
            dimensions = new int[fromArray.Rank];
            for(int i = 0; i < dimensions.Length; i++)
            {
                dimensions[i] = fromArray.GetLength(i);
            }

            backingArray = new T[fromArray.Length];

            // TODO: check if blittable and memcpy?
            int index = 0;
            foreach(var item in fromArray)
            {
                backingArray[index++] = (T)item;
            }
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
                // rank 0 tensor should be a scalar, need to think about how to represent that.
                throw new ArgumentOutOfRangeException(nameof(dimensions));
            }

            long size = GetProduct(dimensions);

            // could throw, let the runtime decide what that limit is
            backingArray = new T[size];

            this.dimensions = new int[dimensions.Length];
            dimensions.CopyTo(this.dimensions, 0);
        }

        public int Length => backingArray.Length;
        public int Rank => dimensions.Length;

        /// <summary>
        /// Sets all elements in Tensor to <paramref name="value"/>.
        /// </summary>
        /// <param name="value">Value to fill</param>
        public void Fill(T value)
        {
            // JIT look here, lend us a hand
            for (int i = 0; i < backingArray.Length; i++)
            {
                // is it possible to fast-path when initialValue == default(T)?
                backingArray[i] = value;
            }
        }

        public Tensor<T> CloneEmpty()
        {
            return new Tensor<T>(dimensions);
        }

        public static Tensor<T> CreateIdentity(int size, T oneValue)
        {
            var result = new Tensor<T>(size, size);

            for(int i = 0; i < size; i++)
            {
                result.backingArray[i] = oneValue;
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

            // TODO: allow specification of axis1 and axis2
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

            // number of 
            var sizePerDiagonal = diagonalTensor.Length / diagonalLength;

            Debug.Assert(sizePerDiagonal == GetProduct(remainingDimensions));

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
                    GetIndicesFromOffset(diagProjectionOffset, sizePerDiagonal, remainingDimensions, diagProjectionIndices);

                    Array.Copy(diagProjectionIndices, 0, sourceIndices, 2, diagProjectionIndices.Length);
                    Array.Copy(diagProjectionIndices, 0, diagnonalIndices, 1, diagProjectionIndices.Length);

                    diagonalTensor[diagnonalIndices] = this[sourceIndices];
                }
            }

            return diagonalTensor;
        }
        

        public T this[int index]
        {
            get
            {
                if (dimensions.Length != 1)
                {
                    throw new ArgumentOutOfRangeException($"Cannot use single dimension indexer on {nameof(Tensor<T>)} with {dimensions.Length} dimensions.");
                }

                return backingArray[index];
            }
            set
            {
                if (dimensions.Length != 1)
                {
                    throw new ArgumentOutOfRangeException($"Cannot use single dimension indexer on {nameof(Tensor<T>)} with {dimensions.Length} dimensions.");
                }

                backingArray[index] = value;
            }
        }

        public T this[int indexRow, int indexColumn]
        {
            get
            {
                var index = GetOffset(indexRow, indexColumn);
                return backingArray[index];
            }
            set
            {

                var index = GetOffset(indexRow, indexColumn);
                backingArray[index] = value;
            }
        }
        
        public T this[params int[] indices]
        {
            get
            {
                var index = GetOffsetFromIndices(indices);
                return backingArray[index];
            }

            set
            {
                var index = GetOffsetFromIndices(indices);
                backingArray[index] = value;
            }
        }

        private int GetOffset(int indexRow, int indexColumn)
        {
            if (dimensions.Length != 2)
            {
                throw new ArgumentOutOfRangeException($"Cannot use 2 dimension indexer on {nameof(Tensor<T>)} with {dimensions.Length} dimensions.");
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

            if (indices.Length != this.dimensions.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(indices));
            }

            return GetOffsetFromIndicies(indices, dimensions);
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

        // Inverse of GetOffsetFromIndices
        private void GetIndicesFromOffset(int offset, int[] indices)
        {
            GetIndicesFromOffset(offset, backingArray.Length, dimensions, indices);
        }

        private static void GetIndicesFromOffset(int offset, int totalLength, int[] dimensions, int[] indices)
        {
            Debug.Assert(indices.Length == dimensions.Length);
            Debug.Assert(totalLength == GetProduct(dimensions));

            var divisor = totalLength;

            for (int i = 0; i < indices.Length; i++)
            {
                divisor /= dimensions[i];

                var current = offset / divisor;

                indices[i] = current;
                offset %= divisor;
            }
        }

        private static long GetProduct(int[] dimensions)
        {
            long product = 1;
            for (int i = 0; i < dimensions.Length; i++)
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

        public IEnumerator GetEnumerator()
        {
            return backingArray.GetEnumerator();
        }

        #region ICollection members
        int ICollection.Count => backingArray.Length;

        bool ICollection.IsSynchronized => false;

        object ICollection.SyncRoot => this; // backingArray.this?

        public void CopyTo(Array array, int index)
        {
            backingArray.CopyTo(array, index);
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
            return ((IList)backingArray).Add(value);
        }

        void IList.Clear()
        {
            ((IList)backingArray).Clear();
        }

        bool IList.Contains(object value)
        {
            return ((IList)backingArray).Contains(value);
        }

        int IList.IndexOf(object value)
        {
            return ((IList)backingArray).IndexOf(value);
        }

        void IList.Insert(int index, object value)
        {
            // will throw
            ((IList)backingArray).Insert(index, value);
        }

        void IList.Remove(object value)
        {
            // will throw
            ((IList)backingArray).Remove(value);
        }

        void IList.RemoveAt(int index)
        {
            // will throw
            ((IList)backingArray).RemoveAt(index);
        }
        #endregion

        #region IStructuralComparable members
        int IStructuralComparable.CompareTo(object other, IComparer comparer)
        {
            if (other == null)
            {
                return 1;
            }

            var otherTensor = other as Tensor<T>;
            
            if (otherTensor != null)
            {
                return CompareTo(otherTensor, comparer);
            }

            var otherArray = other as Array;

            if (otherArray != null)
            {
                return CompareTo(otherArray, comparer);
            }

            // todo: check exception
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
            var indices = new int[Rank];
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

            var otherTensor = other as Tensor<T>;

            if (otherTensor != null)
            {
                return Equals(otherTensor, comparer);
            }

            var otherArray = other as Array;

            if (otherArray != null)
            {
                return Equals(otherArray, comparer);
            }

            // todo: check exception
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
            
            var indices = new int[Rank];
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
