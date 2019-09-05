// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Linq;
using BenchmarkDotNet.Attributes;
using Microsoft.Data;

namespace Benchmarks.Microsoft.Data
{
    public class DataFrameFunctions
    {
        private DataFrame _dataFrame;
        private BaseColumn _column;
        private BaseColumn _string;
        private BaseColumn _bool;

        private DataFrame _otherDataFrame;
        private BaseColumn _otherColumn;
        private BaseColumn _otherString;
        private BaseColumn _otherBool;


        [GlobalSetup]
        public void Setup()
        {
            int length = 50000;
            _dataFrame = new DataFrame();
            _column = new PrimitiveColumn<int>("Int0", Enumerable.Range(0, length));
            _string = new StringColumn("String", Enumerable.Range(0, length).Select(x => x.ToString()));
            _bool = new PrimitiveColumn<bool>("Bool", Enumerable.Range(0, length).Select(x => x % 2 == 0 ? true : false));
            _dataFrame.InsertColumn(0, _column);
            _dataFrame.InsertColumn(1, _string);
            _dataFrame.InsertColumn(2, _bool);

            _otherDataFrame = new DataFrame();
            _otherColumn = new PrimitiveColumn<int>("Int0", Enumerable.Range(0, length/2));
            _otherString = new StringColumn("String", Enumerable.Range(0, length/2).Select(x => x.ToString()));
            _otherBool = new PrimitiveColumn<bool>("Bool", Enumerable.Range(0, length/2).Select(x => x % 2 == 0 ? true : false));
            _otherDataFrame.InsertColumn(0, _otherColumn);
            _otherDataFrame.InsertColumn(1, _otherString);
            _otherDataFrame.InsertColumn(2, _otherBool);
            
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
        public BaseColumn ColumnSort()
        {
            return _column.Sort(false);
        }

        [Benchmark]
        public DataFrame Sort()
        {
            return _dataFrame.Sort("Int0", false);
        }

        [Benchmark]
        public DataFrame Clip()
        {
            return _dataFrame.Clip(10000, 40000);
        }

        [Benchmark]
        public GroupBy GroupBy()
        {
            return _dataFrame.GroupBy("Bool");
        }

        [Benchmark]
        public BaseColumn ColumnFillNulls()
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
