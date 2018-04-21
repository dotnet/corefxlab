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

        public SequencePosition Start => new SequencePosition(_head, 0);

        public bool TryGet(ref SequencePosition position, out T item, bool advance = true)
        {
            if(_count == 0 || position.Equals(default))
            {
                item = default;
                return false;
            }

            var node = (Node) position.GetObject();
            var index = position.GetInteger();
            if (node == null || index != 0) {
                item = default;
                position = default;
                return false;
            }

            item = node._item;
            if (advance) { position = new SequencePosition(node._next, 0); }
            return true;
        }

        public SequenceEnumerator<T> GetEnumerator()
        {
            return new SequenceEnumerator<T>(this);
        }

        public SequencePosition GetPosition(SequencePosition origin, long offset)
        {
            if (offset < 0) throw new InvalidOperationException("cannot seek backwards");
            var node = (Node)origin.GetObject();
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
            return new SequencePosition(node, 0);
        }
    }
}
