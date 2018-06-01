// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace System.Collections.Sequences
{
    // This type is illustrating how to implement the new enumerable on index based datastructure
    public sealed class ArrayList<T> : ISequence<T>
    {
        ResizableArray<T> _items;

        public ArrayList() => _items = new ResizableArray<T>(0);

        public ArrayList(int capacity) => _items = new ResizableArray<T>(capacity);

        public int Length => _items.Count;

        public SequencePosition Start => default;

        public T this[int index] => _items[index];

        public void Add(T item) => _items.Add(item);

        public SequenceEnumerator<T> GetEnumerator() => new SequenceEnumerator<T>(this);

        public bool TryGet(ref SequencePosition position, out T item, bool advance = true) =>
            _items.TryGet(ref position, out item, advance);

        public SequencePosition GetPosition(SequencePosition origin, long offset)
        {
            long index = origin.GetInteger() + offset;
            if (index < 0 || index >= Length) throw new ArgumentOutOfRangeException(nameof(offset));
            return new SequencePosition(null, (int)index);
        }
    }
}
