// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

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

        public DataFrame(IList<DataFrameColumn> columns)
        {
            _table = new DataFrameTable(columns);
        }

        public long RowCount => _table.RowCount;

        public int ColumnCount => _table.ColumnCount;

        public IReadOnlyList<DataFrameColumn> Columns => _table.Columns;

        public IReadOnlyList<string> ColumnNames => _table.ColumnNames;

        public DataFrameColumn Column(int index) => _table.Column(index);

        public void InsertColumn(int columnIndex, DataFrameColumn column)
        {
            _table.InsertColumn(columnIndex, column);
            OnColumnsChanged();
        }

        public void SetColumn(int columnIndex, DataFrameColumn column)
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
        public DataFrame this[DataFrameColumn filterColumn] => Clone(filterColumn);

        public DataFrame this[IEnumerable<int> filter]
        {
            get
            {
                PrimitiveDataFrameColumn<int> filterColumn = new PrimitiveDataFrameColumn<int>("Filter", filter);
                return Clone(filterColumn);
            }
        }

        public DataFrameColumn this[string columnName]
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
                DataFrameColumn newColumn = value;
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

        private DataFrame Clone(DataFrameColumn mapIndices = null, bool invertMapIndices = false)
        {
            List<DataFrameColumn> newColumns = new List<DataFrameColumn>(ColumnCount);
            for (int i = 0; i < ColumnCount; i++)
            {
                newColumns.Add(Column(i).Clone(mapIndices, invertMapIndices));
            }
            return new DataFrame(newColumns);
        }

        public void SetColumnName(DataFrameColumn column, string newName) => _table.SetColumnName(column, newName);

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
                DataFrameColumn column = Column(i);
                if (!column.HasDescription())
                {
                    continue;
                }
                DataFrame columnDescription = column.Description();
                ret = ret.Merge<string>(columnDescription, "Description", "Description", "_left", "_right", JoinAlgorithm.Inner);
                int leftMergeColumn = ret._table.GetColumnIndex("Description" + "_left");
                int rightMergeColumn = ret._table.GetColumnIndex("Description" + "_right");
                if (leftMergeColumn != -1 && rightMergeColumn != -1)
                {
                    ret.RemoveColumn("Description" + "_right");
                    ret._table.SetColumnName(ret["Description_left"], "Description");
                }
            }
            return ret;
        }


        public DataFrame Sort(string columnName, bool ascending = true)
        {
            DataFrameColumn column = this[columnName];
            DataFrameColumn sortIndices = column.GetAscendingSortIndices();
            List<DataFrameColumn> newColumns = new List<DataFrameColumn>(ColumnCount);
            for (int i = 0; i < ColumnCount; i++)
            {
                DataFrameColumn oldColumn = Column(i);
                DataFrameColumn newColumn = oldColumn.Clone(sortIndices, !ascending, oldColumn.NullCount);
                Debug.Assert(newColumn.NullCount == oldColumn.NullCount);
                newColumns.Add(newColumn);
            }
            return new DataFrame(newColumns);
        }

        /// <summary>
        /// Clips values beyond the specified thresholds on numeric columns
        /// </summary>
        /// <typeparam name="U"></typeparam>
        /// <param name="lower">Minimum value. All values below this threshold will be set to it</param>
        /// <param name="upper">Maximum value. All values above this threshold will be set to it</param>
        public DataFrame Clip<U>(U lower, U upper, bool inPlace = false)
        {
            DataFrame ret = inPlace ? this : Clone();

            for (int i = 0; i < ret.ColumnCount; i++)
            {
                DataFrameColumn column = ret.Column(i);
                if (column.IsNumericColumn())
                    column.Clip(lower, upper, inPlace: true);
            }
            return ret;
        }

        /// <summary>
        /// Adds a prefix to the column names
        /// </summary>
        public DataFrame AddPrefix(string prefix, bool inPlace = false)
        {
            DataFrame df = inPlace ? this : Clone();
            for (int i = 0; i < df.ColumnCount; i++)
            {
                DataFrameColumn column = df.Column(i);
                df._table.SetColumnName(column, prefix + column.Name);
                df.OnColumnsChanged();
            }
            return df;
        }

        /// <summary>
        /// Adds a suffix to the column names
        /// </summary>
        public DataFrame AddSuffix(string suffix, bool inPlace = false)
        {
            DataFrame df = inPlace ? this : Clone();
            for (int i = 0; i < df.ColumnCount; i++)
            {
                DataFrameColumn column = df.Column(i);
                df._table.SetColumnName(column, column.Name + suffix);
                df.OnColumnsChanged();
            }
            return df;
        }

        /// <summary>
        /// Returns a random sample of rows
        /// </summary>
        /// <param name="numberOfRows">Number of rows in the returned DataFrame</param>
        public DataFrame Sample(int numberOfRows)
        {
            Random rand = new Random();
            PrimitiveDataFrameColumn<long> indices = new PrimitiveDataFrameColumn<long>("Indices", numberOfRows);
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

            DataFrameColumn column = _table.Column(columnIndex);
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
            PrimitiveDataFrameColumn<bool> filter = new PrimitiveDataFrameColumn<bool>("Filter");
            if (options == DropNullOptions.Any)
            {
                filter.AppendMany(true, RowCount);

                for (int i = 0; i < ColumnCount; i++)
                {
                    DataFrameColumn column = Column(i);
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
                    DataFrameColumn column = Column(i);
                    filter.ApplyElementwise((bool? value, long index) =>
                    {
                        return value.Value || (column[index] == null ? false : true);
                    });
                }
            }
            return this[filter];
        }

        public DataFrame FillNulls(object value, bool inPlace = false)
        {
            DataFrame ret = inPlace ? this : Clone();
            for (int i = 0; i < ret.ColumnCount; i++)
            {
                ret.Column(i).FillNulls(value, inPlace: true);
            }
            return ret;
        }

        public DataFrame FillNulls(IList<object> values, bool inPlace = false)
        {
            if (values.Count != ColumnCount)
                throw new ArgumentException(Strings.MismatchedColumnLengths, nameof(values));

            DataFrame ret = inPlace ? this : Clone();
            for (int i = 0; i < ret.ColumnCount; i++)
            {
                Column(i).FillNulls(values[i], inPlace: true);
            }
            return ret;
        }

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
