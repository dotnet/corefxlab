// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using BenchmarkDotNet.Attributes;

namespace Microsoft.Experimental.Collections.Benchmarks
{
    [MemoryDiagnoser]
    public class DictionarySlimGeneral
    {
        [Params(10_000_000, 20_000_000, 30_000_000)]
        public int Size { get; set; }
        private const int AggCount = 250;

        private ulong[] _keys;
        private DictionarySlim<ulong, int> _refDict;
        private Dictionary<ulong, int> _dict;

        [GlobalSetup]
        public void CreateValuesList()
        {
            var rand = new Random(11231992);

            _keys = new ulong[Size];

            for (int i = 0; i < _keys.Length; i++)
            {
                _keys[i] = (ulong)rand.Next(Size/AggCount);
            }
        }

        [IterationSetup(Targets = new[] { nameof(LoadDictionarySlim) })]
        public void CreateDictionarySlim()
        {
            _refDict = new DictionarySlim<ulong, int>(Size / AggCount);
        }

        [IterationSetup(Targets = new[] { nameof(LoadDictionary) })]
        public void CreateDictionary()
        {
            _dict = new Dictionary<ulong, int>(Size / AggCount);
        }

        [Benchmark]
        public void LoadDictionarySlim()
        {
            for (int i = 0; i < _keys.Length; i++)
            {
                var k = _keys[i];
                _refDict[k] += (int)k;
            }
        }

        [Benchmark(Baseline = true)]
        public void LoadDictionary()
        {
            for (int i = 0; i < _keys.Length; i++)
            {
                var k = _keys[i];
                if (_dict.TryGetValue(k, out int t))
                    _dict[k] = t + (int)k;
                else
                    _dict[k] = (int)k;
            }
        }
    }
}
