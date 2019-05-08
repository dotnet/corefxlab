// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Data
{
    /// <summary>
    /// A column to hold strings
    /// </summary>
    public partial class StringColumn : BaseColumn
    {
        private List<List<string>> _stringBuffers = new List<List<string>>(); // To store more than intMax number of strings

        public StringColumn(string name, long length) : base(name, length, typeof(string))
        {
            _stringBuffers.Add(new List<string>());
        }

        public StringColumn(string name, IEnumerable<string> values) : base(name, 0, typeof(string)) {
            values = values ?? throw new ArgumentNullException(nameof(values));
            if (_stringBuffers.Count == 0)
            {
                _stringBuffers.Add(new List<string>());
            }
            foreach (var value in values)
            {
                Append(value);
            }
        }

        public void Append(string value)
        {
            var lastBuffer = _stringBuffers[_stringBuffers.Count - 1];
            if (lastBuffer.Count == int.MaxValue)
            {
                lastBuffer = new List<string>();
                _stringBuffers.Add(lastBuffer);
            }
            lastBuffer.Add(value);
            Length++;
        }
        private int GetBufferIndexContainingRowIndex(ref long rowIndex)
        {
            if (rowIndex > Length)
            {
                throw new ArgumentOutOfRangeException(strings.ColumnIndexOutOfRange, nameof(rowIndex));
            }
            int curArrayIndex = 0;
            int numBuffers = _stringBuffers.Count;
            while (curArrayIndex < numBuffers && rowIndex > _stringBuffers[curArrayIndex].Count)
            {
                rowIndex -= _stringBuffers[curArrayIndex].Count;
                curArrayIndex++;
            }
            return curArrayIndex;
        }

        public override object this[long rowIndex]
        {
            get
            {
                int bufferIndex = GetBufferIndexContainingRowIndex(ref rowIndex);
                return _stringBuffers[bufferIndex][(int)rowIndex];
            }
            set
            {
                if (value is string)
                {
                    int bufferIndex = GetBufferIndexContainingRowIndex(ref rowIndex);
                    _stringBuffers[bufferIndex][(int)rowIndex] = (string)value;
                }
                else
                {
                    throw new ArgumentException("Expected value to be a string", nameof(value));
                }
            }
        }

        public override object this[long startIndex, int length]
        {
            get
            {
                var ret = new List<string>();
                int bufferIndex = GetBufferIndexContainingRowIndex(ref startIndex);
                while (ret.Count < length && bufferIndex < _stringBuffers.Count)
                {
                    for (int i = (int)startIndex; ret.Count < length && i < _stringBuffers[bufferIndex].Count; i++)
                    {
                        ret.Add(_stringBuffers[bufferIndex][i]);
                    }
                    bufferIndex++;
                    startIndex = 0;
                }
                return ret;
            }
        }
        public override BaseColumn Sort(bool ascending = true)
        {
            PrimitiveColumn<long> columnSortIndices = GetAscendingSortIndices() as PrimitiveColumn<long>;
            return Clone(columnSortIndices, !ascending);
        }

        public override BaseColumn GetAscendingSortIndices()
        {
            _GetSortIndices(Comparer<string>.Default, out PrimitiveColumn<long> columnSortIndices);
            return columnSortIndices;

        }
        private void _GetSortIndices(Comparer<string> comparer, out PrimitiveColumn<long> columnSortIndices)
        {
            List<int[]> bufferSortIndices = new List<int[]>(_stringBuffers.Count);
            foreach (var buffer in _stringBuffers)
            {
                var sortIndices = new int[buffer.Count];
                for (int i = 0; i < buffer.Count; i++) sortIndices[i] = i;
                // TODO: Refactor the sort routine to also work with IList?
                string[] array = buffer.ToArray();
                IntrospectiveSort(array, array.Length, sortIndices, comparer);
                bufferSortIndices.Add(sortIndices);
            }
            // Simple merge sort to build the full column's sort indices
            SortedDictionary<string, List<Tuple<int, int>>> heapOfValueAndListOfTupleOfSortAndBufferIndex = new SortedDictionary<string, List<Tuple<int, int>>>(comparer);
            var buffers = _stringBuffers;
            for (int i = 0; i < buffers.Count; i++)
            {
                List<string> buffer = buffers[i];
                string value = buffer[bufferSortIndices[i][0]];
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
            GetValueAtBuffer<string> getValueAtBuffer = new GetValueAtBuffer<string>((int bufferIndex, int sortIndex) => _stringBuffers[bufferIndex][bufferSortIndices[bufferIndex][sortIndex]]);
            GetBufferLengthAtIndex getBufferLengthAtIndex = new GetBufferLengthAtIndex((int bufferIndex) => bufferSortIndices[bufferIndex].Length);
            PopulateColumnSortIndicesWithHeap(heapOfValueAndListOfTupleOfSortAndBufferIndex, columnSortIndices, getBufferSortIndex, getValueAtBuffer, getBufferLengthAtIndex);
        }

        public override BaseColumn Clone(BaseColumn mapIndices = null, bool invertMapIndices = false)
        {
            if (!(mapIndices is null))
            {
                if (mapIndices.DataType != typeof(long)) throw new ArgumentException($"Expected sortIndices to be a PrimitiveColumn<long>");
                if (mapIndices.Length != Length) throw new ArgumentException(strings.MismatchedColumnLengths, nameof(mapIndices));
                return _Clone(mapIndices as PrimitiveColumn<long>, invertMapIndices);
            }
            return _Clone();
        }

        public StringColumn _Clone(PrimitiveColumn<long> mapIndices = null, bool invertMapIndex = false)
        {
            StringColumn ret = new StringColumn(Name, Length);
            if (mapIndices is null)
            {
                // The runtime can potentially optimize this branch? If not, just get rid of this and use the indexers
                for (int i = 0; i < _stringBuffers.Count; i++)
                {
                    List<string> buffer = _stringBuffers[i];
                    List<string> newBuffer = new List<string>(buffer.Count);
                    ret._stringBuffers.Add(newBuffer);
                    int bufferLen = buffer.Count;
                    for (int j = 0; j < bufferLen; j++)
                    {
                        newBuffer.Add(buffer[j]);
                    }
                }
            }
            else
            {
                if (mapIndices.Length != Length) throw new ArgumentException(strings.MismatchedColumnLengths, nameof(mapIndices));
                if (invertMapIndex == false)
                {
                    for (long i = 0; i < mapIndices.Length; i++)
                    {
                        ret.Append((string)this[(long)mapIndices[i]]);
                    }
                }
                else
                {
                    for (long i = Length - 1; i >= 0; i--)
                    {
                        ret.Append((string)this[(long)mapIndices[i]]);
                    }
                }
            }
            return ret;
        }
    }
}
