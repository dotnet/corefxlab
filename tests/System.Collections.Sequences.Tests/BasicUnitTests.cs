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
            var collection = new ArrayList<int>();
            foreach (var item in array) collection.Add(item);

            int arrayIndex = 0;
            // TODO: I don't like that people need to loop over IsValid, instead of !IsEnd. It's not intuitive.
            var position = Position.First;
            while (true) {
                var item = collection.GetAt(ref position, advance: true);
                if (!position.IsValid) break;
                Assert.Equal(array[arrayIndex++], item);
            }

            arrayIndex = 0;
            var sequence = (ISequence<int>)collection;
            foreach(var item in sequence) {
                Assert.Equal(array[arrayIndex++], item);
            }
        }

        [Theory]
        [InlineData(new int[] { })]
        [InlineData(new int[] { 1 })]
        [InlineData(new int[] { 1, 2, 3 })]
        public void LinkedContainer(int[] array)
        {
            var collection = new LinkedContainer<int>();
            foreach (var item in array) collection.Add(item); // this adds to front

            int arrayIndex = array.Length - 1;
            var position = Position.First;
            while (true) {
                var item = collection.GetAt(ref position, advance:true);
                if (!position.IsValid) break;
                Assert.Equal(array[arrayIndex--], item);
            }

            arrayIndex = array.Length - 1;
            var sequence = (ISequence<int>)collection;
            foreach (var item in sequence) {
                Assert.Equal(array[arrayIndex--], item);
            }
        }

        [Theory]
        [InlineData(new int[] { })]
        [InlineData(new int[] { 1 })]
        [InlineData(new int[] { 1, 2, 3 })]
        public void Hashtable(int[] array)
        {
            var collection = new Hashtable<int, string>(EqualityComparer<int>.Default);
            foreach (var item in array) collection.Add(item, item.ToString());

            int arrayIndex = 0;
            var position = Position.First;
            while (true) {
                var item = collection.GetAt(ref position, advance: true);
                if (!position.IsValid) break;
                Assert.Equal(array[arrayIndex++], item.Key);
            }

            arrayIndex = 0;
            var sequence = (ISequence<KeyValuePair<int, string>>)collection;
            foreach (var item in sequence) {
                Assert.Equal(array[arrayIndex++], item.Key);
            }
        }
    }
}
