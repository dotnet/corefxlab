// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Linq;
using BenchmarkDotNet.Attributes;
using Microsoft.Data;

namespace Benchmarks.Microsoft.Data
{
    public class DataFrameBinaryOps
    {
        private DataFrame _dataFrame;
        private BaseColumn _column;
        private BaseColumn _other;

        [GlobalSetup]
        public void Setup()
        {
            _dataFrame = new DataFrame();
            _column = new PrimitiveColumn<int>("Int0", Enumerable.Range(0, 50000));
            _other = _column.Clone();
            _dataFrame.InsertColumn(0, _column);
        }

        [Benchmark]
        public BaseColumn Add()
        {
            return _column.Add(_other);
        }

        [Benchmark]
        public BaseColumn AddValue()
        {
            return _column.Add(10);
        }

        // The other APIs follow the exact same code as Add, except with the appropriate op. Doesn't make sense to benchmark them too
    }
}
