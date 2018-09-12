// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Microsoft.Experimental.Collections.Tests
{
    public class RefDictionaryTests
    {
        [Fact]
        public void SingleEntry()
        {
            var d = new RefDictionary<ulong, int>();
            d[7]++;
            d[7] += 3;
            Assert.Equal(4, d[7]);
        }

        [Fact]
        public void RefDictionaryVersusDictionary()
        {
            var rand = new Random(1123);
            var rd = new RefDictionary<ulong, int>();
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
