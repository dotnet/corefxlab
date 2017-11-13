// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using Xunit;

namespace System.Collections.Sequences.Tests
{
    public class SequenceTests
    {
        [Theory]
        [InlineData(new int[] { })]
        [InlineData(new int[] { 1 })]
        [InlineData(new int[] { 1, 2, 3})]
        public void ArrayList(int[] array)
        {
            ArrayList<int> collection = CreateArrayList(array);

            var position = Position.First;
            int arrayIndex = 0;
            while (collection.TryGet(ref position, out int item))
            {
                Assert.Equal(array[arrayIndex++], item);
            }
        }

        private static ArrayList<int> CreateArrayList(int[] array)
        {
            var collection = new ArrayList<int>();
            foreach (var arrayItem in array) collection.Add(arrayItem);
            return collection;
        }

        [Theory]
        [InlineData(new int[] { })]
        [InlineData(new int[] { 1 })]
        [InlineData(new int[] { 1, 2, 3 })]
        public void LinkedContainer(int[] array)
        {
            LinkedContainer<int> collection = CreateLinkedContainer(array);

            var position = Position.First;
            int arrayIndex = array.Length;
            while (collection.TryGet(ref position, out int item))
            {
                Assert.Equal(array[--arrayIndex], item);
            }
        }

        private static LinkedContainer<int> CreateLinkedContainer(int[] array)
        {
            var collection = new LinkedContainer<int>();
            foreach (var item in array) collection.Add(item); // this adds to front
            return collection;
        }

        [Theory]
        [InlineData(new int[] { })]
        [InlineData(new int[] { 1 })]
        [InlineData(new int[] { 1, 2, 3 })]
        public void Hashtable(int[] array)
        {
            Hashtable<int, string> collection = CreateHashtable(array);

            int arrayIndex = 0;
            var position = Position.First;
            while (collection.TryGet(ref position, out KeyValuePair<int, string> item))
            {
                Assert.Equal(array[arrayIndex++], item.Key);
            }
        }

        private static Hashtable<int, string> CreateHashtable(int[] array)
        {
            var collection = new Hashtable<int, string>(EqualityComparer<int>.Default);
            foreach (var item in array) collection.Add(item, item.ToString());
            return collection;
        }
    }
}
