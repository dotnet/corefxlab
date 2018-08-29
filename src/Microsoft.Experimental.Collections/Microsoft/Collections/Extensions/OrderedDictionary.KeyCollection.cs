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
        [DebuggerTypeProxy(typeof(DictionaryKeyCollectionDebugView<,>))]
        [DebuggerDisplay("Count = {Count}")]
        public sealed class KeyCollection : IList<TKey>, IReadOnlyList<TKey>
        {
            private readonly OrderedDictionary<TKey, TValue> _orderedDictionary;

            public int Count => _orderedDictionary.Count;

            public TKey this[int index]
            {
                get
                {
                    _orderedDictionary.GetAt(index, out TKey key);
                    return key;
                }
            }

            TKey IList<TKey>.this[int index]
            {
                get => this[index];
                set => throw new NotSupportedException(Strings.NotSupported_KeyCollectionSet);
            }

            bool ICollection<TKey>.IsReadOnly => true;

            internal KeyCollection(OrderedDictionary<TKey, TValue> orderedDictionary)
            {
                _orderedDictionary = orderedDictionary;
            }

            public Enumerator GetEnumerator() => new Enumerator(_orderedDictionary);

            IEnumerator<TKey> IEnumerable<TKey>.GetEnumerator() => GetEnumerator();

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

            int IList<TKey>.IndexOf(TKey item) => _orderedDictionary.IndexOf(item);

            void IList<TKey>.Insert(int index, TKey item) => throw new NotSupportedException(Strings.NotSupported_KeyCollectionSet);

            void IList<TKey>.RemoveAt(int index) => throw new NotSupportedException(Strings.NotSupported_KeyCollectionSet);

            void ICollection<TKey>.Add(TKey item) => throw new NotSupportedException(Strings.NotSupported_KeyCollectionSet);

            void ICollection<TKey>.Clear() => throw new NotSupportedException(Strings.NotSupported_KeyCollectionSet);

            bool ICollection<TKey>.Contains(TKey item) => _orderedDictionary.ContainsKey(item);

            void ICollection<TKey>.CopyTo(TKey[] array, int arrayIndex)
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
                    array[i + arrayIndex] = entries[i].Key;
                }
            }

            bool ICollection<TKey>.Remove(TKey item) => throw new NotSupportedException(Strings.NotSupported_KeyCollectionSet);

            public struct Enumerator : IEnumerator<TKey>
            {
                private readonly OrderedDictionary<TKey, TValue> _orderedDictionary;
                private readonly int _version;
                private int _index;
                private TKey _current;

                public TKey Current => _current;

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
                        _current = _orderedDictionary._entries[_index].Key;
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
