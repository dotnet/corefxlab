// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Microsoft.Data
{
    public partial class PrimitiveColumn<T> : BaseColumn
        where T : unmanaged
    {
        public override BaseColumn Sort(bool ascending = true)
        {
            PrimitiveColumn<long> sortIndices = GetAscendingSortIndices() as PrimitiveColumn<long>;
            return Clone(sortIndices, !ascending, NullCount);
        }

        internal override BaseColumn GetAscendingSortIndices()
        {
            // The return sortIndices contains only the non null indices. 
            GetSortIndices(Comparer<T>.Default, out PrimitiveColumn<long> sortIndices);
            return sortIndices;
        }

        private void GetSortIndices(IComparer<T> comparer, out PrimitiveColumn<long> columnSortIndices)
        {
            List<List<int>> bufferSortIndices = new List<List<int>>(_columnContainer.Buffers.Count);
            // Sort each buffer first
            for (int b = 0; b < _columnContainer.Buffers.Count; b++)
            {
                DataFrameBuffer<T> buffer = _columnContainer.Buffers[b];
                int[] sortIndices = new int[buffer.Length];
                for (int i = 0; i < buffer.Length; i++)
                    sortIndices[i] = i;
                IntrospectiveSort(buffer.ReadOnlySpan, buffer.Length, sortIndices, comparer);
                // Bug fix: QuickSort is not stable. When PrimitiveColumn has null values and default values, they move around
                List<int> nonNullSortIndices = new List<int>();
                for (int i = 0; i < sortIndices.Length; i++)
                {
                    if (IsValid(sortIndices[i] + b * DataFrameBuffer<T>.MaxCapacity))
                        nonNullSortIndices.Add(sortIndices[i]);
                }
                bufferSortIndices.Add(nonNullSortIndices);
            }
            // Simple merge sort to build the full column's sort indices
            ValueTuple<T, int> GetFirstNonNullValueAndBufferIndexStartingAtIndex(int bufferIndex, int startIndex)
            {
                T value = _columnContainer.Buffers[bufferIndex][bufferSortIndices[bufferIndex][startIndex]];
                long rowIndex = bufferSortIndices[bufferIndex][startIndex] + bufferIndex * DataFrameBuffer<T>.MaxCapacity;
                return (value, startIndex);
            }
            SortedDictionary<T, List<ValueTuple<int, int>>> heapOfValueAndListOfTupleOfSortAndBufferIndex = new SortedDictionary<T, List<ValueTuple<int, int>>>(comparer);
            IList<DataFrameBuffer<T>> buffers = _columnContainer.Buffers;
            for (int i = 0; i < buffers.Count; i++)
            {
                DataFrameBuffer<T> buffer = buffers[i];
                if (bufferSortIndices[i].Count == 0)
                {
                    // All nulls
                    continue;
                }
                ValueTuple<T, int> valueAndBufferIndex = GetFirstNonNullValueAndBufferIndexStartingAtIndex(i, 0);
                if (heapOfValueAndListOfTupleOfSortAndBufferIndex.ContainsKey(valueAndBufferIndex.Item1))
                {
                    heapOfValueAndListOfTupleOfSortAndBufferIndex[valueAndBufferIndex.Item1].Add((valueAndBufferIndex.Item2, i));
                }
                else
                {
                    heapOfValueAndListOfTupleOfSortAndBufferIndex.Add(valueAndBufferIndex.Item1, new List<ValueTuple<int, int>>() { (valueAndBufferIndex.Item2, i) });
                }
            }
            columnSortIndices = new PrimitiveColumn<long>("SortIndices");
            GetBufferSortIndex getBufferSortIndex = new GetBufferSortIndex((int bufferIndex, int sortIndex) => (bufferSortIndices[bufferIndex][sortIndex]) + bufferIndex * bufferSortIndices[0].Count);
            GetValueAndBufferSortIndexAtBuffer<T> getValueAndBufferSortIndexAtBuffer = new GetValueAndBufferSortIndexAtBuffer<T>((int bufferIndex, int sortIndex) => GetFirstNonNullValueAndBufferIndexStartingAtIndex(bufferIndex, sortIndex));
            GetBufferLengthAtIndex getBufferLengthAtIndex = new GetBufferLengthAtIndex((int bufferIndex) => bufferSortIndices[bufferIndex].Count);
            PopulateColumnSortIndicesWithHeap(heapOfValueAndListOfTupleOfSortAndBufferIndex, columnSortIndices, getBufferSortIndex, getValueAndBufferSortIndexAtBuffer, getBufferLengthAtIndex);
        }
    }
}
