// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using BenchmarkDotNet.Attributes;

namespace Microsoft.Collections.Extensions.Tests
{
    [MemoryDiagnoser]
    public class MultiValueDictionaryPerformanceTests
    {
        [Params(1000, 10_000, 100_000)]
        public int Size { get; set; }

        private int _randomKey;
        private List<int> _values;
        private MultiValueDictionary<int, int> _dict;
        private int _value;

        [GlobalSetup(Targets = new[] { nameof(Clear), nameof(GetKeys) })]
        public void CreateMultiValueDictionary_SingleValues()
        {
            _dict = new MultiValueDictionary<int, int>();
            Random rand = new Random(11231992);

            while (_dict.Count < Size)
                _dict.Add(rand.Next(), rand.Next());
        }

        [GlobalSetup(Targets = new[] { nameof(TryGetValue), nameof(ContainsKey), nameof(GetItem) })]
        public void CreateMultiValueDictionary_AddRandomKey()
        {
            CreateMultiValueDictionary_SingleValues();
            _randomKey = new Random(837322).Next(0, 400000);
            _dict.Add(_randomKey, 12);
        }

        [GlobalSetup(Target = nameof(Remove))]
        public void Remove_CreateDictionaryMultipleRepeatedValues()
        {
            SetValue();
            _dict = new MultiValueDictionary<int, int>();
            for (int i = 0; i < Size; i++)
                for (int j = 0; j < 100; j++)
                    _dict.Add(i, j);
        }

        [IterationSetup(Targets = new[] { nameof(Add), nameof(ContainsValue) })]
        public void AddContainsSetup()
        {
            SetValue();
            CreateMultiValueDictionary_SingleValues();
        }

        [IterationSetup(Target = nameof(AddRange))]
        public void AddRange_CreateListWithValues()
        {
            SetValue();
            _values = new List<int>();
            for (int i = 0; i < Size; i++)
                _values.Add(i);
            _dict = new MultiValueDictionary<int, int>();
        }

        private void SetValue()
        {
            if (Size >= 1024)
                _value = 1024;
            if (Size >= 4096)
                _value = 4096;
            if (Size >= 16384)
                _value = 16384;
        }

        [Benchmark]
        public void Add() => _dict.Add(_value, 0);

        [Benchmark]
        public void AddRange() => _dict.AddRange(_value, _values);

        [Benchmark]
        public bool Remove() => _dict.Remove(_value);

        [Benchmark]
        public void Clear() => _dict.Clear();

        [Benchmark]
        public MultiValueDictionary<int, string> Ctor() => new MultiValueDictionary<int, string>();

        [Benchmark]
        public MultiValueDictionary<int, string> Ctor_Size() => new MultiValueDictionary<int, string>(Size);

        [Benchmark]
        public IReadOnlyCollection<int> GetItem() => _dict[_randomKey];

        [Benchmark]
        public IEnumerable<int> GetKeys() => _dict.Keys;

        [Benchmark]
        public bool TryGetValue() => _dict.TryGetValue(_randomKey, out var _);

        [Benchmark]
        public bool ContainsKey() =>  _dict.ContainsKey(_randomKey);

        [Benchmark]
        public bool ContainsValue() => _dict.ContainsValue(_value);
    }
}
