// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Microsoft.Data
{
    /// <summary>
    /// The base column type. All APIs should be defined here first
    /// </summary>
    public abstract partial class BaseColumn
    {
        public BaseColumn(string name, long length, Type type)
        {
            Length = length;
            Name = name;
            DataType = type;
        }

        private long _length;
        public long Length
        {
            get => _length;
            protected set
            {
                if (value < 0) throw new ArgumentOutOfRangeException();
                _length = value;
            }
        }

        public long NullCount { get; protected set; }

        public string Name { get; set; }

        public Type DataType { get; }

        public virtual object this[long rowIndex]
        {
            get => GetValue(rowIndex);
            set => SetValue(rowIndex, value);
        }

        protected virtual object GetValue(long rowIndex) => throw new NotImplementedException();
        protected virtual void SetValue(long rowIndex, object value) => throw new NotImplementedException();

        public virtual object this[long startIndex, int length]
        {
            get => throw new NotImplementedException();
        }

        /// <summary>
        /// Clone column to produce a copy potentially changing the order by supplying mapIndices and an invert flag
        /// </summary>
        /// <param name="mapIndices"></param>
        /// <param name="invertMapIndices"></param>
        /// <returns></returns>
        public virtual BaseColumn Clone(BaseColumn mapIndices = null, bool invertMapIndices = false) => throw new NotImplementedException();

        public virtual BaseColumn Sort(bool ascending = true) => throw new NotImplementedException();

        public virtual BaseColumn GetAscendingSortIndices() => throw new NotImplementedException();

        protected delegate long GetBufferSortIndex(int bufferIndex, int sortIndex);
        protected delegate T GetValueAtBuffer<T>(int bufferIndex, int valueIndex);
        protected delegate int GetBufferLengthAtIndex(int bufferIndex);
        protected void PopulateColumnSortIndicesWithHeap<T>(SortedDictionary<T, List<Tuple<int, int>>> heapOfValueAndListOfTupleOfSortAndBufferIndex, PrimitiveColumn<long> columnSortIndices, GetBufferSortIndex getBufferSortIndex, GetValueAtBuffer<T> getValueAtBuffer, GetBufferLengthAtIndex getBufferLengthAtIndex)
        {
            while (heapOfValueAndListOfTupleOfSortAndBufferIndex.Count > 0)
            {
                KeyValuePair<T, List<Tuple<int, int>>> minElement = heapOfValueAndListOfTupleOfSortAndBufferIndex.ElementAt(0);
                T value = minElement.Key;
                List<Tuple<int, int>> tuplesOfSortAndBufferIndex = minElement.Value;
                Tuple<int, int> sortAndBufferIndex;
                if (tuplesOfSortAndBufferIndex.Count == 1)
                {
                    heapOfValueAndListOfTupleOfSortAndBufferIndex.Remove(minElement.Key);
                    sortAndBufferIndex = tuplesOfSortAndBufferIndex[0];
                }
                else
                {
                    sortAndBufferIndex = tuplesOfSortAndBufferIndex[tuplesOfSortAndBufferIndex.Count - 1];
                    tuplesOfSortAndBufferIndex.RemoveAt(tuplesOfSortAndBufferIndex.Count - 1);
                }
                int sortIndex = sortAndBufferIndex.Item1;
                int bufferIndex = sortAndBufferIndex.Item2;
                //long bufferSortIndex = bufferSortIndices[bufferIndex][sortIndex];
                long bufferSortIndex = getBufferSortIndex(bufferIndex, sortIndex);
                //bufferSortIndex += bufferIndex * buffers[0].Count;
                columnSortIndices.Append(bufferSortIndex);
                //if (nextSortIndex < bufferSortIndices[bufferIndex].Length)
                if (sortIndex + 1 < getBufferLengthAtIndex(bufferIndex))
                {
                    int nextSortIndex = sortIndex + 1;
                    //T nextValue = buffer[bufferSortIndices[bufferIndex][nextSortIndex]];
                    T nextValue = getValueAtBuffer(bufferIndex, nextSortIndex);
                    heapOfValueAndListOfTupleOfSortAndBufferIndex.Add(nextValue, new List<Tuple<int, int>>() { new Tuple<int, int>((int)nextSortIndex, bufferIndex) });
                }
            }

        }
        internal int FloorLog2PlusOne(int n)
        {
            Debug.Assert(n >= 2);
            int result = 2;
            n >>= 2;
            while (n > 0)
            {
                ++result;
                n >>= 1;
            }
            return result;
        }

        protected void IntrospectiveSort<T>(
            Span<T> span,
            int length,
            Span<int> sortIndices,
            IComparer<T> comparer)
        {
            var depthLimit = 2 * FloorLog2PlusOne(length);
            IntroSortRecursive(span, 0, length - 1, depthLimit, sortIndices, comparer);
        }

        private static void IntroSortRecursive<T>(
            Span<T> span,
            int lo, int hi, int depthLimit,
            Span<int> sortIndices,
            IComparer<T> comparer)
        {
            Debug.Assert(comparer != null);
            Debug.Assert(lo >= 0);

            while (hi > lo)
            {
                int partitionSize = hi - lo + 1;
                if (partitionSize <= 16)
                {
                    if (partitionSize == 1)
                    {
                        return;
                    }
                    if (partitionSize == 2)
                    {
                        Sort2(span, lo, hi, sortIndices, comparer);
                        return;
                    }
                    if (partitionSize == 3)
                    {
                        Sort3(span, lo, hi - 1, hi, sortIndices, comparer);
                        return;
                    }

                    InsertionSort(span, lo, hi, sortIndices, comparer);
                    return;
                }

                if (depthLimit == 0)
                {
                    HeapSort(span, lo, hi, sortIndices, comparer);
                    return;
                }
                depthLimit--;

                // We should never reach here, unless > 3 elements due to partition size
                int p = PickPivotAndPartition(span, lo, hi, sortIndices, comparer);
                // Note we've already partitioned around the pivot and do not have to move the pivot again.
                IntroSortRecursive(span, p + 1, hi, depthLimit, sortIndices, comparer);
                hi = p - 1;
            }
        }

        private static int PickPivotAndPartition<TKey, TComparer>(
            Span<TKey> span, int lo, int hi,
            Span<int> sortIndices,
            TComparer comparer)
            where TComparer : IComparer<TKey>
        {
            Debug.Assert(comparer != null);
            Debug.Assert(lo >= 0);
            Debug.Assert(hi > lo);

            // median-of-three
            int middle = (int)(((uint)hi + (uint)lo) >> 1);

            // Sort lo, mid and hi appropriately, then pick mid as the pivot.
            Sort3(span, lo, middle, hi, sortIndices, comparer);

            TKey pivot = span[sortIndices[middle]];
            int pivotIndex = sortIndices[middle];

            int left = lo;
            int right = hi - 1;
            // We already partitioned lo and hi and put the pivot in hi - 1.  
            Swap(ref sortIndices[middle], ref sortIndices[right]);

            while (left < right)
            {
                while (left < (hi - 1) && comparer.Compare(span[sortIndices[++left]], pivot) < 0) ;
                // Check if bad comparable/comparer
                if (left == (hi - 1) && comparer.Compare(span[sortIndices[left]], pivot) < 0)
                    throw new ArgumentException("Bad comparer");

                while (right > lo && comparer.Compare(pivot, span[sortIndices[--right]]) < 0) ;
                // Check if bad comparable/comparer
                if (right == lo && comparer.Compare(pivot, span[sortIndices[right]]) < 0)
                    throw new ArgumentException("Bad comparer");

                if (left >= right)
                    break;

                Swap(ref sortIndices[left], ref sortIndices[right]);
            }
            // Put pivot in the right location.
            right = hi - 1;
            if (left != right)
            {
                Swap(ref sortIndices[left], ref sortIndices[right]);
            }
            return left;
        }

        internal static void Swap<TKey>(ref TKey a, ref TKey b)
        {
            TKey temp = a;
            a = b;
            b = temp;
        }

        private static void HeapSort<TKey, TComparer>(
            Span<TKey> span, int lo, int hi,
            Span<int> sortIndices,
            TComparer comparer)
            where TComparer : IComparer<TKey>
        {
            Debug.Assert(comparer != null);
            Debug.Assert(lo >= 0);
            Debug.Assert(hi > lo);

            int n = hi - lo + 1;
            for (int i = n / 2; i >= 1; --i)
            {
                DownHeap(span, i, n, lo, sortIndices, comparer);
            }
            for (int i = n; i > 1; --i)
            {
                Swap(ref sortIndices[lo], ref sortIndices[lo + i - 1]);
                DownHeap(span, 1, i - 1, lo, sortIndices, comparer);
            }
        }

        private static void DownHeap<TKey, TComparer>(
            Span<TKey> span, int i, int n, int lo,
            Span<int> sortIndices,
            TComparer comparer)
            where TComparer : IComparer<TKey>
        {
            // Max Heap
            Debug.Assert(comparer != null);
            Debug.Assert(lo >= 0);

            int di = sortIndices[lo - 1 + i];
            TKey d = span[di];
            var nHalf = n / 2;
            while (i <= nHalf)
            {
                int child = i << 1;

                if (child < n &&
                    comparer.Compare(span[sortIndices[lo + child - 1]], span[sortIndices[lo + child]]) < 0)
                {
                    ++child;
                }

                if (!(comparer.Compare(d, span[sortIndices[lo + child - 1]]) < 0))
                    break;

                sortIndices[lo + i - 1] = sortIndices[lo + child - 1];

                i = child;
            }
            sortIndices[lo + i - 1] = di;
        }

        private static void InsertionSort<TKey, TComparer>(
            Span<TKey> span, int lo, int hi,
            Span<int> sortIndices,
            TComparer comparer)
            where TComparer : IComparer<TKey>
        {
            Debug.Assert(lo >= 0);
            Debug.Assert(hi >= lo);

            for (int i = lo; i < hi; ++i)
            {
                int j = i;
                var t = span[sortIndices[j + 1]];
                var ti = sortIndices[j + 1];
                if (j >= lo && comparer.Compare(t, span[sortIndices[j]]) < 0)
                {
                    do
                    {
                        sortIndices[j + 1] = sortIndices[j];
                        --j;
                    }
                    while (j >= lo && comparer.Compare(t, span[sortIndices[j]]) < 0);

                    sortIndices[j + 1] = ti;
                }
            }
        }

        private static void Sort3<TKey, TComparer>(
            Span<TKey> span, int i, int j, int k,
            Span<int> sortIndices,
            TComparer comparer)
            where TComparer : IComparer<TKey>
        {
            Sort2(span, i, j, sortIndices, comparer);
            Sort2(span, i, k, sortIndices, comparer);
            Sort2(span, j, k, sortIndices, comparer);
        }

        private static void Sort2<TKey>(
            Span<TKey> span, int i, int j,
            Span<int> sortIndices,
            IComparer<TKey> comparer)
        {
            Debug.Assert(i != j);
            if (comparer.Compare(span[sortIndices[i]], span[sortIndices[j]]) > 0)
            {
                int temp = sortIndices[i];
                sortIndices[i] = sortIndices[j];
                sortIndices[j] = temp;
            }
        }
    }
}
