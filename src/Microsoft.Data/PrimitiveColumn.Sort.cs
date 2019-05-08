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
    public partial class PrimitiveColumn<T> : BaseColumn
        where T : unmanaged
    {
        public override BaseColumn Sort(bool ascending = true)
        {
            PrimitiveColumn<long> sortIndices = GetAscendingSortIndices() as PrimitiveColumn<long>;
            return _Clone(sortIndices, ascending);
        }

        public override BaseColumn GetAscendingSortIndices()
        {
            // Is Comparer<T>.Default guaranteed to sort in ascending order?
            _GetSortIndices(Comparer<T>.Default, out PrimitiveColumn<long> sortIndices);
            return sortIndices;
        }

        private void _GetSortIndices(IComparer<T> comparer, out PrimitiveColumn<long> columnSortIndices)
        {
            List<int[]> bufferSortIndices = new List<int[]>(_columnContainer.Buffers.Count);
            // Sort each buffer first
            foreach (DataFrameBuffer<T> buffer in _columnContainer.Buffers)
            {
                var sortIndices = new int[buffer.Length];
                for (int i = 0; i < buffer.Length; i++) sortIndices[i] = i;
                IntrospectiveSort(buffer.Span, buffer.Length, sortIndices, comparer);
                bufferSortIndices.Add(sortIndices);
            }
            // Simple merge sort to build the full column's sort indices
            SortedDictionary<T, List<Tuple<int, int>>> heapOfValueAndListOfTupleOfSortAndBufferIndex = new SortedDictionary<T, List<Tuple<int, int>>>(comparer);
            var buffers = _columnContainer.Buffers;
            for (int i = 0; i < buffers.Count; i++)
            {
                DataFrameBuffer<T> buffer = buffers[i];
                T value = buffer[bufferSortIndices[i][0]];
                if (heapOfValueAndListOfTupleOfSortAndBufferIndex.ContainsKey(value))
                {
                    heapOfValueAndListOfTupleOfSortAndBufferIndex[value].Add(new Tuple<int, int>(0, i));
                }
                else
                {
                    heapOfValueAndListOfTupleOfSortAndBufferIndex.Add(value, new List<Tuple<int, int>>() {new Tuple<int, int>(0, i) });
                }
            }
            columnSortIndices = new PrimitiveColumn<long>("SortIndices");
            GetBufferSortIndex getBufferSortIndex = new GetBufferSortIndex((int bufferIndex, int sortIndex) => bufferSortIndices[bufferIndex][sortIndex]);
            GetValueAtBuffer<T> getValueAtBuffer = new GetValueAtBuffer<T>((int bufferIndex, int sortIndex) => _columnContainer.Buffers[bufferIndex][bufferSortIndices[bufferIndex][sortIndex]]);
            GetBufferLengthAtIndex getBufferLengthAtIndex = new GetBufferLengthAtIndex((int bufferIndex) => bufferSortIndices[bufferIndex].Length);
            PopulateColumnSortIndicesWithHeap(heapOfValueAndListOfTupleOfSortAndBufferIndex, columnSortIndices, getBufferSortIndex, getValueAtBuffer, getBufferLengthAtIndex);
        }
    }
}
