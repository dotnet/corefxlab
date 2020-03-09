// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Linq;
using BenchmarkDotNet.Attributes;
using Microsoft.Data.Analysis;

namespace Benchmarks.Microsoft.Data.Analysis
{
    public class DataFrameFunctions
    {
        private DataFrame _dataFrame;
        private DataFrameColumn _column;
        private DataFrameColumn _string;
        private DataFrameColumn _bool;

        private DataFrame _otherDataFrame;
        private DataFrameColumn _otherColumn;
        private DataFrameColumn _otherString;
        private DataFrameColumn _otherBool;


        [GlobalSetup]
        public void Setup()
        {
            int length = 50000;
            _dataFrame = new DataFrame();
            _column = new PrimitiveDataFrameColumn<int>("Int0", Enumerable.Range(0, length));
            _string = new StringDataFrameColumn("String", Enumerable.Range(0, length).Select(x => x.ToString()));
            _bool = new PrimitiveDataFrameColumn<bool>("Bool", Enumerable.Range(0, length).Select(x => x % 2 == 0 ? true : false));
            _dataFrame.Columns.Insert(0, _column);
            _dataFrame.Columns.Insert(1, _string);
            _dataFrame.Columns.Insert(2, _bool);

            _otherDataFrame = new DataFrame();
            _otherColumn = new PrimitiveDataFrameColumn<int>("Int0", Enumerable.Range(0, length/2));
            _otherString = new StringDataFrameColumn("String", Enumerable.Range(0, length/2).Select(x => x.ToString()));
            _otherBool = new PrimitiveDataFrameColumn<bool>("Bool", Enumerable.Range(0, length/2).Select(x => x % 2 == 0 ? true : false));
            _otherDataFrame.Columns.Insert(0, _otherColumn);
            _otherDataFrame.Columns.Insert(1, _otherString);
            _otherDataFrame.Columns.Insert(2, _otherBool);
            
        }

        [Benchmark]
        public void ColumnIndexer()
        {
            for (long i = 0; i < _column.Length; i++)
            {
                _column[i] = _column[i];
            }
        }

        [Benchmark]
        public DataFrameColumn ColumnSort()
        {
            return _column.Sort(false);
        }

        [Benchmark]
        public DataFrame OrderBy()
        {
            return _dataFrame.OrderBy("Int0");
        }

        [Benchmark]
        public DataFrame OrderByDescending()
        {
            return _dataFrame.OrderByDescending("Int0");
        }

        [Benchmark]
        public DataFrame Clamp()
        {
            return _dataFrame.Clamp(10000, 40000);
        }

        [Benchmark]
        public GroupBy GroupBy()
        {
            return _dataFrame.GroupBy("Bool");
        }

        [Benchmark]
        public DataFrameColumn ColumnFillNulls()
        {
            return _column.FillNulls(5);
        }

        [Benchmark]
        public DataFrame LeftJoin()
        {
            return _dataFrame.Join(_otherDataFrame);
        }

        [Benchmark]
        public DataFrame RightJoin()
        {
            return _dataFrame.Join(_otherDataFrame, joinAlgorithm: JoinAlgorithm.Right);
        }

        [Benchmark]
        public DataFrame OuterJoin()
        {
            return _dataFrame.Join(_otherDataFrame, joinAlgorithm: JoinAlgorithm.FullOuter);
        }

        [Benchmark]
        public DataFrame InnerJoin()
        {
            return _dataFrame.Join(_otherDataFrame, joinAlgorithm: JoinAlgorithm.Inner);
        }

        [Benchmark]
        public DataFrame LeftMerge()
        {
            return _dataFrame.Merge<int>(_otherDataFrame, "Int0", "Int0");
        }

        [Benchmark]
        public DataFrame RightMerge()
        {
            return _dataFrame.Merge<int>(_otherDataFrame, "Int0", "Int0", joinAlgorithm: JoinAlgorithm.Right);
        }

        [Benchmark]
        public DataFrame OuterMerge()
        {
            return _dataFrame.Merge<int>(_otherDataFrame, "Int0", "Int0", joinAlgorithm: JoinAlgorithm.FullOuter);
        }

        [Benchmark]
        public DataFrame InnerMerge()
        {
            return _dataFrame.Merge<int>(_otherDataFrame, "Int0", "Int0", joinAlgorithm: JoinAlgorithm.Inner);
        }
    }
}
