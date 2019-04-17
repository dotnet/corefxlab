// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;

namespace Microsoft.Data
{
    /// <summary>
    /// A DataFrame 
    /// </summary>
    public partial class DataFrame
    {
        internal DataFrameTable _table;
        public DataFrame()
        {
            _table = new DataFrameTable();
        }

        public long RowCount => _table.RowCount;
        public int ColumnCount => _table.ColumnCount;
        public IList<string> Columns()
        {
            var ret = new List<string>();
            for (int i = 0; i < ColumnCount; i++)
            {
                ret.Add(_table.Column(i).Name);
            }
            return ret;
        }
        public BaseDataFrameColumn Column(int index) => _table.Column(index);
        public void InsertColumn(int columnIndex, BaseDataFrameColumn column) => _table.InsertColumn(columnIndex, column);
        public void SetColumn(int columnIndex, BaseDataFrameColumn column) => _table.SetColumn(columnIndex, column);
        public void RemoveColumn(int columnIndex) => _table.RemoveColumn(columnIndex);
        public void RemoveColumn(string columnName) => _table.RemoveColumn(columnName);
        
        public object this[long rowIndex, int columnIndex]
        {
            get
            {
                return _table.Column(columnIndex)[rowIndex];
            }
            set
            {
                _table.Column(columnIndex)[rowIndex] = value;
            }
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

        public object this[string columnName]
        {
            get
            {
                int columnIndex = _table.GetColumnIndex(columnName);
                if (columnIndex == -1) throw new ArgumentException($"{columnName} does not exist");
                return _table.Column(columnIndex); //[0, (int)Math.Min(_table.NumRows, Int32.MaxValue)];
            }
        }

        public IList<IList<object>> Head(int numberOfRows)
        {
            var ret = new List<IList<object>>();
            for (int i= 0; i< numberOfRows; i++)
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
    }
}
