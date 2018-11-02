// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Microsoft.Experimental.Collections
{
    public class DictionarySlim<TKey, TValue> : IReadOnlyCollection<KeyValuePair<TKey, TValue>> where TKey : IEquatable<TKey>
    {
        const int DefaultPrimeSize = 11;
        private int[] _buckets;
        private Entry[] _entries;

        private struct Entry
        {
            public TKey key;
            public TValue value;
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int GetBucketIndex(TKey key) => (key.GetHashCode() & 0x7FFFFFFF) % _buckets.Length;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int GetEntryIndex(TKey key) => _buckets[GetBucketIndex(key)] - 1;

        public int Count { get; private set; }

        public bool ContainsKey(TKey key)
        {
            Entry[] entries = _entries;
            int entryIndex = GetEntryIndex(key);

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
            int entryIndex = GetEntryIndex(key);

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

        public ref TValue this[TKey key]
        {
            get
            {
                Entry[] entries = _entries;
                int entryIndex = GetEntryIndex(key);

                while (entryIndex != -1)
                {
                    if (entries[entryIndex].key.Equals(key))
                    {
                        return ref entries[entryIndex].value;
                    }
                    entryIndex = entries[entryIndex].next;
                }

                if (Count == entries.Length) entries = Resize();

                entryIndex = Count++;
                entries[entryIndex].key = key;
                int bucket = GetBucketIndex(key);
                entries[entryIndex].next = _buckets[bucket] - 1;
                _buckets[bucket] = entryIndex + 1;
                return ref entries[entryIndex].value;
            }
        }

        private Entry[] Resize()
        {
            var count = Count;
            var newSize = HashHelpers.ExpandPrime(count);
            var entries = new Entry[newSize];
            Array.Copy(_entries, 0, entries, 0, count);
            _entries = entries;

            int[] newBuckets = new int[newSize];
            _buckets = newBuckets;
            for (int i = 0; i < count;)
            {
                int bucketIndex = GetBucketIndex(entries[i].key);
                entries[i].next = newBuckets[bucketIndex] - 1;
                newBuckets[bucketIndex] = ++i;
            }

            return entries;
        }

        public IEnumerable<TKey> Keys
        {
            get
            {
                var entries = _entries;
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
                var entries = _entries;
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

            internal Enumerator(DictionarySlim<TKey, TValue> dictionary)
            {
                _dictionary = dictionary;
                _index = 0;
                Current = default;
            }

            public bool MoveNext()
            {
                if (_index < _dictionary.Count)
                {
                    Current = new KeyValuePair<TKey, TValue>(
                        _dictionary._entries[_index].key,
                        _dictionary._entries[_index++].value);
                    return true;
                }
                else
                {
                    Current = default;
                    return false;
                }
            }

            public KeyValuePair<TKey, TValue> Current { get; private set; }
            object IEnumerator.Current => Current;
            void IEnumerator.Reset() => _index = 0;
            public void Dispose() { }
        }
    }
}

namespace System.Collections
{
    using System.Diagnostics;
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

            //outside of our predefined table. 
            //compute the hard way. 
            for (int i = (min | 1); i < int.MaxValue; i += 2)
            {
                if (IsPrime(i) && ((i - 1) % HashPrime != 0))
                    return i;
            }
            return min;
        }

        // Returns size of hashtable to grow to.
        public static int ExpandPrime(int oldSize)
        {
            int newSize = 2 * oldSize;

            // Allow the hashtables to grow to maximum possible size (~2G elements) before encountering capacity overflow.
            // Note that this check works even when _items.Length overflowed thanks to the (uint) cast
            if ((uint)newSize > MaxPrimeArrayLength && MaxPrimeArrayLength > oldSize)
            {
                Debug.Assert(MaxPrimeArrayLength == GetPrime(MaxPrimeArrayLength), "Invalid MaxPrimeArrayLength");
                return MaxPrimeArrayLength;
            }

            return GetPrime(newSize);
        }
    }
}
