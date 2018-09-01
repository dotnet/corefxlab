// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Microsoft.Collections.Extensions
{
    public sealed class RefDictionary<TKey, TValue> : IEnumerable<(TKey Key, TValue Value)> where TValue : new()
    {
        int[] _buckets;
        Entry[] _entries;
        int _count;
        int _hashBits = 18;
        int _hashMask;

        private struct Entry
        {
            public TKey key;
            public TValue value;
            public int next;
        }

        public RefDictionary()
        {
            _buckets = new int[1 << _hashBits];
            _entries = new Entry[1 << _hashBits];
            _hashMask = (1 << _hashBits) - 1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int GetBucketIndex(TKey key)
        {
            return key.GetHashCode() & _hashMask;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int GetEntryIndex(TKey key)
        {
            return _buckets[GetBucketIndex(key)] - 1;
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            Entry[] entries = _entries;
            int entryIndex = GetEntryIndex(key);

            while (true)
            {
                if ((uint)entryIndex >= (uint)entries.Length)
                {
                    break;
                }

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

                while (true)
                {
                    if ((uint)entryIndex >= (uint)entries.Length)
                    {
                        break;
                    }

                    if (entries[entryIndex].key.Equals(key))
                    {
                        return ref entries[entryIndex].value;
                    }

                    entryIndex = entries[entryIndex].next;
                }

                return ref Create(key);
            }
        }

        private ref TValue Create(TKey key)
        {
            if (_count == _entries.Length)
            {
                Resize();
            }

            int entryIndex = _count++;
            _entries[entryIndex].key = key;
            _entries[entryIndex].value = new TValue();
            int bucket = GetBucketIndex(key);
            _entries[entryIndex].next = _buckets[bucket] - 1;
            _buckets[bucket] = entryIndex + 1;
            return ref _entries[entryIndex].value;
        }

        private void Resize()
        {
            Entry[] oldEntries = _entries;
            int[] oldBuckets = _buckets;

            _hashBits++;
            _hashMask = (1 << _hashBits) - 1;

            Entry[] newEntries = new Entry[1 << _hashBits];
            int[] newBuckets = new int[1 << _hashBits];

            _buckets = newBuckets;
            _entries = newEntries;

            Array.Copy(oldEntries, 0, newEntries, 0, _count);

            for (int i = 0; i < _count; i++)
            {
                int bucket = GetBucketIndex(newEntries[i].key);
                newEntries[i].next = newBuckets[bucket] - 1;
                newBuckets[bucket] = i + 1;
            }
        }

        public Enumerator GetEnumerator() => new Enumerator(this);

        IEnumerator<(TKey Key, TValue Value)> IEnumerable<(TKey Key, TValue Value)>.GetEnumerator() => new Enumerator(this);
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => new Enumerator(this);

        public struct Enumerator : IEnumerator<(TKey Key, TValue Value)>
        {
            readonly RefDictionary<TKey, TValue> _dictionary;
            int index;
            (TKey, TValue) current;

            internal Enumerator(RefDictionary<TKey, TValue> dictionary)
            {
                _dictionary = dictionary;
                index = 0;
                current = new ValueTuple<TKey, TValue>();
            }

            public bool MoveNext()
            {
                Entry[] entries = _dictionary._entries;
                int count = _dictionary._count;
                int index = this.index;

                while (index < count)
                {
                    if (entries[index].next > 0)
                    {
                        current = (entries[index].key, entries[index].value);
                        this.index = index + 1;
                        return true;
                    }

                    index++;
                }

                this.index = count + 1;
                current = (default, default);
                return false;
            }

            public (TKey Key, TValue Value) Current => current;

            object System.Collections.IEnumerator.Current => current;
            void System.Collections.IEnumerator.Reset() => throw new NotImplementedException();

            public void Dispose()
            {
            }
        }
    }
}
