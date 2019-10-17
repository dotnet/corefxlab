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
        private List<DataFrameColumn> _columns;

        private List<string> _columnNames = new List<string>();

        private Dictionary<string, int> _columnNameToIndexDictionary = new Dictionary<string, int>(StringComparer.Ordinal);

        public long RowCount { get; internal set; }

        public int ColumnCount { get; private set; }

        public DataFrameTable()
        {
            _columns = new List<DataFrameColumn>();
        }

        public DataFrameTable(IList<DataFrameColumn> columns)
        {
            columns = columns ?? throw new ArgumentNullException(nameof(columns));
            _columns = new List<DataFrameColumn>();
            for (int i = 0; i < columns.Count; i++)
            {
                InsertColumn(i, columns[i]);
            }
        }

        public IReadOnlyList<DataFrameColumn> Columns => _columns;

        public IReadOnlyList<string> ColumnNames => _columnNames;

        public DataFrameColumn Column(int columnIndex) => _columns[columnIndex];

        public IList<object> GetRow(long rowIndex)
        {
            var ret = new List<object>();
            for (int i = 0; i < ColumnCount; i++)
            {
                ret.Add(Column(i)[rowIndex]);
            }
            return ret;
        }

        public void SetColumnName(DataFrameColumn column, string newName)
        {
            string currentName = column.Name;
            int currentIndex = _columnNameToIndexDictionary[currentName];
            column.SetName(newName);
            _columnNameToIndexDictionary.Remove(currentName);
            _columnNameToIndexDictionary.Add(newName, currentIndex);
        }

        public void InsertColumn<T>(int columnIndex, IEnumerable<T> column, string columnName)
            where T : unmanaged
        {
            column = column ?? throw new ArgumentNullException(nameof(column));
            if ((uint)columnIndex > _columns.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(columnIndex));
            }
            DataFrameColumn newColumn = new PrimitiveDataFrameColumn<T>(columnName, column);
            InsertColumn(columnIndex, newColumn);
        }

        public void InsertColumn(int columnIndex, DataFrameColumn column)
        {
            column = column ?? throw new ArgumentNullException(nameof(column));
            if ((uint)columnIndex > _columns.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(columnIndex));
            }
            if (RowCount > 0 && column.Length != RowCount)
            {
                throw new ArgumentException(Strings.MismatchedColumnLengths, nameof(column));
            }
            if (_columnNameToIndexDictionary.ContainsKey(column.Name))
            {
                throw new ArgumentException(string.Format(Strings.DuplicateColumnName, column.Name), nameof(column));
            }
            RowCount = column.Length;
            _columnNames.Insert(columnIndex, column.Name);
            _columnNameToIndexDictionary[column.Name] = columnIndex;
            for (int i = columnIndex + 1; i < ColumnCount; i++)
            {
                _columnNameToIndexDictionary[_columnNames[i]]++;
            }
            _columns.Insert(columnIndex, column);
            ColumnCount++;
        }

        public void SetColumn(int columnIndex, DataFrameColumn column)
        {
            column = column ?? throw new ArgumentNullException(nameof(column));
            if ((uint)columnIndex >= ColumnCount)
            {
                throw new ArgumentOutOfRangeException(nameof(columnIndex));
            }
            if (RowCount > 0 && column.Length != RowCount)
            {
                throw new ArgumentException(Strings.MismatchedColumnLengths, nameof(column));
            }
            bool existingColumn = _columnNameToIndexDictionary.TryGetValue(column.Name, out int existingColumnIndex);
            if (existingColumn && existingColumnIndex != columnIndex)
            {
                throw new ArgumentException(string.Format(Strings.DuplicateColumnName, column.Name), nameof(column));
            }
            _columnNameToIndexDictionary.Remove(_columnNames[columnIndex]);
            _columnNames[columnIndex] = column.Name;
            _columnNameToIndexDictionary[column.Name] = columnIndex;
            _columns[columnIndex] = column;
        }

        public void RemoveColumn(int columnIndex)
        {
            _columnNameToIndexDictionary.Remove(_columnNames[columnIndex]);
            for (int i = columnIndex + 1; i < ColumnCount; i++)
            {
                _columnNameToIndexDictionary[_columnNames[i]]--;
            }
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
