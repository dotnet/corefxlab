﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections;
using System.Collections.Generic;

namespace Microsoft.Collections.Extensions
{
    /// <summary>
    /// A MultiValueDictionary can be viewed as a <see cref="IDictionary" /> that allows multiple 
    /// values for any given unique key. While the MultiValueDictionary API is 
    /// mostly the same as that of a regular <see cref="IDictionary" />, there is a distinction
    /// in that getting the value for a key returns a <see cref="IReadOnlyCollection{TValue}" /> of values
    /// rather than a single value associated with that key. Additionally, 
    /// there is functionality to allow adding or removing more than a single
    /// value at once. 
    /// 
    /// The MultiValueDictionary can also be viewed as a IReadOnlyDictionary&lt;TKey,IReadOnlyCollection&lt;TValue&gt;t&gt;
    /// where the <see cref="IReadOnlyCollection{TValue}" /> is abstracted from the view of the programmer.
    /// 
    /// For a read-only MultiValueDictionary, see <see cref="ILookup{TKey, TValue}" />.
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    public class MultiValueDictionary<TKey, TValue> :
        IReadOnlyDictionary<TKey, IReadOnlyCollection<TValue>>
    {
        #region Variables
        /*======================================================================
        ** Variables
        ======================================================================*/

        /// <summary>
        /// The private dictionary that this class effectively wraps around
        /// </summary>
        private readonly Dictionary<TKey, InnerCollectionView> _dictionary;

        /// <summary>
        /// The function to construct a new <see cref="ICollection{TValue}"/>
        /// </summary>
        /// <returns></returns>
        private Func<ICollection<TValue>> NewCollectionFactory = () => new List<TValue>();

        /// <summary>
        /// The current version of this MultiValueDictionary used to determine MultiValueDictionary modification
        /// during enumeration
        /// </summary>
        private int _version;

        #endregion

        #region Constructors
        /*======================================================================
        ** Constructors
        ======================================================================*/

        /// <summary>
        /// Initializes a new instance of the <see cref="MultiValueDictionary{TKey, TValue}" /> 
        /// class that is empty, has the default initial capacity, and uses the default
        /// <see cref="IEqualityComparer{TKey}" /> for <typeparamref name="TKey"/>.
        /// </summary>
        public MultiValueDictionary()
        {
            _dictionary = new Dictionary<TKey, InnerCollectionView>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MultiValueDictionary{TKey, TValue}" /> class that is 
        /// empty, has the specified initial capacity, and uses the default <see cref="IEqualityComparer{TKey}"/>
        /// for <typeparamref name="TKey"/>.
        /// </summary>
        /// <param name="capacity">Initial number of keys that the <see cref="MultiValueDictionary{TKey, TValue}" /> will allocate space for</param>
        /// <exception cref="ArgumentOutOfRangeException">capacity must be >= 0</exception>
        public MultiValueDictionary(int capacity)
        {
            if (capacity < 0)
                throw new ArgumentOutOfRangeException(nameof(capacity), Strings.ArgumentOutOfRange_NeedNonNegNum);
            _dictionary = new Dictionary<TKey, InnerCollectionView>(capacity);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MultiValueDictionary{TKey, TValue}" /> class 
        /// that is empty, has the default initial capacity, and uses the 
        /// specified <see cref="IEqualityComparer{TKey}" />.
        /// </summary>
        /// <param name="comparer">Specified comparer to use for the <typeparamref name="TKey"/>s</param>
        /// <remarks>If <paramref name="comparer"/> is set to null, then the default <see cref="IEqualityComparer" /> for <typeparamref name="TKey"/> is used.</remarks>
        public MultiValueDictionary(IEqualityComparer<TKey> comparer)
        {
            _dictionary = new Dictionary<TKey, InnerCollectionView>(comparer);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MultiValueDictionary{TKey, TValue}" /> class 
        /// that is empty, has the specified initial capacity, and uses the 
        /// specified <see cref="IEqualityComparer{TKey}" />.
        /// </summary>
        /// <param name="capacity">Initial number of keys that the <see cref="MultiValueDictionary{TKey, TValue}" /> will allocate space for</param>
        /// <param name="comparer">Specified comparer to use for the <typeparamref name="TKey"/>s</param>
        /// <exception cref="ArgumentOutOfRangeException">Capacity must be >= 0</exception>
        /// <remarks>If <paramref name="comparer"/> is set to null, then the default <see cref="IEqualityComparer" /> for <typeparamref name="TKey"/> is used.</remarks>
        public MultiValueDictionary(int capacity, IEqualityComparer<TKey> comparer)
        {
            if (capacity < 0)
                throw new ArgumentOutOfRangeException(nameof(capacity), Strings.ArgumentOutOfRange_NeedNonNegNum);
            _dictionary = new Dictionary<TKey, InnerCollectionView>(capacity, comparer);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MultiValueDictionary{TKey, TValue}" /> class that contains 
        /// elements copied from the specified IEnumerable&lt;KeyValuePair&lt;TKey, IReadOnlyCollection&lt;TValue&gt;&gt;&gt; and uses the 
        /// default <see cref="IEqualityComparer{TKey}" /> for the <typeparamref name="TKey"/> type.
        /// </summary>
        /// <param name="enumerable">IEnumerable to copy elements into this from</param>
        /// <exception cref="ArgumentNullException">enumerable must be non-null</exception>
        public MultiValueDictionary(IEnumerable<KeyValuePair<TKey, IReadOnlyCollection<TValue>>> enumerable)
            : this(enumerable, null)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="MultiValueDictionary{TKey, TValue}" /> class that contains 
        /// elements copied from the specified IEnumerable&lt;KeyValuePair&lt;TKey, IReadOnlyCollection&lt;TValue&gt;&gt;&gt; and uses the 
        /// specified <see cref="IEqualityComparer{TKey}" />.
        /// </summary>
        /// <param name="enumerable">IEnumerable to copy elements into this from</param>
        /// <param name="comparer">Specified comparer to use for the <typeparamref name="TKey"/>s</param>
        /// <exception cref="ArgumentNullException">enumerable must be non-null</exception>
        /// <remarks>If <paramref name="comparer"/> is set to null, then the default <see cref="IEqualityComparer" /> for <typeparamref name="TKey"/> is used.</remarks>
        public MultiValueDictionary(IEnumerable<KeyValuePair<TKey, IReadOnlyCollection<TValue>>> enumerable, IEqualityComparer<TKey> comparer)
        {
            if (enumerable == null)
                throw new ArgumentNullException(nameof(enumerable));

            _dictionary = new Dictionary<TKey, InnerCollectionView>(comparer);
            foreach (var pair in enumerable)
                AddRange(pair.Key, pair.Value);
        }

        #endregion

        #region Static Factories
        /*======================================================================
        ** Static Factories
        ======================================================================*/

        /// <summary>
        /// Creates a new new instance of the <see cref="MultiValueDictionary{TKey, TValue}" /> 
        /// class that is empty, has the default initial capacity, and uses the default
        /// <see cref="IEqualityComparer{TKey}" /> for <typeparamref name="TKey"/>. The 
        /// internal dictionary will use instances of the <typeparamref name="TValueCollection"/>
        /// class as its collection type.
        /// </summary>
        /// <typeparam name="TValueCollection">
        /// The collection type that this <see cref="MultiValueDictionary{TKey, TValue}" />
        /// will contain in its internal dictionary.
        /// </typeparam>
        /// <returns>A new <see cref="MultiValueDictionary{TKey, TValue}" /> with the specified
        /// parameters.</returns>
        /// <exception cref="InvalidOperationException"><typeparamref name="TValueCollection"/> must not have
        /// IsReadOnly set to true by default.</exception>
        /// <remarks>
        /// Note that <typeparamref name="TValueCollection"/> must implement <see cref="ICollection{TValue}"/>
        /// in addition to being constructable through new(). The collection returned from the constructor
        /// must also not have IsReadOnly set to True by default.
        /// </remarks>
        public static MultiValueDictionary<TKey, TValue> Create<TValueCollection>()
            where TValueCollection : ICollection<TValue>, new()
        {
            if (new TValueCollection().IsReadOnly)
                throw new InvalidOperationException(Strings.Create_TValueCollectionReadOnly);

            return new MultiValueDictionary<TKey, TValue>
            {
                NewCollectionFactory = () => new TValueCollection()
            };
        }

        /// <summary>
        /// Creates a new new instance of the <see cref="MultiValueDictionary{TKey, TValue}" /> 
        /// class that is empty, has the specified initial capacity, and uses the default
        /// <see cref="IEqualityComparer{TKey}" /> for <typeparamref name="TKey"/>. The 
        /// internal dictionary will use instances of the <typeparamref name="TValueCollection"/>
        /// class as its collection type.
        /// </summary>
        /// <typeparam name="TValueCollection">
        /// The collection type that this <see cref="MultiValueDictionary{TKey, TValue}" />
        /// will contain in its internal dictionary.
        /// </typeparam>
        /// <param name="capacity">Initial number of keys that the <see cref="MultiValueDictionary{TKey, TValue}" /> will allocate space for</param>
        /// <returns>A new <see cref="MultiValueDictionary{TKey, TValue}" /> with the specified
        /// parameters.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Capacity must be >= 0</exception>
        /// <exception cref="InvalidOperationException"><typeparamref name="TValueCollection"/> must not have
        /// IsReadOnly set to true by default.</exception>
        /// <remarks>
        /// Note that <typeparamref name="TValueCollection"/> must implement <see cref="ICollection{TValue}"/>
        /// in addition to being constructable through new(). The collection returned from the constructor
        /// must also not have IsReadOnly set to True by default.
        /// </remarks>
        public static MultiValueDictionary<TKey, TValue> Create<TValueCollection>(int capacity)
            where TValueCollection : ICollection<TValue>, new()
        {
            if (capacity < 0)
                throw new ArgumentOutOfRangeException(nameof(capacity), Strings.ArgumentOutOfRange_NeedNonNegNum);
            if (new TValueCollection().IsReadOnly)
                throw new InvalidOperationException(Strings.Create_TValueCollectionReadOnly);

            return new MultiValueDictionary<TKey, TValue>(capacity)
            {
                NewCollectionFactory = () => new TValueCollection()
            };
        }

        /// <summary>
        /// Creates a new new instance of the <see cref="MultiValueDictionary{TKey, TValue}" /> 
        /// class that is empty, has the default initial capacity, and uses the specified
        /// <see cref="IEqualityComparer{TKey}" /> for <typeparamref name="TKey"/>. The 
        /// internal dictionary will use instances of the <typeparamref name="TValueCollection"/>
        /// class as its collection type.
        /// </summary>
        /// <typeparam name="TValueCollection">
        /// The collection type that this <see cref="MultiValueDictionary{TKey, TValue}" />
        /// will contain in its internal dictionary.
        /// </typeparam>
        /// <param name="comparer">Specified comparer to use for the <typeparamref name="TKey"/>s</param>
        /// <exception cref="InvalidOperationException"><typeparamref name="TValueCollection"/> must not have
        /// IsReadOnly set to true by default.</exception>
        /// <returns>A new <see cref="MultiValueDictionary{TKey, TValue}" /> with the specified
        /// parameters.</returns>
        /// <remarks>If <paramref name="comparer"/> is set to null, then the default <see cref="IEqualityComparer" /> for <typeparamref name="TKey"/> is used.</remarks>
        /// <remarks>
        /// Note that <typeparamref name="TValueCollection"/> must implement <see cref="ICollection{TValue}"/>
        /// in addition to being constructable through new(). The collection returned from the constructor
        /// must also not have IsReadOnly set to True by default.
        /// </remarks>
        public static MultiValueDictionary<TKey, TValue> Create<TValueCollection>(IEqualityComparer<TKey> comparer)
            where TValueCollection : ICollection<TValue>, new()
        {
            if (new TValueCollection().IsReadOnly)
                throw new InvalidOperationException(Strings.Create_TValueCollectionReadOnly);

            return new MultiValueDictionary<TKey, TValue>(comparer)
            {
                NewCollectionFactory = () => new TValueCollection()
            };
        }

        /// <summary>
        /// Creates a new new instance of the <see cref="MultiValueDictionary{TKey, TValue}" /> 
        /// class that is empty, has the specified initial capacity, and uses the specified
        /// <see cref="IEqualityComparer{TKey}" /> for <typeparamref name="TKey"/>. The 
        /// internal dictionary will use instances of the <typeparamref name="TValueCollection"/>
        /// class as its collection type.
        /// </summary>
        /// <typeparam name="TValueCollection">
        /// The collection type that this <see cref="MultiValueDictionary{TKey, TValue}" />
        /// will contain in its internal dictionary.
        /// </typeparam>
        /// <param name="capacity">Initial number of keys that the <see cref="MultiValueDictionary{TKey, TValue}" /> will allocate space for</param>
        /// <param name="comparer">Specified comparer to use for the <typeparamref name="TKey"/>s</param>
        /// <returns>A new <see cref="MultiValueDictionary{TKey, TValue}" /> with the specified
        /// parameters.</returns>
        /// <exception cref="InvalidOperationException"><typeparamref name="TValueCollection"/> must not have
        /// IsReadOnly set to true by default.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Capacity must be >= 0</exception>
        /// <remarks>If <paramref name="comparer"/> is set to null, then the default <see cref="IEqualityComparer" /> for <typeparamref name="TKey"/> is used.</remarks>
        /// <remarks>
        /// Note that <typeparamref name="TValueCollection"/> must implement <see cref="ICollection{TValue}"/>
        /// in addition to being constructable through new(). The collection returned from the constructor
        /// must also not have IsReadOnly set to True by default.
        /// </remarks>
        public static MultiValueDictionary<TKey, TValue> Create<TValueCollection>(int capacity, IEqualityComparer<TKey> comparer)
            where TValueCollection : ICollection<TValue>, new()
        {
            if (capacity < 0)
                throw new ArgumentOutOfRangeException(nameof(capacity), Strings.ArgumentOutOfRange_NeedNonNegNum);
            if (new TValueCollection().IsReadOnly)
                throw new InvalidOperationException(Strings.Create_TValueCollectionReadOnly);

            return new MultiValueDictionary<TKey, TValue>(capacity, comparer)
            {
                NewCollectionFactory = () => new TValueCollection()
            };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MultiValueDictionary{TKey, TValue}" /> class that contains 
        /// elements copied from the specified IEnumerable&lt;KeyValuePair&lt;TKey, IReadOnlyCollection&lt;TValue&gt;&gt;&gt;
        /// and uses the default <see cref="IEqualityComparer{TKey}" /> for the <typeparamref name="TKey"/> type.
        /// The internal dictionary will use instances of the <typeparamref name="TValueCollection"/>
        /// class as its collection type.
        /// </summary>
        /// <typeparam name="TValueCollection">
        /// The collection type that this <see cref="MultiValueDictionary{TKey, TValue}" />
        /// will contain in its internal dictionary.
        /// </typeparam>
        /// <param name="enumerable">IEnumerable to copy elements into this from</param>
        /// <returns>A new <see cref="MultiValueDictionary{TKey, TValue}" /> with the specified
        /// parameters.</returns>
        /// <exception cref="InvalidOperationException"><typeparamref name="TValueCollection"/> must not have
        /// IsReadOnly set to true by default.</exception>
        /// <exception cref="ArgumentNullException">enumerable must be non-null</exception>
        /// <remarks>
        /// Note that <typeparamref name="TValueCollection"/> must implement <see cref="ICollection{TValue}"/>
        /// in addition to being constructable through new(). The collection returned from the constructor
        /// must also not have IsReadOnly set to True by default.
        /// </remarks>
        public static MultiValueDictionary<TKey, TValue> Create<TValueCollection>(IEnumerable<KeyValuePair<TKey, IReadOnlyCollection<TValue>>> enumerable)
            where TValueCollection : ICollection<TValue>, new()
        {
            if (enumerable == null)
                throw new ArgumentNullException(nameof(enumerable));
            if (new TValueCollection().IsReadOnly)
                throw new InvalidOperationException(Strings.Create_TValueCollectionReadOnly);

            var multiValueDictionary = new MultiValueDictionary<TKey, TValue>
            {
                NewCollectionFactory = () => new TValueCollection()
            };
            foreach (var pair in enumerable)
                multiValueDictionary.AddRange(pair.Key, pair.Value);
            return multiValueDictionary;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MultiValueDictionary{TKey, TValue}" /> class that contains 
        /// elements copied from the specified IEnumerable&lt;KeyValuePair&lt;TKey, IReadOnlyCollection&lt;TValue&gt;&gt;&gt;
        /// and uses the specified <see cref="IEqualityComparer{TKey}" /> for the <typeparamref name="TKey"/> type.
        /// The internal dictionary will use instances of the <typeparamref name="TValueCollection"/>
        /// class as its collection type.
        /// </summary>
        /// <typeparam name="TValueCollection">
        /// The collection type that this <see cref="MultiValueDictionary{TKey, TValue}" />
        /// will contain in its internal dictionary.
        /// </typeparam>
        /// <param name="enumerable">IEnumerable to copy elements into this from</param>
        /// <param name="comparer">Specified comparer to use for the <typeparamref name="TKey"/>s</param>
        /// <returns>A new <see cref="MultiValueDictionary{TKey, TValue}" /> with the specified
        /// parameters.</returns>
        /// <exception cref="InvalidOperationException"><typeparamref name="TValueCollection"/> must not have
        /// IsReadOnly set to true by default.</exception>
        /// <exception cref="ArgumentNullException">enumerable must be non-null</exception>
        /// <remarks>If <paramref name="comparer"/> is set to null, then the default <see cref="IEqualityComparer" /> for <typeparamref name="TKey"/> is used.</remarks>
        /// <remarks>
        /// Note that <typeparamref name="TValueCollection"/> must implement <see cref="ICollection{TValue}"/>
        /// in addition to being constructable through new(). The collection returned from the constructor
        /// must also not have IsReadOnly set to True by default.
        /// </remarks>
        public static MultiValueDictionary<TKey, TValue> Create<TValueCollection>(IEnumerable<KeyValuePair<TKey, IReadOnlyCollection<TValue>>> enumerable, IEqualityComparer<TKey> comparer)
            where TValueCollection : ICollection<TValue>, new()
        {
            if (enumerable == null)
                throw new ArgumentNullException(nameof(enumerable));
            if (new TValueCollection().IsReadOnly)
                throw new InvalidOperationException(Strings.Create_TValueCollectionReadOnly);

            var multiValueDictionary = new MultiValueDictionary<TKey, TValue>(comparer)
            {
                NewCollectionFactory = () => new TValueCollection()
            };
            foreach (var pair in enumerable)
                multiValueDictionary.AddRange(pair.Key, pair.Value);
            return multiValueDictionary;
        }

        #endregion

        #region Static Factories with Func parameters
        /*======================================================================
        ** Static Factories with Func parameters
        ======================================================================*/

        /// <summary>
        /// Creates a new new instance of the <see cref="MultiValueDictionary{TKey, TValue}" /> 
        /// class that is empty, has the default initial capacity, and uses the default
        /// <see cref="IEqualityComparer{TKey}" /> for <typeparamref name="TKey"/>. The 
        /// internal dictionary will use instances of the <typeparamref name="TValueCollection"/>
        /// class as its collection type.
        /// </summary>
        /// <typeparam name="TValueCollection">
        /// The collection type that this <see cref="MultiValueDictionary{TKey, TValue}" />
        /// will contain in its internal dictionary.
        /// </typeparam>
        /// <param name="collectionFactory">A function to create a new <see cref="ICollection{TValue}"/> to use
        /// in the internal dictionary store of this <see cref="MultiValueDictionary{TKey, TValue}" />.</param>
        /// <returns>A new <see cref="MultiValueDictionary{TKey, TValue}" /> with the specified
        /// parameters.</returns>
        /// <exception cref="InvalidOperationException"><paramref name="collectionFactory"/> must create collections with
        /// IsReadOnly set to true by default.</exception>
        /// <remarks>
        /// Note that <typeparamref name="TValueCollection"/> must implement <see cref="ICollection{TValue}"/>
        /// in addition to being constructable through new(). The collection returned from the constructor
        /// must also not have IsReadOnly set to True by default.
        /// </remarks>
        public static MultiValueDictionary<TKey, TValue> Create<TValueCollection>(Func<TValueCollection> collectionFactory)
            where TValueCollection : ICollection<TValue>
        {
            if (collectionFactory().IsReadOnly)
                throw new InvalidOperationException((Strings.Create_TValueCollectionReadOnly));

            return new MultiValueDictionary<TKey, TValue>
            {
                NewCollectionFactory = (Func<ICollection<TValue>>)(Delegate)collectionFactory
            };
        }

        /// <summary>
        /// Creates a new new instance of the <see cref="MultiValueDictionary{TKey, TValue}" /> 
        /// class that is empty, has the specified initial capacity, and uses the default
        /// <see cref="IEqualityComparer{TKey}" /> for <typeparamref name="TKey"/>. The 
        /// internal dictionary will use instances of the <typeparamref name="TValueCollection"/>
        /// class as its collection type.
        /// </summary>
        /// <typeparam name="TValueCollection">
        /// The collection type that this <see cref="MultiValueDictionary{TKey, TValue}" />
        /// will contain in its internal dictionary.
        /// </typeparam>
        /// <param name="capacity">Initial number of keys that the <see cref="MultiValueDictionary{TKey, TValue}" /> will allocate space for</param>
        /// <param name="collectionFactory">A function to create a new <see cref="ICollection{TValue}"/> to use
        /// in the internal dictionary store of this <see cref="MultiValueDictionary{TKey, TValue}" />.</param> 
        /// <returns>A new <see cref="MultiValueDictionary{TKey, TValue}" /> with the specified
        /// parameters.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Capacity must be >= 0</exception>
        /// <exception cref="InvalidOperationException"><paramref name="collectionFactory"/> must create collections with
        /// IsReadOnly set to true by default.</exception>
        /// <remarks>
        /// Note that <typeparamref name="TValueCollection"/> must implement <see cref="ICollection{TValue}"/>
        /// in addition to being constructable through new(). The collection returned from the constructor
        /// must also not have IsReadOnly set to True by default.
        /// </remarks>
        public static MultiValueDictionary<TKey, TValue> Create<TValueCollection>(int capacity, Func<TValueCollection> collectionFactory)
            where TValueCollection : ICollection<TValue>
        {
            if (capacity < 0)
                throw new ArgumentOutOfRangeException(nameof(capacity), Strings.ArgumentOutOfRange_NeedNonNegNum);
            if (collectionFactory().IsReadOnly)
                throw new InvalidOperationException((Strings.Create_TValueCollectionReadOnly));

            return new MultiValueDictionary<TKey, TValue>(capacity)
            {
                NewCollectionFactory = (Func<ICollection<TValue>>)(Delegate)collectionFactory
            };
        }

        /// <summary>
        /// Creates a new new instance of the <see cref="MultiValueDictionary{TKey, TValue}" /> 
        /// class that is empty, has the default initial capacity, and uses the specified
        /// <see cref="IEqualityComparer{TKey}" /> for <typeparamref name="TKey"/>. The 
        /// internal dictionary will use instances of the <typeparamref name="TValueCollection"/>
        /// class as its collection type.
        /// </summary>
        /// <typeparam name="TValueCollection">
        /// The collection type that this <see cref="MultiValueDictionary{TKey, TValue}" />
        /// will contain in its internal dictionary.
        /// </typeparam>
        /// <param name="comparer">Specified comparer to use for the <typeparamref name="TKey"/>s</param>
        /// <param name="collectionFactory">A function to create a new <see cref="ICollection{TValue}"/> to use
        /// in the internal dictionary store of this <see cref="MultiValueDictionary{TKey, TValue}" />.</param> 
        /// <exception cref="InvalidOperationException"><paramref name="collectionFactory"/> must create collections with
        /// IsReadOnly set to true by default.</exception>
        /// <returns>A new <see cref="MultiValueDictionary{TKey, TValue}" /> with the specified
        /// parameters.</returns>
        /// <remarks>If <paramref name="comparer"/> is set to null, then the default <see cref="IEqualityComparer" /> for <typeparamref name="TKey"/> is used.</remarks>
        /// <remarks>
        /// Note that <typeparamref name="TValueCollection"/> must implement <see cref="ICollection{TValue}"/>
        /// in addition to being constructable through new(). The collection returned from the constructor
        /// must also not have IsReadOnly set to True by default.
        /// </remarks>
        public static MultiValueDictionary<TKey, TValue> Create<TValueCollection>(IEqualityComparer<TKey> comparer, Func<TValueCollection> collectionFactory)
            where TValueCollection : ICollection<TValue>
        {
            if (collectionFactory().IsReadOnly)
                throw new InvalidOperationException((Strings.Create_TValueCollectionReadOnly));

            return new MultiValueDictionary<TKey, TValue>(comparer)
            {
                NewCollectionFactory = (Func<ICollection<TValue>>)(Delegate)collectionFactory
            };
        }

        /// <summary>
        /// Creates a new new instance of the <see cref="MultiValueDictionary{TKey, TValue}" /> 
        /// class that is empty, has the specified initial capacity, and uses the specified
        /// <see cref="IEqualityComparer{TKey}" /> for <typeparamref name="TKey"/>. The 
        /// internal dictionary will use instances of the <typeparamref name="TValueCollection"/>
        /// class as its collection type.
        /// </summary>
        /// <typeparam name="TValueCollection">
        /// The collection type that this <see cref="MultiValueDictionary{TKey, TValue}" />
        /// will contain in its internal dictionary.
        /// </typeparam>
        /// <param name="capacity">Initial number of keys that the <see cref="MultiValueDictionary{TKey, TValue}" /> will allocate space for</param>
        /// <param name="comparer">Specified comparer to use for the <typeparamref name="TKey"/>s</param>
        /// <param name="collectionFactory">A function to create a new <see cref="ICollection{TValue}"/> to use
        /// in the internal dictionary store of this <see cref="MultiValueDictionary{TKey, TValue}" />.</param> 
        /// <returns>A new <see cref="MultiValueDictionary{TKey, TValue}" /> with the specified
        /// parameters.</returns>
        /// <exception cref="InvalidOperationException"><paramref name="collectionFactory"/> must create collections with
        /// IsReadOnly set to true by default.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Capacity must be >= 0</exception>
        /// <remarks>If <paramref name="comparer"/> is set to null, then the default <see cref="IEqualityComparer" /> for <typeparamref name="TKey"/> is used.</remarks>
        /// <remarks>
        /// Note that <typeparamref name="TValueCollection"/> must implement <see cref="ICollection{TValue}"/>
        /// in addition to being constructable through new(). The collection returned from the constructor
        /// must also not have IsReadOnly set to True by default.
        /// </remarks>
        public static MultiValueDictionary<TKey, TValue> Create<TValueCollection>(int capacity, IEqualityComparer<TKey> comparer, Func<TValueCollection> collectionFactory)
            where TValueCollection : ICollection<TValue>
        {
            if (capacity < 0)
                throw new ArgumentOutOfRangeException(nameof(capacity), Strings.ArgumentOutOfRange_NeedNonNegNum);
            if (collectionFactory().IsReadOnly)
                throw new InvalidOperationException((Strings.Create_TValueCollectionReadOnly));

            return new MultiValueDictionary<TKey, TValue>(capacity, comparer)
            {
                NewCollectionFactory = (Func<ICollection<TValue>>)(Delegate)collectionFactory
            };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MultiValueDictionary{TKey, TValue}" /> class that contains 
        /// elements copied from the specified IEnumerable&lt;KeyValuePair&lt;TKey, IReadOnlyCollection&lt;TValue&gt;&gt;&gt;
        /// and uses the default <see cref="IEqualityComparer{TKey}" /> for the <typeparamref name="TKey"/> type.
        /// The internal dictionary will use instances of the <typeparamref name="TValueCollection"/>
        /// class as its collection type.
        /// </summary>
        /// <typeparam name="TValueCollection">
        /// The collection type that this <see cref="MultiValueDictionary{TKey, TValue}" />
        /// will contain in its internal dictionary.
        /// </typeparam>
        /// <param name="enumerable">IEnumerable to copy elements into this from</param>
        /// <param name="collectionFactory">A function to create a new <see cref="ICollection{TValue}"/> to use
        /// in the internal dictionary store of this <see cref="MultiValueDictionary{TKey, TValue}" />.</param> 
        /// <returns>A new <see cref="MultiValueDictionary{TKey, TValue}" /> with the specified
        /// parameters.</returns>
        /// <exception cref="InvalidOperationException"><paramref name="collectionFactory"/> must create collections with
        /// IsReadOnly set to true by default.</exception>
        /// <exception cref="ArgumentNullException">enumerable must be non-null</exception>
        /// <remarks>
        /// Note that <typeparamref name="TValueCollection"/> must implement <see cref="ICollection{TValue}"/>
        /// in addition to being constructable through new(). The collection returned from the constructor
        /// must also not have IsReadOnly set to True by default.
        /// </remarks>
        public static MultiValueDictionary<TKey, TValue> Create<TValueCollection>(IEnumerable<KeyValuePair<TKey, IReadOnlyCollection<TValue>>> enumerable, Func<TValueCollection> collectionFactory)
            where TValueCollection : ICollection<TValue>
        {
            if (enumerable == null)
                throw new ArgumentNullException(nameof(enumerable));
            if (collectionFactory().IsReadOnly)
                throw new InvalidOperationException((Strings.Create_TValueCollectionReadOnly));

            var multiValueDictionary = new MultiValueDictionary<TKey, TValue>
            {
                NewCollectionFactory = (Func<ICollection<TValue>>)(Delegate)collectionFactory
            };
            foreach (var pair in enumerable)
                multiValueDictionary.AddRange(pair.Key, pair.Value);
            return multiValueDictionary;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MultiValueDictionary{TKey, TValue}" /> class that contains 
        /// elements copied from the specified IEnumerable&lt;KeyValuePair&lt;TKey, IReadOnlyCollection&lt;TValue&gt;&gt;&gt;
        /// and uses the specified <see cref="IEqualityComparer{TKey}" /> for the <typeparamref name="TKey"/> type.
        /// The internal dictionary will use instances of the <typeparamref name="TValueCollection"/>
        /// class as its collection type.
        /// </summary>
        /// <typeparam name="TValueCollection">
        /// The collection type that this <see cref="MultiValueDictionary{TKey, TValue}" />
        /// will contain in its internal dictionary.
        /// </typeparam>
        /// <param name="enumerable">IEnumerable to copy elements into this from</param>
        /// <param name="comparer">Specified comparer to use for the <typeparamref name="TKey"/>s</param>
        /// <param name="collectionFactory">A function to create a new <see cref="ICollection{TValue}"/> to use
        /// in the internal dictionary store of this <see cref="MultiValueDictionary{TKey, TValue}" />.</param> 
        /// <returns>A new <see cref="MultiValueDictionary{TKey, TValue}" /> with the specified
        /// parameters.</returns>
        /// <exception cref="InvalidOperationException"><paramref name="collectionFactory"/> must create collections with
        /// IsReadOnly set to true by default.</exception>
        /// <exception cref="ArgumentNullException">enumerable must be non-null</exception>
        /// <remarks>If <paramref name="comparer"/> is set to null, then the default <see cref="IEqualityComparer" /> for <typeparamref name="TKey"/> is used.</remarks>
        /// <remarks>
        /// Note that <typeparamref name="TValueCollection"/> must implement <see cref="ICollection{TValue}"/>
        /// in addition to being constructable through new(). The collection returned from the constructor
        /// must also not have IsReadOnly set to True by default.
        /// </remarks>
        public static MultiValueDictionary<TKey, TValue> Create<TValueCollection>(IEnumerable<KeyValuePair<TKey, IReadOnlyCollection<TValue>>> enumerable, IEqualityComparer<TKey> comparer, Func<TValueCollection> collectionFactory)
            where TValueCollection : ICollection<TValue>
        {
            if (enumerable == null)
                throw new ArgumentNullException(nameof(enumerable));
            if (collectionFactory().IsReadOnly)
                throw new InvalidOperationException((Strings.Create_TValueCollectionReadOnly));

            var multiValueDictionary = new MultiValueDictionary<TKey, TValue>(comparer)
            {
                NewCollectionFactory = (Func<ICollection<TValue>>)(Delegate)collectionFactory
            };
            foreach (var pair in enumerable)
                multiValueDictionary.AddRange(pair.Key, pair.Value);
            return multiValueDictionary;
        }

        #endregion

        #region Concrete Methods
        /*======================================================================
        ** Concrete Methods
        ======================================================================*/

        /// <summary>
        /// Adds the specified <typeparamref name="TKey"/> and <typeparamref name="TValue"/> to the <see cref="MultiValueDictionary{TKey,TValue}"/>.
        /// </summary>
        /// <param name="key">The <typeparamref name="TKey"/> of the element to add.</param>
        /// <param name="value">The <typeparamref name="TValue"/> of the element to add.</param>
        /// <exception cref="ArgumentNullException"><paramref name="key"/> is <c>null</c>.</exception>
        /// <remarks>
        /// Unlike the Add for <see cref="IDictionary" />, the <see cref="MultiValueDictionary{TKey,TValue}"/> Add will not
        /// throw any exceptions. If the given <typeparamref name="TKey"/> is already in the <see cref="MultiValueDictionary{TKey,TValue}"/>,
        /// then <typeparamref name="TValue"/> will be added to <see cref="IReadOnlyCollection{TValue}"/> associated with <paramref name="key"/>
        /// </remarks>
        /// <remarks>
        /// A call to this Add method will always invalidate any currently running enumeration regardless
        /// of whether the Add method actually modified the <see cref="MultiValueDictionary{TKey, TValue}" />.
        /// </remarks>
        public void Add(TKey key, TValue value)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));
            if (!_dictionary.TryGetValue(key, out InnerCollectionView collection))
            {
                collection = new InnerCollectionView(key, NewCollectionFactory());
                _dictionary.Add(key, collection);
            }
            collection.AddValue(value);
            _version++;
        }

        /// <summary>
        /// Adds a number of key-value pairs to this <see cref="MultiValueDictionary{TKey,TValue}"/>, where
        /// the key for each value is <paramref name="key"/>, and the value for a pair
        /// is an element from <paramref name="values"/>
        /// </summary>
        /// <param name="key">The <typeparamref name="TKey"/> of all entries to add</param>
        /// <param name="values">An <see cref="IEnumerable{TValue}"/> of values to add</param>
        /// <exception cref="ArgumentNullException"><paramref name="key"/> and <paramref name="values"/> must be non-null</exception>
        /// <remarks>
        /// A call to this AddRange method will always invalidate any currently running enumeration regardless
        /// of whether the AddRange method actually modified the <see cref="MultiValueDictionary{TKey,TValue}"/>.
        /// </remarks>
        public void AddRange(TKey key, IEnumerable<TValue> values)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));
            if (values == null)
                throw new ArgumentNullException(nameof(values));

            if (!_dictionary.TryGetValue(key, out InnerCollectionView collection))
            {
                collection = new InnerCollectionView(key, NewCollectionFactory());
                _dictionary.Add(key, collection);
            }
            foreach (TValue value in values)
            {
                collection.AddValue(value);
            }
            _version++;
        }

        /// <summary>
        /// Removes every <typeparamref name="TValue"/> associated with the given <typeparamref name="TKey"/>
        /// from the <see cref="MultiValueDictionary{TKey,TValue}"/>.
        /// </summary>
        /// <param name="key">The <typeparamref name="TKey"/> of the elements to remove</param>
        /// <returns><c>true</c> if the removal was successful; otherwise <c>false</c></returns>
        /// <exception cref="ArgumentNullException"><paramref name="key"/> is <c>null</c>.</exception>
        public bool Remove(TKey key)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));

            if (_dictionary.TryGetValue(key, out InnerCollectionView _) && _dictionary.Remove(key))
            {
                _version++;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Removes the first instance (if any) of the given <typeparamref name="TKey"/>-<typeparamref name="TValue"/> 
        /// pair from this <see cref="MultiValueDictionary{TKey,TValue}"/>. 
        /// </summary>
        /// <param name="key">The <typeparamref name="TKey"/> of the element to remove</param>
        /// <param name="value">The <typeparamref name="TValue"/> of the element to remove</param>
        /// <exception cref="ArgumentNullException"><paramref name="key"/> must be non-null</exception>
        /// <returns><c>true</c> if the removal was successful; otherwise <c>false</c></returns>
        /// <remarks>
        /// If the <typeparamref name="TValue"/> being removed is the last one associated with its <typeparamref name="TKey"/>, then that 
        /// <typeparamref name="TKey"/> will be removed from the <see cref="MultiValueDictionary{TKey,TValue}"/> and its 
        /// associated <see cref="IReadOnlyCollection{TValue}"/> will be freed as if a call to <see cref="Remove(TKey)"/>
        /// had been made.
        /// </remarks>
        public bool Remove(TKey key, TValue value)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));

            if (_dictionary.TryGetValue(key, out InnerCollectionView collection) && collection.RemoveValue(value))
            {
                if (collection.Count == 0)
                    _dictionary.Remove(key);
                _version++;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Determines if the given <typeparamref name="TKey"/>-<typeparamref name="TValue"/> 
        /// pair exists within this <see cref="MultiValueDictionary{TKey,TValue}"/>.
        /// </summary>
        /// <param name="key">The <typeparamref name="TKey"/> of the element.</param>
        /// <param name="value">The <typeparamref name="TValue"/> of the element.</param>
        /// <returns><c>true</c> if found; otherwise <c>false</c></returns>
        /// <exception cref="ArgumentNullException"><paramref name="key"/> must be non-null</exception>
        public bool Contains(TKey key, TValue value)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));

            return (_dictionary.TryGetValue(key, out InnerCollectionView collection) && collection.Contains(value));
        }

        /// <summary>
        /// Determines if the given <typeparamref name="TValue"/> exists within this <see cref="MultiValueDictionary{TKey,TValue}"/>.
        /// </summary>
        /// <param name="value">A <typeparamref name="TValue"/> to search the <see cref="MultiValueDictionary{TKey,TValue}"/> for</param>
        /// <returns><c>true</c> if the <see cref="MultiValueDictionary{TKey,TValue}"/> contains the <paramref name="value"/>; otherwise <c>false</c></returns>      
        public bool ContainsValue(TValue value)
        {
            foreach (InnerCollectionView sublist in _dictionary.Values)
                if (sublist.Contains(value))
                    return true;
            return false;
        }

        /// <summary>
        /// Removes every <typeparamref name="TKey"/> and <typeparamref name="TValue"/> from this 
        /// <see cref="MultiValueDictionary{TKey,TValue}"/>.
        /// </summary>
        public void Clear()
        {
            _dictionary.Clear();
            _version++;
        }

        #endregion

        #region Members implemented from IReadOnlyDictionary<TKey, IReadOnlyCollection<TValue>>
        /*======================================================================
        ** Members implemented from IReadOnlyDictionary<TKey, IReadOnlyCollection<TValue>>
        ======================================================================*/

        /// <summary>
        /// Determines if the given <typeparamref name="TKey"/> exists within this <see cref="MultiValueDictionary{TKey,TValue}"/> and has
        /// at least one <typeparamref name="TValue"/> associated with it.
        /// </summary>
        /// <param name="key">The <typeparamref name="TKey"/> to search the <see cref="MultiValueDictionary{TKey,TValue}"/> for</param>
        /// <returns><c>true</c> if the <see cref="MultiValueDictionary{TKey,TValue}"/> contains the requested <typeparamref name="TKey"/>;
        /// otherwise <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="key"/> must be non-null</exception>
        public bool ContainsKey(TKey key)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));
            // Since modification to the MultiValueDictionary is only allowed through its own API, we
            // can ensure that if a collection is in the internal dictionary then it must have at least one
            // associated TValue, or else it would have been removed whenever its final TValue was removed.
            return _dictionary.ContainsKey(key);
        }

        /// <summary>
        /// Gets each <typeparamref name="TKey"/> in this <see cref="MultiValueDictionary{TKey,TValue}"/> that
        /// has one or more associated <typeparamref name="TValue"/>.
        /// </summary>
        /// <value>
        /// An <see cref="IEnumerable{TKey}"/> containing each <typeparamref name="TKey"/> 
        /// in this <see cref="MultiValueDictionary{TKey,TValue}"/> that has one or more associated 
        /// <typeparamref name="TValue"/>.
        /// </value>
        public IEnumerable<TKey> Keys => _dictionary.Keys;

        /// <summary>
        /// Attempts to get the <typeparamref name="TValue"/> associated with the given
        /// <typeparamref name="TKey"/> and place it into <paramref name="value"/>.
        /// </summary>
        /// <param name="key">The <typeparamref name="TKey"/> of the element to retrieve</param>
        /// <param name="value">
        /// When this method returns, contains the <typeparamref name="TValue"/> associated with the specified
        /// <typeparamref name="TKey"/> if it is found; otherwise contains the default value of <typeparamref name="TValue"/>.
        /// </param>
        /// <returns>
        /// <c>true</c> if the <see cref="MultiValueDictionary{TKey,TValue}"/> contains an element with the specified 
        /// <typeparamref name="TKey"/>; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="key"/> must be non-null</exception>
        public bool TryGetValue(TKey key, out IReadOnlyCollection<TValue> value)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));

            var success = _dictionary.TryGetValue(key, out InnerCollectionView collection);
            value = collection;
            return success;
        }

        /// <summary>
        /// Gets an enumerable of <see cref="IReadOnlyCollection{TValue}"/> from this <see cref="MultiValueDictionary{TKey,TValue}"/>,
        /// where each <see cref="IReadOnlyCollection{TValue}" /> is the collection of every <typeparamref name="TValue"/> associated
        /// with a <typeparamref name="TKey"/> present in the <see cref="MultiValueDictionary{TKey,TValue}"/>. 
        /// </summary>
        /// <value>An IEnumerable of each <see cref="IReadOnlyCollection{TValue}"/> in this 
        /// <see cref="MultiValueDictionary{TKey,TValue}"/></value>
        public IEnumerable<IReadOnlyCollection<TValue>> Values => _dictionary.Values;

        /// <summary>
        /// Get every <typeparamref name="TValue"/> associated with the given <typeparamref name="TKey"/>. If 
        /// <paramref name="key"/> is not found in this <see cref="MultiValueDictionary{TKey,TValue}"/>, will 
        /// throw a <see cref="KeyNotFoundException"/>.
        /// </summary>
        /// <param name="key">The <typeparamref name="TKey"/> of the elements to retrieve.</param>
        /// <exception cref="ArgumentNullException"><paramref name="key"/> must be non-null</exception>
        /// <exception cref="KeyNotFoundException"><paramref name="key"/> does not have any associated 
        /// <typeparamref name="TValue"/>s in this <see cref="MultiValueDictionary{TKey,TValue}"/>.</exception>
        /// <value>
        /// An <see cref="IReadOnlyCollection{TValue}"/> containing every <typeparamref name="TValue"/>
        /// associated with <paramref name="key"/>.
        /// </value>
        /// <remarks>
        /// Note that the <see cref="IReadOnlyCollection{TValue}"/> returned will change alongside any changes 
        /// to the <see cref="MultiValueDictionary{TKey,TValue}"/> 
        /// </remarks>
        public IReadOnlyCollection<TValue> this[TKey key]
        {
            get
            {
                if (key == null)
                    throw new ArgumentNullException(nameof(key));

                if (_dictionary.TryGetValue(key, out InnerCollectionView collection))
                    return collection;

                throw new KeyNotFoundException();
            }
        }

        /// <summary>
        /// Returns the number of <typeparamref name="TKey"/>s with one or more associated <typeparamref name="TValue"/>
        /// in this <see cref="MultiValueDictionary{TKey,TValue}"/>.
        /// </summary>
        /// <value>The number of <typeparamref name="TKey"/>s in this <see cref="MultiValueDictionary{TKey,TValue}"/>.</value>
        public int Count => _dictionary.Count;

        /// <summary>
        /// Get an Enumerator over the <typeparamref name="TKey"/>-<see cref="IReadOnlyCollection{TValue}"/>
        /// pairs in this <see cref="MultiValueDictionary{TKey,TValue}"/>.
        /// </summary>
        /// <returns>an Enumerator over the <typeparamref name="TKey"/>-<see cref="IReadOnlyCollection{TValue}"/>
        /// pairs in this <see cref="MultiValueDictionary{TKey,TValue}"/>.</returns>
        public IEnumerator<KeyValuePair<TKey, IReadOnlyCollection<TValue>>> GetEnumerator() => new Enumerator(this);

        IEnumerator IEnumerable.GetEnumerator() => new Enumerator(this);

        #endregion

        /// <summary>
        /// The Enumerator class for a <see cref="MultiValueDictionary{TKey, TValue}"/>
        /// that iterates over <typeparamref name="TKey"/>-<see cref="IReadOnlyCollection{TValue}"/>
        /// pairs.
        /// </summary>
        private class Enumerator :
            IEnumerator<KeyValuePair<TKey, IReadOnlyCollection<TValue>>>
        {
            private readonly MultiValueDictionary<TKey, TValue> _multiValueDictionary;
            private readonly int _version;
            private Dictionary<TKey, InnerCollectionView>.Enumerator _enumerator;
            private EnumerationState _state;

            /// <summary>
            /// Constructor for the enumerator
            /// </summary>
            /// <param name="multiValueDictionary">A MultiValueDictionary to iterate over</param>
            internal Enumerator(MultiValueDictionary<TKey, TValue> multiValueDictionary)
            {
                _multiValueDictionary = multiValueDictionary;
                _version = multiValueDictionary._version;
                _enumerator = multiValueDictionary._dictionary.GetEnumerator();
                _state = EnumerationState.BeforeFirst;
                Current = default;
            }

            public KeyValuePair<TKey, IReadOnlyCollection<TValue>> Current { get; private set; }

            object IEnumerator.Current
            {
                get
                {
                    switch (_state)
                    {
                        case EnumerationState.BeforeFirst:
                            throw new InvalidOperationException((Strings.InvalidOperation_EnumNotStarted));
                        case EnumerationState.AfterLast:
                            throw new InvalidOperationException((Strings.InvalidOperation_EnumEnded));
                        default:
                            return Current;
                    }
                }
            }

            /// <summary>
            /// Advances the enumerator to the next element of the collection.
            /// </summary>
            /// <returns>
            /// true if the enumerator was successfully advanced to the next element; false if the enumerator has passed the end of the collection.
            /// </returns>
            /// <exception cref="T:System.InvalidOperationException">The collection was modified after the enumerator was created. </exception>
            public bool MoveNext()
            {
                if (_version != _multiValueDictionary._version)
                    throw new InvalidOperationException(Strings.InvalidOperation_EnumFailedVersion);

                if (_enumerator.MoveNext())
                {
                    Current = new KeyValuePair<TKey, IReadOnlyCollection<TValue>>(_enumerator.Current.Key, _enumerator.Current.Value);
                    _state = EnumerationState.During;
                    return true;
                }

                Current = default;
                _state = EnumerationState.AfterLast;
                return false;
            }

            /// <summary>
            /// Sets the enumerator to its initial position, which is before the first element in the collection.
            /// </summary>
            /// <exception cref="T:System.InvalidOperationException">The collection was modified after the enumerator was created. </exception>
            public void Reset()
            {
                if (_version != _multiValueDictionary._version)
                    throw new InvalidOperationException(Strings.InvalidOperation_EnumFailedVersion);
                _enumerator.Dispose();
                _enumerator = _multiValueDictionary._dictionary.GetEnumerator();
                Current = default;
                _state = EnumerationState.BeforeFirst;
            }

            /// <summary>
            /// Frees resources associated with this Enumerator
            /// </summary>
            public void Dispose() => _enumerator.Dispose();

            private enum EnumerationState
            {
                BeforeFirst,
                During,
                AfterLast
            }
        }

        /// <summary>
        /// An inner class that functions as a view of an ICollection within a MultiValueDictionary
        /// </summary>
        private class InnerCollectionView :
            ICollection<TValue>,
            IReadOnlyCollection<TValue>
        {
            private readonly ICollection<TValue> _collection;

            #region Private Concrete API
            /*======================================================================
            ** Private Concrete API
            ======================================================================*/

            public InnerCollectionView(TKey key, ICollection<TValue> collection)
            {
                Key = key;
                _collection = collection;
            }

            public void AddValue(TValue item) => _collection.Add(item);

            public bool RemoveValue(TValue item) => _collection.Remove(item);

            #endregion

            #region Shared API
            /*======================================================================
            ** Shared API
            ======================================================================*/

            public bool Contains(TValue item) => _collection.Contains(item);

            public void CopyTo(TValue[] array, int arrayIndex)
            {
                if (array == null)
                    throw new ArgumentNullException(nameof(array));
                if (arrayIndex < 0)
                    throw new ArgumentOutOfRangeException(nameof(arrayIndex), Strings.ArgumentOutOfRange_NeedNonNegNum);
                if (arrayIndex > array.Length)
                    throw new ArgumentOutOfRangeException(nameof(arrayIndex), Strings.ArgumentOutOfRange_Index);
                if (array.Length - arrayIndex < _collection.Count)
                    throw new ArgumentException(Strings.CopyTo_ArgumentsTooSmall, nameof(arrayIndex));

                _collection.CopyTo(array, arrayIndex);
            }

            public int Count => _collection.Count;

            public bool IsReadOnly => true;

            public IEnumerator<TValue> GetEnumerator() => _collection.GetEnumerator();

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

            public TKey Key { get; }

            #endregion

            #region Public-Facing API
            /*======================================================================
            ** Public-Facing API
            ======================================================================*/

            void ICollection<TValue>.Add(TValue item) => throw new NotSupportedException(Strings.ReadOnly_Modification);
            

            void ICollection<TValue>.Clear() => throw new NotSupportedException(Strings.ReadOnly_Modification);

            bool ICollection<TValue>.Remove(TValue item) => throw new NotSupportedException(Strings.ReadOnly_Modification);

            #endregion
        }
    }
}
