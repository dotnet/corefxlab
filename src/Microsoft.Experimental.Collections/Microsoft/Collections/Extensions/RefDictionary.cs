// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Microsoft.Experimental.Collections
{
    public sealed class RefDictionary<TKey, TValue> : IReadOnlyCollection<KeyValuePair<TKey, TValue>> where TKey : IEquatable<TKey> // almost IReadOnlyDictionary<TKey, TValue>
    {
        private int[] _buckets;
        private Entry[] _entries;

        private struct Entry
        {
            public TKey key;
            public TValue value;
            public int next;
        }

        public RefDictionary(int capacity = 16)
        {
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

        public bool TryGetValue(TKey key, out TValue value)
        {
            Entry[] entries = _entries;
            int entryIndex = GetEntryIndex(key);

            while (entryIndex != -1)
            {
                if (entries[entryIndex].key.Equals(key))
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
                    if (entries[entryIndex].key.Equals(key))
                    {
                        return ref entries[entryIndex].value;
                    }
                    entryIndex = entries[entryIndex].next;
                }

                if (Count == entries.Length)
                {
                    var count = Count;
                    entries = new Entry[count * 2];
                    Array.Copy(_entries, 0, entries, 0, count);
                    _entries = entries;

                    int[] newBuckets = new int[count * 2];
                    _buckets = newBuckets;
                    for (int i = 0; i < count;)
                    {
                        int bucketIndex = GetBucketIndex(entries[i].key);
                        entries[i].next = newBuckets[bucketIndex] - 1;
                        newBuckets[bucketIndex] = ++i;
                    }
                }

                entryIndex = Count++;
                int bucket = GetBucketIndex(key);
                _entries[entryIndex].next = _buckets[bucket] - 1;
                _buckets[bucket] = entryIndex + 1;
                entries[entryIndex].key = key;
                entries[entryIndex].value = default;
                return ref entries[entryIndex].value;
            }
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
                int count = _dictionary.Count;
                if (_index < count)
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
