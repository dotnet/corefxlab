using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace Microsoft.Collections.Extensions
{
    public partial class BidirectionalDictionary<TFirst, TSecond>
    {
        /// <summary>
        /// Represents the collection of keys in a <see cref="BidirectionalDictionary{TFirst, TSecond}" />. This class cannot be inherited.
        /// </summary>
        [DebuggerTypeProxy(typeof(DictionaryKeyCollectionDebugView<,>))]
        [DebuggerDisplay("Count = {Count}")]
        public sealed class KeyCollection : ICollection<TFirst>, IReadOnlyCollection<TFirst>
        {
            private readonly BidirectionalDictionary<TFirst, TSecond> _bidirectionalDictionary;

            /// <summary>
            /// Gets the number of elements contained in the <see cref="BidirectionalDictionary{TFirst, TSecond}.KeyCollection" />.
            /// </summary>
            /// <returns>The number of elements contained in the <see cref="BidirectionalDictionary{TFirst, TSecond}.KeyCollection" />.</returns>
            public int Count => _bidirectionalDictionary.Count;

            bool ICollection<TFirst>.IsReadOnly => true;

            internal KeyCollection(BidirectionalDictionary<TFirst, TSecond> bidirectionalDictionary)
            {
                _bidirectionalDictionary = bidirectionalDictionary;
            }

            /// <summary>
            /// Determines whether the <see cref="BidirectionalDictionary{TFirst, TSecond}.KeyCollection" /> contains a specific value.
            /// </summary>
            /// <param name="firstKey">The object to locate in the <see cref="BidirectionalDictionary{TFirst, TSecond}.KeyCollection" />.</param>
            /// <returns>true if <paramref name="firstKey" /> is found in the <see cref="BidirectionalDictionary{TFirst, TSecond}.KeyCollection" />; otherwise, false.</returns>
            public bool Contains(TFirst firstKey) => _bidirectionalDictionary.ContainsKey(firstKey);

            /// <summary>
            /// Returns an enumerator that iterates through the <see cref="BidirectionalDictionary{TFirst, TSecond}.KeyCollection" />.
            /// </summary>
            /// <returns>A <see cref="BidirectionalDictionary{TFirst, TSecond}.KeyCollection.Enumerator" /> for the <see cref="BidirectionalDictionary{TFirst, TSecond}.KeyCollection" />.</returns>
            public Enumerator GetEnumerator() => new Enumerator(_bidirectionalDictionary);

            void ICollection<TFirst>.Add(TFirst item) => throw new NotSupportedException(Strings.NotSupported_KeyCollectionSet);

            void ICollection<TFirst>.Clear() => throw new NotSupportedException(Strings.NotSupported_KeyCollectionSet);

            void ICollection<TFirst>.CopyTo(TFirst[] array, int arrayIndex)
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

                int count = _bidirectionalDictionary._shared.Count;
                BidirectionalDictionaryEntry<TFirst>[] entries = _bidirectionalDictionary._firstKeyEntries;
                int currentArrayIndex = arrayIndex;
                for (int i = 0; i < count; ++i)
                {
                    BidirectionalDictionaryEntry<TFirst> entry = entries[i];
                    if (entry.Next >= -1)
                    {
                        array[currentArrayIndex++] = entry.Key;
                    }
                }
            }

            bool ICollection<TFirst>.Remove(TFirst item) => throw new NotSupportedException(Strings.NotSupported_KeyCollectionSet);

            IEnumerator<TFirst> IEnumerable<TFirst>.GetEnumerator() => GetEnumerator();

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

            /// <summary>
            /// Enumerates the elements of a <see cref="BidirectionalDictionary{TFirst, TSecond}.KeyCollection" />.
            /// </summary>
            public struct Enumerator : IEnumerator<TFirst>
            {
                private readonly BidirectionalDictionary<TFirst, TSecond> _bidirectionalDictionary;
                private readonly int _version;
                private int _index;
                private TFirst _current;

                /// <summary>
                /// Gets the element at the current position of the enumerator.
                /// </summary>
                /// <returns>The element in the <see cref="BidirectionalDictionary{TFirst, TSecond}.KeyCollection" /> at the current position of the enumerator.</returns>
                public TFirst Current => _current;

                object IEnumerator.Current => _current;

                internal Enumerator(BidirectionalDictionary<TFirst, TSecond> bidirectionalDictionary)
                {
                    _bidirectionalDictionary = bidirectionalDictionary;
                    _version = bidirectionalDictionary._shared.Version;
                    _index = 0;
                    _current = default;
                }

                /// <summary>
                /// Releases all resources used by the <see cref="BidirectionalDictionary{TFirst, TSecond}.KeyCollection.Enumerator" />.
                /// </summary>
                public void Dispose()
                {
                }

                /// <summary>
                /// Advances the enumerator to the next element of the <see cref="BidirectionalDictionary{TFirst, TSecond}.KeyCollection" />.
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

                    BidirectionalDictionaryEntry<TFirst>[] entries = _bidirectionalDictionary._firstKeyEntries;
                    while ((uint)_index < (uint)shared.Count)
                    {
                        BidirectionalDictionaryEntry<TFirst> entry = entries[_index++];

                        if (entry.Next >= -1)
                        {
                            _current = entry.Key;
                            return true;
                        }
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
}
