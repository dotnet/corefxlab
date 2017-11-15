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

        public bool TryGet(ref Position position, out T item, bool advance = true)
        {
            if(_count == 0)
            {
                item = default;
                return false;
            }

            if (position == default)
            {
                item = _head._item;
                if (advance) position.SetItem(_head._next);
                return _count > 0;
            }

            var node = position.GetItem<Node>();
            if (node == null) {
                item = default;
                position = Position.End;
                return false;
            }

            item = node._item;
            if (advance) { position.SetItem(node._next); }
            return true;
        }

        public SequenceEnumerator<T> GetEnumerator()
        {
            return new SequenceEnumerator<T>(this);
        }
    }
}
