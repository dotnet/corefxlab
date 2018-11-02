// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.Experimental.Collections.Tests
{
    public class DictionarySlimTests
    {
        ITestOutputHelper _output;
        public DictionarySlimTests(ITestOutputHelper output)
        {
            _output = output;
        }
        [Fact]
        public void SingleEntry()
        {
            var d = new DictionarySlim<ulong, int>();
            d[7]++;
            d[7] += 3;
            Assert.Equal(4, d[7]);
        }

        [Fact]
        public void ContainKey()
        {
            var d = new DictionarySlim<ulong, int>();
            d[7] = 9;
            d[10] = 10;
            Assert.True(d.ContainsKey(7));
            Assert.True(d.ContainsKey(10));
            Assert.False(d.ContainsKey(1));
        }

        [Fact]
        public void GetValueOrDefault()
        {
            var d = new DictionarySlim<ulong, int>();
            d[7] = 9;
            d[10] = 11;
            Assert.Equal(9, d.GetValueOrDefault(7));
            Assert.Equal(11, d.GetValueOrDefault(10));
        }

        [Fact]
        public void Keys()
        {
            var d = new DictionarySlim<int, int>();
            d[7] = 9;
            d[10] = 10;
            Assert.True(d.Keys.OrderBy(i => i).SequenceEqual(new[] { 7, 10 }));
        }

        [Fact]
        public void Values()
        {
            var d = new DictionarySlim<int, int>();
            d[7] = 9;
            d[10] = 10;
            Assert.True(d.Values.OrderBy(i => i).SequenceEqual(new[] { 9, 10 }));
        }

        [Fact]
        public void DictionarySlimVersusDictionary()
        {
            var rand = new Random(1123);
            var rd = new DictionarySlim<ulong, int>();
            var d = new Dictionary<ulong, int>();
            var size = 1000;

            for (int i = 0; i < size; i++)
            {
                var k = (ulong)rand.Next(100) + 23;
                var v = rand.Next();

                rd[k] += v;

                if (d.TryGetValue(k, out int t))
                    d[k] = t + v;
                else
                    d[k] = v;
            }

            Assert.True(d.OrderBy(i => i.Key).SequenceEqual(rd.OrderBy(i => i.Key)));
        }
    }
}
