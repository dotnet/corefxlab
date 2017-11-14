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
            item = default;

            if(_count == 0) {
                position = Position.Infinity;
                return false;
            }

            if (position.IsInfinity) {
                return false;
            }

            if(position == default) {
                item = _head._item;
                if (advance) position.Set(_head._next);
                if (_head._next == null) position = Position.Infinity;
                return true;
            }

            var node = position.As<Node>();
            
            if (node == null) {
                position = Position.Infinity;
                return false;
            }

            if (advance) {
                if (node._next != null) {
                    position.Set(node._next);
                    if (node._next == null) position = Position.Infinity;
                }
                else {
                    position = Position.Infinity;
                }
            }

            item = node._item;
            return true;
        }

        public SequenceEnumerator<T> GetEnumerator()
        {
            return new SequenceEnumerator<T>(this);
        }
    }
}
