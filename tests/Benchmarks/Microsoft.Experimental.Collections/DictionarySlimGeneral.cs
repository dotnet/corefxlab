// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using BenchmarkDotNet.Attributes;

namespace Microsoft.Collections.Extensions.Benchmarks
{
    [MemoryDiagnoser]
    public class DictionarySlimGeneral
    {
        [Params(10_000_000, 20_000_000, 30_000_000)]
        public int Size { get; set; }
        private const int AggCount = 250;

        struct KeyWithHashCode : IEquatable<KeyWithHashCode>
        {
            internal ulong Key;
            internal int HashCode;
            internal KeyWithHashCode(ulong i)
            {
                Key = i;
                HashCode = System.HashCode.Combine(i);
            }
            public override bool Equals(object obj)
            {
                return obj is KeyWithHashCode k && k.Key == Key;
            }

            public bool Equals(KeyWithHashCode other)
            {
                return other.Key == Key;
            }

            public override int GetHashCode()
            {
                return HashCode;
            }
        }


        private KeyWithHashCode[] _keys;
        private DictionarySlim<KeyWithHashCode, int> _refDict;
        private Dictionary<KeyWithHashCode, int> _dict;

        [GlobalSetup]
        public void CreateValuesList()
        {
            var rand = new Random(11231992);

            _keys = new KeyWithHashCode[Size];

            for (int i = 0; i < _keys.Length; i++)
            {
                _keys[i] = new KeyWithHashCode((ulong)rand.Next(Size/AggCount));
            }
        }

        [IterationSetup(Targets = new[] { nameof(LoadDictionarySlim) })]
        public void CreateDictionarySlim()
        {
            _refDict = new DictionarySlim<KeyWithHashCode, int>(Size / AggCount);
        }

        [IterationSetup(Targets = new[] { nameof(LoadDictionary) })]
        public void CreateDictionary()
        {
            _dict = new Dictionary<KeyWithHashCode, int>(Size / AggCount);
        }

        [Benchmark]
        public void LoadDictionarySlim()
        {
            for (int i = 0; i < _keys.Length; i++)
            {
                var k = _keys[i];
                _refDict.GetOrAddValueRef(k) += k.HashCode;
            }
        }

        [Benchmark(Baseline = true)]
        public void LoadDictionary()
        {
            for (int i = 0; i < _keys.Length; i++)
            {
                var k = _keys[i];
                if (_dict.TryGetValue(k, out int t))
                    _dict[k] = t + k.HashCode;
                else
                    _dict[k] = k.HashCode;
            }
        }
    }
}
