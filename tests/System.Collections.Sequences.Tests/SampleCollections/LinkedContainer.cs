// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace System.Collections.Sequences
{
    // This type is illustrating how to implement the new enumerable on linked node datastructure
    public sealed class LinkedContainer<T> : ISequence<T>
    {
        Node _head;
        int _count;

        class Node
        {
            public T _item;
            public Node _next;
        }

        public void Add(T item)
        {
            var newNode = new Node() { _item = item };
            newNode._next = _head;
            _head = newNode;
            _count++;
        }

        public int Length => _count;

        public SequenceIndex Start => new SequenceIndex(_head, 0);

        public bool TryGet(ref SequenceIndex sequenceIndex, out T item, bool advance = true)
        {
            if(_count == 0 || sequenceIndex == default)
            {
                item = default;
                return false;
            }

            var (node, index) = sequenceIndex.Get<Node>();
            if (node == null || index != 0) {
                item = default;
                sequenceIndex = default;
                return false;
            }

            item = node._item;
            if (advance) { sequenceIndex = new SequenceIndex(node._next, 0); }
            return true;
        }

        public SequenceEnumerator<T> GetEnumerator()
        {
            return new SequenceEnumerator<T>(this);
        }

        public SequenceIndex GetPosition(SequenceIndex origin, long offset)
        {
            if (offset < 0) throw new InvalidOperationException("cannot seek backwards");
            var (node, index) = origin.Get<Node>();
            while(offset-- > 0)
            {
                if (node != null)
                {
                    node = node._next;
                }
                else
                {
                    throw new ArgumentOutOfRangeException(nameof(offset));
                }
            }
            return new SequenceIndex(node, 0);
        }
    }
}
