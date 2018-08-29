// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace Microsoft.Collections.Extensions
{
    internal enum InsertionBehavior
    {
        None = 0,
        OverwriteExisting = 1,
        ThrowOnExisting = 2
    }

    /// <summary>
    /// Represents an ordered collection of keys and values with the same performance as <see cref="Dictionary{TKey, TValue}"/> with O(1) lookups and adds but with O(n) inserts and removes.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys in the dictionary.</typeparam>
    /// <typeparam name="TValue">The type of the values in the dictionary.</typeparam>
    [DebuggerTypeProxy(typeof(IDictionaryDebugView<,>))]
    [DebuggerDisplay("Count = {Count}")]
    public partial class OrderedDictionary<TKey, TValue> : IDictionary<TKey, TValue>, IReadOnlyDictionary<TKey, TValue>, IList<KeyValuePair<TKey, TValue>>, IReadOnlyList<KeyValuePair<TKey, TValue>>
    {
        private struct Entry
        {
            public uint HashCode;
            public TKey Key;
            public TValue Value;
            public int Next; // the index of the next item in the same bucket, -1 if last
        }

        // We want to initialize without allocating arrays. We also want to avoid null checks.
        // Array.Empty would give divide by zero in modulo operation. So we use static one element arrays.
        // The first add will cause a resize replacing these with real arrays of three elements.
        // Arrays are wrapped in a class to avoid being duplicated for each <TKey, TValue>
        private static readonly Entry[] InitialEntries = new Entry[1];
        // 1-based index into _entries; 0 means empty
        private int[] _buckets = HashHelpers.SizeOneIntArray;
        // remains contiguous and maintains order
        private Entry[] _entries = InitialEntries;
        private int _count;
        private int _version;
        // is null when comparer is EqualityComparer<TKey>.Default so that the GetHashCode method is used explicitly on the object
        private readonly IEqualityComparer<TKey> _comparer;
        private KeyCollection _keys;
        private ValueCollection _values;

        public int Count => _count;

        public IEqualityComparer<TKey> Comparer => _comparer ?? EqualityComparer<TKey>.Default;

        public KeyCollection Keys => _keys ?? (_keys = new KeyCollection(this));

        public ValueCollection Values => _values ?? (_values = new ValueCollection(this));

        public TValue this[TKey key]
        {
            get => GetValue(key);
            set => SetValue(key, value);
        }

        public TValue this[int index]
        {
            get => GetAt(index);
            set => SetAt(index, value);
        }

        public OrderedDictionary()
            : this(0, null)
        {
        }

        public OrderedDictionary(int capacity)
            : this(capacity, null)
        {
        }

        public OrderedDictionary(IEqualityComparer<TKey> comparer)
            : this(0, comparer)
        {
        }

        public OrderedDictionary(int capacity, IEqualityComparer<TKey> comparer)
        {
            if (capacity < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(capacity));
            }
            if (capacity > 0)
            {
                int newSize = HashHelpers.GetPrime(capacity);
                _buckets = new int[newSize];
                _entries = new Entry[newSize];
            }

            if (comparer != EqualityComparer<TKey>.Default)
            {
                _comparer = comparer;
            }
        }

        public OrderedDictionary(IEnumerable<KeyValuePair<TKey, TValue>> collection)
            : this(collection, null)
        {
        }

        public OrderedDictionary(IEnumerable<KeyValuePair<TKey, TValue>> collection, IEqualityComparer<TKey> comparer)
            : this((collection as ICollection<KeyValuePair<TKey, TValue>>)?.Count ?? 0, comparer)
        {
            if (collection == null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            foreach (KeyValuePair<TKey, TValue> pair in collection)
            {
                Add(pair.Key, pair.Value);
            }
        }

        public void Add(TKey key, TValue value) => TryInsert(null, key, value, InsertionBehavior.ThrowOnExisting);

        public void Clear()
        {
            if (_count > 0)
            {
                Array.Clear(_buckets, 0, _buckets.Length);
                Array.Clear(_entries, 0, _count);
                _count = 0;
            }
        }

        public bool ContainsKey(TKey key) => IndexOf(key) >= 0;

        public int EnsureCapacity(int capacity)
        {
            if (capacity < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(capacity));
            }

            if (_entries.Length >= capacity)
            {
                return _entries.Length;
            }
            int newSize = HashHelpers.GetPrime(capacity);
            Resize(newSize);
            ++_version;
            return newSize;
        }

        public TValue GetAt(int index) => GetAt(index, out _);

        public TValue GetAt(int index, out TKey key)
        {
            if ((uint)index >= (uint)Count)
            {
                throw new ArgumentOutOfRangeException(nameof(index), Strings.ArgumentOutOfRange_Index);
            }

            Entry entry = _entries[index];
            key = entry.Key;
            return entry.Value;
        }

        public Enumerator GetEnumerator() => new Enumerator(this);

        public TValue GetOrAdd(TKey key, TValue value) => GetOrAdd(key, () => value);

        public TValue GetOrAdd(TKey key, Func<TValue> valueFactory)
        {
            if (valueFactory == null)
            {
                throw new ArgumentNullException(nameof(valueFactory));
            }

            int index = IndexOf(key, out uint hashCode);
            TValue value;
            if (index < 0)
            {
                value = valueFactory();
                AddInternal(null, key, value, hashCode);
            }
            else
            {
                value = _entries[index].Value;
            }
            return value;
        }

        public TValue GetValue(TKey key)
        {
            int index = IndexOf(key);
            if (index < 0)
            {
                throw new KeyNotFoundException(string.Format(Strings.Arg_KeyNotFoundWithKey, key.ToString()));
            }
            return _entries[index].Value;
        }

        public int IndexOf(TKey key) => IndexOf(key, out _);

        public void Insert(int index, TKey key, TValue value)
        {
            if ((uint)index > (uint)Count)
            {
                throw new ArgumentOutOfRangeException(nameof(index), Strings.ArgumentOutOfRange_Index);
            }

            TryInsert(index, key, value, InsertionBehavior.ThrowOnExisting);
        }

        public void Move(int fromIndex, int toIndex)
        {
            if ((uint)fromIndex >= (uint)Count)
            {
                throw new ArgumentOutOfRangeException(nameof(fromIndex), Strings.ArgumentOutOfRange_Index);
            }
            if ((uint)toIndex >= (uint)Count)
            {
                throw new ArgumentOutOfRangeException(nameof(toIndex), Strings.ArgumentOutOfRange_Index);
            }

            if (fromIndex == toIndex)
            {
                return;
            }

            Entry[] entries = _entries;
            Entry temp = entries[fromIndex];
            RemoveEntryFromBucket(fromIndex);
            int direction = fromIndex < toIndex ? 1 : -1;
            for (int i = fromIndex; i != toIndex; i += direction)
            {
                entries[i] = entries[i + direction];
                UpdateBucketIndex(i + direction, -direction);
            }
            AddEntryToBucket(ref temp, toIndex, _buckets);
            entries[toIndex] = temp;
            ++_version;
        }

        public void MoveRange(int fromIndex, int toIndex, int count)
        {
            if (count == 1)
            {
                Move(fromIndex, toIndex);
                return;
            }

            if ((uint)fromIndex >= (uint)Count)
            {
                throw new ArgumentOutOfRangeException(nameof(fromIndex), Strings.ArgumentOutOfRange_Index);
            }
            if ((uint)toIndex >= (uint)Count)
            {
                throw new ArgumentOutOfRangeException(nameof(toIndex), Strings.ArgumentOutOfRange_Index);
            }
            if (count < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(count), Strings.ArgumentOutOfRange_NeedNonNegNum);
            }
            if (fromIndex + count > Count)
            {
                throw new ArgumentException(Strings.Argument_InvalidOffLen);
            }
            if (toIndex + count > Count)
            {
                throw new ArgumentException(Strings.Argument_InvalidOffLen);
            }

            if (fromIndex == toIndex || count == 0)
            {
                return;
            }

            Entry[] entries = _entries;
            // Make a copy of the entries to move. Consider using ArrayPool instead to avoid allocations?
            Entry[] entriesToMove = new Entry[count];
            for (int i = 0; i < count; ++i)
            {
                entriesToMove[i] = entries[fromIndex + i];
                RemoveEntryFromBucket(fromIndex + i);
            }

            // Move entries in between
            int direction = 1;
            int amount = count;
            int start = fromIndex;
            int end = toIndex;
            if (fromIndex > toIndex)
            {
                direction = -1;
                amount = -count;
                start = fromIndex + count - 1;
                end = toIndex + count - 1;
            }
            for (int i = start; i != end; i += direction)
            {
                entries[i] = entries[i + amount];
                UpdateBucketIndex(i + amount, -amount);
            }

            int[] buckets = _buckets;
            // Copy entries to destination
            for (int i = 0; i < count; ++i)
            {
                Entry temp = entriesToMove[i];
                AddEntryToBucket(ref temp, toIndex + i, buckets);
                entries[toIndex + i] = temp;
            }
            ++_version;
        }

        public bool Remove(TKey key) => Remove(key, out _);

        public bool Remove(TKey key, out TValue value)
        {
            int index = IndexOf(key);
            if (index >= 0)
            {
                value = _entries[index].Value;
                RemoveAt(index);
                return true;
            }
            value = default;
            return false;
        }

        public void RemoveAt(int index)
        {
            int count = Count;
            if ((uint)index >= (uint)count)
            {
                throw new ArgumentOutOfRangeException(nameof(index), Strings.ArgumentOutOfRange_Index);
            }

            // Remove the entry from the bucket
            RemoveEntryFromBucket(index);

            // Decrement the indices > index
            for (int i = index + 1; i < count; ++i)
            {
                UpdateBucketIndex(i, -1);
            }
            Entry[] entries = _entries;
            Array.Copy(entries, index + 1, entries, index, count - index - 1);
            --_count;
            entries[Count] = default;
            ++_version;
        }

        public void SetAt(int index, TValue value)
        {
            if ((uint)index >= (uint)Count)
            {
                throw new ArgumentOutOfRangeException(nameof(index), Strings.ArgumentOutOfRange_Index);
            }

            _entries[index].Value = value;
        }

        public void SetAt(int index, TKey key, TValue value)
        {
            if ((uint)index >= (uint)Count)
            {
                throw new ArgumentOutOfRangeException(nameof(index), Strings.ArgumentOutOfRange_Index);
            }

            int foundIndex = IndexOf(key, out uint hashCode);
            // key does not exist in dictionary thus replace entry at index
            if (foundIndex < 0)
            {
                RemoveEntryFromBucket(index);
                Entry entry = new Entry { HashCode = hashCode, Key = key, Value = value };
                AddEntryToBucket(ref entry, index, _buckets);
                _entries[index] = entry;
                ++_version;
            }
            // key already exists in dictionary at the specified index thus just replace the key and value as hashCode remains the same
            else if (foundIndex == index)
            {
                ref Entry entry = ref _entries[index];
                entry.Key = key;
                entry.Value = value;
            }
            // key already exists in dictionary but not at the specified index thus throw exception as this method shouldn't affect the indices of other entries
            else
            {
                throw new ArgumentException(string.Format(Strings.Argument_AddingDuplicateWithKey, key.ToString()));
            }
        }

        public void SetValue(TKey key, TValue value) => TryInsert(null, key, value, InsertionBehavior.OverwriteExisting);

        public void TrimExcess() => TrimExcess(Count);

        public void TrimExcess(int capacity)
        {
            if (capacity < Count)
            {
                throw new ArgumentOutOfRangeException(nameof(capacity));
            }

            int newSize = HashHelpers.GetPrime(capacity);
            if (newSize < _entries.Length)
            {
                Resize(newSize);
                ++_version;
            }
        }

        public bool TryAdd(TKey key, TValue value) => TryInsert(null, key, value, InsertionBehavior.None);

        public bool TryGetValue(TKey key, out TValue value)
        {
            int index = IndexOf(key);
            if (index >= 0)
            {
                value = _entries[index].Value;
                return true;
            }
            value = default;
            return false;
        }

        #region Explicit Interface Implementation
        KeyValuePair<TKey, TValue> IList<KeyValuePair<TKey, TValue>>.this[int index]
        {
            get
            {
                TValue value = GetAt(index, out TKey key);
                return new KeyValuePair<TKey, TValue>(key, value);
            }
            set => SetAt(index, value.Key, value.Value);
        }

        KeyValuePair<TKey, TValue> IReadOnlyList<KeyValuePair<TKey, TValue>>.this[int index] => ((IList<KeyValuePair<TKey, TValue>>)this)[index];

        ICollection<TKey> IDictionary<TKey, TValue>.Keys => Keys;

        ICollection<TValue> IDictionary<TKey, TValue>.Values => Values;

        IEnumerable<TKey> IReadOnlyDictionary<TKey, TValue>.Keys => Keys;

        IEnumerable<TValue> IReadOnlyDictionary<TKey, TValue>.Values => Values;

        bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly => false;

        IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator() => GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item) => Add(item.Key, item.Value);

        bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> item) => TryGetValue(item.Key, out TValue value) && EqualityComparer<TValue>.Default.Equals(value, item.Value);

        void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            if (array == null)
            {
                throw new ArgumentNullException(nameof(array));
            }
            if ((uint)arrayIndex > (uint)array.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(arrayIndex), Strings.ArgumentOutOfRange_NeedNonNegNum);
            }
            int count = Count;
            if (array.Length - arrayIndex < count)
            {
                throw new ArgumentException(Strings.Arg_ArrayPlusOffTooSmall);
            }

            Entry[] entries = _entries;
            for (int i = 0; i < count; ++i)
            {
                Entry entry = entries[i];
                array[i + arrayIndex] = new KeyValuePair<TKey, TValue>(entry.Key, entry.Value);
            }
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item)
        {
            int index = IndexOf(item.Key);
            if (index >= 0 && EqualityComparer<TValue>.Default.Equals(_entries[index].Value, item.Value))
            {
                RemoveAt(index);
                return true;
            }
            return false;
        }

        int IList<KeyValuePair<TKey, TValue>>.IndexOf(KeyValuePair<TKey, TValue> item)
        {
            int index = IndexOf(item.Key);
            if (index >= 0 && !EqualityComparer<TValue>.Default.Equals(_entries[index].Value, item.Value))
            {
                index = -1;
            }
            return index;
        }

        void IList<KeyValuePair<TKey, TValue>>.Insert(int index, KeyValuePair<TKey, TValue> item) => Insert(index, item.Key, item.Value);
        #endregion

        private Entry[] Resize(int newSize)
        {
            int[] newBuckets = new int[newSize];
            Entry[] newEntries = new Entry[newSize];

            int count = Count;
            Array.Copy(_entries, newEntries, count);
            for (int i = 0; i < count; ++i)
            {
                AddEntryToBucket(ref newEntries[i], i, newBuckets);
            }

            _buckets = newBuckets;
            _entries = newEntries;
            return newEntries;
        }

        private int IndexOf(TKey key, out uint hashCode)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            IEqualityComparer<TKey> comparer = _comparer;
            hashCode = (uint)(comparer?.GetHashCode(key) ?? key.GetHashCode());
            int index = _buckets[(int)(hashCode % (uint)_buckets.Length)] - 1;
            if (index >= 0)
            {
                if (comparer == null)
                {
                    comparer = EqualityComparer<TKey>.Default;
                }
                Entry[] entries = _entries;
                int collisionCount = 0;
                do
                {
                    Entry entry = entries[index];
                    if (entry.HashCode == hashCode && comparer.Equals(entry.Key, key))
                    {
                        break;
                    }
                    index = entry.Next;
                    if (collisionCount >= entries.Length)
                    {
                        // The chain of entries forms a loop; which means a concurrent update has happened.
                        // Break out of the loop and throw, rather than looping forever.
                        throw new InvalidOperationException(Strings.InvalidOperation_ConcurrentOperationsNotSupported);
                    }
                    ++collisionCount;
                } while (index >= 0);
            }
            return index;
        }

        private bool TryInsert(int? index, TKey key, TValue value, InsertionBehavior behavior)
        {
            int i = IndexOf(key, out uint hashCode);
            if (i >= 0)
            {
                switch (behavior)
                {
                    case InsertionBehavior.OverwriteExisting:
                        _entries[i].Value = value;
                        return true;
                    case InsertionBehavior.ThrowOnExisting:
                        throw new ArgumentException(string.Format(Strings.Argument_AddingDuplicateWithKey, key.ToString()));
                    default:
                        return false;
                }
            }

            AddInternal(index, key, value, hashCode);
            return true;
        }

        private int AddInternal(int? index, TKey key, TValue value, uint hashCode)
        {
            Entry[] entries = _entries;
            // Check if resize is needed
            int count = Count;
            if (entries.Length == count || entries.Length == 1)
            {
                entries = Resize(HashHelpers.ExpandPrime(entries.Length));
            }

            // Increment indices >= index;
            int actualIndex = index ?? count;
            for (int i = actualIndex; i < count; ++i)
            {
                UpdateBucketIndex(i, 1);
            }
            Array.Copy(entries, actualIndex, entries, actualIndex + 1, count - actualIndex);

            Entry entry = new Entry { HashCode = hashCode, Key = key, Value = value };
            AddEntryToBucket(ref entry, actualIndex, _buckets);
            entries[actualIndex] = entry;
            ++_count;
            ++_version;
            return actualIndex;
        }

        // Returns the index of the next entry in the bucket
        private void AddEntryToBucket(ref Entry entry, int entryIndex, int[] buckets)
        {
            ref int b = ref buckets[(int)(entry.HashCode % (uint)buckets.Length)];
            entry.Next = b - 1;
            b = entryIndex + 1;
        }

        private void RemoveEntryFromBucket(int entryIndex)
        {
            Entry[] entries = _entries;
            Entry entry = entries[entryIndex];
            ref int b = ref _buckets[(int)(entry.HashCode % (uint)_buckets.Length)];
            // Bucket was pointing to removed entry. Update it to point to the next in the chain
            if (b == entryIndex + 1)
            {
                b = entry.Next + 1;
            }
            else
            {
                // Start at the entry the bucket points to, and walk the chain until we find the entry with the index we want to remove, then fix the chain
                int i = b - 1;
                int collisionCount = 0;
                while (true)
                {
                    ref Entry e = ref entries[i];
                    if (e.Next == entryIndex)
                    {
                        e.Next = entry.Next;
                        return;
                    }
                    i = e.Next;
                    if (collisionCount >= entries.Length)
                    {
                        // The chain of entries forms a loop; which means a concurrent update has happened.
                        // Break out of the loop and throw, rather than looping forever.
                        throw new InvalidOperationException(Strings.InvalidOperation_ConcurrentOperationsNotSupported);
                    }
                    ++collisionCount;
                }
            }
        }

        private void UpdateBucketIndex(int entryIndex, int incrementAmount)
        {
            Entry[] entries = _entries;
            Entry entry = entries[entryIndex];
            ref int b = ref _buckets[(int)(entry.HashCode % (uint)_buckets.Length)];
            // Bucket was pointing to entry. Increment the index by incrementAmount.
            if (b == entryIndex + 1)
            {
                b += incrementAmount;
            }
            else
            {
                // Start at the entry the bucket points to, and walk the chain until we find the entry with the index we want to increment.
                int i = b - 1;
                int collisionCount = 0;
                while (true)
                {
                    ref Entry e = ref entries[i];
                    if (e.Next == entryIndex)
                    {
                        e.Next += incrementAmount;
                        return;
                    }
                    i = e.Next;
                    if (collisionCount >= entries.Length)
                    {
                        // The chain of entries forms a loop; which means a concurrent update has happened.
                        // Break out of the loop and throw, rather than looping forever.
                        throw new InvalidOperationException(Strings.InvalidOperation_ConcurrentOperationsNotSupported);
                    }
                    ++collisionCount;
                }
            }
        }

        public struct Enumerator : IEnumerator<KeyValuePair<TKey, TValue>>
        {
            private readonly OrderedDictionary<TKey, TValue> _orderedDictionary;
            private readonly int _version;
            private int _index;
            private KeyValuePair<TKey, TValue> _current;

            public KeyValuePair<TKey, TValue> Current => _current;

            object IEnumerator.Current => _current;

            internal Enumerator(OrderedDictionary<TKey, TValue> orderedDictionary)
            {
                _orderedDictionary = orderedDictionary;
                _version = orderedDictionary._version;
                _index = 0;
            }

            public void Dispose()
            {
            }

            public bool MoveNext()
            {
                if (_version != _orderedDictionary._version)
                {
                    throw new InvalidOperationException(Strings.InvalidOperation_EnumFailedVersion);
                }

                if (_index < _orderedDictionary.Count)
                {
                    Entry entry = _orderedDictionary._entries[_index];
                    _current = new KeyValuePair<TKey, TValue>(entry.Key, entry.Value);
                    ++_index;
                    return true;
                }
                _current = default;
                return false;
            }

            void IEnumerator.Reset()
            {
                if (_version != _orderedDictionary._version)
                {
                    throw new InvalidOperationException(Strings.InvalidOperation_EnumFailedVersion);
                }

                _index = 0;
                _current = default;
            }
        }
    }
}
