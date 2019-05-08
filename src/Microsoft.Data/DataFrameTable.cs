// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;

namespace Microsoft.Data
{
    /// <summary>
    /// A DataFrameTable is just a container that holds a number of DataFrameColumns. It mainly acts as a convenient store to allow DataFrame to implement its algorithms
    /// </summary>
    internal class DataFrameTable
    {
        private IList<BaseColumn> _columns;

        private List<string> _columnNames = new List<string>();

        private Dictionary<string, int> _columnNameToIndexDictionary = new Dictionary<string, int>(StringComparer.Ordinal);

        public long RowCount { get; private set; }

        public int ColumnCount { get; private set; }

        public DataFrameTable()
        {
            _columns = new List<BaseColumn>();
        }

        public DataFrameTable(IList<BaseColumn> columns)
        {
            columns = columns ?? throw new ArgumentNullException(nameof(columns));
            _columns = columns;
            ColumnCount = columns.Count;
            if (columns.Count > 0)
            {
                RowCount = columns[0].Length;
                for (var i = 0; i < columns.Count; i++)
                {
                    _columnNames.Add(columns[i].Name);
                    _columnNameToIndexDictionary.Add(columns[i].Name, i);
                }
            }
        }

        public DataFrameTable(BaseColumn column) : this(new List<BaseColumn> { column }) { }

        public BaseColumn Column(int columnIndex) => _columns[columnIndex];

        public IList<object> GetRow(long rowIndex)
        {
            var ret = new List<object>();
            for (int i = 0; i < ColumnCount; i++)
            {
                ret.Add(Column(i)[rowIndex]);
            }
            return ret;
        }

        public void InsertColumn<T>(int columnIndex, IEnumerable<T> column, string columnName)
            where T : unmanaged
        {
            column = column ?? throw new ArgumentNullException(nameof(column));
            if ((uint)columnIndex > _columns.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(columnIndex));
            }
            BaseColumn newColumn = new PrimitiveColumn<T>(columnName, column);
            InsertColumn(columnIndex, newColumn);
        }

        public void InsertColumn(int columnIndex, BaseColumn column)
        {
            column = column ?? throw new ArgumentNullException(nameof(column));
            if ((uint)columnIndex > _columns.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(columnIndex));
            }
            if (RowCount > 0 && column.Length != RowCount)
            {
                throw new ArgumentException(strings.MismatchedColumnLengths, nameof(column));
            }
            if (_columnNameToIndexDictionary.ContainsKey(column.Name))
            {
                throw new ArgumentException($"Table already contains a column called {column.Name}");
            }
            RowCount = column.Length;
            _columnNames.Insert(columnIndex, column.Name);
            _columnNameToIndexDictionary[column.Name] = columnIndex;
            _columns.Insert(columnIndex, column);
            ColumnCount++;
        }

        public void SetColumn(int columnIndex, BaseColumn column)
        {
            column = column ?? throw new ArgumentNullException(nameof(column));
            if ((uint)columnIndex >= ColumnCount)
            {
                throw new ArgumentOutOfRangeException(nameof(columnIndex));
            }
            if (RowCount > 0 && column.Length != RowCount)
            {
                throw new ArgumentException(strings.MismatchedColumnLengths, nameof(column));
            }
            if (_columnNameToIndexDictionary.ContainsKey(column.Name))
            {
                throw new ArgumentException($"Table already contains a column called {column.Name}");
            }
            _columnNameToIndexDictionary.Remove(_columnNames[columnIndex]);
            _columnNames[columnIndex] = column.Name;
            _columnNameToIndexDictionary[column.Name] = columnIndex;
            _columns[columnIndex] = column;
        }

        public void RemoveColumn(int columnIndex)
        {
            _columnNameToIndexDictionary.Remove(_columnNames[columnIndex]);
            _columnNames.RemoveAt(columnIndex);
            _columns.RemoveAt(columnIndex);
            ColumnCount--;
        }

        public void RemoveColumn(string columnName)
        {
            int columnIndex = GetColumnIndex(columnName);
            if (columnIndex != -1)
            {
                RemoveColumn(columnIndex);
            }
        }

        public int GetColumnIndex(string columnName)
        {
            if (_columnNameToIndexDictionary.TryGetValue(columnName, out int columnIndex))
            {
                return columnIndex;
            }
            return -1;
        }

    }
}
