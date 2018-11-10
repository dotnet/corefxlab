// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using Xunit;
using BenchmarkDotNet.Attributes;

namespace Microsoft.Collections.Extensions.Benchmarks
{
    [MemoryDiagnoser]
    public class DictionarySlimRemove
    {
        [Params(5_000_000)]
        public int Size { get; set; }

        private DictionarySlim<int, int> _dictSlim;
        private Dictionary<int, int> _dict;

        [IterationSetup(Targets = new[] { nameof(RemoveDictionarySlim) })]
        public void LoadDictionarySlim()
        {
            _dictSlim = new DictionarySlim<int, int>();
            for (int i = 0; i < Size; i++)
                _dictSlim[i * 7] = -i;
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
            int i = Size;
            while (i-- > 0)
                _dictSlim.Remove(i * 7);
            Assert.Equal(0, _dictSlim.Count);
        }

        [Benchmark(Baseline = true)]
        public void RemoveDictionary()
        {
            int i = Size;
            while (i-- > 0)
                _dict.Remove(i * 7);
            Assert.Equal(0, _dict.Count);
        }
    }
}
