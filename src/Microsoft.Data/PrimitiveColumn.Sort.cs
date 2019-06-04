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
            return CloneAndAppendNulls(sortIndices, !ascending);
        }

        internal override BaseColumn GetAscendingSortIndices()
        {
            // The return sortIndices contains only the non null indices. 
            GetSortIndices(Comparer<T>.Default, out PrimitiveColumn<long> sortIndices);
            return sortIndices;
        }

        private void GetSortIndices(IComparer<T> comparer, out PrimitiveColumn<long> columnSortIndices)
        {
            List<int[]> bufferSortIndices = new List<int[]>(_columnContainer.Buffers.Count);
            // Sort each buffer first
            foreach (DataFrameBuffer<T> buffer in _columnContainer.Buffers)
            {
                var sortIndices = new int[buffer.Length];
                for (int i = 0; i < buffer.Length; i++)
                    sortIndices[i] = i;
                IntrospectiveSort(buffer.Span, buffer.Length, sortIndices, comparer);
                bufferSortIndices.Add(sortIndices);
            }
            // Simple merge sort to build the full column's sort indices
            ValueTuple<T, int> GetFirstNonNullValueAndBufferIndexStartingAtIndex(int bufferIndex, int startIndex)
            {
                T value = _columnContainer.Buffers[bufferIndex][bufferSortIndices[bufferIndex][startIndex]];
                long rowIndex = bufferSortIndices[bufferIndex][startIndex] + bufferIndex * _columnContainer.Buffers[0].MaxCapacity;
                while (!IsValid(rowIndex) && ++startIndex < bufferSortIndices[bufferIndex].Length)
                {
                    value = _columnContainer.Buffers[bufferIndex][bufferSortIndices[bufferIndex][startIndex]];
                    rowIndex = startIndex + bufferIndex * _columnContainer.Buffers[0].MaxCapacity;
                }
                return (value, startIndex);
            }
            SortedDictionary<T, List<ValueTuple<int, int>>> heapOfValueAndListOfTupleOfSortAndBufferIndex = new SortedDictionary<T, List<ValueTuple<int, int>>>(comparer);
            IList<DataFrameBuffer<T>> buffers = _columnContainer.Buffers;
            for (int i = 0; i < buffers.Count; i++)
            {
                DataFrameBuffer<T> buffer = buffers[i];
                ValueTuple<T, int> valueAndBufferIndex = GetFirstNonNullValueAndBufferIndexStartingAtIndex(i, 0);
                long columnIndex = valueAndBufferIndex.Item2 + i * bufferSortIndices[0].Length;
                if (columnIndex == Length)
                {
                    // All nulls
                    continue;
                }
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
            GetBufferSortIndex getBufferSortIndex = new GetBufferSortIndex((int bufferIndex, int sortIndex) => (bufferSortIndices[bufferIndex][sortIndex]) + bufferIndex * bufferSortIndices[0].Length);
            GetValueAndBufferSortIndexAtBuffer<T> getValueAndBufferSortIndexAtBuffer = new GetValueAndBufferSortIndexAtBuffer<T>((int bufferIndex, int sortIndex) => GetFirstNonNullValueAndBufferIndexStartingAtIndex(bufferIndex, sortIndex));
            GetBufferLengthAtIndex getBufferLengthAtIndex = new GetBufferLengthAtIndex((int bufferIndex) => bufferSortIndices[bufferIndex].Length);
            PopulateColumnSortIndicesWithHeap(heapOfValueAndListOfTupleOfSortAndBufferIndex, columnSortIndices, getBufferSortIndex, getValueAndBufferSortIndexAtBuffer, getBufferLengthAtIndex);
        }
    }
}
