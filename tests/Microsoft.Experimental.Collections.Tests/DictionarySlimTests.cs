// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.Collections.Extensions.Tests
{
    public class DictionarySlimTests
    {
        ITestOutputHelper _output;
        public DictionarySlimTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void ConstructCapacityNegative()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new DictionarySlim<ulong, int>(-1));
        }

        [Fact]
        public void ConstructCapacity0()
        {
            var d = new DictionarySlim<ulong, int>(0);
            Assert.Equal(0, d.Count);
            Assert.Equal(2, d.GetCapacity());
        }

        [Fact]
        public void ConstructCapacity1()
        {
            var d = new DictionarySlim<ulong, int>(1);
            Assert.Equal(0, d.Count);
            Assert.Equal(2, d.GetCapacity());
        }

        [Fact]
        public void ConstructCapacity2()
        {
            var d = new DictionarySlim<ulong, int>(2);
            Assert.Equal(0, d.Count);
            Assert.Equal(2, d.GetCapacity());
        }

        [Fact]
        public void ConstructCapacity3()
        {
            var d = new DictionarySlim<ulong, int>(3);
            Assert.Equal(0, d.Count);
            Assert.Equal(4, d.GetCapacity());
        }

        [Fact]
        public void ConstructCapacity11()
        {
            var d = new DictionarySlim<ulong, int>(11);
            Assert.Equal(0, d.Count);
            Assert.Equal(16, d.GetCapacity());
        }

        // [Fact] // Test too slow
        public void ResizeToCapacity()
        {
            var d = new DictionarySlim<uint, byte>();
            Assert.Throws<InvalidOperationException>(() =>
            {
                for (uint i = 0; i < uint.MaxValue; i++)
                    d.GetOrAddValueRef(i) = (byte)1;
            });
        }

        [Fact]
        public void SingleEntry()
        {
            var d = new DictionarySlim<ulong, int>();
            d.GetOrAddValueRef(7)++;
            d.GetOrAddValueRef(7) += 3;
            Assert.Equal(4, d.GetOrAddValueRef(7));
        }

        [Fact]
        public void ContainKey()
        {
            var d = new DictionarySlim<ulong, int>();
            d.GetOrAddValueRef(7) = 9;
            d.GetOrAddValueRef(10) = 10;
            Assert.True(d.ContainsKey(7));
            Assert.True(d.ContainsKey(10));
            Assert.False(d.ContainsKey(1));
        }

        [Fact]
        public void TryGetValue_Present()
        {
            var d = new DictionarySlim<char, int>();
            d.GetOrAddValueRef('a') = 9;
            d.GetOrAddValueRef('b') = 11;
            Assert.Equal(true, d.TryGetValue('a', out int value));
            Assert.Equal(9, value);
            Assert.Equal(true, d.TryGetValue('b', out value));
            Assert.Equal(11, value);
        }

        [Fact]
        public void TryGetValue_Missing()
        {
            var d = new DictionarySlim<char, int>();
            d.GetOrAddValueRef('a') = 9;
            d.GetOrAddValueRef('b') = 11;
            d.Remove('b');
            Assert.Equal(false, d.TryGetValue('z', out int value));
            Assert.Equal(default, value);
            Assert.Equal(false, d.TryGetValue('b', out value));
            Assert.Equal(default, value);
        }

        [Fact]
        public void TryGetValue_RefTypeValue()
        {
            var d = new DictionarySlim<int, string>();
            d.GetOrAddValueRef(1) = "a";
            d.GetOrAddValueRef(2) = "b";
            Assert.Equal(true, d.TryGetValue(1, out string value));
            Assert.Equal("a", value);
            Assert.Equal(false, d.TryGetValue(99, out value));
            Assert.Equal(null, value);
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
            d.GetOrAddValueRef(0) = 0;
            Assert.True(d.Remove(0));
            Assert.Equal(0, d.Count);
        }

        [Fact]
        public void RemoveOneOfTwo()
        {
            var d = new DictionarySlim<char, int>();
            d.GetOrAddValueRef('a') = 0;
            d.GetOrAddValueRef('b') = 1;
            Assert.True(d.Remove('a'));
            Assert.Equal(1, d.GetOrAddValueRef('b'));
            Assert.Equal(1, d.Count);
        }

        [Fact]
        public void RemoveThenAdd()
        {
            var d = new DictionarySlim<char, int>();
            d.GetOrAddValueRef('a') = 0;
            d.GetOrAddValueRef('b') = 1;
            d.GetOrAddValueRef('c') = 2;
            Assert.True(d.Remove('b'));
            d.GetOrAddValueRef('d') = 3;
            Assert.Equal(3, d.Count);
            Assert.Equal(new[] {'a', 'c', 'd' }, d.OrderBy(i => i.Key).Select(i => i.Key));
            Assert.Equal(new[] { 0, 2, 3 }, d.OrderBy(i => i.Key).Select(i => i.Value));
        }

        [Fact]
        public void RemoveThenAddAndAddBack()
        {
            var d = new DictionarySlim<char, int>();
            d.GetOrAddValueRef('a') = 0;
            d.GetOrAddValueRef('b') = 1;
            d.GetOrAddValueRef('c') = 2;
            Assert.True(d.Remove('b'));
            d.GetOrAddValueRef('d') = 3;
            d.GetOrAddValueRef('b') = 7;
            Assert.Equal(4, d.Count);
            Assert.Equal(new[] { 'a', 'b', 'c', 'd' }, d.OrderBy(i => i.Key).Select(i => i.Key));
            Assert.Equal(new[] { 0, 7, 2, 3 }, d.OrderBy(i => i.Key).Select(i => i.Value));
        }

        [Fact]
        public void RemoveEnd()
        {
            var d = new DictionarySlim<char, int>();
            d.GetOrAddValueRef('a') = 0;
            d.GetOrAddValueRef('b') = 1;
            d.GetOrAddValueRef('c') = 2;
            Assert.True(d.Remove('c'));
            Assert.Equal(2, d.Count);
            Assert.Equal(new[] { 'a', 'b' }, d.OrderBy(i => i.Key).Select(i => i.Key));
            Assert.Equal(new[] { 0, 1 }, d.OrderBy(i => i.Key).Select(i => i.Value));
        }

        [Fact]
        public void RemoveEndTwice()
        {
            var d = new DictionarySlim<char, int>();
            d.GetOrAddValueRef('a') = 0;
            d.GetOrAddValueRef('b') = 1;
            d.GetOrAddValueRef('c') = 2;
            Assert.True(d.Remove('c'));
            Assert.True(d.Remove('b'));
            Assert.Equal(1, d.Count);
            Assert.Equal(new[] { 'a' }, d.OrderBy(i => i.Key).Select(i => i.Key));
            Assert.Equal(new[] { 0 }, d.OrderBy(i => i.Key).Select(i => i.Value));
        }

        [Fact]
        public void RemoveEndTwiceThenAdd()
        {
            var d = new DictionarySlim<char, int>();
            d.GetOrAddValueRef('a') = 0;
            d.GetOrAddValueRef('b') = 1;
            d.GetOrAddValueRef('c') = 2;
            Assert.True(d.Remove('c'));
            Assert.True(d.Remove('b'));
            d.GetOrAddValueRef('c') = 7;
            Assert.Equal(2, d.Count);
            Assert.Equal(new[] { 'a', 'c' }, d.OrderBy(i => i.Key).Select(i => i.Key));
            Assert.Equal(new[] { 0, 7 }, d.OrderBy(i => i.Key).Select(i => i.Value));
        }

        [Fact]
        public void RemoveSecondOfTwo()
        {
            var d = new DictionarySlim<char, int>();
            d.GetOrAddValueRef('a') = 0;
            d.GetOrAddValueRef('b') = 1;
            Assert.True(d.Remove('b'));
            Assert.Equal(0, d.GetOrAddValueRef('a'));
            Assert.Equal(1, d.Count);
        }

        [Fact]
        public void RemoveSlotReused()
        {
            var d = new DictionarySlim<Collider, int>();
            d.GetOrAddValueRef(C(0)) = 0;
            d.GetOrAddValueRef(C(1)) = 1;
            d.GetOrAddValueRef(C(2)) = 2;
            Assert.True(d.Remove(C(0)));
            _output.WriteLine("{0} {1}", d.GetCapacity(), d.Count);
            var capacity = d.GetCapacity();

            d.GetOrAddValueRef(C(0)) = 3;
            _output.WriteLine("{0} {1}", d.GetCapacity(), d.Count);
            Assert.Equal(d.GetOrAddValueRef(C(0)), 3);
            Assert.Equal(3, d.Count);
            Assert.Equal(capacity, d.GetCapacity());

        }

        [Fact]
        public void RemoveReleasesReferences()
        {
            var d = new DictionarySlim<KeyUseTracking, KeyUseTracking>();

            WeakReference<KeyUseTracking> a()
            {
                var kut = new KeyUseTracking(0);
                var wr = new WeakReference<KeyUseTracking>(kut);
                d.GetOrAddValueRef(kut) = kut;

                d.Remove(kut);
                return wr;
            }
            var ret = a();
            GC.Collect();
            GC.WaitForPendingFinalizers();
            Assert.False(ret.TryGetTarget(out _));
        }

        [Fact]
        public void RemoveEnumerate()
        {
            var d = new DictionarySlim<int, int>();
            d.GetOrAddValueRef(0) = 0;
            Assert.True(d.Remove(0));
            Assert.Empty(d);
        }

        [Fact]
        public void RemoveOneOfTwoEnumerate()
        {
            var d = new DictionarySlim<char, int>();
            d.GetOrAddValueRef('a') = 0;
            d.GetOrAddValueRef('b') = 1;
            Assert.True(d.Remove('a'));
            Assert.Equal(KeyValuePair.Create('b', 1), d.Single());
        }

        [Fact]
        public void EnumerateCheckEnding()
        {
            var d = new DictionarySlim<int, int>();
            int i = 0;
            d.GetOrAddValueRef(++i) = -i;
            d.GetOrAddValueRef(++i) = -i;
            d.GetOrAddValueRef(++i) = -i;
            d.GetOrAddValueRef(++i) = -i;
            while (d.Count < d.GetCapacity())
                d.GetOrAddValueRef(++i) = -i;
            Assert.Equal(d.Count, d.Count());
        }

        [Fact]
        public void EnumerateCheckEndingRemoveLast()
        {
            var d = new DictionarySlim<int, int>();
            int i = 0;
            d.GetOrAddValueRef(++i) = -i;
            d.GetOrAddValueRef(++i) = -i;
            d.GetOrAddValueRef(++i) = -i;
            d.GetOrAddValueRef(++i) = -i;
            while (d.Count < d.GetCapacity())
                d.GetOrAddValueRef(++i) = -i;
            Assert.True(d.Remove(i));
            Assert.Equal(d.Count, d.Count());
        }

        private KeyValuePair<TKey, TValue> P<TKey, TValue>(TKey key, TValue value) => new KeyValuePair<TKey, TValue>(key, value);

        [Fact]
        public void EnumerateReset()
        {
            var d = new DictionarySlim<int, int>();
            d.GetOrAddValueRef(1) = 10;
            d.GetOrAddValueRef(2) = 20;
            IEnumerator<KeyValuePair<int, int>> e = d.GetEnumerator();
            Assert.Equal(P(0, 0), e.Current);
            Assert.Equal(true, e.MoveNext());
            Assert.Equal(P(1, 10), e.Current);
            e.Reset();
            Assert.Equal(P(0, 0), e.Current);
            Assert.Equal(true, e.MoveNext());
            Assert.Equal(true, e.MoveNext());
            Assert.Equal(P(2, 20), e.Current);
            Assert.Equal(false, e.MoveNext());
            e.Reset();
            Assert.Equal(P(0, 0), e.Current);
        }

        [Fact]
        public void Clear()
        {
            var d = new DictionarySlim<int, int>();
            Assert.Equal(1, d.GetCapacity());
            d.GetOrAddValueRef(1) = 10;
            d.GetOrAddValueRef(2) = 20;
            Assert.Equal(2, d.Count);
            Assert.Equal(2, d.GetCapacity());
            d.Clear();
            Assert.Equal(0, d.Count);
            Assert.Equal(false, d.ContainsKey(1));
            Assert.Equal(false, d.ContainsKey(2));
            Assert.Equal(1, d.GetCapacity());
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

                rd.GetOrAddValueRef(k) += v;

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

                    rd.GetOrAddValueRef(k) += v;

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
            d.GetOrAddValueRef(C(5)) = 3;
            d.GetOrAddValueRef(C(7)) = 9;
            d.GetOrAddValueRef(C(10)) = 11;
            Assert.Equal(3, d.GetOrAddValueRef(C(5)));
            Assert.Equal(9, d.GetOrAddValueRef(C(7)));
            Assert.Equal(11, d.GetOrAddValueRef(C(10)));
            d.GetOrAddValueRef(C(23))++;
            d.GetOrAddValueRef(C(23)) += 3;
            Assert.Equal(4, d.GetOrAddValueRef(C(23)));
        }

        [Fact]
        public void UsedIEquatable()
        {
            var d = new DictionarySlim<KeyUseTracking, int>();
            var key = new KeyUseTracking(5);
            d.GetOrAddValueRef(key)++;
            Assert.Equal(2, key.GetHashCodeCount);
            Assert.Equal(0, key.EqualsCount);
        }

        [Fact]
        public void UsedIEquatable2()
        {
            var d = new DictionarySlim<KeyUseTracking, int>();
            var key = new KeyUseTracking(5);
            d.GetOrAddValueRef(key)++;
            d.GetOrAddValueRef(key)++;
            Assert.Equal(3, key.GetHashCodeCount);
            Assert.Equal(1, key.EqualsCount);
        }
    }

    internal static class DictionarySlimExtensions
    {
        // Capacity is not exposed publicly, but is valuable in tests to help
        // ensure everything is working as expected internally
        public static int GetCapacity<TKey, TValue>(this DictionarySlim<TKey, TValue> dict) where TKey : IEquatable<TKey>
        {
            FieldInfo fi = typeof(DictionarySlim<TKey, TValue>).GetField("_entries", BindingFlags.NonPublic | BindingFlags.Instance);
            Object entries = fi.GetValue(dict);

            PropertyInfo pi = typeof(Array).GetProperty("Length");
            return (int)pi.GetValue(entries);
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

    public override string ToString() => Convert.ToString(key);
}

[DebuggerDisplay("{Value}")]
internal class KeyUseTracking : IEquatable<KeyUseTracking>
{
    public int Value { get; }
    public int EqualsCount { get; private set; }
    public int GetHashCodeCount { get; private set; }

    public KeyUseTracking(int v)
    {
        Value = v;
    }

    public bool Equals(KeyUseTracking o)
    {
        EqualsCount++;
        return Value == o.Value;
    }

    public override bool Equals(object o)
    {
        return o is KeyUseTracking ck && Value == ck.Value;
    }

    public override int GetHashCode()
    {
        GetHashCodeCount++;
        return Value;
    }
}
