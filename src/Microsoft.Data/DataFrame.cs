// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Apache.Arrow;
using Apache.Arrow.Types;

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
                switch (fieldType.TypeId)
                {
                    case ArrowTypeId.Boolean:
                        ReadOnlyMemory<byte> valueBuffer = (arrowArray as BooleanArray).ValueBuffer.Memory;
                        ReadOnlyMemory<byte> nullBitMapBuffer = (arrowArray as BooleanArray).NullBitmapBuffer.Memory;
                        dataFrameColumn = new PrimitiveColumn<bool>(field.Name, valueBuffer, nullBitMapBuffer, arrowArray.Length, arrowArray.NullCount);
                        break;
                    case ArrowTypeId.Decimal:
                        ReadOnlyMemory<byte> decimalValueBuffer = (arrowArray as PrimitiveArray<decimal>).ValueBuffer.Memory;
                        ReadOnlyMemory<byte> decimalNullBitMapBuffer = (arrowArray as PrimitiveArray<decimal>).NullBitmapBuffer.Memory;
                        dataFrameColumn = new PrimitiveColumn<decimal>(field.Name, decimalValueBuffer, decimalNullBitMapBuffer, arrowArray.Length, arrowArray.NullCount);
                        break;
                    case ArrowTypeId.Double:
                        ReadOnlyMemory<byte> doubleValueBuffer = (arrowArray as PrimitiveArray<double>).ValueBuffer.Memory;
                        ReadOnlyMemory<byte> doubleNullBitMapBuffer = (arrowArray as PrimitiveArray<double>).NullBitmapBuffer.Memory;
                        dataFrameColumn = new PrimitiveColumn<double>(field.Name, doubleValueBuffer, doubleNullBitMapBuffer, arrowArray.Length, arrowArray.NullCount);
                        break;
                    case ArrowTypeId.Float:
                        ReadOnlyMemory<byte> floatValueBuffer = (arrowArray as PrimitiveArray<float>).ValueBuffer.Memory;
                        ReadOnlyMemory<byte> floatNullBitMapBuffer = (arrowArray as PrimitiveArray<float>).NullBitmapBuffer.Memory;
                        dataFrameColumn = new PrimitiveColumn<float>(field.Name, floatValueBuffer, floatNullBitMapBuffer, arrowArray.Length, arrowArray.NullCount);
                        break;
                    case ArrowTypeId.Int8:
                    case ArrowTypeId.Int16:
                    case ArrowTypeId.Int32:
                        ReadOnlyMemory<byte> intValueBuffer = (arrowArray as PrimitiveArray<int>).ValueBuffer.Memory;
                        ReadOnlyMemory<byte> intNullBitMapBuffer = (arrowArray as PrimitiveArray<int>).NullBitmapBuffer.Memory;
                        dataFrameColumn = new PrimitiveColumn<int>(field.Name, intValueBuffer, intNullBitMapBuffer, arrowArray.Length, arrowArray.NullCount);
                        break;
                    case ArrowTypeId.Int64:
                        ReadOnlyMemory<byte> longValueBuffer = (arrowArray as PrimitiveArray<long>).ValueBuffer.Memory;
                        ReadOnlyMemory<byte> longNullBitMapBuffer = (arrowArray as PrimitiveArray<long>).NullBitmapBuffer.Memory;
                        dataFrameColumn = new PrimitiveColumn<long>(field.Name, longValueBuffer, longNullBitMapBuffer, arrowArray.Length, arrowArray.NullCount);
                        break;
                    case ArrowTypeId.String:
                        StringArray stringArray = arrowArray as StringArray;
                        ReadOnlyMemory<byte> dataMemory = stringArray.ValueBuffer.Memory;
                        DataFrameBuffer<byte> dataBuffer = new DataFrameBuffer<byte>(dataMemory, dataMemory.Length);
                        ReadOnlyMemory<byte> offsetsMemory = stringArray.ValueOffsetsBuffer.Memory;
                        DataFrameBuffer<int> offsetBuffer = new DataFrameBuffer<int>(offsetsMemory, stringArray.Length + 1);
                        ReadOnlyMemory<byte> nullMemory = stringArray.NullBitmapBuffer.Memory;
                        DataFrameBuffer<byte> nullBuffer = new DataFrameBuffer<byte>(nullMemory, stringArray.NullCount);
                        dataFrameColumn = new ArrowStringColumn(field.Name, dataBuffer, offsetBuffer, nullBuffer, stringArray.Length);
                        break;
                    case ArrowTypeId.UInt8:
                    case ArrowTypeId.UInt16:
                    case ArrowTypeId.UInt32:
                        ReadOnlyMemory<byte> uintValueBuffer = (arrowArray as PrimitiveArray<uint>).ValueBuffer.Memory;
                        ReadOnlyMemory<byte> uintNullBitMapBuffer = (arrowArray as PrimitiveArray<uint>).NullBitmapBuffer.Memory;
                        dataFrameColumn = new PrimitiveColumn<uint>(field.Name, uintValueBuffer, uintNullBitMapBuffer, arrowArray.Length, arrowArray.NullCount);
                        break;
                    case ArrowTypeId.UInt64:
                        ReadOnlyMemory<byte> ulongValueBuffer = (arrowArray as PrimitiveArray<ulong>).ValueBuffer.Memory;
                        ReadOnlyMemory<byte> ulongNullBitMapBuffer = (arrowArray as PrimitiveArray<ulong>).NullBitmapBuffer.Memory;
                        dataFrameColumn = new PrimitiveColumn<ulong>(field.Name, ulongValueBuffer, ulongNullBitMapBuffer, arrowArray.Length, arrowArray.NullCount);
                        break;
                    case ArrowTypeId.Binary:
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
                    default:
                        throw new NotImplementedException(nameof(fieldType.Name));
                }
                _table.InsertColumn(ColumnCount, dataFrameColumn);
                fieldIndex++;
            }
        }

        public IEnumerable<RecordBatch> AsArrowRecordBatches()
        {
            Apache.Arrow.Schema.Builder schemaBuilder = new Apache.Arrow.Schema.Builder();

            int columnCount = ColumnCount;
            for (int i = 0; i < columnCount; i++)
            {
                BaseColumn column = Column(i);
                Field field = column.Field;
                schemaBuilder.Field(field);
            }

            Schema schema = schemaBuilder.Build();
            List<Apache.Arrow.Array> arrays = new List<Apache.Arrow.Array>();

            int recordBatchLength = Int32.MaxValue;
            int numberOfRowsInThisRecordBatch = (int)Math.Min(recordBatchLength, RowCount);
            long numberOfRowsProcessed = 0;

            while (numberOfRowsProcessed < RowCount || RowCount == 0)
            {
                for (int i = 0; i < columnCount; i++)
                {
                    BaseColumn column = Column(i);
                    numberOfRowsInThisRecordBatch = (int)Math.Min(numberOfRowsInThisRecordBatch, column.MaxRecordBatchLength(numberOfRowsProcessed));
                }
                for (int i = 0; i < columnCount; i++)
                {
                    BaseColumn column = Column(i);
                    arrays.Add(column.AsArrowArray(numberOfRowsProcessed, numberOfRowsInThisRecordBatch));
                }
                numberOfRowsProcessed += numberOfRowsInThisRecordBatch;
                yield return new RecordBatch(schema, arrays, numberOfRowsInThisRecordBatch);
                if (RowCount == 0)
                    break;
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
                    throw new ArgumentException(Strings.InvalidColumnName, nameof(columnName));
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

        public GroupBy GroupBy(string columnName)
        {
            int columnIndex = _table.GetColumnIndex(columnName);
            if (columnIndex == -1)
                throw new ArgumentException(Strings.InvalidColumnName, nameof(columnName));

            BaseColumn column = _table.Column(columnIndex);
            return column.GroupBy(columnIndex, this);
        }

        // In a GroupBy call, columns get resized. We need to set the RowCount to reflect the true Length of the DataFrame. Internal only. Should not be exposed
        internal void SetTableRowCount(long rowCount)
        {
            // Even if current RowCount == rowCount, do the validation
            for (int i = 0; i < ColumnCount; i++)
            {
                if (Column(i).Length != rowCount)
                    throw new ArgumentException(String.Format("{0} {1}", Strings.MismatchedRowCount, Column(i).Name));
            }
            _table.RowCount = rowCount;
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
                sb.Append(string.Format(Column(i).Name.PadRight(longestColumnName)));
            }
            sb.AppendLine();
            long numberOfRows = Math.Min(RowCount, 25);
            for (int i = 0; i < numberOfRows; i++)
            {
                IList<object> row = this[i];
                foreach (object obj in row)
                {
                    sb.Append((obj ?? "null").ToString().PadRight(longestColumnName));
                }
                sb.AppendLine();
            }
            return sb.ToString();
        }
    }
}
