// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace System.Text.CaseFolding.Benchmarks
{
    public class StringComparerBenchmark
    {
        [Benchmark(Baseline = true)]
        [ArgumentsSource(nameof(Data))]
        public void CoreFXCompare(string strA, string strB)
        {
            string.Compare(strA, strB, StringComparison.OrdinalIgnoreCase);
        }

        [Benchmark]
        [ArgumentsSource(nameof(Data))]
        public int SimpleCaseFoldCompare(string strA, string strB)
        {
            var comparer = new SimpleCaseFoldingStringComparer();
            return comparer.Compare(strA, strB);
        }

        public IEnumerable<object[]> Data()
        {
            yield return new object[] { "CaseFolding1", "cASEfOLDING2" };
            yield return new object[] { "ЯяЯяЯяЯяЯяЯ1", "яЯяЯяЯяЯяЯя2" };
        }
    }
}
