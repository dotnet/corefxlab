// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.ML;
using Microsoft.ML.Data;

namespace Microsoft.Data
{
    /// <summary>
    /// A mutable column to hold strings
    /// </summary>
    /// <remarks> Is NOT Arrow compatible </remarks>
    public partial class StringColumn : BaseColumn, IEnumerable<string>
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

        public override void Resize(long length)
        {
            if (length < Length)
                throw new ArgumentException(Strings.CannotResizeDown, nameof(length));

            for (long i = Length; i < length; i++)
            {
                Append(null);
            }
        }

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

        public IEnumerator<string> GetEnumerator()
        {
            foreach (List<string> buffer in _stringBuffers)
            {
                foreach (string value in buffer)
                {
                    yield return value;
                }
            }
        }

        protected override IEnumerator GetEnumeratorCore() => GetEnumerator();

        public override BaseColumn Clip<U>(U lower, U upper) => throw new NotSupportedException();

        public override BaseColumn Sort(bool ascending = true)
        {
            PrimitiveColumn<long> columnSortIndices = GetAscendingSortIndices() as PrimitiveColumn<long>;
            return Clone(columnSortIndices, !ascending, NullCount);
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
            ValueTuple<string, int> GetFirstNonNullValueStartingAtIndex(int stringBufferIndex, int startIndex)
            {
                string value = _stringBuffers[stringBufferIndex][bufferSortIndices[stringBufferIndex][startIndex]];
                while (value == null && ++startIndex < bufferSortIndices[stringBufferIndex].Length)
                {
                    value = _stringBuffers[stringBufferIndex][bufferSortIndices[stringBufferIndex][startIndex]];
                }
                return (value, startIndex);
            }

            SortedDictionary<string, List<ValueTuple<int, int>>> heapOfValueAndListOfTupleOfSortAndBufferIndex = new SortedDictionary<string, List<ValueTuple<int, int>>>(comparer);
            List<List<string>> buffers = _stringBuffers;
            for (int i = 0; i < buffers.Count; i++)
            {
                List<string> buffer = buffers[i];
                ValueTuple<string, int> valueAndBufferSortIndex = GetFirstNonNullValueStartingAtIndex(i, 0);
                if (valueAndBufferSortIndex.Item1 == null)
                {
                    // All nulls
                    continue;
                }
                if (heapOfValueAndListOfTupleOfSortAndBufferIndex.ContainsKey(valueAndBufferSortIndex.Item1))
                {
                    heapOfValueAndListOfTupleOfSortAndBufferIndex[valueAndBufferSortIndex.Item1].Add((valueAndBufferSortIndex.Item2, i));
                }
                else
                {
                    heapOfValueAndListOfTupleOfSortAndBufferIndex.Add(valueAndBufferSortIndex.Item1, new List<ValueTuple<int, int>>() { (valueAndBufferSortIndex.Item2, i) });
                }
            }
            columnSortIndices = new PrimitiveColumn<long>("SortIndices");
            GetBufferSortIndex getBufferSortIndex = new GetBufferSortIndex((int bufferIndex, int sortIndex) => (bufferSortIndices[bufferIndex][sortIndex]) + bufferIndex * bufferSortIndices[0].Length);
            GetValueAndBufferSortIndexAtBuffer<string> getValueAtBuffer = new GetValueAndBufferSortIndexAtBuffer<string>((int bufferIndex, int sortIndex) => GetFirstNonNullValueStartingAtIndex(bufferIndex, sortIndex));
            GetBufferLengthAtIndex getBufferLengthAtIndex = new GetBufferLengthAtIndex((int bufferIndex) => bufferSortIndices[bufferIndex].Length);
            PopulateColumnSortIndicesWithHeap(heapOfValueAndListOfTupleOfSortAndBufferIndex, columnSortIndices, getBufferSortIndex, getValueAtBuffer, getBufferLengthAtIndex);
        }

        public override BaseColumn Clone(BaseColumn mapIndices = null, bool invertMapIndices = false, long numberOfNullsToAppend = 0)
        {
            StringColumn clone;
            if (!(mapIndices is null))
            {
                Type dataType = mapIndices.DataType;
                if (dataType != typeof(long) && dataType != typeof(int) && dataType != typeof(bool))
                    throw new ArgumentException(String.Format(Strings.MultipleMismatchedValueType, typeof(long), typeof(int), typeof(bool)), nameof(mapIndices));
                if (mapIndices.DataType == typeof(long))
                    clone = Clone(mapIndices as PrimitiveColumn<long>, invertMapIndices);
                else if (dataType == typeof(int))
                    clone = Clone(mapIndices as PrimitiveColumn<int>, invertMapIndices);
                else
                    clone = Clone(mapIndices as PrimitiveColumn<bool>);
            }
            else
            {
                clone = Clone();
            }
            for (long i = 0; i < numberOfNullsToAppend; i++)
            {
                clone.Append(null);
            }
            return clone;
        }

        private StringColumn Clone(PrimitiveColumn<bool> boolColumn)
        {
            if (boolColumn.Length > Length)
                throw new ArgumentException(Strings.MapIndicesExceedsColumnLenth, nameof(boolColumn));
            StringColumn ret = new StringColumn(Name, 0);
            for (long i = 0; i < boolColumn.Length; i++)
            {
                bool? value = boolColumn[i];
                if (value.HasValue && value.Value == true)
                    ret.Append(this[i]);
            }
            return ret;
        }

        private StringColumn CloneImplementation<U>(PrimitiveColumn<U> mapIndices, bool invertMapIndices = false)
            where U : unmanaged
        {
            mapIndices = mapIndices ?? throw new ArgumentNullException(nameof(mapIndices));
            StringColumn ret = new StringColumn(Name, mapIndices.Length);

            List<string> setBuffer = ret._stringBuffers[0];
            long setBufferMinRange = 0;
            long setBufferMaxRange = int.MaxValue;
            List<string> getBuffer = _stringBuffers[0];
            long getBufferMinRange = 0;
            long getBufferMaxRange = int.MaxValue;
            long maxCapacity = int.MaxValue;
            if (mapIndices.DataType == typeof(long))
            {
                PrimitiveColumn<long> longMapIndices = mapIndices as PrimitiveColumn<long>;
                longMapIndices.ApplyElementwise((long? mapIndex, long rowIndex) =>
                {
                    long index = rowIndex;
                    if (invertMapIndices)
                        index = longMapIndices.Length - 1 - index;
                    if (index < setBufferMinRange || index >= setBufferMaxRange)
                    {
                        int bufferIndex = (int)(index / maxCapacity);
                        setBuffer = ret._stringBuffers[bufferIndex];
                        setBufferMinRange = bufferIndex * maxCapacity;
                        setBufferMaxRange = (bufferIndex + 1) * maxCapacity;
                    }
                    index -= setBufferMinRange;
                    if (mapIndex == null)
                    {
                        setBuffer[(int)index] = null;
                        ret._nullCount++;
                        return mapIndex;
                    }

                    if (mapIndex.Value < getBufferMinRange || mapIndex.Value >= getBufferMaxRange)
                    {
                        int bufferIndex = (int)(mapIndex.Value / maxCapacity);
                        getBuffer = _stringBuffers[bufferIndex];
                        getBufferMinRange = bufferIndex * maxCapacity;
                        getBufferMaxRange = (bufferIndex + 1) * maxCapacity;
                    }
                    int bufferLocalMapIndex = (int)(mapIndex - getBufferMinRange);
                    setBuffer[(int)index] = getBuffer[bufferLocalMapIndex];
                    return mapIndex;
                });
            }
            else if (mapIndices.DataType == typeof(int))
            {
                PrimitiveColumn<int> intMapIndices = mapIndices as PrimitiveColumn<int>;
                intMapIndices.ApplyElementwise((int? mapIndex, long rowIndex) =>
                {
                    long index = rowIndex;
                    if (invertMapIndices)
                        index = intMapIndices.Length - 1 - index;

                    if (mapIndex == null)
                    {
                        setBuffer[(int)index] = null;
                        ret._nullCount++;
                        return mapIndex;
                    }
                    setBuffer[(int)index] = getBuffer[mapIndex.Value];
                    return mapIndex;
                });
            }
            else
            {
                Debug.Assert(false, nameof(mapIndices.DataType));
            }

            return ret;
        }

        private StringColumn Clone(PrimitiveColumn<long> mapIndices = null, bool invertMapIndex = false)
        {
            if (mapIndices is null)
            {
                StringColumn ret = new StringColumn(Name, mapIndices is null ? Length : mapIndices.Length);
                for (long i = 0; i < Length; i++)
                {
                    ret[i] = this[i];
                }
                return ret;
            }
            else
            {
                return CloneImplementation(mapIndices, invertMapIndex);
            }
        }

        private StringColumn Clone(PrimitiveColumn<int> mapIndices, bool invertMapIndex = false)
        {
            long? ConvertInt(long index)
            {
                return mapIndices[index];
            }
            return CloneImplementation(mapIndices, invertMapIndex);
        }

        internal static DataFrame ValueCountsImplementation(Dictionary<string, ICollection<long>> groupedValues)
        {
            StringColumn keys = new StringColumn("Values", 0);
            PrimitiveColumn<long> counts = new PrimitiveColumn<long>("Counts");
            foreach (KeyValuePair<string, ICollection<long>> keyValuePair in groupedValues)
            {
                keys.Append(keyValuePair.Key);
                counts.Append(keyValuePair.Value.Count);
            }
            return new DataFrame(new List<BaseColumn> { keys, counts });
        }

        public override DataFrame ValueCounts()
        {
            Dictionary<string, ICollection<long>> groupedValues = GroupColumnValues<string>();
            return ValueCountsImplementation(groupedValues);
        }

        public override GroupBy GroupBy(int columnIndex, DataFrame parent)
        {
            Dictionary<string, ICollection<long>> dictionary = GroupColumnValues<string>();
            return new GroupBy<string>(parent, columnIndex, dictionary);
        }

        public override Dictionary<TKey, ICollection<long>> GroupColumnValues<TKey>()
        {
            if (typeof(TKey) == typeof(string))
            {
                Dictionary<string, ICollection<long>> multimap = new Dictionary<string, ICollection<long>>(EqualityComparer<string>.Default);
                for (long i = 0; i < Length; i++)
                {
                    bool containsKey = multimap.TryGetValue(this[i] ?? default, out ICollection<long> values);
                    if (containsKey)
                    {
                        values.Add(i);
                    }
                    else
                    {
                        multimap.Add(this[i] ?? default, new List<long>() { i });
                    }
                }
                return multimap as Dictionary<TKey, ICollection<long>>;
            }
            else
            {
                throw new NotImplementedException(nameof(TKey));
            }
        }

        public override BaseColumn FillNulls(object value, bool inPlace = false)
        {
            if (value.GetType() != typeof(string) || value == null)
                throw new ArgumentException(nameof(value));
            string stringValue = (string)value;
            StringColumn column;
            if (inPlace)
                column = this;
            else
                column = Clone();

            for (long i = 0; i < Length; i++)
            {
                if (this[i] == null)
                    this[i] = stringValue;
            }
            return column;
        }

        protected internal override void AddDataViewColumn(DataViewSchema.Builder builder)
        {
            builder.AddColumn(Name, TextDataViewType.Instance);
        }

        protected internal override Delegate GetDataViewGetter(DataViewRowCursor cursor)
        {
            return CreateValueGetterDelegate(cursor);
        }

        private ValueGetter<ReadOnlyMemory<char>> CreateValueGetterDelegate(DataViewRowCursor cursor) =>
            (ref ReadOnlyMemory<char> value) => value = this[cursor.Position].AsMemory();
    }
}
