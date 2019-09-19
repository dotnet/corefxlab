// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Apache.Arrow;
using Apache.Arrow.Types;

namespace Microsoft.Data
{

    /// <summary>
    /// Options for DropNull(). 
    /// </summary>
    public enum DropNullOptions
    {
        /// <summary>
        /// "Any" drops a row if any of the row values are null. 
        /// </summary>
        Any,
        /// <summary>
        /// "All" drops a row when all of the row values are null.
        /// </summary>
        All
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
                        BooleanArray arrowBooleanArray = (BooleanArray)arrowArray;
                        ReadOnlyMemory<byte> valueBuffer = arrowBooleanArray.ValueBuffer.Memory;
                        ReadOnlyMemory<byte> nullBitMapBuffer = arrowBooleanArray.NullBitmapBuffer.Memory;
                        dataFrameColumn = new PrimitiveColumn<bool>(field.Name, valueBuffer, nullBitMapBuffer, arrowArray.Length, arrowArray.NullCount);
                        break;
                    case ArrowTypeId.Double:
                        PrimitiveArray<double> arrowDoubleArray = (PrimitiveArray<double>)arrowArray;
                        ReadOnlyMemory<byte> doubleValueBuffer = arrowDoubleArray.ValueBuffer.Memory;
                        ReadOnlyMemory<byte> doubleNullBitMapBuffer = arrowDoubleArray.NullBitmapBuffer.Memory;
                        dataFrameColumn = new PrimitiveColumn<double>(field.Name, doubleValueBuffer, doubleNullBitMapBuffer, arrowArray.Length, arrowArray.NullCount);
                        break;
                    case ArrowTypeId.Float:
                        PrimitiveArray<float> arrowFloatArray = (PrimitiveArray<float>)arrowArray;
                        ReadOnlyMemory<byte> floatValueBuffer = arrowFloatArray.ValueBuffer.Memory;
                        ReadOnlyMemory<byte> floatNullBitMapBuffer = arrowFloatArray.NullBitmapBuffer.Memory;
                        dataFrameColumn = new PrimitiveColumn<float>(field.Name, floatValueBuffer, floatNullBitMapBuffer, arrowArray.Length, arrowArray.NullCount);
                        break;
                    case ArrowTypeId.Int8:
                        PrimitiveArray<sbyte> arrowsbyteArray = (PrimitiveArray<sbyte>)arrowArray;
                        ReadOnlyMemory<byte> sbyteValueBuffer = arrowsbyteArray.ValueBuffer.Memory;
                        ReadOnlyMemory<byte> sbyteNullBitMapBuffer = arrowsbyteArray.NullBitmapBuffer.Memory;
                        dataFrameColumn = new PrimitiveColumn<sbyte>(field.Name, sbyteValueBuffer, sbyteNullBitMapBuffer, arrowArray.Length, arrowArray.NullCount);
                        break;
                    case ArrowTypeId.Int16:
                        PrimitiveArray<short> arrowshortArray = (PrimitiveArray<short>)arrowArray;
                        ReadOnlyMemory<byte> shortValueBuffer = arrowshortArray.ValueBuffer.Memory;
                        ReadOnlyMemory<byte> shortNullBitMapBuffer = arrowshortArray.NullBitmapBuffer.Memory;
                        dataFrameColumn = new PrimitiveColumn<short>(field.Name, shortValueBuffer, shortNullBitMapBuffer, arrowArray.Length, arrowArray.NullCount);
                        break;
                    case ArrowTypeId.Int32:
                        PrimitiveArray<int> arrowIntArray = (PrimitiveArray<int>)arrowArray;
                        ReadOnlyMemory<byte> intValueBuffer = arrowIntArray.ValueBuffer.Memory;
                        ReadOnlyMemory<byte> intNullBitMapBuffer = arrowIntArray.NullBitmapBuffer.Memory;
                        dataFrameColumn = new PrimitiveColumn<int>(field.Name, intValueBuffer, intNullBitMapBuffer, arrowArray.Length, arrowArray.NullCount);
                        break;
                    case ArrowTypeId.Int64:
                        PrimitiveArray<long> arrowLongArray = (PrimitiveArray<long>)arrowArray;
                        ReadOnlyMemory<byte> longValueBuffer = arrowLongArray.ValueBuffer.Memory;
                        ReadOnlyMemory<byte> longNullBitMapBuffer = arrowLongArray.NullBitmapBuffer.Memory;
                        dataFrameColumn = new PrimitiveColumn<long>(field.Name, longValueBuffer, longNullBitMapBuffer, arrowArray.Length, arrowArray.NullCount);
                        break;
                    case ArrowTypeId.String:
                        StringArray stringArray = (StringArray)arrowArray;
                        ReadOnlyMemory<byte> dataMemory = stringArray.ValueBuffer.Memory;
                        ReadOnlyMemory<byte> offsetsMemory = stringArray.ValueOffsetsBuffer.Memory;
                        ReadOnlyMemory<byte> nullMemory = stringArray.NullBitmapBuffer.Memory;
                        dataFrameColumn = new ArrowStringColumn(field.Name, dataMemory, offsetsMemory, nullMemory, stringArray.Length, stringArray.NullCount);
                        break;
                    case ArrowTypeId.UInt8:
                        PrimitiveArray<byte> arrowbyteArray = (PrimitiveArray<byte>)arrowArray;
                        ReadOnlyMemory<byte> byteValueBuffer = arrowbyteArray.ValueBuffer.Memory;
                        ReadOnlyMemory<byte> byteNullBitMapBuffer = arrowbyteArray.NullBitmapBuffer.Memory;
                        dataFrameColumn = new PrimitiveColumn<byte>(field.Name, byteValueBuffer, byteNullBitMapBuffer, arrowArray.Length, arrowArray.NullCount);
                        break;
                    case ArrowTypeId.UInt16:
                        PrimitiveArray<ushort> arrowUshortArray = (PrimitiveArray<ushort>)arrowArray;
                        ReadOnlyMemory<byte> ushortValueBuffer = arrowUshortArray.ValueBuffer.Memory;
                        ReadOnlyMemory<byte> ushortNullBitMapBuffer = arrowUshortArray.NullBitmapBuffer.Memory;
                        dataFrameColumn = new PrimitiveColumn<ushort>(field.Name, ushortValueBuffer, ushortNullBitMapBuffer, arrowArray.Length, arrowArray.NullCount);
                        break;
                    case ArrowTypeId.UInt32:
                        PrimitiveArray<uint> arrowUintArray = (PrimitiveArray<uint>)arrowArray;
                        ReadOnlyMemory<byte> uintValueBuffer = arrowUintArray.ValueBuffer.Memory;
                        ReadOnlyMemory<byte> uintNullBitMapBuffer = arrowUintArray.NullBitmapBuffer.Memory;
                        dataFrameColumn = new PrimitiveColumn<uint>(field.Name, uintValueBuffer, uintNullBitMapBuffer, arrowArray.Length, arrowArray.NullCount);
                        break;
                    case ArrowTypeId.UInt64:
                        PrimitiveArray<ulong> arrowUlongArray = (PrimitiveArray<ulong>)arrowArray;
                        ReadOnlyMemory<byte> ulongValueBuffer = arrowUlongArray.ValueBuffer.Memory;
                        ReadOnlyMemory<byte> ulongNullBitMapBuffer = arrowUlongArray.NullBitmapBuffer.Memory;
                        dataFrameColumn = new PrimitiveColumn<ulong>(field.Name, ulongValueBuffer, ulongNullBitMapBuffer, arrowArray.Length, arrowArray.NullCount);
                        break;
                    case ArrowTypeId.Decimal:
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
                Field field = column.Field();
                schemaBuilder.Field(field);
            }

            Schema schema = schemaBuilder.Build();
            List<Apache.Arrow.Array> arrays = new List<Apache.Arrow.Array>();

            int recordBatchLength = Int32.MaxValue;
            int numberOfRowsInThisRecordBatch = (int)Math.Min(recordBatchLength, RowCount);
            long numberOfRowsProcessed = 0;

            // Sometimes .NET for Spark passes in DataFrames with no rows. In those cases, we just return a RecordBatch with the right Schema and no rows
            do
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
            } while (numberOfRowsProcessed < RowCount);
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

        public IList<string> ColumnTypes
        {
            get
            {
                var ret = new List<string>(ColumnCount);
                for (int i = 0; i < ColumnCount; i++)
                {
                    ret.Add(_table.Column(i).DataType.ToString());
                }
                return ret;
            }
        }

        public BaseColumn Column(int index) => _table.Column(index);

        public void InsertColumn(int columnIndex, BaseColumn column)
        {
            _table.InsertColumn(columnIndex, column);
            OnColumnsChanged();
        }

        public void SetColumn(int columnIndex, BaseColumn column)
        {
            _table.SetColumn(columnIndex, column);
            OnColumnsChanged();
        }

        public void RemoveColumn(int columnIndex)
        {
            _table.RemoveColumn(columnIndex);
            OnColumnsChanged();
        }

        public void RemoveColumn(string columnName)
        {
            _table.RemoveColumn(columnName);
            OnColumnsChanged();
        }

        internal int GetColumnIndex(string columnName) => _table.GetColumnIndex(columnName);

        #region Operators
        public object this[long rowIndex, int columnIndex]
        {
            get => _table.Column(columnIndex)[rowIndex];
            set => _table.Column(columnIndex)[rowIndex] = value;
        }

        public IList<object> this[long rowIndex]
        {
            get
            {
                return _table.GetRow(rowIndex);
            }
            //TODO?: set?
        }

        /// <summary>
        /// Returns a new DataFrame using the rows or true values in filterColumn
        /// </summary>
        /// <param name="filterColumn">A column of rows/bools</param>
        /// <remarks>filterColumn must be of type long, int or bool</remarks>
        public DataFrame this[BaseColumn filterColumn] => Clone(filterColumn);

        public DataFrame this[IEnumerable<int> filter]
        {
            get
            {
                PrimitiveColumn<int> filterColumn = new PrimitiveColumn<int>("Filter", filter);
                return Clone(filterColumn);
            }
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
                newColumn.SetName(columnName);
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

        public void SetColumnName(BaseColumn column, string newName) => _table.SetColumnName(column, newName);

        /// <summary>
        /// Generates a concise summary of each column
        /// </summary>
        public DataFrame Info()
        {
            DataFrame ret = new DataFrame();
            if (ColumnCount == 0)
                return ret;

            for (int i = 0; i < ColumnCount; i++)
            {
                BaseColumn column = Column(i);
                DataFrame columnInfo = column.Info();
                if (i == 0)
                    ret = columnInfo;
                else
                    ret = ret.Merge<string>(columnInfo, "Info", "Info", "_left", "_right", joinAlgorithm: JoinAlgorithm.Inner);
                RemoveRightAndRenameLeftColumn(ret, "Info", "_left", "_right");
            }
            return ret;
        }

        private void RemoveRightAndRenameLeftColumn(DataFrame dataFrame, string columnName, string left, string right)
        {
            string leftColumnName = columnName + left;
            string rightColumnName = columnName + right;
            int leftMergeColumn = dataFrame._table.GetColumnIndex(leftColumnName);
            int rightMergeColumn = dataFrame._table.GetColumnIndex(rightColumnName);
            if (leftMergeColumn != -1 && rightMergeColumn != -1)
            {
                dataFrame.RemoveColumn(rightColumnName);
                dataFrame._table.SetColumnName(dataFrame[leftColumnName], columnName);
            }
        }

        /// <summary>
        /// Generates descriptive statistics that summarize each numeric column
        /// </summary>
        public DataFrame Description()
        {
            DataFrame ret = new DataFrame();
            if (ColumnCount == 0)
                return ret;
            int i = 0;
            while (!Column(i).HasDescription())
            {
                i++;
            }
            ret = Column(i).Description();
            i++;
            for (; i < ColumnCount; i++)
            {
                BaseColumn column = Column(i);
                if (!column.HasDescription())
                {
                    continue;
                }
                DataFrame columnDescription = column.Description();
                ret = ret.Merge<string>(columnDescription, "Description", "Description", "_left", "_right", JoinAlgorithm.Inner);
                RemoveRightAndRenameLeftColumn(ret, "Description", "_left", "_right");
            }
            return ret;
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

        /// <summary>
        /// Clips values beyond the specified thresholds
        /// </summary>
        /// <typeparam name="U"></typeparam>
        /// <param name="lower">Minimum value. All values below this threshold will be set to it</param>
        /// <param name="upper">Maximum value. All values above this threshold will be set to it</param>
        public DataFrame Clip<U>(U lower, U upper)
        {
            DataFrame ret = new DataFrame();
            for (int i = 0; i < ColumnCount; i++)
            {
                BaseColumn column = Column(i);
                BaseColumn insert = column;
                if (column.IsNumericColumn())
                    insert = column.Clip(lower, upper);
                ret.InsertColumn(i, insert);
            }
            return ret;
        }

        /// <summary>
        /// Adds a prefix to the column names
        /// </summary>
        public DataFrame AddPrefix(string prefix)
        {
            for (int i = 0; i < ColumnCount; i++)
            {
                BaseColumn column = Column(i);
                _table.SetColumnName(column, prefix + column.Name);
                OnColumnsChanged();
            }
            return this;
        }

        /// <summary>
        /// Adds a suffix to the column names
        /// </summary>
        public DataFrame AddSuffix(string suffix)
        {
            for (int i = 0; i < ColumnCount; i++)
            {
                BaseColumn column = Column(i);
                _table.SetColumnName(column, column.Name + suffix);
                OnColumnsChanged();
            }
            return this;
        }

        /// <summary>
        /// Returns a random sample of rows
        /// </summary>
        /// <param name="numberOfRows">Number of rows in the returned DataFrame</param>
        public DataFrame Sample(int numberOfRows)
        {
            Random rand = new Random();
            PrimitiveColumn<long> indices = new PrimitiveColumn<long>("Indices", numberOfRows);
            int randMaxValue = (int)Math.Min(Int32.MaxValue, RowCount);
            for (long i = 0; i < numberOfRows; i++)
            {
                indices[i] = rand.Next(randMaxValue);
            }

            return Clone(indices);
        }

        public GroupBy GroupBy(string columnName)
        {
            int columnIndex = _table.GetColumnIndex(columnName);
            if (columnIndex == -1)
                throw new ArgumentException(Strings.InvalidColumnName, nameof(columnName));

            BaseColumn column = _table.Column(columnIndex);
            return column.GroupBy(columnIndex, this);
        }

        // In GroupBy and ReadCsv calls, columns get resized. We need to set the RowCount to reflect the true Length of the DataFrame. This does internal validation
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

        /// <summary>
        /// Returns a DataFrame with no missing values
        /// </summary>
        /// <param name="options"></param>
        public DataFrame DropNulls(DropNullOptions options = DropNullOptions.Any)
        {
            DataFrame ret = new DataFrame();
            PrimitiveColumn<bool> filter = new PrimitiveColumn<bool>("Filter");
            if (options == DropNullOptions.Any)
            {
                filter.AppendMany(true, RowCount);

                for (int i = 0; i < ColumnCount; i++)
                {
                    BaseColumn column = Column(i);
                    filter.ApplyElementwise((bool? value, long index) =>
                    {
                        return value.Value && (column[index] == null ? false : true);
                    });
                }
            }
            else
            {
                filter.AppendMany(false, RowCount);
                for (int i = 0; i < ColumnCount; i++)
                {
                    BaseColumn column = Column(i);
                    filter.ApplyElementwise((bool? value, long index) =>
                    {
                        return value.Value || (column[index] == null ? false : true);
                    });
                }
            }
            return this[filter];
        }

        public DataFrame FillNulls(object value)
        {
            DataFrame ret = new DataFrame();
            for (int i = 0; i < ColumnCount; i++)
            {
                ret.InsertColumn(i, Column(i).FillNulls(value));
            }
            return ret;
        }

        public DataFrame FillNulls(IList<object> values)
        {
            if (values.Count != ColumnCount)
                throw new ArgumentException(nameof(values));
            DataFrame ret = new DataFrame();
            for (int i = 0; i < ColumnCount; i++)
            {
                ret.InsertColumn(i, Column(i).FillNulls(values[i]));
            }
            return ret;

        }

        /// <summary>
        /// Appends a row inplace to the DataFrame
        /// </summary>
        /// <remarks>If a row value doesn't match its column's data type, a conversion will be attempted</remarks>
        /// <param name="row"></param>
        public void Append(IEnumerable<object> row) => _table.Append(row);

        /// <summary>
        /// Invalidates any cached data after a column has changed.
        /// </summary>
        private void OnColumnsChanged()
        {
            _schema = null;
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
