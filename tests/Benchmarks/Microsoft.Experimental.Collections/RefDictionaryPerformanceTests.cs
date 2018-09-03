// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using BenchmarkDotNet.Attributes;

namespace Microsoft.Collections.Extensions.Tests
{
    [MemoryDiagnoser]
    public class RefDictionaryPerformanceTests
    {
        [Params(100000)]
        public int Size { get; set; }

        ulong[] _keys;
        int[] _values;

        RefDictionary<ulong, int> _refDict;
        Dictionary<ulong, int> _dict;

        [GlobalSetup(Targets = new[] { nameof(LoadRefDictionary), nameof(LoadDictionary) })]
        public void CreateValuesList()
        {
            var rand = new Random(11231992);

            _keys = new ulong[Size];
            _values = new int[Size];

            for (int i = 0; i < Size; i++)
            {
                _keys[i] = (ulong)rand.Next(Size/100);
                _values[i] = rand.Next();
            }
        }

        [IterationSetup(Targets = new[] { nameof(LoadRefDictionary) })]
        public void CreateRefDictionary()
        {
            _refDict = new RefDictionary<ulong, int>(1024);
        }

        [IterationSetup(Targets = new[] { nameof(LoadDictionary) })]
        public void CreateDictionary()
        {
            _dict = new Dictionary<ulong, int>(1024);
        }

        [Benchmark]
        public void LoadRefDictionary()
        {
            for (int i = 0; i < Size; i++)
            {
                _refDict[_keys[i]] += _values[i];
            }
        }

        [Benchmark]
        public void LoadDictionary()
        {
            for (int i = 0; i < Size; i++)
            {
                var k = _keys[i];
                if (_dict.TryGetValue(k, out int t))
                    _dict[k] = t + _values[i];
                else
                    _dict[k] = _values[i];
            }
        }
    }
}
