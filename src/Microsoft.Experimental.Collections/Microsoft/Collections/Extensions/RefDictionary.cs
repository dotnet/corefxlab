// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Microsoft.Experimental.Collections
{
    public class RefDictionary<TKey, TValue> : IReadOnlyCollection<KeyValuePair<TKey, TValue>> where TKey : IEquatable<TKey> // almost IReadOnlyDictionary<TKey, TValue>
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private Entry[] Resize()
        {
            var count = Count;
            var entries = new Entry[count * 2];
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
