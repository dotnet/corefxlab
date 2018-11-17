// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using BenchmarkDotNet.Attributes;

namespace Microsoft.Collections.Extensions.Benchmarks
{
    [MemoryDiagnoser]
    public class DictionarySlimString
    {
        [Params(1_000_000, 5_000_000, 10_000_000)]
        public int Size { get; set; }
        private const int AggCount = 250;

        private string[] _keys;
        private DictionarySlim<string, int> _refDict;
        private Dictionary<string, int> _dict;

        [GlobalSetup]
        public void CreateValuesList()
        {
            var rand = new Random(11231992);

            _keys = new string[Size];

            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

            for (int i = 0; i < _keys.Length; i++)
            {
                var stringChars = new char[rand.Next(9)+1];
                for (int j = 0; j < stringChars.Length; j++)
                    stringChars[j] = chars[rand.Next(chars.Length)];
                _keys[i] = new string(stringChars);
            }
        }

        [IterationSetup(Targets = new[] { nameof(LoadDictionarySlim) })]
        public void CreateDictionarySlim()
        {
            _refDict = new DictionarySlim<string, int>(Size / AggCount);
        }

        [IterationSetup(Targets = new[] { nameof(LoadDictionary) })]
        public void CreateDictionary()
        {
            _dict = new Dictionary<string, int>(Size / AggCount);
        }

        [Benchmark]
        public void LoadDictionarySlim()
        {
            for (int i = 0; i < _keys.Length; i++)
            {
                var k = _keys[i];
                _refDict.GetOrAddValueRef(k) += i;
            }
        }

        [Benchmark(Baseline = true)]
        public void LoadDictionary()
        {
            for (int i = 0; i < _keys.Length; i++)
            {
                var k = _keys[i];
                if (_dict.TryGetValue(k, out int t))
                    _dict[k] = t + i;
                else
                    _dict[k] = i;
            }
        }
    }
}
