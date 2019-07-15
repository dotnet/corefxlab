// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using BenchmarkDotNet.Attributes;
using Xunit;

namespace Microsoft.Collections.Extensions.Benchmarks
{
    [MemoryDiagnoser]
    public class DictionarySlimRemove
    {
        [Params(5_000_000)]
        public int Size { get; set; }

        int[] _removeOrder;
        private DictionarySlim<int, int> _dictSlim;
        private Dictionary<int, int> _dict;

        void Shuffle(int[] array)
        {
            var rng = new Random(1234617);
            int n = array.Length;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                int value = array[k];
                array[k] = array[n];
                array[n] = value;
            }
        }

        [GlobalSetup]
        public void CreateRemoveOrder()
        {
            _removeOrder = new int[Size];
            for (int i = 0; i < Size; i++)
                _removeOrder[i] = i * 7;
            Shuffle(_removeOrder);
        }


        [IterationSetup(Targets = new[] { nameof(RemoveDictionarySlim) })]
        public void LoadDictionarySlim()
        {
            _dictSlim = new DictionarySlim<int, int>();
            for (int i = 0; i < Size; i++)
                _dictSlim.GetOrAddValueRef(i * 7) = -i;
        }

        [IterationSetup(Targets = new[] { nameof(RemoveDictionary) })]
        public void LoadDictionary()
        {
            _dict = new Dictionary<int, int>();
            for (int i = 0; i < Size; i++)
                _dict[i * 7] = -i;
        }

        [Benchmark]
        public void RemoveDictionarySlim()
        {
            for (int i = 0; i < _removeOrder.Length; i++)
                _dictSlim.Remove(_removeOrder[i]);
            Assert.Equal(0, _dictSlim.Count);
        }

        [Benchmark(Baseline = true)]
        public void RemoveDictionary()
        {
            for (int i = 0; i < _removeOrder.Length; i++)
                _dict.Remove(_removeOrder[i]);
            Assert.Equal(0, _dict.Count);
        }
    }
}
