// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;

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

        private void SetSuffixForDuplicatedColumnNames(DataFrame dataFrame, BaseColumn column, string leftSuffix, string rightSuffix)
        {
            int index = dataFrame._table.GetColumnIndex(column.Name);
            while (index != -1)
            {
                // Pre-existing column. Change name
                BaseColumn existingColumn = dataFrame.Column(index);
                dataFrame._table.SetColumnName(existingColumn, existingColumn.Name + leftSuffix);
                column.SetName(column.Name + rightSuffix);
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

        private void AppendForMerge(DataFrame dataFrame, long dataFrameRow, DataFrame left, DataFrame right, long leftRow, long rightRow)
        {
            for (int i = 0; i < left.ColumnCount; i++)
            {
                BaseColumn leftColumn = left.Column(i);
                BaseColumn column = dataFrame.Column(i);
                if (leftRow == -1)
                {
                    column[dataFrameRow] = null;
                }
                else
                {
                    column[dataFrameRow] = leftColumn[leftRow];
                }
            }
            for (int i = 0; i < right.ColumnCount; i++)
            {
                BaseColumn rightColumn = right.Column(i);
                BaseColumn column = dataFrame.Column(i + left.ColumnCount);
                if (rightRow == -1)
                {
                    column[dataFrameRow] = null;
                }
                else
                {
                    column[dataFrameRow] = rightColumn[rightRow];
                }
            }
        }

        // TODO: Merge API with an "On" parameter that merges on a column common to 2 dataframes 

        /// <summary> 
        /// Merge DataFrames with a database style join 
        /// </summary> 
        /// <param name="other"></param> 
        /// <param name="leftJoinColumn"></param> 
        /// <param name="rightJoinColumn"></param> 
        /// <param name="leftSuffix"></param> 
        /// <param name="rightSuffix"></param> 
        /// <param name="joinAlgorithm"></param> 
        /// <returns></returns> 
        public DataFrame Merge<TKey>(DataFrame other, string leftJoinColumn, string rightJoinColumn, string leftSuffix = "_left", string rightSuffix = "_right", JoinAlgorithm joinAlgorithm = JoinAlgorithm.Left)
        {
            // A simple hash join 
            DataFrame ret = new DataFrame();
            PrimitiveColumn<long> emptyMap = new PrimitiveColumn<long>("Empty");
            for (int i = 0; i < ColumnCount; i++)
            {
                // Create empty columns 
                BaseColumn column = Column(i).Clone(emptyMap);
                ret.InsertColumn(ret.ColumnCount, column);
            }

            for (int i = 0; i < other.ColumnCount; i++)
            {
                // Create empty columns 
                BaseColumn column = other.Column(i).Clone(emptyMap);
                SetSuffixForDuplicatedColumnNames(ret, column, leftSuffix, rightSuffix);
                ret.InsertColumn(ret.ColumnCount, column);
            }

            // The final table size is not known until runtime 
            long rowNumber = 0;
            if (joinAlgorithm == JoinAlgorithm.Left)
            {
                // First hash other dataframe on the rightJoinColumn 
                BaseColumn otherColumn = other[rightJoinColumn];
                Dictionary<TKey, ICollection<long>> multimap = otherColumn.GroupColumnValues<TKey>();

                // Go over the records in this dataframe and match with the dictionary 
                BaseColumn thisColumn = this[leftJoinColumn];
                for (int c = 0; c < ret.ColumnCount; c++)
                {
                    ret.Column(c).Resize(thisColumn.Length);
                }

                for (long i = 0; i < thisColumn.Length; i++)
                {
                    if (rowNumber >= thisColumn.Length)
                    {
                        for (int c = 0; c < ret.ColumnCount; c++)
                        {
                            ret.Column(c).Resize(rowNumber + 1);
                        }
                    }
                    TKey value = (TKey)(thisColumn[i] ?? default(TKey));
                    if (multimap.TryGetValue(value, out ICollection<long> rowNumbers))
                    {
                        foreach (long row in rowNumbers)
                        {
                            if (thisColumn[i] == null)
                            {
                                // Match only with nulls in otherColumn 
                                if (otherColumn[row] == null)
                                {
                                    AppendForMerge(ret, rowNumber++, this, other, i, row);
                                }
                            }
                            else
                            {
                                // Cannot match nulls in otherColumn 
                                if (otherColumn[row] != null)
                                {
                                    AppendForMerge(ret, rowNumber++, this, other, i, row);
                                }
                            }
                        }
                    }
                    else
                    {
                        AppendForMerge(ret, rowNumber++, this, other, i, -1);
                    }
                }
                ret._table.RowCount = rowNumber;
            }
            else if (joinAlgorithm == JoinAlgorithm.Right)
            {
                BaseColumn thisColumn = this[leftJoinColumn];
                Dictionary<TKey, ICollection<long>> multimap = thisColumn.GroupColumnValues<TKey>();

                BaseColumn otherColumn = other[rightJoinColumn];
                for (int c = 0; c < ret.ColumnCount; c++)
                {
                    ret.Column(c).Resize(otherColumn.Length);
                }

                for (long i = 0; i < otherColumn.Length; i++)
                {
                    if (rowNumber >= otherColumn.Length)
                    {
                        for (int c = 0; c < ret.ColumnCount; c++)
                        {
                            ret.Column(c).Resize(rowNumber + 1);
                        }
                    }
                    TKey value = (TKey)(otherColumn[i] ?? default(TKey));
                    if (multimap.TryGetValue(value, out ICollection<long> rowNumbers))
                    {
                        foreach (long row in rowNumbers)
                        {
                            if (otherColumn[i] == null)
                            {
                                if (thisColumn[row] == null)
                                {
                                    AppendForMerge(ret, rowNumber++, this, other, row, i);
                                }
                            }
                            else
                            {
                                if (thisColumn[row] != null)
                                {
                                    AppendForMerge(ret, rowNumber++, this, other, row, i);
                                }
                            }
                        }
                    }
                    else
                    {
                        AppendForMerge(ret, rowNumber++, this, other, -1, i);
                    }
                }
                ret._table.RowCount = rowNumber;
            }
            else if (joinAlgorithm == JoinAlgorithm.Inner)
            {
                // Hash the column with the smaller RowCount 
                long leftRowCount = RowCount;
                long rightRowCount = other.RowCount;
                DataFrame longerDataFrame = leftRowCount < rightRowCount ? other : this;
                DataFrame shorterDataFrame = ReferenceEquals(longerDataFrame, this) ? other : this;
                BaseColumn hashColumn = (leftRowCount < rightRowCount) ? this[leftJoinColumn] : other[rightJoinColumn];
                BaseColumn otherColumn = ReferenceEquals(hashColumn, this[leftJoinColumn]) ? other[rightJoinColumn] : this[leftJoinColumn];
                Dictionary<TKey, ICollection<long>> multimap = hashColumn.GroupColumnValues<TKey>();

                for (int c = 0; c < ret.ColumnCount; c++)
                {
                    ret.Column(c).Resize(1);
                }

                for (long i = 0; i < otherColumn.Length; i++)
                {
                    if (rowNumber >= ret.Column(0).Length)
                    {
                        for (int c = 0; c < ret.ColumnCount; c++)
                        {
                            ret.Column(c).Resize(rowNumber + 1);
                        }
                    }
                    TKey value = (TKey)(otherColumn[i] ?? default(TKey));
                    if (multimap.TryGetValue(value, out ICollection<long> rowNumbers))
                    {
                        foreach (long row in rowNumbers)
                        {
                            if (otherColumn[i] == null)
                            {
                                if (hashColumn[row] == null)
                                {
                                    AppendForMerge(ret, rowNumber++, this, other, ReferenceEquals(this, shorterDataFrame) ? row : i, ReferenceEquals(this, shorterDataFrame) ? i : row);
                                }
                            }
                            else
                            {
                                if (hashColumn[row] != null)
                                {
                                    AppendForMerge(ret, rowNumber++, this, other, ReferenceEquals(this, shorterDataFrame) ? row : i, ReferenceEquals(this, shorterDataFrame) ? i : row);
                                }
                            }
                        }
                    }
                }
                ret._table.RowCount = rowNumber;
            }
            else if (joinAlgorithm == JoinAlgorithm.FullOuter)
            {
                BaseColumn otherColumn = other[rightJoinColumn];
                Dictionary<TKey, ICollection<long>> multimap = otherColumn.GroupColumnValues<TKey>();
                Dictionary<TKey, long> intersection = new Dictionary<TKey, long>(EqualityComparer<TKey>.Default);

                // Go over the records in this dataframe and match with the dictionary 
                BaseColumn thisColumn = this[leftJoinColumn];
                for (int c = 0; c < ret.ColumnCount; c++)
                {
                    ret.Column(c).Resize(thisColumn.Length + 1);
                }

                for (long i = 0; i < thisColumn.Length; i++)
                {
                    if (rowNumber >= thisColumn.Length)
                    {
                        for (int c = 0; c < ret.ColumnCount; c++)
                        {
                            ret.Column(c).Resize(rowNumber + 1);
                        }
                    }
                    TKey value = (TKey)(thisColumn[i] ?? default(TKey));
                    if (multimap.TryGetValue(value, out ICollection<long> rowNumbers))
                    {
                        foreach (long row in rowNumbers)
                        {
                            if (thisColumn[i] == null)
                            {
                                // Has to match only with nulls in otherColumn 
                                if (otherColumn[row] == null)
                                {
                                    AppendForMerge(ret, rowNumber++, this, other, i, row);
                                    if (!intersection.ContainsKey(value))
                                    {
                                        intersection.Add(value, rowNumber);
                                    }
                                }
                            }
                            else
                            {
                                // Cannot match to nulls in otherColumn 
                                if (otherColumn[row] != null)
                                {
                                    AppendForMerge(ret, rowNumber++, this, other, i, row);
                                    if (!intersection.ContainsKey(value))
                                    {
                                        intersection.Add(value, rowNumber);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        AppendForMerge(ret, rowNumber++, this, other, i, -1);
                    }
                }
                for (long i = 0; i < otherColumn.Length; i++)
                {
                    if (rowNumber >= ret.Column(0).Length)
                    {
                        for (int c = 0; c < ret.ColumnCount; c++)
                        {
                            ret.Column(c).Resize(rowNumber + 1);
                        }
                    }
                    TKey value = (TKey)(otherColumn[i] ?? default(TKey));
                    if (!intersection.ContainsKey(value))
                    {
                        if (rowNumber >= otherColumn.Length)
                        {
                            for (int c = 0; c < ret.ColumnCount; c++)
                            {
                                ret.Column(c).Resize(rowNumber + 1);
                            }
                        }
                        AppendForMerge(ret, rowNumber++, this, other, -1, i);
                    }
                }
                ret._table.RowCount = rowNumber;
            }
            return ret;
        }

    }
}
