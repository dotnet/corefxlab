// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Linq;
using BenchmarkDotNet.Attributes;
using Microsoft.Data;

namespace Benchmarks.Microsoft.Data
{
    public class DataFrameComputations
    {
        private DataFrame _dataFrame;
        private DataFrameColumn _column0;

        [GlobalSetup]
        public void Setup()
        {
            _dataFrame = new DataFrame();
            _column0 = new PrimitiveDataFrameColumn<int>("Int0", Enumerable.Range(0, 50000));
            _dataFrame.Columns.Insert(0, _column0);
        }

        [Benchmark]
        public object Sum()
        {
            return _column0.Sum();
        }

        [Benchmark]
        public object Max()
        {
            return _column0.Max();
        }

        [Benchmark]
        public void Abs()
        {
            _column0.Abs();
        }

        [Benchmark]
        public void CumulativeMax()
        {
            _column0.CumulativeMax();
        }

        [Benchmark]
        public void CumulativeSum()
        {
            _column0.CumulativeSum();
        }
    }
}
