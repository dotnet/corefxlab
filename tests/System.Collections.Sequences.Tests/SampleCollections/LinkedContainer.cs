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

        public int Length {
            get {
                return _count;
            }
        }

        public T TryGetItem(ref Position position)
        {
            if (_head != null && position.IsValid && !position.IsEnd) {
                var node = (Node)position.ObjectPosition;
                if (node == null) { node = _head; }
                var result = node._item;
                if (node._next != null) {
                    position.ObjectPosition = node._next;
                } else {
                    position = Position.End;
                }
                return result;
            }

            position = Position.Invalid;
            return default(T);
        }

        public SequenceEnumerator<T> GetEnumerator()
        {
            return new SequenceEnumerator<T>(this);
        }
    }
}
