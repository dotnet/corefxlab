// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections;
using System.Collections.Generic;

namespace Microsoft.Data.Analysis
{
    /// <summary>
    /// Represents the rows of a <see cref="DataFrame"/>
    /// </summary>
    public class DataFrameRowCollection : IEnumerable<DataFrameRow>
    {
        private long _currentRowIndex;
        private DataFrame _dataFrame;

        /// <summary>
        /// Initializes a <see cref="DataFrameRowCollection"/>.
        /// </summary>
        internal DataFrameRowCollection(DataFrame dataFrame)
        {
            _dataFrame = dataFrame ?? throw new ArgumentNullException(nameof(dataFrame));
        }

        /// <summary>
        /// An indexer to return the <see cref="DataFrameRow"/> at <paramref name="index"/>
        /// </summary>
        /// <param name="index">The row index</param>
        public DataFrameRow this[long index]
        {
            get
            {
                return new DataFrameRow(_dataFrame, index);
            }
        }

        internal List<object> GetRow(long rowIndex)
        {
            DataFrameColumnCollection columns = _dataFrame.Columns;
            List<object> ret = new List<object>(columns.Count);
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
            while (_currentRowIndex < _dataFrame.RowCount)
            {
                yield return new DataFrameRow(_dataFrame, _currentRowIndex);
                _currentRowIndex++;
            }
        }

        /// <summary>
        /// The number of rows in this <see cref="DataFrame"/>.
        /// </summary>
        public long Length => _dataFrame.RowCount;

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
