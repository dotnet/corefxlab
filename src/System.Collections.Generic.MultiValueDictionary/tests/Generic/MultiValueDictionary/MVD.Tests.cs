// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xunit;

namespace System.Collections.Tests
{
    public class MVD_Tests : MVD_TestBase
    {
        #region Helper Methods
        /*======================================================================
        ** Helper Methods
        ======================================================================*/
        public static MultiValueDictionary<TKey, TValue>[] Multi<TKey, TValue, TValueCollection>()
            where TValueCollection : ICollection<TValue>, new()
        {
            MultiValueDictionary<TKey, TValue>[] ret = new MultiValueDictionary<TKey, TValue>[6];

            //Empty MultiValueDictionary
            ret[0] = MultiValueDictionary<TKey, TValue>.Create<TValueCollection>();

            //1-element MultiValueDictionary
            ret[1] = MultiValueDictionary<TKey, TValue>.Create<TValueCollection>();
            ret[1].Add((TKey)TypeBuilder<TKey>(), (TValue)TypeBuilder<TValue>());

            //2-element MultiValueDictionary
            ret[2] = MultiValueDictionary<TKey, TValue>.Create<TValueCollection>();
            ret[2].Add((TKey)TypeBuilder<TKey>(), (TValue)TypeBuilder<TValue>());
            ret[2].Add((TKey)TypeBuilder<TKey>(), (TValue)TypeBuilder<TValue>());

            //Lightly filled MultiValueDictionary
            ret[3] = MultiValueDictionary<TKey, TValue>.Create<TValueCollection>();
            for (int i = 0; i < 20; i++)
                ret[3].Add((TKey)TypeBuilder<TKey>(), (TValue)TypeBuilder<TValue>());

            //Moderately filled MultiValueDictionary
            ret[4] = MultiValueDictionary<TKey, TValue>.Create<TValueCollection>();
            for (int i = 0; i < 200; i++)
                ret[4].Add((TKey)TypeBuilder<TKey>(), (TValue)TypeBuilder<TValue>());

            //Very filled MultiValueDictionary
            ret[5] = MultiValueDictionary<TKey, TValue>.Create<TValueCollection>();
            for (int i = 0; i < 5000; i++)
                ret[4].Add((TKey)TypeBuilder<TKey>(), (TValue)TypeBuilder<TValue>());

            return ret;
        }

        private void TestCollectionType<TKey, TValue>(Type type, MultiValueDictionary<TKey, TValue> mvd)
        {
            var newCollection = (ICollection<TValue>)Activator.CreateInstance(type);
            var key = (TKey)TypeBuilder<TKey>();
            mvd.Remove(key);
            for (int i = 0; i < (int)TypeBuilder<int>(); i++)
            {
                var value = (TValue)TypeBuilder<TValue>();
                mvd.Add(key, value);
                newCollection.Add(value);
            }
            CompareEnumerables<TValue>(mvd[key], newCollection, true);
        }

        private int PairsCount<TKey, TValue>(MultiValueDictionary<TKey, TValue> mvd)
        {
            int count = 0;
            foreach (var pair in mvd)
                count += pair.Value.Count;
            return count;
        }

        #endregion

        #region Test callers
        /*======================================================================
        ** Contains the [Fact] tests that call all actual tests
        ======================================================================*/

        private void CallTests(string testsPrefix)
        {
            var testMethods =
            from method in this.GetType().GetRuntimeMethods()
            where method.Name.StartsWith(testsPrefix)
                && method.IsGenericMethod
            select method;

            foreach (var test in testMethods)
            {
                test.MakeGenericMethod(typeof(char), typeof(int)).Invoke(this, new object[] { });
                test.MakeGenericMethod(typeof(char), typeof(string)).Invoke(this, new object[] { });
                test.MakeGenericMethod(typeof(char), typeof(String)).Invoke(this, new object[] { });
                test.MakeGenericMethod(typeof(int), typeof(int)).Invoke(this, new object[] { });
                test.MakeGenericMethod(typeof(string), typeof(string)).Invoke(this, new object[] { });
                test.MakeGenericMethod(typeof(String), typeof(int)).Invoke(this, new object[] { });
                test.MakeGenericMethod(typeof(int), typeof(string)).Invoke(this, new object[] { });
            }
        }

        [Fact]
        public void MVD_Constructor1() { CallTests("MVD_Constructor1_"); }
        [Fact]
        public void MVD_Constructor2() { CallTests("MVD_Constructor2_"); }
        [Fact]
        public void MVD_Constructor3() { CallTests("MVD_Constructor3_"); }
        [Fact]
        public void MVD_Constructor4() { CallTests("MVD_Constructor4_"); }
        [Fact]
        public void MVD_Constructor5() { CallTests("MVD_Constructor5_"); }
        [Fact]
        public void MVD_Constructor6() { CallTests("MVD_Constructor6_"); }
        [Fact]
        public void MVD_Constructor7() { CallTests("MVD_Constructor7_"); }
        [Fact]
        public void MVD_Constructor8() { CallTests("MVD_Constructor8_"); }
        [Fact]
        public void MVD_Add() { CallTests("MVD_Add_"); }
        [Fact]
        public void MVD_AddRange() { CallTests("MVD_AddRange_"); }
        [Fact]
        public void MVD_Clear() { CallTests("MVD_Clear_"); }
        [Fact]
        public void MVD_Contains() { CallTests("MVD_Contains_"); }
        [Fact]
        public void MVD_ContainsKey() { CallTests("MVD_ContainsKey_"); }
        [Fact]
        public void MVD_ContainsValue() { CallTests("MVD_ContainsValue_"); }
        [Fact]
        public void MVD_Count() { CallTests("MVD_Count_"); }
        [Fact]
        public void MVD_GetEnumerator() { CallTests("MVD_GetEnumerator_"); }
        [Fact]
        public void MVD_Item() { CallTests("MVD_Item_"); }
        [Fact]
        public void MVD_Keys() { CallTests("MVD_Keys_"); }
        [Fact]
        public void MVD_Remove() { CallTests("MVD_Remove_"); }
        [Fact]
        public void MVD_RemoveItem() { CallTests("MVD_RemoveItem_"); }
        [Fact]
        public void MVD_TryGetValue() { CallTests("MVD_TryGetValue_"); }
        [Fact]
        public void MVD_Values() { CallTests("MVD_Values_"); }

        #endregion

        #region Tests for: Constructor1
        /*======================================================================
        ** Constructor 1 includes:
        **     -MultiValueDictionary() 
        **     -MultiValueDictionary.Create<TValueCollection>()
        **     -MultiValueDictionary.Create<TValueCollection>(Func<ICollection<TValue>> collectionFactory)
        ======================================================================*/

        private delegate MultiValueDictionary<TKey, TValue> Constructor1Delegate<TKey, TValue>();
        private Tuple<Constructor1Delegate<TKey, TValue>, Type>[] Constructor1<TKey, TValue>()
        {
            var ret = new Tuple<Constructor1Delegate<TKey, TValue>, Type>[5];
            ret[0] = Tuple.Create<Constructor1Delegate<TKey, TValue>, Type>(() => new MultiValueDictionary<TKey, TValue>(), typeof(List<TValue>));
            ret[1] = Tuple.Create<Constructor1Delegate<TKey, TValue>, Type>(() => MultiValueDictionary<TKey, TValue>.Create<List<TValue>>(), typeof(List<TValue>));
            ret[2] = Tuple.Create<Constructor1Delegate<TKey, TValue>, Type>(() => MultiValueDictionary<TKey, TValue>.Create<HashSet<TValue>>(), typeof(HashSet<TValue>));
            ret[3] = Tuple.Create<Constructor1Delegate<TKey, TValue>, Type>(() => MultiValueDictionary<TKey, TValue>.Create<List<TValue>>(() => new List<TValue>()), typeof(List<TValue>));
            ret[4] = Tuple.Create<Constructor1Delegate<TKey, TValue>, Type>(() => MultiValueDictionary<TKey, TValue>.Create<HashSet<TValue>>(() => new HashSet<TValue>()), typeof(HashSet<TValue>));
            return ret;
        }

        public void MVD_Constructor1_Valid<TKey, TValue>()
        {
            foreach (var tuple in Constructor1<TKey, TValue>())
            {
                var mvd = tuple.Item1();
                TestCollectionType<TKey, TValue>(tuple.Item2, mvd);
            }
        }

        public void MVD_Constructor1_InvalidTCollection<TKey, TValue>()
        {
            Assert.Throws<InvalidOperationException>(() => MultiValueDictionary<TKey, TValue>.Create<DummyReadOnlyCollection<TValue>>());
            Assert.Throws<InvalidOperationException>(() => MultiValueDictionary<TKey, TValue>.Create<DummyReadOnlyCollection<TValue>>(() => new DummyReadOnlyCollection<TValue>()));
        }

        #endregion

        #region Tests for: Constructor2
        /*======================================================================
        ** Constructor 2 includes:
        **     -MultiValueDictionary(int capacity) 
        **     -MultiValueDictionary.Create<TValueCollection>(int capacity)
        **     -MultiValueDictionary.Create<TValueCollection>(int capacity, Func<ICollection<TValue>> collectionFactory)
        ======================================================================*/

        private delegate MultiValueDictionary<TKey, TValue> Constructor2Delegate<TKey, TValue>(int capacity);
        private Tuple<Constructor2Delegate<TKey, TValue>, Type>[] Constructor2<TKey, TValue>()
        {
            var ret = new Tuple<Constructor2Delegate<TKey, TValue>, Type>[5];
            ret[0] = Tuple.Create<Constructor2Delegate<TKey, TValue>, Type>((int capacity) => new MultiValueDictionary<TKey, TValue>(capacity), typeof(List<TValue>));
            ret[1] = Tuple.Create<Constructor2Delegate<TKey, TValue>, Type>((int capacity) => MultiValueDictionary<TKey, TValue>.Create<List<TValue>>(capacity), typeof(List<TValue>));
            ret[2] = Tuple.Create<Constructor2Delegate<TKey, TValue>, Type>((int capacity) => MultiValueDictionary<TKey, TValue>.Create<List<TValue>>(capacity, () => new List<TValue>()), typeof(List<TValue>));
            ret[3] = Tuple.Create<Constructor2Delegate<TKey, TValue>, Type>((int capacity) => MultiValueDictionary<TKey, TValue>.Create<HashSet<TValue>>(capacity), typeof(HashSet<TValue>));
            ret[4] = Tuple.Create<Constructor2Delegate<TKey, TValue>, Type>((int capacity) => MultiValueDictionary<TKey, TValue>.Create<HashSet<TValue>>(capacity, () => new HashSet<TValue>()), typeof(HashSet<TValue>));
            return ret;
        }

        public void MVD_Constructor2_PositiveCapacity<TKey, TValue>()
        {
            foreach (var tuple in Constructor2<TKey, TValue>())
            {
                var mvd = tuple.Item1(1);
                TestCollectionType<TKey, TValue>(tuple.Item2, mvd);
            }
        }

        public void MVD_Constructor2_ZeroCapacity<TKey, TValue>()
        {
            foreach (var tuple in Constructor2<TKey, TValue>())
            {
                var mvd = tuple.Item1(0);
                TestCollectionType<TKey, TValue>(tuple.Item2, mvd);
            }
        }

        public void MVD_Constructor2_NegativeCapacity<TKey, TValue>()
        {
            foreach (var tuple in Constructor2<TKey, TValue>())
            {
                Assert.Throws<ArgumentOutOfRangeException>(() => tuple.Item1(-1));
            }
        }

        public void MVD_Constructor2_InvalidTCollection<TKey, TValue>()
        {
            Assert.Throws<InvalidOperationException>(() => MultiValueDictionary<TKey, TValue>.Create<DummyReadOnlyCollection<TValue>>(1));
            Assert.Throws<InvalidOperationException>(() => MultiValueDictionary<TKey, TValue>.Create<DummyReadOnlyCollection<TValue>>(1, () => new DummyReadOnlyCollection<TValue>()));
        }

        #endregion

        #region Tests for: Constructor3
        /*======================================================================
        ** Constructor 3 includes:
        **     -MultiValueDictionary(IEqualityComparer<TKey> comparer) 
        **     -MultiValueDictionary.Create<TValueCollection>(IEqualityComparer<TKey> comparer)
        **     -MultiValueDictionary.Create<TValueCollection>(IEqualityComparer<TKey> comparer, Func<ICollection<TValue>> collectionFactory)
        ======================================================================*/

        private delegate MultiValueDictionary<TKey, TValue> Constructor3Delegate<TKey, TValue>(IEqualityComparer<TKey> comparer);
        private Tuple<Constructor3Delegate<TKey, TValue>, Type>[] Constructor3<TKey, TValue>()
        {
            var ret = new Tuple<Constructor3Delegate<TKey, TValue>, Type>[5];
            ret[0] = Tuple.Create<Constructor3Delegate<TKey, TValue>, Type>((IEqualityComparer<TKey> comparer) => new MultiValueDictionary<TKey, TValue>(comparer), typeof(List<TValue>));
            ret[1] = Tuple.Create<Constructor3Delegate<TKey, TValue>, Type>((IEqualityComparer<TKey> comparer) => MultiValueDictionary<TKey, TValue>.Create<List<TValue>>(comparer), typeof(List<TValue>));
            ret[2] = Tuple.Create<Constructor3Delegate<TKey, TValue>, Type>((IEqualityComparer<TKey> comparer) => MultiValueDictionary<TKey, TValue>.Create<HashSet<TValue>>(comparer), typeof(HashSet<TValue>));
            ret[3] = Tuple.Create<Constructor3Delegate<TKey, TValue>, Type>((IEqualityComparer<TKey> comparer) => MultiValueDictionary<TKey, TValue>.Create<List<TValue>>(comparer, () => new List<TValue>()), typeof(List<TValue>));
            ret[4] = Tuple.Create<Constructor3Delegate<TKey, TValue>, Type>((IEqualityComparer<TKey> comparer) => MultiValueDictionary<TKey, TValue>.Create<HashSet<TValue>>(comparer, () => new HashSet<TValue>()), typeof(HashSet<TValue>));
            return ret;
        }

        public void MVD_Constructor3_DummyComparer<TKey, TValue>()
        {
            foreach (var tuple in Constructor3<TKey, TValue>())
            {
                Assert.Throws<InvalidOperationException>(() =>
                {
                    var mvd = tuple.Item1(new dummyComparer<TKey>());
                    mvd.Add((TKey)TypeBuilder<TKey>(), (TValue)TypeBuilder<TValue>());
                });
            }
        }

        public void MVD_Constructor3_NullComparer<TKey, TValue>()
        {
            foreach (var tuple in Constructor3<TKey, TValue>())
            {
                var mvd = tuple.Item1(null);
                TestCollectionType<TKey, TValue>(tuple.Item2, mvd);
            }
        }

        public void MVD_Constructor3_InvalidTCollection<TKey, TValue>()
        {
            Assert.Throws<InvalidOperationException>(() => MultiValueDictionary<TKey, TValue>.Create<DummyReadOnlyCollection<TValue>>((IEqualityComparer<TKey>)null));
            Assert.Throws<InvalidOperationException>(() => MultiValueDictionary<TKey, TValue>.Create<DummyReadOnlyCollection<TValue>>((IEqualityComparer<TKey>)null, () => new DummyReadOnlyCollection<TValue>()));
        }

        #endregion

        #region Tests for: Constructor4
        /*======================================================================
        ** Constructor 4 includes:
        **     -MultiValueDictionary(int capacity, IEqualityComparer<TKey> comparer) 
        **     -MultiValueDictionary.Create<TValueCollection>(int capacity, IEqualityComparer<TKey> comparer)
        **     -MultiValueDictionary.Create<TValueCollection>(int capacity, IEqualityComparer<TKey> comparer, Func<ICollection<TValue>> collectionFactory)
        ======================================================================*/

        private delegate MultiValueDictionary<TKey, TValue> Constructor4Delegate<TKey, TValue>(int capacity, IEqualityComparer<TKey> comparer);
        private Tuple<Constructor4Delegate<TKey, TValue>, Type>[] Constructor4<TKey, TValue>()
        {
            var ret = new Tuple<Constructor4Delegate<TKey, TValue>, Type>[5];
            ret[0] = Tuple.Create<Constructor4Delegate<TKey, TValue>, Type>((int capacity, IEqualityComparer<TKey> comparer) => new MultiValueDictionary<TKey, TValue>(capacity, comparer), typeof(List<TValue>));
            ret[1] = Tuple.Create<Constructor4Delegate<TKey, TValue>, Type>((int capacity, IEqualityComparer<TKey> comparer) => MultiValueDictionary<TKey, TValue>.Create<List<TValue>>(capacity, comparer), typeof(List<TValue>));
            ret[2] = Tuple.Create<Constructor4Delegate<TKey, TValue>, Type>((int capacity, IEqualityComparer<TKey> comparer) => MultiValueDictionary<TKey, TValue>.Create<HashSet<TValue>>(capacity, comparer, () => new HashSet<TValue>()), typeof(HashSet<TValue>));
            ret[3] = Tuple.Create<Constructor4Delegate<TKey, TValue>, Type>((int capacity, IEqualityComparer<TKey> comparer) => MultiValueDictionary<TKey, TValue>.Create<List<TValue>>(capacity, comparer, () => new List<TValue>()), typeof(List<TValue>));
            ret[4] = Tuple.Create<Constructor4Delegate<TKey, TValue>, Type>((int capacity, IEqualityComparer<TKey> comparer) => MultiValueDictionary<TKey, TValue>.Create<HashSet<TValue>>(capacity, comparer), typeof(HashSet<TValue>));
            return ret;
        }

        public void MVD_Constructor4_DummyComparerValidCapacity<TKey, TValue>()
        {
            foreach (var tuple in Constructor4<TKey, TValue>())
            {
                Assert.Throws<InvalidOperationException>(() =>
                {
                    var mvd = tuple.Item1(0, new dummyComparer<TKey>());
                    mvd.Add((TKey)TypeBuilder<TKey>(), (TValue)TypeBuilder<TValue>());
                });
            }
        }

        public void MVD_Constructor4_NullComparerValidCapacity<TKey, TValue>()
        {
            foreach (var tuple in Constructor4<TKey, TValue>())
            {
                var mvd = tuple.Item1(1, null);
                TestCollectionType<TKey, TValue>(tuple.Item2, mvd);
            }
        }

        public void MVD_Constructor4_DummyComparerInvalidCapacity<TKey, TValue>()
        {
            foreach (var tuple in Constructor4<TKey, TValue>())
            {
                Assert.Throws<ArgumentOutOfRangeException>(() =>
                {
                    var mvd = tuple.Item1(-1, new dummyComparer<TKey>());
                    mvd.Add((TKey)TypeBuilder<TKey>(), (TValue)TypeBuilder<TValue>());
                });
            }
        }

        public void MVD_Constructor4_NullComparerInvalidCapacity<TKey, TValue>()
        {
            foreach (var tuple in Constructor4<TKey, TValue>())
            {
                Assert.Throws<ArgumentOutOfRangeException>(() =>
                {
                    var mvd = tuple.Item1(-1, null);
                    mvd.Add((TKey)TypeBuilder<TKey>(), (TValue)TypeBuilder<TValue>());
                });
            }
        }

        public void MVD_Constructor4_InvalidTCollection<TKey, TValue>()
        {
            Assert.Throws<InvalidOperationException>(() => MultiValueDictionary<TKey, TValue>.Create<DummyReadOnlyCollection<TValue>>(1, (IEqualityComparer<TKey>)null));
            Assert.Throws<InvalidOperationException>(() => MultiValueDictionary<TKey, TValue>.Create<DummyReadOnlyCollection<TValue>>(1, (IEqualityComparer<TKey>)null, () => new DummyReadOnlyCollection<TValue>()));
        }

        #endregion    

        #region Tests for: Constructor5
        /*======================================================================
        ** Constructor 5 includes:
        **     -MultiValueDictionary(IEnumerable<KeyValuePair<TKey, IReadOnlyCollection<TValue>>> enumerable) 
        **     -MultiValueDictionary.Create<TValueCollection>(IEnumerable<KeyValuePair<TKey, IReadOnlyCollection<TValue>>> enumerable)
        **     -MultiValueDictionary.Create<TValueCollection>(IEnumerable<KeyValuePair<TKey, IReadOnlyCollection<TValue>>> enumerable, Func<ICollection<TValue>> collectionFactory)
        ======================================================================*/

        private delegate MultiValueDictionary<TKey, TValue> Constructor5Delegate<TKey, TValue>(IEnumerable<KeyValuePair<TKey, IReadOnlyCollection<TValue>>> enumerable);
        private Tuple<Constructor5Delegate<TKey, TValue>, Type>[] Constructor5<TKey, TValue>()
        {
            var ret = new Tuple<Constructor5Delegate<TKey, TValue>, Type>[5];
            ret[0] = Tuple.Create<Constructor5Delegate<TKey, TValue>, Type>((IEnumerable<KeyValuePair<TKey, IReadOnlyCollection<TValue>>> enumerable) => new MultiValueDictionary<TKey, TValue>(enumerable), typeof(List<TValue>));
            ret[1] = Tuple.Create<Constructor5Delegate<TKey, TValue>, Type>((IEnumerable<KeyValuePair<TKey, IReadOnlyCollection<TValue>>> enumerable) => MultiValueDictionary<TKey, TValue>.Create<List<TValue>>(enumerable), typeof(List<TValue>));
            ret[2] = Tuple.Create<Constructor5Delegate<TKey, TValue>, Type>((IEnumerable<KeyValuePair<TKey, IReadOnlyCollection<TValue>>> enumerable) => MultiValueDictionary<TKey, TValue>.Create<HashSet<TValue>>(enumerable), typeof(HashSet<TValue>));
            ret[3] = Tuple.Create<Constructor5Delegate<TKey, TValue>, Type>((IEnumerable<KeyValuePair<TKey, IReadOnlyCollection<TValue>>> enumerable) => MultiValueDictionary<TKey, TValue>.Create<List<TValue>>(enumerable, () => new List<TValue>()), typeof(List<TValue>));
            ret[4] = Tuple.Create<Constructor5Delegate<TKey, TValue>, Type>((IEnumerable<KeyValuePair<TKey, IReadOnlyCollection<TValue>>> enumerable) => MultiValueDictionary<TKey, TValue>.Create<HashSet<TValue>>(enumerable, () => new HashSet<TValue>()), typeof(HashSet<TValue>));
            return ret;
        }

        public void MVD_Constructor5_EmptyIEnumerable<TKey, TValue>()
        {
            foreach (var tuple in Constructor5<TKey, TValue>())
            {
                var enumerable = new MultiValueDictionary<TKey, TValue>();
                var mvd = tuple.Item1(enumerable);
                CompareMVDs<TKey, TValue>(enumerable, mvd);

                var newKey = (TKey)TypeBuilder<TKey>();
                var newValue = (TValue)TypeBuilder<TValue>();
                while (mvd.Contains(newKey, newValue))
                    newKey = (TKey)TypeBuilder<TKey>();
                mvd.Add(newKey, newValue);

                CompareEnumerables<KeyValuePair<TKey, IReadOnlyCollection<TValue>>>(enumerable, mvd, false);
                Assert.Equal(PairsCount<TKey, TValue>(enumerable) + 1, PairsCount<TKey, TValue>(mvd));
            }
        }

        public void MVD_Constructor5_NullIEnumerable<TKey, TValue>()
        {
            foreach (var tuple in Constructor5<TKey, TValue>())
            {
                IEnumerable<KeyValuePair<TKey, IReadOnlyCollection<TValue>>> enumerable = null;
                Assert.Throws<ArgumentNullException>(() =>
                {
                    var mvd = tuple.Item1(enumerable);
                });
            }
        }

        public void MVD_Constructor5_NonEmptyIEnumerable<TKey, TValue>()
        {
            foreach (var tuple in Constructor5<TKey, TValue>())
            {
                var enumerable = Multi<TKey, TValue, HashSet<TValue>>()[1];
                var mvd = tuple.Item1(enumerable);
                CompareMVDs<TKey, TValue>(enumerable, mvd);

                var newKey = (TKey)TypeBuilder<TKey>();
                var newValue = (TValue)TypeBuilder<TValue>();
                while (mvd.Contains(newKey, newValue))
                    newKey = (TKey)TypeBuilder<TKey>();
                mvd.Add(newKey, newValue);

                CompareEnumerables<KeyValuePair<TKey, IReadOnlyCollection<TValue>>>(enumerable, mvd, false);
                Assert.Equal(PairsCount<TKey, TValue>(enumerable) + 1, PairsCount<TKey, TValue>(mvd));
            }
        }

        public void MVD_Constructor5_ValueCopyIEnumerable<TKey, TValue>()
        {
            foreach (var tuple in Constructor5<TKey, TValue>())
            {
                var enumerable = Multi<TKey, TValue, HashSet<TValue>>()[1];
                var mvd = tuple.Item1(enumerable);
                CompareMVDs<TKey, TValue>(enumerable, mvd);

                var newKey = (TKey)TypeBuilder<TKey>();
                var newValue = (TValue)TypeBuilder<TValue>();
                while (enumerable.Contains(newKey, newValue))
                    newKey = (TKey)TypeBuilder<TKey>();
                enumerable.Add(newKey, newValue);

                CompareEnumerables<KeyValuePair<TKey, IReadOnlyCollection<TValue>>>(enumerable, mvd, false);
                Assert.Equal(PairsCount<TKey, TValue>(mvd) + 1, PairsCount<TKey, TValue>(enumerable));
            }
        }

        public void MVD_Constructor5_InvalidTCollection<TKey, TValue>()
        {
            Assert.Throws<InvalidOperationException>(() => MultiValueDictionary<TKey, TValue>.Create<DummyReadOnlyCollection<TValue>>(new List<KeyValuePair<TKey, IReadOnlyCollection<TValue>>>(), () => new DummyReadOnlyCollection<TValue>()));
            Assert.Throws<InvalidOperationException>(() => MultiValueDictionary<TKey, TValue>.Create<DummyReadOnlyCollection<TValue>>(new List<KeyValuePair<TKey, IReadOnlyCollection<TValue>>>()));
        }

        #endregion

        #region Tests for: Constructor6
        /*======================================================================
        ** Constructor 6 includes:
        **     -MultiValueDictionary(IEnumerable<KeyValuePair<TKey, IReadOnlyCollection<TValue>>> enumerable, IEqualityComparer<TKey> comparer) 
        **     -MultiValueDictionary.Create<TValueCollection>(IEnumerable<KeyValuePair<TKey, IReadOnlyCollection<TValue>>> enumerable, IEqualityComparer<TKey> comparer)
        **     -MultiValueDictionary.Create<TValueCollection>(IEnumerable<KeyValuePair<TKey, IReadOnlyCollection<TValue>>> enumerable, IEqualityComparer<TKey> comparer, Func<ICollection<TValue>> collectionFactory)
        ======================================================================*/

        private delegate MultiValueDictionary<TKey, TValue> Constructor6Delegate<TKey, TValue>(IEnumerable<KeyValuePair<TKey, IReadOnlyCollection<TValue>>> enumerable, IEqualityComparer<TKey> comparer);
        private Tuple<Constructor6Delegate<TKey, TValue>, Type>[] Constructor6<TKey, TValue>()
        {
            var ret = new Tuple<Constructor6Delegate<TKey, TValue>, Type>[5];
            ret[0] = Tuple.Create<Constructor6Delegate<TKey, TValue>, Type>((IEnumerable<KeyValuePair<TKey, IReadOnlyCollection<TValue>>> enumerable, IEqualityComparer<TKey> comparer) => new MultiValueDictionary<TKey, TValue>(enumerable, comparer), typeof(List<TValue>));
            ret[1] = Tuple.Create<Constructor6Delegate<TKey, TValue>, Type>((IEnumerable<KeyValuePair<TKey, IReadOnlyCollection<TValue>>> enumerable, IEqualityComparer<TKey> comparer) => MultiValueDictionary<TKey, TValue>.Create<List<TValue>>(enumerable, comparer), typeof(List<TValue>));
            ret[2] = Tuple.Create<Constructor6Delegate<TKey, TValue>, Type>((IEnumerable<KeyValuePair<TKey, IReadOnlyCollection<TValue>>> enumerable, IEqualityComparer<TKey> comparer) => MultiValueDictionary<TKey, TValue>.Create<HashSet<TValue>>(enumerable, comparer), typeof(HashSet<TValue>));
            ret[3] = Tuple.Create<Constructor6Delegate<TKey, TValue>, Type>((IEnumerable<KeyValuePair<TKey, IReadOnlyCollection<TValue>>> enumerable, IEqualityComparer<TKey> comparer) => MultiValueDictionary<TKey, TValue>.Create<List<TValue>>(enumerable, comparer, () => new List<TValue>()), typeof(List<TValue>));
            ret[4] = Tuple.Create<Constructor6Delegate<TKey, TValue>, Type>((IEnumerable<KeyValuePair<TKey, IReadOnlyCollection<TValue>>> enumerable, IEqualityComparer<TKey> comparer) => MultiValueDictionary<TKey, TValue>.Create<HashSet<TValue>>(enumerable, comparer, () => new HashSet<TValue>()), typeof(HashSet<TValue>));
            return ret;
        }

        public void MVD_Constructor6_NullIEnumerable<TKey, TValue>()
        {
            foreach (var tuple in Constructor6<TKey, TValue>())
            {
                IEnumerable<KeyValuePair<TKey, IReadOnlyCollection<TValue>>> enumerable = null;
                Assert.Throws<ArgumentNullException>(() =>
                {
                    var mvd = tuple.Item1(enumerable, null);
                });
            }
        }

        public void MVD_Constructor6_EmptyIEnumerable<TKey, TValue>()
        {
            foreach (var tuple in Constructor6<TKey, TValue>())
            {
                var enumerable = new MultiValueDictionary<TKey, TValue>();
                var mvd = tuple.Item1(enumerable, null);
                CompareMVDs<TKey, TValue>(enumerable, mvd);

                var newKey = (TKey)TypeBuilder<TKey>();
                var newValue = (TValue)TypeBuilder<TValue>();
                while (mvd.Contains(newKey, newValue))
                    newKey = (TKey)TypeBuilder<TKey>();
                mvd.Add(newKey, newValue);

                Assert.Equal(PairsCount<TKey, TValue>(enumerable) + 1, PairsCount<TKey, TValue>(mvd));
            }
        }

        public void MVD_Constructor6_NonEmptyIEnumerable<TKey, TValue>()
        {
            foreach (var tuple in Constructor6<TKey, TValue>())
            {
                var enumerable = Multi<TKey, TValue, HashSet<TValue>>()[1];
                var mvd = tuple.Item1(enumerable, null);
                CompareMVDs<TKey, TValue>(enumerable, mvd);

                var newKey = (TKey)TypeBuilder<TKey>();
                var newValue = (TValue)TypeBuilder<TValue>();
                while (mvd.Contains(newKey, newValue))
                    newKey = (TKey)TypeBuilder<TKey>();
                mvd.Add(newKey, newValue);

                CompareEnumerables<KeyValuePair<TKey, IReadOnlyCollection<TValue>>>(enumerable, mvd, false);
                Assert.Equal(PairsCount<TKey, TValue>(enumerable) + 1, PairsCount<TKey, TValue>(mvd));
            }
        }

        public void MVD_Constructor6_ValueCopyIEnumerable<TKey, TValue>()
        {
            foreach (var tuple in Constructor6<TKey, TValue>())
            {
                var enumerable = Multi<TKey, TValue, HashSet<TValue>>()[1];
                var mvd = tuple.Item1(enumerable, null);
                CompareMVDs<TKey, TValue>(enumerable, mvd);

                var newKey = (TKey)TypeBuilder<TKey>();
                var newValue = (TValue)TypeBuilder<TValue>();
                while (enumerable.Contains(newKey, newValue))
                    newKey = (TKey)TypeBuilder<TKey>();
                enumerable.Add(newKey, newValue);

                CompareEnumerables<KeyValuePair<TKey, IReadOnlyCollection<TValue>>>(enumerable, mvd, false);
                Assert.Equal(PairsCount<TKey, TValue>(mvd) + 1, PairsCount<TKey, TValue>(enumerable));
            }
        }

        public void MVD_Constructor6_DummyComparer<TKey, TValue>()
        {
            foreach (var tuple in Constructor6<TKey, TValue>())
            {
                Assert.Throws<InvalidOperationException>(() =>
                {
                    var enumerable = new MultiValueDictionary<TKey, TValue>();
                    var mvd = tuple.Item1(enumerable, new dummyComparer<TKey>());
                    mvd.Add((TKey)TypeBuilder<TKey>(), (TValue)TypeBuilder<TValue>());
                });
            }
        }

        public void MVD_Constructor6_InvalidTCollection<TKey, TValue>()
        {
            Assert.Throws<InvalidOperationException>(() => MultiValueDictionary<TKey, TValue>.Create<DummyReadOnlyCollection<TValue>>(new List<KeyValuePair<TKey, IReadOnlyCollection<TValue>>>(), null, () => new DummyReadOnlyCollection<TValue>()));
            Assert.Throws<InvalidOperationException>(() => MultiValueDictionary<TKey, TValue>.Create<DummyReadOnlyCollection<TValue>>(new List<KeyValuePair<TKey, IReadOnlyCollection<TValue>>>(), (IEqualityComparer<TKey>)null));
        }

        #endregion

        #region Tests for: Add
        /*======================================================================
        ** Add includes:
        **     -multiDictionary.Add(TKey key, TValue value)
        ======================================================================*/

        private delegate void AddDelegate<TKey, TValue>(MultiValueDictionary<TKey, TValue> mvd, TKey key, TValue value);
        private AddDelegate<TKey, TValue>[] Add<TKey, TValue>()
        {
            var ret = new AddDelegate<TKey, TValue>[1];
            ret[0] = (MultiValueDictionary<TKey, TValue> mvd, TKey key, TValue value) => mvd.Add(key, value);
            return ret;
        }

        [Fact]
        public void MVD_Add_NullKey()
        {
            foreach (var add in Add<string, int>())
            {
                foreach (var mvd in Multi<string, int, List<int>>())
                {
                    string newKey = null;
                    int newValue = (int)TypeBuilder<int>();
                    Assert.Throws<ArgumentNullException>(() =>
                    {
                        add(mvd, newKey, newValue);
                    });
                }
                foreach (var mvd in Multi<string, int, HashSet<int>>())
                {
                    string newKey = null;
                    int newValue = (int)TypeBuilder<int>();
                    Assert.Throws<ArgumentNullException>(() =>
                    {
                        add(mvd, newKey, newValue);
                    });
                }
            }
        }

        public void MVD_Add_ValidKeyNullValue<TKey, TValue>()
        {
            foreach (var add in Add<TKey, String>())
            {
                foreach (var mvd in Multi<TKey, String, List<String>>())
                {
                    TKey newKey = (TKey)TypeBuilder<TKey>();
                    String newValue = null;

                    mvd.Remove(newKey, newValue);
                    int oldCount = PairsCount<TKey, String>(mvd);
                    add(mvd, newKey, newValue);
                    Assert.Equal(oldCount + 1, PairsCount<TKey, String>(mvd));
                }
                foreach (var mvd in Multi<TKey, String, HashSet<String>>())
                {
                    TKey newKey = (TKey)TypeBuilder<TKey>();
                    String newValue = null;

                    mvd.Remove(newKey, newValue);
                    int oldCount = PairsCount<TKey, String>(mvd);
                    add(mvd, newKey, newValue);
                    Assert.Equal(oldCount + 1, PairsCount<TKey, String>(mvd));
                }
            }
        }

        public void MVD_Add_ValidKeyValidValue<TKey, TValue>()
        {
            foreach (var add in Add<TKey, TValue>())
            {
                foreach (var mvd in Multi<TKey, TValue, List<TValue>>())
                {
                    TKey newKey = (TKey)TypeBuilder<TKey>();
                    TValue newValue = (TValue)TypeBuilder<TValue>();

                    mvd.Remove(newKey, newValue);
                    int oldCount = PairsCount<TKey, TValue>(mvd);
                    add(mvd, newKey, newValue);
                    Assert.Equal(oldCount + 1, PairsCount<TKey, TValue>(mvd));
                }
                foreach (var mvd in Multi<TKey, TValue, HashSet<TValue>>())
                {
                    TKey newKey = (TKey)TypeBuilder<TKey>();
                    TValue newValue = (TValue)TypeBuilder<TValue>();

                    mvd.Remove(newKey, newValue);
                    int oldCount = PairsCount<TKey, TValue>(mvd);
                    add(mvd, newKey, newValue);
                    Assert.Equal(oldCount + 1, PairsCount<TKey, TValue>(mvd));
                }
            }
        }

        public void MVD_Add_AlreadyPresentPair<TKey, TValue>()
        {
            foreach (var add in Add<TKey, TValue>())
            {
                foreach (var mvd in Multi<TKey, TValue, List<TValue>>())
                {
                    TKey newKey = (TKey)TypeBuilder<TKey>();
                    TValue newValue = (TValue)TypeBuilder<TValue>();

                    mvd.Remove(newKey, newValue);
                    int oldCount = PairsCount<TKey, TValue>(mvd);
                    add(mvd, newKey, newValue);
                    add(mvd, newKey, newValue);
                    add(mvd, newKey, newValue);
                    Assert.Equal(oldCount + 3, PairsCount<TKey, TValue>(mvd));
                }
                foreach (var mvd in Multi<TKey, TValue, HashSet<TValue>>())
                {
                    TKey newKey = (TKey)TypeBuilder<TKey>();
                    TValue newValue = (TValue)TypeBuilder<TValue>();

                    mvd.Remove(newKey, newValue);
                    int oldCount = PairsCount<TKey, TValue>(mvd);
                    add(mvd, newKey, newValue);
                    add(mvd, newKey, newValue);
                    Assert.Equal(oldCount + 1, PairsCount<TKey, TValue>(mvd));
                }
            }
        }

        #endregion

        #region Tests for: AddRange
        /*======================================================================
        ** AddRange includes:
        **     -multiDictionary.AddRange(TKey key, IEnumerable<TValue> values)
        ======================================================================*/

        private delegate void AddRangeDelegate<TKey, TValue>(MultiValueDictionary<TKey, TValue> mvd, TKey key, IEnumerable<TValue> values);
        private AddRangeDelegate<TKey, TValue>[] AddRange<TKey, TValue>()
        {
            var ret = new AddRangeDelegate<TKey, TValue>[1];
            ret[0] = (MultiValueDictionary<TKey, TValue> mvd, TKey key, IEnumerable<TValue> values) => mvd.AddRange(key, values);
            return ret;
        }

        [Fact]
        public void MVD_AddRange_NullKey()
        {
            foreach (var addRange in AddRange<string, int>())
            {
                foreach (var mvd in Multi<string, int, List<int>>())
                {
                    string newKey = null;
                    var newValues = CreateRange<int, List<int>>((int)TypeBuilder<int>());
                    int oldCount = mvd.Count;
                    Assert.Throws<ArgumentNullException>(() =>
                    {
                        addRange(mvd, newKey, newValues);
                    });
                    Assert.Equal(oldCount, mvd.Count);
                }
                foreach (var mvd in Multi<string, int, HashSet<int>>())
                {
                    string newKey = null;
                    var newValues = CreateRange<int, HashSet<int>>((int)TypeBuilder<int>());
                    int oldCount = mvd.Count;

                    Assert.Throws<ArgumentNullException>(() =>
                    {
                        addRange(mvd, newKey, newValues);
                    });
                    Assert.Equal(oldCount, mvd.Count);

                }
            }
        }

        public void MVD_AddRange_NullValues<TKey, TValue>()
        {
            foreach (var addRange in AddRange<TKey, int>())
            {
                foreach (var mvd in Multi<TKey, int, List<int>>())
                {
                    TKey newKey = (TKey)TypeBuilder<TKey>();
                    List<int> newValues = null;
                    int oldCount = mvd.Count;

                    Assert.Throws<ArgumentNullException>(() =>
                    {
                        addRange(mvd, newKey, newValues);
                    });

                    Assert.Equal(oldCount, mvd.Count);
                }
                foreach (var mvd in Multi<TKey, int, HashSet<int>>())
                {
                    TKey newKey = (TKey)TypeBuilder<TKey>();
                    HashSet<int> newValues = null;
                    int oldCount = mvd.Count;

                    Assert.Throws<ArgumentNullException>(() =>
                    {
                        addRange(mvd, newKey, newValues);
                    });

                    Assert.Equal(oldCount, mvd.Count);

                }
            }
        }

        public void MVD_AddRange_ValidKeyEmptyValues<TKey, TValue>()
        {
            foreach (var addRange in AddRange<TKey, TValue>())
            {
                foreach (var mvd in Multi<TKey, TValue, List<TValue>>())
                {
                    TKey newKey = (TKey)TypeBuilder<TKey>();
                    var newValues = new List<TValue>();
                    int oldCount = PairsCount<TKey, TValue>(mvd);

                    addRange(mvd, newKey, newValues);

                    Assert.Equal(oldCount, PairsCount<TKey, TValue>(mvd));
                }
                foreach (var mvd in Multi<TKey, TValue, HashSet<TValue>>())
                {
                    TKey newKey = (TKey)TypeBuilder<TKey>();
                    var newValues = new HashSet<TValue>();
                    int oldCount = PairsCount<TKey, TValue>(mvd);

                    addRange(mvd, newKey, newValues);

                    Assert.Equal(oldCount, PairsCount<TKey, TValue>(mvd));
                }
            }
        }

        public void MVD_AddRange_ValidKeyNonEmptyValues<TKey, TValue>()
        {
            foreach (var addRange in AddRange<TKey, TValue>())
            {
                foreach (var mvd in Multi<TKey, TValue, List<TValue>>())
                {
                    TKey newKey = (TKey)TypeBuilder<TKey>();
                    var newValues = CreateRange<TValue, List<TValue>>((int)TypeBuilder<int>());
                    int oldCount = PairsCount<TKey, TValue>(mvd);

                    addRange(mvd, newKey, newValues);

                    Assert.Equal(oldCount + newValues.Count, PairsCount<TKey, TValue>(mvd));
                }
                foreach (var mvd in Multi<TKey, TValue, HashSet<TValue>>())
                {
                    TKey newKey = (TKey)TypeBuilder<TKey>();
                    var newValues = CreateRange<TValue, HashSet<TValue>>((int)TypeBuilder<int>());
                    mvd.Remove(newKey);
                    int oldCount = PairsCount<TKey, TValue>(mvd);

                    addRange(mvd, newKey, newValues);

                    Assert.Equal(oldCount + newValues.Count, PairsCount<TKey, TValue>(mvd));
                }
            }
        }

        public void MVD_AddRange_ValidKeyDuplicateValues<TKey, TValue>()
        {
            foreach (var addRange in AddRange<TKey, TValue>())
            {
                foreach (var mvd in Multi<TKey, TValue, List<TValue>>())
                {
                    TKey newKey = (TKey)TypeBuilder<TKey>();
                    TValue newValue = (TValue)TypeBuilder<TValue>();
                    var newValues = new List<TValue>();
                    newValues.Add(newValue);
                    newValues.Add(newValue);
                    newValues.Add(newValue);
                    newValues.Add(newValue);
                    newValues.Add(newValue);

                    int oldCount = PairsCount<TKey, TValue>(mvd);

                    addRange(mvd, newKey, newValues);

                    Assert.Equal(oldCount + newValues.Count, PairsCount<TKey, TValue>(mvd));
                }
                foreach (var mvd in Multi<TKey, TValue, HashSet<TValue>>())
                {
                    TKey newKey = (TKey)TypeBuilder<TKey>();
                    TValue newValue = (TValue)TypeBuilder<TValue>();
                    while (mvd.Contains(newKey, newValue))
                        newValue = (TValue)TypeBuilder<TValue>();
                    var newValues = new List<TValue>();
                    newValues.Add(newValue);
                    newValues.Add(newValue);
                    newValues.Add(newValue);
                    newValues.Add(newValue);
                    newValues.Add(newValue);

                    int oldCount = PairsCount<TKey, TValue>(mvd);

                    addRange(mvd, newKey, newValues);

                    Assert.Equal(oldCount + 1, PairsCount<TKey, TValue>(mvd));
                }
            }
        }

        public void MVD_AddRange_PreexistingKey<TKey, TValue>()
        {
            foreach (var addRange in AddRange<TKey, TValue>())
            {
                foreach (var mvd in Multi<TKey, TValue, List<TValue>>())
                {
                    TKey newKey = (TKey)TypeBuilder<TKey>();
                    mvd.Remove(newKey);
                    var newValues = CreateRange<TValue, List<TValue>>((int)TypeBuilder<int>());
                    var preValue = (TValue)TypeBuilder<TValue>();
                    while (newValues.Contains(preValue))
                        preValue = (TValue)TypeBuilder<TValue>();
                    mvd.Add(newKey, preValue);
                    int oldCount = PairsCount<TKey, TValue>(mvd);

                    addRange(mvd, newKey, newValues);

                    Assert.Equal(oldCount + newValues.Count, PairsCount<TKey, TValue>(mvd));
                    newValues.Add(preValue);
                    CompareEnumerables<TValue>(mvd[newKey], newValues, true);
                }
                foreach (var mvd in Multi<TKey, TValue, HashSet<TValue>>())
                {
                    TKey newKey = (TKey)TypeBuilder<TKey>();
                    mvd.Remove(newKey);
                    var newValues = CreateRange<TValue, HashSet<TValue>>((int)TypeBuilder<int>());
                    var preValue = (TValue)TypeBuilder<TValue>();
                    while (newValues.Contains(preValue))
                        preValue = (TValue)TypeBuilder<TValue>();
                    mvd.Add(newKey, preValue);
                    int oldCount = PairsCount<TKey, TValue>(mvd);

                    addRange(mvd, newKey, newValues);

                    Assert.Equal(oldCount + newValues.Count, PairsCount<TKey, TValue>(mvd));
                    newValues.Add(preValue);
                    CompareEnumerables<TValue>(mvd[newKey], newValues, true);
                }
            }
        }

        #endregion

        #region Tests for: Clear
        /*======================================================================
        ** Clear includes:
        **      -multiDictionary.Clear()
        ======================================================================*/

        private delegate void ClearDelegate<TKey, TValue>(MultiValueDictionary<TKey, TValue> mvd);
        private ClearDelegate<TKey, TValue>[] Clear<TKey, TValue>()
        {
            var ret = new ClearDelegate<TKey, TValue>[1];
            ret[0] = (MultiValueDictionary<TKey, TValue> mvd) => mvd.Clear();
            return ret;
        }

        public void MVD_Values_Validity<TKey, TValue>()
        {
            foreach (var clear in Clear<TKey, TValue>())
            {
                foreach (var mvd in Multi<TKey, TValue, List<TValue>>())
                {
                    mvd.Clear();
                    Assert.Empty(mvd);
                    Assert.Empty(mvd.Keys);
                    Assert.Empty(mvd.Values);
                    Assert.Equal(PairsCount<TKey, TValue>(mvd), 0);
                    Assert.Equal(mvd.Count, 0);
                }
            }
        }

        #endregion

        #region Tests for: Contains
        /*======================================================================
        ** Contains includes:
        **      -multiDictionary.Contains(TKey key, TValue value)
        ======================================================================*/

        private delegate bool ContainsDelegate<TKey, TValue>(MultiValueDictionary<TKey, TValue> mvd, TKey key, TValue value);
        private ContainsDelegate<TKey, TValue>[] Contains<TKey, TValue>()
        {
            var ret = new ContainsDelegate<TKey, TValue>[1];
            ret[0] = (MultiValueDictionary<TKey, TValue> mvd, TKey key, TValue value) => mvd.Contains(key, value);
            return ret;
        }

        public void MVD_Contains_NullKey<TKey, TValue>()
        {
            foreach (var contains in Contains<string, TValue>())
            {
                foreach (var mvd in Multi<string, TValue, List<TValue>>())
                {
                    string newKey = null;

                    Assert.Throws<ArgumentNullException>(() =>
                    {
                        contains(mvd, newKey, (TValue)TypeBuilder<TValue>());
                    });
                }
            }
        }

        public void MVD_Contains_ValidKeyInvalidValue<TKey, TValue>()
        {
            foreach (var contains in Contains<TKey, TValue>())
            {
                foreach (var mvd in Multi<TKey, TValue, List<TValue>>())
                {
                    TKey newKey = (TKey)TypeBuilder<TKey>();
                    TValue newValue1 = (TValue)TypeBuilder<TValue>();
                    TValue newValue2 = (TValue)TypeBuilder<TValue>();
                    while (newValue1.Equals(newValue2))
                        newValue2 = (TValue)TypeBuilder<TValue>();
                    mvd.Add(newKey, newValue1);
                    while (mvd.Contains(newKey, newValue2))
                        mvd.Remove(newKey, newValue2);

                    Assert.True(contains(mvd, newKey, newValue1));
                    Assert.False(contains(mvd, newKey, newValue2));
                }
            }
        }

        #endregion

        #region Tests for: ContainsKey
        /*======================================================================
        ** ContainsKey includes:
        **      -multiDictionary.ContainsKey(TKey key)
        ======================================================================*/

        private delegate bool ContainsKeyDelegate<TKey, TValue>(MultiValueDictionary<TKey, TValue> mvd, TKey key);
        private ContainsKeyDelegate<TKey, TValue>[] ContainsKey<TKey, TValue>()
        {
            var ret = new ContainsKeyDelegate<TKey, TValue>[1];
            ret[0] = (MultiValueDictionary<TKey, TValue> mvd, TKey key) => mvd.ContainsKey(key);
            return ret;
        }

        public void MVD_ContainsKey_NullKey<TKey, TValue>()
        {
            foreach (var contains in ContainsKey<string, TValue>())
            {
                foreach (var mvd in Multi<string, TValue, List<TValue>>())
                {
                    string newKey = null;

                    Assert.Throws<ArgumentNullException>(() =>
                    {
                        contains(mvd, newKey);
                    });
                }
            }
        }

        public void MVD_ContainsKey_ValidNonEmptyKey<TKey, TValue>()
        {
            foreach (var contains in ContainsKey<TKey, TValue>())
            {
                foreach (var mvd in Multi<TKey, TValue, List<TValue>>())
                {
                    TKey newKey = (TKey)TypeBuilder<TKey>();
                    TValue newValue = (TValue)TypeBuilder<TValue>();
                    mvd.Add(newKey, newValue);

                    Assert.True(contains(mvd, newKey));
                }
            }
        }

        public void MVD_ContainsKey_PreviouslyValidKey<TKey, TValue>()
        {
            foreach (var contains in ContainsKey<TKey, TValue>())
            {
                foreach (var mvd in Multi<TKey, TValue, List<TValue>>())
                {
                    TKey newKey = (TKey)TypeBuilder<TKey>();
                    TValue newValue = (TValue)TypeBuilder<TValue>();
                    mvd.Add(newKey, newValue);
                    mvd.Remove(newKey);
                    Assert.False(contains(mvd, newKey));

                    mvd.Add(newKey, newValue);
                    mvd.Remove(newKey, newValue);
                    Assert.False(contains(mvd, newKey));
                }
            }
        }

        public void MVD_ContainsKey_EmptyKey<TKey, TValue>()
        {
            foreach (var contains in ContainsKey<TKey, TValue>())
            {
                foreach (var mvd in Multi<TKey, TValue, List<TValue>>())
                {
                    TKey newKey = (TKey)TypeBuilder<TKey>();
                    mvd.Remove(newKey);
                    Assert.False(contains(mvd, newKey));
                }
            }
        }

        #endregion

        #region Tests for: ContainsValue
        /*======================================================================
        ** ContainsValue includes:
        **      -multiDictionary.ContainsValue(TValue value)
        ======================================================================*/

        private delegate bool ContainsValueDelegate<TKey, TValue>(MultiValueDictionary<TKey, TValue> mvd, TValue value);
        private ContainsValueDelegate<TKey, TValue>[] ContainsValue<TKey, TValue>()
        {
            var ret = new ContainsValueDelegate<TKey, TValue>[1];
            ret[0] = (MultiValueDictionary<TKey, TValue> mvd, TValue value) => mvd.ContainsValue(value);
            return ret;
        }

        public void MVD_ContainsValue_NullValidValue<TKey, TValue>()
        {
            foreach (var contains in ContainsValue<TKey, string>())
            {
                foreach (var mvd in Multi<TKey, string, List<string>>())
                {
                    TKey newKey = (TKey)TypeBuilder<TKey>();
                    string newValue = null;
                    mvd.Add(newKey, newValue);
                    Assert.True(contains(mvd, null));
                }
            }
        }

        public void MVD_ContainsValue_NonExistentValue<TKey, TValue>()
        {
            foreach (var contains in ContainsValue<TKey, TValue>())
            {
                foreach (var mvd in Multi<TKey, TValue, List<TValue>>())
                {
                    TValue newValue1 = (TValue)TypeBuilder<TValue>();
                    var keys = new List<TKey>(mvd.Keys);
                    foreach (var key in keys)
                        while (mvd.Contains(key, newValue1))
                            mvd.Remove(key, newValue1);

                    Assert.False(contains(mvd, newValue1));
                }
            }
        }

        public void MVD_ContainsValue_ValidValueExists<TKey, TValue>()
        {
            foreach (var contains in ContainsValue<TKey, TValue>())
            {
                foreach (var mvd in Multi<TKey, TValue, List<TValue>>())
                {
                    TKey newKey = (TKey)TypeBuilder<TKey>();
                    TValue newValue = (TValue)TypeBuilder<TValue>();
                    mvd.Add(newKey, newValue);
                    Assert.True(contains(mvd, newValue));
                }
            }
        }

        public void MVD_ContainsValue_ValidValueExistsMultipleTimes<TKey, TValue>()
        {
            foreach (var contains in ContainsValue<TKey, TValue>())
            {
                foreach (var mvd in Multi<TKey, TValue, List<TValue>>())
                {
                    TKey newKey1 = (TKey)TypeBuilder<TKey>();
                    TKey newKey2 = (TKey)TypeBuilder<TKey>();
                    while (newKey1.Equals(newKey2))
                        newKey2 = (TKey)TypeBuilder<TKey>();
                    TValue newValue = (TValue)TypeBuilder<TValue>();
                    mvd.Add(newKey1, newValue);
                    mvd.Add(newKey1, newValue);
                    mvd.Add(newKey2, newValue);
                    Assert.True(contains(mvd, newValue));
                }
            }
        }

        #endregion

        #region Tests for: Count
        /*======================================================================
        ** Count includes:
        **      -multiDictionary.Count
        ======================================================================*/

        private delegate int CountDelegate<TKey, TValue>(MultiValueDictionary<TKey, TValue> mvd);
        private CountDelegate<TKey, TValue>[] Count<TKey, TValue>()
        {
            var ret = new CountDelegate<TKey, TValue>[1];
            ret[0] = (MultiValueDictionary<TKey, TValue> mvd) => mvd.Count;
            return ret;
        }

        public void MVD_Count_Empty<TKey, TValue>()
        {
            foreach (var count in Count<TKey, TValue>())
            {
                foreach (var mvd in Multi<TKey, TValue, List<TValue>>())
                {
                    mvd.Clear();
                    Assert.Equal(0, count(mvd));
                }
            }
        }

        public void MVD_Count_NonEmpty<TKey, TValue>()
        {
            foreach (var count in Count<TKey, TValue>())
            {
                foreach (var mvd in Multi<TKey, TValue, List<TValue>>())
                {
                    var key1 = (TKey)TypeBuilder<TKey>();
                    var key2 = (TKey)TypeBuilder<TKey>();
                    var key3 = (TKey)TypeBuilder<TKey>();
                    while (key1.Equals(key2) || key1.Equals(key3) || key2.Equals(key3))
                    {
                        key1 = (TKey)TypeBuilder<TKey>();
                        key2 = (TKey)TypeBuilder<TKey>();
                        key3 = (TKey)TypeBuilder<TKey>();
                    }

                    mvd.Clear();
                    mvd.Add(key1, (TValue)TypeBuilder<TValue>());
                    mvd.Add(key2, (TValue)TypeBuilder<TValue>());
                    mvd.Add(key3, (TValue)TypeBuilder<TValue>());

                    Assert.Equal(3, count(mvd));
                }
            }
        }

        #endregion

        #region Tests for: GetEnumerator
        /*======================================================================
        ** GetEnumerator includes:
        **      -multiDictionary.GetEnumerator()
        ======================================================================*/

        private delegate IEnumerator GetEnumeratorDelegate<TKey, TValue>(MultiValueDictionary<TKey, TValue> mvd);
        private GetEnumeratorDelegate<TKey, TValue>[] GetEnumerator<TKey, TValue>()
        {
            var ret = new GetEnumeratorDelegate<TKey, TValue>[1];
            ret[0] = (MultiValueDictionary<TKey, TValue> mvd) => mvd.GetEnumerator();
            return ret;
        }

        public void MVD_GetEnumerator_ValidReset<TKey, TValue>()
        {
            foreach (var enu in GetEnumerator<TKey, TValue>())
            {
                foreach (var mvd in Multi<TKey, TValue, List<TValue>>())
                {
                    var enumerator = enu(mvd);
                    for (int i = 0; i < mvd.Count; i++)
                        Assert.True(enumerator.MoveNext());
                    enumerator.Reset();
                    for (int i = 0; i < mvd.Count; i++)
                        Assert.True(enumerator.MoveNext());
                }
            }
        }

        public void MVD_GetEnumerator_ResetAfterModificationToMVD<TKey, TValue>()
        {
            foreach (var enu in GetEnumerator<TKey, TValue>())
            {
                foreach (var mvd in Multi<TKey, TValue, List<TValue>>())
                {
                    var enumerator = enu(mvd);
                    mvd.Add((TKey)TypeBuilder<TKey>(), (TValue)TypeBuilder<TValue>());
                    Assert.Throws<InvalidOperationException>(() => enumerator.Reset());
                }
            }
        }

        public void MVD_GetEnumerator_MoveNextAfterModificationToMVD<TKey, TValue>()
        {
            foreach (var enu in GetEnumerator<TKey, TValue>())
            {
                foreach (var mvd in Multi<TKey, TValue, List<TValue>>())
                {
                    var enumerator = enu(mvd);
                    mvd.Add((TKey)TypeBuilder<TKey>(), (TValue)TypeBuilder<TValue>());
                    Assert.Throws<InvalidOperationException>(() => enumerator.MoveNext());
                }
            }
        }

        public void MVD_GetEnumerator_Validity<TKey, TValue>()
        {
            foreach (var mvd in Multi<TKey, TValue, List<TValue>>())
                foreach (var pair in mvd)
                    CompareEnumerables<TValue>(pair.Value, mvd[pair.Key], true);
        }

        public void MVD_GetEnumerator_IEnumeratorValidity<TKey, TValue>()
        {
            foreach (var mvd in Multi<TKey, TValue, List<TValue>>())
            {
                IEnumerator enumerator = mvd.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    var current = (KeyValuePair<TKey, IReadOnlyCollection<TValue>>)enumerator.Current;
                    CompareEnumerables<TValue>(current.Value, mvd[current.Key], true);
                }
            }
        }

        public void MVD_GetEnumerator_IEnumeratorBeforeFirst<TKey, TValue>()
        {
            foreach (var enu in GetEnumerator<TKey, TValue>())
            {
                foreach (var mvd in Multi<TKey, TValue, List<TValue>>())
                {
                    IEnumerator enumerator = enu(mvd);
                    Assert.Throws<InvalidOperationException>(() => enumerator.Current);
                }
            }
        }

        public void MVD_GetEnumerator_IEnumeratorAfterLast<TKey, TValue>()
        {
            foreach (var enu in GetEnumerator<TKey, TValue>())
            {
                foreach (var mvd in Multi<TKey, TValue, List<TValue>>())
                {
                    IEnumerator enumerator = enu(mvd);
                    while (enumerator.MoveNext()) ;
                    Assert.Throws<InvalidOperationException>(() => enumerator.Current);
                }
            }
        }

        #endregion

        #region Tests for: Item
        /*======================================================================
        ** Item includes:
        **      -multiDictionary[TKey key]
        ======================================================================*/

        private delegate IEnumerable<TValue> ItemDelegate<TKey, TValue>(MultiValueDictionary<TKey, TValue> mvd, TKey key);
        private ItemDelegate<TKey, TValue>[] Item<TKey, TValue>()
        {
            var ret = new ItemDelegate<TKey, TValue>[1];
            ret[0] = (MultiValueDictionary<TKey, TValue> mvd, TKey key) => mvd[key];
            return ret;
        }

        public void MVD_Item_NullKey<TKey, TValue>()
        {
            foreach (var item in Item<string, TValue>())
            {
                foreach (var mvd in Multi<string, TValue, List<TValue>>())
                {
                    Assert.Throws<ArgumentNullException>(() =>
                    {
                        item(mvd, null);
                    });
                }
            }
        }

        public void MVD_Item_ExistentKey<TKey, TValue>()
        {
            foreach (var item in Item<TKey, TValue>())
            {
                foreach (var mvd in Multi<TKey, TValue, List<TValue>>())
                {
                    TKey newKey = (TKey)TypeBuilder<TKey>();
                    TValue newValue = (TValue)TypeBuilder<TValue>();

                    mvd.Remove(newKey);
                    mvd.Add(newKey, newValue);
                    CompareEnumerables<TValue>(item(mvd, newKey), new TValue[] { newValue }, true);
                }
            }
        }

        public void MVD_Item_ActiveView<TKey, TValue>()
        {
            foreach (var item in Item<TKey, TValue>())
            {
                foreach (var mvd in Multi<TKey, TValue, List<TValue>>())
                {
                    TKey newKey = (TKey)TypeBuilder<TKey>();
                    TValue newValue = (TValue)TypeBuilder<TValue>();

                    mvd.Remove(newKey);
                    mvd.Add(newKey, newValue);
                    var retCol = item(mvd, newKey);
                    mvd.Add(newKey, (TValue)TypeBuilder<TValue>());
                    mvd.Add(newKey, (TValue)TypeBuilder<TValue>());
                    mvd.Add(newKey, (TValue)TypeBuilder<TValue>());
                    mvd.Add(newKey, (TValue)TypeBuilder<TValue>());
                    mvd.Add(newKey, (TValue)TypeBuilder<TValue>());
                    CompareEnumerables(item(mvd, newKey), retCol, true);
                    Assert.Equal(retCol, item(mvd, newKey));
                }
            }
        }

        public void MVD_Item_NonExistentKey<TKey, TValue>()
        {
            ItemDelegate<TKey, TValue> item = (MultiValueDictionary<TKey, TValue> mvd, TKey key) => mvd[key];
            foreach (var mvd in Multi<TKey, TValue, List<TValue>>())
            {
                TKey key = (TKey)TypeBuilder<TKey>();
                mvd.Remove(key);
                Assert.Throws<KeyNotFoundException>(() =>
                {
                    item(mvd, key);
                });
            }
        }

        #endregion

        #region Tests for: Keys
        /*======================================================================
        ** Keys includes:
        **      -multiDictionary.Keys
        ======================================================================*/

        private delegate IEnumerable<TKey> KeysDelegate<TKey, TValue>(MultiValueDictionary<TKey, TValue> mvd);
        private KeysDelegate<TKey, TValue>[] Keys<TKey, TValue>()
        {
            var ret = new KeysDelegate<TKey, TValue>[1];
            ret[0] = (MultiValueDictionary<TKey, TValue> mvd) => mvd.Keys;
            return ret;
        }

        public void MVD_Keys_EmptyMVD<TKey, TValue>()
        {
            foreach (var keys in Keys<TKey, TValue>())
            {
                foreach (var mvd in Multi<TKey, TValue, List<TValue>>())
                {
                    mvd.Clear();
                    Assert.Empty(keys(mvd));
                }
            }
        }

        public void MVD_Keys_NonEmptyMVD<TKey, TValue>()
        {
            foreach (var keys in Keys<TKey, TValue>())
            {
                foreach (var mvd in Multi<TKey, TValue, List<TValue>>())
                {
                    TKey newKey = (TKey)TypeBuilder<TKey>();
                    TValue newValue = (TValue)TypeBuilder<TValue>();
                    mvd.Add(newKey, newValue);

                    Assert.NotEmpty(keys(mvd));
                    var keyList = new List<TKey>();
                    foreach (var pair in mvd)
                        keyList.Add(pair.Key);
                    CompareEnumerables<TKey>(keys(mvd), keyList, true);
                }
            }
        }

        #endregion

        #region Tests for: Remove
        /*======================================================================
        ** Remove includes:
        **      -multiDictionary.Remove(TKey key)
        ======================================================================*/

        private delegate bool RemoveDelegate<TKey, TValue>(MultiValueDictionary<TKey, TValue> mvd, TKey key);
        private RemoveDelegate<TKey, TValue>[] Remove<TKey, TValue>()
        {
            var ret = new RemoveDelegate<TKey, TValue>[1];
            ret[0] = (MultiValueDictionary<TKey, TValue> mvd, TKey key) => mvd.Remove(key);
            return ret;
        }

        public void MVD_Remove_NullKey<TKey, TValue>()
        {
            foreach (var remove in Remove<string, TValue>())
            {
                foreach (var mvd in Multi<string, TValue, List<TValue>>())
                {
                    string newKey = null;

                    int oldCount = mvd.Count;
                    Assert.Throws<ArgumentNullException>(() =>
                    {
                        remove(mvd, newKey);
                    });
                    Assert.Equal(oldCount, mvd.Count);
                }
                foreach (var mvd in Multi<string, TValue, HashSet<TValue>>())
                {
                    string newKey = null;

                    int oldCount = mvd.Count;
                    Assert.Throws<ArgumentNullException>(() =>
                    {
                        remove(mvd, newKey);
                    });
                    Assert.Equal(oldCount, mvd.Count);
                }
            }
        }

        public void MVD_Remove_NonexistentKey<TKey, TValue>()
        {
            foreach (var remove in Remove<TKey, TValue>())
            {
                foreach (var mvd in Multi<TKey, TValue, List<TValue>>())
                {
                    TKey newKey = (TKey)TypeBuilder<TKey>();
                    remove(mvd, newKey);
                    int oldCount = mvd.Count;

                    Assert.False(remove(mvd, newKey));

                    Assert.Throws<KeyNotFoundException>(() => mvd[newKey]);
                    Assert.Equal(oldCount, mvd.Count);
                }
                foreach (var mvd in Multi<TKey, TValue, HashSet<TValue>>())
                {
                    TKey newKey = (TKey)TypeBuilder<TKey>();
                    remove(mvd, newKey);
                    int oldCount = mvd.Count;

                    Assert.False(remove(mvd, newKey));

                    Assert.Equal(oldCount, mvd.Count);
                    Assert.Throws<KeyNotFoundException>(() => mvd[newKey]);
                }
            }
        }

        public void MVD_Remove_PreExistingKey<TKey, TValue>()
        {
            foreach (var remove in Remove<TKey, TValue>())
            {
                foreach (var mvd in Multi<TKey, TValue, List<TValue>>())
                {
                    TKey newKey = (TKey)TypeBuilder<TKey>();
                    mvd.Add(newKey, (TValue)TypeBuilder<TValue>());
                    mvd.Add(newKey, (TValue)TypeBuilder<TValue>());
                    mvd.Add(newKey, (TValue)TypeBuilder<TValue>());
                    int keyCount = mvd[newKey].Count;
                    int oldCount = PairsCount<TKey, TValue>(mvd);

                    Assert.True(remove(mvd, newKey));

                    Assert.Throws<KeyNotFoundException>(() => mvd[newKey]);
                    Assert.Equal(oldCount - keyCount, PairsCount<TKey, TValue>(mvd));
                }
                foreach (var mvd in Multi<TKey, TValue, HashSet<TValue>>())
                {
                    TKey newKey = (TKey)TypeBuilder<TKey>();
                    mvd.Add(newKey, (TValue)TypeBuilder<TValue>());
                    int keyCount = mvd[newKey].Count;
                    int oldCount = PairsCount<TKey, TValue>(mvd);

                    Assert.True(remove(mvd, newKey));

                    Assert.Throws<KeyNotFoundException>(() => mvd[newKey]);
                    Assert.Equal(oldCount - keyCount, PairsCount<TKey, TValue>(mvd));
                }
            }
        }

        #endregion

        #region Tests for: RemoveItem
        /*======================================================================
        ** RemoveItem includes:
        **      -multiDictionary.Remove(TKey key, TValue value)
        ======================================================================*/

        private delegate bool RemoveItemDelegate<TKey, TValue>(MultiValueDictionary<TKey, TValue> mvd, TKey key, TValue value);
        private RemoveItemDelegate<TKey, TValue>[] RemoveItem<TKey, TValue>()
        {
            var ret = new RemoveItemDelegate<TKey, TValue>[1];
            ret[0] = (MultiValueDictionary<TKey, TValue> mvd, TKey key, TValue value) => mvd.Remove(key, value);
            return ret;
        }

        public void MVD_RemoveItem_NullKey<TKey, TValue>()
        {
            foreach (var remove in RemoveItem<string, TValue>())
            {
                foreach (var mvd in Multi<string, TValue, List<TValue>>())
                {
                    string newKey = null;

                    int oldCount = mvd.Count;
                    Assert.Throws<ArgumentNullException>(() =>
                    {
                        remove(mvd, newKey, (TValue)TypeBuilder<TValue>());
                    });
                    Assert.Equal(oldCount, mvd.Count);
                }
            }
        }

        public void MVD_RemoveItem_ValidKeyInvalidValue<TKey, TValue>()
        {
            foreach (var remove in RemoveItem<TKey, TValue>())
            {
                foreach (var mvd in Multi<TKey, TValue, List<TValue>>())
                {
                    TKey newKey = (TKey)TypeBuilder<TKey>();
                    TValue newValue1 = (TValue)TypeBuilder<TValue>();
                    TValue newValue2 = (TValue)TypeBuilder<TValue>();
                    while (newValue1.Equals(newValue2))
                        newValue2 = (TValue)TypeBuilder<TValue>();
                    mvd.Add(newKey, newValue1);
                    while (mvd.Contains(newKey, newValue2))
                        Assert.True(remove(mvd, newKey, newValue2));
                    int oldCount = PairsCount<TKey, TValue>(mvd);
                    int keyCount = mvd[newKey].Count;

                    Assert.False(remove(mvd, newKey, newValue2));

                    Assert.Equal(oldCount, PairsCount<TKey, TValue>(mvd));
                    Assert.Equal(keyCount, mvd[newKey].Count);
                }
            }
        }

        public void MVD_RemoveItem_ValidKeyValidValue<TKey, TValue>()
        {
            foreach (var remove in RemoveItem<TKey, TValue>())
            {
                foreach (var mvd in Multi<TKey, TValue, List<TValue>>())
                {
                    TKey newKey = (TKey)TypeBuilder<TKey>();
                    TValue newValue1 = (TValue)TypeBuilder<TValue>();
                    TValue newValue2 = (TValue)TypeBuilder<TValue>();
                    while (newValue1.Equals(newValue2))
                        newValue2 = (TValue)TypeBuilder<TValue>();
                    mvd.Add(newKey, newValue1);
                    mvd.Add(newKey, newValue2);
                    int oldCount = PairsCount<TKey, TValue>(mvd);
                    int keyCount = mvd[newKey].Count;

                    Assert.True(remove(mvd, newKey, newValue2));

                    Assert.Equal(oldCount - 1, PairsCount<TKey, TValue>(mvd));
                    Assert.Equal(keyCount - 1, mvd[newKey].Count);
                    Assert.True(mvd[newKey].Count > 0);
                }
            }
        }

        public void MVD_RemoveItem_RemovesOnlyOneInstanceOfValue<TKey, TValue>()
        {
            foreach (var remove in RemoveItem<TKey, TValue>())
            {
                foreach (var mvd in Multi<TKey, TValue, List<TValue>>())
                {
                    TKey newKey = (TKey)TypeBuilder<TKey>();
                    TValue newValue1 = (TValue)TypeBuilder<TValue>();
                    mvd.Add(newKey, newValue1);
                    mvd.Add(newKey, newValue1);
                    int oldCount = PairsCount<TKey, TValue>(mvd);
                    int keyCount = mvd[newKey].Count;

                    Assert.True(remove(mvd, newKey, newValue1));

                    Assert.Equal(oldCount - 1, PairsCount<TKey, TValue>(mvd));
                    Assert.Equal(keyCount - 1, mvd[newKey].Count);
                    Assert.True(mvd[newKey].Count > 0);
                }
            }
        }

        #endregion

        #region Tests for: TryGetValue
        /*======================================================================
        ** TryGetValue includes:
        **      -multiDictionary.TryGetValue(TKey key, out IReadOnlyCollection<TValue> value)
        ======================================================================*/

        private delegate bool TryGetValueDelegate<TKey, TValue>(MultiValueDictionary<TKey, TValue> mvd, TKey key, out IReadOnlyCollection<TValue> value);
        private TryGetValueDelegate<TKey, TValue>[] TryGetValue<TKey, TValue>()
        {
            var ret = new TryGetValueDelegate<TKey, TValue>[1];
            ret[0] = (MultiValueDictionary<TKey, TValue> mvd, TKey key, out IReadOnlyCollection<TValue> value) => mvd.TryGetValue(key, out value);
            return ret;
        }

        public void MVD_TryGetValue_NullKey<TKey, TValue>()
        {
            foreach (var tgv in TryGetValue<string, TValue>())
            {
                foreach (var mvd in Multi<string, TValue, List<TValue>>())
                {
                    IReadOnlyCollection<TValue> retCol;
                    Assert.Throws<ArgumentNullException>(() =>
                    {
                        tgv(mvd, null, out retCol);
                    });
                }
            }
        }

        public void MVD_TryGetValue_EmptyKey<TKey, TValue>()
        {
            foreach (var tgv in TryGetValue<TKey, TValue>())
            {
                foreach (var mvd in Multi<TKey, TValue, List<TValue>>())
                {
                    TKey newKey = (TKey)TypeBuilder<TKey>();
                    IReadOnlyCollection<TValue> retCol;
                    mvd.Remove(newKey);

                    Assert.False(tgv(mvd, newKey, out retCol));
                }
            }
        }

        public void MVD_TryGetValue_NonEmptyKey<TKey, TValue>()
        {
            foreach (var tgv in TryGetValue<TKey, TValue>())
            {
                foreach (var mvd in Multi<TKey, TValue, List<TValue>>())
                {
                    TKey newKey = (TKey)TypeBuilder<TKey>();
                    TValue newValue = (TValue)TypeBuilder<TValue>();
                    IReadOnlyCollection<TValue> retCol;
                    mvd.Add(newKey, newValue);

                    Assert.True(tgv(mvd, newKey, out retCol));
                    Assert.True(retCol.Contains(newValue));
                    CompareEnumerables<TValue>(retCol, mvd[newKey], true);
                }
            }
        }

        public void MVD_TryGetValue_ReturnedCollectionIsAView<TKey, TValue>()
        {
            foreach (var tgv in TryGetValue<TKey, TValue>())
            {
                foreach (var mvd in Multi<TKey, TValue, List<TValue>>())
                {
                    TKey newKey = (TKey)TypeBuilder<TKey>();
                    IReadOnlyCollection<TValue> retCol;
                    mvd.Remove(newKey);
                    mvd.Add(newKey, (TValue)TypeBuilder<TValue>());

                    Assert.True(tgv(mvd, newKey, out retCol));
                    mvd.Add(newKey, (TValue)TypeBuilder<TValue>());
                    mvd.Add(newKey, (TValue)TypeBuilder<TValue>());
                    mvd.Add(newKey, (TValue)TypeBuilder<TValue>());
                    mvd.Add(newKey, (TValue)TypeBuilder<TValue>());

                    CompareEnumerables<TValue>(retCol, mvd[newKey], true);
                }
            }
        }

        #endregion

        #region Tests for: Values
        /*======================================================================
        ** Values includes:
        **      -multiDictionary.Values
        ======================================================================*/

        private delegate IEnumerable<IReadOnlyCollection<TValue>> ValuesDelegate<TKey, TValue>(MultiValueDictionary<TKey, TValue> mvd);
        private ValuesDelegate<TKey, TValue>[] Values<TKey, TValue>()
        {
            var ret = new ValuesDelegate<TKey, TValue>[1];
            ret[0] = (MultiValueDictionary<TKey, TValue> mvd) => mvd.Values;
            return ret;
        }

        public void MVD_Values_EmptyMVD<TKey, TValue>()
        {
            foreach (var values in Values<TKey, TValue>())
            {
                foreach (var mvd in Multi<TKey, TValue, List<TValue>>())
                {
                    mvd.Clear();
                    Assert.Empty(values(mvd));
                }
            }
        }

        public void MVD_Values_NonEmptyMVD<TKey, TValue>()
        {
            foreach (var values in Values<TKey, TValue>())
            {
                foreach (var mvd in Multi<TKey, TValue, List<TValue>>())
                {
                    TKey newKey2 = (TKey)TypeBuilder<TKey>();
                    TKey newKey1 = (TKey)TypeBuilder<TKey>();
                    TValue newValue = (TValue)TypeBuilder<TValue>();
                    mvd.Add(newKey2, newValue);
                    mvd.Add(newKey1, newValue);

                    Assert.NotEmpty(values(mvd));
                    var valueList = new List<IReadOnlyCollection<TValue>>();
                    foreach (var pair in mvd)
                        valueList.Add(pair.Value);
                    CompareEnumerables<IReadOnlyCollection<TValue>>(values(mvd), valueList, true);
                }
            }
        }

        #endregion
    }
}
