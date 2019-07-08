// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Apache.Arrow;
using Apache.Arrow.Types;
using Microsoft.Collections.Extensions;

namespace Microsoft.Data
{
    public enum JoinAlgorithm
    {
        Left,
        Right,
        FullOuter,
        Inner
    }

    /// <summary>
    /// A DataFrame to support indexing, binary operations, sorting, selection and other APIs. This will eventually also expose an IDataView for ML.NET
    /// </summary>
    public partial class DataFrame
    {
        private readonly DataFrameTable _table;
        public DataFrame()
        {
            _table = new DataFrameTable();
        }

        public DataFrame(IList<BaseColumn> columns)
        {
            _table = new DataFrameTable(columns);
        }

        public DataFrame(RecordBatch recordBatch)
        {
            _table = new DataFrameTable();
            Apache.Arrow.Schema arrowSchema = recordBatch.Schema;
            int fieldIndex = 0;
            IEnumerable<IArrowArray> arrowArrays = recordBatch.Arrays;
            foreach (IArrowArray arrowArray in arrowArrays)
            {
                Field field = arrowSchema.GetFieldByIndex(fieldIndex);
                IArrowType fieldType = field.DataType;
                BaseColumn dataFrameColumn = null;
                Memory<byte> dataFrameMemory, nullBitMapMemory;
                bool copied;
                switch (fieldType.TypeId)
                {
                    case ArrowTypeId.Boolean:
                        ReadOnlyMemory<byte> valueBuffer = (arrowArray as BooleanArray).ValueBuffer.Memory;
                        ReadOnlyMemory<byte> nullBitMapBuffer = (arrowArray as BooleanArray).NullBitmapBuffer.Memory;
                        dataFrameMemory = new Memory<byte>(new byte[valueBuffer.Length]);
                        nullBitMapMemory = new Memory<byte>(new byte[nullBitMapBuffer.Length]);
                        copied = valueBuffer.TryCopyTo(dataFrameMemory) && nullBitMapBuffer.TryCopyTo(nullBitMapMemory);
                        if (copied)
                        {
                            dataFrameColumn = new PrimitiveColumn<bool>(field.Name, dataFrameMemory, nullBitMapMemory, arrowArray.Length);
                        }
                        break;
                    case ArrowTypeId.Decimal:
                        ReadOnlyMemory<byte> decimalValueBuffer = (arrowArray as PrimitiveArray<decimal>).ValueBuffer.Memory;
                        ReadOnlyMemory<byte> decimalNullBitMapBuffer = (arrowArray as PrimitiveArray<decimal>).NullBitmapBuffer.Memory;
                        dataFrameMemory = new Memory<byte>(new byte[decimalValueBuffer.Length]);
                        nullBitMapMemory = new Memory<byte>(new byte[decimalNullBitMapBuffer.Length]);
                        copied = decimalValueBuffer.TryCopyTo(dataFrameMemory) && decimalNullBitMapBuffer.TryCopyTo(nullBitMapMemory);
                        if (copied)
                        {
                            dataFrameColumn = new PrimitiveColumn<decimal>(field.Name, dataFrameMemory, nullBitMapMemory, arrowArray.Length);
                        }
                        break;
                    case ArrowTypeId.Double:
                        ReadOnlyMemory<byte> doubleValueBuffer = (arrowArray as PrimitiveArray<double>).ValueBuffer.Memory;
                        ReadOnlyMemory<byte> doubleNullBitMapBuffer = (arrowArray as PrimitiveArray<double>).NullBitmapBuffer.Memory;
                        dataFrameMemory = new Memory<byte>(new byte[doubleValueBuffer.Length]);
                        nullBitMapMemory = new Memory<byte>(new byte[doubleNullBitMapBuffer.Length]);
                        copied = doubleValueBuffer.TryCopyTo(dataFrameMemory) && doubleNullBitMapBuffer.TryCopyTo(nullBitMapMemory);
                        if (copied)
                        {
                            dataFrameColumn = new PrimitiveColumn<double>(field.Name, dataFrameMemory, nullBitMapMemory, arrowArray.Length);
                        }
                        break;
                    case ArrowTypeId.Float:
                        ReadOnlyMemory<byte> floatValueBuffer = (arrowArray as PrimitiveArray<float>).ValueBuffer.Memory;
                        ReadOnlyMemory<byte> floatNullBitMapBuffer = (arrowArray as PrimitiveArray<float>).NullBitmapBuffer.Memory;
                        dataFrameMemory = new Memory<byte>(new byte[floatValueBuffer.Length]);
                        nullBitMapMemory = new Memory<byte>(new byte[floatNullBitMapBuffer.Length]);
                        copied = floatValueBuffer.TryCopyTo(dataFrameMemory) && floatNullBitMapBuffer.TryCopyTo(nullBitMapMemory);
                        if (copied)
                        {
                            dataFrameColumn = new PrimitiveColumn<float>(field.Name, dataFrameMemory, nullBitMapMemory, arrowArray.Length);
                        }
                        break;
                    case ArrowTypeId.Int8:
                    case ArrowTypeId.Int16:
                    case ArrowTypeId.Int32:
                        ReadOnlyMemory<byte> intValueBuffer = (arrowArray as PrimitiveArray<int>).ValueBuffer.Memory;
                        ReadOnlyMemory<byte> intNullBitMapBuffer = (arrowArray as PrimitiveArray<int>).NullBitmapBuffer.Memory;
                        dataFrameMemory = new Memory<byte>(new byte[intValueBuffer.Length]);
                        nullBitMapMemory = new Memory<byte>(new byte[intNullBitMapBuffer.Length]);
                        copied = intValueBuffer.TryCopyTo(dataFrameMemory) && intNullBitMapBuffer.TryCopyTo(nullBitMapMemory);
                        if (copied)
                        {
                            dataFrameColumn = new PrimitiveColumn<int>(field.Name, dataFrameMemory, nullBitMapMemory, arrowArray.Length);
                        }
                        break;
                    case ArrowTypeId.Int64:
                        ReadOnlyMemory<byte> longValueBuffer = (arrowArray as PrimitiveArray<long>).ValueBuffer.Memory;
                        ReadOnlyMemory<byte> longNullBitMapBuffer = (arrowArray as PrimitiveArray<long>).NullBitmapBuffer.Memory;
                        dataFrameMemory = new Memory<byte>(new byte[longValueBuffer.Length]);
                        nullBitMapMemory = new Memory<byte>(new byte[longNullBitMapBuffer.Length]);
                        copied = longValueBuffer.TryCopyTo(dataFrameMemory) && longNullBitMapBuffer.TryCopyTo(nullBitMapMemory);
                        if (copied)
                        {
                            dataFrameColumn = new PrimitiveColumn<long>(field.Name, dataFrameMemory, nullBitMapMemory, arrowArray.Length);
                        }
                        break;
                    case ArrowTypeId.UInt8:
                    case ArrowTypeId.UInt16:
                    case ArrowTypeId.UInt32:
                        ReadOnlyMemory<byte> uintValueBuffer = (arrowArray as PrimitiveArray<uint>).ValueBuffer.Memory;
                        ReadOnlyMemory<byte> uintNullBitMapBuffer = (arrowArray as PrimitiveArray<uint>).NullBitmapBuffer.Memory;
                        dataFrameMemory = new Memory<byte>(new byte[uintValueBuffer.Length]);
                        nullBitMapMemory = new Memory<byte>(new byte[uintNullBitMapBuffer.Length]);
                        copied = uintValueBuffer.TryCopyTo(dataFrameMemory) && uintNullBitMapBuffer.TryCopyTo(nullBitMapMemory);
                        if (copied)
                        {
                            dataFrameColumn = new PrimitiveColumn<uint>(field.Name, dataFrameMemory, nullBitMapMemory, arrowArray.Length);
                        }
                        break;
                    case ArrowTypeId.UInt64:
                        ReadOnlyMemory<byte> ulongValueBuffer = (arrowArray as PrimitiveArray<ulong>).ValueBuffer.Memory;
                        ReadOnlyMemory<byte> ulongNullBitMapBuffer = (arrowArray as PrimitiveArray<ulong>).NullBitmapBuffer.Memory;
                        dataFrameMemory = new Memory<byte>(new byte[ulongValueBuffer.Length]);
                        nullBitMapMemory = new Memory<byte>(new byte[ulongNullBitMapBuffer.Length]);
                        copied = ulongValueBuffer.TryCopyTo(dataFrameMemory) && ulongNullBitMapBuffer.TryCopyTo(nullBitMapMemory);
                        if (copied)
                        {
                            dataFrameColumn = new PrimitiveColumn<ulong>(field.Name, dataFrameMemory, nullBitMapMemory, arrowArray.Length);
                        }
                        break;
                    case ArrowTypeId.Binary:
                        ReadOnlyMemory<byte> byteValueBuffer = (arrowArray as PrimitiveArray<byte>).ValueBuffer.Memory;
                        ReadOnlyMemory<byte> byteNullBitMapBuffer = (arrowArray as PrimitiveArray<byte>).NullBitmapBuffer.Memory;
                        dataFrameMemory = new Memory<byte>(new byte[byteValueBuffer.Length]);
                        nullBitMapMemory = new Memory<byte>(new byte[byteNullBitMapBuffer.Length]);
                        copied = byteValueBuffer.TryCopyTo(dataFrameMemory) && byteNullBitMapBuffer.TryCopyTo(nullBitMapMemory);
                        if (copied)
                        {
                            dataFrameColumn = new PrimitiveColumn<byte>(field.Name, dataFrameMemory, nullBitMapMemory, arrowArray.Length);
                        }
                        break;
                    case ArrowTypeId.Date32:
                    case ArrowTypeId.Date64:
                    case ArrowTypeId.Dictionary:
                    case ArrowTypeId.FixedSizedBinary:
                    case ArrowTypeId.HalfFloat:
                    case ArrowTypeId.Interval:
                    case ArrowTypeId.List:
                    case ArrowTypeId.Map:
                    case ArrowTypeId.Null:
                    case ArrowTypeId.Struct:
                    case ArrowTypeId.Time32:
                    case ArrowTypeId.Time64:
                    case ArrowTypeId.Timestamp:
                    case ArrowTypeId.Union:
                    default:
                        throw new NotImplementedException(nameof(fieldType.Name));
                }
                _table.InsertColumn(ColumnCount, dataFrameColumn);
                fieldIndex++;
            }
        }

        public IEnumerable<RecordBatch> AsArrowRecordBatch()
        {
            Apache.Arrow.Schema.Builder schemaBuilder = new Apache.Arrow.Schema.Builder();
            Field.Builder fieldBuilder = new Field.Builder();
            // TODO: Sanity check. Some column types are not Arrow Compatible
            // Derived column types will NOT be supported

            // Find the number of DataFrameBuffers in each column. All columns have the same number of buffers
            int numberOfColumns = ColumnCount;
            // Assuming that all columns are Arrow compatible 
            int numberOfRecordBatches = 0;
            for (int i = 0; i < numberOfColumns; i++)
            {
                BaseColumn column = Column(i);
                switch (column)
                {
                    case PrimitiveColumn<bool> boolColumn:
                        numberOfRecordBatches = boolColumn.NumberOfBuffers;
                        Field boolField = fieldBuilder.Name(boolColumn.Name).Nullable(true).DataType(BooleanType.Default).Build();
                        schemaBuilder.Field(boolField);
                        break;
                    case PrimitiveColumn<byte> byteColumn:
                    case PrimitiveColumn<char> charColumn:
                    case PrimitiveColumn<decimal> decimalColumn:
                    case PrimitiveColumn<sbyte> sbyteColumn:
                        throw new NotImplementedException(nameof(byteColumn.DataType));
                    case PrimitiveColumn<double> doubleColumn:
                        numberOfRecordBatches = doubleColumn.NumberOfBuffers;
                        Field doubleField = fieldBuilder.Name(doubleColumn.Name).Nullable(true).DataType(DoubleType.Default).Build();
                        schemaBuilder.Field(doubleField);
                        break;
                    case PrimitiveColumn<float> floatColumn:
                        numberOfRecordBatches = floatColumn.NumberOfBuffers;
                        Field floatField = fieldBuilder.Name(floatColumn.Name).Nullable(true).DataType(FloatType.Default).Build();
                        schemaBuilder.Field(floatField);
                        break;
                    case PrimitiveColumn<int> intColumn:
                        numberOfRecordBatches = intColumn.NumberOfBuffers;
                        Field intField = fieldBuilder.Name(intColumn.Name).Nullable(true).DataType(Int32Type.Default).Build();
                        schemaBuilder.Field(intField);
                        break;
                    case PrimitiveColumn<long> longColumn:
                        numberOfRecordBatches = longColumn.NumberOfBuffers;
                        Field longField = fieldBuilder.Name(longColumn.Name).Nullable(true).DataType(Int64Type.Default).Build();
                        schemaBuilder.Field(longField);
                        break;
                    case PrimitiveColumn<short> shortColumn:
                        numberOfRecordBatches = shortColumn.NumberOfBuffers;
                        Field shortField = fieldBuilder.Name(shortColumn.Name).Nullable(true).DataType(Int16Type.Default).Build();
                        schemaBuilder.Field(shortField);
                        break;
                    case PrimitiveColumn<uint> uintColumn:
                        numberOfRecordBatches = uintColumn.NumberOfBuffers;
                        Field uintField = fieldBuilder.Name(uintColumn.Name).Nullable(true).DataType(UInt32Type.Default).Build();
                        schemaBuilder.Field(uintField);
                        break;
                    case PrimitiveColumn<ulong> ulongColumn:
                        numberOfRecordBatches = ulongColumn.NumberOfBuffers;
                        Field ulongField = fieldBuilder.Name(ulongColumn.Name).Nullable(true).DataType(UInt64Type.Default).Build();
                        schemaBuilder.Field(ulongField);
                        break;
                    case PrimitiveColumn<ushort> ushortColumn:
                        numberOfRecordBatches = ushortColumn.NumberOfBuffers;
                        Field ushortField = fieldBuilder.Name(ushortColumn.Name).Nullable(true).DataType(UInt16Type.Default).Build();
                        schemaBuilder.Field(ushortField);
                        break;
                }
            }
            Schema schema = schemaBuilder.Build();
            List<Apache.Arrow.Array> arrays = new List<Apache.Arrow.Array>();

            for (int n = 0; n < numberOfRecordBatches; n++)
            {
                for (int i = 0; i < numberOfColumns; i++)
                {
                    BaseColumn column = Column(i);
                    //Memory<byte> values, nulls;
                    //ArrowBuffer valueBuffer, nullBuffer;
                    switch (column)
                    {
                        case PrimitiveColumn<bool> boolColumn:
                            arrays.Add(boolColumn.AsArrowArray(n));
                            break;
                        case PrimitiveColumn<byte> byteColumn:
                            arrays.Add(byteColumn.AsArrowArray(n));
                            break;
                        case PrimitiveColumn<char> charColumn:
                            arrays.Add(charColumn.AsArrowArray(n));
                            break;
                        case PrimitiveColumn<decimal> decimalColumn:
                            arrays.Add(decimalColumn.AsArrowArray(n));
                            break;
                        case PrimitiveColumn<double> doubleColumn:
                            arrays.Add(doubleColumn.AsArrowArray(n));
                            break;
                        case PrimitiveColumn<float> floatColumn:
                            arrays.Add(floatColumn.AsArrowArray(n));
                            break;
                        case PrimitiveColumn<int> intColumn:
                            arrays.Add(intColumn.AsArrowArray(n));
                            break;
                        case PrimitiveColumn<long> longColumn:
                            arrays.Add(longColumn.AsArrowArray(n));
                            break;
                        case PrimitiveColumn<sbyte> sbyteColumn:
                            arrays.Add(sbyteColumn.AsArrowArray(n));
                            break;
                        case PrimitiveColumn<short> shortColumn:
                            arrays.Add(shortColumn.AsArrowArray(n));
                            break;
                        case PrimitiveColumn<uint> uintColumn:
                            arrays.Add(uintColumn.AsArrowArray(n));
                            break;
                        case PrimitiveColumn<ulong> ulongColumn:
                            arrays.Add(ulongColumn.AsArrowArray(n));
                            break;
                        case PrimitiveColumn<ushort> ushortColumn:
                            arrays.Add(ushortColumn.AsArrowArray(n));
                            break;
                        default:
                            throw new NotImplementedException(nameof(column.DataType));
                    }
                }
                yield return new RecordBatch(schema, arrays, 0);
            }
        }

        public long RowCount => _table.RowCount;

        public int ColumnCount => _table.ColumnCount;

        public IList<string> Columns
        {
            get
            {
                var ret = new List<string>(ColumnCount);
                for (int i = 0; i < ColumnCount; i++)
                {
                    ret.Add(_table.Column(i).Name);
                }
                return ret;
            }
        }

        public BaseColumn Column(int index) => _table.Column(index);

        public void InsertColumn(int columnIndex, BaseColumn column) => _table.InsertColumn(columnIndex, column);

        public void SetColumn(int columnIndex, BaseColumn column) => _table.SetColumn(columnIndex, column);

        public void RemoveColumn(int columnIndex) => _table.RemoveColumn(columnIndex);

        public void RemoveColumn(string columnName) => _table.RemoveColumn(columnName);

        public object this[long rowIndex, int columnIndex]
        {
            get => _table.Column(columnIndex)[rowIndex];
            set => _table.Column(columnIndex)[rowIndex] = value;
        }

        #region Operators
        public IList<object> this[long rowIndex]
        {
            get
            {
                return _table.GetRow(rowIndex);
            }
            //TODO?: set?
        }

        public BaseColumn this[string columnName]
        {
            get
            {
                int columnIndex = _table.GetColumnIndex(columnName);
                if (columnIndex == -1)
                    throw new ArgumentException($"{columnName} does not exist");
                return _table.Column(columnIndex);
            }
            set
            {
                int columnIndex = _table.GetColumnIndex(columnName);
                BaseColumn newColumn = value;
                newColumn.Name = columnName;
                if (columnIndex == -1)
                {
                    _table.InsertColumn(ColumnCount, newColumn);
                }
                else
                {
                    _table.SetColumn(columnIndex, newColumn);
                }
            }
        }

        public IList<IList<object>> Head(int numberOfRows)
        {
            var ret = new List<IList<object>>();
            for (int i = 0; i < numberOfRows; i++)
            {
                ret.Add(this[i]);
            }
            return ret;
        }

        public IList<IList<object>> Tail(int numberOfRows)
        {
            var ret = new List<IList<object>>();
            for (long i = RowCount - numberOfRows; i < RowCount; i++)
            {
                ret.Add(this[i]);
            }
            return ret;
        }
        // TODO: Add strongly typed versions of these APIs
        #endregion

        private DataFrame Clone(BaseColumn mapIndices = null, bool invertMapIndices = false)
        {
            List<BaseColumn> newColumns = new List<BaseColumn>(ColumnCount);
            for (int i = 0; i < ColumnCount; i++)
            {
                newColumns.Add(Column(i).Clone(mapIndices, invertMapIndices));
            }
            return new DataFrame(newColumns);
        }

        public DataFrame Sort(string columnName, bool ascending = true)
        {
            BaseColumn column = this[columnName];
            BaseColumn sortIndices = column.GetAscendingSortIndices();
            List<BaseColumn> newColumns = new List<BaseColumn>(ColumnCount);
            for (int i = 0; i < ColumnCount; i++)
            {
                BaseColumn oldColumn = Column(i);
                BaseColumn newColumn = oldColumn.Clone(sortIndices, !ascending, oldColumn.NullCount);
                Debug.Assert(newColumn.NullCount == oldColumn.NullCount);
                newColumns.Add(newColumn);
            }
            return new DataFrame(newColumns);
        }

        private void SetSuffixForDuplicatedColumnNames(DataFrame dataFrame, BaseColumn column, string leftSuffix, string rightSuffix)
        {
            int index = dataFrame._table.GetColumnIndex(column.Name);
            while (index != -1)
            {
                // Pre-existing column. Change name
                BaseColumn existingColumn = dataFrame.Column(index);
                dataFrame._table.SetColumnName(existingColumn, existingColumn.Name + leftSuffix);
                column.Name += rightSuffix;
                index = dataFrame._table.GetColumnIndex(column.Name);
            }
        }

        public DataFrame Join(DataFrame other, string leftSuffix = "_left", string rightSuffix = "_right", JoinAlgorithm joinAlgorithm = JoinAlgorithm.Left)
        {
            DataFrame ret = new DataFrame();
            if (joinAlgorithm == JoinAlgorithm.Left)
            {
                for (int i = 0; i < ColumnCount; i++)
                {
                    BaseColumn newColumn = Column(i).Clone();
                    ret.InsertColumn(ret.ColumnCount, newColumn);
                }
                long minLength = Math.Min(RowCount, other.RowCount);
                PrimitiveColumn<long> mapIndices = new PrimitiveColumn<long>("mapIndices", minLength);
                for (long i = 0; i < minLength; i++)
                {
                    mapIndices[i] = i;
                }
                for (int i = 0; i < other.ColumnCount; i++)
                {
                    BaseColumn newColumn;
                    if (other.RowCount < RowCount)
                    {
                        newColumn = other.Column(i).Clone(numberOfNullsToAppend: RowCount - other.RowCount);
                    }
                    else
                    {
                        newColumn = other.Column(i).Clone(mapIndices);
                    }
                    SetSuffixForDuplicatedColumnNames(ret, newColumn, leftSuffix, rightSuffix);
                    ret.InsertColumn(ret.ColumnCount, newColumn);
                }
            }
            else if (joinAlgorithm == JoinAlgorithm.Right)
            {
                long minLength = Math.Min(RowCount, other.RowCount);
                PrimitiveColumn<long> mapIndices = new PrimitiveColumn<long>("mapIndices", minLength);
                for (long i = 0; i < minLength; i++)
                {
                    mapIndices[i] = i;
                }
                for (int i = 0; i < ColumnCount; i++)
                {
                    BaseColumn newColumn;
                    if (RowCount < other.RowCount)
                    {
                        newColumn = Column(i).Clone(numberOfNullsToAppend: other.RowCount - RowCount);
                    }
                    else
                    {
                        newColumn = Column(i).Clone(mapIndices);
                    }
                    ret.InsertColumn(ret.ColumnCount, newColumn);
                }
                for (int i = 0; i < other.ColumnCount; i++)
                {
                    BaseColumn newColumn = other.Column(i).Clone();
                    SetSuffixForDuplicatedColumnNames(ret, newColumn, leftSuffix, rightSuffix);
                    ret.InsertColumn(ret.ColumnCount, newColumn);
                }
            }
            else if (joinAlgorithm == JoinAlgorithm.FullOuter)
            {
                long newRowCount = Math.Max(RowCount, other.RowCount);
                long numberOfNulls = newRowCount - RowCount;
                for (int i = 0; i < ColumnCount; i++)
                {
                    BaseColumn newColumn = Column(i).Clone(numberOfNullsToAppend: numberOfNulls);
                    ret.InsertColumn(ret.ColumnCount, newColumn);
                }
                numberOfNulls = newRowCount - other.RowCount;
                for (int i = 0; i < other.ColumnCount; i++)
                {
                    BaseColumn newColumn = other.Column(i).Clone(numberOfNullsToAppend: numberOfNulls);
                    SetSuffixForDuplicatedColumnNames(ret, newColumn, leftSuffix, rightSuffix);
                    ret.InsertColumn(ret.ColumnCount, newColumn);
                }
            }
            else if (joinAlgorithm == JoinAlgorithm.Inner)
            {
                long newRowCount = Math.Min(RowCount, other.RowCount);
                PrimitiveColumn<long> mapIndices = new PrimitiveColumn<long>("mapIndices", newRowCount);
                for (long i = 0; i < newRowCount; i++)
                {
                    mapIndices[i] = i;
                }
                for (int i = 0; i < ColumnCount; i++)
                {
                    BaseColumn newColumn = Column(i).Clone(mapIndices);
                    ret.InsertColumn(ret.ColumnCount, newColumn);
                }
                for (int i = 0; i < other.ColumnCount; i++)
                {
                    BaseColumn newColumn = other.Column(i).Clone(mapIndices);
                    SetSuffixForDuplicatedColumnNames(ret, newColumn, leftSuffix, rightSuffix);
                    ret.InsertColumn(ret.ColumnCount, newColumn);
                }
            }
            return ret;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            int longestColumnName = 0;
            for (int i = 0; i < ColumnCount; i++)
            {
                longestColumnName = Math.Max(longestColumnName, Column(i).Name.Length);
            }
            for (int i = 0; i < ColumnCount; i++)
            {
                // Left align by 10
                sb.Append(string.Format($"0,{-longestColumnName}", Column(i).Name));
            }
            sb.AppendLine();
            long numberOfRows = Math.Min(RowCount, 25);
            for (int i = 0; i < numberOfRows; i++)
            {
                IList<object> row = this[i];
                foreach (object obj in row)
                {
                    sb.Append(string.Format($"0,{-longestColumnName}", obj.ToString()));
                }
                sb.AppendLine();
            }
            return sb.ToString();
        }
    }
}
