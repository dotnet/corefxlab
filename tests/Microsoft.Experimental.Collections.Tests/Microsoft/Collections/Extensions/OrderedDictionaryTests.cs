// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Xunit;

namespace Microsoft.Collections.Extensions.Tests
{
    public class OrderedDictionaryTests
    {
        private const int s_defaultCount = 5; // tests rely on this being >= 5

        private static OrderedDictionary<string, int> GetDefaultOrderedDictionary(int? count = null) => new OrderedDictionary<string, int>(Enumerable.Range(0, count ?? s_defaultCount).ToDictionary(i => i.ToString()));

        private static void ValidateDefaultOrderedDictionaryIsContiguous(OrderedDictionary<string, int> dictionary, int? count = null)
        {
            Assert.Equal(count ?? s_defaultCount, dictionary.Count);
            Assert.Equal(count ?? s_defaultCount, dictionary.Keys.Count);
            Assert.Equal(count ?? s_defaultCount, dictionary.Values.Count);
            for (int i = 0; i < (count ?? s_defaultCount); ++i)
            {
                string key = i.ToString();
                Assert.True(dictionary.ContainsKey(key));
                Assert.Equal(i, dictionary.IndexOf(key));
                Assert.Equal(i, dictionary[key]);
                Assert.Equal(i, dictionary[i]);
                Assert.Equal(key, dictionary.Keys[i]);
                Assert.True(dictionary.TryGetValue(key, out int value));
                Assert.Equal(i, value);
            }
            Assert.Equal(dictionary.Select(p => p.Key), dictionary.Keys);
            Assert.Equal(dictionary.Select(p => p.Value), dictionary.Values);
        }

        [Fact]
        public void Count()
        {
            OrderedDictionary<string, int> dictionary = GetDefaultOrderedDictionary();
            Assert.Equal(s_defaultCount, dictionary.Count);
        }

        [Fact]
        public void KeysCount()
        {
            OrderedDictionary<string, int> dictionary = GetDefaultOrderedDictionary();
            Assert.Equal(s_defaultCount, dictionary.Keys.Count);
        }

        [Fact]
        public void ValuesCount()
        {
            OrderedDictionary<string, int> dictionary = GetDefaultOrderedDictionary();
            Assert.Equal(s_defaultCount, dictionary.Values.Count);
        }

        [Fact]
        public void Comparer_IsDefaultEqualityComparer_WhenNotProvided()
        {
            OrderedDictionary<string, int> dictionary = new OrderedDictionary<string, int>();
            Assert.Same(EqualityComparer<string>.Default, dictionary.Comparer);
        }

        [Fact]
        public void Comparer_IsDefaultEqualityComparer_WhenNull()
        {
            OrderedDictionary<string, int> dictionary = new OrderedDictionary<string, int>((IEqualityComparer<string>)null);
            Assert.Same(EqualityComparer<string>.Default, dictionary.Comparer);
        }

        [Fact]
        public void Comparer_IsDefaultEqualityComparer_WhenDefaultIsProvided()
        {
            OrderedDictionary<string, int> dictionary = new OrderedDictionary<string, int>(EqualityComparer<string>.Default);
            Assert.Same(EqualityComparer<string>.Default, dictionary.Comparer);
        }

        [Fact]
        public void Comparer_IsProvided_WhenProvided()
        {
            OrderedDictionary<string, int> dictionary = new OrderedDictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            Assert.Same(StringComparer.OrdinalIgnoreCase, dictionary.Comparer);
        }

        [Fact]
        public void ContainsKey_ReturnsTrue_WhenContainsKey()
        {
            OrderedDictionary<string, int> dictionary = GetDefaultOrderedDictionary();
            for (int i = 0; i < s_defaultCount; ++i)
            {
                Assert.True(dictionary.ContainsKey(i.ToString()));
            }
        }

        [Fact]
        public void ContainsKey_ReturnsFalse_WhenNotContainsKey()
        {
            OrderedDictionary<string, int> dictionary = GetDefaultOrderedDictionary();
            Assert.False(dictionary.ContainsKey(s_defaultCount.ToString()));
        }

        [Fact]
        public void ContainsKey_ThrowsArgumentNullException_WhenKeyIsNull()
        {
            OrderedDictionary<string, int> dictionary = GetDefaultOrderedDictionary();
            Assert.Throws<ArgumentNullException>(() => dictionary.ContainsKey(null));
        }

        [Fact]
        public void KeyIndexerGetter_ReturnsValue_WhenContainsKey()
        {
            OrderedDictionary<string, int> dictionary = GetDefaultOrderedDictionary();
            for (int i = 0; i < s_defaultCount; ++i)
            {
                Assert.Equal(i, dictionary[i.ToString()]);
            }
        }

        [Fact]
        public void KeyIndexerGetter_ThrowsKeyNotFoundException_WhenNotContainsKey()
        {
            OrderedDictionary<string, int> dictionary = GetDefaultOrderedDictionary();
            Assert.Throws<KeyNotFoundException>(() => dictionary[s_defaultCount.ToString()]);
        }

        [Fact]
        public void KeyIndexerGetter_ThrowsArgumentNullException_WhenKeyIsNull()
        {
            OrderedDictionary<string, int> dictionary = GetDefaultOrderedDictionary();
            Assert.Throws<ArgumentNullException>(() => dictionary[null]);
        }

        [Fact]
        public void IndexIndexerGetter_ReturnsValue_WhenIndexIsValid()
        {
            OrderedDictionary<string, int> dictionary = GetDefaultOrderedDictionary();
            for (int i = 0; i < s_defaultCount; ++i)
            {
                Assert.Equal(i, dictionary[i]);
            }
        }

        [Fact]
        public void IndexIndexerGetter_ThrowsArgumentOutOfRangeException_WhenIndexIsLessThanZero()
        {
            OrderedDictionary<string, int> dictionary = GetDefaultOrderedDictionary();
            Assert.Throws<ArgumentOutOfRangeException>(() => dictionary[-1]);
        }

        [Fact]
        public void IndexIndexerGetter_ThrowsArgumentOutOfRangeException_WhenIndexIsCount()
        {
            OrderedDictionary<string, int> dictionary = GetDefaultOrderedDictionary();
            Assert.Throws<ArgumentOutOfRangeException>(() => dictionary[s_defaultCount]);
        }

        [Fact]
        public void IndexOf_ReturnsIndex_WhenContainsKey()
        {
            OrderedDictionary<string, int> dictionary = GetDefaultOrderedDictionary();
            for (int i = 0; i < s_defaultCount; ++i)
            {
                Assert.Equal(i, dictionary.IndexOf(i.ToString()));
            }
        }

        [Fact]
        public void IndexOf_ReturnsNegativeOne_WhenNotContainsKey()
        {
            OrderedDictionary<string, int> dictionary = GetDefaultOrderedDictionary();
            Assert.Equal(-1, dictionary.IndexOf(s_defaultCount.ToString()));
        }

        [Fact]
        public void IndexOf_ThrowsArgumentNullException_WhenKeyIsNull()
        {
            OrderedDictionary<string, int> dictionary = GetDefaultOrderedDictionary();
            Assert.Throws<ArgumentNullException>(() => dictionary.IndexOf(null));
        }

        [Fact]
        public void Remove_ReturnsTrue_WhenContainsKey()
        {
            OrderedDictionary<string, int> dictionary = GetDefaultOrderedDictionary();
            Assert.True(dictionary.Remove("1"));
            ValidateDefaultDictionaryWith1Removed(dictionary);
        }

        [Fact]
        public void Remove_ReturnsTrueAndValue_WhenContainsKey()
        {
            OrderedDictionary<string, int> dictionary = GetDefaultOrderedDictionary();
            Assert.True(dictionary.Remove("1", out int value));
            Assert.Equal(1, value);
            ValidateDefaultDictionaryWith1Removed(dictionary);
        }

        private void ValidateDefaultDictionaryWith1Removed(OrderedDictionary<string, int> dictionary)
        {
            Assert.Equal(s_defaultCount - 1, dictionary.Count);
            Assert.Equal(s_defaultCount - 1, dictionary.Keys.Count);
            Assert.Equal(s_defaultCount - 1, dictionary.Values.Count);
            for (int i = 0; i < s_defaultCount; ++i)
            {
                string key = i.ToString();
                if (i < 1)
                {
                    Assert.True(dictionary.ContainsKey(key));
                    Assert.Equal(i, dictionary.IndexOf(key));
                    Assert.Equal(i, dictionary[key]);
                    Assert.Equal(i, dictionary[i]);
                    Assert.Equal(key, dictionary.Keys[i]);
                    Assert.True(dictionary.TryGetValue(key, out int value));
                    Assert.Equal(i, value);
                }
                else if (i == 1)
                {
                    Assert.False(dictionary.ContainsKey(key));
                    Assert.Equal(-1, dictionary.IndexOf(key));
                    Assert.Throws<KeyNotFoundException>(() => dictionary[key]);
                    Assert.False(dictionary.TryGetValue(key, out int value));
                    Assert.Equal(0, value);
                }
                else
                {
                    Assert.True(dictionary.ContainsKey(key));
                    Assert.Equal(i - 1, dictionary.IndexOf(key));
                    Assert.Equal(i, dictionary[key]);
                    Assert.Equal(i, dictionary[i - 1]);
                    Assert.Equal(key, dictionary.Keys[i - 1]);
                    Assert.True(dictionary.TryGetValue(key, out int value));
                    Assert.Equal(i, value);
                }
            }
            Assert.Throws<ArgumentOutOfRangeException>(() => dictionary[s_defaultCount - 1]);
            Assert.Throws<ArgumentOutOfRangeException>(() => dictionary.Keys[s_defaultCount - 1]);
            Assert.Equal(dictionary.Select(p => p.Key), dictionary.Keys);
            Assert.Equal(dictionary.Select(p => p.Value), dictionary.Values);
        }

        [Fact]
        public void Remove_ReturnsFalse_WhenNotContainsKey()
        {
            OrderedDictionary<string, int> dictionary = GetDefaultOrderedDictionary();
            Assert.False(dictionary.Remove(s_defaultCount.ToString()));
            Assert.False(dictionary.Remove(s_defaultCount.ToString(), out int value));
            Assert.Equal(0, value);
            ValidateDefaultOrderedDictionaryIsContiguous(dictionary);
        }

        [Fact]
        public void Remove_ThrowsArgumentNullException_WhenKeyIsNull()
        {
            OrderedDictionary<string, int> dictionary = GetDefaultOrderedDictionary();
            Assert.Throws<ArgumentNullException>(() => dictionary.Remove(null));
            Assert.Throws<ArgumentNullException>(() => dictionary.Remove(null, out _));
            ValidateDefaultOrderedDictionaryIsContiguous(dictionary);
        }

        [Fact]
        public void RemoveAt_Succeeds_WhenIndexIsValid()
        {
            OrderedDictionary<string, int> dictionary = GetDefaultOrderedDictionary();
            dictionary.RemoveAt(1);
            ValidateDefaultDictionaryWith1Removed(dictionary);
        }

        [Fact]
        public void RemoveAt_ThrowsArgumentOutOfRangeException_WhenIndexIsLessThanZero()
        {
            OrderedDictionary<string, int> dictionary = GetDefaultOrderedDictionary();
            Assert.Throws<ArgumentOutOfRangeException>(() => dictionary.RemoveAt(-1));
            ValidateDefaultOrderedDictionaryIsContiguous(dictionary);
        }

        [Fact]
        public void RemoveAt_ThrowsArgumentOutOfRangeException_WhenIndexIsCount()
        {
            OrderedDictionary<string, int> dictionary = GetDefaultOrderedDictionary();
            Assert.Throws<ArgumentOutOfRangeException>(() => dictionary.RemoveAt(s_defaultCount));
            ValidateDefaultOrderedDictionaryIsContiguous(dictionary);
        }

        [Fact]
        public void Insert_Succeeds_WhenIndexIsValidAndNotContainsKey()
        {
            OrderedDictionary<string, int> dictionary = GetDefaultOrderedDictionary();
            dictionary.Insert(2, s_defaultCount.ToString(), s_defaultCount);
            Assert.Equal(s_defaultCount + 1, dictionary.Count);
            Assert.Equal(s_defaultCount + 1, dictionary.Keys.Count);
            Assert.Equal(s_defaultCount + 1, dictionary.Values.Count);
            for (int i = 0; i < s_defaultCount + 1; ++i)
            {
                string key = i.ToString();
                Assert.True(dictionary.ContainsKey(key));
                Assert.Equal(i, dictionary[key]);
                Assert.True(dictionary.TryGetValue(key, out int value));
                Assert.Equal(i, value);
                if (i < 2)
                {
                    Assert.Equal(i, dictionary.IndexOf(key));
                    Assert.Equal(i, dictionary[i]);
                    Assert.Equal(key, dictionary.Keys[i]);
                }
                else if (i == 2)
                {
                    Assert.Equal(i + 1, dictionary.IndexOf(key));
                    Assert.Equal(s_defaultCount, dictionary[i]);
                    Assert.Equal(s_defaultCount.ToString(), dictionary.Keys[i]);
                }
                else
                {
                    Assert.Equal(i == s_defaultCount ? 2 : i + 1, dictionary.IndexOf(key));
                    Assert.Equal(i - 1, dictionary[i]);
                    Assert.Equal((i - 1).ToString(), dictionary.Keys[i]);
                }
            }
            Assert.Equal(dictionary.Select(p => p.Key), dictionary.Keys);
            Assert.Equal(dictionary.Select(p => p.Value), dictionary.Values);
        }

        [Fact]
        public void Insert_Succeeds_WhenIndexIsCountAndNotContainsKey()
        {
            OrderedDictionary<string, int> dictionary = GetDefaultOrderedDictionary();
            dictionary.Insert(s_defaultCount, s_defaultCount.ToString(), s_defaultCount);
            ValidateDefaultOrderedDictionaryIsContiguous(dictionary, s_defaultCount + 1);
        }

        [Fact]
        public void Insert_ThrowsArgumentOutOfRangeException_WhenIndexIsLessThanZero()
        {
            OrderedDictionary<string, int> dictionary = GetDefaultOrderedDictionary();
            Assert.Throws<ArgumentOutOfRangeException>(() => dictionary.Insert(-1, s_defaultCount.ToString(), s_defaultCount));
            ValidateDefaultOrderedDictionaryIsContiguous(dictionary);
        }

        [Fact]
        public void Insert_ThrowsArgumentOutOfRangeException_WhenIndexIsGreaterThanCount()
        {
            OrderedDictionary<string, int> dictionary = GetDefaultOrderedDictionary();
            Assert.Throws<ArgumentOutOfRangeException>(() => dictionary.Insert(s_defaultCount + 1, s_defaultCount.ToString(), s_defaultCount));
            ValidateDefaultOrderedDictionaryIsContiguous(dictionary);
        }

        [Fact]
        public void Insert_ThrowsArgumentNullException_WhenKeyIsNull()
        {
            OrderedDictionary<string, int> dictionary = GetDefaultOrderedDictionary();
            Assert.Throws<ArgumentNullException>(() => dictionary.Insert(2, null, s_defaultCount));
            ValidateDefaultOrderedDictionaryIsContiguous(dictionary);
        }

        [Fact]
        public void Insert_ThrowsArgumentException_WhenContainsKey()
        {
            OrderedDictionary<string, int> dictionary = GetDefaultOrderedDictionary();
            Assert.Throws<ArgumentException>(() => dictionary.Insert(2, "2", s_defaultCount));
            ValidateDefaultOrderedDictionaryIsContiguous(dictionary);
        }

        [Fact]
        public void Enumeration()
        {
            OrderedDictionary<string, int> dictionary = GetDefaultOrderedDictionary();

            int temp = 0;
            foreach (KeyValuePair<string, int> pair in dictionary)
            {
                Assert.Equal(temp.ToString(), pair.Key);
                Assert.Equal(temp, pair.Value);
                ++temp;
            }
            Assert.Equal(s_defaultCount, temp);
        }

        [Fact]
        public void KeysEnumeration()
        {
            OrderedDictionary<string, int> dictionary = GetDefaultOrderedDictionary();

            int temp = 0;
            foreach (string key in dictionary.Keys)
            {
                Assert.Equal(temp.ToString(), key);
                ++temp;
            }
            Assert.Equal(s_defaultCount, temp);
        }

        [Fact]
        public void ValuesEnumeration()
        {
            OrderedDictionary<string, int> dictionary = GetDefaultOrderedDictionary();

            int temp = 0;
            foreach (int value in dictionary.Values)
            {
                Assert.Equal(temp, value);
                ++temp;
            }
            Assert.Equal(s_defaultCount, temp);
        }

        [Fact]
        public void Add_Succeeds_WhenNotContainsKey()
        {
            OrderedDictionary<string, int> dictionary = GetDefaultOrderedDictionary();
            dictionary.Add(s_defaultCount.ToString(), s_defaultCount);
            ValidateDefaultOrderedDictionaryIsContiguous(dictionary, s_defaultCount + 1);
        }

        [Fact]
        public void Add_ThrowsArgumentException_WhenContainsKey()
        {
            OrderedDictionary<string, int> dictionary = GetDefaultOrderedDictionary();
            Assert.Throws<ArgumentException>(() => dictionary.Add("2", s_defaultCount));
            ValidateDefaultOrderedDictionaryIsContiguous(dictionary);
        }

        [Fact]
        public void Add_ThrowsArgumentNullException_WhenKeyIsNull()
        {
            OrderedDictionary<string, int> dictionary = GetDefaultOrderedDictionary();
            Assert.Throws<ArgumentNullException>(() => dictionary.Add(null, s_defaultCount));
            ValidateDefaultOrderedDictionaryIsContiguous(dictionary);
        }

        [Fact]
        public void TryAdd_ReturnsTrue_WhenNotContainsKey()
        {
            OrderedDictionary<string, int> dictionary = GetDefaultOrderedDictionary();
            Assert.True(dictionary.TryAdd(s_defaultCount.ToString(), s_defaultCount));
            ValidateDefaultOrderedDictionaryIsContiguous(dictionary, s_defaultCount + 1);
        }

        [Fact]
        public void TryAdd_ReturnsFalse_WhenContainsKey()
        {
            OrderedDictionary<string, int> dictionary = GetDefaultOrderedDictionary();
            Assert.False(dictionary.TryAdd("2", s_defaultCount));
            ValidateDefaultOrderedDictionaryIsContiguous(dictionary);
        }

        [Fact]
        public void TryAdd_ThrowsArgumentNullException_WhenKeyIsNull()
        {
            OrderedDictionary<string, int> dictionary = GetDefaultOrderedDictionary();
            Assert.Throws<ArgumentNullException>(() => dictionary.TryAdd(null, s_defaultCount));
            ValidateDefaultOrderedDictionaryIsContiguous(dictionary);
        }

        [Fact]
        public void TryGetValue_ReturnsTrueAndValue_WhenContainsKey()
        {
            OrderedDictionary<string, int> dictionary = GetDefaultOrderedDictionary();
            for (int i = 0; i < s_defaultCount; ++i)
            {
                Assert.True(dictionary.TryGetValue(i.ToString(), out int value));
                Assert.Equal(i, value);
            }
        }

        [Fact]
        public void TryGetValue_ReturnsFalseAndDefaultValue_WhenNotContainsKey()
        {
            OrderedDictionary<string, int> dictionary = GetDefaultOrderedDictionary();
            Assert.False(dictionary.TryGetValue(s_defaultCount.ToString(), out int value));
            Assert.Equal(0, value);
        }

        [Fact]
        public void TryGetValue_ThrowsArgumentNullException_WhenKeyIsNull()
        {
            OrderedDictionary<string, int> dictionary = GetDefaultOrderedDictionary();
            Assert.Throws<ArgumentNullException>(() => dictionary.TryGetValue(null, out _));
        }

        [Fact]
        public void KeyIndexerSetter_Succeeds_WhenContainsKey()
        {
            OrderedDictionary<string, int> dictionary = GetDefaultOrderedDictionary();
            dictionary["3"] = s_defaultCount;
            ValidateDefaultOrderedDictionaryWith3sValueEqualToDefaultCount(dictionary);
        }

        private void ValidateDefaultOrderedDictionaryWith3sValueEqualToDefaultCount(OrderedDictionary<string, int> dictionary)
        {
            Assert.Equal(s_defaultCount, dictionary.Count);
            Assert.Equal(s_defaultCount, dictionary.Keys.Count);
            Assert.Equal(s_defaultCount, dictionary.Values.Count);
            for (int i = 0; i < s_defaultCount; ++i)
            {
                string key = i.ToString();
                Assert.True(dictionary.ContainsKey(key));
                Assert.Equal(i, dictionary.IndexOf(key));
                Assert.True(dictionary.TryGetValue(key, out int value));
                if (i == 3)
                {
                    Assert.Equal(s_defaultCount, dictionary[key]);
                    Assert.Equal(s_defaultCount, dictionary[i]);
                    Assert.Equal(s_defaultCount, value);
                }
                else
                {
                    Assert.Equal(i, dictionary[key]);
                    Assert.Equal(i, dictionary[i]);
                    Assert.Equal(i, value);
                }
                Assert.Equal(key, dictionary.Keys[i]);
            }
            Assert.Equal(dictionary.Select(p => p.Key), dictionary.Keys);
            Assert.Equal(dictionary.Select(p => p.Value), dictionary.Values);
        }

        [Fact]
        public void KeyIndexerSetter_Succeeds_WhenNotContainsKey()
        {
            OrderedDictionary<string, int> dictionary = GetDefaultOrderedDictionary();
            dictionary[s_defaultCount.ToString()] = s_defaultCount;
            ValidateDefaultOrderedDictionaryIsContiguous(dictionary, s_defaultCount + 1);
        }

        [Fact]
        public void KeyIndexerSetter_ThrowsArgumentNullException_WhenKeyIsNull()
        {
            OrderedDictionary<string, int> dictionary = GetDefaultOrderedDictionary();
            Assert.Throws<ArgumentNullException>(() => dictionary[null] = s_defaultCount);
        }

        [Fact]
        public void IndexIndexerSetter_Succeeds_WhenIndexIsValid()
        {
            OrderedDictionary<string, int> dictionary = GetDefaultOrderedDictionary();
            dictionary[3] = s_defaultCount;
            ValidateDefaultOrderedDictionaryWith3sValueEqualToDefaultCount(dictionary);
        }

        [Fact]
        public void IndexIndexerSetter_ThrowsArgumentOutOfRangeException_WhenIndexIsLessThanZero()
        {
            OrderedDictionary<string, int> dictionary = GetDefaultOrderedDictionary();
            Assert.Throws<ArgumentOutOfRangeException>(() => dictionary[-1] = s_defaultCount);
            ValidateDefaultOrderedDictionaryIsContiguous(dictionary);
        }

        [Fact]
        public void IndexIndexerSetter_ThrowsArgumentOutOfRangeException_WhenIndexIsCount()
        {
            OrderedDictionary<string, int> dictionary = GetDefaultOrderedDictionary();
            Assert.Throws<ArgumentOutOfRangeException>(() => dictionary[s_defaultCount] = s_defaultCount);
            ValidateDefaultOrderedDictionaryIsContiguous(dictionary);
        }

        [Fact]
        public void IListIndexerSetter_ReplacesExistingEntry_WhenIndexIsValidAndKeyIsNotContained()
        {
            OrderedDictionary<string, int> dictionary = GetDefaultOrderedDictionary();
            ((IList<KeyValuePair<string, int>>)dictionary)[2] = KeyValuePair.Create("-1", -1);
            Assert.Equal(s_defaultCount, dictionary.Count);
            Assert.False(dictionary.ContainsKey("2"));
            Assert.True(dictionary.ContainsKey("-1"));
            for (int i = 0; i < s_defaultCount; ++i)
            {
                if (i == 2)
                {
                    Assert.Equal(i, dictionary.IndexOf("-1"));
                    Assert.Equal(-1, dictionary[i]);
                }
                else
                {
                    Assert.Equal(i, dictionary.IndexOf(i.ToString()));
                    Assert.Equal(i, dictionary[i]);
                }
            }
        }

        [Fact]
        public void IListIndexerSetter_ReplacesExistingEntry_WhenIndexIsValidAndKeyIsContainedAtIndex()
        {
            OrderedDictionary<string, int> dictionary = GetDefaultOrderedDictionary();
            ((IList<KeyValuePair<string, int>>)dictionary)[2] = KeyValuePair.Create("2", -1);
            Assert.Equal(s_defaultCount, dictionary.Count);
            Assert.True(dictionary.ContainsKey("2"));
            for (int i = 0; i < s_defaultCount; ++i)
            {
                Assert.Equal(i, dictionary.IndexOf(i.ToString()));
                if (i == 2)
                {
                    Assert.Equal(-1, dictionary[i]);
                }
                else
                {
                    Assert.Equal(i, dictionary[i]);
                }
            }
        }

        [Fact]
        public void IListIndexerSetter_ThrowsArgumentException_WhenIndexIsValidAndKeyIsContainedAtDifferentIndex()
        {
            OrderedDictionary<string, int> dictionary = GetDefaultOrderedDictionary();
            Assert.Throws<ArgumentException>(() => ((IList<KeyValuePair<string, int>>)dictionary)[3] = KeyValuePair.Create("2", -1));
            ValidateDefaultOrderedDictionaryIsContiguous(dictionary);
        }

        [Fact]
        public void IListIndexerSetter_ThrowsArgumentOutOfRangeException_WhenIndexIsLessThanZero()
        {
            OrderedDictionary<string, int> dictionary = GetDefaultOrderedDictionary();
            Assert.Throws<ArgumentOutOfRangeException>(() => ((IList<KeyValuePair<string, int>>)dictionary)[-1] = KeyValuePair.Create("2", -1));
            ValidateDefaultOrderedDictionaryIsContiguous(dictionary);
        }

        [Fact]
        public void IListIndexerSetter_ThrowsArgumentOutOfRangeException_WhenIndexIsCount()
        {
            OrderedDictionary<string, int> dictionary = GetDefaultOrderedDictionary();
            Assert.Throws<ArgumentOutOfRangeException>(() => ((IList<KeyValuePair<string, int>>)dictionary)[s_defaultCount] = KeyValuePair.Create("2", -1));
            ValidateDefaultOrderedDictionaryIsContiguous(dictionary);
        }

        [Fact]
        public void IListIndexerSetter_ThrowsArgumentNullException_WhenKeyIsNull()
        {
            OrderedDictionary<string, int> dictionary = GetDefaultOrderedDictionary();
            Assert.Throws<ArgumentNullException>(() => ((IList<KeyValuePair<string, int>>)dictionary)[2] = KeyValuePair.Create((string)null, -1));
            ValidateDefaultOrderedDictionaryIsContiguous(dictionary);
        }

        [Fact]
        public void IListIndexerSetter_ReplacesEqualKey_WhenIndexIsValidAndKeyIsContainedAtIndex()
        {
            OrderedDictionary<string, char> dictionary = new OrderedDictionary<string, char>(StringComparer.OrdinalIgnoreCase);
            for (char c = 'A'; c <= 'Z'; ++c)
            {
                dictionary.Add(c.ToString(), c);
            }
            ((IList<KeyValuePair<string, char>>)dictionary)[2] = KeyValuePair.Create("c", 'c');
            Assert.Equal(2, dictionary.IndexOf("c"));
            Assert.Equal(2, dictionary.IndexOf("C"));
            Assert.Equal("c", dictionary.Keys[2]);
            Assert.Equal('c', dictionary[2]);
        }

        [Fact]
        public void EnsureCapacity_Succeeds_WhenCapacityIsGreaterThanCount()
        {
            OrderedDictionary<string, int> dictionary = GetDefaultOrderedDictionary();
            int requestedCapacity = s_defaultCount << 1;
            int actualCapacity = dictionary.EnsureCapacity(requestedCapacity);
            Assert.True(actualCapacity >= requestedCapacity);
            ValidateDefaultOrderedDictionaryIsContiguous(dictionary);
        }

        [Fact]
        public void EnsureCapacity_Succeeds_WhenCapacityIsCount()
        {
            OrderedDictionary<string, int> dictionary = GetDefaultOrderedDictionary();
            int actualCapacity = dictionary.EnsureCapacity(s_defaultCount);
            Assert.True(actualCapacity >= s_defaultCount);
            ValidateDefaultOrderedDictionaryIsContiguous(dictionary);
        }

        [Fact]
        public void EnsureCapacity_Succeeds_WhenCapacityIsLessThanCount()
        {
            OrderedDictionary<string, int> dictionary = GetDefaultOrderedDictionary();
            int actualCapacity = dictionary.EnsureCapacity(s_defaultCount - 1);
            Assert.True(actualCapacity >= s_defaultCount);
            ValidateDefaultOrderedDictionaryIsContiguous(dictionary);
        }

        [Fact]
        public void EnsureCapacity_ThrowsArgumentOutOfRangeException_WhenCapacityIsLessThanZero()
        {
            OrderedDictionary<string, int> dictionary = GetDefaultOrderedDictionary();
            Assert.Throws<ArgumentOutOfRangeException>(() => dictionary.EnsureCapacity(-1));
            ValidateDefaultOrderedDictionaryIsContiguous(dictionary);
        }

        [Fact]
        public void TrimExcess()
        {
            OrderedDictionary<string, int> dictionary = GetDefaultOrderedDictionary(4);
            dictionary.RemoveAt(3);
            dictionary.TrimExcess();
            ValidateDefaultOrderedDictionaryIsContiguous(dictionary, 3);
        }

        [Fact]
        public void TrimExcess_Succeeds_WhenCapacityIsGreaterThanCount()
        {
            OrderedDictionary<string, int> dictionary = GetDefaultOrderedDictionary(4);
            dictionary.RemoveAt(3);
            dictionary.TrimExcess(4);
            ValidateDefaultOrderedDictionaryIsContiguous(dictionary, 3);
        }

        [Fact]
        public void TrimExcess_Succeeds_WhenCapacityIsCount()
        {
            OrderedDictionary<string, int> dictionary = GetDefaultOrderedDictionary(4);
            dictionary.RemoveAt(3);
            dictionary.TrimExcess(3);
            ValidateDefaultOrderedDictionaryIsContiguous(dictionary, 3);
        }

        [Fact]
        public void TrimExcess_ThrowsArgumentOutOfRangeException_WhenCapacityIsLessThanCount()
        {
            OrderedDictionary<string, int> dictionary = GetDefaultOrderedDictionary(4);
            dictionary.RemoveAt(3);
            Assert.Throws<ArgumentOutOfRangeException>(() => dictionary.TrimExcess(2));
            ValidateDefaultOrderedDictionaryIsContiguous(dictionary, 3);
        }

        [Fact]
        public void TrimExcess_ThrowsArgumentOutOfRangeException_WhenCapacityIsLessThanZero()
        {
            OrderedDictionary<string, int> dictionary = GetDefaultOrderedDictionary(4);
            dictionary.RemoveAt(3);
            Assert.Throws<ArgumentOutOfRangeException>(() => dictionary.TrimExcess(-1));
            ValidateDefaultOrderedDictionaryIsContiguous(dictionary, 3);
        }

        [Fact]
        public void Clear()
        {
            OrderedDictionary<string, int> dictionary = GetDefaultOrderedDictionary();

            dictionary.Clear();
            Assert.Equal(0, dictionary.Count);
            foreach (KeyValuePair<string, int> pair in dictionary)
            {
                Assert.True(false);
            }
            Assert.Equal(0, dictionary.Keys.Count);
            foreach (string key in dictionary.Keys)
            {
                Assert.True(false);
            }
            Assert.Equal(0, dictionary.Values.Count);
            foreach (int value in dictionary.Values)
            {
                Assert.True(false);
            }
        }

        [Fact]
        public void Constructor_Succeeds_WhenUsingNoParameters()
        {
            OrderedDictionary<string, int> dictionary = new OrderedDictionary<string, int>();
        }

        [Fact]
        public void Constructor_Succeeds_WhenCapacityIsZero()
        {
            OrderedDictionary<string, int> dictionary = new OrderedDictionary<string, int>(0);
            dictionary = new OrderedDictionary<string, int>(0, StringComparer.OrdinalIgnoreCase);
        }

        [Fact]
        public void Constructor_Succeeds_WhenCapacityIsGreaterThanZero()
        {
            OrderedDictionary<string, int> dictionary = new OrderedDictionary<string, int>(10);
            dictionary = new OrderedDictionary<string, int>(10, StringComparer.OrdinalIgnoreCase);
        }

        [Fact]
        public void Constructor_ThrowsArgumentOutOfRangeException_WhenCapacityIsLessThanZero()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new OrderedDictionary<string, int>(-1));
            Assert.Throws<ArgumentOutOfRangeException>(() => new OrderedDictionary<string, int>(-1, StringComparer.OrdinalIgnoreCase));
        }

        [Fact]
        public void Constructor_Succeeds_WhenComparerIsNull()
        {
            OrderedDictionary<string, int> dictionary = new OrderedDictionary<string, int>((IEqualityComparer<string>)null);
            dictionary = new OrderedDictionary<string, int>(10, null);
            dictionary = new OrderedDictionary<string, int>(new[] { KeyValuePair.Create("1", 1) }, null);
        }

        [Fact]
        public void Constructor_Succeeds_WhenComparerIsNotNull()
        {
            OrderedDictionary<string, int> dictionary = new OrderedDictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            dictionary = new OrderedDictionary<string, int>(10, StringComparer.OrdinalIgnoreCase);
            dictionary = new OrderedDictionary<string, int>(new[] { KeyValuePair.Create("1", 1) }, StringComparer.OrdinalIgnoreCase);
        }

        [Fact]
        public void Constructor_Succeeds_WhenCollectionIsProvided()
        {
            List<KeyValuePair<string, int>> collection = Enumerable.Range(0, s_defaultCount).Select(i => KeyValuePair.Create(i.ToString(), i)).ToList();
            OrderedDictionary<string, int> dictionary = new OrderedDictionary<string, int>(collection);
            ValidateDefaultOrderedDictionaryIsContiguous(dictionary);
            new OrderedDictionary<string, int>(collection, StringComparer.OrdinalIgnoreCase);
            ValidateDefaultOrderedDictionaryIsContiguous(dictionary);
        }

        [Fact]
        public void Constructor_ThrowsArgumentNullException_WhenCollectionIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new OrderedDictionary<string, int>((IEnumerable<KeyValuePair<string, int>>)null));
            Assert.Throws<ArgumentNullException>(() => new OrderedDictionary<string, int>(null, StringComparer.OrdinalIgnoreCase));
        }

        [Fact]
        public void Move_Succeeds_WhenIndicesAreValidAndFromIsLessThanTo()
        {
            OrderedDictionary<string, int> dictionary = GetDefaultOrderedDictionary();
            dictionary.Move(0, 3);
            Assert.Equal(s_defaultCount, dictionary.Count);
            for (int i = 0; i < s_defaultCount; ++i)
            {
                string key = i.ToString();
                Assert.True(dictionary.ContainsKey(key));
                Assert.Equal(i, dictionary[key]);
                int index = dictionary.IndexOf(key);
                if (i == 0)
                {
                    Assert.Equal(3, index);
                }
                else if (i <= 3)
                {
                    Assert.Equal(i - 1, index);
                }
                else
                {
                    Assert.Equal(i, index);
                }
                int value = dictionary[i];
                if (i < 3)
                {
                    Assert.Equal(i + 1, value);
                }
                else if (i == 3)
                {
                    Assert.Equal(0, value);
                }
                else
                {
                    Assert.Equal(i, value);
                }
            }
        }

        [Fact]
        public void Move_Succeeds_WhenIndicesAreValidAndFromIsGreaterThanTo()
        {
            OrderedDictionary<string, int> dictionary = GetDefaultOrderedDictionary();
            dictionary.Move(3, 0);
            Assert.Equal(s_defaultCount, dictionary.Count);
            for (int i = 0; i < s_defaultCount; ++i)
            {
                string key = i.ToString();
                Assert.True(dictionary.ContainsKey(key));
                Assert.Equal(i, dictionary[key]);
                int index = dictionary.IndexOf(key);
                if (i == 3)
                {
                    Assert.Equal(0, index);
                }
                else if (i < 3)
                {
                    Assert.Equal(i + 1, index);
                }
                else
                {
                    Assert.Equal(i, index);
                }
                int value = dictionary[i];
                if (i == 0)
                {
                    Assert.Equal(3, value);
                }
                else if (i <= 3)
                {
                    Assert.Equal(i - 1, value);
                }
                else
                {
                    Assert.Equal(i, value);
                }
            }
        }

        [Fact]
        public void Move_Succeeds_WhenIndicesAreValidAndEqual()
        {
            OrderedDictionary<string, int> dictionary = GetDefaultOrderedDictionary();
            dictionary.Move(2, 2);
            ValidateDefaultOrderedDictionaryIsContiguous(dictionary);
        }

        [Fact]
        public void Move_ThrowsArgumentOutOfRangeException_WhenFromIndexIsLessThanZero()
        {
            OrderedDictionary<string, int> dictionary = GetDefaultOrderedDictionary();
            Assert.Throws<ArgumentOutOfRangeException>(() => dictionary.Move(-1, 1));
            ValidateDefaultOrderedDictionaryIsContiguous(dictionary);
        }

        [Fact]
        public void Move_ThrowsArgumentOutOfRangeException_WhenFromIndexIsCount()
        {
            OrderedDictionary<string, int> dictionary = GetDefaultOrderedDictionary();
            Assert.Throws<ArgumentOutOfRangeException>(() => dictionary.Move(s_defaultCount, 1));
            ValidateDefaultOrderedDictionaryIsContiguous(dictionary);
        }

        [Fact]
        public void Move_ThrowsArgumentOutOfRangeException_WhenToIndexIsLessThanZero()
        {
            OrderedDictionary<string, int> dictionary = GetDefaultOrderedDictionary();
            Assert.Throws<ArgumentOutOfRangeException>(() => dictionary.Move(1, -1));
            ValidateDefaultOrderedDictionaryIsContiguous(dictionary);
        }

        [Fact]
        public void Move_ThrowsArgumentOutOfRangeException_WhenToIndexIsCount()
        {
            OrderedDictionary<string, int> dictionary = GetDefaultOrderedDictionary();
            Assert.Throws<ArgumentOutOfRangeException>(() => dictionary.Move(1, s_defaultCount));
            ValidateDefaultOrderedDictionaryIsContiguous(dictionary);
        }

        [Fact]
        public void MoveRange_Succeeds_WhenIndicesAndCountAreValidAndFromIsLessThanTo()
        {
            OrderedDictionary<string, int> dictionary = GetDefaultOrderedDictionary();
            dictionary.MoveRange(1, 3, 2);
            Assert.Equal(s_defaultCount, dictionary.Count);
            for (int i = 0; i < s_defaultCount; ++i)
            {
                string key = i.ToString();
                Assert.True(dictionary.ContainsKey(key));
                Assert.Equal(i, dictionary[key]);
                int index = dictionary.IndexOf(key);
                if (i == 0 || i > 4)
                {
                    Assert.Equal(i, index);
                }
                else if (i <= 2)
                {
                    Assert.Equal(i + 2, index);
                }
                else
                {
                    Assert.Equal(i - 2, index);
                }
                int value = dictionary[i];
                if (i == 0 || i > 4)
                {
                    Assert.Equal(i, value);
                }
                else if (i <= 2)
                {
                    Assert.Equal(i + 2, value);
                }
                else
                {
                    Assert.Equal(i - 2, value);
                }
            }
        }

        [Fact]
        public void MoveRange_Succeeds_WhenIndicesAndCountAreValidAndFromIsGreaterThanTo()
        {
            OrderedDictionary<string, int> dictionary = GetDefaultOrderedDictionary();
            dictionary.MoveRange(3, 0, 2);
            Assert.Equal(s_defaultCount, dictionary.Count);
            for (int i = 0; i < s_defaultCount; ++i)
            {
                string key = i.ToString();
                Assert.True(dictionary.ContainsKey(key));
                Assert.Equal(i, dictionary[key]);
                int index = dictionary.IndexOf(key);
                if (i > 4)
                {
                    Assert.Equal(i, index);
                }
                else if (i >= 3)
                {
                    Assert.Equal(i - 3, index);
                }
                else
                {
                    Assert.Equal(i + 2, index);
                }
                int value = dictionary[i];
                if (i > 4)
                {
                    Assert.Equal(i, value);
                }
                else if (i <= 1)
                {
                    Assert.Equal(i + 3, value);
                }
                else
                {
                    Assert.Equal(i - 2, value);
                }
            }
        }

        [Fact]
        public void MoveRange_Succeeds_WhenIndicesAndCountAreValidAndFromIsLessThanToAndOverlapping()
        {
            OrderedDictionary<string, int> dictionary = GetDefaultOrderedDictionary();
            dictionary.MoveRange(1, 2, 3);
            Assert.Equal(s_defaultCount, dictionary.Count);
            for (int i = 0; i < s_defaultCount; ++i)
            {
                string key = i.ToString();
                Assert.True(dictionary.ContainsKey(key));
                Assert.Equal(i, dictionary[key]);
                int index = dictionary.IndexOf(key);
                if (i == 0 || i > 4)
                {
                    Assert.Equal(i, index);
                }
                else if (i == 4)
                {
                    Assert.Equal(1, index);
                }
                else
                {
                    Assert.Equal(i + 1, index);
                }
                int value = dictionary[i];
                if (i == 0 || i > 4)
                {
                    Assert.Equal(i, value);
                }
                else if (i == 1)
                {
                    Assert.Equal(4, value);
                }
                else
                {
                    Assert.Equal(i - 1, value);
                }
            }
        }

        [Fact]
        public void MoveRange_Succeeds_WhenIndicesAndCountAreValidAndFromIsGreaterThanToAndOverlapping()
        {
            OrderedDictionary<string, int> dictionary = GetDefaultOrderedDictionary();
            dictionary.MoveRange(2, 0, 3);
            Assert.Equal(s_defaultCount, dictionary.Count);
            for (int i = 0; i < s_defaultCount; ++i)
            {
                string key = i.ToString();
                Assert.True(dictionary.ContainsKey(key));
                Assert.Equal(i, dictionary[key]);
                int index = dictionary.IndexOf(key);
                if (i > 4)
                {
                    Assert.Equal(i, index);
                }
                else if (i >= 2)
                {
                    Assert.Equal(i - 2, index);
                }
                else
                {
                    Assert.Equal(i + 3, index);
                }
                int value = dictionary[i];
                if (i > 4)
                {
                    Assert.Equal(i, value);
                }
                else if (i < 3)
                {
                    Assert.Equal(i + 2, value);
                }
                else
                {
                    Assert.Equal(i - 3, value);
                }
            }
        }

        [Fact]
        public void MoveRange_ThrowsArgumentOutOfRangeException_WhenFromIndexIsLessThanZero()
        {
            OrderedDictionary<string, int> dictionary = GetDefaultOrderedDictionary();
            Assert.Throws<ArgumentOutOfRangeException>(() => dictionary.MoveRange(-1, 3, 2));
            ValidateDefaultOrderedDictionaryIsContiguous(dictionary);
        }

        [Fact]
        public void MoveRange_ThrowsArgumentOutOfRangeException_WhenFromIndexIsCount()
        {
            OrderedDictionary<string, int> dictionary = GetDefaultOrderedDictionary();
            Assert.Throws<ArgumentOutOfRangeException>(() => dictionary.MoveRange(s_defaultCount, 3, 2));
            ValidateDefaultOrderedDictionaryIsContiguous(dictionary);
        }

        [Fact]
        public void MoveRange_ThrowsArgumentOutOfRangeException_WhenToIndexIsLessThanZero()
        {
            OrderedDictionary<string, int> dictionary = GetDefaultOrderedDictionary();
            Assert.Throws<ArgumentOutOfRangeException>(() => dictionary.MoveRange(3, -1, 2));
            ValidateDefaultOrderedDictionaryIsContiguous(dictionary);
        }

        [Fact]
        public void MoveRange_ThrowsArgumentOutOfRangeException_WhenToIndexIsCount()
        {
            OrderedDictionary<string, int> dictionary = GetDefaultOrderedDictionary();
            Assert.Throws<ArgumentOutOfRangeException>(() => dictionary.MoveRange(3, s_defaultCount, 2));
            ValidateDefaultOrderedDictionaryIsContiguous(dictionary);
        }

        [Fact]
        public void MoveRange_ThrowsArgumentOutOfRangeException_WhenCountIsLessThanZero()
        {
            OrderedDictionary<string, int> dictionary = GetDefaultOrderedDictionary();
            Assert.Throws<ArgumentOutOfRangeException>(() => dictionary.MoveRange(1, 3, -1));
            ValidateDefaultOrderedDictionaryIsContiguous(dictionary);
        }

        [Fact]
        public void MoveRange_ThrowsArgumentException_WhenFromIndexPlusCountIsGreaterThanCount()
        {
            OrderedDictionary<string, int> dictionary = GetDefaultOrderedDictionary();
            Assert.Throws<ArgumentException>(() => dictionary.MoveRange(s_defaultCount - 1, 1, 2));
            ValidateDefaultOrderedDictionaryIsContiguous(dictionary);
        }

        [Fact]
        public void MoveRange_ThrowsArgumentException_WhenToIndexPlusCountIsGreaterThanCount()
        {
            OrderedDictionary<string, int> dictionary = GetDefaultOrderedDictionary();
            Assert.Throws<ArgumentException>(() => dictionary.MoveRange(1, s_defaultCount - 1, 2));
            ValidateDefaultOrderedDictionaryIsContiguous(dictionary);
        }

        [Fact]
        public void GetOrAdd_GetsItem_WhenContainsKey()
        {
            OrderedDictionary<string, int> dictionary = GetDefaultOrderedDictionary();
            Assert.Equal(2, dictionary.GetOrAdd("2", 1));
            ValidateDefaultOrderedDictionaryIsContiguous(dictionary);
        }

        [Fact]
        public void GetOrAdd_AddsItem_WhenNotContainsKey()
        {
            OrderedDictionary<string, int> dictionary = GetDefaultOrderedDictionary();
            Assert.Equal(s_defaultCount, dictionary.GetOrAdd(s_defaultCount.ToString(), s_defaultCount));
            ValidateDefaultOrderedDictionaryIsContiguous(dictionary, s_defaultCount + 1);
        }

        [Fact]
        public void GetOrAdd_ThrowsArgumentNullException_WhenKeyIsNull()
        {
            OrderedDictionary<string, int> dictionary = GetDefaultOrderedDictionary();
            Assert.Throws<ArgumentNullException>(() => dictionary.GetOrAdd(null, 0));
            ValidateDefaultOrderedDictionaryIsContiguous(dictionary);
        }

        [Fact]
        public void GetOrAdd_GetsItemAndDoesNotInvokeValueFactory_WhenContainsKey()
        {
            OrderedDictionary<string, int> dictionary = GetDefaultOrderedDictionary();
            bool invoked = false;
            Assert.Equal(2, dictionary.GetOrAdd("2", () =>
            {
                invoked = true;
                return 1;
            }));
            Assert.False(invoked);
            ValidateDefaultOrderedDictionaryIsContiguous(dictionary);
        }

        [Fact]
        public void GetOrAdd_GetsItemAndInvokesValueFactory_WhenNotContainsKey()
        {
            OrderedDictionary<string, int> dictionary = GetDefaultOrderedDictionary();
            bool invoked = false;
            Assert.Equal(s_defaultCount, dictionary.GetOrAdd(s_defaultCount.ToString(), () =>
            {
                invoked = true;
                return s_defaultCount;
            }));
            Assert.True(invoked);
            ValidateDefaultOrderedDictionaryIsContiguous(dictionary, s_defaultCount + 1);
        }

        [Fact]
        public void GetOrAdd_ThrowsArgumentNullException_WhenKeyIsNull2()
        {
            OrderedDictionary<string, int> dictionary = GetDefaultOrderedDictionary();
            Assert.Throws<ArgumentNullException>(() => dictionary.GetOrAdd(null, () => 0));
            ValidateDefaultOrderedDictionaryIsContiguous(dictionary);
        }

        [Fact]
        public void GetOrAdd_ThrowsArgumentNullException_WhenValueFactoryIsNull()
        {
            OrderedDictionary<string, int> dictionary = GetDefaultOrderedDictionary();
            Assert.Throws<ArgumentNullException>(() => dictionary.GetOrAdd("0", null));
            ValidateDefaultOrderedDictionaryIsContiguous(dictionary);
        }

        [Fact]
        public void TestWithStringComparerIgnoreCase()
        {
            OrderedDictionary<string, char> dictionary = new OrderedDictionary<string, char>(StringComparer.OrdinalIgnoreCase);
            for (char c = 'A'; c <= 'Z'; ++c)
            {
                dictionary.Add(c.ToString(), c);
            }
            Assert.Equal(dictionary.Select(p => p.Key), dictionary.Keys);
            Assert.Equal(dictionary.Select(p => p.Value), dictionary.Values);

            for (char c = 'A'; c <= 'Z'; ++c)
            {
                string key = c.ToString();
                int i = c - 'A';
                Assert.True(dictionary.ContainsKey(key));
                Assert.Equal(i, dictionary.IndexOf(key));
                Assert.Equal(c, dictionary[key]);
                Assert.Equal(c, dictionary[i]);
                Assert.Equal(key, dictionary.Keys[i]);
                Assert.True(dictionary.TryGetValue(key, out char value));
                Assert.Equal(c, value);
            }

            for (char c = 'a'; c <= 'z'; ++c)
            {
                string key = c.ToString();
                int i = c - 'a';
                char upperC = char.ToUpper(c);
                Assert.True(dictionary.ContainsKey(key));
                Assert.Equal(i, dictionary.IndexOf(key));
                Assert.Equal(upperC, dictionary[key]);
                Assert.Equal(upperC, dictionary[i]);
                Assert.Equal(upperC.ToString(), dictionary.Keys[i]);
                Assert.True(dictionary.TryGetValue(key, out char value));
                Assert.Equal(upperC, value);
            }

            Assert.False(dictionary.ContainsKey("1"));
        }

        [Fact]
        public void TestWithIntKey()
        {
            OrderedDictionary<int, string> dictionary = new OrderedDictionary<int, string>();
            for (int i = 1; i <= s_defaultCount; ++i)
            {
                dictionary.Add(-i, i.ToString());
            }
            Assert.Equal(s_defaultCount, dictionary.Count);
            Assert.Equal(dictionary.Select(p => p.Key), dictionary.Keys);
            Assert.Equal(dictionary.Select(p => p.Value), dictionary.Values);

            for (int i = 1; i <= s_defaultCount; ++i)
            {
                string value = i.ToString();
                Assert.False(dictionary.ContainsKey(i));
                Assert.True(dictionary.ContainsKey(-i));
                Assert.Equal(-1, dictionary.IndexOf(i));
                Assert.Equal(i - 1, dictionary.IndexOf(-i));
                Assert.Throws<KeyNotFoundException>(() => dictionary[key: i]);
                Assert.Equal(value, dictionary[key: -i]);
                // Resolves to the index indexer
                Assert.Equal(value, dictionary[i - 1]);
                Assert.Equal(value, dictionary[index: i - 1]);
                Assert.Equal(-i, dictionary.Keys[i - 1]);
                Assert.False(dictionary.TryGetValue(i, out string foundValue));
                Assert.Null(foundValue);
                Assert.True(dictionary.TryGetValue(-i, out foundValue));
                Assert.Equal(value, foundValue);
            }
        }

        [Fact]
        public void TestLargeOrderedDictionaryOperations()
        {
            const int count = 1_000_000;

            // Without initializing capacity will necessitate resizing of the internal array many times
            OrderedDictionary<string, int> dictionary = new OrderedDictionary<string, int>();

            for (int i = 0; i < count; ++i)
            {
                dictionary.Add(i.ToString(), i);
            }
            Assert.Equal(dictionary.Select(p => p.Key), dictionary.Keys);
            Assert.Equal(dictionary.Select(p => p.Value), dictionary.Values);

            for (int i = 0; i < count; ++i)
            {
                string key = i.ToString();
                Assert.True(dictionary.ContainsKey(key));
                Assert.Equal(i, dictionary.IndexOf(key));
                Assert.Equal(i, dictionary[key]);
                Assert.Equal(i, dictionary[i]);
                Assert.Equal(key, dictionary.Keys[i]);
                Assert.True(dictionary.TryGetValue(key, out int value));
                Assert.Equal(i, value);
            }

            Assert.False(dictionary.ContainsKey(count.ToString()));
        }

        [Fact]
        public void RemoveSlotReused()
        {
            OrderedDictionary<Collider, int> d = new OrderedDictionary<Collider, int>();
            d[C(0)] = 0;
            d[C(1)] = 1;
            d[C(2)] = 2;
            Assert.True(d.Remove(C(0)));

            d[C(0)] = 3;
            Assert.Equal(d[C(0)], 3);
            Assert.Equal(3, d.Count);
        }

        [Fact]
        public void Insert_Succeeds_WhenThereAreCollisions()
        {
            OrderedDictionary<int, string> dictionary = new OrderedDictionary<int, string>(3);
            dictionary.Add(1, "1");
            dictionary.Insert(0, 4, "4"); // should be in the same bucket
            dictionary.Insert(0, 7, "7"); // should be in the same bucket
            Assert.Equal("7", dictionary[index: 0]);
            Assert.Equal("4", dictionary[index: 1]);
            Assert.Equal("1", dictionary[index: 2]);
            Assert.Equal("7", dictionary[key: 7]);
            Assert.Equal("1", dictionary[key: 1]);
            Assert.Equal("4", dictionary[key: 4]);
        }

        [Fact]
        public void RemoveAt_Succeeds_WhenThereAreCollisions()
        {
            OrderedDictionary<int, string> dictionary = new OrderedDictionary<int, string>(3);
            dictionary.Add(1, "1");
            dictionary.Add(4, "4"); // should be in the same bucket
            dictionary.Insert(1, 7, "7"); // should be in the same bucket
            dictionary.RemoveAt(0);
            Assert.Equal("7", dictionary[index: 0]);
            Assert.Equal("4", dictionary[index: 1]);
            Assert.Equal("7", dictionary[key: 7]);
            Assert.Equal("4", dictionary[key: 4]);
        }

        [Fact]
        public void RemoveAt_Succeeds_WhenRemovingFromTheEndOfTheChain()
        {
            OrderedDictionary<int, string> dictionary = new OrderedDictionary<int, string>(3);
            dictionary.Add(1, "1");
            dictionary.Add(4, "4"); // should be in the same bucket
            dictionary.Add(7, "7"); // should be in the same bucket
            dictionary.RemoveAt(0);
            Assert.Equal("4", dictionary[index: 0]);
            Assert.Equal("7", dictionary[index: 1]);
            Assert.Equal("7", dictionary[key: 7]);
            Assert.Equal("4", dictionary[key: 4]);
        }

        [Fact]
        public void RemoveAt_Succeeds_WhenRemovingFromTheStartOfTheChain()
        {
            OrderedDictionary<int, string> dictionary = new OrderedDictionary<int, string>(3);
            dictionary.Add(1, "1");
            dictionary.Add(4, "4"); // should be in the same bucket
            dictionary.Insert(0, 7, "7"); // should be in the same bucket
            dictionary.RemoveAt(0);
            Assert.Equal("1", dictionary[index: 0]);
            Assert.Equal("4", dictionary[index: 1]);
            Assert.Equal("1", dictionary[key: 1]);
            Assert.Equal("4", dictionary[key: 4]);
        }

        [Fact]
        public void RemoveAt_Succeeds_WhenRemovingFromTheMiddleOfTheChain()
        {
            OrderedDictionary<int, string> dictionary = new OrderedDictionary<int, string>(3);
            dictionary.Add(1, "1");
            dictionary.Insert(0, 4, "4"); // should be in the same bucket
            dictionary.Add(7, "7"); // should be in the same bucket
            dictionary.RemoveAt(0);
            Assert.Equal("1", dictionary[index: 0]);
            Assert.Equal("7", dictionary[index: 1]);
            Assert.Equal("1", dictionary[key: 1]);
            Assert.Equal("7", dictionary[key: 7]);
        }

        [Fact]
        public void OrderedDictionaryVersusDictionary_AllCollisions()
        {
            Random rand = new Random(333);
            OrderedDictionary<Collider, int> od = new OrderedDictionary<Collider, int>();
            Dictionary<Collider, int> d = new Dictionary<Collider, int>();
            int size = rand.Next(1234);
            for (int i = 0; i < size; i++)
            {
                if (rand.Next(5) != 0)
                {
                    Collider k = C(rand.Next(100) + 23);
                    int v = rand.Next();
                    if (od.TryGetValue(k, out int t))
                    {
                        od[k] = t + v;
                    }
                    else
                    {
                        od[k] = v;
                    }
                    if (d.TryGetValue(k, out t))
                    {
                        d[k] = t + v;
                    }
                    else
                    {
                        d[k] = v;
                    }
                }
                if (rand.Next(3) == 0 && d.Count > 0)
                {
                    Collider el = GetRandomElement(d);
                    Assert.True(od.Remove(el));
                    Assert.True(d.Remove(el));
                }
            }
            Assert.Equal(d.Count, od.Count);
            Assert.Equal(d.OrderBy(i => i.Key), (od.OrderBy(i => i.Key)));
            Assert.Equal(d.OrderBy(i => i.Value), (od.OrderBy(i => i.Value)));
        }

        private TKey GetRandomElement<TKey, TValue>(IDictionary<TKey, TValue> d)
        {
            int index = 0;
            Random rand = new Random(42);
            foreach (KeyValuePair<TKey, TValue> entry in d)
            {
                if (rand.Next(d.Count) == 0 || index == d.Count - 1)
                {
                    return entry.Key;
                }
                index++;
            }
            throw new InvalidOperationException();
        }

        [DebuggerStepThrough]
        private Collider C(int val) => new Collider(val);

        [DebuggerDisplay("{Key}")]
        private struct Collider : IEquatable<Collider>, IComparable<Collider>
        {
            public int Key { get; }

            [DebuggerStepThrough]
            public Collider(int key)
            {
                Key = key;
            }

            [DebuggerStepThrough]
            public override int GetHashCode() => 42;

            public override bool Equals(object obj) => obj is Collider collider && Equals(collider);

            public bool Equals(Collider that) => that.Key == Key;

            public int CompareTo(Collider that) => Key.CompareTo(that.Key);

            public override string ToString() => Key.ToString();
        }
    }
}
