using System.Collections.Generic;

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

        int? ISequence<T>.Length => Length;

        public bool TryGet(ref Position position, out T item, bool advance = true)
        {
            item = default(T);

            if(_count == 0) {
                position = Position.AfterLast;
                return false;
            }

            if (position.Equals(Position.AfterLast)) {
                return false;
            }

            if(position.Equals(Position.First)) {
                item = _head._item;
                if (advance) position.ObjectPosition = _head._next;
                if (position.ObjectPosition == null) position = Position.AfterLast;
                return true;
            }

            var node = (Node)position.ObjectPosition;
            
            if (node == null) {
                position = Position.AfterLast;
                return false;
            }

            if (advance) {
                if (node._next != null) {
                    position.ObjectPosition = node._next;
                    if (position.ObjectPosition == null) position = Position.AfterLast;
                }
                else {
                    position = Position.AfterLast;
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
