// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Xunit;

namespace System.Slices.Tests
{
    public class IndexOfTests
    {
        [Fact]
        public void FindInUniqueValues()
        {
            var range = Enumerable.Range(0, 10).ToArray();

            UniqueValuesSuccess(range);
            UniqueValuesSuccess(range.Select(i => Guid.NewGuid()).ToArray());
        }

        [Fact]
        public void FindInNonUniqueValues()
        {
            var range = Enumerable.Range(0, 10).ToArray();

            NonUniqueValuesSuccess(range.Concat(range).ToArray());

            var guids = range.Select(i => Guid.NewGuid()).ToArray();
            NonUniqueValuesSuccess(guids.Concat(guids).ToArray());
        }

        [Fact]
        public void DontFindNonExisting()
        {
            var range = Enumerable.Range(0, 10).ToArray();

            NonExistingValuesFailure(
                range.Take(5).ToArray(),
                range.Skip(5).ToArray());

            var guids = range.Select(i => Guid.NewGuid()).ToArray();
            NonExistingValuesFailure(
                guids.Take(5).ToArray(),
                guids.Skip(5).ToArray());
        }

        private void UniqueValuesSuccess<T>(T[] values)
            where T : struct, IEquatable<T>
        {
            Debug.Assert(values.Distinct().Count() == values.Length);

            ReadOnlySpan<T> slice = values.Slice();

            for (int i = 0; i < values.Length; i++)
            {
                T value = values[i];
                int indexInArray = Array.IndexOf(values, value);

                int index = slice.IndexOf(value);

                Assert.True(index >= 0);
                Assert.Equal(indexInArray, index);
                
            }
        }

        private void NonUniqueValuesSuccess<T>(T[] values)
            where T : struct, IEquatable<T>
        {
            Debug.Assert(values.Distinct().Count() < values.Length);

            ReadOnlySpan<T> slice = values.Slice();

            for (int i = 0; i < values.Length; i++)
            {
                T value = values[i];
                int indexInArray = Array.IndexOf(values, value);

                int index = slice.IndexOf(value);

                Assert.True(index >= 0);
                Assert.Equal(indexInArray, index);
            }
        }

        private void NonExistingValuesFailure<T>(T[] values, T[] nonExistingValues)
            where T : struct, IEquatable<T>
        {
            Debug.Assert(!values.Intersect(nonExistingValues).Any());

            ReadOnlySpan<T> slice = values.Slice();

            for (int i = 0; i < nonExistingValues.Length; i++)
            {
                T value = nonExistingValues[i];
                int indexInArray = Array.IndexOf(values, value);

                int index = slice.IndexOf(value);

                Assert.True(index == -1);
                Assert.Equal(indexInArray, index);
            }
        }
    }
}