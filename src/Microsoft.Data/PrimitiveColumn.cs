// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Apache.Arrow;
using Apache.Arrow.Types;
using Microsoft.ML;
using Microsoft.ML.Data;

namespace Microsoft.Data
{
    /// <summary>
    /// A column to hold primitive types such as int, float etc.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public partial class PrimitiveColumn<T> : BaseColumn, IEnumerable<ReadOnlyMemory<T>>
        where T : unmanaged
    {
        private PrimitiveColumnContainer<T> _columnContainer;

        internal PrimitiveColumn(string name, PrimitiveColumnContainer<T> column) : base(name, column.Length, typeof(T))
        {
            _columnContainer = column;
        }

        public PrimitiveColumn(string name, IEnumerable<T> values) : base(name, 0, typeof(T))
        {
            _columnContainer = new PrimitiveColumnContainer<T>(values);
            Length = _columnContainer.Length;
        }

        public PrimitiveColumn(string name, long length = 0) : base(name, length, typeof(T))
        {
            _columnContainer = new PrimitiveColumnContainer<T>(length);
        }

        public PrimitiveColumn(string name, ReadOnlyMemory<byte> buffer, ReadOnlyMemory<byte> nullBitMap, int length = 0, int nullCount = 0) : base(name, length, typeof(T))
        {
            _columnContainer = new PrimitiveColumnContainer<T>(buffer, nullBitMap, length, nullCount);
        }

        private IArrowType GetArrowType()
        {
            if (typeof(T) == typeof(bool))
                return BooleanType.Default;
            else if (typeof(T) == typeof(double))
                return DoubleType.Default;
            else if (typeof(T) == typeof(float))
                return FloatType.Default;
            else if (typeof(T) == typeof(sbyte))
                return Int8Type.Default;
            else if (typeof(T) == typeof(int))
                return Int32Type.Default;
            else if (typeof(T) == typeof(long))
                return Int64Type.Default;
            else if (typeof(T) == typeof(short))
                return Int16Type.Default;
            else if (typeof(T) == typeof(byte))
                return UInt8Type.Default;
            else if (typeof(T) == typeof(uint))
                return UInt32Type.Default;
            else if (typeof(T) == typeof(ulong))
                return UInt64Type.Default;
            else if (typeof(T) == typeof(ushort))
                return UInt16Type.Default;
            else
                throw new NotImplementedException(nameof(T));
        }

        protected internal override Field Field() => new Field(Name, GetArrowType(), NullCount != 0);

        protected internal override int MaxRecordBatchLength(long startIndex) => _columnContainer.MaxRecordBatchLength(startIndex);

        private int GetNullCount(long startIndex, int numberOfRows)
        {
            int nullCount = 0;
            for (long i = startIndex; i < numberOfRows; i++)
            {
                if (!IsValid(i))
                    nullCount++;
            }
            return nullCount;
        }

        protected internal override Apache.Arrow.Array AsArrowArray(long startIndex, int numberOfRows)
        {
            int arrayIndex = numberOfRows == 0 ? 0 : _columnContainer.GetArrayContainingRowIndex(startIndex);
            int offset = (int)(startIndex - arrayIndex * ReadOnlyDataFrameBuffer<T>.MaxCapacity);
            if (numberOfRows != 0 && numberOfRows > _columnContainer.Buffers[arrayIndex].Length - offset)
            {
                throw new ArgumentException(Strings.SpansMultipleBuffers, nameof(numberOfRows));
            }
            ArrowBuffer valueBuffer = numberOfRows == 0 ? ArrowBuffer.Empty : new ArrowBuffer(_columnContainer.GetValueBuffer(startIndex));
            ArrowBuffer nullBuffer = numberOfRows == 0 ? ArrowBuffer.Empty : new ArrowBuffer(_columnContainer.GetNullBuffer(startIndex));
            int nullCount = GetNullCount(startIndex, numberOfRows);
            Type type = this.DataType;
            if (type == typeof(bool))
                return new BooleanArray(valueBuffer, nullBuffer, numberOfRows, nullCount, offset);
            else if (type == typeof(double))
                return new DoubleArray(valueBuffer, nullBuffer, numberOfRows, nullCount, offset);
            else if (type == typeof(float))
                return new FloatArray(valueBuffer, nullBuffer, numberOfRows, nullCount, offset);
            else if (type == typeof(int))
                return new Int32Array(valueBuffer, nullBuffer, numberOfRows, nullCount, offset);
            else if (type == typeof(long))
                return new Int64Array(valueBuffer, nullBuffer, numberOfRows, nullCount, offset);
            else if (type == typeof(sbyte))
                return new Int8Array(valueBuffer, nullBuffer, numberOfRows, nullCount, offset);
            else if (type == typeof(short))
                return new Int16Array(valueBuffer, nullBuffer, numberOfRows, nullCount, offset);
            else if (type == typeof(uint))
                return new UInt32Array(valueBuffer, nullBuffer, numberOfRows, nullCount, offset);
            else if (type == typeof(ulong))
                return new UInt64Array(valueBuffer, nullBuffer, numberOfRows, nullCount, offset);
            else if (type == typeof(ushort))
                return new UInt16Array(valueBuffer, nullBuffer, numberOfRows, nullCount, offset);
            else if (type == typeof(byte))
                return new UInt8Array(valueBuffer, nullBuffer, numberOfRows, nullCount, offset);
            else
                throw new NotImplementedException(type.ToString());
        }

        public new IList<T?> this[long startIndex, int length]
        {
            get
            {
                if (startIndex > Length)
                {
                    throw new ArgumentOutOfRangeException(nameof(startIndex));
                }
                return _columnContainer[startIndex, length];
            }
        }

        protected override object GetValue(long startIndex, int length)
        {
            if (startIndex > Length)
            {
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            }
            return _columnContainer[startIndex, length];
        }

        internal T? GetTypedValue(long rowIndex) => _columnContainer[rowIndex];

        protected override object GetValue(long rowIndex) => GetTypedValue(rowIndex);

        protected override void SetValue(long rowIndex, object value)
        {
            if (value == null || value.GetType() == typeof(T))
            {
                _columnContainer[rowIndex] = (T?)value;
            }
            else
            {
                throw new ArgumentException(Strings.MismatchedValueType + $" {DataType.ToString()}", nameof(value));
            }
        }

        public new T? this[long rowIndex]
        {
            get => GetTypedValue(rowIndex);
            set
            {
                if (value == null || value.GetType() == typeof(T))
                {
                    _columnContainer[rowIndex] = value;
                }
                else
                {
                    throw new ArgumentException(Strings.MismatchedValueType + $" {DataType.ToString()}", nameof(value));
                }
            }
        }

        public override double Median()
        {
            // Not the most efficient implementation. Using a selection algorithm here would be O(n) instead of O(nLogn)
            if (Length == 0)
                return 0;
            PrimitiveColumn<long> sortIndices = GetAscendingSortIndices() as PrimitiveColumn<long>;
            long middle = sortIndices.Length / 2;
            double middleValue = (double)Convert.ChangeType(this[sortIndices[middle].Value].Value, typeof(double));
            if (Length % 2 == 0)
            {
                double otherMiddleValue = (double)Convert.ChangeType(this[sortIndices[middle - 1].Value].Value, typeof(double));
                return (middleValue + otherMiddleValue) / 2;
            }
            else
            {
                return middleValue;
            }
        }

        public override double Mean()
        {
            if (Length == 0)
                return 0;
            return (double)Convert.ChangeType((T)Sum(), typeof(double)) / Length;
        }

        public override void Resize(long length)
        {
            _columnContainer.Resize(length);
            Length = _columnContainer.Length;
        }

        public void Append(T? value)
        {
            _columnContainer.Append(value);
            Length++;
        }

        public void AppendMany(T? value, long count)
        {
            _columnContainer.AppendMany(value, count);
            Length += count;
        }

        public override long NullCount
        {
            get
            {
                Debug.Assert(_columnContainer.NullCount >= 0);
                return _columnContainer.NullCount;
            }
        }

        public bool IsValid(long index) => _columnContainer.IsValid(index);

        public IEnumerator<ReadOnlyMemory<T>> GetEnumerator() => _columnContainer.GetEnumerator();

        protected override IEnumerator GetEnumeratorCore() => GetEnumerator();

        public override bool IsNumericColumn()
        {
            bool ret = true;
            if (typeof(T) == typeof(char) || typeof(T) == typeof(bool))
                ret = false;
            return ret;
        }

        public override BaseColumn FillNulls(object value, bool inPlace = false)
        {
            T Tvalue = (T)Convert.ChangeType(value, typeof(T));
            PrimitiveColumn<T> column;
            if (inPlace)
                column = this;
            else
                column = Clone();
            column.ApplyElementwise((T? columnValue, long index) =>
            {
                if (columnValue.HasValue == false)
                    return Tvalue;
                else
                    return columnValue.Value;
            });
            return column;
        }

        public override DataFrame ValueCounts()
        {
            Dictionary<T, ICollection<long>> groupedValues = GroupColumnValues<T>();
            PrimitiveColumn<T> keys = new PrimitiveColumn<T>("Values");
            PrimitiveColumn<long> counts = new PrimitiveColumn<long>("Counts");
            foreach (KeyValuePair<T, ICollection<long>> keyValuePair in groupedValues)
            {
                keys.Append(keyValuePair.Key);
                counts.Append(keyValuePair.Value.Count);
            }
            return new DataFrame(new List<BaseColumn> { keys, counts });
        }

        public override bool HasDescription() => IsNumericColumn();

        public override string ToString()
        {
            return $"{Name}: {_columnContainer.ToString()}";
        }

        public override BaseColumn Clone(BaseColumn mapIndices = null, bool invertMapIndices = false, long numberOfNullsToAppend = 0)
        {
            PrimitiveColumn<T> clone;
            if (!(mapIndices is null))
            {
                Type dataType = mapIndices.DataType;
                if (dataType != typeof(long) && dataType != typeof(int) && dataType != typeof(bool))
                    throw new ArgumentException(String.Format(Strings.MultipleMismatchedValueType, typeof(long), typeof(int), typeof(bool)), nameof(mapIndices));
                if (mapIndices.Length > Length)
                    throw new ArgumentException(Strings.MapIndicesExceedsColumnLenth, nameof(mapIndices));
                if (dataType == typeof(long))
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
            Debug.Assert(!ReferenceEquals(clone, null));
            clone.AppendMany(null, numberOfNullsToAppend);
            return clone;
        }

        private PrimitiveColumn<T> Clone(PrimitiveColumn<bool> boolColumn)
        {
            if (boolColumn.Length > Length)
                throw new ArgumentException(Strings.MapIndicesExceedsColumnLenth, nameof(boolColumn));
            PrimitiveColumn<T> ret = new PrimitiveColumn<T>(Name);
            for (long i = 0; i < boolColumn.Length; i++)
            {
                bool? value = boolColumn[i];
                if (value.HasValue && value.Value == true)
                    ret.Append(this[i]);
            }
            return ret;
        }

        private PrimitiveColumn<T> CloneImplementation(BaseColumn mapIndices, Func<long, long?> getIndex, bool invertMapIndices = false)
        {
            if (!mapIndices.IsNumericColumn())
                throw new ArgumentException(String.Format(Strings.MismatchedValueType, Strings.NumericColumnType), nameof(mapIndices));

            if (mapIndices.Length > Length)
                throw new ArgumentException(Strings.MapIndicesExceedsColumnLenth, nameof(mapIndices));

            PrimitiveColumn<T> ret = new PrimitiveColumn<T>(Name, mapIndices.Length);
            ret._columnContainer._modifyNullCountWhileIndexing = false;
            if (invertMapIndices == false)
            {
                for (long i = 0; i < mapIndices.Length; i++)
                {
                    T? value = _columnContainer[getIndex(i).Value];
                    ret[i] = value;
                    if (!value.HasValue)
                        ret._columnContainer.NullCount++;
                }
            }
            else
            {
                long mapIndicesIndex = mapIndices.Length - 1;
                for (long i = 0; i < mapIndices.Length; i++)
                {
                    T? value = _columnContainer[getIndex(mapIndicesIndex - i).Value];
                    ret[i] = value;
                    if (!value.HasValue)
                        ret._columnContainer.NullCount++;
                }
            }
            ret._columnContainer._modifyNullCountWhileIndexing = true;
            return ret;
        }

        public PrimitiveColumn<T> Clone(PrimitiveColumn<long> mapIndices = null, bool invertMapIndices = false)
        {
            if (mapIndices is null)
            {
                PrimitiveColumnContainer<T> newColumnContainer = _columnContainer.Clone();
                return new PrimitiveColumn<T>(Name, newColumnContainer);
            }
            else
            {
                return CloneImplementation(mapIndices, mapIndices.GetTypedValue, invertMapIndices);
            }
        }

        public PrimitiveColumn<T> Clone(PrimitiveColumn<int> mapIndices, bool invertMapIndices = false)
        {
            long? ConvertInt(long index)
            {
                return mapIndices[index];
            }
            return CloneImplementation(mapIndices, ConvertInt, invertMapIndices);
        }

        public PrimitiveColumn<T> Clone(IEnumerable<long> mapIndices)
        {
            IEnumerator<long> rows = mapIndices.GetEnumerator();
            PrimitiveColumn<T> ret = new PrimitiveColumn<T>(Name);
            ret._columnContainer._modifyNullCountWhileIndexing = false;
            long numberOfRows = 0;
            while (rows.MoveNext() && numberOfRows < Length)
            {
                numberOfRows++;
                long curRow = rows.Current;
                T? value = _columnContainer[curRow];
                ret[curRow] = value;
                if (!value.HasValue)
                    ret._columnContainer.NullCount++;
            }
            ret._columnContainer._modifyNullCountWhileIndexing = true;
            return ret;
        }

        internal PrimitiveColumn<bool> CloneAsBoolColumn()
        {
            PrimitiveColumnContainer<bool> newColumnContainer = _columnContainer.CloneAsBoolContainer();
            return new PrimitiveColumn<bool>(Name, newColumnContainer);
        }

        internal PrimitiveColumn<double> CloneAsDoubleColumn()
        {
            PrimitiveColumnContainer<double> newColumnContainer = _columnContainer.CloneAsDoubleContainer();
            return new PrimitiveColumn<double>(Name, newColumnContainer);
        }

        internal PrimitiveColumn<decimal> CloneAsDecimalColumn()
        {
            PrimitiveColumnContainer<decimal> newColumnContainer = _columnContainer.CloneAsDecimalContainer();
            return new PrimitiveColumn<decimal>(Name, newColumnContainer);
        }

        public override GroupBy GroupBy(int columnIndex, DataFrame parent)
        {
            Dictionary<T, ICollection<long>> dictionary = GroupColumnValues<T>();
            return new GroupBy<T>(parent, columnIndex, dictionary);
        }

        public override Dictionary<TKey, ICollection<long>> GroupColumnValues<TKey>()
        {
            if (typeof(TKey) == typeof(T))
            {
                Dictionary<T, ICollection<long>> multimap = new Dictionary<T, ICollection<long>>(EqualityComparer<T>.Default);
                for (int b = 0; b < _columnContainer.Buffers.Count; b++)
                {
                    ReadOnlyDataFrameBuffer<T> buffer = _columnContainer.Buffers[b];
                    ReadOnlySpan<T> readOnlySpan = buffer.ReadOnlySpan;
                    long previousLength = b * ReadOnlyDataFrameBuffer<T>.MaxCapacity;
                    for (int i = 0; i < readOnlySpan.Length; i++)
                    {
                        long currentLength = i + previousLength;
                        bool containsKey = multimap.TryGetValue(readOnlySpan[i], out ICollection<long> values);
                        if (containsKey)
                        {
                            values.Add(currentLength);
                        }
                        else
                        {
                            multimap.Add(readOnlySpan[i], new List<long>() { currentLength });
                        }
                    }
                }
                return multimap as Dictionary<TKey, ICollection<long>>;
            }
            else
            {
                throw new NotImplementedException(nameof(TKey));
            }
        }

        public void ApplyElementwise(Func<T?, long, T?> func) => _columnContainer.ApplyElementwise(func);

        public override BaseColumn Clip<U>(U lower, U upper)
        {
            object convertedLower = Convert.ChangeType(lower, typeof(T));
            if (typeof(T) == typeof(U) || convertedLower != null)
            {
                return _Clip((T)convertedLower, (T)Convert.ChangeType(upper, typeof(T)));
            }
            else
                throw new ArgumentException(Strings.MismatchedValueType + typeof(T).ToString(), nameof(U));
        }

        public override BaseColumn Filter<U>(U lower, U upper)
        {
            object convertedLower = Convert.ChangeType(lower, typeof(T));
            if (typeof(T) == typeof(U) || convertedLower != null)
            {
                return _Filter((T)convertedLower, (T)Convert.ChangeType(upper, typeof(T)));
            }
            else
                throw new ArgumentException(Strings.MismatchedValueType + typeof(T).ToString(), nameof(U));
        }

        public override DataFrame Description()
        {
            DataFrame ret = new DataFrame();
            StringColumn stringColumn = new StringColumn("Description", 0);
            stringColumn.Append("Length");
            stringColumn.Append("Max");
            stringColumn.Append("Min");
            stringColumn.Append("Mean");
            float max = (float)Convert.ChangeType(Max(), typeof(float));
            float min = (float)Convert.ChangeType(Min(), typeof(float));
            float mean = (float)Convert.ChangeType(Sum(), typeof(float)) / Length;
            PrimitiveColumn<float> column = new PrimitiveColumn<float>(Name);
            column.Append(Length - NullCount);
            column.Append(max);
            column.Append(min);
            column.Append(mean);
            ret.InsertColumn(0, stringColumn);
            ret.InsertColumn(1, column);
            return ret;
        }

        private PrimitiveColumn<T> _Filter(T lower, T upper)
        {
            PrimitiveColumn<T> ret = new PrimitiveColumn<T>(Name);
            Comparer<T> comparer = Comparer<T>.Default;
            for (long i = 0; i < Length; i++)
            {
                T? value = this[i];
                if (value == null)
                    continue;

                if (comparer.Compare(value.Value, lower) >= 0 && comparer.Compare(value.Value, upper) <= 0)
                {
                    ret.Append(value);
                }
            }
            return ret;
        }

        private PrimitiveColumn<T> _Clip(T lower, T upper)
        {
            PrimitiveColumn<T> ret = Clone() as PrimitiveColumn<T>;
            Comparer<T> comparer = Comparer<T>.Default;
            for (long i = 0; i < Length; i++)
            {
                T? value = ret[i];
                if (value == null)
                    continue;

                if (comparer.Compare(value.Value, lower) < 0)
                {
                    ret[i] = lower;
                }
                if (comparer.Compare(value.Value, upper) > 0)
                {
                    ret[i] = upper;
                }
            }
            return ret;
        }

        protected internal override void AddDataViewColumn(DataViewSchema.Builder builder)
        {
            builder.AddColumn(Name, GetDataViewType());
        }

        private static DataViewType GetDataViewType()
        {
            if (typeof(T) == typeof(bool))
            {
                return BooleanDataViewType.Instance;
            }
            else if (typeof(T) == typeof(byte))
            {
                return NumberDataViewType.Byte;
            }
            else if (typeof(T) == typeof(double))
            {
                return NumberDataViewType.Double;
            }
            else if (typeof(T) == typeof(float))
            {
                return NumberDataViewType.Single;
            }
            else if (typeof(T) == typeof(int))
            {
                return NumberDataViewType.Int32;
            }
            else if (typeof(T) == typeof(long))
            {
                return NumberDataViewType.Int64;
            }
            else if (typeof(T) == typeof(sbyte))
            {
                return NumberDataViewType.SByte;
            }
            else if (typeof(T) == typeof(short))
            {
                return NumberDataViewType.Int16;
            }
            else if (typeof(T) == typeof(uint))
            {
                return NumberDataViewType.UInt32;
            }
            else if (typeof(T) == typeof(ulong))
            {
                return NumberDataViewType.UInt64;
            }
            else if (typeof(T) == typeof(ushort))
            {
                return NumberDataViewType.UInt16;
            }
            // The following 2 implementations are not ideal, but IDataView doesn't support
            // these types
            else if (typeof(T) == typeof(char))
            {
                return NumberDataViewType.UInt16;
            }
            else if (typeof(T) == typeof(decimal))
            {
                return NumberDataViewType.Double;
            }

            throw new NotSupportedException();
        }

        protected internal override Delegate GetDataViewGetter(DataViewRowCursor cursor)
        {
            // special cases for types not supported
            if (typeof(T) == typeof(char))
            {
                return CreateCharValueGetterDelegate(cursor);
            }
            else if (typeof(T) == typeof(decimal))
            {
                return CreateDecimalValueGetterDelegate(cursor);
            }
            return CreateValueGetterDelegate(cursor);
        }

        private ValueGetter<T> CreateValueGetterDelegate(DataViewRowCursor cursor) =>
            (ref T value) => value = this[cursor.Position].Value;

        private ValueGetter<ushort> CreateCharValueGetterDelegate(DataViewRowCursor cursor) =>
            (ref ushort value) => value = (ushort)Convert.ChangeType(this[cursor.Position].Value, typeof(ushort));

        private ValueGetter<double> CreateDecimalValueGetterDelegate(DataViewRowCursor cursor) =>
            (ref double value) => value = (double)Convert.ChangeType(this[cursor.Position].Value, typeof(double));
    }
}
