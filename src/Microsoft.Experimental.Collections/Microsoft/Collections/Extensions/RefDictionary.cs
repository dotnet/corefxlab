// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Microsoft.Collections.Extensions
{
    public sealed class RefDictionary<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>>
    {
        private IEqualityComparer<TKey> _comparer;
        private int[] _buckets;
        private Entry[] _entries;
        private int _count;

        private struct Entry
        {
            public TKey key;
            public TValue value;
            public int next;
        }

        public RefDictionary(int capacity, IEqualityComparer<TKey> comparer)
        {
            _buckets = new int[capacity];
            _entries = new Entry[capacity];
            _comparer = comparer ?? EqualityComparer<TKey>.Default;
        }

        public RefDictionary(IEqualityComparer<TKey> comparer) : this(16, comparer) { }
        public RefDictionary(int capacity) : this(capacity, null) { }
        public RefDictionary() : this(null) { }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int GetBucketIndex(TKey key) => (_comparer.GetHashCode(key) & 0x7FFFFFFF) % _buckets.Length;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int GetEntryIndex(TKey key) => _buckets[GetBucketIndex(key)] - 1;

        public bool TryGetValue(TKey key, out TValue value)
        {
            Entry[] entries = _entries;
            int entryIndex = GetEntryIndex(key);

            while (entryIndex != -1)
            {
                if (_comparer.Equals(entries[entryIndex].key, key))
                {
                    value = entries[entryIndex].value;
                    return true;
                }
                entryIndex = entries[entryIndex].next;
            }

            value = default;
            return false;
        }

        public ref TValue this[TKey key]
        {
            get
            {
                Entry[] entries = _entries;
                int entryIndex = GetEntryIndex(key);

                while (entryIndex != -1)
                {
                    if (_comparer.Equals(entries[entryIndex].key, key))
                    {
                        return ref entries[entryIndex].value;
                    }
                    entryIndex = entries[entryIndex].next;
                }

                if (_count == entries.Length)
                {
                    entries = new Entry[_count * 2];
                    Array.Copy(_entries, 0, entries, 0, _count);
                    _entries = entries;

                    int[] newBuckets = new int[_count * 2];
                    _buckets = newBuckets;
                    for (int i = 0; i < _count; i++)
                    {
                        int bucketIndex = GetBucketIndex(entries[i].key);
                        entries[i].next = newBuckets[bucketIndex] - 1;
                        newBuckets[bucketIndex] = i + 1;
                    }
                }

                entryIndex = _count++;
                int bucket = GetBucketIndex(key);
                _entries[entryIndex].next = _buckets[bucket] - 1;
                _buckets[bucket] = entryIndex + 1;
                entries[entryIndex].key = key;
                entries[entryIndex].value = default;
                return ref entries[entryIndex].value;
            }
        }

        public Enumerator GetEnumerator() => new Enumerator(this);

        IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator() => new Enumerator(this);
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => new Enumerator(this);

        public struct Enumerator : IEnumerator<KeyValuePair<TKey, TValue>>
        {
            private readonly RefDictionary<TKey, TValue> _dictionary;
            private int _index;

            internal Enumerator(RefDictionary<TKey, TValue> dictionary)
            {
                _dictionary = dictionary;
                _index = 0;
                Current = default;
            }

            public bool MoveNext()
            {
                Entry[] entries = _dictionary._entries;
                int count = _dictionary._count;
                if(_index < count)
                {
                    Current = new KeyValuePair<TKey, TValue>(entries[_index].key, entries[_index].value);
                    _index++;
                    return true;
                }
                else
                {
                    Current = default;
                    _index++;
                    return false;
                }
            }

            public KeyValuePair<TKey, TValue> Current { get; private set; }

            object System.Collections.IEnumerator.Current => Current;
            void System.Collections.IEnumerator.Reset() => _index = 0;

            public void Dispose() { }
        }
    }
}
