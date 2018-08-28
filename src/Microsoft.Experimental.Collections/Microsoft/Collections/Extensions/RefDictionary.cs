// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Microsoft.Collections.Extensions
{
    public sealed class RefDictionary<K, V> : IEnumerable<(K Key, V Value)>
    {
        private int[] buckets;
        private Entry[] entries;
        private int count;
        private int hashBits = 18;
        private int hashMask;

        private struct Entry
        {
            public K key;
            public V value;
            public int next;
        }

        public RefDictionary()
        {
            buckets = new int[1 << hashBits];
            entries = new Entry[1 << hashBits];
            hashMask = (1 << hashBits) - 1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int GetBucketIndex(K key)
        {
            return key.GetHashCode() & hashMask;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int GetEntryIndex(K key)
        {
            return buckets[GetBucketIndex(key)] - 1;
        }

        public bool TryGetValue(K key, out V value)
        {
            Entry[] entries = this.entries;
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

        public ref V GetRef(K key)
        {
            Entry[] entries = this.entries;
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

        private ref V Create(K key)
        {
            if (count == entries.Length)
            {
                Resize();
            }

            int entryIndex = count++;
            entries[entryIndex].key = key;
            entries[entryIndex].value = default;
            int bucket = GetBucketIndex(key);
            entries[entryIndex].next = buckets[bucket] - 1;
            buckets[bucket] = entryIndex + 1;
            return ref entries[entryIndex].value;
        }

        private void Resize()
        {
            Entry[] oldEntries = entries;
            int[] oldBuckets = buckets;

            hashBits++;
            hashMask = (1 << hashBits) - 1;

            Entry[] newEntries = new Entry[1 << hashBits];
            int[] newBuckets = new int[1 << hashBits];

            buckets = newBuckets;
            entries = newEntries;

            Array.Copy(oldEntries, 0, newEntries, 0, count);

            for (int i = 0; i < count; i++)
            {
                int bucket = GetBucketIndex(newEntries[i].key);
                newEntries[i].next = newBuckets[bucket] - 1;
                newBuckets[bucket] = i + 1;
            }
        }

        public Enumerator GetEnumerator() => new Enumerator(this);

        IEnumerator<(K Key, V Value)> IEnumerable<(K Key, V Value)>.GetEnumerator() => new Enumerator(this);
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => new Enumerator(this);

        public struct Enumerator : IEnumerator<(K Key, V Value)>
        {
            private readonly RefDictionary<K, V> dictionary;
            private int index;
            private (K, V) current;

            internal Enumerator(RefDictionary<K, V> dictionary)
            {
                this.dictionary = dictionary;

                index = 0;
                current = new ValueTuple<K, V>();
            }

            public bool MoveNext()
            {
                Entry[] entries = dictionary.entries;
                int count = dictionary.count;
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

            public (K Key, V Value) Current => current;

            object System.Collections.IEnumerator.Current => current;
            void System.Collections.IEnumerator.Reset() => throw new NotImplementedException();

            public void Dispose()
            {
            }
        }
    }
}
