// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;

namespace System.Collections.Sequences
{
    public sealed class Hashtable<K, V> : ISequence<KeyValuePair<K, V>>
    {
        const double Slack = 0.2;
        const int MinCapacity = 4;

        Entry[] _entries = new Entry[0];
        EqualityComparer<K> _comparer;
        int _count;
        int _capacity;

        struct Entry
        {
            public KeyValuePair<K, V> _pair;
            public int _code;

            public bool IsEmpty { get { return _code == 0; } }
        }

        public Hashtable(EqualityComparer<K> comparer) : this(comparer, 0)
        { }

        public Hashtable(EqualityComparer<K> comparer, int capacity)
        {
            _comparer = comparer;
            if (capacity > 0) {
                int size = GetNextPrime(capacity + (int)(Slack * capacity));
                _entries = new Entry[size];
                _capacity = _entries.Length - (int)(Slack * _entries.Length);
            }
        }

        public bool Add(K key, V value)
        {
            if (_count >= _capacity) {
                EnsureCapacity(_capacity == 0 ? MinCapacity : _capacity * 2);
            }
            _count++;
            var code = _comparer.GetHashCode(key);
            if (code == 0) code = 1;

            int bucket = code;
            while (true) {
                int index = bucket % _entries.Length;
                if (_entries[index].IsEmpty) {
                    _entries[index]._code = code;
                    _entries[index]._pair = new KeyValuePair<K, V>(key, value);
                    return false;
                }
                if (_comparer.Equals(_entries[index]._pair.Key, key)) {
                    throw new InvalidOperationException("key already exists");
                }
                bucket++;
            }
        }

        public void EnsureCapacity(int capacity)
        {
            var newTable = new Hashtable<K, V>(_comparer, capacity);
            foreach (var entry in _entries) {
                if (entry.IsEmpty) continue;
                newTable.Add(entry._pair.Key, entry._pair.Value);
            }
            _entries = newTable._entries;
            _capacity = newTable._capacity;
        }

        static int[] s_primes = new int[] { 5, 11, 19, 37, 83, 157, 311, 613, 1231, 2539, 5009,
            10009, 20011, 40009, 80021, 160001, 320009, 640007, 1280023, 2500009, 5000011, 10000019, 20000003
        };
        private int GetNextPrime(int value)
        {
            foreach (var prime in s_primes) {
                if (prime >= value) return prime;
            }

            // TODO: implement
            throw new NotImplementedException();
        }

        public int Length => _count;

        public SequencePosition Start => default;

        public bool TryGet(ref SequencePosition position, out KeyValuePair<K, V> item, bool advance = true)
        {
            item = default;

            if (_count == 0 | position.Equals(default)) {
                position = default;
                return false;
            }

            if (position.Equals(default)) {
                var firstOccupiedSlot = FindFirstStartingAt(0);
                if (firstOccupiedSlot == -1) {
                    position = default;
                    return false;
                }

                position = new SequencePosition(null, firstOccupiedSlot);
            }

            var index = position.GetInteger();
            var entry = _entries[index];
            if (entry.IsEmpty) {
                throw new InvalidOperationException();
            }

            if (advance) {
                var first = FindFirstStartingAt(index + 1);
                position = new SequencePosition(null, first);
                if (first == -1) {
                    position = default;
                }
            }

            item = entry._pair;
            return true;
        }

        private int FindFirstStartingAt(int index)
        {
            for (int i = index; i < _entries.Length; i++) {
                if (!_entries[i].IsEmpty) {
                    return i;
                }
            }

            return -1;
        }

        public SequenceEnumerator<KeyValuePair<K, V>> GetEnumerator()
        {
            return new SequenceEnumerator<KeyValuePair<K, V>>(this);
        }

        public SequencePosition GetPosition(SequencePosition origin, long offset)
        {
            if (offset<0) throw new ArgumentOutOfRangeException(nameof(offset));
            while (offset-- > 0 && TryGet(ref origin, out _));
            if (offset == 0) return origin;
            throw new ArgumentOutOfRangeException(nameof(offset));
        }
    }
}
