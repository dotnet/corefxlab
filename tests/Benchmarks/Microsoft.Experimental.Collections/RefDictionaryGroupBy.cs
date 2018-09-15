// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;

namespace Microsoft.Experimental.Collections.Benchmarks
{
    public class RefDictionaryGroupBy
    {
        [Params(250_000, 2_500_000, 25_000_000)]
        public int Size { get; set; }
        private const int AggCount = 250;
        private ulong[] _keys;
        
        [GlobalSetup]
        public void CreateValuesList()
        {
            var rand = new Random(11231992);

            _keys = new ulong[Size];

            for (int i = 0; i < _keys.Length; i++)
            {
                _keys[i] = (ulong)rand.Next(Size / AggCount);
            }
        }

        [Benchmark(Baseline = true)]
        public void GroupBy()
        {
            var x = _keys.GroupBy(i => i / 10).Select(g => g.Max()).Min();
        }

        [Benchmark]
        public void RefDictionary()
        {
            var x = _keys.GroupByRef(i => i / 10).Select(g => g.Max()).Min();
        }
    }

    public static class Enumerable2
    {
        public class Grouping<TKey, TElement> : List<TElement>, IGrouping<TKey, TElement>
        {
            public Grouping(TKey k)
            {
                Key = k;
            }
            public TKey Key { get; }
        }

        public static IEnumerable<IGrouping<TKey, TSource>> GroupByRef<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector) where TKey : IEquatable<TKey>
        {
            var dict = new RefDictionary<TKey, Grouping<TKey, TSource>>();
            foreach (var t in source)
            {
                var k = keySelector(t);
                ref var g = ref dict[k];
                if (g == null) g = new Grouping<TKey, TSource>(k);
                g.Add(t);
            }
            return dict.Values;
        }
    }
}
