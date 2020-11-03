// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Microsoft.Data.Analysis
{
    public partial class PrimitiveDataFrameColumn<T> : DataFrameColumn
        where T : unmanaged
    {
        public new PrimitiveDataFrameColumn<T> Sort(bool ascending = true)
        {
            PrimitiveDataFrameColumn<long> sortIndices;
            if (ascending)
            {
                sortIndices = GetAscendingSortIndices();
            }
            else
            {
                sortIndices = GetDescendingSortIndices();
            }
            return Clone(sortIndices);
        }

        internal override PrimitiveDataFrameColumn<long> GetAscendingSortIndices()
        {
            PrimitiveDataFrameColumn<long> sortIndices = GetSortIndices(Comparer<T>.Default, out PrimitiveDataFrameColumn<long> columnNullIndices);
            for (int i = 0; i < columnNullIndices.Length; i++)
            {
                sortIndices.Append(columnNullIndices[i]);
            }
            return sortIndices;
        }

        internal override PrimitiveDataFrameColumn<long> GetDescendingSortIndices()
        {
            Comparison<T> descendingComparison = new Comparison<T>((T a, T b) =>
            {
                return Comparer<T>.Default.Compare(b, a);
            });
            Comparer<T> descending = Comparer<T>.Create(descendingComparison);
            PrimitiveDataFrameColumn<long> sortIndices = GetSortIndices(descending, out PrimitiveDataFrameColumn<long> nullIndices);
            for (long i = 0; i < nullIndices.Length; i++)
            {
                sortIndices.Append(nullIndices[i]);
            }
            return sortIndices;
        }

        private PrimitiveDataFrameColumn<long> GetSortIndices(IComparer<T> comparer, out PrimitiveDataFrameColumn<long> columnNullIndices)
        {
            List<List<int>> bufferSortIndices = new List<List<int>>(_columnContainer.Buffers.Count);
            columnNullIndices = new PrimitiveDataFrameColumn<long>("columnNullIndices");

            // Sort each buffer first
            for (int b = 0; b < _columnContainer.Buffers.Count; b++)
            {
                ReadOnlyDataFrameBuffer<T> buffer = _columnContainer.Buffers[b];
                ReadOnlySpan<byte> nullBitMapSpan = _columnContainer.NullBitMapBuffers[b].ReadOnlySpan;
                int[] sortIndices = new int[buffer.Length];
                for (int i = 0; i < buffer.Length; i++)
                    sortIndices[i] = i;
                IntrospectiveSort(buffer.ReadOnlySpan, buffer.Length, sortIndices, comparer);
                // Bug fix: QuickSort is not stable. When PrimitiveDataFrameColumn has null values and default values, they move around
                List<int> nonNullSortIndices = new List<int>();
                for (int i = 0; i < sortIndices.Length; i++)
                {
                    if (_columnContainer.IsValid(nullBitMapSpan, sortIndices[i]))
                        nonNullSortIndices.Add(sortIndices[i]);
                    else
                    {
                        columnNullIndices.Append(sortIndices[i] + b * _columnContainer.Buffers[0].Length);
                    }

                }
                bufferSortIndices.Add(nonNullSortIndices);
            }
            // Simple merge sort to build the full column's sort indices
            ValueTuple<T, int> GetFirstNonNullValueAndBufferIndexStartingAtIndex(int bufferIndex, int startIndex)
            {
                int index = bufferSortIndices[bufferIndex][startIndex];
                T value;
                ReadOnlyMemory<byte> buffer = _columnContainer.Buffers[bufferIndex].ReadOnlyBuffer;
                ReadOnlyMemory<T> typedBuffer = Unsafe.As<ReadOnlyMemory<byte>, ReadOnlyMemory<T>>(ref buffer);
                if (!typedBuffer.IsEmpty)
                {
                    bool isArray = MemoryMarshal.TryGetArray(typedBuffer, out ArraySegment<T> arraySegment);
                    if (isArray)
                    {
                        value = arraySegment.Array[index + arraySegment.Offset];
                    }
                    else
                        value = _columnContainer.Buffers[bufferIndex][index];
                }
                else
                {
                    value = _columnContainer.Buffers[bufferIndex][index];
                }
                return (value, startIndex);
            }
            SortedDictionary<T, List<ValueTuple<int, int>>> heapOfValueAndListOfTupleOfSortAndBufferIndex = new SortedDictionary<T, List<ValueTuple<int, int>>>(comparer);
            IList<ReadOnlyDataFrameBuffer<T>> buffers = _columnContainer.Buffers;
            for (int i = 0; i < buffers.Count; i++)
            {
                ReadOnlyDataFrameBuffer<T> buffer = buffers[i];
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
            PrimitiveDataFrameColumn<long> columnSortIndices = new PrimitiveDataFrameColumn<long>("SortIndices");
            GetBufferSortIndex getBufferSortIndex = new GetBufferSortIndex((int bufferIndex, int sortIndex) => (bufferSortIndices[bufferIndex][sortIndex]) + bufferIndex * bufferSortIndices[0].Count);
            GetValueAndBufferSortIndexAtBuffer<T> getValueAndBufferSortIndexAtBuffer = new GetValueAndBufferSortIndexAtBuffer<T>((int bufferIndex, int sortIndex) => GetFirstNonNullValueAndBufferIndexStartingAtIndex(bufferIndex, sortIndex));
            GetBufferLengthAtIndex getBufferLengthAtIndex = new GetBufferLengthAtIndex((int bufferIndex) => bufferSortIndices[bufferIndex].Count);
            PopulateColumnSortIndicesWithHeap(heapOfValueAndListOfTupleOfSortAndBufferIndex, columnSortIndices, getBufferSortIndex, getValueAndBufferSortIndexAtBuffer, getBufferLengthAtIndex);
            return columnSortIndices;
        }
    }
}
