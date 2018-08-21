// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using Xunit;
using BenchmarkDotNet.Attributes;

namespace Microsoft.Collections.Extensions.Tests
{
    public class MultiValueDictionaryPerformanceTests
    {
        [Params(1000, 10000, 10000)]
        public int Size { get; set; }

        public int RandomKey { get; set; }

        public MultiValueDictionary<int, int> dict;
        
        [GlobalSetup(Targets = new[] { nameof(Add), nameof(Clear), nameof(GetItem), nameof(GetKeys), nameof(ContainsValue) })]
        public void CreateMultiValueDictionary_SingleValues()
        {
            dict = new MultiValueDictionary<int, int>();
            Random rand = new Random(11231992);

            while (dict.Count < Size)
                dict.Add(rand.Next(), rand.Next());
        }

        [GlobalSetup(Targets = new[] { nameof(TryGetValue), nameof(ContainsKey) })]
        public void CreateMultiValueDictionary_AddRandomKey()
        {
            CreateMultiValueDictionary_SingleValues();
            RandomKey = new Random(837322).Next(0, 400000);
            dict.Add(RandomKey, 12);
        }

        [Benchmark]
        public void Add()
        {
            for (int i = 0; i <= 20000; i++)
            {
                dict.Add(i * 10 + 1, 0); dict.Add(i * 10 + 2, 0); dict.Add(i * 10 + 3, 0);
                dict.Add(i * 10 + 4, 0); dict.Add(i * 10 + 5, 0); dict.Add(i * 10 + 6, 0);
                dict.Add(i * 10 + 7, 0); dict.Add(i * 10 + 8, 0); dict.Add(i * 10 + 9, 0);
            }
        }

        public List<int> values;

        [GlobalSetup(Target = nameof(AddRange))]
        public void AddRange_CreateListWithValues()
        {
            values = new List<int>();
            for (int i = 0; i < Size; i++)
                values.Add(i);
            dict = new MultiValueDictionary<int, int>();
        }

        [Benchmark]
        public void AddRange()
        {
            for (int i = 0; i <= 20000; i++)
                dict.AddRange(i, values);
        }

        [GlobalSetup(Target = nameof(Remove))]
        public void Remove_CreateDictionaryMultipleRepeatedValues()
        {
            dict = new MultiValueDictionary<int, int>();
            for (int i = 0; i < Size; i++)
                for (int j = 0; j < 100; j++)
                    dict.Add(i, j);
        }

        [Benchmark]
        public void Remove()
        {
            for (int i = 0; i <= Size; i++)
            {
                dict.Remove(i);
            }
        }

        [Benchmark]
        public void Clear()
        {
            dict.Clear();
        }

        [Benchmark]
        public void Ctor()
        {
            for (int i = 0; i <= 20000; i++)
            {
                new MultiValueDictionary<int, string>(); new MultiValueDictionary<int, string>(); new MultiValueDictionary<int, string>();
                new MultiValueDictionary<int, string>(); new MultiValueDictionary<int, string>(); new MultiValueDictionary<int, string>();
                new MultiValueDictionary<int, string>(); new MultiValueDictionary<int, string>(); new MultiValueDictionary<int, string>();
            }
        }

        [Benchmark]
        [Arguments(0)]
        [Arguments(1024)]
        [Arguments(4096)]
        [Arguments(16384)]
        public void Ctor_Size(int size)
        {
            for (int i = 0; i <= 500; i++)
            {
                new MultiValueDictionary<int, string>(size); new MultiValueDictionary<int, string>(size); new MultiValueDictionary<int, string>(size);
                new MultiValueDictionary<int, string>(size); new MultiValueDictionary<int, string>(size); new MultiValueDictionary<int, string>(size);
                new MultiValueDictionary<int, string>(size); new MultiValueDictionary<int, string>(size); new MultiValueDictionary<int, string>(size);
            }
        }

        [Benchmark]
        public void GetItem()
        {
            IReadOnlyCollection<int> retrieved;
            for (int i = 0; i <= 10000; i++)
            {
                retrieved = dict[1]; retrieved = dict[2]; retrieved = dict[3];
                retrieved = dict[4]; retrieved = dict[5]; retrieved = dict[6];
                retrieved = dict[7]; retrieved = dict[8]; retrieved = dict[9];
            }
        }

        [Benchmark]
        public void GetKeys()
        {
            IEnumerable<int> result;
            for (int i = 0; i <= 20000; i++)
            {
                result = dict.Keys; result = dict.Keys; result = dict.Keys;
                result = dict.Keys; result = dict.Keys; result = dict.Keys;
                result = dict.Keys; result = dict.Keys; result = dict.Keys;
            }
        }

        [Benchmark]
        public void TryGetValue()
        {
            for (int i = 0; i <= 1000; i++)
            {
                dict.TryGetValue(RandomKey, out var _); dict.TryGetValue(RandomKey, out var _);
                dict.TryGetValue(RandomKey, out var _); dict.TryGetValue(RandomKey, out var _);
                dict.TryGetValue(RandomKey, out var _); dict.TryGetValue(RandomKey, out var _);
                dict.TryGetValue(RandomKey, out var _); dict.TryGetValue(RandomKey, out var _);
            }
        }

        [Benchmark]
        public void ContainsKey()
        {
            for (int i = 0; i <= 10000; i++)
            {
                dict.ContainsKey(RandomKey); dict.ContainsKey(RandomKey); dict.ContainsKey(RandomKey);
                dict.ContainsKey(RandomKey); dict.ContainsKey(RandomKey); dict.ContainsKey(RandomKey);
                dict.ContainsKey(RandomKey); dict.ContainsKey(RandomKey); dict.ContainsKey(RandomKey);
                dict.ContainsKey(RandomKey); dict.ContainsKey(RandomKey); dict.ContainsKey(RandomKey);
            }
        }

        [Benchmark]
        public void ContainsValue()
        {
            for (int i = 0; i <= 20000; i++)
            {
                dict.ContainsValue(i); dict.ContainsValue(i); dict.ContainsValue(i);
                dict.ContainsValue(i); dict.ContainsValue(i); dict.ContainsValue(i);
                dict.ContainsValue(i); dict.ContainsValue(i); dict.ContainsValue(i);
            }
        }
    }
}
