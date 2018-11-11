// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Linq;

namespace Microsoft.Collections.Extensions
{
    /// <summary>
    /// DictionarySlim<TKey, TValue> is similar to Dictionary<TKey, TValue> but optimized in three ways:
    /// 1) It allows access to the value by ref replacing the common TryGetValue and Add pattern.
    /// 2) It does not store the hash code (assumes it is cheap to equate values).
    /// 3) It does not accept an equality comparer (assumes Object.GetHashCode() and Object.Equals() or overridden implementation are cheap and sufficient).
    /// </summary>
    [DebuggerTypeProxy(typeof(Extensions.DictionarySlimDebugView<,>))]
    [DebuggerDisplay("Count = {Count}")]
    public class DictionarySlim<TKey, TValue> : IReadOnlyCollection<KeyValuePair<TKey, TValue>> where TKey : IEquatable<TKey>
    {
        // We want to initialize without allocating arrays. We also want to avoid null checks.
        // Array.Empty would give divide by zero in modulo operation. So we use static one element arrays.
        // The first add will cause a resize replacing these with real arrays of three elements.
        // Arrays are wrapped in a class to avoid being duplicated for each <TKey, TValue>
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
            // 0-based index of next entry in chain: -1 means end of chain
            // also encodes whether this entry _itself_ is part of the free list by changing sign and subtracting 3,
            // so -2 means end of free list, -3 means index 0 but on free list, -4 means index 1 but on free list, etc.
            public int next;
        }

        public DictionarySlim()
        {
            _buckets = HashHelpers.DictionarySlimSizeOneIntArray;
            _entries = InitialEntries;
        }

        public DictionarySlim(int capacity)
        {
            capacity = HashHelpers.GetPrime(capacity);
            _buckets = new int[capacity];
            _entries = new Entry[capacity];
        }

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
                    throw new InvalidOperationException(Strings.InvalidOperation_ConcurrentOperationsNotSupported);
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
                    throw new InvalidOperationException(Strings.InvalidOperation_ConcurrentOperationsNotSupported);
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
                Entry candidate = entries[entryIndex];
                if (candidate.key.Equals(key))
                {
                    if (lastIndex != -1)
                    {   // Fixup preceding element in chain to point to next (if any)
                        entries[lastIndex].next = candidate.next;
                    }
                    else
                    {   // Fixup bucket to new head (if any)
                        _buckets[bucketIndex] = candidate.next + 1;
                    }

                    entries[entryIndex] = default; // could use RuntimeHelpers.IsReferenceOrContainsReferences

                    entries[entryIndex].next = -3 - _freeList; // New head of free list
                    _freeList = entryIndex;

                    Count--;
                    return true;
                }
                lastIndex = entryIndex;
                entryIndex = candidate.next;
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
                        throw new InvalidOperationException(Strings.InvalidOperation_ConcurrentOperationsNotSupported);
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
                Entry entry = entries[i];
                if (entry.next > -2) // part of free list?
                {
                    count--;
                    array[index++] = new KeyValuePair<TKey, TValue>(
                        entry.key,
                        entry.value);
                }
                i++;
            }
        }

        public Enumerator GetEnumerator() => new Enumerator(this); // avoid boxing
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

        public struct KeyEnumerable : ICollection<TKey>, IReadOnlyCollection<TKey>
        {
            private readonly DictionarySlim<TKey, TValue> _dictionary;

            internal KeyEnumerable(DictionarySlim<TKey, TValue> dictionary)
            {
                _dictionary = dictionary;
            }

            public int Count => _dictionary.Count;

            public bool IsReadOnly => true;

            void ICollection<TKey>.Add(TKey item) => throw new NotSupportedException(Strings.ReadOnly_Modification);

            void ICollection<TKey>.Clear() => throw new NotSupportedException(Strings.ReadOnly_Modification);

            public bool Contains(TKey item) => _dictionary.ContainsKey(item);

            bool ICollection<TKey>.Remove(TKey item) => throw new NotSupportedException(Strings.ReadOnly_Modification);

            public void CopyTo(TKey[] array, int index)
            {
                Entry[] entries = _dictionary._entries;
                int i = 0;
                int count = _dictionary.Count;
                while (count > 0)
                {
                    Entry entry = entries[i];
                    if (entry.next > -2)  // part of free list?
                    {
                        array[index++] = entry.key;
                        count--;
                    }
                    i++;
                }
            }

            public Enumerator GetEnumerator() => new Enumerator(_dictionary); // avoid boxing
            IEnumerator<TKey> IEnumerable<TKey>.GetEnumerator() => new Enumerator(_dictionary);
            IEnumerator IEnumerable.GetEnumerator() => new Enumerator(_dictionary);

            public struct Enumerator : IEnumerator<TKey>
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
        }

        public struct ValueEnumerable : ICollection<TValue>, IReadOnlyCollection<TValue>
        {
            private readonly DictionarySlim<TKey, TValue> _dictionary;

            internal ValueEnumerable(DictionarySlim<TKey, TValue> dictionary)
            {
                _dictionary = dictionary;
            }

            public int Count => _dictionary.Count;

            public bool IsReadOnly => true;

            void ICollection<TValue>.Add(TValue item) => throw new NotSupportedException(Strings.ReadOnly_Modification);

            void ICollection<TValue>.Clear() => throw new NotSupportedException(Strings.ReadOnly_Modification);

            bool ICollection<TValue>.Contains(TValue item) => throw new NotSupportedException(); // performance antipattern

            bool ICollection<TValue>.Remove(TValue item) => throw new NotSupportedException(Strings.ReadOnly_Modification);

            public void CopyTo(TValue[] array, int index)
            {
                Entry[] entries = _dictionary._entries;
                int i = 0;
                int count = _dictionary.Count;
                while (count > 0)
                {
                    if (entries[i].next > -2)  // part of free list?
                    {
                        array[index++] = entries[i].value;
                        count--;
                    }
                    i++;
                }
            }

            public Enumerator GetEnumerator() => new Enumerator(_dictionary); // avoid boxing
            IEnumerator<TValue> IEnumerable<TValue>.GetEnumerator() => new Enumerator(_dictionary);
            IEnumerator IEnumerable.GetEnumerator() => new Enumerator(_dictionary);

            public struct Enumerator : IEnumerator<TValue>
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
