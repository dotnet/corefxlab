// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Linq;
using BenchmarkDotNet.Attributes;
using Microsoft.Data.Analysis;

namespace Benchmarks.Microsoft.Data.Analysis
{
    public class DataFrameBinaryOps
    {
        private DataFrame _dataFrame;
        private DataFrameColumn _column;
        private DataFrameColumn _other;

        [GlobalSetup]
        public void Setup()
        {
            _dataFrame = new DataFrame();
            _column = new PrimitiveDataFrameColumn<int>("Int0", Enumerable.Range(0, 50000));
            _other = _column.Clone();
            _dataFrame.Columns.Insert(0, _column);
        }

        [Benchmark]
        public DataFrameColumn Add()
        {
            return _column.Add(_other);
        }

        [Benchmark]
        public DataFrameColumn AddValue()
        {
            return _column.Add(10);
        }

        // The other APIs follow the exact same code as Add, except with the appropriate op. Doesn't make sense to benchmark them too
    }
}
