// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Data.Analysis
{

    /// <summary>
    /// Represents the rows of a <see cref="DataFrame"/>
    /// </summary>
    public class DataFrameRowCollection : IEnumerable<DataFrameRow>
    {
        private long _currentIndex = 0;
        private DataFrame _dataFrame;

        /// <summary>
        /// Initializes a <see cref="DataFrameRowCollection"/>.
        /// </summary>
        internal DataFrameRowCollection(DataFrame dataFrame)
        {
            _dataFrame = dataFrame;
        }

        /// <summary>
        /// An indexer to return the <see cref="DataFrameRow"/> at <paramref name="index"/>
        /// </summary>
        /// <param name="index">The row index</param>
        public DataFrameRow this[long index]
        {
            get
            {
                List<object> row = GetRow(index);
                return new DataFrameRow(row);
            }
        }

        internal List<object> GetRow(long rowIndex)
        {
            var ret = new List<object>();
            var columns = _dataFrame.Columns;
            foreach (var column in columns)
            {
                ret.Add(column[rowIndex]);
            }
            return ret;
        }

        /// <summary>
        /// Returns an enumerator of <see cref="DataFrameRow"/> objects
        /// </summary>
        public IEnumerator<DataFrameRow> GetEnumerator()
        {
            DataFrameRow _currentRow = new DataFrameRow(null);
            while (_currentIndex < _dataFrame.RowCount)
            {
                _currentRow._row = GetRow(_currentIndex);
                yield return _currentRow;
                _currentIndex++;
            }
        }

        /// <summary>
        /// The number of rows in this <see cref="DataFrame"/>.
        /// </summary>
        public long Length => _dataFrame.RowCount;

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }


    /// <summary>
    /// A DataFrameRow is a collection of values that represent a row in a <see cref="DataFrame"/>.
    /// </summary>
    public class DataFrameRow : IEnumerable<object>
    {
        internal List<object> _row;
        internal DataFrameRow(List<object> row)
        {
            _row = row;
        }

        /// <summary>
        /// Returns an enumerator of the values in this row.
        /// </summary>
        public IEnumerator<object> GetEnumerator()
        {
            foreach (object value in _row)
            {
                yield return value;
            }
        }

        /// <summary>
        /// An indexer to return the value at <paramref name="index"/>.
        /// </summary>
        /// <param name="index">The index of the value to return</param>
        /// <returns>The value at this <paramref name="index"/>.</returns>
        public object this[int index]
        {
            get
            {
                return _row[index];
            }
        }

        /// <summary>
        /// The number of values in this row.
        /// </summary>
        public int Count => _row.Count;

        /// <summary>
        /// A simple string representation of the values in this row
        /// </summary>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (object value in _row)
            {
                sb.Append(value != null ? value.ToString() : "null").Append(" ");
            }
            return sb.ToString();
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
