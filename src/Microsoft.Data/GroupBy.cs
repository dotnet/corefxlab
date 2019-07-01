// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
//using System.Linq;
using Microsoft.Collections.Extensions;

namespace Microsoft.Data
{
    /// <summary>
    /// A GroupBy class that is typically the product of a DataFrame.GroupBy call
    /// It holds information to perform typical aggregation ops on it
    /// </summary>
    public abstract class GroupBy
    {
        /// <summary>
        /// Returns true if all values in the group are true
        /// </summary>
        /// <returns></returns>
        public abstract DataFrame All();

        /// <summary>
        /// Compute the number of values in each group 
        /// </summary>
        /// <returns></returns>
        public abstract DataFrame Count();

        /// <summary>
        /// Return the first value in each group
        /// </summary>
        /// <returns></returns>
        public abstract DataFrame First();

        /// <summary>
        /// Returns the first numberOfRows rows of each group
        /// </summary>
        /// <param name="numberOfRowsInEachGroup"></param>
        /// <returns></returns>
        public abstract DataFrame Head(int numberOfRows);

        /// <summary>
        /// Returns the last numberOfRows rows of each group
        /// </summary>
        /// <param name="numberOfRowsInEachGroup"></param>
        /// <returns></returns>
        public abstract DataFrame Tail(int numberOfRows);

        /// <summary>
        /// Compute the max of group values
        /// </summary>
        /// <returns></returns>
        public abstract DataFrame Max();
        public abstract DataFrame Min();
        public abstract DataFrame Product();
        public abstract DataFrame Sum();
    }

    public class GroupBy<TKey> : GroupBy
    {
        private int _groupByColumnIndex;
        private MultiValueDictionary<TKey, long> _keyValuePairs;
        private DataFrame _dataFrame;
        private PrimitiveColumn<long> _mapIndices;

        public GroupBy(DataFrame dataFrame, int groupByColumnIndex, MultiValueDictionary<TKey, long> keyValuePairs)
        {
            _groupByColumnIndex = groupByColumnIndex;
            _keyValuePairs = keyValuePairs;
            _dataFrame = dataFrame;
        }

        public override DataFrame All()
        {
            throw new NotImplementedException();
        }

        internal delegate void ColumnDelegate(int columnIndex, long rowIndex, IEnumerable<long> rows, TKey key, bool firstGroup);
        internal delegate void GroupByColumnDelegate(long rowNumber, TKey key);
        internal void EnumerateColumnsWithRows(GroupByColumnDelegate groupByColumnDelegate, ColumnDelegate columnDelegate)
        {
            long rowNumber = 0;
            bool firstGroup = true;
            foreach (KeyValuePair<TKey, IReadOnlyCollection<long>> pairs in _keyValuePairs)
            {
                groupByColumnDelegate(rowNumber, pairs.Key);
                IReadOnlyCollection<long> rows = pairs.Value;
                // Assuming that the dataframe has not been modified after the groupby call
                int numberOfColumns = _dataFrame.ColumnCount;
                for (int i = 0; i < numberOfColumns; i++)
                {
                    columnDelegate(i, rowNumber, rows, pairs.Key, firstGroup);
                }
                firstGroup = false;
                rowNumber++;
            }

        }

        public override DataFrame Count()
        {
            DataFrame ret = new DataFrame();
            PrimitiveColumn<long> empty = new PrimitiveColumn<long>("Empty");
            BaseColumn firstColumn = _dataFrame.Column(_groupByColumnIndex).Clone(empty);
            ret.InsertColumn(ret.ColumnCount, firstColumn);
            GroupByColumnDelegate groupByColumnDelegate = new GroupByColumnDelegate((long rowIndex, TKey key) =>
            {
                firstColumn.Resize(rowIndex + 1);
                firstColumn[rowIndex] = key;
            });
            ColumnDelegate columnDelegate = new ColumnDelegate((int columnIndex, long rowIndex, IEnumerable<long> rowEnumerable, TKey key, bool firstGroup) =>
            {
                if (columnIndex == _groupByColumnIndex)
                    return;
                BaseColumn column = _dataFrame.Column(columnIndex);
                long count = 0;
                IEnumerator<long> rows = rowEnumerable.GetEnumerator();
                while (rows.MoveNext())
                {
                    long row = rows.Current;
                    if (column[row] != null)
                        count++;
                }
                rows.Reset();
                BaseColumn retColumn;
                if (firstGroup)
                {
                    retColumn = new PrimitiveColumn<long>(column.Name);
                    ret.InsertColumn(ret.ColumnCount, retColumn);
                }
                else
                {
                    // Assuming non duplicate column names
                    retColumn = ret[column.Name];
                }
                retColumn.Resize(rowIndex + 1);
                retColumn[rowIndex] = count;
            });

            EnumerateColumnsWithRows(groupByColumnDelegate, columnDelegate);
            ret.SetTableRowCount(firstColumn.Length);

            return ret;
        }


        public override DataFrame First()
        {
            DataFrame ret = new DataFrame();
            PrimitiveColumn<long> empty = new PrimitiveColumn<long>("Empty");
            BaseColumn firstColumn = _dataFrame.Column(_groupByColumnIndex).Clone(empty);
            ret.InsertColumn(ret.ColumnCount, firstColumn);

            GroupByColumnDelegate groupByColumnDelegate = new GroupByColumnDelegate((long rowIndex, TKey key) =>
            {
                firstColumn.Resize(rowIndex + 1);
                firstColumn[rowIndex] = key;
            });

            ColumnDelegate columnDelegate = new ColumnDelegate((int columnIndex, long rowIndex, IEnumerable<long> rowEnumerable, TKey key, bool firstGroup) =>
            {
                if (columnIndex == _groupByColumnIndex)
                    return;
                BaseColumn column = _dataFrame.Column(columnIndex);
                long count = 0;
                IEnumerator<long> rows = rowEnumerable.GetEnumerator();
                while (rows.MoveNext() && count < 1)
                {
                    count++;
                    long row = rows.Current;
                    BaseColumn retColumn;
                    if (firstGroup)
                    {
                        retColumn = column.Clone(empty);
                        ret.InsertColumn(ret.ColumnCount, retColumn);
                    }
                    else
                    {
                        // Assuming non duplicate column names
                        retColumn = ret[column.Name];
                    }
                    retColumn.Resize(rowIndex + 1);
                    retColumn[rowIndex] = column[row];
                }
            });

            EnumerateColumnsWithRows(groupByColumnDelegate, columnDelegate);
            ret.SetTableRowCount(firstColumn.Length);
            return ret;
        }

        public override DataFrame Head(int numberOfRows)
        {
            DataFrame ret = new DataFrame();
            PrimitiveColumn<long> empty = new PrimitiveColumn<long>("Empty");
            BaseColumn firstColumn = _dataFrame.Column(_groupByColumnIndex).Clone(empty);
            ret.InsertColumn(ret.ColumnCount, firstColumn);

            GroupByColumnDelegate groupByColumnDelegate = new GroupByColumnDelegate((long rowIndex, TKey key) =>
            {
            });

            ColumnDelegate columnDelegate = new ColumnDelegate((int columnIndex, long rowIndex, IEnumerable<long> rowEnumerable, TKey key, bool firstGroup) =>
            {
                if (columnIndex == _groupByColumnIndex)
                    return;
                BaseColumn column = _dataFrame.Column(columnIndex);
                long count = 0;
                bool firstRow = true;
                IEnumerator<long> rows = rowEnumerable.GetEnumerator();
                while (rows.MoveNext() && count < numberOfRows)
                {
                    long row = rows.Current;
                    BaseColumn retColumn;
                    if (firstGroup && firstRow)
                    {
                        firstRow = false;
                        retColumn = column.Clone(empty);
                        ret.InsertColumn(ret.ColumnCount, retColumn);
                    }
                    else
                    {
                        // Assuming non duplicate column names
                        retColumn = ret[column.Name];
                    }
                    long retColumnLength = retColumn.Length;
                    retColumn.Resize(retColumnLength + 1);
                    retColumn[retColumnLength] = column[row];
                    if (firstColumn.Length <= retColumnLength)
                    {
                        firstColumn.Resize(retColumnLength + 1);
                    }
                    firstColumn[retColumnLength] = key;
                    count++;
                }
            });

            EnumerateColumnsWithRows(groupByColumnDelegate, columnDelegate);
            ret.SetTableRowCount(firstColumn.Length);
            return ret;
        }

        public override DataFrame Tail(int numberOfRows)
        {
            DataFrame ret = new DataFrame();
            PrimitiveColumn<long> empty = new PrimitiveColumn<long>("Empty");
            BaseColumn firstColumn = _dataFrame.Column(_groupByColumnIndex).Clone(empty);
            ret.InsertColumn(ret.ColumnCount, firstColumn);

            GroupByColumnDelegate groupByColumnDelegate = new GroupByColumnDelegate((long rowIndex, TKey key) =>
            {
            });

            ColumnDelegate columnDelegate = new ColumnDelegate((int columnIndex, long rowIndex, IEnumerable<long> rowEnumerable, TKey key, bool firstGroup) =>
            {
                if (columnIndex == _groupByColumnIndex)
                    return;
                BaseColumn column = _dataFrame.Column(columnIndex);
                long count = 0;
                bool firstRow = true;
                IReadOnlyCollection<long> values = _keyValuePairs[key];
                int numberOfValues = values.Count;
                IEnumerator<long> rows = rowEnumerable.GetEnumerator();
                while (rows.MoveNext())
                {
                    if (count >= numberOfValues - numberOfRows)
                    {
                        long row = rows.Current;
                        BaseColumn retColumn;
                        if (firstGroup && firstRow)
                        {
                            firstRow = false;
                            retColumn = column.Clone(empty);
                            ret.InsertColumn(ret.ColumnCount, retColumn);
                        }
                        else
                        {
                            // Assuming non duplicate column names
                            retColumn = ret[column.Name];
                        }
                        long retColumnLength = retColumn.Length;
                        if (firstColumn.Length <= retColumnLength)
                        {
                            firstColumn.Resize(retColumnLength + 1);
                            firstColumn[retColumnLength] = key;
                        }
                        retColumn.Resize(retColumnLength + 1);
                        retColumn[retColumnLength] = column[row];
                    }
                    count++;
                }
            });

            EnumerateColumnsWithRows(groupByColumnDelegate, columnDelegate);
            ret.SetTableRowCount(firstColumn.Length);
            return ret;
        }

        private delegate object columnFunc(IEnumerable<long> rows);
        private BaseColumn ResizeAndInsertColumn(int columnIndex, long rowIndex, bool firstGroup, DataFrame ret, PrimitiveColumn<long> empty)
        {
            if (columnIndex == _groupByColumnIndex)
                return null;
            BaseColumn column = _dataFrame.Column(columnIndex);
            BaseColumn retColumn;
            if (firstGroup)
            {
                retColumn = column.Clone(empty);
                ret.InsertColumn(ret.ColumnCount, retColumn);
            }
            else
            {
                // Assuming unique column names
                retColumn = ret[column.Name];
            }
            retColumn.Resize(rowIndex + 1);
            return retColumn;
        }

        public override DataFrame Max()
        {
            DataFrame ret = new DataFrame();
            PrimitiveColumn<long> empty = new PrimitiveColumn<long>("Empty");
            BaseColumn firstColumn = _dataFrame.Column(_groupByColumnIndex).Clone(empty);
            ret.InsertColumn(ret.ColumnCount, firstColumn);
            GroupByColumnDelegate groupByColumnDelegate = new GroupByColumnDelegate((long rowIndex, TKey key) =>
            {
                firstColumn.Resize(rowIndex + 1);
                firstColumn[rowIndex] = key;
            });

            ColumnDelegate columnDelegate = new ColumnDelegate((int columnIndex, long rowIndex, IEnumerable<long> rowEnumerable, TKey key, bool firstGroup) =>
            {
                BaseColumn retColumn = ResizeAndInsertColumn(columnIndex, rowIndex, firstGroup, ret, empty);

                if (!ReferenceEquals(retColumn, null))
                {
                    retColumn[rowIndex] = _dataFrame.Column(columnIndex).Max(rowEnumerable);
                }
            });

            EnumerateColumnsWithRows(groupByColumnDelegate, columnDelegate);
            ret.SetTableRowCount(firstColumn.Length);

            return ret;
        }

        public override DataFrame Min()
        {
            DataFrame ret = new DataFrame();
            PrimitiveColumn<long> empty = new PrimitiveColumn<long>("Empty");
            BaseColumn firstColumn = _dataFrame.Column(_groupByColumnIndex).Clone(empty);
            ret.InsertColumn(ret.ColumnCount, firstColumn);
            GroupByColumnDelegate groupByColumnDelegate = new GroupByColumnDelegate((long rowIndex, TKey key) =>
            {
                firstColumn.Resize(rowIndex + 1);
                firstColumn[rowIndex] = key;
            });

            ColumnDelegate columnDelegate = new ColumnDelegate((int columnIndex, long rowIndex, IEnumerable<long> rowEnumerable, TKey key, bool firstGroup) =>
            {
                BaseColumn retColumn = ResizeAndInsertColumn(columnIndex, rowIndex, firstGroup, ret, empty);

                if (!ReferenceEquals(retColumn, null))
                {
                    retColumn[rowIndex] = _dataFrame.Column(columnIndex).Min(rowEnumerable);
                }
            });

            EnumerateColumnsWithRows(groupByColumnDelegate, columnDelegate);
            ret.SetTableRowCount(firstColumn.Length);

            return ret;
        }

        public override DataFrame Product()
        {
            DataFrame ret = new DataFrame();
            PrimitiveColumn<long> empty = new PrimitiveColumn<long>("Empty");
            BaseColumn firstColumn = _dataFrame.Column(_groupByColumnIndex).Clone(empty);
            ret.InsertColumn(ret.ColumnCount, firstColumn);
            GroupByColumnDelegate groupByColumnDelegate = new GroupByColumnDelegate((long rowIndex, TKey key) =>
            {
                firstColumn.Resize(rowIndex + 1);
                firstColumn[rowIndex] = key;
            });

            ColumnDelegate columnDelegate = new ColumnDelegate((int columnIndex, long rowIndex, IEnumerable<long> rowEnumerable, TKey key, bool firstGroup) =>
            {
                BaseColumn retColumn = ResizeAndInsertColumn(columnIndex, rowIndex, firstGroup, ret, empty);

                if (!ReferenceEquals(retColumn, null))
                {
                    retColumn[rowIndex] = _dataFrame.Column(columnIndex).Product(rowEnumerable);
                }
            });

            EnumerateColumnsWithRows(groupByColumnDelegate, columnDelegate);
            ret.SetTableRowCount(firstColumn.Length);

            return ret;
        }

        public override DataFrame Sum()
        {
            DataFrame ret = new DataFrame();
            PrimitiveColumn<long> empty = new PrimitiveColumn<long>("Empty");
            BaseColumn firstColumn = _dataFrame.Column(_groupByColumnIndex).Clone(empty);
            ret.InsertColumn(ret.ColumnCount, firstColumn);
            GroupByColumnDelegate groupByColumnDelegate = new GroupByColumnDelegate((long rowIndex, TKey key) =>
            {
                firstColumn.Resize(rowIndex + 1);
                firstColumn[rowIndex] = key;
            });

            ColumnDelegate columnDelegate = new ColumnDelegate((int columnIndex, long rowIndex, IEnumerable<long> rowEnumerable, TKey key, bool firstGroup) =>
            {
                BaseColumn retColumn = ResizeAndInsertColumn(columnIndex, rowIndex, firstGroup, ret, empty);

                if (!ReferenceEquals(retColumn, null))
                {
                    retColumn[rowIndex] = _dataFrame.Column(columnIndex).Sum(rowEnumerable);
                }
            });

            EnumerateColumnsWithRows(groupByColumnDelegate, columnDelegate);
            ret.SetTableRowCount(firstColumn.Length);

            return ret;
        }

    }
}
