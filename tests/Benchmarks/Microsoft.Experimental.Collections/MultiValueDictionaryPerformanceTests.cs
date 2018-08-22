﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

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

        private void SetValue()
        {
            if (Size >= 1024)
                _value = 1024;
            if (Size >= 4096)
                _value = 4096;
            if (Size >= 16384)
                _value = 16384;
        }

        [IterationSetup(Targets = new[] { nameof(Add), nameof(ContainsValue) })]
        public void AddContainsSetup()
        {
            SetValue();
            CreateMultiValueDictionary_SingleValues();
        }

        [Benchmark]
        public void Add()
        {
            _dict.Add(_value, 0);
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

        [Benchmark]
        public void AddRange()
        {
            _dict.AddRange(_value, _values);
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

        [Benchmark]
        public void Remove()
        {
            _dict.Remove(_value);
        }

        [Benchmark]
        public void Clear()
        {
            _dict.Clear();
        }

        [Benchmark]
        public void Ctor()
        {
            var _ = new MultiValueDictionary<int, string>();
        }

        [Benchmark]
        public void Ctor_Size()
        {
            var _ = new MultiValueDictionary<int, string>(Size);
        }

        [Benchmark]
        public void GetItem()
        {
            IReadOnlyCollection<int> retrieved = _dict[_randomKey];
        }

        [Benchmark]
        public void GetKeys()
        {
            IEnumerable<int> result;
            result = _dict.Keys;
        }

        [Benchmark]
        public void TryGetValue()
        {
            _dict.TryGetValue(_randomKey, out var _);
        }

        [Benchmark]
        public void ContainsKey()
        {
            _dict.ContainsKey(_randomKey);
        }

        [Benchmark]
        public void ContainsValue()
        {
            _dict.ContainsValue(_value);
        }
    }
}
