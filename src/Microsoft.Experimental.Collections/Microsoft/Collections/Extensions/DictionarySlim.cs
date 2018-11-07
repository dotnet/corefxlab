// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Linq;

namespace Microsoft.Experimental.Collections
{
    /// <summary>
    /// DictionarySlim<TKey, TValue> is similar to Dictionary<TKey, TValue> but optimized in three ways:
    /// 1) It allows access to the value by ref.
    /// 2) It does not store the hash code (assumes it is cheap to equate values).
    /// 3) It does not accept an equality comparer(assumes Object.GetHashCode() and Object.Equals() or overridden implementation are cheap and sufficient).
    /// </summary>
    [DebuggerTypeProxy(typeof(Extensions.DictionarySlimDebugView<,>))]
    [DebuggerDisplay("Count = {Count}")]
    public class DictionarySlim<TKey, TValue> : IReadOnlyCollection<KeyValuePair<TKey, TValue>> where TKey : IEquatable<TKey>
    {
        const int DefaultPrimeSize = 3;
        // 1-based index into _entries; 0 means empty
        private int[] _buckets;
        private Entry[] _entries;
        // 0-based index into _entries of head of free chain: -1 means empty
        private int _freeList = -1;

        private struct Entry
        {
            public TKey key;
            public TValue value;
            // 0-based index of next entry in chain: -1 means empty
            public int next;
        }

        public DictionarySlim()
        {
            _buckets = new int[DefaultPrimeSize];
            _entries = new Entry[DefaultPrimeSize];
        }

        public DictionarySlim(int capacity)
        {
            capacity = HashHelpers.GetPrime(capacity);
            _buckets = new int[capacity];
            _entries = new Entry[capacity];
        }

        // Drop sign bit to ensure non negative index
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private uint GetHashCode(TKey key) => (uint)key.GetHashCode();//key.GetHashCode() & 0x7FFFFFFF;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int GetEntryIndex(int bucketIndex) => _buckets[bucketIndex] - 1;

        public int Count { get; private set; }

        public int Capacity => _entries.Length;

        public bool ContainsKey(TKey key)
        {
            Entry[] entries = _entries;
            int entryIndex = GetEntryIndex((int)(GetHashCode(key) % (uint)entries.Length));

            while (entryIndex != -1)
            {
                if (entries[entryIndex].key.Equals(key))
                {
                    return true;
                }
                entryIndex = entries[entryIndex].next;
            }

            return false;
        }

        public TValue GetValueOrDefault(TKey key)
        {
            Entry[] entries = _entries;
            int entryIndex = GetEntryIndex((int)(GetHashCode(key) % (uint)entries.Length));

            while (entryIndex != -1)
            {
                if (entries[entryIndex].key.Equals(key))
                {
                    return entries[entryIndex].value;
                }
                entryIndex = entries[entryIndex].next;
            }

            return default;
        }

        public bool Remove(TKey key)
        {
            Entry[] entries = _entries;
            int bucketIndex = (int)(GetHashCode(key) % (uint)entries.Length);
            int entryIndex = GetEntryIndex(bucketIndex);
            
            int lastIndex = -1;
            while (entryIndex != -1)
            {
                if (entries[entryIndex].key.Equals(key))
                {
                    if (lastIndex != -1)
                    {   // Fixup preceding element in chain to point to next (if any)
                        entries[lastIndex].next = entries[entryIndex].next;
                    }
                    else
                    {   // Fixup bucket to new head (if any)
                        _buckets[bucketIndex] = entries[entryIndex].next + 1;
                    }

                    entries[entryIndex] = default; // could use RuntimeHelpers.IsReferenceOrContainsReferences

                    entries[entryIndex].next = _freeList; // New head of free list
                    _freeList = entryIndex;

                    Count--;
                    return true;
                }
                lastIndex = entryIndex;
                entryIndex = entries[entryIndex].next;
            }

            return false;
        }

        public ref TValue this[TKey key]
        {
            get
            {
                Entry[] entries = _entries;
                int bucketIndex = (int)(GetHashCode(key) % (uint)entries.Length);
                int entryIndex = GetEntryIndex(bucketIndex);

                for(int i = entryIndex; i != -1; i = entries[i].next)
                {
                    if (entries[i].key.Equals(key))
                    {
                        return ref entries[i].value;
                    }
                }

                if (_freeList != -1)
                {
                    entryIndex = _freeList;
                    _freeList = entries[_freeList].next;
                }
                else
                {
                    if (Count == entries.Length)
                    {
                        entries = Resize();
                        bucketIndex = (int)(GetHashCode(key) % (uint)entries.Length);
                        // entry indexes were not changed by Resize
                    }
                    entryIndex = Count;
                }

                entries[entryIndex].key = key;
                entries[entryIndex].next = _buckets[bucketIndex] - 1;
                _buckets[bucketIndex] = entryIndex + 1;
                Count++;
                return ref entries[entryIndex].value;
            }
        }

        private Entry[] Resize()
        {
            int count = Count;
            int newSize = HashHelpers.ExpandPrime(count);
            var entries = new Entry[newSize];
            Array.Copy(_entries, 0, entries, 0, count);

            var newBuckets = new int[newSize];
            while (count-- > 0)
            {
                int bucketIndex = (int)(GetHashCode(entries[count].key) % (uint)newBuckets.Length);
                entries[count].next = newBuckets[bucketIndex] - 1;
                newBuckets[bucketIndex] = count + 1;
            }

            _buckets = newBuckets;
            _entries = entries;

            return entries;
        }

        public IEnumerable<TKey> Keys
        {
            get
            {
                Entry[] entries = _entries;
                for (int i = 0; i < Count; i++)
                {
                    yield return entries[i].key;
                }
            }
        }

        public IEnumerable<TValue> Values
        {
            get
            {
                Entry[] entries = _entries;
                for (int i = 0; i < Count; i++)
                {
                    yield return entries[i].value;
                }
            }
        }

        IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator() => new Enumerator(this);
        IEnumerator IEnumerable.GetEnumerator() => new Enumerator(this);

        public struct Enumerator : IEnumerator<KeyValuePair<TKey, TValue>>
        {
            private readonly DictionarySlim<TKey, TValue> _dictionary;
            private int _index;
            private int _found;

            private readonly HashSet<int> _freeEntries;

            internal Enumerator(DictionarySlim<TKey, TValue> dictionary)
            {
                _dictionary = dictionary;
                _index = 0;
                _found = 0;
                Current = default;

                _freeEntries = new HashSet<int>();
                int free = dictionary._freeList;
                while (free != -1)
                {
                    _freeEntries.Add(free);
                    free = dictionary._entries[free].next;
                }
            }

            public bool MoveNext()
            {
                while (_index < _dictionary._entries.Length && _found < _dictionary.Count)
                {
                    if (_freeEntries.Contains(_index))
                    {
                        _index++;
                        continue;
                    }
                    Current = new KeyValuePair<TKey, TValue>(
                        _dictionary._entries[_index].key,
                        _dictionary._entries[_index++].value);
                    _found++;
                    return true;
                }

                Current = default;
                return false;
             }

            public KeyValuePair<TKey, TValue> Current { get; private set; }
            object IEnumerator.Current => Current;
            void IEnumerator.Reset() => _index = 0;
            public void Dispose() { }
        }
    }
}

namespace Microsoft.Experimental.Collections.Extensions
{
    internal sealed class DictionarySlimDebugView<K, V> where K : IEquatable<K>
    {
        private readonly DictionarySlim<K, V> _dict;

        public DictionarySlimDebugView(DictionarySlim<K, V> dictionary)
        {
            _dict = dictionary ?? throw new ArgumentNullException(nameof(dictionary));
        }

        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        public KeyValuePair<K, V>[] Items
        {
            get
            {
                return _dict.ToArray();
            }
        }
    }
}

namespace System.Collections
{
    internal static partial class HashHelpers
    {
        public const int HashCollisionThreshold = 100;

        // This is the maximum prime smaller than Array.MaxArrayLength
        public const int MaxPrimeArrayLength = 0x7FEFFFFD;

        public const int HashPrime = 101;

        // Table of prime numbers to use as hash table sizes. 
        // A typical resize algorithm would pick the smallest prime number in this array
        // that is larger than twice the previous capacity. 
        // Suppose our Hashtable currently has capacity x and enough elements are added 
        // such that a resize needs to occur. Resizing first computes 2x then finds the 
        // first prime in the table greater than 2x, i.e. if primes are ordered 
        // p_1, p_2, ..., p_i, ..., it finds p_n such that p_n-1 < 2x < p_n. 
        // Doubling is important for preserving the asymptotic complexity of the 
        // hashtable operations such as add.  Having a prime guarantees that double 
        // hashing does not lead to infinite loops.  IE, your hash function will be 
        // h1(key) + i*h2(key), 0 <= i < size.  h2 and the size must be relatively prime.
        // We prefer the low computation costs of higher prime numbers over the increased
        // memory allocation of a fixed prime number i.e. when right sizing a HashSet.
        public static readonly int[] primes = {
            3, 7, 11, 17, 23, 29, 37, 47, 59, 71, 89, 107, 131, 163, 197, 239, 293, 353, 431, 521, 631, 761, 919,
            1103, 1327, 1597, 1931, 2333, 2801, 3371, 4049, 4861, 5839, 7013, 8419, 10103, 12143, 14591,
            17519, 21023, 25229, 30293, 36353, 43627, 52361, 62851, 75431, 90523, 108631, 130363, 156437,
            187751, 225307, 270371, 324449, 389357, 467237, 560689, 672827, 807403, 968897, 1162687, 1395263,
            1674319, 2009191, 2411033, 2893249, 3471899, 4166287, 4999559, 5999471, 7199369 };

        public static bool IsPrime(int candidate)
        {
            if ((candidate & 1) != 0)
            {
                int limit = (int)Math.Sqrt(candidate);
                for (int divisor = 3; divisor <= limit; divisor += 2)
                {
                    if ((candidate % divisor) == 0)
                        return false;
                }
                return true;
            }
            return (candidate == 2);
        }

        public static int GetPrime(int min)
        {
            //if (min < 0)
            //    throw new ArgumentException(SR.Arg_HTCapacityOverflow);

            for (int i = 0; i < primes.Length; i++)
            {
                int prime = primes[i];
                if (prime >= min)
                    return prime;
            }


            throw new Exception("need to do something here : " + min);
            //outside of our predefined table. 
            //compute the hard way. 
            //for (int i = (min | 1); i < int.MaxValue; i += 2)
            //{
            //    if (IsPrime(i) && ((i - 1) % HashPrime != 0))
            //        return i;
            //}
            //return min;
        }

        // Returns size of hashtable to grow to.
        public static int ExpandPrime(int oldSize)
        {
            int newSize = 2 * oldSize;

            // Allow the hashtables to grow to maximum possible size (~2G elements) before encountering capacity overflow.
            // Note that this check works even when _items.Length overflowed thanks to the (uint) cast
            //if ((uint)newSize > MaxPrimeArrayLength && MaxPrimeArrayLength > oldSize)
            //{
            //    Debug.Assert(MaxPrimeArrayLength == GetPrime(MaxPrimeArrayLength), "Invalid MaxPrimeArrayLength");
            //    return MaxPrimeArrayLength;
            //}

            return GetPrime(newSize);
        }
    }
}
