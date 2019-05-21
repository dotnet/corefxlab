// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;

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
            int numberOfBuffersRequired = Math.Max((int)(length / int.MaxValue), 1);
            for (int i = 0; i < numberOfBuffersRequired; i++)
            {
                long bufferLen = length - _stringBuffers.Count * int.MaxValue;
                List<string> buffer = new List<string>((int)Math.Min(int.MaxValue, bufferLen));
                _stringBuffers.Add(buffer);
                for (int j = 0; j < bufferLen; j++)
                {
                    buffer.Add(default);
                }
            }
        }

        public StringColumn(string name, IEnumerable<string> values) : base(name, 0, typeof(string))
        {
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

        private long _nullCount;
        public override long NullCount => _nullCount;

        public void Append(string value)
        {
            List<string> lastBuffer = _stringBuffers[_stringBuffers.Count - 1];
            if (lastBuffer.Count == int.MaxValue)
            {
                lastBuffer = new List<string>();
                _stringBuffers.Add(lastBuffer);
            }
            lastBuffer.Add(value);
            if (value == null)
                _nullCount++;
            Length++;
        }
        private int GetBufferIndexContainingRowIndex(ref long rowIndex)
        {
            if (rowIndex > Length)
            {
                throw new ArgumentOutOfRangeException(Strings.ColumnIndexOutOfRange, nameof(rowIndex));
            }
            return (int)(rowIndex / int.MaxValue);
        }

        protected override object GetValue(long rowIndex)
        {
            int bufferIndex = GetBufferIndexContainingRowIndex(ref rowIndex);
            return _stringBuffers[bufferIndex][(int)rowIndex];
        }

        protected override object GetValue(long startIndex, int length)
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

        protected override void SetValue(long rowIndex, object value)
        {
            if (value == null || value is string)
            {
                int bufferIndex = GetBufferIndexContainingRowIndex(ref rowIndex);
                var oldValue = this[rowIndex];
                _stringBuffers[bufferIndex][(int)rowIndex] = (string)value;
                if (oldValue != (string)value)
                {
                    if (value == null)
                        _nullCount++;
                    if (oldValue == null && _nullCount > 0)
                        _nullCount--;
                }
            }
            else
            {
                throw new ArgumentException(Strings.MismatchedValueType + " string", nameof(value));
            }
        }

        public new string this[long rowIndex]
        {
            get => (string)GetValue(rowIndex);
            set => SetValue(rowIndex, value);
        }

        public new List<string> this[long startIndex, int length]
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
            return CloneAndAppendNulls((BaseColumn)columnSortIndices, !ascending);
        }

        internal override BaseColumn GetAscendingSortIndices()
        {
            GetSortIndices(Comparer<string>.Default, out PrimitiveColumn<long> columnSortIndices);
            return columnSortIndices;
        }

        private void GetSortIndices(Comparer<string> comparer, out PrimitiveColumn<long> columnSortIndices)
        {
            List<int[]> bufferSortIndices = new List<int[]>(_stringBuffers.Count);
            foreach (List<string> buffer in _stringBuffers)
            {
                var sortIndices = new int[buffer.Count];
                for (int i = 0; i < buffer.Count; i++)
                {
                    sortIndices[i] = i;
                }
                // TODO: Refactor the sort routine to also work with IList?
                string[] array = buffer.ToArray();
                IntrospectiveSort(array, array.Length, sortIndices, comparer);
                bufferSortIndices.Add(sortIndices);
            }
            // Simple merge sort to build the full column's sort indices
            Tuple<string, int> GetFirstNonNullValueStartingAtIndex(int stringBufferIndex, int startIndex)
            {
                string value = _stringBuffers[stringBufferIndex][bufferSortIndices[stringBufferIndex][startIndex]];
                while (value == null && ++startIndex < bufferSortIndices[stringBufferIndex].Length)
                {
                    value = _stringBuffers[stringBufferIndex][bufferSortIndices[stringBufferIndex][startIndex]];
                }
                return new Tuple<string, int>(value, startIndex);
            }

            SortedDictionary<string, List<Tuple<int, int>>> heapOfValueAndListOfTupleOfSortAndBufferIndex = new SortedDictionary<string, List<Tuple<int, int>>>(comparer);
            List<List<string>> buffers = _stringBuffers;
            for (int i = 0; i < buffers.Count; i++)
            {
                List<string> buffer = buffers[i];
                Tuple<string, int> valueAndBufferSortIndex = GetFirstNonNullValueStartingAtIndex(i, 0);
                if (valueAndBufferSortIndex.Item1 == null)
                {
                    // All nulls
                    continue;
                }
                if (heapOfValueAndListOfTupleOfSortAndBufferIndex.ContainsKey(valueAndBufferSortIndex.Item1))
                {
                    heapOfValueAndListOfTupleOfSortAndBufferIndex[valueAndBufferSortIndex.Item1].Add(new Tuple<int, int>(valueAndBufferSortIndex.Item2, i));
                }
                else
                {
                    heapOfValueAndListOfTupleOfSortAndBufferIndex.Add(valueAndBufferSortIndex.Item1, new List<Tuple<int, int>>() { new Tuple<int, int>(valueAndBufferSortIndex.Item2, i) });
                }
            }
            columnSortIndices = new PrimitiveColumn<long>("SortIndices");
            GetBufferSortIndex getBufferSortIndex = new GetBufferSortIndex((int bufferIndex, int sortIndex) => (bufferSortIndices[bufferIndex][sortIndex]) + bufferIndex * bufferSortIndices[0].Length);
            GetValueAndBufferSortIndexAtBuffer<string> getValueAtBuffer = new GetValueAndBufferSortIndexAtBuffer<string>((int bufferIndex, int sortIndex) => GetFirstNonNullValueStartingAtIndex(bufferIndex, sortIndex));
            GetBufferLengthAtIndex getBufferLengthAtIndex = new GetBufferLengthAtIndex((int bufferIndex) => bufferSortIndices[bufferIndex].Length);
            PopulateColumnSortIndicesWithHeap(heapOfValueAndListOfTupleOfSortAndBufferIndex, columnSortIndices, getBufferSortIndex, getValueAtBuffer, getBufferLengthAtIndex);
        }

        public override BaseColumn Clone(BaseColumn mapIndices = null, bool invertMapIndices = false)
        {
            if (!(mapIndices is null))
            {
                if (mapIndices.DataType != typeof(long))
                    throw new ArgumentException(Strings.MismatchedValueType + " PrimitiveColumn<long>", nameof(mapIndices));
                if (mapIndices.Length >= Length)
                    throw new ArgumentException(Strings.MismatchedColumnLengths, nameof(mapIndices));
                return Clone(mapIndices as PrimitiveColumn<long>, invertMapIndices);
            }
            return Clone();
        }

        internal override BaseColumn CloneAndAppendNulls(BaseColumn mapIndices = null, bool invertMapIndices = false)
        {
            StringColumn ret = Clone(mapIndices, invertMapIndices) as StringColumn;
            for (long i = 0; i < NullCount; i++)
            {
                ret.Append(null);
            }
            return ret;
        }

        private StringColumn Clone(PrimitiveColumn<long> mapIndices = null, bool invertMapIndex = false)
        {
            StringColumn ret = new StringColumn(Name, mapIndices is null ? Length : mapIndices.Length);
            if (mapIndices is null)
            {
                for (long i = 0; i < Length; i++)
                {
                    ret[i] = this[i];
                }
            }
            else
            {
                if (mapIndices.Length >= Length)
                    throw new ArgumentException(Strings.MismatchedColumnLengths, nameof(mapIndices));
                if (invertMapIndex == false)
                {
                    for (long i = 0; i < mapIndices.Length; i++)
                    {
                        ret[i] = this[(long)mapIndices[i]];
                    }
                }
                else
                {
                    long mapIndicesLengthIndex = mapIndices.Length - 1;
                    for (long i = 0; i < mapIndices.Length; i++)
                    {
                        ret[i] = this[(long)mapIndices[mapIndicesLengthIndex - i]];
                    }
                }
            }
            return ret;
        }
    }
}
