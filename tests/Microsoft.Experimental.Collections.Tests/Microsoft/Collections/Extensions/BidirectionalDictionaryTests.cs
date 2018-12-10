using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Xunit;

namespace Microsoft.Collections.Extensions.Tests
{
    public class BidirectionalDictionaryTests
    {
        private const int s_defaultCount = 5; // tests rely on this being >= 5

        private BidirectionalDictionary<string, int?> GetDefaultBidirectionalDictionary(int? count = null) => new BidirectionalDictionary<string, int?>(Enumerable.Range(0, count ?? s_defaultCount).ToDictionary(i => i.ToString(), i => (int?)i));

        private static void ValidateDefaultBidirectionalDictionaryIsContiguous(BidirectionalDictionary<string, int?> dictionary, int? count = null)
        {
            Assert.Equal(count ?? s_defaultCount, dictionary.Count);
            Assert.Equal(count ?? s_defaultCount, dictionary.FirstKeys.Count);
            Assert.Equal(count ?? s_defaultCount, dictionary.SecondKeys.Count);
            for (int i = 0; i < (count ?? s_defaultCount); ++i)
            {
                string key = i.ToString();
                Assert.True(dictionary.ContainsKey(key));
                Assert.True(dictionary.Reverse.ContainsKey(i));
                Assert.Equal(i, dictionary[key]);
                Assert.Equal(key, dictionary.Reverse[i]);
                Assert.True(dictionary.TryGetValue(key, out int? value));
                Assert.Equal(i, value);
                Assert.True(dictionary.Reverse.TryGetValue(i, out string firstKey));
                Assert.Equal(key, firstKey);
            }
            Assert.Equal(dictionary.Select(p => p.Key), dictionary.FirstKeys);
            Assert.Equal(dictionary.Select(p => p.Value), dictionary.SecondKeys);
            Assert.Equal(Enumerable.Range(0, count ?? s_defaultCount).ToDictionary(i => i.ToString(), i => (int?)i), dictionary);
            Assert.Equal(Enumerable.Range(0, count ?? s_defaultCount).Select(i => i.ToString()), dictionary.FirstKeys);
            Assert.Equal(Enumerable.Range(0, count ?? s_defaultCount).Select(i => (int?)i), dictionary.SecondKeys);
        }

        [Fact]
        public void Count()
        {
            BidirectionalDictionary<string, int?> dictionary = GetDefaultBidirectionalDictionary();
            Assert.Equal(s_defaultCount, dictionary.Count);
            Assert.Equal(s_defaultCount, dictionary.Reverse.Count);
        }

        [Fact]
        public void FirstKeysCount()
        {
            BidirectionalDictionary<string, int?> dictionary = GetDefaultBidirectionalDictionary();
            Assert.Equal(s_defaultCount, dictionary.FirstKeys.Count);
        }

        [Fact]
        public void SecondKeysCount()
        {
            BidirectionalDictionary<string, int?> dictionary = GetDefaultBidirectionalDictionary();
            Assert.Equal(s_defaultCount, dictionary.SecondKeys.Count);
        }

        [Fact]
        public void FirstKeys()
        {
            BidirectionalDictionary<string, int?> dictionary = GetDefaultBidirectionalDictionary();
            Assert.Equal(Enumerable.Range(0, s_defaultCount).Select(i => i.ToString()), dictionary.FirstKeys);
        }

        [Fact]
        public void SecondKeys()
        {
            BidirectionalDictionary<string, int?> dictionary = GetDefaultBidirectionalDictionary();
            Assert.Equal(Enumerable.Range(0, s_defaultCount).Select(i => (int?)i), dictionary.SecondKeys);
        }

        [Fact]
        public void Reverse()
        {
            BidirectionalDictionary<string, int?> dictionary = GetDefaultBidirectionalDictionary();
            Assert.Same(dictionary, dictionary.Reverse.Reverse);
            Assert.Same(dictionary.FirstKeys, dictionary.Reverse.SecondKeys);
            Assert.Same(dictionary.SecondKeys, dictionary.Reverse.FirstKeys);
        }

        [Fact]
        public void Comparer_IsDefaultEqualityComparer_WhenNotProvided()
        {
            BidirectionalDictionary<string, int?> dictionary = new BidirectionalDictionary<string, int?>();
            Assert.Same(EqualityComparer<string>.Default, dictionary.FirstKeyComparer);
            Assert.Same(EqualityComparer<int?>.Default, dictionary.SecondKeyComparer);
            Assert.Same(dictionary.FirstKeyComparer, dictionary.Reverse.SecondKeyComparer);
            Assert.Same(dictionary.SecondKeyComparer, dictionary.Reverse.FirstKeyComparer);
        }

        [Fact]
        public void Comparer_IsDefaultEqualityComparer_WhenNull()
        {
            BidirectionalDictionary<string, int?> dictionary = new BidirectionalDictionary<string, int?>(null, null);
            Assert.Same(EqualityComparer<string>.Default, dictionary.FirstKeyComparer);
            Assert.Same(EqualityComparer<int?>.Default, dictionary.SecondKeyComparer);
            Assert.Same(dictionary.FirstKeyComparer, dictionary.Reverse.SecondKeyComparer);
            Assert.Same(dictionary.SecondKeyComparer, dictionary.Reverse.FirstKeyComparer);
        }

        [Fact]
        public void Comparer_IsDefaultEqualityComparer_WhenDefaultIsProvided()
        {
            BidirectionalDictionary<string, int?> dictionary = new BidirectionalDictionary<string, int?>(EqualityComparer<string>.Default, EqualityComparer<int?>.Default);
            Assert.Same(EqualityComparer<string>.Default, dictionary.FirstKeyComparer);
            Assert.Same(EqualityComparer<int?>.Default, dictionary.SecondKeyComparer);
            Assert.Same(dictionary.FirstKeyComparer, dictionary.Reverse.SecondKeyComparer);
            Assert.Same(dictionary.SecondKeyComparer, dictionary.Reverse.FirstKeyComparer);
        }

        [Fact]
        public void Comparer_IsProvided_WhenProvided()
        {
            BidirectionalDictionary<string, int?> dictionary = new BidirectionalDictionary<string, int?>(StringComparer.OrdinalIgnoreCase, CustomIntComparer.Instance);
            Assert.Same(StringComparer.OrdinalIgnoreCase, dictionary.FirstKeyComparer);
            Assert.Same(CustomIntComparer.Instance, dictionary.SecondKeyComparer);
            Assert.Same(dictionary.FirstKeyComparer, dictionary.Reverse.SecondKeyComparer);
            Assert.Same(dictionary.SecondKeyComparer, dictionary.Reverse.FirstKeyComparer);
        }

        [Fact]
        public void ContainsKey_ReturnsTrue_WhenContainsKey()
        {
            BidirectionalDictionary<string, int?> dictionary = GetDefaultBidirectionalDictionary();
            for (int i = 0; i < s_defaultCount; ++i)
            {
                Assert.True(dictionary.ContainsKey(i.ToString()));
                Assert.True(dictionary.Reverse.ContainsKey(i));
            }
        }

        [Fact]
        public void ContainsKey_ReturnsFalse_WhenNotContainsKey()
        {
            BidirectionalDictionary<string, int?> dictionary = GetDefaultBidirectionalDictionary();
            Assert.False(dictionary.ContainsKey(s_defaultCount.ToString()));
            Assert.False(dictionary.Reverse.ContainsKey(s_defaultCount));
        }

        [Fact]
        public void ContainsKey_ThrowsArgumentNullException_WhenKeyIsNull()
        {
            BidirectionalDictionary<string, int?> dictionary = GetDefaultBidirectionalDictionary();
            Assert.Throws<ArgumentNullException>(() => dictionary.ContainsKey(null));
            Assert.Throws<ArgumentNullException>(() => dictionary.Reverse.ContainsKey(null));
        }

        [Fact]
        public void IndexerGetter_ReturnsValue_WhenContainsKey()
        {
            BidirectionalDictionary<string, int?> dictionary = GetDefaultBidirectionalDictionary();
            for (int i = 0; i < s_defaultCount; ++i)
            {
                string key = i.ToString();
                Assert.Equal(i, dictionary[key]);
                Assert.Equal(key, dictionary.Reverse[i]);
            }
        }

        [Fact]
        public void IndexerGetter_ThrowsKeyNotFoundException_WhenNotContainsKey()
        {
            BidirectionalDictionary<string, int?> dictionary = GetDefaultBidirectionalDictionary();
            Assert.Throws<KeyNotFoundException>(() => dictionary[s_defaultCount.ToString()]);
            Assert.Throws<KeyNotFoundException>(() => dictionary.Reverse[s_defaultCount]);
        }

        [Fact]
        public void IndexerGetter_ThrowsArgumentNullException_WhenKeyIsNull()
        {
            BidirectionalDictionary<string, int?> dictionary = GetDefaultBidirectionalDictionary();
            Assert.Throws<ArgumentNullException>(() => dictionary[null]);
            Assert.Throws<ArgumentNullException>(() => dictionary.Reverse[null]);
        }

        [Fact]
        public void Remove_ReturnsTrue_WhenContainsKey()
        {
            BidirectionalDictionary<string, int?> dictionary = GetDefaultBidirectionalDictionary();
            Assert.True(dictionary.Remove("1"));
            ValidateDefaultDictionaryWith1Removed(dictionary);
            dictionary = GetDefaultBidirectionalDictionary();
            Assert.True(dictionary.Reverse.Remove(1));
            ValidateDefaultDictionaryWith1Removed(dictionary);
        }

        [Fact]
        public void Remove_ReturnsTrueAndValue_WhenContainsKey()
        {
            BidirectionalDictionary<string, int?> dictionary = GetDefaultBidirectionalDictionary();
            Assert.True(dictionary.Remove("1", out int? value));
            Assert.Equal(1, value);
            ValidateDefaultDictionaryWith1Removed(dictionary);
            dictionary = GetDefaultBidirectionalDictionary();
            Assert.True(dictionary.Reverse.Remove(1, out string key));
            Assert.Equal("1", key);
            ValidateDefaultDictionaryWith1Removed(dictionary);
        }

        private void ValidateDefaultDictionaryWith1Removed(BidirectionalDictionary<string, int?> dictionary)
        {
            Assert.Equal(s_defaultCount - 1, dictionary.Count);
            Assert.Equal(s_defaultCount - 1, dictionary.FirstKeys.Count);
            Assert.Equal(s_defaultCount - 1, dictionary.SecondKeys.Count);
            for (int i = 0; i < s_defaultCount; ++i)
            {
                string key = i.ToString();
                if (i == 1)
                {
                    Assert.False(dictionary.ContainsKey(key));
                    Assert.False(dictionary.Reverse.ContainsKey(i));
                    Assert.Throws<KeyNotFoundException>(() => dictionary[key]);
                    Assert.Throws<KeyNotFoundException>(() => dictionary.Reverse[i]);
                    Assert.False(dictionary.TryGetValue(key, out int? value));
                    Assert.Null(value);
                    Assert.False(dictionary.Reverse.TryGetValue(i, out string firstKey));
                    Assert.Null(firstKey);
                }
                else
                {
                    Assert.True(dictionary.ContainsKey(key));
                    Assert.True(dictionary.Reverse.ContainsKey(i));
                    Assert.Equal(i, dictionary[key]);
                    Assert.Equal(key, dictionary.Reverse[i]);
                    Assert.True(dictionary.TryGetValue(key, out int? value));
                    Assert.Equal(i, value);
                    Assert.True(dictionary.Reverse.TryGetValue(i, out string firstKey));
                    Assert.Equal(key, firstKey);
                }
            }
            Assert.Equal(dictionary.Select(p => p.Key), dictionary.FirstKeys);
            Assert.Equal(dictionary.Select(p => p.Value), dictionary.SecondKeys);
        }

        [Fact]
        public void Remove_ReturnsFalse_WhenNotContainsKey()
        {
            BidirectionalDictionary<string, int?> dictionary = GetDefaultBidirectionalDictionary();
            Assert.False(dictionary.Remove(s_defaultCount.ToString()));
            Assert.False(dictionary.Remove(s_defaultCount.ToString(), out int? value));
            Assert.Null(value);
            ValidateDefaultBidirectionalDictionaryIsContiguous(dictionary);
            dictionary = GetDefaultBidirectionalDictionary();
            Assert.False(dictionary.Reverse.Remove(s_defaultCount));
            Assert.False(dictionary.Reverse.Remove(s_defaultCount, out string key));
            Assert.Null(key);
            ValidateDefaultBidirectionalDictionaryIsContiguous(dictionary);
        }

        [Fact]
        public void Remove_ThrowsArgumentNullException_WhenKeyIsNull()
        {
            BidirectionalDictionary<string, int?> dictionary = GetDefaultBidirectionalDictionary();
            Assert.Throws<ArgumentNullException>(() => dictionary.Remove(null));
            Assert.Throws<ArgumentNullException>(() => dictionary.Remove(null, out _));
            ValidateDefaultBidirectionalDictionaryIsContiguous(dictionary);
            dictionary = GetDefaultBidirectionalDictionary();
            Assert.Throws<ArgumentNullException>(() => dictionary.Reverse.Remove(null));
            Assert.Throws<ArgumentNullException>(() => dictionary.Reverse.Remove(null, out _));
            ValidateDefaultBidirectionalDictionaryIsContiguous(dictionary);
        }

        [Fact]
        public void Enumeration()
        {
            BidirectionalDictionary<string, int?> dictionary = GetDefaultBidirectionalDictionary();

            int temp = 0;
            foreach (KeyValuePair<string, int?> pair in dictionary)
            {
                Assert.Equal(temp.ToString(), pair.Key);
                Assert.Equal(temp, pair.Value);
                ++temp;
            }
            Assert.Equal(s_defaultCount, temp);

            temp = 0;
            foreach (KeyValuePair<int?, string> pair in dictionary.Reverse)
            {
                Assert.Equal(temp, pair.Key);
                Assert.Equal(temp.ToString(), pair.Value);
                ++temp;
            }
            Assert.Equal(s_defaultCount, temp);
        }

        [Fact]
        public void FirstKeysEnumeration()
        {
            BidirectionalDictionary<string, int?> dictionary = GetDefaultBidirectionalDictionary();

            int temp = 0;
            foreach (string key in dictionary.FirstKeys)
            {
                Assert.Equal(temp.ToString(), key);
                ++temp;
            }
            Assert.Equal(s_defaultCount, temp);
        }

        [Fact]
        public void SecondKeysEnumeration()
        {
            BidirectionalDictionary<string, int?> dictionary = GetDefaultBidirectionalDictionary();

            int temp = 0;
            foreach (int? value in dictionary.SecondKeys)
            {
                Assert.Equal(temp, value);
                ++temp;
            }
            Assert.Equal(s_defaultCount, temp);
        }

        [Fact]
        public void Add_ReturnsTrue_WhenNotContainsEitherKey()
        {
            BidirectionalDictionary<string, int?> dictionary = GetDefaultBidirectionalDictionary();
            Assert.True(dictionary.Add(s_defaultCount.ToString(), s_defaultCount));
            ValidateDefaultBidirectionalDictionaryIsContiguous(dictionary, s_defaultCount + 1);
            dictionary = GetDefaultBidirectionalDictionary();
            Assert.True(dictionary.Reverse.Add(s_defaultCount, s_defaultCount.ToString()));
            ValidateDefaultBidirectionalDictionaryIsContiguous(dictionary, s_defaultCount + 1);
        }

        [Fact]
        public void Add_ReturnsFalse_WhenContainsFirstKey()
        {
            BidirectionalDictionary<string, int?> dictionary = GetDefaultBidirectionalDictionary();
            Assert.False(dictionary.Add("2", s_defaultCount));
            ValidateDefaultBidirectionalDictionaryIsContiguous(dictionary);
            dictionary = GetDefaultBidirectionalDictionary();
            Assert.False(dictionary.Reverse.Add(2, s_defaultCount.ToString()));
            ValidateDefaultBidirectionalDictionaryIsContiguous(dictionary);
        }

        [Fact]
        public void Add_ReturnsFalse_WhenContainsSecondKey()
        {
            BidirectionalDictionary<string, int?> dictionary = GetDefaultBidirectionalDictionary();
            Assert.False(dictionary.Add(s_defaultCount.ToString(), 2));
            ValidateDefaultBidirectionalDictionaryIsContiguous(dictionary);
            dictionary = GetDefaultBidirectionalDictionary();
            Assert.False(dictionary.Reverse.Add(s_defaultCount, "2"));
            ValidateDefaultBidirectionalDictionaryIsContiguous(dictionary);
        }

        [Fact]
        public void Add_ThrowsArgumentNullException_WhenFirstKeyIsNull()
        {
            BidirectionalDictionary<string, int?> dictionary = GetDefaultBidirectionalDictionary();
            Assert.Throws<ArgumentNullException>(() => dictionary.Add(null, s_defaultCount));
            ValidateDefaultBidirectionalDictionaryIsContiguous(dictionary);
            dictionary = GetDefaultBidirectionalDictionary();
            Assert.Throws<ArgumentNullException>(() => dictionary.Reverse.Add(null, s_defaultCount.ToString()));
            ValidateDefaultBidirectionalDictionaryIsContiguous(dictionary);
        }

        [Fact]
        public void Add_ThrowsArgumentNullException_WhenSecondKeyIsNull()
        {
            BidirectionalDictionary<string, int?> dictionary = GetDefaultBidirectionalDictionary();
            Assert.Throws<ArgumentNullException>(() => dictionary.Add(s_defaultCount.ToString(), null));
            ValidateDefaultBidirectionalDictionaryIsContiguous(dictionary);
            dictionary = GetDefaultBidirectionalDictionary();
            Assert.Throws<ArgumentNullException>(() => dictionary.Reverse.Add(s_defaultCount, null));
            ValidateDefaultBidirectionalDictionaryIsContiguous(dictionary);
        }

        [Fact]
        public void TryGetValue_ReturnsTrueAndValue_WhenContainsKey()
        {
            BidirectionalDictionary<string, int?> dictionary = GetDefaultBidirectionalDictionary();
            for (int i = 0; i < s_defaultCount; ++i)
            {
                string key = i.ToString();
                Assert.True(dictionary.TryGetValue(key, out int? value));
                Assert.Equal(i, value);
                Assert.True(dictionary.Reverse.TryGetValue(i, out string firstKey));
                Assert.Equal(key, firstKey);
            }
        }

        [Fact]
        public void TryGetValue_ReturnsFalseAndDefaultValue_WhenNotContainsKey()
        {
            BidirectionalDictionary<string, int?> dictionary = GetDefaultBidirectionalDictionary();
            Assert.False(dictionary.TryGetValue(s_defaultCount.ToString(), out int? value));
            Assert.Null(value);
            Assert.False(dictionary.Reverse.TryGetValue(s_defaultCount, out string firstKey));
            Assert.Null(firstKey);
        }

        [Fact]
        public void TryGetValue_ThrowsArgumentNullException_WhenKeyIsNull()
        {
            BidirectionalDictionary<string, int?> dictionary = GetDefaultBidirectionalDictionary();
            Assert.Throws<ArgumentNullException>(() => dictionary.TryGetValue(null, out _));
            Assert.Throws<ArgumentNullException>(() => dictionary.Reverse.TryGetValue(null, out _));
        }

        [Fact]
        public void IndexerSetter_Succeeds_WhenContainsFirstKey()
        {
            BidirectionalDictionary<string, int?> dictionary = GetDefaultBidirectionalDictionary();
            dictionary["3"] = s_defaultCount;
            ValidateDefaultOrderedDictionaryWith3sSecondKeyEqualToDefaultCount(dictionary);
            dictionary = GetDefaultBidirectionalDictionary();
            dictionary.Reverse[3] = s_defaultCount.ToString();
            ValidateDefaultOrderedDictionaryWith3sFirstKeyEqualToDefaultCount(dictionary);
        }

        private void ValidateDefaultOrderedDictionaryWith3sSecondKeyEqualToDefaultCount(BidirectionalDictionary<string, int?> dictionary)
        {
            Assert.Equal(s_defaultCount, dictionary.Count);
            Assert.Equal(s_defaultCount, dictionary.FirstKeys.Count);
            Assert.Equal(s_defaultCount, dictionary.SecondKeys.Count);
            for (int i = 0; i < s_defaultCount; ++i)
            {
                string key = i.ToString();
                Assert.True(dictionary.ContainsKey(key));
                Assert.True(dictionary.TryGetValue(key, out int? value));
                if (i == 3)
                {
                    Assert.Equal(s_defaultCount, dictionary[key]);
                    Assert.Equal(s_defaultCount, value);
                    Assert.False(dictionary.Reverse.ContainsKey(i));
                }
                else
                {
                    Assert.Equal(i, dictionary[key]);
                    Assert.Equal(i, value);
                    Assert.True(dictionary.Reverse.ContainsKey(i));
                    Assert.Equal(key, dictionary.Reverse[i]);
                    Assert.True(dictionary.Reverse.TryGetValue(i, out string firstKey));
                    Assert.Equal(key, firstKey);
                }
            }
            Assert.Equal(dictionary.Select(p => p.Key), dictionary.FirstKeys);
            Assert.Equal(dictionary.Select(p => p.Value), dictionary.SecondKeys);
        }

        private void ValidateDefaultOrderedDictionaryWith3sFirstKeyEqualToDefaultCount(BidirectionalDictionary<string, int?> dictionary)
        {
            Assert.Equal(s_defaultCount, dictionary.Count);
            Assert.Equal(s_defaultCount, dictionary.FirstKeys.Count);
            Assert.Equal(s_defaultCount, dictionary.SecondKeys.Count);
            for (int i = 0; i < s_defaultCount; ++i)
            {
                string key = i.ToString();
                Assert.True(dictionary.Reverse.ContainsKey(i));
                Assert.True(dictionary.Reverse.TryGetValue(i, out string value));
                if (i == 3)
                {
                    Assert.Equal(s_defaultCount.ToString(), dictionary.Reverse[i]);
                    Assert.Equal(s_defaultCount.ToString(), value);
                    Assert.False(dictionary.ContainsKey(key));
                }
                else
                {
                    Assert.Equal(key, dictionary.Reverse[i]);
                    Assert.Equal(key, value);
                    Assert.True(dictionary.ContainsKey(key));
                    Assert.Equal(i, dictionary[key]);
                    Assert.True(dictionary.TryGetValue(key, out int? secondKey));
                    Assert.Equal(i, secondKey);
                }
            }
            Assert.Equal(dictionary.Select(p => p.Key), dictionary.FirstKeys);
            Assert.Equal(dictionary.Select(p => p.Value), dictionary.SecondKeys);
        }

        [Fact]
        public void IndexerSetter_Succeeds_WhenNotContainsKeys()
        {
            BidirectionalDictionary<string, int?> dictionary = GetDefaultBidirectionalDictionary();
            dictionary[s_defaultCount.ToString()] = s_defaultCount;
            ValidateDefaultBidirectionalDictionaryIsContiguous(dictionary, s_defaultCount + 1);
            dictionary = GetDefaultBidirectionalDictionary();
            dictionary.Reverse[s_defaultCount] = s_defaultCount.ToString();
            ValidateDefaultBidirectionalDictionaryIsContiguous(dictionary, s_defaultCount + 1);
        }

        [Fact]
        public void IndexerSetter_ThrowsArgumentNullException_WhenKeyIsNull()
        {
            BidirectionalDictionary<string, int?> dictionary = GetDefaultBidirectionalDictionary();
            Assert.Throws<ArgumentNullException>(() => dictionary[null] = s_defaultCount);
            Assert.Throws<ArgumentNullException>(() => dictionary.Reverse[null] = s_defaultCount.ToString());
            Assert.Throws<ArgumentNullException>(() => dictionary[s_defaultCount.ToString()] = null);
            Assert.Throws<ArgumentNullException>(() => dictionary.Reverse[s_defaultCount] = null);
            ValidateDefaultBidirectionalDictionaryIsContiguous(dictionary, s_defaultCount);
        }

        [Fact]
        public void IndexerSetter_ThrowsArgumentException_WhenContainsSecondKeyAssociatedWithDifferentFirstKey()
        {
            BidirectionalDictionary<string, int?> dictionary = GetDefaultBidirectionalDictionary();
            Assert.Throws<ArgumentException>(() => dictionary["1"] = 3);
            Assert.Throws<ArgumentException>(() => dictionary.Reverse[3] = "1");
            ValidateDefaultBidirectionalDictionaryIsContiguous(dictionary, s_defaultCount);
        }

        [Fact]
        public void EnsureCapacity_Succeeds_WhenCapacityIsGreaterThanCount()
        {
            BidirectionalDictionary<string, int?> dictionary = GetDefaultBidirectionalDictionary();
            int requestedCapacity = s_defaultCount << 1;
            int actualCapacity = dictionary.EnsureCapacity(requestedCapacity);
            Assert.True(actualCapacity >= requestedCapacity);
            ValidateDefaultBidirectionalDictionaryIsContiguous(dictionary);
            dictionary = GetDefaultBidirectionalDictionary();
            actualCapacity = dictionary.Reverse.EnsureCapacity(requestedCapacity);
            Assert.True(actualCapacity >= requestedCapacity);
            ValidateDefaultBidirectionalDictionaryIsContiguous(dictionary);
        }

        [Fact]
        public void EnsureCapacity_Succeeds_WhenCapacityIsCount()
        {
            BidirectionalDictionary<string, int?> dictionary = GetDefaultBidirectionalDictionary();
            int actualCapacity = dictionary.EnsureCapacity(s_defaultCount);
            Assert.True(actualCapacity >= s_defaultCount);
            ValidateDefaultBidirectionalDictionaryIsContiguous(dictionary);
            dictionary = GetDefaultBidirectionalDictionary();
            actualCapacity = dictionary.Reverse.EnsureCapacity(s_defaultCount);
            Assert.True(actualCapacity >= s_defaultCount);
            ValidateDefaultBidirectionalDictionaryIsContiguous(dictionary);
        }

        [Fact]
        public void EnsureCapacity_Succeeds_WhenCapacityIsLessThanCount()
        {
            BidirectionalDictionary<string, int?> dictionary = GetDefaultBidirectionalDictionary();
            int actualCapacity = dictionary.EnsureCapacity(s_defaultCount - 1);
            Assert.True(actualCapacity >= s_defaultCount);
            ValidateDefaultBidirectionalDictionaryIsContiguous(dictionary);
            dictionary = GetDefaultBidirectionalDictionary();
            actualCapacity = dictionary.Reverse.EnsureCapacity(s_defaultCount - 1);
            Assert.True(actualCapacity >= s_defaultCount);
            ValidateDefaultBidirectionalDictionaryIsContiguous(dictionary);
        }

        [Fact]
        public void EnsureCapacity_ThrowsArgumentOutOfRangeException_WhenCapacityIsLessThanZero()
        {
            BidirectionalDictionary<string, int?> dictionary = GetDefaultBidirectionalDictionary();
            Assert.Throws<ArgumentOutOfRangeException>(() => dictionary.EnsureCapacity(-1));
            ValidateDefaultBidirectionalDictionaryIsContiguous(dictionary);
            dictionary = GetDefaultBidirectionalDictionary();
            Assert.Throws<ArgumentOutOfRangeException>(() => dictionary.Reverse.EnsureCapacity(-1));
            ValidateDefaultBidirectionalDictionaryIsContiguous(dictionary);
        }

        [Fact]
        public void TrimExcess()
        {
            BidirectionalDictionary<string, int?> dictionary = GetDefaultBidirectionalDictionary(4);
            dictionary.Remove(3.ToString());
            dictionary.TrimExcess();
            ValidateDefaultBidirectionalDictionaryIsContiguous(dictionary, 3);
            dictionary = GetDefaultBidirectionalDictionary(4);
            dictionary.Remove(3.ToString());
            dictionary.Reverse.TrimExcess();
            ValidateDefaultBidirectionalDictionaryIsContiguous(dictionary, 3);
        }

        [Fact]
        public void TrimExcess_Succeeds_WhenCapacityIsGreaterThanCount()
        {
            BidirectionalDictionary<string, int?> dictionary = GetDefaultBidirectionalDictionary(4);
            dictionary.Remove(3.ToString());
            dictionary.TrimExcess(4);
            ValidateDefaultBidirectionalDictionaryIsContiguous(dictionary, 3);
            dictionary = GetDefaultBidirectionalDictionary(4);
            dictionary.Remove(3.ToString());
            dictionary.Reverse.TrimExcess(4);
            ValidateDefaultBidirectionalDictionaryIsContiguous(dictionary, 3);
        }

        [Fact]
        public void TrimExcess_Succeeds_WhenCapacityIsCount()
        {
            BidirectionalDictionary<string, int?> dictionary = GetDefaultBidirectionalDictionary(4);
            dictionary.Remove(3.ToString());
            dictionary.TrimExcess(3);
            ValidateDefaultBidirectionalDictionaryIsContiguous(dictionary, 3);
            dictionary = GetDefaultBidirectionalDictionary(4);
            dictionary.Remove(3.ToString());
            dictionary.Reverse.TrimExcess(3);
            ValidateDefaultBidirectionalDictionaryIsContiguous(dictionary, 3);
        }

        [Fact]
        public void TrimExcess_ThrowsArgumentOutOfRangeException_WhenCapacityIsLessThanCount()
        {
            BidirectionalDictionary<string, int?> dictionary = GetDefaultBidirectionalDictionary(4);
            dictionary.Remove(3.ToString());
            Assert.Throws<ArgumentOutOfRangeException>(() => dictionary.TrimExcess(2));
            ValidateDefaultBidirectionalDictionaryIsContiguous(dictionary, 3);
            dictionary = GetDefaultBidirectionalDictionary(4);
            dictionary.Remove(3.ToString());
            Assert.Throws<ArgumentOutOfRangeException>(() => dictionary.Reverse.TrimExcess(2));
            ValidateDefaultBidirectionalDictionaryIsContiguous(dictionary, 3);
        }

        [Fact]
        public void TrimExcess_ThrowsArgumentOutOfRangeException_WhenCapacityIsLessThanZero()
        {
            BidirectionalDictionary<string, int?> dictionary = GetDefaultBidirectionalDictionary(4);
            dictionary.Remove(3.ToString());
            Assert.Throws<ArgumentOutOfRangeException>(() => dictionary.TrimExcess(-1));
            ValidateDefaultBidirectionalDictionaryIsContiguous(dictionary, 3);
            dictionary = GetDefaultBidirectionalDictionary(4);
            dictionary.Remove(3.ToString());
            Assert.Throws<ArgumentOutOfRangeException>(() => dictionary.Reverse.TrimExcess(-1));
            ValidateDefaultBidirectionalDictionaryIsContiguous(dictionary, 3);
        }

        [Fact]
        public void Clear()
        {
            BidirectionalDictionary<string, int?> dictionary = GetDefaultBidirectionalDictionary();

            dictionary.Clear();
            Assert.Equal(0, dictionary.Count);
            foreach (KeyValuePair<string, int?> pair in dictionary)
            {
                Assert.True(false);
            }
            Assert.Equal(0, dictionary.FirstKeys.Count);
            foreach (string key in dictionary.FirstKeys)
            {
                Assert.True(false);
            }
            Assert.Equal(0, dictionary.SecondKeys.Count);
            foreach (int? value in dictionary.SecondKeys)
            {
                Assert.True(false);
            }

            dictionary = GetDefaultBidirectionalDictionary();

            dictionary.Reverse.Clear();
            Assert.Equal(0, dictionary.Count);
            foreach (KeyValuePair<string, int?> pair in dictionary)
            {
                Assert.True(false);
            }
            Assert.Equal(0, dictionary.FirstKeys.Count);
            foreach (string key in dictionary.FirstKeys)
            {
                Assert.True(false);
            }
            Assert.Equal(0, dictionary.SecondKeys.Count);
            foreach (int? value in dictionary.SecondKeys)
            {
                Assert.True(false);
            }
        }

        [Fact]
        public void Constructor_Succeeds_WhenUsingNoParameters()
        {
            BidirectionalDictionary<string, int?> dictionary = new BidirectionalDictionary<string, int?>();
        }

        [Fact]
        public void Constructor_Succeeds_WhenCapacityIsZero()
        {
            BidirectionalDictionary<string, int?> dictionary = new BidirectionalDictionary<string, int?>(0);
            dictionary = new BidirectionalDictionary<string, int?>(0, StringComparer.OrdinalIgnoreCase, CustomIntComparer.Instance);
        }

        [Fact]
        public void Constructor_Succeeds_WhenCapacityIsGreaterThanZero()
        {
            BidirectionalDictionary<string, int?> dictionary = new BidirectionalDictionary<string, int?>(10);
            dictionary = new BidirectionalDictionary<string, int?>(10, StringComparer.OrdinalIgnoreCase, CustomIntComparer.Instance);
        }

        [Fact]
        public void Constructor_ThrowsArgumentOutOfRangeException_WhenCapacityIsLessThanZero()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new BidirectionalDictionary<string, int?>(-1));
            Assert.Throws<ArgumentOutOfRangeException>(() => new BidirectionalDictionary<string, int?>(-1, StringComparer.OrdinalIgnoreCase, CustomIntComparer.Instance));
        }

        [Fact]
        public void Constructor_Succeeds_WhenComparerIsNull()
        {
            BidirectionalDictionary<string, int?> dictionary = new BidirectionalDictionary<string, int?>(null, null);
            dictionary = new BidirectionalDictionary<string, int?>(10, null, null);
            dictionary = new BidirectionalDictionary<string, int?>(new[] { KeyValuePair.Create("1", (int?)1) }, null, null);
        }

        [Fact]
        public void Constructor_Succeeds_WhenComparerIsNotNull()
        {
            BidirectionalDictionary<string, int?> dictionary = new BidirectionalDictionary<string, int?>(StringComparer.OrdinalIgnoreCase, CustomIntComparer.Instance);
            dictionary = new BidirectionalDictionary<string, int?>(10, StringComparer.OrdinalIgnoreCase, CustomIntComparer.Instance);
            dictionary = new BidirectionalDictionary<string, int?>(new[] { KeyValuePair.Create("1", (int?)1) }, StringComparer.OrdinalIgnoreCase, CustomIntComparer.Instance);
        }

        [Fact]
        public void Constructor_Succeeds_WhenCollectionIsProvided()
        {
            List<KeyValuePair<string, int?>> collection = Enumerable.Range(0, s_defaultCount).Select(i => KeyValuePair.Create(i.ToString(), (int?)i)).ToList();
            BidirectionalDictionary<string, int?> dictionary = new BidirectionalDictionary<string, int?>(collection);
            ValidateDefaultBidirectionalDictionaryIsContiguous(dictionary);
            dictionary = new BidirectionalDictionary<string, int?>(collection, StringComparer.OrdinalIgnoreCase, CustomIntComparer.Instance);
            ValidateDefaultBidirectionalDictionaryIsContiguous(dictionary);
        }

        [Fact]
        public void Constructor_ThrowsArgumentNullException_WhenCollectionIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new BidirectionalDictionary<string, int?>(null));
            Assert.Throws<ArgumentNullException>(() => new BidirectionalDictionary<string, int?>(null, StringComparer.OrdinalIgnoreCase, CustomIntComparer.Instance));
        }

        [Fact]
        public void TestWithStringComparerIgnoreCase()
        {
            BidirectionalDictionary<string, char> dictionary = new BidirectionalDictionary<string, char>(StringComparer.OrdinalIgnoreCase, null);
            for (char c = 'A'; c <= 'Z'; ++c)
            {
                dictionary.Add(c.ToString(), c);
            }
            Assert.Equal(dictionary.Select(p => p.Key), dictionary.FirstKeys);
            Assert.Equal(dictionary.Select(p => p.Value), dictionary.SecondKeys);

            for (char c = 'A'; c <= 'Z'; ++c)
            {
                string key = c.ToString();
                int i = c - 'A';
                Assert.True(dictionary.ContainsKey(key));
                Assert.Equal(c, dictionary[key]);
                Assert.True(dictionary.TryGetValue(key, out char value));
                Assert.Equal(c, value);
            }

            for (char c = 'a'; c <= 'z'; ++c)
            {
                string key = c.ToString();
                int i = c - 'a';
                char upperC = char.ToUpper(c);
                Assert.True(dictionary.ContainsKey(key));
                Assert.Equal(upperC, dictionary[key]);
                Assert.True(dictionary.TryGetValue(key, out char value));
                Assert.Equal(upperC, value);
            }

            Assert.False(dictionary.ContainsKey("1"));
        }

        [Fact]
        public void TestLargeBidirectionalDictionaryOperations()
        {
            const int count = 1_000_000;

            // Without initializing capacity will necessitate resizing of the internal array many times
            BidirectionalDictionary<string, int?> dictionary = new BidirectionalDictionary<string, int?>();

            for (int i = 0; i < count; ++i)
            {
                dictionary.Add(i.ToString(), i);
            }
            Assert.Equal(dictionary.Select(p => p.Key), dictionary.FirstKeys);
            Assert.Equal(dictionary.Select(p => p.Value), dictionary.SecondKeys);

            for (int i = 0; i < count; ++i)
            {
                string key = i.ToString();
                Assert.True(dictionary.ContainsKey(key));
                Assert.Equal(i, dictionary[key]);
                Assert.True(dictionary.TryGetValue(key, out int? value));
                Assert.Equal(i, value);
                Assert.True(dictionary.Reverse.ContainsKey(i));
                Assert.Equal(key, dictionary.Reverse[i]);
                Assert.True(dictionary.Reverse.TryGetValue(i, out string firstKey));
                Assert.Equal(key, firstKey);
            }

            Assert.False(dictionary.ContainsKey(count.ToString()));
            Assert.False(dictionary.Reverse.ContainsKey(count));
        }

        [Fact]
        public void RemoveSlotReused()
        {
            BidirectionalDictionary<Collider, int> d = new BidirectionalDictionary<Collider, int>();
            d[C(0)] = 0;
            d[C(1)] = 1;
            d[C(2)] = 2;
            Assert.True(d.Remove(C(0)));

            d[C(0)] = 3;
            Assert.Equal(d[C(0)], 3);
            Assert.Equal(3, d.Count);
        }

        [Fact]
        public void BidirectionalDictionaryVersusDictionary_AllCollisions()
        {
            Random rand = new Random(333);
            BidirectionalDictionary<Collider, int> bd = new BidirectionalDictionary<Collider, int>();
            Dictionary<Collider, int> d = new Dictionary<Collider, int>();
            int size = rand.Next(1234);
            for (int i = 0; i < size; i++)
            {
                if (rand.Next(5) != 0)
                {
                    Collider k = C(rand.Next(100) + 23);
                    int v = rand.Next();
                    if (bd.TryGetValue(k, out int t))
                    {
                        bd[k] = t + v;
                    }
                    else
                    {
                        bd[k] = v;
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
                    Assert.True(bd.Remove(el));
                    Assert.True(d.Remove(el));
                }
            }
            Assert.Equal(d.Count, bd.Count);
            Assert.Equal(d.OrderBy(i => i.Key), (bd.OrderBy(i => i.Key)));
            Assert.Equal(d.OrderBy(i => i.Value), (bd.OrderBy(i => i.Value)));
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

        private sealed class CustomIntComparer : IEqualityComparer<int?>
        {
            public static CustomIntComparer Instance { get; } = new CustomIntComparer();

            public bool Equals(int? x, int? y) => x == y;

            public int GetHashCode(int? obj) => obj.GetValueOrDefault();
        }
    }
}
