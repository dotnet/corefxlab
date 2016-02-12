// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using Xunit;

namespace System.Collections.Tests
{
    public class MVD_TestBase
    {
        private static Random rand = new Random(11231992);

        /// <summary>
        /// Attempts to create a new instance of the given type parameter
        /// and return it. Only supports a limited number of types that can 
        /// be successfully created.
        /// </summary>
        /// <typeparam name="T">The type to be instantiated and returned</typeparam>
        /// <returns>A new object of type <typeparamref name="T"/></returns>
        protected static object TypeBuilder<T>()
        {
            if (typeof(T) == typeof(int))
            {
                return rand.Next(2, 50);
            }
            else if (typeof(T) == typeof(string))
            {
                byte[] bytes = new byte[50];
                rand.NextBytes(bytes);
                return Convert.ToBase64String(bytes);
            }
            else if (typeof(T) == typeof(char))
            {
                return (char)rand.Next(2, 50);
            }

            throw new ArgumentException();
        }

        /// <summary>
        /// Method to create an <see cref="ICollection{T}"/> (where T is 
        /// created randomly through the TypeBuilder method) with the given
        /// number of elements inside of it.
        /// </summary>
        /// <typeparam name="T">The type that the returned collection will contain</typeparam>
        /// <typeparam name="TCollection">The type of the collection to return</typeparam>
        /// <param name="count">The number of elements in the returned collection</param>
        /// <returns>A new collection containing <paramref name="count"/> elements of type <typeparamref name="T"/></returns>
        protected ICollection<T> CreateRange<T, TCollection>(int count)
            where TCollection : ICollection<T>, new()
        {
            var ret = new TCollection();
            for (int i = 0; i < count; i++)
                ret.Add((T)TypeBuilder<T>());
            return ret;
        }

        protected static void CompareEnumerables<TValue>(IEnumerable<TValue> enum1, IEnumerable<TValue> enum2)
        {
            CompareEnumerables<TValue>(enum1, enum2, true);
        }

        /// <summary>
        /// Performs validation to ensure that the two enumerables are equal or not equal depending on areEqual.
        /// </summary>
        protected static void CompareEnumerables<TValue>(IEnumerable<TValue> enum1, IEnumerable<TValue> enum2, bool areEqual)
        {
            var multiDictionary1 = new Dictionary<TValue, int>();
            var multiDictionary2 = new Dictionary<TValue, int>();
            int multiDictionary1nullcount = 0;
            int multiDictionary2nullcount = 0;
            foreach (TValue val in enum1)
            {
                if (val == null)
                    multiDictionary1nullcount++;
                else if (multiDictionary1.ContainsKey(val))
                    multiDictionary1[val] += 1;
                else multiDictionary1.Add(val, 1);
            }
            foreach (TValue val in enum2)
            {
                if (val == null)
                    multiDictionary2nullcount++;
                else if (multiDictionary2.ContainsKey(val))
                    multiDictionary2[val] += 1;
                else multiDictionary2.Add(val, 1);
            }
            if (areEqual)
            {
                Assert.Equal(multiDictionary1.Count, multiDictionary2.Count);
                Assert.Equal(multiDictionary1nullcount, multiDictionary2nullcount);
                foreach (TValue key in multiDictionary1.Keys)
                    Assert.Equal(multiDictionary2[key], multiDictionary1[key]);
            }
            else
            {
                if (multiDictionary1.Count != multiDictionary2.Count && multiDictionary1nullcount != multiDictionary2nullcount)
                {
                    foreach (TValue key in multiDictionary1.Keys)
                        if (multiDictionary2[key] != multiDictionary1[key])
                            return;
                    Assert.True(false);
                }
            }
        }

        /// <summary>
        /// Does validation to determine if the two MVDs contain all of the same elements
        /// </summary>
        protected static void CompareMVDs<TKey, TValue>(MultiValueDictionary<TKey, TValue> mvd1, MultiValueDictionary<TKey, TValue> mvd2)
        {
            Assert.Equal(mvd1.Count, mvd2.Count);
            Assert.Equal(mvd2.Keys, mvd1.Keys);
            foreach (var key in mvd1.Keys)
            {
                var temp = mvd2[key]; // does not throw
                var countMap1 = new Dictionary<TValue, int>();
                var countMap2 = new Dictionary<TValue, int>();
                foreach (var value in mvd1[key])
                {
                    if (countMap1.ContainsKey(value))
                        countMap1[value] = countMap1[value] + 1;
                    else
                        countMap1[value] = 1;
                }
                foreach (var value in mvd1[key])
                {
                    if (countMap2.ContainsKey(value))
                        countMap2[value] = countMap2[value] + 1;
                    else
                        countMap2[value] = 1;
                }
                foreach (var pair in countMap1)
                    Assert.Equal(pair.Value, countMap2[pair.Key]);
            }
        }

        protected class dummyComparer<TKey> : IEqualityComparer<TKey>
        {
            public bool Equals(TKey x, TKey y)
            {
                throw new InvalidOperationException();
            }

            public int GetHashCode(TKey obj)
            {
                throw new InvalidOperationException();
            }
        }

        protected class DummyReadOnlyCollection<TValue> : ICollection<TValue>
        {
            public void Add(TValue item)
            {
                throw new InvalidOperationException();
            }

            public void Clear()
            {
                throw new InvalidOperationException();
            }

            public bool Contains(TValue item)
            {
                throw new InvalidOperationException();
            }

            public void CopyTo(TValue[] array, int arrayIndex)
            {
                throw new InvalidOperationException();
            }

            public int Count
            {
                get { throw new InvalidOperationException(); }
            }

            public bool IsReadOnly
            {
                get { return true; }
            }

            public bool Remove(TValue item)
            {
                throw new InvalidOperationException();
            }

            public IEnumerator<TValue> GetEnumerator()
            {
                throw new InvalidOperationException();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                throw new InvalidOperationException();
            }
        }
    }
}
