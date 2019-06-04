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

        public long RowCount { get; internal set; }

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

        public void SetColumnName(BaseColumn column, string newName)
        {
            string currentName = column.Name;
            int currentIndex = _columnNameToIndexDictionary[currentName];
            column.Name = newName;
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
                throw new ArgumentException(Strings.MismatchedColumnLengths, nameof(column));
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
                throw new ArgumentException(Strings.MismatchedColumnLengths, nameof(column));
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

        public void AppendRow(long index, string[] values)
        {
            if (values.Length != ColumnCount)
            {
                throw new ArgumentException($"Expected values.Length {values.Length} to be the number of columns in the table {ColumnCount}");
            }
            for (int i = 0; i < values.Length; i++)
            {
                string value = values[i];
                BaseColumn column = Column(i);
                bool parse;
                switch (column.DataType)
                {
                    case Type boolType when boolType == typeof(bool):
                        parse = bool.TryParse(value, out bool boolAppend);
                        (column as PrimitiveColumn<bool>).Append(parse ? boolAppend : default);
                        continue;
                    case Type byteType when byteType == typeof(byte):
                        parse = byte.TryParse(value, out byte byteAppend);
                        (column as PrimitiveColumn<byte>).Append(parse ? byteAppend : default);
                        continue;
                    case Type charType when charType == typeof(char):
                        parse = char.TryParse(value, out char charAppend);
                        (column as PrimitiveColumn<char>).Append(parse ? charAppend : default);
                        continue;
                    case Type decimalType when decimalType == typeof(decimal):
                        parse = decimal.TryParse(value, out decimal decimalAppend);
                        (column as PrimitiveColumn<decimal>).Append(parse ? decimalAppend : default);
                        continue;
                    case Type doubleType when doubleType == typeof(double):
                        parse = double.TryParse(value, out double doubleAppend);
                        (column as PrimitiveColumn<double>).Append(parse ? doubleAppend : default);
                        continue;
                    case Type floatType when floatType == typeof(float):
                        parse = float.TryParse(value, out float floatAppend);
                        (column as PrimitiveColumn<float>).Append(parse ? floatAppend : default);
                        continue;
                    case Type intType when intType == typeof(int):
                        parse = int.TryParse(value, out int intAppend);
                        (column as PrimitiveColumn<int>).Append(parse ? intAppend : default);
                        continue;
                    case Type longType when longType == typeof(long):
                        parse = long.TryParse(value, out long longAppend);
                        (column as PrimitiveColumn<long>).Append(parse ? longAppend : default);
                        continue;
                    case Type sbyteType when sbyteType == typeof(sbyte):
                        parse = sbyte.TryParse(value, out sbyte sbyteAppend);
                        (column as PrimitiveColumn<sbyte>).Append(parse ? sbyteAppend : default);
                        continue;
                    case Type shortType when shortType == typeof(short):
                        parse = short.TryParse(value, out short shortAppend);
                        (column as PrimitiveColumn<short>).Append(parse ? shortAppend : default);
                        continue;
                    case Type uintType when uintType == typeof(uint):
                        parse = uint.TryParse(value, out uint uintAppend);
                        (column as PrimitiveColumn<uint>).Append(parse ? uintAppend : default);
                        continue;
                    case Type ulongType when ulongType == typeof(ulong):
                        parse = ulong.TryParse(value, out ulong ulongAppend);
                        (column as PrimitiveColumn<ulong>).Append(parse ? ulongAppend : default);
                        continue;
                    case Type ushortType when ushortType == typeof(ushort):
                        parse = ushort.TryParse(value, out ushort ushortAppend);
                        (column as PrimitiveColumn<ushort>).Append(parse ? ushortAppend : default);
                        continue;
                    case Type stringType when stringType == typeof(string):
                        (column as StringColumn).Append(string.IsNullOrEmpty(value) ? null : value);
                        continue;
                    default:
                        long currentColumnLength = column.Length;
                        column.Resize(currentColumnLength + 1);
                        column[currentColumnLength] = value;
                        continue;
                }
            }
            RowCount++;
        }
    }
}
