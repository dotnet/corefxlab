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
        private int _count;

        [DebuggerDisplay("({key}, {value})->{next}")]
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

        public int Count => _count;

        public int Capacity => _entries.Length;

        public bool ContainsKey(TKey key)
        {
            Entry[] entries = _entries;
            int collisionCount = 0;
            for (int i = _buckets[(int)((uint)key.GetHashCode() % (uint)_buckets.Length)] - 1;
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
            for (int i = _buckets[(int)((uint)key.GetHashCode() % (uint)_buckets.Length)] - 1;
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
            int bucketIndex = (int)((uint)key.GetHashCode() % (uint)_buckets.Length);
            int entryIndex = _buckets[bucketIndex] - 1;

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

                    _count--;
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
                int collisionCount = 0;
                int bucketIndex = (int)((uint)key.GetHashCode() % (uint)_buckets.Length);
                for (int i = _buckets[bucketIndex] - 1;
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
                if (_count == entries.Length || entries.Length == 1)
                {
                    entries = Resize();
                    bucketIndex = (int)((uint)key.GetHashCode() % (uint)_buckets.Length);
                    // entry indexes were not changed by Resize
                }
                entryIndex = _count;
            }

            entries[entryIndex].key = key;
            entries[entryIndex].next = _buckets[bucketIndex] - 1;
            _buckets[bucketIndex] = entryIndex + 1;
            _count++;
            return ref entries[entryIndex].value;
        }

        private Entry[] Resize()
        {
            int count = _count;
            int newSize = HashHelpers.ExpandPrime(count);
            var entries = new Entry[newSize];
            Array.Copy(_entries, 0, entries, 0, count);

            var newBuckets = new int[newSize];
            while (count-- > 0)
            {
                int bucketIndex = (int)((uint)entries[count].key.GetHashCode() % (uint)newBuckets.Length);
                entries[count].next = newBuckets[bucketIndex] - 1;
                newBuckets[bucketIndex] = count + 1;
            }

            _buckets = newBuckets;
            _entries = entries;

            return entries;
        }

        public KeyCollection Keys => new KeyCollection(this);

        public ValueCollection Values => new ValueCollection(this);

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int index)
        {
            Entry[] entries = _entries;
            int i = 0;
            int count = _count;
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
            private KeyValuePair<TKey, TValue> _current;

            internal Enumerator(DictionarySlim<TKey, TValue> dictionary)
            {
                _dictionary = dictionary;
                _index = 0;
                _count = _dictionary._count;
                _current = default;
            }

            public bool MoveNext()
            {
                if (_count == 0)
                {
                    _current = default;
                    return false;
                }

                _count--;

                while (_dictionary._entries[_index].next < -1)
                    _index++;

                _current = new KeyValuePair<TKey, TValue>(
                    _dictionary._entries[_index].key,
                    _dictionary._entries[_index++].value);
                return true;
            }

            public KeyValuePair<TKey, TValue> Current => _current;

            object IEnumerator.Current => _current;

            void IEnumerator.Reset()
            {
                _index = 0;
                _count = _dictionary._count;
            }

            public void Dispose() { }
        }

        public struct KeyCollection : ICollection<TKey>, IReadOnlyCollection<TKey>
        {
            private readonly DictionarySlim<TKey, TValue> _dictionary;

            internal KeyCollection(DictionarySlim<TKey, TValue> dictionary)
            {
                _dictionary = dictionary;
            }

            public int Count => _dictionary._count;

            bool ICollection<TKey>.IsReadOnly => true;

            void ICollection<TKey>.Add(TKey item) => throw new NotSupportedException(Strings.ReadOnly_Modification);

            void ICollection<TKey>.Clear() => throw new NotSupportedException(Strings.ReadOnly_Modification);

            public bool Contains(TKey item) => _dictionary.ContainsKey(item);

            bool ICollection<TKey>.Remove(TKey item) => throw new NotSupportedException(Strings.ReadOnly_Modification);

            public void CopyTo(TKey[] array, int index)
            {
                Entry[] entries = _dictionary._entries;
                int i = 0;
                int count = _dictionary._count;
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
                private TKey _current;

                internal Enumerator(DictionarySlim<TKey, TValue> dictionary)
                {
                    _dictionary = dictionary;
                    _index = 0;
                    _count = _dictionary._count;
                    _current = default;
                }

                public TKey Current => _current;

                object IEnumerator.Current => _current;

                public void Dispose() { }

                public bool MoveNext()
                {
                    if (_count == 0)
                    {
                        _current = default;
                        return false;
                    }

                    _count--;

                    while (_dictionary._entries[_index].next < -1)
                        _index++;

                    _current = _dictionary._entries[_index++].key;
                    return true;
                }

                public void Reset()
                {
                    _index = 0;
                    _count = _dictionary._count;
                }
            }
        }

        public struct ValueCollection : ICollection<TValue>, IReadOnlyCollection<TValue>
        {
            private readonly DictionarySlim<TKey, TValue> _dictionary;

            internal ValueCollection(DictionarySlim<TKey, TValue> dictionary)
            {
                _dictionary = dictionary;
            }

            public int Count => _dictionary._count;

            bool ICollection<TValue>.IsReadOnly => true;

            void ICollection<TValue>.Add(TValue item) => throw new NotSupportedException(Strings.ReadOnly_Modification);

            void ICollection<TValue>.Clear() => throw new NotSupportedException(Strings.ReadOnly_Modification);

            bool ICollection<TValue>.Contains(TValue item) => throw new NotSupportedException(); // performance antipattern

            bool ICollection<TValue>.Remove(TValue item) => throw new NotSupportedException(Strings.ReadOnly_Modification);

            public void CopyTo(TValue[] array, int index)
            {
                Entry[] entries = _dictionary._entries;
                int i = 0;
                int count = _dictionary._count;
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
                private TValue _current;

                internal Enumerator(DictionarySlim<TKey, TValue> dictionary)
                {
                    _dictionary = dictionary;
                    _index = 0;
                    _count = _dictionary._count;
                    _current = default;
                }

                public TValue Current => _current;

                object IEnumerator.Current => _current;

                public void Dispose() { }

                public bool MoveNext()
                {
                    if (_count == 0)
                    {
                        _current = default;
                        return false;
                    }

                    _count--;

                    while (_dictionary._entries[_index].next < -1)
                        _index++;

                    _current = _dictionary._entries[_index++].value;
                    return true;
                }

                public void Reset()
                {
                    _index = 0;
                    _count = _dictionary._count;
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
