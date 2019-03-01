using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace Microsoft.Collections.Extensions
{
    // The shared fields between a BidirectionalDictionary and its reverse view, updates in one will reflect in the other as they're shared through this class
    internal sealed class BidirectionalDictionaryShared
    {
        // The number of initialized items, either in collection or in free list
        public int Count;
        public int Version;
        public int FreeList = -1;
        public int FreeCount;
    }

    /// <summary>
    /// Represents a bi-directional collection of first and second keys.
    /// </summary>
    /// <typeparam name="TFirst">The type of the first keys in the dictionary.</typeparam>
    /// <typeparam name="TSecond">The type of the second keys in the dictionary.</typeparam>
    [DebuggerTypeProxy(typeof(IDictionaryDebugView<,>))]
    [DebuggerDisplay("Count = {Count}")]
    public partial class BidirectionalDictionary<TFirst, TSecond> : IDictionary<TFirst, TSecond>, IReadOnlyDictionary<TFirst, TSecond>
    {
        internal struct Entry
        {
            public uint HashCode;
            public TFirst Key;
            public int Next; // the index of the next item in the same bucket, -1 if last
        }

        // We want to initialize without allocating arrays. We also want to avoid null checks.
        // Array.Empty would give divide by zero in modulo operation. So we use static one element arrays.
        // The first add will cause a resize replacing these with real arrays of three elements.
        // Arrays are wrapped in a class to avoid being duplicated for each <TKey, TValue>
        private static readonly Entry[] s_initialEntries = new Entry[1];

        private int[] _buckets;
        private Entry[] _entries;
        private readonly IEqualityComparer<TFirst> _comparer;
        private readonly BidirectionalDictionaryShared _shared;
        private readonly BidirectionalDictionary<TSecond, TFirst> _reverse;
        private KeyCollection _firstKeys;

        /// <summary>
        /// Gets the number of key pairs contained in the <see cref="BidirectionalDictionary{TFirst, TSecond}" />.
        /// </summary>
        /// <returns>The number of key pairs contained in the <see cref="BidirectionalDictionary{TFirst, TSecond}" />.</returns>
        public int Count => _shared.Count - _shared.FreeCount;

        /// <summary>
        /// Gets the <see cref="IEqualityComparer{TFirst}" /> that is used to determine equality of first keys for the dictionary.
        /// </summary>
        /// <returns>The <see cref="IEqualityComparer{TFirst}" /> generic interface implementation that is used to determine equality of first keys for the current <see cref="BidirectionalDictionary{TFirst, TSecond}" /> and to provide hash values for the first keys.</returns>
        public IEqualityComparer<TFirst> FirstKeyComparer => _comparer ?? EqualityComparer<TFirst>.Default;

        /// <summary>
        /// Gets the <see cref="IEqualityComparer{TSecond}" /> that is used to determine equality of second keys for the dictionary.
        /// </summary>
        /// <returns>The <see cref="IEqualityComparer{TSecond}" /> generic interface implementation that is used to determine equality of second keys for the current <see cref="BidirectionalDictionary{TFirst, TSecond}" /> and to provide hash values for the second keys.</returns>
        public IEqualityComparer<TSecond> SecondKeyComparer => _reverse.FirstKeyComparer;

        /// <summary>
        /// Gets a collection containing the first keys in the <see cref="BidirectionalDictionary{TFirst, TSecond}" />.
        /// </summary>
        /// <returns>An <see cref="BidirectionalDictionary{TFirst, TSecond}.KeyCollection" /> containing the first keys in the <see cref="BidirectionalDictionary{TFirst, TSecond}" />.</returns>
        public KeyCollection FirstKeys => _firstKeys ?? (_firstKeys = new KeyCollection(this));

        /// <summary>
        /// Gets a collection containing the second keys in the <see cref="BidirectionalDictionary{TFirst, TSecond}" />.
        /// </summary>
        /// <returns>An <see cref="BidirectionalDictionary{TFirst, TSecond}.KeyCollection" /> containing the second keys in the <see cref="BidirectionalDictionary{TFirst, TSecond}" />.</returns>
        public BidirectionalDictionary<TSecond, TFirst>.KeyCollection SecondKeys => _reverse.FirstKeys;

        /// <summary>
        /// Gets the reverse view of the <see cref="BidirectionalDictionary{TFirst, TSecond}" /> so that the first key becomes the second key and the second key becomes the first key.
        /// </summary>
        public BidirectionalDictionary<TSecond, TFirst> Reverse => _reverse;

        /// <summary>
        /// Gets or sets the second key associated with the specified first key as an O(1) operation.
        /// </summary>
        /// <param name="firstKey">The first key of the key pair to get or set.</param>
        /// <returns>The second key associated with the specified first key. If the specified first key is not found, a get operation throws a <see cref="KeyNotFoundException" />, and a set operation creates a new element with the specified first key.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="firstKey" /> or value is null.</exception>
        /// <exception cref="ArgumentException">The property is set and value already exists in the collection.</exception>
        /// <exception cref="KeyNotFoundException">The property is retrieved and <paramref name="firstKey" /> does not exist in the collection.</exception>
        public TSecond this[TFirst firstKey]
        {
            get
            {
                int index = IndexOf(firstKey, out _);
                if (index < 0)
                {
                    throw new KeyNotFoundException(string.Format(Strings.Arg_KeyNotFoundWithKey, firstKey.ToString()));
                }
                return _reverse._entries[index].Key;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value));
                }
                TryInsert(firstKey, value, returnFalseOnExisting: false);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BidirectionalDictionary{TFirst, TSecond}" /> class that is empty, has the default initial capacity, and uses the default equality comparer for the key types.
        /// </summary>
        public BidirectionalDictionary()
            : this(0, null, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BidirectionalDictionary{TFirst, TSecond}" /> class that is empty, has the specified initial capacity, and uses the default equality comparer for the key types.
        /// </summary>
        /// <param name="capacity">The initial number of elements that the <see cref="BidirectionalDictionary{TFirst, TSecond}" /> can contain.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="capacity" /> is less than 0.</exception>
        public BidirectionalDictionary(int capacity)
            : this(capacity, null, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BidirectionalDictionary{TFirst, TSecond}" /> class that is empty, has the default initial capacity, and uses the specified <see cref="IEqualityComparer{T}" />s.
        /// </summary>
        /// <param name="firstKeyComparer">The <see cref="IEqualityComparer{TFirst}" /> implementation to use when comparing first keys, or null to use the default <see cref="EqualityComparer{TFirst}" /> for the type of the first key.</param>
        /// <param name="secondKeyComparer">The <see cref="IEqualityComparer{TSecond}" /> implementation to use when comparing second keys, or null to use the default <see cref="EqualityComparer{TSecond}" /> for the type of the second key.</param>
        public BidirectionalDictionary(IEqualityComparer<TFirst> firstKeyComparer, IEqualityComparer<TSecond> secondKeyComparer)
            : this(0, firstKeyComparer, secondKeyComparer)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BidirectionalDictionary{TFirst, TSecond}" /> class that is empty, has the specified initial capacity, and uses the specified <see cref="IEqualityComparer{T}" />s.
        /// </summary>
        /// <param name="capacity">The initial number of elements that the <see cref="BidirectionalDictionary{TFirst, TSecond}" /> can contain.</param>
        /// <param name="firstKeyComparer">The <see cref="IEqualityComparer{TFirst}" /> implementation to use when comparing first keys, or null to use the default <see cref="EqualityComparer{TFirst}" /> for the type of the first key.</param>
        /// <param name="secondKeyComparer">The <see cref="IEqualityComparer{TSecond}" /> implementation to use when comparing second keys, or null to use the default <see cref="EqualityComparer{TSecond}" /> for the type of the second key.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="capacity" /> is less than 0.</exception>
        public BidirectionalDictionary(int capacity, IEqualityComparer<TFirst> firstKeyComparer, IEqualityComparer<TSecond> secondKeyComparer)
            : this(GetSize(capacity), firstKeyComparer, secondKeyComparer, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BidirectionalDictionary{TFirst, TSecond}" /> class that contains elements copied from the specified <see cref="IEnumerable{KeyValuePair{TFirst, TSecond}}" /> and uses the default equality comparer for the key type.
        /// </summary>
        /// <param name="collection">The <see cref="IEnumerable{KeyValuePair{TFirst, TSecond}}" /> whose elements are copied to the new <see cref="BidirectionalDictionary{TFirst, TSecond}" />.</param>
        /// <exception cref="ArgumentNullException"><paramref name="collection" /> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="collection" /> contains one or more duplicate keys.</exception>
        public BidirectionalDictionary(IEnumerable<KeyValuePair<TFirst, TSecond>> collection)
            : this(collection, null, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BidirectionalDictionary{TFirst, TSecond}" /> class that contains elements copied from the specified <see cref="IEnumerable{KeyValuePair{TFirst, TSecond}}" /> and uses the specified <see cref="IEqualityComparer{T}" />s.
        /// </summary>
        /// <param name="collection">The <see cref="IEnumerable{KeyValuePair{TFirst, TSecond}}" /> whose elements are copied to the new <see cref="BidirectionalDictionary{TFirst, TSecond}" />.</param>
        /// <param name="firstKeyComparer">The <see cref="IEqualityComparer{TFirst}" /> implementation to use when comparing first keys, or null to use the default <see cref="EqualityComparer{TFirst}" /> for the type of the first key.</param>
        /// <param name="secondKeyComparer">The <see cref="IEqualityComparer{TSecond}" /> implementation to use when comparing second keys, or null to use the default <see cref="EqualityComparer{TSecond}" /> for the type of the second key.</param>
        /// <exception cref="ArgumentNullException"><paramref name="collection" /> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="collection" /> contains one or more duplicate keys.</exception>
        public BidirectionalDictionary(IEnumerable<KeyValuePair<TFirst, TSecond>> collection, IEqualityComparer<TFirst> firstKeyComparer, IEqualityComparer<TSecond> secondKeyComparer)
            : this((collection as ICollection<KeyValuePair<TFirst, TSecond>>)?.Count ?? 0, firstKeyComparer, secondKeyComparer)
        {
            if (collection == null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            foreach (KeyValuePair<TFirst, TSecond> pair in collection)
            {
                ((ICollection<KeyValuePair<TFirst, TSecond>>)this).Add(pair);
            }
        }

        private BidirectionalDictionary(int size, IEqualityComparer<TFirst> firstKeyComparer, IEqualityComparer<TSecond> secondKeyComparer, BidirectionalDictionary<TSecond, TFirst> reverse)
        {
            if (size > 0)
            {
                _buckets = new int[size];
                _entries = new Entry[size];
            }
            else
            {
                _buckets = HashHelpers.SizeOneIntArray;
                _entries = s_initialEntries;
            }
            if (firstKeyComparer != EqualityComparer<TFirst>.Default)
            {
                _comparer = firstKeyComparer;
            }
            if (reverse == null)
            {
                _shared = new BidirectionalDictionaryShared();
                _reverse = new BidirectionalDictionary<TSecond, TFirst>(size, secondKeyComparer, firstKeyComparer, this);
            }
            else
            {
                _shared = reverse._shared;
                _reverse = reverse;
            }
        }

        private static int GetSize(int capacity)
        {
            if (capacity < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(capacity));
            }

            return capacity > 0 ? HashHelpers.GetPrime(capacity) : 0;
        }

        /// <summary>
        /// Adds the specified key pair to the dictionary as an O(1) operation.
        /// </summary>
        /// <param name="firstKey">The first key of the pair to add.</param>
        /// <param name="secondKey">The second key of the pair to add.</param>
        /// <exception cref="ArgumentNullException"><paramref name="firstKey" /> or <paramref name="secondKey"/> is null.</exception>
        /// <exception cref="ArgumentException">A pair with the same first or second key already exists in the <see cref="BidirectionalDictionary{TFirst, TSecond}" />.</exception>
        public bool Add(TFirst firstKey, TSecond secondKey)
        {
            if (secondKey == null)
            {
                throw new ArgumentNullException(nameof(secondKey));
            }
            return TryInsert(firstKey, secondKey, returnFalseOnExisting: true);
        }

        /// <summary>
        /// Removes all key pairs from the <see cref="BidirectionalDictionary{TFirst, TSecond}" />.
        /// </summary>
        public void Clear()
        {
            BidirectionalDictionaryShared shared = _shared;
            int count = shared.Count;
            Array.Clear(_buckets, 0, _buckets.Length);
            Array.Clear(_entries, 0, count);
            Array.Clear(_reverse._buckets, 0, _reverse._buckets.Length);
            Array.Clear(_reverse._entries, 0, count);
            shared.Count = 0;
            shared.FreeList = -1;
            shared.FreeCount = 0;
            ++shared.Version;
        }

        /// <summary>
        /// Determines whether the <see cref="BidirectionalDictionary{TFirst, TSecond}" /> contains the specified first key as an O(1) operation.
        /// </summary>
        /// <param name="firstKey">The first key to locate in the <see cref="BidirectionalDictionary{TFirst, TSecond}" />.</param>
        /// <returns>true if the <see cref="BidirectionalDictionary{TFirst, TSecond}" /> contains a key pair with the specified first key; otherwise, false.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="firstKey" /> is null.</exception>
        public bool ContainsKey(TFirst firstKey) => IndexOf(firstKey, out _) >= 0;

        /// <summary>
        /// Resizes the internal data structure if necessary to ensure no additional resizing to support the specified capacity.
        /// </summary>
        /// <param name="capacity">The number of elements that the <see cref="BidirectionalDictionary{TFirst, TSecond}" /> must be able to contain.</param>
        /// <returns>The capacity of the <see cref="BidirectionalDictionary{TFirst, TSecond}" />.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="capacity" /> is less than 0.</exception>
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
            BidirectionalDictionaryShared shared = _shared;
            int count = shared.Count;
            Resize(newSize, count);
            _reverse.Resize(newSize, count);
            ++shared.Version;
            return newSize;
        }

        /// <summary>
        /// Returns an enumerator that iterates through the <see cref="BidirectionalDictionary{TFirst, TSecond}" />.
        /// </summary>
        /// <returns>An <see cref="BidirectionalDictionary{TFirst, TSecond}.Enumerator" /> structure for the <see cref="BidirectionalDictionary{TFirst, TSecond}" />.</returns>
        public Enumerator GetEnumerator() => new Enumerator(this);

        /// <summary>
        /// Removes the key pair with the specified first key from the <see cref="BidirectionalDictionary{TFirst, TSecond}" /> as an O(1) operation.
        /// </summary>
        /// <param name="firstKey">The first key of the key pair to remove.</param>
        /// <returns>true if the key pair is successfully found and removed; otherwise, false. This method returns false if <paramref name="firstKey" /> is not found in the <see cref="BidirectionalDictionary{TFirst, TSecond}" />.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="firstKey" /> is null.</exception>
        public bool Remove(TFirst firstKey) => Remove(firstKey, out _);

        /// <summary>
        /// Removes the key pair with the specified first key from the <see cref="BidirectionalDictionary{TFirst, TSecond}" /> and returns the second key as an O(1) operation.
        /// </summary>
        /// <param name="firstKey">The first key of the key pair to remove.</param>
        /// <param name="secondKey">When this method returns, contains the second key associated with the specified first key, if the first key is found; otherwise, the default value for the type of the <paramref name="secondKey" /> parameter. This parameter is passed uninitialized.</param>
        /// <returns>true if the key pair is successfully found and removed; otherwise, false. This method returns false if <paramref name="firstKey" /> is not found in the <see cref="BidirectionalDictionary{TFirst, TSecond}" />.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="firstKey" /> is null.</exception>
        public bool Remove(TFirst firstKey, out TSecond secondKey)
        {
            int index = IndexOf(firstKey, out _);
            if (index >= 0)
            {
                ref Entry firstKeyEntry = ref RemoveEntryFromBucket(index);
                firstKeyEntry.Key = default;
                firstKeyEntry.HashCode = default;
                ref BidirectionalDictionary<TSecond, TFirst>.Entry secondKeyEntry = ref _reverse.RemoveEntryFromBucket(index);
                secondKey = secondKeyEntry.Key;
                secondKeyEntry.Key = default;
                secondKeyEntry.HashCode = default;
                BidirectionalDictionaryShared shared = _shared;
                firstKeyEntry.Next = secondKeyEntry.Next = -3 - shared.FreeList;
                shared.FreeList = index;
                ++shared.FreeCount;
                return true;
            }
            secondKey = default;
            return false;
        }

        /// <summary>
        /// Sets the capacity of an <see cref="BidirectionalDictionary{TFirst, TSecond}" /> object to the actual number of elements it contains, rounded up to a nearby, implementation-specific value.
        /// </summary>
        public void TrimExcess() => TrimExcess(Count);

        /// <summary>
        /// Sets the capacity of an <see cref="BidirectionalDictionary{TFirst, TSecond}" /> object to the specified capacity, rounded up to a nearby, implementation-specific value.
        /// </summary>
        /// <param name="capacity">The number of elements that the <see cref="BidirectionalDictionary{TFirst, TSecond}" /> must be able to contain.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="capacity"/> is less than <see cref="BidirectionalDictionary{TFirst, TSecond}.Count" />.</exception>
        public void TrimExcess(int capacity)
        {
            if (capacity < Count)
            {
                throw new ArgumentOutOfRangeException(nameof(capacity));
            }

            int newSize = HashHelpers.GetPrime(capacity);
            if (newSize < _entries.Length)
            {
                BidirectionalDictionaryShared shared = _shared;
                int count = shared.Count;
                Resize(newSize, count, compact: true);
                _reverse.Resize(newSize, count, compact: true);
                shared.Count = Count;
                shared.FreeCount = 0;
                shared.FreeList = -1;
                ++shared.Version;
            }
        }

        /// <summary>
        /// Gets the second key associated with the specified first key as an O(1) operation.
        /// </summary>
        /// <param name="firstKey">The first key of the key pair to get.</param>
        /// <param name="secondKey">When this method returns, contains the value associated with the specified key, if the key is found; otherwise, the default value for the type of the <paramref name="secondKey" /> parameter. This parameter is passed uninitialized.</param>
        /// <returns>true if the <see cref="BidirectionalDictionary{TFirst, TSecond}" /> contains an element with the specified key; otherwise, false.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="firstKey" /> is null.</exception>
        public bool TryGetValue(TFirst firstKey, out TSecond secondKey)
        {
            int index = IndexOf(firstKey, out _);
            if (index >= 0)
            {
                secondKey = _reverse._entries[index].Key;
                return true;
            }
            secondKey = default;
            return false;
        }

        private bool TryInsert(TFirst firstKey, TSecond secondKey, bool returnFalseOnExisting)
        {
            int i = IndexOf(firstKey, out uint firstKeyHashCode);
            if (returnFalseOnExisting && i >= 0)
            {
                return false;
            }
            
            int j = _reverse.IndexOf(secondKey, out uint secondKeyHashCode);
            if (j >= 0)
            {
                if (returnFalseOnExisting)
                {
                    return false;
                }
                if (i == j)
                {
                    // HashCode is the same so just replace the value
                    _reverse._entries[j].Key = secondKey;
                }
                else
                {
                    throw new ArgumentException(string.Format(Strings.Argument_AddingDuplicateWithKey, secondKey.ToString()));
                }
            }
            else if (i >= 0)
            {
                ref BidirectionalDictionary<TSecond, TFirst>.Entry entry = ref _reverse.RemoveEntryFromBucket(i);
                entry.HashCode = secondKeyHashCode;
                entry.Key = secondKey;
                _reverse.AddEntryToBucket(_reverse._buckets, ref entry, i);
            }
            else
            {
                BidirectionalDictionaryShared shared = _shared;
                bool updateFreeList = false;
                int index;
                Entry[] firstKeyEntries = _entries;
                if (shared.FreeCount > 0)
                {
                    index = shared.FreeList;
                    updateFreeList = true;
                    --shared.FreeCount;
                }
                else
                {
                    index = shared.Count;
                    // Check if resize is needed
                    if (firstKeyEntries.Length == index || firstKeyEntries.Length == 1)
                    {
                        int newSize = HashHelpers.ExpandPrime(firstKeyEntries.Length);
                        Resize(newSize, index);
                        _reverse.Resize(newSize, index);
                        firstKeyEntries = _entries;
                    }
                    ++shared.Count;
                }

                ref Entry firstKeyEntry = ref firstKeyEntries[index];
                if (updateFreeList)
                {
                    shared.FreeList = -3 - firstKeyEntry.Next;
                }
                firstKeyEntry.HashCode = firstKeyHashCode;
                firstKeyEntry.Key = firstKey;
                AddEntryToBucket(_buckets, ref firstKeyEntry, index);

                ref BidirectionalDictionary<TSecond, TFirst>.Entry secondKeyEntry = ref _reverse._entries[index];
                secondKeyEntry.HashCode = secondKeyHashCode;
                secondKeyEntry.Key = secondKey;
                _reverse.AddEntryToBucket(_reverse._buckets, ref secondKeyEntry, index);
                ++shared.Version;
            }
            return true;
        }

        private int IndexOf(TFirst firstKey, out uint hashCode)
        {
            if (firstKey == null)
            {
                throw new ArgumentNullException(nameof(firstKey));
            }

            IEqualityComparer<TFirst> comparer = _comparer;
            hashCode = (uint)(comparer?.GetHashCode(firstKey) ?? firstKey.GetHashCode());
            int index = _buckets[(int)(hashCode % (uint)_buckets.Length)] - 1;
            if (index >= 0)
            {
                if (comparer == null)
                {
                    comparer = EqualityComparer<TFirst>.Default;
                }
                int collisionCount = 0;
                Entry[] entries = _entries;
                do
                {
                    Entry entry = entries[index];
                    if (entry.HashCode == hashCode && comparer.Equals(entry.Key, firstKey))
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

        private void Resize(int newSize, int count, bool compact = false)
        {
            int[] newBuckets = new int[newSize];
            Entry[] newEntries = new Entry[newSize];

            Entry[] entries = _entries;
            if (compact)
            {
                int j = 0;
                for (int i = 0; i < count; ++i)
                {
                    Entry entry = entries[i];
                    if (entry.Next >= -1)
                    {
                        newEntries[j++] = entry;
                    }
                }
                count = j;
            }
            else
            {
                Array.Copy(entries, newEntries, count);
            }
            for (int i = 0; i < count; ++i)
            {
                ref Entry entry = ref newEntries[i];
                if (entry.Next >= -1)
                {
                    AddEntryToBucket(newBuckets, ref entry, i);
                }
            }

            _buckets = newBuckets;
            _entries = newEntries;
        }

        private void AddEntryToBucket(int[] buckets, ref Entry entry, int entryIndex)
        {
            ref int b = ref buckets[(int)(entry.HashCode % (uint)buckets.Length)];
            entry.Next = b - 1;
            b = entryIndex + 1;
        }

        private ref Entry RemoveEntryFromBucket(int entryIndex)
        {
            Entry[] entries = _entries;
            ref Entry entry = ref entries[entryIndex];
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
                        break;
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
            return ref entry;
        }

        #region Default Interface Implementation
        ICollection<TFirst> IDictionary<TFirst, TSecond>.Keys => FirstKeys;

        ICollection<TSecond> IDictionary<TFirst, TSecond>.Values => SecondKeys;

        IEnumerable<TFirst> IReadOnlyDictionary<TFirst, TSecond>.Keys => FirstKeys;

        IEnumerable<TSecond> IReadOnlyDictionary<TFirst, TSecond>.Values => SecondKeys;

        bool ICollection<KeyValuePair<TFirst, TSecond>>.IsReadOnly => false;

        void IDictionary<TFirst, TSecond>.Add(TFirst key, TSecond value)
        {
            if (!Add(key, value))
            {
                throw new ArgumentException(string.Format(Strings.Argument_AddingDuplicateWithKey, $"{key.ToString()} or {value.ToString()}"));
            }
        }

        void ICollection<KeyValuePair<TFirst, TSecond>>.Add(KeyValuePair<TFirst, TSecond> item) => ((IDictionary<TFirst, TSecond>)this).Add(item.Key, item.Value);

        bool ICollection<KeyValuePair<TFirst, TSecond>>.Contains(KeyValuePair<TFirst, TSecond> item) => TryGetValue(item.Key, out TSecond value) && SecondKeyComparer.Equals(value, item.Value);

        void ICollection<KeyValuePair<TFirst, TSecond>>.CopyTo(KeyValuePair<TFirst, TSecond>[] array, int arrayIndex)
        {
            if (array == null)
            {
                throw new ArgumentNullException(nameof(array));
            }
            if ((uint)arrayIndex > (uint)array.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(arrayIndex), Strings.ArgumentOutOfRange_NeedNonNegNum);
            }
            if (array.Length - arrayIndex < Count)
            {
                throw new ArgumentException(Strings.Arg_ArrayPlusOffTooSmall);
            }

            int count = _shared.Count;
            Entry[] firstKeyEntries = _entries;
            BidirectionalDictionary<TSecond, TFirst>.Entry[] secondKeyEntries = _reverse._entries;
            int currentArrayIndex = arrayIndex;
            for (int i = 0; i < count; ++i)
            {
                Entry firstKeyEntry = firstKeyEntries[i];
                if (firstKeyEntry.Next >= -1)
                {
                    array[currentArrayIndex++] = new KeyValuePair<TFirst, TSecond>(firstKeyEntry.Key, secondKeyEntries[i].Key);
                }
            }
        }

        bool ICollection<KeyValuePair<TFirst, TSecond>>.Remove(KeyValuePair<TFirst, TSecond> item) => TryGetValue(item.Key, out TSecond value) && SecondKeyComparer.Equals(value, item.Value) && Remove(item.Key);

        IEnumerator<KeyValuePair<TFirst, TSecond>> IEnumerable<KeyValuePair<TFirst, TSecond>>.GetEnumerator() => GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        #endregion

        /// <summary>
        /// Enumerates the elements of a <see cref="BidirectionalDictionary{TFirst, TSecond}" />.
        /// </summary>
        public struct Enumerator : IEnumerator<KeyValuePair<TFirst, TSecond>>
        {
            private readonly BidirectionalDictionary<TFirst, TSecond> _bidirectionalDictionary;
            private readonly int _version;
            private int _index;
            private KeyValuePair<TFirst, TSecond> _current;

            /// <summary>
            /// Gets the element at the current position of the enumerator.
            /// </summary>
            /// <returns>The element in the <see cref="BidirectionalDictionary{TFirst, TSecond}" /> at the current position of the enumerator.</returns>
            public KeyValuePair<TFirst, TSecond> Current => _current;

            object IEnumerator.Current => _current;

            internal Enumerator(BidirectionalDictionary<TFirst, TSecond> bidirectionalDictionary)
            {
                _bidirectionalDictionary = bidirectionalDictionary;
                _version = bidirectionalDictionary._shared.Version;
                _index = 0;
                _current = default;
            }

            /// <summary>
            /// Releases all resources used by the <see cref="BidirectionalDictionary{TFirst, TSecond}.Enumerator" />.
            /// </summary>
            public void Dispose()
            {
            }

            /// <summary>
            /// Advances the enumerator to the next element of the <see cref="BidirectionalDictionary{TFirst, TSecond}" />.
            /// </summary>
            /// <returns>true if the enumerator was successfully advanced to the next element; false if the enumerator has passed the end of the collection.</returns>
            /// <exception cref="InvalidOperationException">The collection was modified after the enumerator was created.</exception>
            public bool MoveNext()
            {
                BidirectionalDictionaryShared shared = _bidirectionalDictionary._shared;
                if (_version != shared.Version)
                {
                    throw new InvalidOperationException(Strings.InvalidOperation_EnumFailedVersion);
                }

                Entry[] firstKeyEntries = _bidirectionalDictionary._entries;
                while ((uint)_index < (uint)shared.Count)
                {
                    Entry entry = firstKeyEntries[_index];
                    if (entry.Next >= -1)
                    {
                        _current = new KeyValuePair<TFirst, TSecond>(entry.Key, _bidirectionalDictionary._reverse._entries[_index].Key);
                        ++_index;
                        return true;
                    }
                    ++_index;
                }
                _current = default;
                return false;
            }

            void IEnumerator.Reset()
            {
                if (_version != _bidirectionalDictionary._shared.Version)
                {
                    throw new InvalidOperationException(Strings.InvalidOperation_EnumFailedVersion);
                }

                _index = 0;
                _current = default;
            }
        }
    }
}
