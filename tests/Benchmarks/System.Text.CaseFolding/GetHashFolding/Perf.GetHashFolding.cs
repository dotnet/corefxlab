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

namespace System.Text.CaseFolding
{
    public class GetHashBenchmark
    {
        [Benchmark(Baseline = true)]
        [ArgumentsSource(nameof(Data))]
        public int CoreFXMarvinOrdinalIgnoreCase(string StrA)
        {
            return Marvin.ComputeHash32OrdinalIgnoreCase(StrA, Marvin.DefaultSeed);
        }

        [Benchmark]
        [ArgumentsSource(nameof(Data))]
        public int SCFMarvinOrdinalIgnoreCase(string StrA)
        {
            return SCFMarvin.ComputeHash32OrdinalIgnoreCase(StrA, SCFMarvin.DefaultSeed);
        }

        public IEnumerable<object> Data()
        {
            yield return "CaseFolding1";
            yield return "ЯяЯяЯяЯяЯяЯ1";
        }
    }
}
