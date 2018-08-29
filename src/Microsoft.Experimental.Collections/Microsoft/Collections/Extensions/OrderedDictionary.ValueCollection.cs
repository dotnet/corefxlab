// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace Microsoft.Collections.Extensions
{
    public partial class OrderedDictionary<TKey, TValue>
    {
        [DebuggerTypeProxy(typeof(DictionaryValueCollectionDebugView<,>))]
        [DebuggerDisplay("Count = {Count}")]
        public sealed class ValueCollection : IList<TValue>, IReadOnlyList<TValue>
        {
            private readonly OrderedDictionary<TKey, TValue> _orderedDictionary;

            public int Count => _orderedDictionary.Count;

            public TValue this[int index] => _orderedDictionary[index];

            TValue IList<TValue>.this[int index]
            {
                get => this[index];
                set => throw new NotSupportedException(Strings.NotSupported_ValueCollectionSet);
            }

            bool ICollection<TValue>.IsReadOnly => true;

            internal ValueCollection(OrderedDictionary<TKey, TValue> orderedDictionary)
            {
                _orderedDictionary = orderedDictionary;
            }

            public Enumerator GetEnumerator() => new Enumerator(_orderedDictionary);

            IEnumerator<TValue> IEnumerable<TValue>.GetEnumerator() => GetEnumerator();

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

            int IList<TValue>.IndexOf(TValue item)
            {
                EqualityComparer<TValue> comparer = EqualityComparer<TValue>.Default;
                Entry[] entries = _orderedDictionary._entries;
                int count = Count;
                for (int i = 0; i < count; ++i)
                {
                    if (comparer.Equals(entries[i].Value, item))
                    {
                        return i;
                    }
                }
                return -1;
            }

            void IList<TValue>.Insert(int index, TValue item) => throw new NotSupportedException(Strings.NotSupported_ValueCollectionSet);

            void IList<TValue>.RemoveAt(int index) => throw new NotSupportedException(Strings.NotSupported_ValueCollectionSet);

            void ICollection<TValue>.Add(TValue item) => throw new NotSupportedException(Strings.NotSupported_ValueCollectionSet);

            void ICollection<TValue>.Clear() => throw new NotSupportedException(Strings.NotSupported_ValueCollectionSet);

            bool ICollection<TValue>.Contains(TValue item) => ((IList<TValue>)this).IndexOf(item) >= 0;

            void ICollection<TValue>.CopyTo(TValue[] array, int arrayIndex)
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

                Entry[] entries = _orderedDictionary._entries;
                for (int i = 0; i < count; ++i)
                {
                    array[i + arrayIndex] = entries[i].Value;
                }
            }

            bool ICollection<TValue>.Remove(TValue item) => throw new NotSupportedException(Strings.NotSupported_ValueCollectionSet);

            public struct Enumerator : IEnumerator<TValue>
            {
                private readonly OrderedDictionary<TKey, TValue> _orderedDictionary;
                private readonly int _version;
                private int _index;
                private TValue _current;

                public TValue Current => _current;

                object IEnumerator.Current => _current;

                internal Enumerator(OrderedDictionary<TKey, TValue> orderedDictionary)
                {
                    _orderedDictionary = orderedDictionary;
                    _version = orderedDictionary._version;
                    _index = 0;
                    _current = default;
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
                        _current = _orderedDictionary._entries[_index].Value;
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
}
