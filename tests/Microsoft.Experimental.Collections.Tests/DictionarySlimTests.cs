// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            var d = new DictionarySlim<char, int>();
            d['a'] = 9;
            d['b'] = 10;
            Assert.Equal(d.Keys, new[] { 'a', 'b' });
        }

        [Fact]
        public void Values()
        {
            var d = new DictionarySlim<int, int>();
            d['a'] = 9;
            d['b'] = 10;
            Assert.Equal(d.Values, new[] { 9, 10 });
        }

        [Fact]
        public void RemoveNonExistent()
        {
            var d = new DictionarySlim<int, int>();
            Assert.False(d.Remove(0));
        }        

        [Fact]
        public void RemoveSimple()
        {
            var d = new DictionarySlim<int, int>();
            d[0] = 0;
            Assert.True(d.Remove(0));
            Assert.Equal(0, d.Count);
        }

        [Fact]
        public void RemoveOneOfTwo()
        {
            var d = new DictionarySlim<char, int>();
            d['a'] = 0;
            d['b'] = 1;
            Assert.True(d.Remove('a'));
            Assert.Equal(d['b'], 1);
            Assert.Equal(1, d.Count);
        }

        [Fact]
        public void RemoveSlotReused()
        {
            var d = new DictionarySlim<Collider, int>();
            d[C(0)] = 0;
            d[C(1)] = 1;
            d[C(2)] = 2;
            Assert.True(d.Remove(C(0)));
            _output.WriteLine("{0} {1}", d.Capacity, d.Count);
            var capacity = d.Capacity;

            d[C(0)] = 3;
            _output.WriteLine("{0} {1}", d.Capacity, d.Count);
            Assert.Equal(d[C(0)], 3);
            Assert.Equal(3, d.Count);
            Assert.Equal(capacity, d.Capacity);

        }

        [Fact]
        public void RemoveEnumerate()
        {
            var d = new DictionarySlim<int, int>();
            d[0] = 0;
            Assert.True(d.Remove(0));
            Assert.Empty(d);
            Assert.Empty(d.Keys);
            Assert.Empty(d.Values);
        }

        [Fact]
        public void RemoveOneOfTwoEnumerate()
        {
            var d = new DictionarySlim<char, int>();
            d['a'] = 0;
            d['b'] = 1;
            Assert.True(d.Remove('a'));
            Assert.Equal(KeyValuePair.Create('b', 1), d.Single());
            Assert.Equal('b', d.Keys.Single());
            Assert.Equal(1, d.Values.Single());
        }

        [Fact]
        public void CopyTo()
        {
            var d = new DictionarySlim<char, int>();
            d['a'] = 0;
            d['b'] = 1;
            var a = new KeyValuePair<char, int>[3];
            d.CopyTo(a, 1);
            Assert.Equal(KeyValuePair.Create('a', 0), a[1]);
            Assert.Equal(KeyValuePair.Create('b', 1), a[2]);
        }

        [Fact]
        public void KeysCopyTo()
        {
            var d = new DictionarySlim<char, int>();
            d['a'] = 0;
            d['b'] = 1;
            var a = new char[3];
            d.Keys.CopyTo(a, 1);
            Assert.Equal('a', a[1]);
            Assert.Equal('b', a[2]);
        }

        [Fact]
        public void ValuesCopyTo()
        {
            var d = new DictionarySlim<char, int>();
            d['a'] = 0;
            d['b'] = 1;
            var a = new int[3];
            d.Values.CopyTo(a, 1);
            Assert.Equal(0, a[1]);
            Assert.Equal(1, a[2]);
        }

        [Fact]
        public void KeysThreeRemoveOneCopyTo()
        {
            var d = new DictionarySlim<char, int>();
            d['a'] = 0;
            d['b'] = 1;
            d['c'] = 2;
            d.Remove('b');
            var a = new char[3];
            d.Keys.CopyTo(a, 1);
            Assert.Equal('a', a[1]);
            Assert.Equal('c', a[2]);
        }

        [Fact]
        public void ValuessThreeRemoveOneCopyTo()
        {
            var d = new DictionarySlim<char, int>();
            d['a'] = 0;
            d['b'] = 1;
            d['c'] = 2;
            d.Remove('b');
            var a = new int[3];
            d.Values.CopyTo(a, 1);
            Assert.Equal(0, a[1]);
            Assert.Equal(2, a[2]);
        }

        [Fact]
        public void CopyToThreeRemoveOne()
        {
            var d = new DictionarySlim<char, int>();
            d['a'] = 0;
            d['b'] = 1;
            d['c'] = 2;
            Assert.True(d.Remove('b'));
            var a = new KeyValuePair<char, int>[3];
            d.CopyTo(a, 1);
            Assert.Equal(KeyValuePair.Create('a', 0), a[1]);
            Assert.Equal(KeyValuePair.Create('c', 2), a[2]);
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

            Assert.Equal(d.Count, rd.Count);
            Assert.Equal(d.OrderBy(i => i.Key), (rd.OrderBy(i => i.Key)));
            Assert.Equal(d.OrderBy(i => i.Value), (rd.OrderBy(i => i.Value)));
        }

        [Fact]
        public void DictionarySlimVersusDictionary_AllCollisions()
        {
            var rand = new Random(333);
            var rd = new DictionarySlim<Collider, int>();
            var d = new Dictionary<Collider, int>();
            var size = rand.Next(1234);

            for (int i = 0; i < size; i++)
            {
                if (rand.Next(5) != 0)
                {
                    var k = C(rand.Next(100) + 23);
                    var v = rand.Next();

                    rd[k] += v;

                    if (d.TryGetValue(k, out int t))
                        d[k] = t + v;
                    else
                        d[k] = v;
                }

                if (rand.Next(3) == 0 && d.Count > 0)
                {
                    var el = GetRandomElement(d);
                    Assert.True(rd.Remove(el));
                    Assert.True(d.Remove(el));
                }
            }

            Assert.Equal(d.Count, rd.Count);
            Assert.Equal(d.OrderBy(i => i.Key), (rd.OrderBy(i => i.Key)));
            Assert.Equal(d.OrderBy(i => i.Value), (rd.OrderBy(i => i.Value)));
        }  

        private TKey GetRandomElement<TKey, TValue>(IDictionary<TKey, TValue> d)
        {
            int index = 0;
            var rand = new Random(42);
            foreach(var entry in d)
            {
                if (rand.Next(d.Count) == 0 || index == d.Count - 1)
                {
                    return entry.Key;
                }

                index++;
            }

            throw new InvalidOperationException(); 
        }             

        [DebuggerStepThrough]
        internal Collider C(int val) => new Collider(val);

        [Fact]
        public void Collision()
        {
            var d = new DictionarySlim<Collider, int>();
            d[C(5)] = 3;
            d[C(7)] = 9;
            d[C(10)] = 11;
            Assert.Equal(3, d.GetValueOrDefault(C(5)));
            Assert.Equal(9, d.GetValueOrDefault(C(7)));
            Assert.Equal(11, d.GetValueOrDefault(C(10)));
            d[C(23)]++;
            d[C(23)] += 3;
            Assert.Equal(4, d[C(23)]);
        }

        internal class UsedKey : IEquatable<UsedKey>
        {
            public int Value { get; }
            public int EqualsCount { get; private set; }
            public int GetHashCodeCount { get; private set; }

            public UsedKey(int v)
            {
                Value = v;
            }

            public bool Equals(UsedKey o)
            {
                EqualsCount++;
                return Value == o.Value;
            }

            public override bool Equals(object o)
            {
                return o is UsedKey ck && Value == ck.Value;
            }

            public override int GetHashCode()
            {
                GetHashCodeCount++;
                return Value;
            }
        }

        [Fact]
        public void UsedIEquatable()
        {
            var d = new DictionarySlim<UsedKey, int>();
            var key = new UsedKey(5);
            d[key]++;
            Assert.Equal(1, key.GetHashCodeCount);
            Assert.Equal(0, key.EqualsCount);
        }

        [Fact]
        public void UsedIEquatable2()
        {
            var d = new DictionarySlim<UsedKey, int>();
            var key = new UsedKey(5);
            d[key]++;
            d[key]++;
            Assert.Equal(2, key.GetHashCodeCount);
            Assert.Equal(1, key.EqualsCount);
        }
    }
}

[DebuggerDisplay("{key}")]
internal struct Collider : IEquatable<Collider>, IComparable<Collider>
{
    int key;

    [DebuggerStepThrough]
    internal Collider(int key)
    {
        this.key = key;
    }
    
    internal int Key => key;

    [DebuggerStepThrough]
    public override int GetHashCode() => 42;

    public override bool Equals(object obj) => obj.GetType() == typeof(Collider) && Equals((Collider)obj);

    public bool Equals(Collider that) => that.Key == Key;

    public int CompareTo(Collider that) => key.CompareTo(that.key);
}
