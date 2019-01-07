// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;

namespace Microsoft.Collections.Extensions.Benchmarks
{
    public class DictionarySlimGroupBy
    {
        [Params(2_500, 25_000, 250_000)]
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

        [GlobalSetup]
        public void CreateValuesList()
        {
            var rand = new Random(5737262);

            _keys = new KeyWithHashCode[Size];

            for (int i = 0; i < _keys.Length; i++)
            {
                _keys[i] = new KeyWithHashCode((ulong)rand.Next(Size / AggCount));
            }
        }

        [Benchmark(Baseline = true)]
        public void GroupBy()
        {
            var x = _keys.GroupBy(i => i.Key / 10).Select(g => g.Max(i => i.HashCode)).Min();
        }

        [Benchmark]
        public void DictionarySlim()
        {
            var x = _keys.GroupByRef(i => i.Key / 10).Select(g => g.Max(i => i.HashCode)).Min();
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
            var dict = new DictionarySlim<TKey, Grouping<TKey, TSource>>();
            foreach (var t in source)
            {
                var k = keySelector(t);
                ref var g = ref dict.GetOrAddValueRef(k);
                if (g == null) g = new Grouping<TKey, TSource>(k);
                g.Add(t);
            }
            return dict.Select(x => x.Value);
        }
    }
}
