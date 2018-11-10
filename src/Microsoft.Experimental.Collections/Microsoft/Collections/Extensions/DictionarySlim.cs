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
    /// 1) It allows access to the value by ref replacing the common TryGetValue and Add pattern.
    /// 2) It does not store the hash code (assumes it is cheap to equate values).
    /// 3) It does not accept an equality comparer(assumes Object.GetHashCode() and Object.Equals() or overridden implementation are cheap and sufficient).
    /// </summary>
    [DebuggerTypeProxy(typeof(Extensions.DictionarySlimDebugView<,>))]
    [DebuggerDisplay("Count = {Count}")]
    public class DictionarySlim<TKey, TValue> : IReadOnlyCollection<KeyValuePair<TKey, TValue>> where TKey : IEquatable<TKey>
    {
        // Initial shared empty buckets and entries. The first add with cause a resize replacing these.
        private static readonly int[] InitialBuckets = new int[1];
        private static readonly Entry[] InitialEntries = new Entry[1];
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
            _buckets = InitialBuckets;
            _entries = InitialEntries;
        }

        public DictionarySlim(int capacity)
        {
            capacity = HashHelpers.GetPrime(capacity);
            _buckets = new int[capacity];
            _entries = new Entry[capacity];
        }

        // Drop sign bit to ensure non negative index
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private uint GetHashCode(TKey key) => (uint)key.GetHashCode();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int GetEntryIndex(int bucketIndex) => _buckets[bucketIndex] - 1;

        public int Count { get; private set; }

        public int Capacity => _entries.Length;

        public bool ContainsKey(TKey key)
        {
            Entry[] entries = _entries;
            int collisionCount = 0;
            for (int i = GetEntryIndex((int)(GetHashCode(key) % (uint)entries.Length));
                    (uint)i < (uint)entries.Length; i = entries[i].next)
            {
                if (key.Equals(entries[i].key))
                    return true;
                if (collisionCount >= entries.Length)
                {
                    // The chain of entries forms a loop; which means a concurrent update has happened.
                    // Break out of the loop and throw, rather than looping forever.
                    throw new Exception("ThrowHelper.ThrowInvalidOperationException_ConcurrentOperationsNotSupported()");
                }
                collisionCount++;
            }

            return false;
        }

        public TValue GetValueOrDefault(TKey key)
        {
            Entry[] entries = _entries;
            int collisionCount = 0;
            for (int i = GetEntryIndex((int)(GetHashCode(key) % (uint)entries.Length));
                    (uint)i < (uint)entries.Length; i = entries[i].next)
            {
                if (key.Equals(entries[i].key))
                    return entries[i].value;
                if (collisionCount >= entries.Length)
                {
                    // The chain of entries forms a loop; which means a concurrent update has happened.
                    // Break out of the loop and throw, rather than looping forever.
                    throw new Exception("ThrowHelper.ThrowInvalidOperationException_ConcurrentOperationsNotSupported()");
                }
                collisionCount++;
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

                    entries[entryIndex].next = -3 - _freeList; // New head of free list
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
                int collisionCount = 0;
                for (int i = GetEntryIndex(bucketIndex);
                        (uint)i < (uint)entries.Length; i = entries[i].next)
                {
                    if (key.Equals(entries[i].key))
                        return ref entries[i].value;
                    if (collisionCount >= entries.Length)
                    {
                        // The chain of entries forms a loop; which means a concurrent update has happened.
                        // Break out of the loop and throw, rather than looping forever.
                        throw new Exception("ThrowHelper.ThrowInvalidOperationException_ConcurrentOperationsNotSupported()");
                    }
                    collisionCount++;
                }

                return ref AddKey(key, bucketIndex);
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private ref TValue AddKey(TKey key, int bucketIndex)
        {
            Entry[] entries = _entries;
            int entryIndex;
            if (_freeList != -1)
            {
                entryIndex = _freeList;
                _freeList = -3 - entries[_freeList].next;
            }
            else
            {
                if (Count == entries.Length || entries.Length == 1)
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

        public KeyEnumerable Keys => new KeyEnumerable(this);

        public ValueEnumerable Values => new ValueEnumerable(this);

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int index)
        {
            Entry[] entries = _entries;
            int i = 0;
            int count = Count;
            while (count > 0)
            {
                if (entries[i].next > -2)
                {
                    count--;
                    array[index++] = new KeyValuePair<TKey, TValue>(
                        entries[i].key,
                        entries[i].value);
                }
                i++;
            }
        }

        IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator() => new Enumerator(this);
        IEnumerator IEnumerable.GetEnumerator() => new Enumerator(this);

        public struct Enumerator : IEnumerator<KeyValuePair<TKey, TValue>>
        {
            private readonly DictionarySlim<TKey, TValue> _dictionary;
            private int _index, _count;

            internal Enumerator(DictionarySlim<TKey, TValue> dictionary)
            {
                _dictionary = dictionary;
                _index = 0;
                _count = _dictionary.Count;
                Current = default;
            }

            public bool MoveNext()
            {
                if (_count == 0)
                {
                    Current = default;
                    return false;
                }

                _count--;

                while (_dictionary._entries[_index].next < -1)
                    _index++;

                Current = new KeyValuePair<TKey, TValue>(
                    _dictionary._entries[_index].key,
                    _dictionary._entries[_index++].value);
                return true;
            }

            public KeyValuePair<TKey, TValue> Current { get; private set; }
            object IEnumerator.Current => Current;
            void IEnumerator.Reset()
            {
                _index = 0;
                _count = _dictionary.Count;
            }
            public void Dispose() { }
        }

        public struct KeyEnumerable : IEnumerable<TKey>
        {
            private readonly DictionarySlim<TKey, TValue> _dictionary;

            internal KeyEnumerable(DictionarySlim<TKey, TValue> dictionary)
            {
                _dictionary = dictionary;
            }

            public void CopyTo(TKey[] array, int index)
            {
                Entry[] entries = _dictionary._entries;
                int i = 0;
                int count = _dictionary.Count;
                while (count > 0)
                {
                    if (entries[i].next > -2)
                    {
                        array[index++] = entries[i].key;
                        count--;
                    }
                    i++;
                }
            }

            public IEnumerator<TKey> GetEnumerator() => new KeyEnumerator(_dictionary);
            IEnumerator IEnumerable.GetEnumerator() => new KeyEnumerator(_dictionary);
        }

        public struct KeyEnumerator : IEnumerator<TKey>
        {
            private readonly DictionarySlim<TKey, TValue> _dictionary;
            private int _index, _count;

            internal KeyEnumerator(DictionarySlim<TKey, TValue> dictionary)
            {
                _dictionary = dictionary;
                _index = 0;
                _count = _dictionary.Count;
                Current = default;
            }

            public TKey Current { get; private set; }

            object IEnumerator.Current => Current;

            public void Dispose() { }

            public bool MoveNext()
            {
                if (_count == 0)
                {
                    Current = default;
                    return false;
                }

                _count--;

                while (_dictionary._entries[_index].next < -1)
                    _index++;

                Current = _dictionary._entries[_index++].key;
                return true;
            }

            public void Reset()
            {
                _index = 0;
                _count = _dictionary.Count;
            }
        }

        public struct ValueEnumerable : IEnumerable<TValue>
        {
            private readonly DictionarySlim<TKey, TValue> _dictionary;

            internal ValueEnumerable(DictionarySlim<TKey, TValue> dictionary)
            {
                _dictionary = dictionary;
            }

            public void CopyTo(TValue[] array, int index)
            {
                Entry[] entries = _dictionary._entries;
                int i = 0;
                int count = _dictionary.Count;
                while (count > 0)
                {
                    if (entries[i].next > -2)
                    {
                        array[index++] = entries[i].value;
                        count--;
                    }
                    i++;
                }
            }

            public IEnumerator<TValue> GetEnumerator() => new ValueEnumerator(_dictionary);
            IEnumerator IEnumerable.GetEnumerator() => new ValueEnumerator(_dictionary);
        }

        public struct ValueEnumerator : IEnumerator<TValue>
        {
            private readonly DictionarySlim<TKey, TValue> _dictionary;
            private int _index, _count;

            internal ValueEnumerator(DictionarySlim<TKey, TValue> dictionary)
            {
                _dictionary = dictionary;
                _index = 0;
                _count = _dictionary.Count;
                Current = default;
            }

            public TValue Current { get; private set; }

            object IEnumerator.Current => Current;

            public void Dispose() { }

            public bool MoveNext()
            {
                if (_count == 0)
                {
                    Current = default;
                    return false;
                }

                _count--;

                while (_dictionary._entries[_index].next < -1)
                    _index++;

                Current = _dictionary._entries[_index++].value;
                return true;
            }

            public void Reset()
            {
                _index = 0;
                _count = _dictionary.Count;
            }
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
            //if ((uint)newSize > MaxPrimeArrayLength && MaxPrimeArrayLength > oldSize)
            //{
            //    Debug.Assert(MaxPrimeArrayLength == GetPrime(MaxPrimeArrayLength), "Invalid MaxPrimeArrayLength");
            //    return MaxPrimeArrayLength;
            //}

            return GetPrime(newSize);
        }
    }
}
